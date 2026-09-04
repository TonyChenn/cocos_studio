using System;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Helper methods for COM.
	/// </summary>
	// Token: 0x02000070 RID: 112
	public static class ComHelper
	{
		// Token: 0x06000399 RID: 921 RVA: 0x00008B1F File Offset: 0x00007B1F
		private static bool IsComAttribute(IAttribute attribute, string name)
		{
			return attribute.AttributeType.Name == name && attribute.AttributeType.Namespace == "System.Runtime.InteropServices";
		}

		/// <summary>
		/// Gets whether the specified type is imported from COM.
		/// </summary>
		// Token: 0x0600039A RID: 922 RVA: 0x00008B58 File Offset: 0x00007B58
		public static bool IsComImport(ITypeDefinition typeDefinition)
		{
			if (typeDefinition != null && typeDefinition.Kind == TypeKind.Interface)
			{
				return typeDefinition.Attributes.Any((IAttribute a) => ComHelper.IsComAttribute(a, "ComImportAttribute"));
			}
			return false;
		}

		/// <summary>
		/// Gets the CoClass of the specified COM interface.
		/// </summary>
		// Token: 0x0600039B RID: 923 RVA: 0x00008BA0 File Offset: 0x00007BA0
		public static IType GetCoClass(ITypeDefinition typeDefinition)
		{
			if (typeDefinition == null)
			{
				return SpecialType.UnknownType;
			}
			IAttribute attribute = typeDefinition.Attributes.FirstOrDefault((IAttribute a) => ComHelper.IsComAttribute(a, "CoClassAttribute"));
			if (attribute != null && attribute.PositionalArguments.Count == 1)
			{
				TypeOfResolveResult typeOfResolveResult = attribute.PositionalArguments[0] as TypeOfResolveResult;
				if (typeOfResolveResult != null)
				{
					return typeOfResolveResult.ReferencedType;
				}
			}
			return SpecialType.UnknownType;
		}
	}
}
