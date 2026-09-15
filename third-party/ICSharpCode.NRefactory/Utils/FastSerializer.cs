using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace ICSharpCode.NRefactory.Utils
{
	public class FastSerializer
	{
		/// <summary>
		/// Gets/Sets the serialization binder that is being used.
		/// The default value is null, which will cause the FastSerializer to use the
		/// full assembly and type names.
		/// </summary>
		public SerializationBinder SerializationBinder { get; set; }

		/// <summary>
		/// Can be used to set several 'fixed' instances.
		/// When serializing, such instances will not be included; and any references to a fixed instance
		/// will be stored as the index in this array.
		/// When deserializing, the same (or equivalent) instances must be specified, and the deserializer
		/// will use them in place of the fixed instances.
		/// </summary>
		public object[] FixedInstances { get; set; }

		private FastSerializer.ObjectScanner GetScanner(Type type)
		{
			FastSerializer.ObjectScanner objectScanner;
			if (!this.scanners.TryGetValue(type, out objectScanner))
			{
				objectScanner = this.CreateScanner(type);
				this.scanners.Add(type, objectScanner);
			}
			return objectScanner;
		}

		private FastSerializer.ObjectScanner CreateScanner(Type type)
		{
			bool isArray = type.IsArray;
			if (isArray)
			{
				if (type.GetArrayRank() != 1)
				{
					throw new NotSupportedException();
				}
				type = type.GetElementType();
				if (!type.IsValueType)
				{
					return delegate(FastSerializer.SerializationContext context, object array)
					{
						foreach (object instance in (object[])array)
						{
							context.Mark(instance);
						}
					};
				}
			}
			Type type2 = type;
			while (type2 != null)
			{
				if (!type2.IsSerializable)
				{
					throw new SerializationException("Type " + type2 + " is not [Serializable].");
				}
				type2 = type2.BaseType;
			}
			List<FieldInfo> serializableFields = FastSerializer.GetSerializableFields(type);
			serializableFields.RemoveAll((FieldInfo f) => !FastSerializer.IsReferenceOrContainsReferences(f.FieldType));
			if (serializableFields.Count == 0)
			{
				return delegate(FastSerializer.SerializationContext param0, object param1)
				{
				};
			}
			DynamicMethod dynamicMethod = new DynamicMethod((isArray ? "ScanArray_" : "Scan_") + type.Name, typeof(void), new Type[]
			{
				typeof(FastSerializer.SerializationContext),
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			if (isArray)
			{
				LocalBuilder local = ilgenerator.DeclareLocal(type.MakeArrayType());
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type.MakeArrayType());
				ilgenerator.Emit(OpCodes.Stloc, local);
				Label label = ilgenerator.DefineLabel();
				Label label2 = ilgenerator.DefineLabel();
				LocalBuilder local2 = ilgenerator.DeclareLocal(typeof(int));
				ilgenerator.Emit(OpCodes.Ldc_I4_0);
				ilgenerator.Emit(OpCodes.Stloc, local2);
				ilgenerator.Emit(OpCodes.Br, label2);
				ilgenerator.MarkLabel(label);
				ilgenerator.Emit(OpCodes.Ldloc, local);
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldelem, type);
				this.EmitScanValueType(ilgenerator, type);
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldc_I4_1);
				ilgenerator.Emit(OpCodes.Add);
				ilgenerator.Emit(OpCodes.Stloc, local2);
				ilgenerator.MarkLabel(label2);
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldloc, local);
				ilgenerator.Emit(OpCodes.Ldlen);
				ilgenerator.Emit(OpCodes.Conv_I4);
				ilgenerator.Emit(OpCodes.Blt, label);
			}
			else if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Unbox_Any, type);
				this.EmitScanValueType(ilgenerator, type);
			}
			else
			{
				LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
				foreach (FieldInfo field in serializableFields)
				{
					this.EmitScanField(ilgenerator, localBuilder, field);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (FastSerializer.ObjectScanner)dynamicMethod.CreateDelegate(typeof(FastSerializer.ObjectScanner));
		}

		/// <summary>
		/// Emit 'scan instance.Field'.
		/// Stack transition: ... =&gt; ...
		/// </summary>
		private void EmitScanField(ILGenerator il, LocalBuilder instance, FieldInfo field)
		{
			if (field.FieldType.IsValueType)
			{
				il.Emit(OpCodes.Ldloc, instance);
				il.Emit(OpCodes.Ldfld, field);
				this.EmitScanValueType(il, field.FieldType);
				return;
			}
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldloc, instance);
			il.Emit(OpCodes.Ldfld, field);
			il.Emit(OpCodes.Call, FastSerializer.mark);
		}

		/// <summary>
		/// Stack transition: ..., value =&gt; ...
		/// </summary>
		private void EmitScanValueType(ILGenerator il, Type valType)
		{
			LocalBuilder localBuilder = il.DeclareLocal(valType);
			il.Emit(OpCodes.Stloc, localBuilder);
			foreach (FieldInfo fieldInfo in FastSerializer.GetSerializableFields(valType))
			{
				if (FastSerializer.IsReferenceOrContainsReferences(fieldInfo.FieldType))
				{
					this.EmitScanField(il, localBuilder, fieldInfo);
				}
			}
		}

		private static List<FieldInfo> GetSerializableFields(Type type)
		{
			List<FieldInfo> list = new List<FieldInfo>();
			Type type2 = type;
			while (type2 != null)
			{
				FieldInfo[] fields = type2.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				Array.Sort<FieldInfo>(fields, (FieldInfo a, FieldInfo b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
				list.AddRange(fields);
				type2 = type2.BaseType;
			}
			list.RemoveAll((FieldInfo f) => f.IsNotSerialized);
			return list;
		}

		private static bool IsReferenceOrContainsReferences(Type type)
		{
			if (!type.IsValueType)
			{
				return true;
			}
			if (type.IsPrimitive)
			{
				return false;
			}
			foreach (FieldInfo fieldInfo in FastSerializer.GetSerializableFields(type))
			{
				if (FastSerializer.IsReferenceOrContainsReferences(fieldInfo.FieldType))
				{
					return true;
				}
			}
			return false;
		}

		private FastSerializer.ObjectWriter GetWriter(Type type)
		{
			FastSerializer.ObjectWriter objectWriter;
			if (!this.writers.TryGetValue(type, out objectWriter))
			{
				objectWriter = this.CreateWriter(type);
				this.writers.Add(type, objectWriter);
			}
			return objectWriter;
		}

		private FastSerializer.ObjectWriter CreateWriter(Type type)
		{
			if (type == typeof(string))
			{
				return delegate(FastSerializer.SerializationContext param0, object param1)
				{
				};
			}
			bool isArray = type.IsArray;
			if (isArray)
			{
				if (type.GetArrayRank() != 1)
				{
					throw new NotSupportedException();
				}
				type = type.GetElementType();
				if (!type.IsValueType)
				{
					return delegate(FastSerializer.SerializationContext context, object array)
					{
						foreach (object instance in (object[])array)
						{
							context.WriteObjectID(instance);
						}
					};
				}
				if (type == typeof(byte))
				{
					return delegate(FastSerializer.SerializationContext context, object array)
					{
						context.writer.Write((byte[])array);
					};
				}
			}
			List<FieldInfo> serializableFields = FastSerializer.GetSerializableFields(type);
			if (serializableFields.Count == 0)
			{
				return delegate(FastSerializer.SerializationContext param0, object param1)
				{
				};
			}
			DynamicMethod dynamicMethod = new DynamicMethod((isArray ? "WriteArray_" : "Write_") + type.Name, typeof(void), new Type[]
			{
				typeof(FastSerializer.SerializationContext),
				typeof(object)
			}, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(typeof(BinaryWriter));
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, FastSerializer.writerField);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			if (isArray)
			{
				LocalBuilder local = ilgenerator.DeclareLocal(type.MakeArrayType());
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type.MakeArrayType());
				ilgenerator.Emit(OpCodes.Stloc, local);
				Label label = ilgenerator.DefineLabel();
				Label label2 = ilgenerator.DefineLabel();
				LocalBuilder local2 = ilgenerator.DeclareLocal(typeof(int));
				ilgenerator.Emit(OpCodes.Ldc_I4_0);
				ilgenerator.Emit(OpCodes.Stloc, local2);
				ilgenerator.Emit(OpCodes.Br, label2);
				ilgenerator.MarkLabel(label);
				if (type.IsEnum || type.IsPrimitive)
				{
					if (type.IsEnum)
					{
						type = type.GetEnumUnderlyingType();
					}
					ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
					ilgenerator.Emit(OpCodes.Ldloc, local);
					ilgenerator.Emit(OpCodes.Ldloc, local2);
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Byte:
						ilgenerator.Emit(OpCodes.Ldelem_I1);
						ilgenerator.Emit(this.callVirt, FastSerializer.writeByte);
						break;
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.UInt16:
						ilgenerator.Emit(OpCodes.Ldelem_I2);
						ilgenerator.Emit(this.callVirt, FastSerializer.writeShort);
						break;
					case TypeCode.Int32:
					case TypeCode.UInt32:
						ilgenerator.Emit(OpCodes.Ldelem_I4);
						ilgenerator.Emit(this.callVirt, FastSerializer.writeInt);
						break;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						ilgenerator.Emit(OpCodes.Ldelem_I8);
						ilgenerator.Emit(this.callVirt, FastSerializer.writeLong);
						break;
					case TypeCode.Single:
						ilgenerator.Emit(OpCodes.Ldelem_R4);
						ilgenerator.Emit(this.callVirt, FastSerializer.writeFloat);
						break;
					case TypeCode.Double:
						ilgenerator.Emit(OpCodes.Ldelem_R8);
						ilgenerator.Emit(this.callVirt, FastSerializer.writeDouble);
						break;
					default:
						throw new NotSupportedException("Unknown primitive type " + type);
					}
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldloc, local);
					ilgenerator.Emit(OpCodes.Ldloc, local2);
					ilgenerator.Emit(OpCodes.Ldelem, type);
					this.EmitWriteValueType(ilgenerator, localBuilder, type);
				}
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldc_I4_1);
				ilgenerator.Emit(OpCodes.Add);
				ilgenerator.Emit(OpCodes.Stloc, local2);
				ilgenerator.MarkLabel(label2);
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldloc, local);
				ilgenerator.Emit(OpCodes.Ldlen);
				ilgenerator.Emit(OpCodes.Conv_I4);
				ilgenerator.Emit(OpCodes.Blt, label);
			}
			else if (type.IsValueType)
			{
				if (type.IsEnum || type.IsPrimitive)
				{
					ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
					ilgenerator.Emit(OpCodes.Ldarg_1);
					ilgenerator.Emit(OpCodes.Unbox_Any, type);
					this.WritePrimitiveValue(ilgenerator, type);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldarg_1);
					ilgenerator.Emit(OpCodes.Unbox_Any, type);
					this.EmitWriteValueType(ilgenerator, localBuilder, type);
				}
			}
			else
			{
				LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(type);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
				foreach (FieldInfo field in serializableFields)
				{
					this.EmitWriteField(ilgenerator, localBuilder, localBuilder2, field);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (FastSerializer.ObjectWriter)dynamicMethod.CreateDelegate(typeof(FastSerializer.ObjectWriter));
		}

		/// <summary>
		/// Emit 'write instance.Field'.
		/// Stack transition: ... =&gt; ...
		/// </summary>
		private void EmitWriteField(ILGenerator il, LocalBuilder writer, LocalBuilder instance, FieldInfo field)
		{
			Type fieldType = field.FieldType;
			if (!fieldType.IsValueType)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Ldloc, instance);
				il.Emit(OpCodes.Ldfld, field);
				il.Emit(OpCodes.Call, FastSerializer.writeObjectID);
				return;
			}
			if (fieldType.IsPrimitive || fieldType.IsEnum)
			{
				il.Emit(OpCodes.Ldloc, writer);
				il.Emit(OpCodes.Ldloc, instance);
				il.Emit(OpCodes.Ldfld, field);
				this.WritePrimitiveValue(il, fieldType);
				return;
			}
			il.Emit(OpCodes.Ldloc, instance);
			il.Emit(OpCodes.Ldfld, field);
			this.EmitWriteValueType(il, writer, fieldType);
		}

		/// <summary>
		/// Writes a primitive value of the specified type.
		/// Stack transition: ..., writer, value =&gt; ...
		/// </summary>
		private void WritePrimitiveValue(ILGenerator il, Type fieldType)
		{
			if (fieldType.IsEnum)
			{
				fieldType = fieldType.GetEnumUnderlyingType();
			}
			switch (Type.GetTypeCode(fieldType))
			{
			case TypeCode.Boolean:
			case TypeCode.SByte:
			case TypeCode.Byte:
				il.Emit(this.callVirt, FastSerializer.writeByte);
				return;
			case TypeCode.Char:
			case TypeCode.Int16:
			case TypeCode.UInt16:
				il.Emit(this.callVirt, FastSerializer.writeShort);
				return;
			case TypeCode.Int32:
			case TypeCode.UInt32:
				il.Emit(this.callVirt, FastSerializer.writeInt);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				il.Emit(this.callVirt, FastSerializer.writeLong);
				return;
			case TypeCode.Single:
				il.Emit(this.callVirt, FastSerializer.writeFloat);
				return;
			case TypeCode.Double:
				il.Emit(this.callVirt, FastSerializer.writeDouble);
				return;
			default:
				throw new NotSupportedException("Unknown primitive type " + fieldType);
			}
		}

		/// <summary>
		/// Stack transition: ..., value =&gt; ...
		/// </summary>
		private void EmitWriteValueType(ILGenerator il, LocalBuilder writer, Type valType)
		{
			LocalBuilder localBuilder = il.DeclareLocal(valType);
			il.Emit(OpCodes.Stloc, localBuilder);
			foreach (FieldInfo field in FastSerializer.GetSerializableFields(valType))
			{
				this.EmitWriteField(il, writer, localBuilder, field);
			}
		}

		public void Serialize(Stream stream, object instance)
		{
			this.Serialize(new BinaryWriterWith7BitEncodedInts(stream), instance);
		}

		public void Serialize(BinaryWriter writer, object instance)
		{
			FastSerializer.SerializationContext serializationContext = new FastSerializer.SerializationContext(this, writer);
			serializationContext.MarkFixedInstances(this.FixedInstances);
			serializationContext.Mark(instance);
			serializationContext.Scan();
			serializationContext.ScanTypes();
			serializationContext.Write();
			serializationContext.WriteObjectID(instance);
		}

		public object Deserialize(Stream stream)
		{
			return this.Deserialize(new BinaryReaderWith7BitEncodedInts(stream));
		}

		public object Deserialize(BinaryReader reader)
		{
			if (reader.ReadInt32() != 1909623390)
			{
				throw new SerializationException("The data cannot be read by FastSerializer (unknown magic value)");
			}
			FastSerializer.DeserializationContext deserializationContext = new FastSerializer.DeserializationContext();
			deserializationContext.Reader = reader;
			deserializationContext.Objects = new object[reader.ReadInt32()];
			deserializationContext.Types = new Type[reader.ReadInt32()];
			string[] array = new string[reader.ReadInt32()];
			int num = reader.ReadInt32();
			if (num != 0)
			{
				if (this.FixedInstances == null || this.FixedInstances.Length != num)
				{
					throw new SerializationException("Number of fixed instances doesn't match");
				}
				for (int i = 0; i < num; i++)
				{
					deserializationContext.Objects[i + 1] = this.FixedInstances[i];
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = reader.ReadString();
			}
			int num2 = -1;
			for (int k = 0; k < deserializationContext.Types.Length; k++)
			{
				byte b = reader.ReadByte();
				switch (b)
				{
				case 1:
				case 2:
				{
					int num3;
					if (array.Length <= 65535)
					{
						num3 = (int)reader.ReadUInt16();
					}
					else
					{
						num3 = reader.ReadInt32();
					}
					string text = array[num3];
					string text2 = reader.ReadString();
					Type type;
					if (this.SerializationBinder != null)
					{
						type = this.SerializationBinder.BindToType(text, text2);
					}
					else
					{
						type = Assembly.Load(text).GetType(text2);
					}
					if (type == null)
					{
						throw new SerializationException(string.Concat(new string[]
						{
							"Could not find '",
							text2,
							"' in '",
							text,
							"'"
						}));
					}
					if (b == 2 && !type.IsValueType)
					{
						throw new SerializationException("Expected '" + text2 + "' to be a value type, but it is reference type");
					}
					if (b == 1 && type.IsValueType)
					{
						throw new SerializationException("Expected '" + text2 + "' to be a reference type, but it is value type");
					}
					deserializationContext.Types[k] = type;
					if (type == typeof(string))
					{
						num2 = k;
					}
					break;
				}
				case 3:
					deserializationContext.Types[k] = deserializationContext.Types[deserializationContext.ReadTypeID()].MakeArrayType();
					break;
				case 4:
				{
					Type type2 = deserializationContext.Types[deserializationContext.ReadTypeID()];
					int num4 = type2.GetGenericArguments().Length;
					Type[] array2 = new Type[num4];
					for (int l = 0; l < array2.Length; l++)
					{
						array2[l] = deserializationContext.Types[deserializationContext.ReadTypeID()];
					}
					deserializationContext.Types[k] = type2.MakeGenericType(array2);
					break;
				}
				default:
					throw new SerializationException("Unknown type kind");
				}
			}
			deserializationContext.DeserializeTypeDescriptions();
			int[] array3 = new int[deserializationContext.Objects.Length];
			for (int m = 1 + num; m < deserializationContext.Objects.Length; m++)
			{
				int num5 = deserializationContext.ReadTypeID();
				object obj;
				if (num5 == num2)
				{
					obj = reader.ReadString();
				}
				else
				{
					Type type3 = deserializationContext.Types[num5];
					if (type3.IsArray)
					{
						int length = reader.ReadInt32();
						obj = Array.CreateInstance(type3.GetElementType(), length);
					}
					else
					{
						obj = FormatterServices.GetUninitializedObject(type3);
					}
				}
				deserializationContext.Objects[m] = obj;
				array3[m] = num5;
			}
			List<FastSerializer.CustomDeserialization> list = new List<FastSerializer.CustomDeserialization>();
			FastSerializer.ObjectReader[] array4 = new FastSerializer.ObjectReader[deserializationContext.Types.Length];
			for (int n = 1 + num; n < deserializationContext.Objects.Length; n++)
			{
				object obj2 = deserializationContext.Objects[n];
				int num6 = array3[n];
				ISerializable serializable = obj2 as ISerializable;
				if (serializable != null)
				{
					Type type4 = deserializationContext.Types[num6];
					SerializationInfo serializationInfo = new SerializationInfo(type4, this.formatterConverter);
					int num7 = reader.ReadInt32();
					for (int num8 = 0; num8 < num7; num8++)
					{
						string name = reader.ReadString();
						object value = deserializationContext.ReadObject();
						serializationInfo.AddValue(name, value);
					}
					FastSerializer.CustomDeserializationAction customDeserializationAction = this.GetCustomDeserializationAction(type4);
					list.Add(new FastSerializer.CustomDeserialization(obj2, serializationInfo, customDeserializationAction));
				}
				else
				{
					FastSerializer.ObjectReader objectReader = array4[num6];
					if (objectReader == null)
					{
						objectReader = this.GetReader(deserializationContext.Types[num6]);
						array4[num6] = objectReader;
					}
					objectReader(deserializationContext, obj2);
				}
			}
			foreach (FastSerializer.CustomDeserialization customDeserialization in list)
			{
				customDeserialization.Run(this.streamingContext);
			}
			for (int num9 = 1 + num; num9 < deserializationContext.Objects.Length; num9++)
			{
				IDeserializationCallback deserializationCallback = deserializationContext.Objects[num9] as IDeserializationCallback;
				if (deserializationCallback != null)
				{
					deserializationCallback.OnDeserialization(null);
				}
			}
			return deserializationContext.ReadObject();
		}

		private FastSerializer.ObjectReader GetReader(Type type)
		{
			FastSerializer.ObjectReader objectReader;
			if (!this.readers.TryGetValue(type, out objectReader))
			{
				objectReader = this.CreateReader(type);
				this.readers.Add(type, objectReader);
			}
			return objectReader;
		}

		private FastSerializer.ObjectReader CreateReader(Type type)
		{
			if (type == typeof(string))
			{
				return delegate(FastSerializer.DeserializationContext param0, object param1)
				{
				};
			}
			bool isArray = type.IsArray;
			if (isArray)
			{
				if (type.GetArrayRank() != 1)
				{
					throw new NotSupportedException();
				}
				type = type.GetElementType();
				if (!type.IsValueType)
				{
					return delegate(FastSerializer.DeserializationContext context, object arrayInstance)
					{
						object[] array = (object[])arrayInstance;
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = context.ReadObject();
						}
					};
				}
				if (type == typeof(byte))
				{
					return delegate(FastSerializer.DeserializationContext context, object arrayInstance)
					{
						byte[] array = (byte[])arrayInstance;
						BinaryReader reader = context.Reader;
						int num = 0;
						int num2;
						do
						{
							num2 = reader.Read(array, num, array.Length - num);
							num += num2;
						}
						while (num2 > 0);
						if (num != array.Length)
						{
							throw new EndOfStreamException();
						}
					};
				}
			}
			List<FieldInfo> serializableFields = FastSerializer.GetSerializableFields(type);
			if (serializableFields.Count == 0)
			{
				return delegate(FastSerializer.DeserializationContext param0, object param1)
				{
				};
			}
			DynamicMethod dynamicMethod = new DynamicMethod((isArray ? "ReadArray_" : "Read_") + type.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, typeof(void), new Type[]
			{
				typeof(FastSerializer.DeserializationContext),
				typeof(object)
			}, type, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(typeof(BinaryReader));
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, FastSerializer.readerField);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			if (isArray)
			{
				LocalBuilder local = ilgenerator.DeclareLocal(type.MakeArrayType());
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type.MakeArrayType());
				ilgenerator.Emit(OpCodes.Stloc, local);
				Label label = ilgenerator.DefineLabel();
				Label label2 = ilgenerator.DefineLabel();
				LocalBuilder local2 = ilgenerator.DeclareLocal(typeof(int));
				ilgenerator.Emit(OpCodes.Ldc_I4_0);
				ilgenerator.Emit(OpCodes.Stloc, local2);
				ilgenerator.Emit(OpCodes.Br, label2);
				ilgenerator.MarkLabel(label);
				if (type.IsEnum || type.IsPrimitive)
				{
					if (type.IsEnum)
					{
						type = type.GetEnumUnderlyingType();
					}
					ilgenerator.Emit(OpCodes.Ldloc, local);
					ilgenerator.Emit(OpCodes.Ldloc, local2);
					this.ReadPrimitiveValue(ilgenerator, localBuilder, type);
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Byte:
						ilgenerator.Emit(OpCodes.Stelem_I1);
						break;
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.UInt16:
						ilgenerator.Emit(OpCodes.Stelem_I2);
						break;
					case TypeCode.Int32:
					case TypeCode.UInt32:
						ilgenerator.Emit(OpCodes.Stelem_I4);
						break;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						ilgenerator.Emit(OpCodes.Stelem_I8);
						break;
					case TypeCode.Single:
						ilgenerator.Emit(OpCodes.Stelem_R4);
						break;
					case TypeCode.Double:
						ilgenerator.Emit(OpCodes.Stelem_R8);
						break;
					default:
						throw new NotSupportedException("Unknown primitive type " + type);
					}
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldloc, local);
					ilgenerator.Emit(OpCodes.Ldloc, local2);
					ilgenerator.Emit(OpCodes.Ldelema, type);
					this.EmitReadValueType(ilgenerator, localBuilder, type);
				}
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldc_I4_1);
				ilgenerator.Emit(OpCodes.Add);
				ilgenerator.Emit(OpCodes.Stloc, local2);
				ilgenerator.MarkLabel(label2);
				ilgenerator.Emit(OpCodes.Ldloc, local2);
				ilgenerator.Emit(OpCodes.Ldloc, local);
				ilgenerator.Emit(OpCodes.Ldlen);
				ilgenerator.Emit(OpCodes.Conv_I4);
				ilgenerator.Emit(OpCodes.Blt, label);
			}
			else if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Unbox, type);
				if (type.IsEnum || type.IsPrimitive)
				{
					if (type.IsEnum)
					{
						type = type.GetEnumUnderlyingType();
					}
					this.ReadPrimitiveValue(ilgenerator, localBuilder, type);
					switch (Type.GetTypeCode(type))
					{
					case TypeCode.Boolean:
					case TypeCode.SByte:
					case TypeCode.Byte:
						ilgenerator.Emit(OpCodes.Stind_I1);
						break;
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.UInt16:
						ilgenerator.Emit(OpCodes.Stind_I2);
						break;
					case TypeCode.Int32:
					case TypeCode.UInt32:
						ilgenerator.Emit(OpCodes.Stind_I4);
						break;
					case TypeCode.Int64:
					case TypeCode.UInt64:
						ilgenerator.Emit(OpCodes.Stind_I8);
						break;
					case TypeCode.Single:
						ilgenerator.Emit(OpCodes.Stind_R4);
						break;
					case TypeCode.Double:
						ilgenerator.Emit(OpCodes.Stind_R8);
						break;
					default:
						throw new NotSupportedException("Unknown primitive type " + type);
					}
				}
				else
				{
					this.EmitReadValueType(ilgenerator, localBuilder, type);
				}
			}
			else
			{
				LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(type);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Castclass, type);
				ilgenerator.Emit(OpCodes.Stloc, localBuilder2);
				foreach (FieldInfo field in serializableFields)
				{
					this.EmitReadField(ilgenerator, localBuilder, localBuilder2, field);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			return (FastSerializer.ObjectReader)dynamicMethod.CreateDelegate(typeof(FastSerializer.ObjectReader));
		}

		private void EmitReadField(ILGenerator il, LocalBuilder reader, LocalBuilder instance, FieldInfo field)
		{
			Type fieldType = field.FieldType;
			if (!fieldType.IsValueType)
			{
				il.Emit(OpCodes.Ldloc, instance);
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Call, FastSerializer.readObject);
				il.Emit(OpCodes.Castclass, fieldType);
				il.Emit(OpCodes.Stfld, field);
				return;
			}
			if (fieldType.IsPrimitive || fieldType.IsEnum)
			{
				il.Emit(OpCodes.Ldloc, instance);
				this.ReadPrimitiveValue(il, reader, fieldType);
				il.Emit(OpCodes.Stfld, field);
				return;
			}
			il.Emit(OpCodes.Ldloc, instance);
			il.Emit(OpCodes.Ldflda, field);
			this.EmitReadValueType(il, reader, fieldType);
		}

		/// <summary>
		/// Reads a primitive value of the specified type.
		/// Stack transition: ... =&gt; ..., value
		/// </summary>
		private void ReadPrimitiveValue(ILGenerator il, LocalBuilder reader, Type fieldType)
		{
			if (fieldType.IsEnum)
			{
				fieldType = fieldType.GetEnumUnderlyingType();
			}
			il.Emit(OpCodes.Ldloc, reader);
			switch (Type.GetTypeCode(fieldType))
			{
			case TypeCode.Boolean:
			case TypeCode.SByte:
			case TypeCode.Byte:
				il.Emit(this.callVirt, FastSerializer.readByte);
				return;
			case TypeCode.Char:
			case TypeCode.Int16:
			case TypeCode.UInt16:
				il.Emit(this.callVirt, FastSerializer.readShort);
				return;
			case TypeCode.Int32:
			case TypeCode.UInt32:
				il.Emit(this.callVirt, FastSerializer.readInt);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				il.Emit(this.callVirt, FastSerializer.readLong);
				return;
			case TypeCode.Single:
				il.Emit(this.callVirt, FastSerializer.readFloat);
				return;
			case TypeCode.Double:
				il.Emit(this.callVirt, FastSerializer.readDouble);
				return;
			default:
				throw new NotSupportedException("Unknown primitive type " + fieldType);
			}
		}

		/// <summary>
		/// Stack transition: ..., field-ref =&gt; ...
		/// </summary>
		private void EmitReadValueType(ILGenerator il, LocalBuilder reader, Type valType)
		{
			LocalBuilder localBuilder = il.DeclareLocal(valType.MakeByRefType());
			il.Emit(OpCodes.Stloc, localBuilder);
			foreach (FieldInfo field in FastSerializer.GetSerializableFields(valType))
			{
				this.EmitReadField(il, reader, localBuilder, field);
			}
		}

		private FastSerializer.CustomDeserializationAction GetCustomDeserializationAction(Type type)
		{
			FastSerializer.CustomDeserializationAction customDeserializationAction;
			if (!this.customDeserializationActions.TryGetValue(type, out customDeserializationAction))
			{
				customDeserializationAction = FastSerializer.CreateCustomDeserializationAction(type);
				this.customDeserializationActions.Add(type, customDeserializationAction);
			}
			return customDeserializationAction;
		}

		private static FastSerializer.CustomDeserializationAction CreateCustomDeserializationAction(Type type)
		{
			ConstructorInfo constructor = type.GetConstructor(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.ExactBinding, null, new Type[]
			{
				typeof(SerializationInfo),
				typeof(StreamingContext)
			}, null);
			if (constructor == null)
			{
				throw new SerializationException("Could not find deserialization constructor for " + type.FullName);
			}
			DynamicMethod dynamicMethod = new DynamicMethod("CallCtor_" + type.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, typeof(void), new Type[]
			{
				typeof(object),
				typeof(SerializationInfo),
				typeof(StreamingContext)
			}, type, true);
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_2);
			ilgenerator.Emit(OpCodes.Call, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			return (FastSerializer.CustomDeserializationAction)dynamicMethod.CreateDelegate(typeof(FastSerializer.CustomDeserializationAction));
		}

		[Conditional("DEBUG_SERIALIZER")]
		private static void Log(string format, params object[] args)
		{
		}

		private const int magic = 1909623390;

		private const byte Type_ReferenceType = 1;

		private const byte Type_ValueType = 2;

		private const byte Type_SZArray = 3;

		private const byte Type_ParameterizedType = 4;

		private static readonly MethodInfo mark = typeof(FastSerializer.SerializationContext).GetMethod("Mark", new Type[]
		{
			typeof(object)
		});

		private static readonly FieldInfo writerField = typeof(FastSerializer.SerializationContext).GetField("writer");

		private Dictionary<Type, FastSerializer.ObjectScanner> scanners = new Dictionary<Type, FastSerializer.ObjectScanner>();

		private static readonly MethodInfo writeObjectID = typeof(FastSerializer.SerializationContext).GetMethod("WriteObjectID", new Type[]
		{
			typeof(object)
		});

		private static readonly MethodInfo writeByte = typeof(BinaryWriter).GetMethod("Write", new Type[]
		{
			typeof(byte)
		});

		private static readonly MethodInfo writeShort = typeof(BinaryWriter).GetMethod("Write", new Type[]
		{
			typeof(short)
		});

		private static readonly MethodInfo writeInt = typeof(BinaryWriter).GetMethod("Write", new Type[]
		{
			typeof(int)
		});

		private static readonly MethodInfo writeLong = typeof(BinaryWriter).GetMethod("Write", new Type[]
		{
			typeof(long)
		});

		private static readonly MethodInfo writeFloat = typeof(BinaryWriter).GetMethod("Write", new Type[]
		{
			typeof(float)
		});

		private static readonly MethodInfo writeDouble = typeof(BinaryWriter).GetMethod("Write", new Type[]
		{
			typeof(double)
		});

		private OpCode callVirt = OpCodes.Callvirt;

		private static readonly FastSerializer.ObjectWriter serializationInfoWriter = delegate(FastSerializer.SerializationContext context, object instance)
		{
			BinaryWriter writer = context.writer;
			SerializationInfo serializationInfo = (SerializationInfo)instance;
			writer.Write(serializationInfo.MemberCount);
			foreach (SerializationEntry serializationEntry in serializationInfo)
			{
				writer.Write(serializationEntry.Name);
				context.WriteObjectID(serializationEntry.Value);
			}
		};

		private Dictionary<Type, FastSerializer.ObjectWriter> writers = new Dictionary<Type, FastSerializer.ObjectWriter>();

		private StreamingContext streamingContext = new StreamingContext(StreamingContextStates.All);

		private FormatterConverter formatterConverter = new FormatterConverter();

		private static readonly FieldInfo readerField = typeof(FastSerializer.DeserializationContext).GetField("Reader");

		private static readonly MethodInfo readObject = typeof(FastSerializer.DeserializationContext).GetMethod("ReadObject");

		private static readonly MethodInfo readByte = typeof(BinaryReader).GetMethod("ReadByte");

		private static readonly MethodInfo readShort = typeof(BinaryReader).GetMethod("ReadInt16");

		private static readonly MethodInfo readInt = typeof(BinaryReader).GetMethod("ReadInt32");

		private static readonly MethodInfo readLong = typeof(BinaryReader).GetMethod("ReadInt64");

		private static readonly MethodInfo readFloat = typeof(BinaryReader).GetMethod("ReadSingle");

		private static readonly MethodInfo readDouble = typeof(BinaryReader).GetMethod("ReadDouble");

		private Dictionary<Type, FastSerializer.ObjectReader> readers = new Dictionary<Type, FastSerializer.ObjectReader>();

		private Dictionary<Type, FastSerializer.CustomDeserializationAction> customDeserializationActions = new Dictionary<Type, FastSerializer.CustomDeserializationAction>();

		private sealed class SerializationType
		{
			public SerializationType(int iD, Type type)
			{
				this.ID = iD;
				this.Type = type;
			}

			public readonly int ID;

			public readonly Type Type;

			public FastSerializer.ObjectScanner Scanner;

			public FastSerializer.ObjectWriter Writer;

			public string TypeName;

			public int AssemblyNameID;
		}

		private sealed class SerializationContext
		{
			internal SerializationContext(FastSerializer fastSerializer, BinaryWriter writer)
			{
				this.fastSerializer = fastSerializer;
				this.writer = writer;
				this.instances.Add(null);
				this.objectTypes.Add(null);
			}

			public void MarkFixedInstances(object[] fixedInstances)
			{
				if (fixedInstances == null)
				{
					return;
				}
				foreach (object obj in fixedInstances)
				{
					if (!this.objectToID.ContainsKey(obj))
					{
						this.objectToID.Add(obj, this.instances.Count);
						this.instances.Add(obj);
						this.fixedInstanceCount++;
					}
				}
			}

			/// <summary>
			/// Marks an instance for future scanning.
			/// </summary>
			public void Mark(object instance)
			{
				if (instance == null || this.objectToID.ContainsKey(instance))
				{
					return;
				}
				this.objectToID.Add(instance, this.instances.Count);
				this.instances.Add(instance);
			}

			internal void Scan()
			{
				for (int i = 1 + this.fixedInstanceCount; i < this.instances.Count; i++)
				{
					object obj = this.instances[i];
					ISerializable serializable = obj as ISerializable;
					Type type = obj.GetType();
					FastSerializer.SerializationType serializationType = this.MarkType(type);
					this.objectTypes.Add(serializationType);
					if (serializable != null)
					{
						SerializationInfo serializationInfo = new SerializationInfo(type, this.fastSerializer.formatterConverter);
						serializable.GetObjectData(serializationInfo, this.fastSerializer.streamingContext);
						this.instances[i] = serializationInfo;
						foreach (SerializationEntry serializationEntry in serializationInfo)
						{
							this.Mark(serializationEntry.Value);
						}
						serializationType.Writer = FastSerializer.serializationInfoWriter;
					}
					else
					{
						FastSerializer.ObjectScanner scanner = serializationType.Scanner;
						if (scanner == null)
						{
							scanner = this.fastSerializer.GetScanner(type);
							serializationType.Scanner = scanner;
							serializationType.Writer = this.fastSerializer.GetWriter(type);
						}
						scanner(this, obj);
					}
				}
			}

			private FastSerializer.SerializationType MarkType(Type type)
			{
				FastSerializer.SerializationType serializationType;
				if (!this.typeMap.TryGetValue(type, out serializationType))
				{
					string text = null;
					string typeName = null;
					if (type.HasElementType)
					{
						this.MarkType(type.GetElementType());
					}
					else if (type.IsGenericType && !type.IsGenericTypeDefinition)
					{
						this.MarkType(type.GetGenericTypeDefinition());
						foreach (Type type2 in type.GetGenericArguments())
						{
							this.MarkType(type2);
						}
					}
					else
					{
						if (type.IsGenericParameter)
						{
							throw new NotSupportedException();
						}
						SerializationBinder serializationBinder = this.fastSerializer.SerializationBinder;
						if (serializationBinder != null)
						{
							serializationBinder.BindToName(type, out text, out typeName);
						}
						else
						{
							text = type.Assembly.FullName;
							typeName = type.FullName;
						}
					}
					serializationType = new FastSerializer.SerializationType(this.typeMap.Count, type);
					serializationType.TypeName = typeName;
					if (text != null && !this.assemblyNameToID.TryGetValue(text, out serializationType.AssemblyNameID))
					{
						serializationType.AssemblyNameID = this.assemblyNames.Count;
						this.assemblyNameToID.Add(text, serializationType.AssemblyNameID);
						this.assemblyNames.Add(text);
					}
					this.typeMap.Add(type, serializationType);
					this.types.Add(serializationType);
					if (type == typeof(string))
					{
						this.stringType = serializationType;
					}
				}
				return serializationType;
			}

			internal void ScanTypes()
			{
				for (int i = 0; i < this.types.Count; i++)
				{
					Type type = this.types[i].Type;
					if (!type.IsGenericTypeDefinition && !type.HasElementType && !typeof(ISerializable).IsAssignableFrom(type))
					{
						foreach (FieldInfo fieldInfo in FastSerializer.GetSerializableFields(type))
						{
							this.MarkType(fieldInfo.FieldType);
						}
					}
				}
			}

			public void WriteObjectID(object instance)
			{
				int num = (instance == null) ? 0 : this.objectToID[instance];
				if (this.instances.Count <= 65535)
				{
					this.writer.Write((ushort)num);
					return;
				}
				this.writer.Write(num);
			}

			private void WriteTypeID(Type type)
			{
				int id = this.typeMap[type].ID;
				if (this.types.Count <= 65535)
				{
					this.writer.Write((ushort)id);
					return;
				}
				this.writer.Write(id);
			}

			internal void Write()
			{
				this.writer.Write(1909623390);
				this.writer.Write(this.instances.Count);
				this.writer.Write(this.types.Count);
				this.writer.Write(this.assemblyNames.Count);
				this.writer.Write(this.fixedInstanceCount);
				foreach (string value in this.assemblyNames)
				{
					this.writer.Write(value);
				}
				foreach (FastSerializer.SerializationType serializationType in this.types)
				{
					Type type = serializationType.Type;
					if (type.HasElementType)
					{
						if (!type.IsArray)
						{
							throw new NotSupportedException();
						}
						if (type.GetArrayRank() != 1)
						{
							throw new NotSupportedException();
						}
						this.writer.Write(3);
						this.WriteTypeID(type.GetElementType());
					}
					else if (type.IsGenericType && !type.IsGenericTypeDefinition)
					{
						this.writer.Write(4);
						this.WriteTypeID(type.GetGenericTypeDefinition());
						foreach (Type type2 in type.GetGenericArguments())
						{
							this.WriteTypeID(type2);
						}
					}
					else
					{
						if (type.IsValueType)
						{
							this.writer.Write(2);
						}
						else
						{
							this.writer.Write(1);
						}
						if (this.assemblyNames.Count <= 65535)
						{
							this.writer.Write((ushort)serializationType.AssemblyNameID);
						}
						else
						{
							this.writer.Write(serializationType.AssemblyNameID);
						}
						this.writer.Write(serializationType.TypeName);
					}
				}
				foreach (FastSerializer.SerializationType serializationType2 in this.types)
				{
					Type type3 = serializationType2.Type;
					if (!type3.IsGenericTypeDefinition && !type3.HasElementType)
					{
						this.writer.Write(FastSerializerVersionAttribute.GetVersionNumber(type3));
						if (type3.IsPrimitive || typeof(ISerializable).IsAssignableFrom(type3))
						{
							this.writer.Write(byte.MaxValue);
						}
						else
						{
							List<FieldInfo> serializableFields = FastSerializer.GetSerializableFields(type3);
							if (serializableFields.Count >= 255)
							{
								throw new SerializationException("Too many fields.");
							}
							this.writer.Write((byte)serializableFields.Count);
							foreach (FieldInfo fieldInfo in serializableFields)
							{
								this.WriteTypeID(fieldInfo.FieldType);
								this.writer.Write(fieldInfo.Name);
							}
						}
					}
				}
				for (int j = 1 + this.fixedInstanceCount; j < this.instances.Count; j++)
				{
					FastSerializer.SerializationType serializationType3 = this.objectTypes[j];
					if (this.types.Count <= 65535)
					{
						this.writer.Write((ushort)serializationType3.ID);
					}
					else
					{
						this.writer.Write(serializationType3.ID);
					}
					if (serializationType3 == this.stringType)
					{
						this.writer.Write((string)this.instances[j]);
					}
					else if (serializationType3.Type.IsArray)
					{
						this.writer.Write(((Array)this.instances[j]).Length);
					}
				}
				for (int k = 1 + this.fixedInstanceCount; k < this.instances.Count; k++)
				{
					this.objectTypes[k].Writer(this, this.instances[k]);
				}
			}

			private readonly Dictionary<object, int> objectToID = new Dictionary<object, int>(ReferenceComparer.Instance);

			private readonly List<object> instances = new List<object>();

			private readonly List<FastSerializer.SerializationType> objectTypes = new List<FastSerializer.SerializationType>();

			private FastSerializer.SerializationType stringType;

			private readonly Dictionary<Type, FastSerializer.SerializationType> typeMap = new Dictionary<Type, FastSerializer.SerializationType>();

			private readonly List<FastSerializer.SerializationType> types = new List<FastSerializer.SerializationType>();

			private readonly Dictionary<string, int> assemblyNameToID = new Dictionary<string, int>();

			private readonly List<string> assemblyNames = new List<string>();

			private readonly FastSerializer fastSerializer;

			public readonly BinaryWriter writer;

			private int fixedInstanceCount;
		}

		private delegate void ObjectScanner(FastSerializer.SerializationContext context, object instance);

		private delegate void ObjectWriter(FastSerializer.SerializationContext context, object instance);

		private delegate void TypeSerializer(object instance, FastSerializer.SerializationContext context);

		private sealed class DeserializationContext
		{
			public object ReadObject()
			{
				if (this.Objects.Length <= 65535)
				{
					return this.Objects[(int)this.Reader.ReadUInt16()];
				}
				return this.Objects[this.Reader.ReadInt32()];
			}

			internal int ReadTypeID()
			{
				if (this.Types.Length <= 65535)
				{
					return (int)this.Reader.ReadUInt16();
				}
				return this.Reader.ReadInt32();
			}

			internal void DeserializeTypeDescriptions()
			{
				for (int i = 0; i < this.Types.Length; i++)
				{
					Type type = this.Types[i];
					if (!type.IsGenericTypeDefinition && !type.HasElementType)
					{
						int num = this.Reader.ReadInt32();
						if (num != FastSerializerVersionAttribute.GetVersionNumber(type))
						{
							throw new SerializationException(string.Concat(new object[]
							{
								"Type '",
								type.FullName,
								"' was serialized with version ",
								num,
								", but is version ",
								FastSerializerVersionAttribute.GetVersionNumber(type)
							}));
						}
						bool flag = typeof(ISerializable).IsAssignableFrom(type);
						bool flag2 = type.IsPrimitive || flag;
						byte b = this.Reader.ReadByte();
						if (b == 255)
						{
							if (!flag2)
							{
								throw new SerializationException("Type '" + type.FullName + "' was serialized as special type, but isn't special now.");
							}
						}
						else
						{
							if (flag2)
							{
								throw new SerializationException("Type '" + type.FullName + "' wasn't serialized as special type, but is special now.");
							}
							List<FieldInfo> serializableFields = FastSerializer.GetSerializableFields(this.Types[i]);
							if (serializableFields.Count != (int)b)
							{
								throw new SerializationException("Number of fields on " + type.FullName + " has changed.");
							}
							for (int j = 0; j < (int)b; j++)
							{
								int num2 = this.ReadTypeID();
								string text = this.Reader.ReadString();
								FieldInfo fieldInfo = serializableFields[j];
								if (fieldInfo.Name != text)
								{
									throw new SerializationException("Field mismatch on type " + type.FullName);
								}
								if (fieldInfo.FieldType != this.Types[num2])
								{
									throw new SerializationException(string.Concat(new object[]
									{
										type.FullName,
										".",
										text,
										" was serialized as ",
										this.Types[num2],
										", but now is ",
										fieldInfo.FieldType
									}));
								}
							}
						}
					}
				}
			}

			public Type[] Types;

			public object[] Objects;

			public BinaryReader Reader;
		}

		private delegate void ObjectReader(FastSerializer.DeserializationContext context, object instance);

		private struct CustomDeserialization
		{
			public CustomDeserialization(object instance, SerializationInfo serializationInfo, FastSerializer.CustomDeserializationAction action)
			{
				this.instance = instance;
				this.serializationInfo = serializationInfo;
				this.action = action;
			}

			public void Run(StreamingContext context)
			{
				this.action(this.instance, this.serializationInfo, context);
			}

			private readonly object instance;

			private readonly SerializationInfo serializationInfo;

			private readonly FastSerializer.CustomDeserializationAction action;
		}

		private delegate void CustomDeserializationAction(object instance, SerializationInfo info, StreamingContext context);
	}
}
