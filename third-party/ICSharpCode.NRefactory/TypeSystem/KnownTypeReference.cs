using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Contains well-known type references.
	/// </summary>
	[Serializable]
	public sealed class KnownTypeReference : ITypeReference
	{
		/// <summary>
		/// Gets the known type reference for the specified type code.
		/// Returns null for KnownTypeCode.None.
		/// </summary>
		public static KnownTypeReference Get(KnownTypeCode typeCode)
		{
			return KnownTypeReference.knownTypeReferences[(int)typeCode];
		}

		private KnownTypeReference(KnownTypeCode knownTypeCode, string namespaceName, string name, int typeParameterCount = 0, KnownTypeCode baseType = KnownTypeCode.Object)
		{
			this.knownTypeCode = knownTypeCode;
			this.namespaceName = namespaceName;
			this.name = name;
			this.typeParameterCount = typeParameterCount;
			this.baseType = baseType;
		}

		public KnownTypeCode KnownTypeCode
		{
			get
			{
				return this.knownTypeCode;
			}
		}

		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public int TypeParameterCount
		{
			get
			{
				return this.typeParameterCount;
			}
		}

		public IType Resolve(ITypeResolveContext context)
		{
			return context.Compilation.FindType(this.knownTypeCode);
		}

		public override string ToString()
		{
			return KnownTypeReference.GetCSharpNameByTypeCode(this.knownTypeCode) ?? (this.Namespace + "." + this.Name);
		}

		/// <summary>
		/// Gets the C# primitive type name from the known type code.
		/// Returns null if there is no primitive name for the specified type.
		/// </summary>
		public static string GetCSharpNameByTypeCode(KnownTypeCode knownTypeCode)
		{
			switch (knownTypeCode)
			{
			case KnownTypeCode.Object:
				return "object";
			case KnownTypeCode.Boolean:
				return "bool";
			case KnownTypeCode.Char:
				return "char";
			case KnownTypeCode.SByte:
				return "sbyte";
			case KnownTypeCode.Byte:
				return "byte";
			case KnownTypeCode.Int16:
				return "short";
			case KnownTypeCode.UInt16:
				return "ushort";
			case KnownTypeCode.Int32:
				return "int";
			case KnownTypeCode.UInt32:
				return "uint";
			case KnownTypeCode.Int64:
				return "long";
			case KnownTypeCode.UInt64:
				return "ulong";
			case KnownTypeCode.Single:
				return "float";
			case KnownTypeCode.Double:
				return "double";
			case KnownTypeCode.Decimal:
				return "decimal";
			case KnownTypeCode.String:
				return "string";
			case KnownTypeCode.Void:
				return "void";
			}
			return null;
		}

		internal const int KnownTypeCodeCount = 46;

		private static readonly KnownTypeReference[] knownTypeReferences = new KnownTypeReference[]
		{
			null,
			new KnownTypeReference(KnownTypeCode.Object, "System", "Object", 0, KnownTypeCode.None),
			new KnownTypeReference(KnownTypeCode.DBNull, "System", "DBNull", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Boolean, "System", "Boolean", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Char, "System", "Char", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.SByte, "System", "SByte", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Byte, "System", "Byte", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Int16, "System", "Int16", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.UInt16, "System", "UInt16", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Int32, "System", "Int32", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.UInt32, "System", "UInt32", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Int64, "System", "Int64", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.UInt64, "System", "UInt64", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Single, "System", "Single", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Double, "System", "Double", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Decimal, "System", "Decimal", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.DateTime, "System", "DateTime", 0, KnownTypeCode.ValueType),
			null,
			new KnownTypeReference(KnownTypeCode.String, "System", "String", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Void, "System", "Void", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Type, "System", "Type", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Array, "System", "Array", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Attribute, "System", "Attribute", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.ValueType, "System", "ValueType", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Enum, "System", "Enum", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.Delegate, "System", "Delegate", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.MulticastDelegate, "System", "MulticastDelegate", 0, KnownTypeCode.Delegate),
			new KnownTypeReference(KnownTypeCode.Exception, "System", "Exception", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IntPtr, "System", "IntPtr", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.UIntPtr, "System", "UIntPtr", 0, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.IEnumerable, "System.Collections", "IEnumerable", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IEnumerator, "System.Collections", "IEnumerator", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IEnumerableOfT, "System.Collections.Generic", "IEnumerable", 1, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IEnumeratorOfT, "System.Collections.Generic", "IEnumerator", 1, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.ICollection, "System.Collections", "ICollection", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.ICollectionOfT, "System.Collections.Generic", "ICollection", 1, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IList, "System.Collections", "IList", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IListOfT, "System.Collections.Generic", "IList", 1, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IReadOnlyCollectionOfT, "System.Collections.Generic", "IReadOnlyCollection", 1, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.IReadOnlyListOfT, "System.Collections.Generic", "IReadOnlyList", 1, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.Task, "System.Threading.Tasks", "Task", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.TaskOfT, "System.Threading.Tasks", "Task", 1, KnownTypeCode.Task),
			new KnownTypeReference(KnownTypeCode.NullableOfT, "System", "Nullable", 1, KnownTypeCode.ValueType),
			new KnownTypeReference(KnownTypeCode.IDisposable, "System", "IDisposable", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.INotifyCompletion, "System.Runtime.CompilerServices", "INotifyCompletion", 0, KnownTypeCode.Object),
			new KnownTypeReference(KnownTypeCode.ICriticalNotifyCompletion, "System.Runtime.CompilerServices", "ICriticalNotifyCompletion", 0, KnownTypeCode.Object)
		};

		/// <summary>
		/// Gets a type reference pointing to the <c>object</c> type.
		/// </summary>
		public static readonly KnownTypeReference Object = KnownTypeReference.Get(KnownTypeCode.Object);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.DBNull</c> type.
		/// </summary>
		public static readonly KnownTypeReference DBNull = KnownTypeReference.Get(KnownTypeCode.DBNull);

		/// <summary>
		/// Gets a type reference pointing to the <c>bool</c> type.
		/// </summary>
		public static readonly KnownTypeReference Boolean = KnownTypeReference.Get(KnownTypeCode.Boolean);

		/// <summary>
		/// Gets a type reference pointing to the <c>char</c> type.
		/// </summary>
		public static readonly KnownTypeReference Char = KnownTypeReference.Get(KnownTypeCode.Char);

		/// <summary>
		/// Gets a type reference pointing to the <c>sbyte</c> type.
		/// </summary>
		public static readonly KnownTypeReference SByte = KnownTypeReference.Get(KnownTypeCode.SByte);

		/// <summary>
		/// Gets a type reference pointing to the <c>byte</c> type.
		/// </summary>
		public static readonly KnownTypeReference Byte = KnownTypeReference.Get(KnownTypeCode.Byte);

		/// <summary>
		/// Gets a type reference pointing to the <c>short</c> type.
		/// </summary>
		public static readonly KnownTypeReference Int16 = KnownTypeReference.Get(KnownTypeCode.Int16);

		/// <summary>
		/// Gets a type reference pointing to the <c>ushort</c> type.
		/// </summary>
		public static readonly KnownTypeReference UInt16 = KnownTypeReference.Get(KnownTypeCode.UInt16);

		/// <summary>
		/// Gets a type reference pointing to the <c>int</c> type.
		/// </summary>
		public static readonly KnownTypeReference Int32 = KnownTypeReference.Get(KnownTypeCode.Int32);

		/// <summary>
		/// Gets a type reference pointing to the <c>uint</c> type.
		/// </summary>
		public static readonly KnownTypeReference UInt32 = KnownTypeReference.Get(KnownTypeCode.UInt32);

		/// <summary>
		/// Gets a type reference pointing to the <c>long</c> type.
		/// </summary>
		public static readonly KnownTypeReference Int64 = KnownTypeReference.Get(KnownTypeCode.Int64);

		/// <summary>
		/// Gets a type reference pointing to the <c>ulong</c> type.
		/// </summary>
		public static readonly KnownTypeReference UInt64 = KnownTypeReference.Get(KnownTypeCode.UInt64);

		/// <summary>
		/// Gets a type reference pointing to the <c>float</c> type.
		/// </summary>
		public static readonly KnownTypeReference Single = KnownTypeReference.Get(KnownTypeCode.Single);

		/// <summary>
		/// Gets a type reference pointing to the <c>double</c> type.
		/// </summary>
		public static readonly KnownTypeReference Double = KnownTypeReference.Get(KnownTypeCode.Double);

		/// <summary>
		/// Gets a type reference pointing to the <c>decimal</c> type.
		/// </summary>
		public static readonly KnownTypeReference Decimal = KnownTypeReference.Get(KnownTypeCode.Decimal);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.DateTime</c> type.
		/// </summary>
		public static readonly KnownTypeReference DateTime = KnownTypeReference.Get(KnownTypeCode.DateTime);

		/// <summary>
		/// Gets a type reference pointing to the <c>string</c> type.
		/// </summary>
		public static readonly KnownTypeReference String = KnownTypeReference.Get(KnownTypeCode.String);

		/// <summary>
		/// Gets a type reference pointing to the <c>void</c> type.
		/// </summary>
		public static readonly KnownTypeReference Void = KnownTypeReference.Get(KnownTypeCode.Void);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Type</c> type.
		/// </summary>
		public static readonly KnownTypeReference Type = KnownTypeReference.Get(KnownTypeCode.Type);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Array</c> type.
		/// </summary>
		public static readonly KnownTypeReference Array = KnownTypeReference.Get(KnownTypeCode.Array);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Attribute</c> type.
		/// </summary>
		public static readonly KnownTypeReference Attribute = KnownTypeReference.Get(KnownTypeCode.Attribute);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.ValueType</c> type.
		/// </summary>
		public static readonly KnownTypeReference ValueType = KnownTypeReference.Get(KnownTypeCode.ValueType);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Enum</c> type.
		/// </summary>
		public static readonly KnownTypeReference Enum = KnownTypeReference.Get(KnownTypeCode.Enum);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Delegate</c> type.
		/// </summary>
		public static readonly KnownTypeReference Delegate = KnownTypeReference.Get(KnownTypeCode.Delegate);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.MulticastDelegate</c> type.
		/// </summary>
		public static readonly KnownTypeReference MulticastDelegate = KnownTypeReference.Get(KnownTypeCode.MulticastDelegate);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Exception</c> type.
		/// </summary>
		public static readonly KnownTypeReference Exception = KnownTypeReference.Get(KnownTypeCode.Exception);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.IntPtr</c> type.
		/// </summary>
		public static readonly KnownTypeReference IntPtr = KnownTypeReference.Get(KnownTypeCode.IntPtr);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.UIntPtr</c> type.
		/// </summary>
		public static readonly KnownTypeReference UIntPtr = KnownTypeReference.Get(KnownTypeCode.UIntPtr);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.IEnumerable</c> type.
		/// </summary>
		public static readonly KnownTypeReference IEnumerable = KnownTypeReference.Get(KnownTypeCode.IEnumerable);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.IEnumerator</c> type.
		/// </summary>
		public static readonly KnownTypeReference IEnumerator = KnownTypeReference.Get(KnownTypeCode.IEnumerator);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.Generic.IEnumerable{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference IEnumerableOfT = KnownTypeReference.Get(KnownTypeCode.IEnumerableOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.Generic.IEnumerator{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference IEnumeratorOfT = KnownTypeReference.Get(KnownTypeCode.IEnumeratorOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.ICollection</c> type.
		/// </summary>
		public static readonly KnownTypeReference ICollection = KnownTypeReference.Get(KnownTypeCode.ICollection);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.Generic.ICollection{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference ICollectionOfT = KnownTypeReference.Get(KnownTypeCode.ICollectionOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.IList</c> type.
		/// </summary>
		public static readonly KnownTypeReference IList = KnownTypeReference.Get(KnownTypeCode.IList);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.Generic.IList{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference IListOfT = KnownTypeReference.Get(KnownTypeCode.IListOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.Generic.IReadOnlyCollection{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference IReadOnlyCollectionOfT = KnownTypeReference.Get(KnownTypeCode.IReadOnlyCollectionOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Collections.Generic.IReadOnlyList{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference IReadOnlyListOfT = KnownTypeReference.Get(KnownTypeCode.IReadOnlyListOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Threading.Tasks.Task</c> type.
		/// </summary>
		public static readonly KnownTypeReference Task = KnownTypeReference.Get(KnownTypeCode.Task);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Threading.Tasks.Task{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference TaskOfT = KnownTypeReference.Get(KnownTypeCode.TaskOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Nullable{T}</c> type.
		/// </summary>
		public static readonly KnownTypeReference NullableOfT = KnownTypeReference.Get(KnownTypeCode.NullableOfT);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.IDisposable</c> type.
		/// </summary>
		public static readonly KnownTypeReference IDisposable = KnownTypeReference.Get(KnownTypeCode.IDisposable);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Runtime.CompilerServices.INotifyCompletion</c> type.
		/// </summary>
		public static readonly KnownTypeReference INotifyCompletion = KnownTypeReference.Get(KnownTypeCode.INotifyCompletion);

		/// <summary>
		/// Gets a type reference pointing to the <c>System.Runtime.CompilerServices.ICriticalNotifyCompletion</c> type.
		/// </summary>
		public static readonly KnownTypeReference ICriticalNotifyCompletion = KnownTypeReference.Get(KnownTypeCode.ICriticalNotifyCompletion);

		private readonly KnownTypeCode knownTypeCode;

		private readonly string namespaceName;

		private readonly string name;

		private readonly int typeParameterCount;

		internal readonly KnownTypeCode baseType;
	}
}
