using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x02000080 RID: 128
	[Serializable]
	public sealed class UnresolvedSecurityDeclarationBlob
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x00009F6C File Offset: 0x00008F6C
		public UnresolvedSecurityDeclarationBlob(int securityAction, byte[] blob)
		{
			BlobReader blobReader = new BlobReader(blob, null);
			this.securityAction = new SimpleConstantValue(UnresolvedSecurityDeclarationBlob.securityActionTypeReference, securityAction);
			this.blob = blob;
			if (blobReader.ReadByte() == 46)
			{
				uint num = blobReader.ReadCompressedUInt32();
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					this.unresolvedAttributes.Add(new UnresolvedSecurityAttribute(this, (int)num2));
				}
				return;
			}
			DefaultUnresolvedAttribute defaultUnresolvedAttribute = new DefaultUnresolvedAttribute(UnresolvedSecurityDeclarationBlob.permissionSetAttributeTypeReference);
			defaultUnresolvedAttribute.ConstructorParameterTypes.Add(UnresolvedSecurityDeclarationBlob.securityActionTypeReference);
			defaultUnresolvedAttribute.PositionalArguments.Add(this.securityAction);
			string @string = Encoding.Unicode.GetString(blob);
			defaultUnresolvedAttribute.AddNamedPropertyArgument("XML", new SimpleConstantValue(KnownTypeReference.String, @string));
			this.unresolvedAttributes.Add(defaultUnresolvedAttribute);
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0000A03B File Offset: 0x0000903B
		public IList<IUnresolvedAttribute> UnresolvedAttributes
		{
			get
			{
				return this.unresolvedAttributes;
			}
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0000A044 File Offset: 0x00009044
		public IList<IAttribute> Resolve(IAssembly currentAssembly)
		{
			ITypeResolveContext context = new SimpleTypeResolveContext(currentAssembly);
			BlobReader blobReader = new BlobReader(this.blob, currentAssembly);
			if (blobReader.ReadByte() != 46)
			{
				throw new InvalidOperationException();
			}
			ResolveResult securityActionRR = this.securityAction.Resolve(context);
			uint num = blobReader.ReadCompressedUInt32();
			IAttribute[] array = new IAttribute[num];
			try
			{
				this.ReadSecurityBlob(blobReader, array, context, securityActionRR);
			}
			catch (NotSupportedException)
			{
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == null)
				{
					array[i] = new CecilResolvedAttribute(context, SpecialType.UnknownType);
				}
			}
			return array;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0000A0E0 File Offset: 0x000090E0
		private void ReadSecurityBlob(BlobReader reader, IAttribute[] attributes, ITypeResolveContext context, ResolveResult securityActionRR)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				string reflectionTypeName = reader.ReadSerString();
				ITypeReference typeReference = ReflectionHelper.ParseReflectionName(reflectionTypeName);
				IType attributeType = typeReference.Resolve(context);
				reader.ReadCompressedUInt32();
				uint num = reader.ReadCompressedUInt32();
				List<KeyValuePair<IMember, ResolveResult>> list = new List<KeyValuePair<IMember, ResolveResult>>((int)num);
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					KeyValuePair<IMember, ResolveResult> item = reader.ReadNamedArg(attributeType);
					if (item.Key != null)
					{
						list.Add(item);
					}
				}
				attributes[i] = new DefaultAttribute(attributeType, new ResolveResult[]
				{
					securityActionRR
				}, list, default(DomRegion));
			}
		}

		// Token: 0x0400010E RID: 270
		private static readonly ITypeReference securityActionTypeReference = typeof(SecurityAction).ToTypeReference();

		// Token: 0x0400010F RID: 271
		private static readonly ITypeReference permissionSetAttributeTypeReference = typeof(PermissionSetAttribute).ToTypeReference();

		// Token: 0x04000110 RID: 272
		private readonly IConstantValue securityAction;

		// Token: 0x04000111 RID: 273
		private readonly byte[] blob;

		// Token: 0x04000112 RID: 274
		private readonly IList<IUnresolvedAttribute> unresolvedAttributes = new List<IUnresolvedAttribute>();
	}
}
