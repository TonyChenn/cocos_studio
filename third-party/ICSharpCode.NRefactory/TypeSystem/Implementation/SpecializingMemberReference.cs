using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	[Serializable]
	public sealed class SpecializingMemberReference : IMemberReference, ISymbolReference
	{
		public SpecializingMemberReference(IMemberReference memberDefinitionReference, IList<ITypeReference> classTypeArgumentReferences = null, IList<ITypeReference> methodTypeArgumentReferences = null)
		{
			if (memberDefinitionReference == null)
			{
				throw new ArgumentNullException("memberDefinitionReference");
			}
			this.memberDefinitionReference = memberDefinitionReference;
			this.classTypeArgumentReferences = classTypeArgumentReferences;
			this.methodTypeArgumentReferences = methodTypeArgumentReferences;
		}

		public IMember Resolve(ITypeResolveContext context)
		{
			IMember member = this.memberDefinitionReference.Resolve(context);
			if (member == null)
			{
				return null;
			}
			return member.Specialize(new TypeParameterSubstitution((this.classTypeArgumentReferences != null) ? this.classTypeArgumentReferences.Resolve(context) : null, (this.methodTypeArgumentReferences != null) ? this.methodTypeArgumentReferences.Resolve(context) : null));
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context);
		}

		public ITypeReference DeclaringTypeReference
		{
			get
			{
				if (this.classTypeArgumentReferences != null)
				{
					return new ParameterizedTypeReference(this.memberDefinitionReference.DeclaringTypeReference, this.classTypeArgumentReferences);
				}
				return this.memberDefinitionReference.DeclaringTypeReference;
			}
		}

		private IMemberReference memberDefinitionReference;

		private IList<ITypeReference> classTypeArgumentReferences;

		private IList<ITypeReference> methodTypeArgumentReferences;
	}
}
