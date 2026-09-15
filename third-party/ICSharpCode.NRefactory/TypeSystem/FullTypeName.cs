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
	[Serializable]
	public struct FullTypeName : IEquatable<FullTypeName>
	{
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

		public static implicit operator FullTypeName(TopLevelTypeName topLevelTypeName)
		{
			return new FullTypeName(topLevelTypeName);
		}

		public override string ToString()
		{
			return this.ReflectionName;
		}

		public override bool Equals(object obj)
		{
			return obj is FullTypeName && this.Equals((FullTypeName)obj);
		}

		public bool Equals(FullTypeName other)
		{
			return FullTypeNameComparer.Ordinal.Equals(this, other);
		}

		public override int GetHashCode()
		{
			return FullTypeNameComparer.Ordinal.GetHashCode(this);
		}

		public static bool operator ==(FullTypeName left, FullTypeName right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(FullTypeName left, FullTypeName right)
		{
			return !left.Equals(right);
		}

		private readonly TopLevelTypeName topLevelType;

		private readonly FullTypeName.NestedTypeName[] nestedTypes;

		[Serializable]
		private struct NestedTypeName
		{
			public NestedTypeName(string name, int additionalTypeParameterCount)
			{
				if (name == null)
				{
					throw new ArgumentNullException("name");
				}
				this.Name = name;
				this.AdditionalTypeParameterCount = additionalTypeParameterCount;
			}

			public readonly string Name;

			public readonly int AdditionalTypeParameterCount;
		}
	}
}
