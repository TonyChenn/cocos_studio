using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// <c>IUnresolvedAttribute</c> implementation that loads the arguments from a binary blob.
	/// </summary>
	[Serializable]
	public sealed class UnresolvedAttributeBlob : IUnresolvedAttribute, ISupportsInterning
	{
		public UnresolvedAttributeBlob(ITypeReference attributeType, IList<ITypeReference> ctorParameterTypes, byte[] blob)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (ctorParameterTypes == null)
			{
				throw new ArgumentNullException("ctorParameterTypes");
			}
			if (blob == null)
			{
				throw new ArgumentNullException("blob");
			}
			this.attributeType = attributeType;
			this.ctorParameterTypes = ctorParameterTypes;
			this.blob = blob;
		}

		DomRegion IUnresolvedAttribute.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		public IAttribute CreateResolvedAttribute(ITypeResolveContext context)
		{
			if (context.CurrentAssembly == null)
			{
				throw new InvalidOperationException("Cannot resolve CecilUnresolvedAttribute without a parent assembly");
			}
			return new CecilResolvedAttribute(context, this);
		}

		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.attributeType.GetHashCode() ^ this.ctorParameterTypes.GetHashCode() ^ BlobReader.GetBlobHashCode(this.blob);
		}

		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			UnresolvedAttributeBlob unresolvedAttributeBlob = other as UnresolvedAttributeBlob;
			return unresolvedAttributeBlob != null && this.attributeType == unresolvedAttributeBlob.attributeType && this.ctorParameterTypes == unresolvedAttributeBlob.ctorParameterTypes && BlobReader.BlobEquals(this.blob, unresolvedAttributeBlob.blob);
		}

		internal readonly ITypeReference attributeType;

		internal readonly IList<ITypeReference> ctorParameterTypes;

		internal readonly byte[] blob;
	}
}
