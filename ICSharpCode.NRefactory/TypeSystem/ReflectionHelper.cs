using System;
using System.Collections.Generic;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Static helper methods for reflection names.
	/// </summary>
	// Token: 0x020000FF RID: 255
	public static class ReflectionHelper
	{
		/// <summary>
		/// Retrieves the specified type in this compilation.
		/// Returns <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" /> if the type cannot be found in this compilation.
		/// </summary>
		/// <remarks>
		/// This method cannot be used with open types; all type parameters will be substituted
		/// with <see cref="F:ICSharpCode.NRefactory.TypeSystem.SpecialType.UnknownType" />.
		/// </remarks>
		// Token: 0x06000956 RID: 2390 RVA: 0x00018E4F File Offset: 0x00017E4F
		public static IType FindType(this ICompilation compilation, Type type)
		{
			return type.ToTypeReference().Resolve(compilation.TypeResolveContext);
		}

		/// <summary>
		/// Creates a reference to the specified type.
		/// </summary>
		/// <param name="type">The type to be converted.</param>
		/// <returns>Returns the type reference.</returns>
		/// <remarks>
		/// If the type is open (contains type parameters '`0' or '``0'),
		/// an <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext" /> with the appropriate CurrentTypeDefinition/CurrentMember is required
		/// to resolve the type reference.
		/// For closed types, the root type resolve context for the compilation is sufficient.
		/// </remarks>
		// Token: 0x06000957 RID: 2391 RVA: 0x00018E64 File Offset: 0x00017E64
		public static ITypeReference ToTypeReference(this Type type)
		{
			if (type == null)
			{
				return SpecialType.UnknownType;
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				ITypeReference typeReference = type.GetGenericTypeDefinition().ToTypeReference();
				Type[] genericArguments = type.GetGenericArguments();
				ITypeReference[] array = new ITypeReference[genericArguments.Length];
				bool flag = true;
				for (int i = 0; i < genericArguments.Length; i++)
				{
					array[i] = genericArguments[i].ToTypeReference();
					flag &= array[i].Equals(SpecialType.UnboundTypeArgument);
				}
				if (flag)
				{
					return typeReference;
				}
				return new ParameterizedTypeReference(typeReference, array);
			}
			else
			{
				if (type.IsArray)
				{
					return new ArrayTypeReference(type.GetElementType().ToTypeReference(), type.GetArrayRank());
				}
				if (type.IsPointer)
				{
					return new PointerTypeReference(type.GetElementType().ToTypeReference());
				}
				if (type.IsByRef)
				{
					return new ByReferenceTypeReference(type.GetElementType().ToTypeReference());
				}
				if (type.IsGenericParameter)
				{
					if (type.DeclaringMethod != null)
					{
						return TypeParameterReference.Create(SymbolKind.Method, type.GenericParameterPosition);
					}
					return TypeParameterReference.Create(SymbolKind.TypeDefinition, type.GenericParameterPosition);
				}
				else
				{
					if (!(type.DeclaringType != null))
					{
						IAssemblyReference assembly = new DefaultAssemblyReference(type.Assembly.FullName);
						int typeParameterCount;
						string name = ReflectionHelper.SplitTypeParameterCountFromReflectionName(type.Name, out typeParameterCount);
						return new GetClassTypeReference(assembly, type.Namespace, name, typeParameterCount);
					}
					if (type == typeof(ReflectionHelper.Dynamic))
					{
						return SpecialType.Dynamic;
					}
					if (type == typeof(ReflectionHelper.Null))
					{
						return SpecialType.NullType;
					}
					if (type == typeof(ReflectionHelper.UnboundTypeArgument))
					{
						return SpecialType.UnboundTypeArgument;
					}
					ITypeReference declaringTypeRef = type.DeclaringType.ToTypeReference();
					int additionalTypeParameterCount;
					string name2 = ReflectionHelper.SplitTypeParameterCountFromReflectionName(type.Name, out additionalTypeParameterCount);
					return new NestedTypeReference(declaringTypeRef, name2, additionalTypeParameterCount);
				}
			}
		}

		/// <summary>
		/// Removes the ` with type parameter count from the reflection name.
		/// </summary>
		/// <remarks>Do not use this method with the full name of inner classes.</remarks>
		// Token: 0x06000958 RID: 2392 RVA: 0x0001901C File Offset: 0x0001801C
		public static string SplitTypeParameterCountFromReflectionName(string reflectionName)
		{
			int num = reflectionName.LastIndexOf('`');
			if (num < 0)
			{
				return reflectionName;
			}
			return reflectionName.Substring(0, num);
		}

		/// <summary>
		/// Removes the ` with type parameter count from the reflection name.
		/// </summary>
		/// <remarks>Do not use this method with the full name of inner classes.</remarks>
		// Token: 0x06000959 RID: 2393 RVA: 0x00019040 File Offset: 0x00018040
		public static string SplitTypeParameterCountFromReflectionName(string reflectionName, out int typeParameterCount)
		{
			int num = reflectionName.LastIndexOf('`');
			if (num < 0)
			{
				typeParameterCount = 0;
				return reflectionName;
			}
			string s = reflectionName.Substring(num + 1);
			if (int.TryParse(s, out typeParameterCount))
			{
				return reflectionName.Substring(0, num);
			}
			return reflectionName;
		}

		/// <summary>
		/// Retrieves a built-in type using the specified type code.
		/// </summary>
		// Token: 0x0600095A RID: 2394 RVA: 0x0001907C File Offset: 0x0001807C
		public static IType FindType(this ICompilation compilation, TypeCode typeCode)
		{
			return compilation.FindType((KnownTypeCode)typeCode);
		}

		/// <summary>
		/// Creates a reference to the specified type.
		/// </summary>
		/// <param name="typeCode">The type to be converted.</param>
		/// <returns>Returns the type reference.</returns>
		// Token: 0x0600095B RID: 2395 RVA: 0x00019085 File Offset: 0x00018085
		public static ITypeReference ToTypeReference(this TypeCode typeCode)
		{
			return KnownTypeReference.Get((KnownTypeCode)typeCode);
		}

		/// <summary>
		/// Gets the type code for the specified type, or TypeCode.Empty if none of the other type codes match.
		/// </summary>
		// Token: 0x0600095C RID: 2396 RVA: 0x00019090 File Offset: 0x00018090
		public static TypeCode GetTypeCode(IType type)
		{
			ITypeDefinition typeDefinition = type as ITypeDefinition;
			if (typeDefinition == null)
			{
				return TypeCode.Empty;
			}
			KnownTypeCode knownTypeCode = typeDefinition.KnownTypeCode;
			if (knownTypeCode <= KnownTypeCode.String && knownTypeCode != KnownTypeCode.Void)
			{
				return (TypeCode)knownTypeCode;
			}
			return TypeCode.Empty;
		}

		/// <summary>
		/// Parses a reflection name into a type reference.
		/// </summary>
		/// <param name="reflectionTypeName">The reflection name of the type.</param>
		/// <returns>A type reference that represents the reflection name.</returns>
		/// <exception cref="T:ICSharpCode.NRefactory.TypeSystem.ReflectionNameParseException">The syntax of the reflection type name is invalid</exception>
		/// <remarks>
		/// If the type is open (contains type parameters '`0' or '``0'),
		/// an <see cref="T:ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext" /> with the appropriate CurrentTypeDefinition/CurrentMember is required
		/// to resolve the reference to the ITypeParameter.
		/// For looking up closed, assembly qualified type names, the root type resolve context for the compilation
		/// is sufficient.
		/// When looking up a type name that isn't assembly qualified, the type reference will look in
		/// <see cref="P:ICSharpCode.NRefactory.TypeSystem.ITypeResolveContext.CurrentAssembly" /> first, and if the type is not found there,
		/// it will look in all other assemblies of the compilation.
		/// </remarks>
		/// <seealso cref="M:ICSharpCode.NRefactory.TypeSystem.FullTypeName.#ctor(System.String)" />
		// Token: 0x0600095D RID: 2397 RVA: 0x000190C0 File Offset: 0x000180C0
		public static ITypeReference ParseReflectionName(string reflectionTypeName)
		{
			if (reflectionTypeName == null)
			{
				throw new ArgumentNullException("reflectionTypeName");
			}
			int num = 0;
			ITypeReference result = ReflectionHelper.ParseReflectionName(reflectionTypeName, ref num);
			if (num < reflectionTypeName.Length)
			{
				throw new ReflectionNameParseException(num, "Expected end of type name");
			}
			return result;
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x000190FC File Offset: 0x000180FC
		private static bool IsReflectionNameSpecialCharacter(char c)
		{
			switch (c)
			{
			case '&':
			case '*':
			case '+':
			case ',':
				break;
			case '\'':
			case '(':
			case ')':
				return false;
			default:
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
						return false;
					}
					break;
				}
				break;
			}
			return true;
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00019150 File Offset: 0x00018150
		private static ITypeReference ParseReflectionName(string reflectionTypeName, ref int pos)
		{
			if (pos == reflectionTypeName.Length)
			{
				throw new ReflectionNameParseException(pos, "Unexpected end");
			}
			ITypeReference typeReference;
			if (reflectionTypeName[pos] == '`')
			{
				pos++;
				if (pos == reflectionTypeName.Length)
				{
					throw new ReflectionNameParseException(pos, "Unexpected end");
				}
				if (reflectionTypeName[pos] == '`')
				{
					pos++;
					int index = ReflectionHelper.ReadTypeParameterCount(reflectionTypeName, ref pos);
					typeReference = TypeParameterReference.Create(SymbolKind.Method, index);
				}
				else
				{
					int index2 = ReflectionHelper.ReadTypeParameterCount(reflectionTypeName, ref pos);
					typeReference = TypeParameterReference.Create(SymbolKind.TypeDefinition, index2);
				}
			}
			else
			{
				int tpc;
				string typeName = ReflectionHelper.ReadTypeName(reflectionTypeName, ref pos, out tpc);
				string assemblyName = ReflectionHelper.SkipAheadAndReadAssemblyName(reflectionTypeName, pos);
				typeReference = ReflectionHelper.CreateGetClassTypeReference(assemblyName, typeName, tpc);
			}
			while (pos < reflectionTypeName.Length)
			{
				char c = reflectionTypeName[pos++];
				switch (c)
				{
				case '&':
					typeReference = new ByReferenceTypeReference(typeReference);
					continue;
				case '\'':
				case '(':
				case ')':
					break;
				case '*':
					typeReference = new PointerTypeReference(typeReference);
					continue;
				case '+':
				{
					int additionalTypeParameterCount;
					string name = ReflectionHelper.ReadTypeName(reflectionTypeName, ref pos, out additionalTypeParameterCount);
					typeReference = new NestedTypeReference(typeReference, name, additionalTypeParameterCount);
					continue;
				}
				case ',':
					while (pos < reflectionTypeName.Length)
					{
						if (reflectionTypeName[pos] == ']')
						{
							break;
						}
						pos++;
					}
					continue;
				default:
					if (c == '[')
					{
						if (pos == reflectionTypeName.Length)
						{
							throw new ReflectionNameParseException(pos, "Unexpected end");
						}
						if (reflectionTypeName[pos] == '[')
						{
							List<ITypeReference> list = new List<ITypeReference>();
							pos++;
							list.Add(ReflectionHelper.ParseReflectionName(reflectionTypeName, ref pos));
							if (pos >= reflectionTypeName.Length || reflectionTypeName[pos] != ']')
							{
								throw new ReflectionNameParseException(pos, "Expected end of type argument");
							}
							pos++;
							while (pos < reflectionTypeName.Length && reflectionTypeName[pos] == ',')
							{
								pos++;
								if (pos >= reflectionTypeName.Length || reflectionTypeName[pos] != '[')
								{
									throw new ReflectionNameParseException(pos, "Expected another type argument");
								}
								pos++;
								list.Add(ReflectionHelper.ParseReflectionName(reflectionTypeName, ref pos));
								if (pos >= reflectionTypeName.Length || reflectionTypeName[pos] != ']')
								{
									throw new ReflectionNameParseException(pos, "Expected end of type argument");
								}
								pos++;
							}
							if (pos < reflectionTypeName.Length && reflectionTypeName[pos] == ']')
							{
								pos++;
								typeReference = new ParameterizedTypeReference(typeReference, list);
								continue;
							}
							throw new ReflectionNameParseException(pos, "Expected end of generic type");
						}
						else
						{
							int num = 1;
							while (pos < reflectionTypeName.Length && reflectionTypeName[pos] == ',')
							{
								num++;
								pos++;
							}
							if (pos < reflectionTypeName.Length && reflectionTypeName[pos] == ']')
							{
								pos++;
								typeReference = new ArrayTypeReference(typeReference, num);
								continue;
							}
							throw new ReflectionNameParseException(pos, "Invalid array modifier");
						}
					}
					break;
				}
				pos--;
				if (reflectionTypeName[pos] == ']')
				{
					return typeReference;
				}
				throw new ReflectionNameParseException(pos, "Unexpected character: '" + reflectionTypeName[pos] + "'");
			}
			return typeReference;
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00019458 File Offset: 0x00018458
		private static ITypeReference CreateGetClassTypeReference(string assemblyName, string typeName, int tpc)
		{
			IAssemblyReference assembly;
			if (assemblyName != null)
			{
				assembly = new DefaultAssemblyReference(assemblyName);
			}
			else
			{
				assembly = null;
			}
			int num = typeName.LastIndexOf('.');
			if (num < 0)
			{
				return new GetClassTypeReference(assembly, string.Empty, typeName, tpc);
			}
			return new GetClassTypeReference(assembly, typeName.Substring(0, num), typeName.Substring(num + 1), tpc);
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x000194A8 File Offset: 0x000184A8
		private static string SkipAheadAndReadAssemblyName(string reflectionTypeName, int pos)
		{
			int num = 0;
			while (pos < reflectionTypeName.Length)
			{
				char c = reflectionTypeName[pos++];
				if (c != ',')
				{
					switch (c)
					{
					case '[':
						num++;
						break;
					case ']':
						if (num == 0)
						{
							return null;
						}
						num--;
						break;
					}
				}
				else if (num == 0)
				{
					while (pos < reflectionTypeName.Length && reflectionTypeName[pos] == ' ')
					{
						pos++;
					}
					int num2 = pos;
					while (num2 < reflectionTypeName.Length && reflectionTypeName[num2] != ']')
					{
						num2++;
					}
					return reflectionTypeName.Substring(pos, num2 - pos);
				}
			}
			return null;
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00019548 File Offset: 0x00018548
		private static string ReadTypeName(string reflectionTypeName, ref int pos, out int tpc)
		{
			int num = pos;
			while (pos < reflectionTypeName.Length && !ReflectionHelper.IsReflectionNameSpecialCharacter(reflectionTypeName[pos]))
			{
				pos++;
			}
			if (pos == num)
			{
				throw new ReflectionNameParseException(pos, "Expected type name");
			}
			string result = reflectionTypeName.Substring(num, pos - num);
			if (pos < reflectionTypeName.Length && reflectionTypeName[pos] == '`')
			{
				pos++;
				tpc = ReflectionHelper.ReadTypeParameterCount(reflectionTypeName, ref pos);
			}
			else
			{
				tpc = 0;
			}
			return result;
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x000195C4 File Offset: 0x000185C4
		internal static int ReadTypeParameterCount(string reflectionTypeName, ref int pos)
		{
			int num = pos;
			while (pos < reflectionTypeName.Length)
			{
				char c = reflectionTypeName[pos];
				if (c < '0' || c > '9')
				{
					break;
				}
				pos++;
			}
			int result;
			if (!int.TryParse(reflectionTypeName.Substring(num, pos - num), out result))
			{
				throw new ReflectionNameParseException(pos, "Expected type parameter count");
			}
			return result;
		}

		/// <summary>
		/// A reflection class used to represent <c>null</c>.
		/// </summary>
		// Token: 0x02000100 RID: 256
		public sealed class Null
		{
		}

		/// <summary>
		/// A reflection class used to represent <c>dynamic</c>.
		/// </summary>
		// Token: 0x02000101 RID: 257
		public sealed class Dynamic
		{
		}

		/// <summary>
		/// A reflection class used to represent an unbound type argument.
		/// </summary>
		// Token: 0x02000102 RID: 258
		public sealed class UnboundTypeArgument
		{
		}
	}
}
