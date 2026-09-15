using System;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// Given a reference to an accessor, returns the accessor's owner.
	/// </summary>
	[Serializable]
	internal sealed class AccessorOwnerMemberReference : IMemberReference, ISymbolReference
	{
		public AccessorOwnerMemberReference(IMemberReference accessorReference)
		{
			if (accessorReference == null)
			{
				throw new ArgumentNullException("accessorReference");
			}
			this.accessorReference = accessorReference;
		}

		public ITypeReference DeclaringTypeReference
		{
			get
			{
				return this.accessorReference.DeclaringTypeReference;
			}
		}

		public IMember Resolve(ITypeResolveContext context)
		{
			IMethod method = this.accessorReference.Resolve(context) as IMethod;
			if (method != null)
			{
				return method.AccessorOwner;
			}
			return null;
		}

		ISymbol ISymbolReference.Resolve(ITypeResolveContext context)
		{
			return ((IMemberReference)this).Resolve(context);
		}

		private readonly IMemberReference accessorReference;
	}
}
