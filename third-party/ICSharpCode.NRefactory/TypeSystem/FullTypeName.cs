using System;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Holds the full name of a type definition.
	/// A full type name uniquely identifies a type definition within a single assembly.
	/// </summary>
	/// <remarks>
	/// A full type name can only represent type definitions, not arbitrary types.
	/// It does not include any type arguments, and can not refer to array or pointer types.
	///
	/// A full type name represented as reflection name has the syntax:
	/// <c>NamespaceName '.' TopLevelTypeName ['`'#] { '+' NestedTypeName ['`'#] }</c>
	/// </remarks>
	// Token: 0x02000088 RID: 136
	[Serializable]
	public struct FullTypeName : IEquatable<FullTypeName>
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x0000B945 File Offset: 0x0000A945
		private FullTypeName(TopLevelTypeName topLevelTypeName, FullTypeName.NestedTypeName[] nestedTypes)
		{
			this.topLevelType = topLevelTypeName;
			this.nestedTypes = nestedTypes;
		}

		/// <summary>
		/// Constructs a FullTypeName representing the given top-level type.
		/// </summary>
		/// <remarks>
		/// FullTypeName has an implicit conversion operator from TopLevelTypeName,
		/// so you can simply write:
		/// <c>FullTypeName f = new TopLevelTypeName(...);</c>
		/// </remarks>
		// Token: 0x06000469 RID: 1129 RVA: 0x0000B955 File Offset: 0x0000A955
		public FullTypeName(TopLevelTypeName topLevelTypeName)
		{
			this.topLevelType = topLevelTypeName;
			this.nestedTypes = null;
		}

		/// <summary>
		/// Constructs a FullTypeName by parsing the given reflection name.
		/// Note that FullTypeName can only represent type definition names. If the reflection name
		/// might refer to a parameterized type or array etc., use
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.ReflectionHelper.ParseReflectionName(System.String)" /> instead.
		/// </summary>
		/// <remarks>
		/// Expected syntax: <c>NamespaceName '.' TopLevelTypeName ['`'#] { '+' NestedTypeName ['`'#] }</c>
		/// where # are type parameter counts
		/// </remarks>
		// Token: 0x0600046A RID: 1130 RVA: 0x0000B968 File Offset: 0x0000A968
		public FullTypeName(string reflectionName)
		{
			int num = reflectionName.IndexOf('+');
			if (num < 0)
			{
				this.topLevelType = new TopLevelTypeName(reflectionName);
				this.nestedTypes = null;
				return;
			}
			string[] array = reflectionName.Split(new char[]
			{
				'+'
			});
			this.topLevelType = new TopLevelTypeName(array[0]);
			this.nestedTypes = new FullTypeName.NestedTypeName[array.Length - 1];
			for (int i = 0; i < this.nestedTypes.Length; i++)
			{
				int additionalTypeParameterCount;
				string name = ReflectionHelper.SplitTypeParameterCountFromReflectionName(array[i + 1], out additionalTypeParameterCount);
				this.nestedTypes[i] = new FullTypeName.NestedTypeName(name, additionalTypeParameterCount);
			}
		}

		/// <summary>
		/// Gets the top-level type name.
		/// </summary>
		// Token: 0x17000190 RID: 400
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0000BA05 File Offset: 0x0000AA05
		public TopLevelTypeName TopLevelTypeName
		{
			get
			{
				return this.topLevelType;
			}
		}

		/// <summary>
		/// Gets whether this is a nested type.
		/// </summary>
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0000BA0D File Offset: 0x0000AA0D
		public bool IsNested
		{
			get
			{
				return this.nestedTypes != null;
			}
		}

		/// <summary>
		/// Gets the nesting level.
		/// </summary>
		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x0000BA1B File Offset: 0x0000AA1B
		public int NestingLevel
		{
			get
			{
				if (this.nestedTypes == null)
				{
					return 0;
				}
				return this.nestedTypes.Length;
			}
		}

		/// <summary>
		/// Gets the name of the type.
		/// For nested types, this is the name of the innermost type.
		/// </summary>
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000BA30 File Offset: 0x0000AA30
		public string Name
		{
			get
			{
				if (this.nestedTypes != null)
				{
					return this.nestedTypes[this.nestedTypes.Length - 1].Name;
				}
				return this.topLevelType.Name;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x0000BA74 File Offset: 0x0000AA74
		public string ReflectionName
		{
			get
			{
				if (this.nestedTypes == null)
				{
					return this.topLevelType.ReflectionName;
				}
				StringBuilder stringBuilder = new StringBuilder(this.topLevelType.ReflectionName);
				foreach (FullTypeName.NestedTypeName nestedTypeName in this.nestedTypes)
				{
					stringBuilder.Append('+');
					stringBuilder.Append(nestedTypeName.Name);
					if (nestedTypeName.AdditionalTypeParameterCount > 0)
					{
						stringBuilder.Append('`');
						stringBuilder.Append(nestedTypeName.AdditionalTypeParameterCount);
					}
				}
				return stringBuilder.ToString();
			}
		}

		/// <summary>
		/// Gets the total type parameter count.
		/// </summary>
		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x0000BB14 File Offset: 0x0000AB14
		public int TypeParameterCount
		{
			get
			{
				int num = this.topLevelType.TypeParameterCount;
				if (this.nestedTypes != null)
				{
					foreach (FullTypeName.NestedTypeName nestedTypeName in this.nestedTypes)
					{
						num += nestedTypeName.AdditionalTypeParameterCount;
					}
				}
				return num;
			}
		}

		/// <summary>
		/// Gets the name of the nested type at the given level.
		/// </summary>
		// Token: 0x06000471 RID: 1137 RVA: 0x0000BB6A File Offset: 0x0000AB6A
		public string GetNestedTypeName(int nestingLevel)
		{
			if (this.nestedTypes == null)
			{
				throw new InvalidOperationException();
			}
			return this.nestedTypes[nestingLevel].Name;
		}

		/// <summary>
		/// Gets the number of additional type parameters of the nested type at the given level.
		/// </summary>
		// Token: 0x06000472 RID: 1138 RVA: 0x0000BB90 File Offset: 0x0000AB90
		public int GetNestedTypeAdditionalTypeParameterCount(int nestingLevel)
		{
			if (this.nestedTypes == null)
			{
				throw new InvalidOperationException();
			}
			return this.nestedTypes[nestingLevel].AdditionalTypeParameterCount;
		}

		/// <summary>
		/// Gets the declaring type name.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">This is a top-level type name.</exception>
		/// <example><c>new FullTypeName("NS.A+B+C").GetDeclaringType()</c> will return <c>new FullTypeName("NS.A+B")</c></example>
		// Token: 0x06000473 RID: 1139 RVA: 0x0000BBB8 File Offset: 0x0000ABB8
		public FullTypeName GetDeclaringType()
		{
			if (this.nestedTypes == null)
			{
				throw new InvalidOperationException();
			}
			if (this.nestedTypes.Length == 1)
			{
				return this.topLevelType;
			}
			FullTypeName.NestedTypeName[] array = new FullTypeName.NestedTypeName[this.nestedTypes.Length - 1];
			Array.Copy(this.nestedTypes, 0, array, 0, array.Length);
			return new FullTypeName(this.topLevelType, this.nestedTypes);
		}

		/// <summary>
		/// Creates a nested type name.
		/// </summary>
		/// <example><c>new FullTypeName("NS.A+B").NestedType("C", 1)</c> will return <c>new FullTypeName("NS.A+B+C`1")</c></example>
		// Token: 0x06000474 RID: 1140 RVA: 0x0000BC1C File Offset: 0x0000AC1C
		public FullTypeName NestedType(string name, int additionalTypeParameterCount)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			FullTypeName.NestedTypeName nestedTypeName = new FullTypeName.NestedTypeName(name, additionalTypeParameterCount);
			if (this.nestedTypes == null)
			{
				return new FullTypeName(this.topLevelType, new FullTypeName.NestedTypeName[]
				{
					nestedTypeName
				});
			}
			FullTypeName.NestedTypeName[] array = new FullTypeName.NestedTypeName[this.nestedTypes.Length + 1];
			this.nestedTypes.CopyTo(array, 0);
			array[array.Length - 1] = nestedTypeName;
			return new FullTypeName(this.topLevelType, array);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000BCA3 File Offset: 0x0000ACA3
		public static implicit operator FullTypeName(TopLevelTypeName topLevelTypeName)
		{
			return new FullTypeName(topLevelTypeName);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000BCAB File Offset: 0x0000ACAB
		public override string ToString()
		{
			return this.ReflectionName;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000BCB3 File Offset: 0x0000ACB3
		public override bool Equals(object obj)
		{
			return obj is FullTypeName && this.Equals((FullTypeName)obj);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000BCCB File Offset: 0x0000ACCB
		public bool Equals(FullTypeName other)
		{
			return FullTypeNameComparer.Ordinal.Equals(this, other);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000BCDE File Offset: 0x0000ACDE
		public override int GetHashCode()
		{
			return FullTypeNameComparer.Ordinal.GetHashCode(this);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000BCF0 File Offset: 0x0000ACF0
		public static bool operator ==(FullTypeName left, FullTypeName right)
		{
			return left.Equals(right);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000BCFA File Offset: 0x0000ACFA
		public static bool operator !=(FullTypeName left, FullTypeName right)
		{
			return !left.Equals(right);
		}

		// Token: 0x04000139 RID: 313
		private readonly TopLevelTypeName topLevelType;

		// Token: 0x0400013A RID: 314
		private readonly FullTypeName.NestedTypeName[] nestedTypes;

		// Token: 0x02000089 RID: 137
		[Serializable]
		private struct NestedTypeName
		{
			// Token: 0x0600047C RID: 1148 RVA: 0x0000BD07 File Offset: 0x0000AD07
			public NestedTypeName(string name, int additionalTypeParameterCount)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				this.Name = name;
				this.AdditionalTypeParameterCount = additionalTypeParameterCount;
			}

			// Token: 0x0400013B RID: 315
			public readonly string Name;

			// Token: 0x0400013C RID: 316
			public readonly int AdditionalTypeParameterCount;
		}
	}
}
