using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public struct AssemblyQualifiedTypeName : IEquatable<AssemblyQualifiedTypeName>
	{
		public AssemblyQualifiedTypeName(FullTypeName typeName, string assemblyName)
		{
			this.AssemblyName = assemblyName;
			this.TypeName = typeName;
		}

		public AssemblyQualifiedTypeName(ITypeDefinition typeDefinition)
		{
			this.AssemblyName = typeDefinition.ParentAssembly.AssemblyName;
			this.TypeName = typeDefinition.FullTypeName;
		}

		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.AssemblyName))
			{
				return this.TypeName.ToString();
			}
			return this.TypeName.ToString() + ", " + this.AssemblyName;
		}

		public override bool Equals(object obj)
		{
			return obj is AssemblyQualifiedTypeName && this.Equals((AssemblyQualifiedTypeName)obj);
		}

		public bool Equals(AssemblyQualifiedTypeName other)
		{
			return this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (this.AssemblyName != null)
			{
				num += 1000000007 * this.AssemblyName.GetHashCode();
			}
			return num + this.TypeName.GetHashCode();
		}

		public static bool operator ==(AssemblyQualifiedTypeName lhs, AssemblyQualifiedTypeName rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(AssemblyQualifiedTypeName lhs, AssemblyQualifiedTypeName rhs)
		{
			return !lhs.Equals(rhs);
		}

		public readonly string AssemblyName;

		public readonly FullTypeName TypeName;
	}
}
