using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.Documentation
{
	/// <summary>
	/// Provides ID strings for entities. (C# 4.0 spec, §A.3.1)
	/// ID strings are used to identify members in XML documentation files.
	/// </summary>
	// Token: 0x0200012F RID: 303
	public static class IdStringProvider
	{
		/// <summary>
		/// Gets the ID string (C# 4.0 spec, §A.3.1) for the specified entity.
		/// </summary>
		// Token: 0x06000A88 RID: 2696 RVA: 0x0001F1D4 File Offset: 0x0001E1D4
		public static string GetIdString(this IEntity entity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (entity.SymbolKind)
			{
			case SymbolKind.TypeDefinition:
				stringBuilder.Append("T:");
				IdStringProvider.AppendTypeName(stringBuilder, (ITypeDefinition)entity, false);
				return stringBuilder.ToString();
			case SymbolKind.Field:
				stringBuilder.Append("F:");
				break;
			case SymbolKind.Property:
			case SymbolKind.Indexer:
				stringBuilder.Append("P:");
				break;
			case SymbolKind.Event:
				stringBuilder.Append("E:");
				break;
			default:
				stringBuilder.Append("M:");
				break;
			}
			IMember member = (IMember)entity;
			IdStringProvider.AppendTypeName(stringBuilder, member.DeclaringType, false);
			stringBuilder.Append('.');
			if (member.IsExplicitInterfaceImplementation && member.Name.IndexOf('.') < 0 && member.ImplementedInterfaceMembers.Count == 1)
			{
				IdStringProvider.AppendTypeName(stringBuilder, member.ImplementedInterfaceMembers[0].DeclaringType, true);
				stringBuilder.Append('#');
			}
			stringBuilder.Append(member.Name.Replace('.', '#'));
			IMethod method = member as IMethod;
			if (method != null && method.TypeParameters.Count > 0)
			{
				stringBuilder.Append("``");
				stringBuilder.Append(method.TypeParameters.Count);
			}
			IParameterizedMember parameterizedMember = member as IParameterizedMember;
			if (parameterizedMember != null && parameterizedMember.Parameters.Count > 0)
			{
				stringBuilder.Append('(');
				IList<IParameter> parameters = parameterizedMember.Parameters;
				for (int i = 0; i < parameters.Count; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(',');
					}
					IdStringProvider.AppendTypeName(stringBuilder, parameters[i].Type, false);
				}
				stringBuilder.Append(')');
			}
			if (member.SymbolKind == SymbolKind.Operator && (member.Name == "op_Implicit" || member.Name == "op_Explicit"))
			{
				stringBuilder.Append('~');
				IdStringProvider.AppendTypeName(stringBuilder, member.ReturnType, false);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0001F3C8 File Offset: 0x0001E3C8
		public static string GetTypeName(IType type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			StringBuilder stringBuilder = new StringBuilder();
			IdStringProvider.AppendTypeName(stringBuilder, type, false);
			return stringBuilder.ToString();
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0001F3F8 File Offset: 0x0001E3F8
		private static void AppendTypeName(StringBuilder b, IType type, bool explicitInterfaceImpl)
		{
			switch (type.Kind)
			{
			case TypeKind.Dynamic:
				b.Append(explicitInterfaceImpl ? "System#Object" : "System.Object");
				return;
			case TypeKind.TypeParameter:
			{
				ITypeParameter typeParameter = (ITypeParameter)type;
				if (explicitInterfaceImpl)
				{
					b.Append(typeParameter.Name);
					return;
				}
				b.Append('`');
				if (typeParameter.OwnerType == SymbolKind.Method)
				{
					b.Append('`');
				}
				b.Append(typeParameter.Index);
				return;
			}
			case TypeKind.Array:
			{
				ArrayType arrayType = (ArrayType)type;
				IdStringProvider.AppendTypeName(b, arrayType.ElementType, explicitInterfaceImpl);
				b.Append('[');
				if (arrayType.Dimensions > 1)
				{
					for (int i = 0; i < arrayType.Dimensions; i++)
					{
						if (i > 0)
						{
							b.Append(explicitInterfaceImpl ? '@' : ',');
						}
						if (!explicitInterfaceImpl)
						{
							b.Append("0:");
						}
					}
				}
				b.Append(']');
				return;
			}
			case TypeKind.Pointer:
				IdStringProvider.AppendTypeName(b, ((PointerType)type).ElementType, explicitInterfaceImpl);
				b.Append('*');
				return;
			case TypeKind.ByReference:
				IdStringProvider.AppendTypeName(b, ((ByReferenceType)type).ElementType, explicitInterfaceImpl);
				b.Append('@');
				return;
			}
			IType declaringType = type.DeclaringType;
			if (declaringType != null)
			{
				IdStringProvider.AppendTypeName(b, declaringType, explicitInterfaceImpl);
				b.Append(explicitInterfaceImpl ? '#' : '.');
				b.Append(type.Name);
				IdStringProvider.AppendTypeParameters(b, type, declaringType.TypeParameterCount, explicitInterfaceImpl);
				return;
			}
			if (explicitInterfaceImpl)
			{
				b.Append(type.FullName.Replace('.', '#'));
			}
			else
			{
				b.Append(type.FullName);
			}
			IdStringProvider.AppendTypeParameters(b, type, 0, explicitInterfaceImpl);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0001F598 File Offset: 0x0001E598
		private static void AppendTypeParameters(StringBuilder b, IType type, int outerTypeParameterCount, bool explicitInterfaceImpl)
		{
			int num = type.TypeParameterCount - outerTypeParameterCount;
			if (num > 0)
			{
				ParameterizedType parameterizedType = type as ParameterizedType;
				if (parameterizedType != null)
				{
					b.Append('{');
					IList<IType> typeArguments = parameterizedType.TypeArguments;
					for (int i = outerTypeParameterCount; i < typeArguments.Count; i++)
					{
						if (i > outerTypeParameterCount)
						{
							b.Append(explicitInterfaceImpl ? '@' : ',');
						}
						IdStringProvider.AppendTypeName(b, typeArguments[i], explicitInterfaceImpl);
					}
					b.Append('}');
					return;
				}
				b.Append('`');
				b.Append(num);
			}
		}

		/// <summary>
		/// Parse the ID string into a member reference.
		/// </summary>
		/// <param name="memberIdString">The ID string representing the member (with "M:", "F:", "P:" or "E:" prefix).</param>
		/// <returns>A member reference that represents the ID string.</returns>
		/// <exception cref="T:ICSharpCode.NRefactory.TypeSystem.ReflectionNameParseException">The syntax of the ID string is invalid</exception>
		/// <remarks>
		/// The member reference will look in <see cref="P:ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext.CurrentAssembly" /> first,
		/// and if the member is not found there,
		/// it will look in all other assemblies of the compilation.
		/// </remarks>
		// Token: 0x06000A8C RID: 2700 RVA: 0x0001F61C File Offset: 0x0001E61C
		public static IMemberReference ParseMemberIdString(string memberIdString)
		{
			if (memberIdString == null)
			{
				throw new ArgumentNullException("memberIdString");
			}
			if (memberIdString.Length < 2 || memberIdString[1] != ':')
			{
				throw new ReflectionNameParseException(0, "Missing type tag");
			}
			char memberType = memberIdString[0];
			int num = memberIdString.IndexOf('(');
			if (num < 0)
			{
				num = memberIdString.LastIndexOf('~');
			}
			if (num < 0)
			{
				num = memberIdString.Length;
			}
			int num2 = memberIdString.LastIndexOf('.', num - 1);
			if (num2 < 0)
			{
				throw new ReflectionNameParseException(0, "Could not find '.' separating type name from member name");
			}
			string text = memberIdString.Substring(0, num2);
			int num3 = 2;
			ITypeReference declaringTypeReference = IdStringProvider.ParseTypeName(text, ref num3);
			if (num3 != text.Length)
			{
				throw new ReflectionNameParseException(num3, "Expected end of type name");
			}
			return new IdStringMemberReference(declaringTypeReference, memberType, memberIdString);
		}

		/// <summary>
		/// Parse the ID string type name into a type reference.
		/// </summary>
		/// <param name="typeName">The ID string representing the type (the "T:" prefix is optional).</param>
		/// <returns>A type reference that represents the ID string.</returns>
		/// <exception cref="T:ICSharpCode.NRefactory.TypeSystem.ReflectionNameParseException">The syntax of the ID string is invalid</exception>
		/// <remarks>
		/// <para>
		/// The type reference will look in <see cref="P:ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext.CurrentAssembly" /> first,
		/// and if the type is not found there,
		/// it will look in all other assemblies of the compilation.
		/// </para>
		/// <para>
		/// If the type is open (contains type parameters '`0' or '``0'),
		/// an <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext" /> with the appropriate CurrentTypeDefinition/CurrentMember is required
		/// to resolve the reference to the ITypeParameter.
		/// </para>
		/// </remarks>
		// Token: 0x06000A8D RID: 2701 RVA: 0x0001F6D4 File Offset: 0x0001E6D4
		public static ITypeReference ParseTypeName(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			int num = 0;
			if (typeName.StartsWith("T:", StringComparison.Ordinal))
			{
				num = 2;
			}
			ITypeReference result = IdStringProvider.ParseTypeName(typeName, ref num);
			if (num < typeName.Length)
			{
				throw new ReflectionNameParseException(num, "Expected end of type name");
			}
			return result;
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0001F720 File Offset: 0x0001E720
		private static bool IsIDStringSpecialCharacter(char c)
		{
			if (c <= '@')
			{
				switch (c)
				{
				case '(':
				case ')':
				case '*':
				case ',':
					break;
				case '+':
					return false;
				default:
					if (c != ':' && c != '@')
					{
						return false;
					}
					break;
				}
			}
			else
			{
				switch (c)
				{
				case '[':
				case ']':
					break;
				case '\\':
					return false;
				default:
					if (c != '`')
					{
						switch (c)
						{
						case '{':
						case '}':
							break;
						case '|':
							return false;
						default:
							return false;
						}
					}
					break;
				}
			}
			return true;
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0001F794 File Offset: 0x0001E794
		private static ITypeReference ParseTypeName(string typeName, ref int pos)
		{
			if (pos == typeName.Length)
			{
				throw new ReflectionNameParseException(pos, "Unexpected end");
			}
			ITypeReference typeReference;
			if (typeName[pos] == '`')
			{
				pos++;
				if (pos == typeName.Length)
				{
					throw new ReflectionNameParseException(pos, "Unexpected end");
				}
				if (typeName[pos] == '`')
				{
					pos++;
					int index = ReflectionHelper.ReadTypeParameterCount(typeName, ref pos);
					typeReference = TypeParameterReference.Create(SymbolKind.Method, index);
				}
				else
				{
					int index2 = ReflectionHelper.ReadTypeParameterCount(typeName, ref pos);
					typeReference = TypeParameterReference.Create(SymbolKind.TypeDefinition, index2);
				}
			}
			else
			{
				List<ITypeReference> list = new List<ITypeReference>();
				int num;
				string typeName2 = IdStringProvider.ReadTypeName(typeName, ref pos, true, out num, list);
				typeReference = new GetPotentiallyNestedClassTypeReference(typeName2, num);
				while (pos < typeName.Length && typeName[pos] == '.')
				{
					pos++;
					string name = IdStringProvider.ReadTypeName(typeName, ref pos, false, out num, list);
					typeReference = new NestedTypeReference(typeReference, name, num);
				}
				if (list.Count > 0)
				{
					typeReference = new ParameterizedTypeReference(typeReference, list);
				}
			}
			while (pos < typeName.Length)
			{
				char c = typeName[pos];
				if (c != '*')
				{
					if (c != '@')
					{
						if (c == '[')
						{
							int num2 = 1;
							do
							{
								pos++;
								if (pos == typeName.Length)
								{
									goto Block_10;
								}
								if (typeName[pos] == ',')
								{
									num2++;
								}
							}
							while (typeName[pos] != ']');
							typeReference = new ArrayTypeReference(typeReference, num2);
							goto IL_165;
							Block_10:
							throw new ReflectionNameParseException(pos, "Unexpected end");
						}
						return typeReference;
					}
					else
					{
						typeReference = new ByReferenceTypeReference(typeReference);
					}
				}
				else
				{
					typeReference = new PointerTypeReference(typeReference);
				}
				IL_165:
				pos++;
			}
			return typeReference;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0001F91C File Offset: 0x0001E91C
		private static string ReadTypeName(string typeName, ref int pos, bool allowDottedName, out int typeParameterCount, List<ITypeReference> typeArguments)
		{
			int num = pos;
			while (pos < typeName.Length && !IdStringProvider.IsIDStringSpecialCharacter(typeName[pos]) && (allowDottedName || typeName[pos] != '.'))
			{
				pos++;
			}
			if (pos == num)
			{
				throw new ReflectionNameParseException(pos, "Expected type name");
			}
			string result = typeName.Substring(num, pos - num);
			typeParameterCount = 0;
			if (pos < typeName.Length && typeName[pos] == '`')
			{
				pos++;
				typeParameterCount = ReflectionHelper.ReadTypeParameterCount(typeName, ref pos);
			}
			else if (pos < typeName.Length && typeName[pos] == '{')
			{
				typeArguments = new List<ITypeReference>();
				for (;;)
				{
					pos++;
					typeArguments.Add(IdStringProvider.ParseTypeName(typeName, ref pos));
					typeParameterCount++;
					if (pos == typeName.Length)
					{
						break;
					}
					if (typeName[pos] != ',')
					{
						goto Block_10;
					}
				}
				throw new ReflectionNameParseException(pos, "Unexpected end");
				Block_10:
				if (typeName[pos] != '}')
				{
					throw new ReflectionNameParseException(pos, "Expected '}'");
				}
				pos++;
			}
			return result;
		}

		/// <summary>
		/// Finds the entity in the given type resolve context.
		/// </summary>
		/// <param name="idString">ID string of the entity.</param>
		/// <param name="context">Type resolve context</param>
		/// <returns>Returns the entity, or null if it is not found.</returns>
		/// <exception cref="T:ICSharpCode.NRefactory.TypeSystem.ReflectionNameParseException">The syntax of the ID string is invalid</exception>
		// Token: 0x06000A91 RID: 2705 RVA: 0x0001FA20 File Offset: 0x0001EA20
		public static IEntity FindEntity(string idString, ITypeResolveContext context)
		{
			if (idString == null)
			{
				throw new ArgumentNullException("idString");
			}
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (idString.StartsWith("T:", StringComparison.Ordinal))
			{
				return IdStringProvider.ParseTypeName(idString.Substring(2)).Resolve(context).GetDefinition();
			}
			return IdStringProvider.ParseMemberIdString(idString).Resolve(context);
		}
	}
}
