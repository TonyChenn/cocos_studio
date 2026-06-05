using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// A type reference of the form 'Some.Namespace.TopLevelType.NestedType`n'.
	/// We do not know the boundary between namespace name and top level type, so we have to try
	/// all possibilities.
	/// The type parameter count only applies to the innermost type, all outer types must be non-generic.
	/// </summary>
	// Token: 0x02000009 RID: 9
	[Serializable]
	internal class GetPotentiallyNestedClassTypeReference : ITypeReference
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002B2E File Offset: 0x00001B2E
		public GetPotentiallyNestedClassTypeReference(string typeName, int typeParameterCount)
		{
			this.typeName = typeName;
			this.typeParameterCount = typeParameterCount;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002B94 File Offset: 0x00001B94
		public IType Resolve(ITypeResolveContext context)
		{
			string[] parts = this.typeName.Split(new char[]
			{
				'.'
			});
			IEnumerable<IAssembly> enumerable = new IAssembly[]
			{
				context.CurrentAssembly
			}.Concat(context.Compilation.Assemblies);
			for (int i = parts.Length - 1; i >= 0; i--)
			{
				string namespaceName = string.Join(".", parts, 0, i);
				string name = parts[i];
				int num = (i == parts.Length - 1) ? this.typeParameterCount : 0;
				foreach (IAssembly assembly in enumerable)
				{
					if (assembly != null)
					{
						ITypeDefinition typeDefinition = assembly.GetTypeDefinition(new TopLevelTypeName(namespaceName, name, num));
						int j = i + 1;
						while (j < parts.Length && typeDefinition != null)
						{
							int tpc = (j == parts.Length - 1) ? this.typeParameterCount : 0;
							typeDefinition = typeDefinition.NestedTypes.FirstOrDefault((ITypeDefinition n) => n.Name == parts[j] && n.TypeParameterCount == tpc);
							j++;
						}
						if (typeDefinition != null)
						{
							return typeDefinition;
						}
					}
				}
			}
			int num2 = this.typeName.LastIndexOf('.');
			if (num2 < 0)
			{
				return new UnknownType("", this.typeName, this.typeParameterCount);
			}
			return new UnknownType(this.typeName.Substring(0, num2), this.typeName.Substring(num2 + 1), this.typeParameterCount);
		}

		// Token: 0x04000010 RID: 16
		private readonly string typeName;

		// Token: 0x04000011 RID: 17
		private readonly int typeParameterCount;
	}
}
