using System;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Holds the name of a top-level type.
	/// This struct cannot refer to nested classes.
	/// </summary>
	[Serializable]
	public struct TopLevelTypeName : IEquatable<TopLevelTypeName>
	{
		public TopLevelTypeName(string namespaceName, string name, int typeParameterCount = 0)
		{
			if (namespaceName == null)
			{
				throw new ArgumentNullException("namespaceName");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.namespaceName = namespaceName;
			this.name = name;
			this.typeParameterCount = typeParameterCount;
		}

		public TopLevelTypeName(string reflectionName)
		{
			int num = reflectionName.LastIndexOf('.');
			if (num < 0)
			{
				this.namespaceName = string.Empty;
				this.name = reflectionName;
			}
			else
			{
				this.namespaceName = reflectionName.Substring(0, num);
				this.name = reflectionName.Substring(num + 1);
			}
			this.name = ReflectionHelper.SplitTypeParameterCountFromReflectionName(this.name, out this.typeParameterCount);
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

		public string ReflectionName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (!string.IsNullOrEmpty(this.namespaceName))
				{
					stringBuilder.Append(this.namespaceName);
					stringBuilder.Append('.');
				}
				stringBuilder.Append(this.name);
				if (this.typeParameterCount > 0)
				{
					stringBuilder.Append('`');
					stringBuilder.Append(this.typeParameterCount);
				}
				return stringBuilder.ToString();
			}
		}

		public override string ToString()
		{
			return this.ReflectionName;
		}

		public override bool Equals(object obj)
		{
			return obj is TopLevelTypeName && this.Equals((TopLevelTypeName)obj);
		}

		public bool Equals(TopLevelTypeName other)
		{
			return this.namespaceName == other.namespaceName && this.name == other.name && this.typeParameterCount == other.typeParameterCount;
		}

		public override int GetHashCode()
		{
			return ((this.name != null) ? this.name.GetHashCode() : 0) ^ ((this.namespaceName != null) ? this.namespaceName.GetHashCode() : 0) ^ this.typeParameterCount;
		}

		public static bool operator ==(TopLevelTypeName lhs, TopLevelTypeName rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(TopLevelTypeName lhs, TopLevelTypeName rhs)
		{
			return !lhs.Equals(rhs);
		}

		private readonly string namespaceName;

		private readonly string name;

		private readonly int typeParameterCount;
	}
}
