using System;
using System.Collections.Generic;
using System.Threading;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.Utils;

namespace ICSharpCode.NRefactory.TypeSystem.Implementation
{
	// Token: 0x0200007D RID: 125
	internal sealed class CecilResolvedAttribute : IAttribute
	{
		// Token: 0x060003F6 RID: 1014 RVA: 0x00009B3F File Offset: 0x00008B3F
		public CecilResolvedAttribute(ITypeResolveContext context, UnresolvedAttributeBlob unresolved)
		{
			this.context = context;
			this.blob = unresolved.blob;
			this.ctorParameterTypes = unresolved.ctorParameterTypes;
			this.attributeType = unresolved.attributeType.Resolve(context);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00009B78 File Offset: 0x00008B78
		public CecilResolvedAttribute(ITypeResolveContext context, IType attributeType)
		{
			this.context = context;
			this.attributeType = attributeType;
			this.ctorParameterTypes = EmptyList<ITypeReference>.Instance;
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060003F8 RID: 1016 RVA: 0x00009B99 File Offset: 0x00008B99
		DomRegion IAttribute.Region
		{
			get
			{
				return DomRegion.Empty;
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00009BA0 File Offset: 0x00008BA0
		public IType AttributeType
		{
			get
			{
				return this.attributeType;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00009BA8 File Offset: 0x00008BA8
		public IMethod Constructor
		{
			get
			{
				if (!this.constructorResolved)
				{
					this.constructor = this.ResolveConstructor();
					this.constructorResolved = true;
				}
				return this.constructor;
			}
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00009BF4 File Offset: 0x00008BF4
		private IMethod ResolveConstructor()
		{
			IList<IType> parameterTypes = this.ctorParameterTypes.Resolve(this.context);
			foreach (IMethod method in this.attributeType.GetConstructors((IUnresolvedMethod m) => m.Parameters.Count == parameterTypes.Count, GetMemberOptions.IgnoreInheritedMembers))
			{
				bool flag = true;
				for (int i = 0; i < parameterTypes.Count; i++)
				{
					if (!method.Parameters[i].Type.Equals(parameterTypes[i]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					return method;
				}
			}
			return null;
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00009CBC File Offset: 0x00008CBC
		public IList<ResolveResult> PositionalArguments
		{
			get
			{
				IList<ResolveResult> list = LazyInit.VolatileRead<IList<ResolveResult>>(ref this.positionalArguments);
				if (list != null)
				{
					return list;
				}
				this.DecodeBlob();
				return this.positionalArguments;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00009CE8 File Offset: 0x00008CE8
		public IList<KeyValuePair<IMember, ResolveResult>> NamedArguments
		{
			get
			{
				IList<KeyValuePair<IMember, ResolveResult>> list = LazyInit.VolatileRead<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments);
				if (list != null)
				{
					return list;
				}
				this.DecodeBlob();
				return this.namedArguments;
			}
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00009D12 File Offset: 0x00008D12
		public override string ToString()
		{
			return "[" + this.attributeType.ToString() + "(...)]";
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00009D30 File Offset: 0x00008D30
		private void DecodeBlob()
		{
			List<ResolveResult> value = new List<ResolveResult>();
			List<KeyValuePair<IMember, ResolveResult>> value2 = new List<KeyValuePair<IMember, ResolveResult>>();
			this.DecodeBlob(value, value2);
			Interlocked.CompareExchange<IList<ResolveResult>>(ref this.positionalArguments, value, null);
			Interlocked.CompareExchange<IList<KeyValuePair<IMember, ResolveResult>>>(ref this.namedArguments, value2, null);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00009D70 File Offset: 0x00008D70
		private void DecodeBlob(List<ResolveResult> positionalArguments, List<KeyValuePair<IMember, ResolveResult>> namedArguments)
		{
			if (this.blob == null)
			{
				return;
			}
			BlobReader blobReader = new BlobReader(this.blob, this.context.CurrentAssembly);
			if (blobReader.ReadUInt16() != 1)
			{
				return;
			}
			foreach (IType argType in this.ctorParameterTypes.Resolve(this.context))
			{
				bool flag;
				try
				{
					ResolveResult resolveResult = blobReader.ReadFixedArg(argType);
					positionalArguments.Add(resolveResult);
					flag = resolveResult.IsError;
				}
				catch (Exception)
				{
					flag = true;
				}
				if (flag)
				{
					while (positionalArguments.Count < this.ctorParameterTypes.Count)
					{
						positionalArguments.Add(ErrorResolveResult.UnknownError);
					}
					return;
				}
			}
			try
			{
				ushort num = blobReader.ReadUInt16();
				for (int i = 0; i < (int)num; i++)
				{
					KeyValuePair<IMember, ResolveResult> item = blobReader.ReadNamedArg(this.attributeType);
					if (item.Key != null)
					{
						namedArguments.Add(item);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x04000103 RID: 259
		private readonly ITypeResolveContext context;

		// Token: 0x04000104 RID: 260
		private readonly byte[] blob;

		// Token: 0x04000105 RID: 261
		private readonly IList<ITypeReference> ctorParameterTypes;

		// Token: 0x04000106 RID: 262
		private readonly IType attributeType;

		// Token: 0x04000107 RID: 263
		private IMethod constructor;

		// Token: 0x04000108 RID: 264
		private volatile bool constructorResolved;

		// Token: 0x04000109 RID: 265
		private IList<ResolveResult> positionalArguments;

		// Token: 0x0400010A RID: 266
		private IList<KeyValuePair<IMember, ResolveResult>> namedArguments;
	}
}
