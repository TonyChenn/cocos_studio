using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200006D RID: 109
	public struct AssemblyQualifiedTypeName : IEquatable<AssemblyQualifiedTypeName>
	{
		// Token: 0x06000382 RID: 898 RVA: 0x000088C1 File Offset: 0x000078C1
		public AssemblyQualifiedTypeName(FullTypeName typeName, string assemblyName)
		{
			this.AssemblyName = assemblyName;
			this.TypeName = typeName;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x000088D1 File Offset: 0x000078D1
		public AssemblyQualifiedTypeName(ITypeDefinition typeDefinition)
		{
			this.AssemblyName = typeDefinition.ParentAssembly.AssemblyName;
			this.TypeName = typeDefinition.FullTypeName;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x000088F0 File Offset: 0x000078F0
		public override string ToString()
		{
			if (string.IsNullOrEmpty(this.AssemblyName))
			{
				return this.TypeName.ToString();
			}
			return this.TypeName.ToString() + ", " + this.AssemblyName;
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00008943 File Offset: 0x00007943
		public override bool Equals(object obj)
		{
			return obj is AssemblyQualifiedTypeName && this.Equals((AssemblyQualifiedTypeName)obj);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0000895B File Offset: 0x0000795B
		public bool Equals(AssemblyQualifiedTypeName other)
		{
			return this.AssemblyName == other.AssemblyName && this.TypeName == other.TypeName;
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00008988 File Offset: 0x00007988
		public override int GetHashCode()
		{
			int num = 0;
			if (this.AssemblyName != null)
			{
				num += 1000000007 * this.AssemblyName.GetHashCode();
			}
			return num + this.TypeName.GetHashCode();
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000089CB File Offset: 0x000079CB
		public static bool operator ==(AssemblyQualifiedTypeName lhs, AssemblyQualifiedTypeName rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x000089D5 File Offset: 0x000079D5
		public static bool operator !=(AssemblyQualifiedTypeName lhs, AssemblyQualifiedTypeName rhs)
		{
			return !lhs.Equals(rhs);
		}

		// Token: 0x040000DD RID: 221
		public readonly string AssemblyName;

		// Token: 0x040000DE RID: 222
		public readonly FullTypeName TypeName;
	}
}
