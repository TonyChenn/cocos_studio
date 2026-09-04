using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Default ITypeResolveContext implementation.
	/// </summary>
	// Token: 0x020000F9 RID: 249
	public class SimpleTypeResolveContext : ITypeResolveContext, ICompilationProvider
	{
		// Token: 0x0600092D RID: 2349 RVA: 0x00018986 File Offset: 0x00017986
		public SimpleTypeResolveContext(ICompilation compilation)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			this.compilation = compilation;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000189A3 File Offset: 0x000179A3
		public SimpleTypeResolveContext(IAssembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.compilation = assembly.Compilation;
			this.currentAssembly = assembly;
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x000189CC File Offset: 0x000179CC
		public SimpleTypeResolveContext(IEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			this.compilation = entity.Compilation;
			this.currentAssembly = entity.ParentAssembly;
			this.currentTypeDefinition = ((entity as ITypeDefinition) ?? entity.DeclaringTypeDefinition);
			this.currentMember = (entity as IMember);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00018A27 File Offset: 0x00017A27
		private SimpleTypeResolveContext(ICompilation compilation, IAssembly currentAssembly, ITypeDefinition currentTypeDefinition, IMember currentMember)
		{
			this.compilation = compilation;
			this.currentAssembly = currentAssembly;
			this.currentTypeDefinition = currentTypeDefinition;
			this.currentMember = currentMember;
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00018A4C File Offset: 0x00017A4C
		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00018A54 File Offset: 0x00017A54
		public IAssembly CurrentAssembly
		{
			get
			{
				return this.currentAssembly;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x00018A5C File Offset: 0x00017A5C
		public ITypeDefinition CurrentTypeDefinition
		{
			get
			{
				return this.currentTypeDefinition;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x00018A64 File Offset: 0x00017A64
		public IMember CurrentMember
		{
			get
			{
				return this.currentMember;
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00018A6C File Offset: 0x00017A6C
		public ITypeResolveContext WithCurrentTypeDefinition(ITypeDefinition typeDefinition)
		{
			return new SimpleTypeResolveContext(this.compilation, this.currentAssembly, typeDefinition, this.currentMember);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00018A86 File Offset: 0x00017A86
		public ITypeResolveContext WithCurrentMember(IMember member)
		{
			return new SimpleTypeResolveContext(this.compilation, this.currentAssembly, this.currentTypeDefinition, member);
		}

		// Token: 0x040002ED RID: 749
		private readonly ICompilation compilation;

		// Token: 0x040002EE RID: 750
		private readonly IAssembly currentAssembly;

		// Token: 0x040002EF RID: 751
		private readonly ITypeDefinition currentTypeDefinition;

		// Token: 0x040002F0 RID: 752
		private readonly IMember currentMember;
	}
}
