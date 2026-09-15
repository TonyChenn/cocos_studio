using System;
using System.Linq;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Helper methods for COM.
	/// </summary>
	public static class ComHelper
	{
		private static bool IsComAttribute(IAttribute attribute, string name)
		{
			return attribute.AttributeType.Name == name && attribute.AttributeType.Namespace == "System.Runtime.InteropServices";
		}

		/// <summary>
		/// Gets whether the specified type is imported from COM.
		/// </summary>
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
