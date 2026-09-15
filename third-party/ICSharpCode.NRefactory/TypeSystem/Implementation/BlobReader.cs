using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	internal sealed class BlobReader
	{
		internal static int GetBlobHashCode(byte[] blob)
		{
			int num = 0;
			foreach (byte b in blob)
			{
				num *= 257;
				num += (int)b;
			}
			return num;
		}

		internal static bool BlobEquals(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
				{
					return false;
				}
			}
			return true;
		}

		public BlobReader(byte[] buffer, IAssembly currentResolvedAssembly)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			this.buffer = buffer;
			this.currentResolvedAssembly = currentResolvedAssembly;
		}

		public byte ReadByte()
		{
			return this.buffer[this.position++];
		}

		public sbyte ReadSByte()
		{
			return (sbyte)this.ReadByte();
		}

		public byte[] ReadBytes(int length)
		{
			byte[] array = new byte[length];
			Buffer.BlockCopy(this.buffer, this.position, array, 0, length);
			this.position += length;
			return array;
		}

		public ushort ReadUInt16()
		{
			ushort result = (ushort)((int)this.buffer[this.position] | (int)this.buffer[this.position + 1] << 8);
			this.position += 2;
			return result;
		}

		public short ReadInt16()
		{
			return (short)this.ReadUInt16();
		}

		public uint ReadUInt32()
		{
			uint result = (uint)((int)this.buffer[this.position] | (int)this.buffer[this.position + 1] << 8 | (int)this.buffer[this.position + 2] << 16 | (int)this.buffer[this.position + 3] << 24);
			this.position += 4;
			return result;
		}

		public int ReadInt32()
		{
			return (int)this.ReadUInt32();
		}

		public ulong ReadUInt64()
		{
			uint num = this.ReadUInt32();
			uint num2 = this.ReadUInt32();
			return (ulong)num2 << 32 | (ulong)num;
		}

		public long ReadInt64()
		{
			return (long)this.ReadUInt64();
		}

		public uint ReadCompressedUInt32()
		{
			byte b = this.ReadByte();
			if ((b & 128) == 0)
			{
				return (uint)b;
			}
			if ((b & 64) == 0)
			{
				return ((uint)b & 4294967167U) << 8 | (uint)this.ReadByte();
			}
			return (uint)(((int)b & -193) << 24 | (int)this.ReadByte() << 16 | (int)this.ReadByte() << 8 | (int)this.ReadByte());
		}

		public float ReadSingle()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(4);
				Array.Reverse(array);
				return BitConverter.ToSingle(array, 0);
			}
			float result = BitConverter.ToSingle(this.buffer, this.position);
			this.position += 4;
			return result;
		}

		public double ReadDouble()
		{
			if (!BitConverter.IsLittleEndian)
			{
				byte[] array = this.ReadBytes(8);
				Array.Reverse(array);
				return BitConverter.ToDouble(array, 0);
			}
			double result = BitConverter.ToDouble(this.buffer, this.position);
			this.position += 8;
			return result;
		}

		public ResolveResult ReadFixedArg(IType argType)
		{
			if (argType.Kind != TypeKind.Array)
			{
				return this.ReadElem(argType);
			}
			if (((ArrayType)argType).Dimensions != 1)
			{
				return ErrorResolveResult.UnknownError;
			}
			IType elementType = ((ArrayType)argType).ElementType;
			uint num = this.ReadUInt32();
			if (num == 4294967295U)
			{
				return new ConstantResolveResult(argType, null);
			}
			ResolveResult[] array = new ResolveResult[num];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.ReadElem(elementType);
				if (array[i].IsError)
				{
					return ErrorResolveResult.UnknownError;
				}
			}
			IType type = this.currentResolvedAssembly.Compilation.FindType(KnownTypeCode.Int32);
			ResolveResult[] sizeArguments = new ResolveResult[]
			{
				new ConstantResolveResult(type, array.Length)
			};
			return new ArrayCreateResolveResult(argType, sizeArguments, array);
		}

		public ResolveResult ReadElem(IType elementType)
		{
			ITypeDefinition definition;
			if (elementType.Kind == TypeKind.Enum)
			{
				definition = elementType.GetDefinition().EnumUnderlyingType.GetDefinition();
			}
			else
			{
				definition = elementType.GetDefinition();
			}
			if (definition == null)
			{
				return ErrorResolveResult.UnknownError;
			}
			KnownTypeCode knownTypeCode = definition.KnownTypeCode;
			if (knownTypeCode == KnownTypeCode.Object)
			{
				IType elementType2 = this.ReadCustomAttributeFieldOrPropType();
				ResolveResult resolveResult = this.ReadElem(elementType2);
				if (resolveResult.IsCompileTimeConstant && resolveResult.ConstantValue == null)
				{
					return new ConstantResolveResult(elementType, null);
				}
				return new ConversionResolveResult(elementType, resolveResult, Conversion.BoxingConversion);
			}
			else
			{
				if (knownTypeCode == KnownTypeCode.Type)
				{
					return new TypeOfResolveResult(definition, this.ReadType());
				}
				return new ConstantResolveResult(elementType, this.ReadElemValue(knownTypeCode));
			}
		}

		private object ReadElemValue(KnownTypeCode typeCode)
		{
			switch (typeCode)
			{
			case KnownTypeCode.Boolean:
				return this.ReadByte() != 0;
			case KnownTypeCode.Char:
				return (char)this.ReadUInt16();
			case KnownTypeCode.SByte:
				return this.ReadSByte();
			case KnownTypeCode.Byte:
				return this.ReadByte();
			case KnownTypeCode.Int16:
				return this.ReadInt16();
			case KnownTypeCode.UInt16:
				return this.ReadUInt16();
			case KnownTypeCode.Int32:
				return this.ReadInt32();
			case KnownTypeCode.UInt32:
				return this.ReadUInt32();
			case KnownTypeCode.Int64:
				return this.ReadInt64();
			case KnownTypeCode.UInt64:
				return this.ReadUInt64();
			case KnownTypeCode.Single:
				return this.ReadSingle();
			case KnownTypeCode.Double:
				return this.ReadDouble();
			case KnownTypeCode.String:
				return this.ReadSerString();
			}
			throw new NotSupportedException();
		}

		public string ReadSerString()
		{
			if (this.buffer[this.position] == 255)
			{
				this.position++;
				return null;
			}
			int num = (int)this.ReadCompressedUInt32();
			if (num == 0)
			{
				return string.Empty;
			}
			string @string = Encoding.UTF8.GetString(this.buffer, this.position, (this.buffer[this.position + num - 1] == 0) ? (num - 1) : num);
			this.position += num;
			return @string;
		}

		public KeyValuePair<IMember, ResolveResult> ReadNamedArg(IType attributeType)
		{
			byte b = this.ReadByte();
			SymbolKind memberType;
			switch (b)
			{
			case 83:
				memberType = SymbolKind.Field;
				break;
			case 84:
				memberType = SymbolKind.Property;
				break;
			default:
				throw new NotSupportedException(string.Format("Custom member type 0x{0:x} is not supported.", b));
			}
			IType type = this.ReadCustomAttributeFieldOrPropType();
			string name = this.ReadSerString();
			ResolveResult value = this.ReadFixedArg(type);
			IMember key = null;
			foreach (IMember member in attributeType.GetMembers((IUnresolvedMember m) => m.SymbolKind == memberType && m.Name == name, GetMemberOptions.None))
			{
				if (member.ReturnType.Equals(type))
				{
					key = member;
				}
			}
			return new KeyValuePair<IMember, ResolveResult>(key, value);
		}

		private IType ReadCustomAttributeFieldOrPropType()
		{
			ICompilation compilation = this.currentResolvedAssembly.Compilation;
			byte b = this.ReadByte();
			byte b2 = b;
			if (b2 <= 29)
			{
				switch (b2)
				{
				case 2:
					return compilation.FindType(KnownTypeCode.Boolean);
				case 3:
					return compilation.FindType(KnownTypeCode.Char);
				case 4:
					return compilation.FindType(KnownTypeCode.SByte);
				case 5:
					return compilation.FindType(KnownTypeCode.Byte);
				case 6:
					return compilation.FindType(KnownTypeCode.Int16);
				case 7:
					return compilation.FindType(KnownTypeCode.UInt16);
				case 8:
					return compilation.FindType(KnownTypeCode.Int32);
				case 9:
					return compilation.FindType(KnownTypeCode.UInt32);
				case 10:
					return compilation.FindType(KnownTypeCode.Int64);
				case 11:
					return compilation.FindType(KnownTypeCode.UInt64);
				case 12:
					return compilation.FindType(KnownTypeCode.Single);
				case 13:
					return compilation.FindType(KnownTypeCode.Double);
				case 14:
					return compilation.FindType(KnownTypeCode.String);
				default:
					if (b2 == 29)
					{
						return new ArrayType(compilation, this.ReadCustomAttributeFieldOrPropType(), 1);
					}
					break;
				}
			}
			else
			{
				switch (b2)
				{
				case 80:
					return compilation.FindType(KnownTypeCode.Type);
				case 81:
					return compilation.FindType(KnownTypeCode.Object);
				default:
					if (b2 == 85)
					{
						return this.ReadType();
					}
					break;
				}
			}
			throw new NotSupportedException(string.Format("Custom attribute type 0x{0:x} is not supported.", b));
		}

		private IType ReadType()
		{
			string reflectionTypeName = this.ReadSerString();
			ITypeReference typeReference = ReflectionHelper.ParseReflectionName(reflectionTypeName);
			IType type = typeReference.Resolve(new SimpleTypeResolveContext(this.currentResolvedAssembly));
			if (type.Kind != TypeKind.Unknown)
			{
				return type;
			}
			ITypeDefinition definition = this.currentResolvedAssembly.Compilation.FindType(KnownTypeCode.Object).GetDefinition();
			if (definition != null)
			{
				return typeReference.Resolve(new SimpleTypeResolveContext(definition.ParentAssembly));
			}
			return type;
		}

		private byte[] buffer;

		private int position;

		private readonly IAssembly currentResolvedAssembly;
	}
}
