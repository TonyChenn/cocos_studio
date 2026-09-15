using System;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Text;
using ICSharpCode.NRefactory.Semantics;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	[Serializable]
	public sealed class UnresolvedSecurityDeclarationBlob
	{
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

		public IList<IUnresolvedAttribute> UnresolvedAttributes
		{
			get
			{
				return this.unresolvedAttributes;
			}
		}

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

		private static readonly ITypeReference securityActionTypeReference = typeof(SecurityAction).ToTypeReference();

		private static readonly ITypeReference permissionSetAttributeTypeReference = typeof(PermissionSetAttribute).ToTypeReference();

		private readonly IConstantValue securityAction;

		private readonly byte[] blob;

		private readonly IList<IUnresolvedAttribute> unresolvedAttributes = new List<IUnresolvedAttribute>();
	}
}
