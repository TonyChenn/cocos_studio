using System;
using System.Collections.Generic;
using System.Text;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Substitutes class and method type parameters.
	/// </summary>
	public class TypeParameterSubstitution : TypeVisitor
	{
		/// <summary>
		/// Creates a new type parameter substitution.
		/// </summary>
		/// <param name="classTypeArguments">
		/// The type arguments to substitute for class type parameters.
		/// Pass <c>null</c> to keep class type parameters unmodified.
		/// </param>
		/// <param name="methodTypeArguments">
		/// The type arguments to substitute for method type parameters.
		/// Pass <c>null</c> to keep method type parameters unmodified.
		/// </param>
		public TypeParameterSubstitution(IList<IType> classTypeArguments, IList<IType> methodTypeArguments)
		{
			this.classTypeArguments = classTypeArguments;
			this.methodTypeArguments = methodTypeArguments;
		}

		/// <summary>
		/// Gets the list of class type arguments.
		/// Returns <c>null</c> if this substitution keeps class type parameters unmodified.
		/// </summary>
		public IList<IType> ClassTypeArguments
		{
			get
			{
				return this.classTypeArguments;
			}
		}

		/// <summary>
		/// Gets the list of method type arguments.
		/// Returns <c>null</c> if this substitution keeps method type parameters unmodified.
		/// </summary>
		public IList<IType> MethodTypeArguments
		{
			get
			{
				return this.methodTypeArguments;
			}
		}

		/// <summary>
		/// Computes a single TypeParameterSubstitution so that for all types <c>t</c>:
		/// <c>t.AcceptVisitor(Compose(g, f)) equals t.AcceptVisitor(f).AcceptVisitor(g)</c>
		/// </summary>
		/// <remarks>If you consider type parameter substitution to be a function, this is function composition.</remarks>
		public static TypeParameterSubstitution Compose(TypeParameterSubstitution g, TypeParameterSubstitution f)
		{
			if (g == null)
			{
				return f;
			}
			if (f == null || (f.classTypeArguments == null && f.methodTypeArguments == null))
			{
				return g;
			}
			IList<IType> list = (f.classTypeArguments != null) ? TypeParameterSubstitution.GetComposedTypeArguments(f.classTypeArguments, g) : g.classTypeArguments;
			IList<IType> list2 = (f.methodTypeArguments != null) ? TypeParameterSubstitution.GetComposedTypeArguments(f.methodTypeArguments, g) : g.methodTypeArguments;
			return new TypeParameterSubstitution(list, list2);
		}

		private static IList<IType> GetComposedTypeArguments(IList<IType> input, TypeParameterSubstitution substitution)
		{
			IType[] array = new IType[input.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = input[i].AcceptVisitor(substitution);
			}
			return array;
		}

		public override bool Equals(object obj)
		{
			TypeParameterSubstitution typeParameterSubstitution = obj as TypeParameterSubstitution;
			return typeParameterSubstitution != null && TypeParameterSubstitution.TypeListEquals(this.classTypeArguments, typeParameterSubstitution.classTypeArguments) && TypeParameterSubstitution.TypeListEquals(this.methodTypeArguments, typeParameterSubstitution.methodTypeArguments);
		}

		public override int GetHashCode()
		{
			return 1124131 * TypeParameterSubstitution.TypeListHashCode(this.classTypeArguments) + 1821779 * TypeParameterSubstitution.TypeListHashCode(this.methodTypeArguments);
		}

		private static bool TypeListEquals(IList<IType> a, IList<IType> b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			if (a.Count != b.Count)
			{
				return false;
			}
			for (int i = 0; i < a.Count; i++)
			{
				if (!a[i].Equals(b[i]))
				{
					return false;
				}
			}
			return true;
		}

		private static int TypeListHashCode(IList<IType> obj)
		{
			if (obj == null)
			{
				return 0;
			}
			int num = 1;
			foreach (IType type in obj)
			{
				num *= 27;
				num += type.GetHashCode();
			}
			return num;
		}

		public override IType VisitTypeParameter(ITypeParameter type)
		{
			int index = type.Index;
			if (this.classTypeArguments != null && type.OwnerType == SymbolKind.TypeDefinition)
			{
				if (index >= 0 && index < this.classTypeArguments.Count)
				{
					return this.classTypeArguments[index];
				}
				return SpecialType.UnknownType;
			}
			else
			{
				if (this.methodTypeArguments == null || type.OwnerType != SymbolKind.Method)
				{
					return base.VisitTypeParameter(type);
				}
				if (index >= 0 && index < this.methodTypeArguments.Count)
				{
					return this.methodTypeArguments[index];
				}
				return SpecialType.UnknownType;
			}
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			bool flag = true;
			if (this.classTypeArguments != null)
			{
				for (int i = 0; i < this.classTypeArguments.Count; i++)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append('`');
					stringBuilder.Append(i);
					stringBuilder.Append(" -> ");
					stringBuilder.Append(this.classTypeArguments[i].ReflectionName);
				}
			}
			if (this.methodTypeArguments != null)
			{
				for (int j = 0; j < this.methodTypeArguments.Count; j++)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append("``");
					stringBuilder.Append(j);
					stringBuilder.Append(" -> ");
					stringBuilder.Append(this.methodTypeArguments[j].ReflectionName);
				}
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		/// <summary>
		/// The identity function.
		/// </summary>
		public static readonly TypeParameterSubstitution Identity = new TypeParameterSubstitution(null, null);

		private readonly IList<IType> classTypeArguments;

		private readonly IList<IType> methodTypeArguments;
	}
}
