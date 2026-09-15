using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Default ITypeResolveContext implementation.
	/// </summary>
	public class SimpleTypeResolveContext : ITypeResolveContext, ICompilationProvider
	{
		public SimpleTypeResolveContext(ICompilation compilation)
		{
			if (compilation == null)
			{
				throw new ArgumentNullException("compilation");
			}
			this.compilation = compilation;
		}

		public SimpleTypeResolveContext(IAssembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			this.compilation = assembly.Compilation;
			this.currentAssembly = assembly;
		}

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

		private SimpleTypeResolveContext(ICompilation compilation, IAssembly currentAssembly, ITypeDefinition currentTypeDefinition, IMember currentMember)
		{
			this.compilation = compilation;
			this.currentAssembly = currentAssembly;
			this.currentTypeDefinition = currentTypeDefinition;
			this.currentMember = currentMember;
		}

		public ICompilation Compilation
		{
			get
			{
				return this.compilation;
			}
		}

		public IAssembly CurrentAssembly
		{
			get
			{
				return this.currentAssembly;
			}
		}

		public ITypeDefinition CurrentTypeDefinition
		{
			get
			{
				return this.currentTypeDefinition;
			}
		}

		public IMember CurrentMember
		{
			get
			{
				return this.currentMember;
			}
		}

		public ITypeResolveContext WithCurrentTypeDefinition(ITypeDefinition typeDefinition)
		{
			return new SimpleTypeResolveContext(this.compilation, this.currentAssembly, typeDefinition, this.currentMember);
		}

		public ITypeResolveContext WithCurrentMember(IMember member)
		{
			return new SimpleTypeResolveContext(this.compilation, this.currentAssembly, this.currentTypeDefinition, member);
		}

		private readonly ICompilation compilation;

		private readonly IAssembly currentAssembly;

		private readonly ITypeDefinition currentTypeDefinition;

		private readonly IMember currentMember;
	}
}
