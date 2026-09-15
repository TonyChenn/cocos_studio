using System;
using System.Reflection;
using System.Threading;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	public abstract class AssemblyLoader
	{
		public static AssemblyLoader Create()
		{
			return AssemblyLoader.Create(AssemblyLoaderBackend.Auto);
		}

		public static AssemblyLoader Create(AssemblyLoaderBackend backend)
		{
			switch (backend)
			{
			case AssemblyLoaderBackend.Auto:
			case AssemblyLoaderBackend.Cecil:
				return (AssemblyLoader)Assembly.Load("ICSharpCode.NRefactory.Cecil").CreateInstance("ICSharpCode.NRefactory.TypeSystem.CecilLoader");
			case AssemblyLoaderBackend.IKVM:
				return (AssemblyLoader)Assembly.Load("ICSharpCode.NRefactory.IKVM").CreateInstance("ICSharpCode.NRefactory.TypeSystem.IkvmLoader");
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Specifies whether to include internal members. The default is false.
		/// </summary>
		public bool IncludeInternalMembers { get; set; }

		/// <summary>
		/// Gets/Sets the cancellation token used by the assembly loader.
		/// </summary>
		public CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// Gets/Sets the documentation provider that is used to retrieve the XML documentation for all members.
		/// </summary>
		public IDocumentationProvider DocumentationProvider { get; set; }

		/// <summary>
		/// Gets/Sets the interning provider.
		/// </summary>
		public InterningProvider InterningProvider
		{
			get
			{
				return this.interningProvider;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				this.interningProvider = value;
			}
		}

		public abstract IUnresolvedAssembly LoadAssemblyFile(string fileName);

		[CLSCompliant(false)]
		protected InterningProvider interningProvider = new SimpleInterningProvider();
	}
}
