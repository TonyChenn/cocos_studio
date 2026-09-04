using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x020000E4 RID: 228
	[Serializable]
	public sealed class SpecializingMemberReference : IMemberReference, ISymbolReference
	{
		// Token: 0x06000887 RID: 2183 RVA: 0x000165E4 File Offset: 0x000155E4
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

		// Token: 0x06000888 RID: 2184 RVA: 0x00016610 File Offset: 0x00015610
		public IMember Resolve(ITypeResolveContext context)
		{
			IMember member = this.memberDefinitionReference.Resolve(context);
			if (member == null)
			{
				return null;
			}
			return member.Specialize(new TypeParameterSubstitution((this.classTypeArgumentReferences != null) ? this.classTypeArgumentReferences.Resolve(context) : null, (this.methodTypeArgumentReferences != null) ? this.methodTypeArgumentReferences.Resolve(context) : null));
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00016668 File Offset: 0x00015668
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return this.Resolve(context);
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00016671 File Offset: 0x00015671
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

		// Token: 0x0400026F RID: 623
		private IMemberReference memberDefinitionReference;

		// Token: 0x04000270 RID: 624
		private IList<ITypeReference> classTypeArgumentReferences;

		// Token: 0x04000271 RID: 625
		private IList<ITypeReference> methodTypeArgumentReferences;
	}
}
