using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	/// <summary>
	/// <c>IUnresolvedAttribute</c> implementation that loads the arguments from a binary blob.
	/// </summary>
	// Token: 0x0200007F RID: 127
	[Serializable]
	public sealed class UnresolvedAttributeBlob : IUnresolvedAttribute, ISupportsInterning
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x00009E88 File Offset: 0x00008E88
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

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00009EDA File Offset: 0x00008EDA
		DomRegion IUnresolvedAttribute.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00009EE1 File Offset: 0x00008EE1
		public IAttribute CreateResolvedAttribute(ITypeResolveContext context)
		{
			if (context.CurrentAssembly == null)
			{
				throw new InvalidOperationException("Cannot resolve CecilUnresolvedAttribute without a parent assembly");
			}
			return new CecilResolvedAttribute(context, this);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00009EFD File Offset: 0x00008EFD
		int ISupportsInterning.GetHashCodeForInterning()
		{
			return this.attributeType.GetHashCode() ^ this.ctorParameterTypes.GetHashCode() ^ BlobReader.GetBlobHashCode(this.blob);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00009F24 File Offset: 0x00008F24
		bool ISupportsInterning.EqualsForInterning(ISupportsInterning other)
		{
			UnresolvedAttributeBlob unresolvedAttributeBlob = other as UnresolvedAttributeBlob;
			return unresolvedAttributeBlob != null && this.attributeType == unresolvedAttributeBlob.attributeType && this.ctorParameterTypes == unresolvedAttributeBlob.ctorParameterTypes && BlobReader.BlobEquals(this.blob, unresolvedAttributeBlob.blob);
		}

		// Token: 0x0400010B RID: 267
		internal readonly ITypeReference attributeType;

		// Token: 0x0400010C RID: 268
		internal readonly IList<ITypeReference> ctorParameterTypes;

		// Token: 0x0400010D RID: 269
		internal readonly byte[] blob;
	}
}
