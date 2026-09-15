using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// An unknown type where (part) of the name is known.
	/// </summary>
	[Serializable]
	public class UnknownType : AbstractType, ITypeReference
	{
		/// <summary>
		/// Creates a new unknown type.
		/// </summary>
		/// <param name="namespaceName">Namespace name, if known. Can be null if unknown.</param>
		/// <param name="name">Name of the type, must not be null.</param>
		/// <param name="typeParameterCount">Type parameter count, zero if unknown.</param>
		public UnknownType(string namespaceName, string name, int typeParameterCount = 0)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.namespaceKnown = (namespaceName != null);
			this.fullTypeName = new TopLevelTypeName(namespaceName ?? string.Empty, name, typeParameterCount);
		}

		/// <summary>
		/// Creates a new unknown type.
		/// </summary>
		/// <param name="fullTypeName">Full name of the unknown type.</param>
		public UnknownType(FullTypeName fullTypeName)
		{
			if (fullTypeName.Name == null)
			{
				this.namespaceKnown = false;
				this.fullTypeName = new TopLevelTypeName(string.Empty, "?", 0);
				return;
			}
			this.namespaceKnown = true;
			this.fullTypeName = fullTypeName;
		}

		public override TypeKind Kind
		{
			get
			{
				return TypeKind.Unknown;
			}
		}

		public override ITypeReference ToTypeReference()
		{
			return this;
		}

		IType ITypeReference.Resolve(ITypeResolveContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			return this;
		}

		public override string Name
		{
			get
			{
				return this.fullTypeName.Name;
			}
		}

		public override string Namespace
		{
			get
			{
				return this.fullTypeName.TopLevelTypeName.Namespace;
			}
		}

		public override string ReflectionName
		{
			get
			{
				if (!this.namespaceKnown)
				{
					return "?";
				}
				return this.fullTypeName.ReflectionName;
			}
		}

		public override int TypeParameterCount
		{
			get
			{
				return this.fullTypeName.TypeParameterCount;
			}
		}

		public override bool? IsReferenceType
		{
			get
			{
				return null;
			}
		}

		public override int GetHashCode()
		{
			return (this.namespaceKnown ? 812571 : 12651) ^ this.fullTypeName.GetHashCode();
		}

		public override bool Equals(IType other)
		{
			UnknownType unknownType = other as UnknownType;
			return unknownType != null && this.namespaceKnown == unknownType.namespaceKnown && this.fullTypeName == unknownType.fullTypeName;
		}

		public override string ToString()
		{
			return "[UnknownType " + this.fullTypeName.ReflectionName + "]";
		}

		private readonly bool namespaceKnown;

		private readonly FullTypeName fullTypeName;
	}
}
