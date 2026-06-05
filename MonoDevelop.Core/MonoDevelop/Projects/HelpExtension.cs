using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.TypeSystem;

namespace MonoDevelop.Projects
{
	// Token: 0x02000189 RID: 393
	public static class HelpExtension
	{
		// Token: 0x06000F51 RID: 3921 RVA: 0x000396F8 File Offset: 0x000378F8
		private static void AppendTypeReference(StringBuilder result, ITypeReference type)
		{
			if (type is ArrayTypeReference)
			{
				ArrayTypeReference arrayTypeReference = (ArrayTypeReference)type;
				HelpExtension.AppendTypeReference(result, arrayTypeReference.ElementType);
				result.Append("[");
				result.Append(new string(',', arrayTypeReference.Dimensions));
				result.Append("]");
				return;
			}
			if (type is PointerTypeReference)
			{
				PointerTypeReference pointerTypeReference = (PointerTypeReference)type;
				HelpExtension.AppendTypeReference(result, pointerTypeReference.ElementType);
				result.Append("*");
				return;
			}
			if (type is IType)
			{
				result.Append(((IType)type).FullName);
			}
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x00039790 File Offset: 0x00037990
		private static void AppendHelpParameterList(StringBuilder result, IList<IParameter> parameters)
		{
			result.Append('(');
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					if (i > 0)
					{
						result.Append(',');
					}
					IParameter parameter = parameters[i];
					if (parameter != null)
					{
						if (parameter.IsRef || parameter.IsOut)
						{
							result.Append("&");
						}
						HelpExtension.AppendTypeReference(result, parameter.Type.ToTypeReference());
					}
				}
			}
			result.Append(')');
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x00039808 File Offset: 0x00037A08
		private static void AppendHelpParameterList(StringBuilder result, IList<IUnresolvedParameter> parameters)
		{
			result.Append('(');
			if (parameters != null)
			{
				for (int i = 0; i < parameters.Count; i++)
				{
					if (i > 0)
					{
						result.Append(',');
					}
					IUnresolvedParameter unresolvedParameter = parameters[i];
					if (unresolvedParameter != null)
					{
						if (unresolvedParameter.IsRef || unresolvedParameter.IsOut)
						{
							result.Append("&");
						}
						HelpExtension.AppendTypeReference(result, unresolvedParameter.Type);
					}
				}
			}
			result.Append(')');
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003987C File Offset: 0x00037A7C
		private static XmlNode FindMatch(IMethod method, XmlNodeList nodes)
		{
			foreach (object obj in nodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("Parameters/*");
				if (method.Parameters.Count == 0 && xmlNodeList.Count == 0)
				{
					return xmlNode;
				}
				if (method.Parameters.Count == xmlNodeList.Count)
				{
					return xmlNode;
				}
			}
			return null;
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0003990C File Offset: 0x00037B0C
		public static XmlNode GetMonodocDocumentation(this IEntity member)
		{
			if (member.SymbolKind == SymbolKind.TypeDefinition)
			{
				XmlDocument xmlDocument = (HelpService.HelpTree != null) ? HelpService.HelpTree.GetHelpXml(member.GetIdString()) : null;
				if (xmlDocument == null)
				{
					return null;
				}
				return xmlDocument.SelectSingleNode("/Type/Docs");
			}
			else
			{
				XmlDocument xmlDocument2 = (HelpService.HelpTree != null && member.DeclaringTypeDefinition != null) ? HelpService.HelpTree.GetHelpXml(member.DeclaringTypeDefinition.GetIdString()) : null;
				if (xmlDocument2 == null)
				{
					return null;
				}
				switch (member.SymbolKind)
				{
				case SymbolKind.Method:
				{
					XmlNodeList xmlNodeList = xmlDocument2.SelectNodes("/Type/Members/Member[@MemberName='" + member.Name + "']");
					XmlNode xmlNode = (xmlNodeList.Count == 1) ? xmlNodeList[0] : HelpExtension.FindMatch((IMethod)member, xmlNodeList);
					if (xmlNode != null)
					{
						return xmlNode.SelectSingleNode("Docs");
					}
					return null;
				}
				case SymbolKind.Constructor:
				{
					XmlNodeList xmlNodeList2 = xmlDocument2.SelectNodes("/Type/Members/Member[@MemberName='.ctor']");
					XmlNode xmlNode2 = (xmlNodeList2.Count == 1) ? xmlNodeList2[0] : HelpExtension.FindMatch((IMethod)member, xmlNodeList2);
					if (xmlNode2 != null)
					{
						return xmlNode2.SelectSingleNode("Docs");
					}
					return null;
				}
				}
				return xmlDocument2.SelectSingleNode("/Type/Members/Member[@MemberName='" + member.Name + "']/Docs");
			}
		}
	}
}
