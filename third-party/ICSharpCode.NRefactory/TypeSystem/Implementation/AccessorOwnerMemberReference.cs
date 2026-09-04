using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Given a reference to an accessor, returns the accessor's owner.
	/// </summary>
	// Token: 0x020000A6 RID: 166
	[Serializable]
	internal sealed class AccessorOwnerMemberReference : IMemberReference, ISymbolReference
	{
		// Token: 0x06000581 RID: 1409 RVA: 0x0000D170 File Offset: 0x0000C170
		public AccessorOwnerMemberReference(IMemberReference accessorReference)
		{
			if (accessorReference == null)
			{
				throw new ArgumentNullException("accessorReference");
			}
			this.accessorReference = accessorReference;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000D18D File Offset: 0x0000C18D
		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.accessorReference.DeclaringTypeReference;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000D19C File Offset: 0x0000C19C
		public IMember Resolve(ITypeResolveContext context)
		{
			IMethod method = this.accessorReference.Resolve(context) as IMethod;
			if (method != null)
			{
				return method.AccessorOwner;
			}
			return null;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000D1C6 File Offset: 0x0000C1C6
		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return ((IMemberReference)this).Resolve(context);
		}

		// Token: 0x0400017F RID: 383
		private readonly IMemberReference accessorReference;
	}
}
