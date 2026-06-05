using System;
using System.Reflection;
using System.Threading;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.TypeSystem.Implementation;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x0200013E RID: 318
	public abstract class AssemblyLoader
	{
		// Token: 0x06000AEE RID: 2798 RVA: 0x00020FD0 File Offset: 0x0001FFD0
		public static AssemblyLoader Create()
		{
			return AssemblyLoader.Create(AssemblyLoaderBackend.Auto);
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00020FD8 File Offset: 0x0001FFD8
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
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00021034 File Offset: 0x00020034
		// (set) Token: 0x06000AF1 RID: 2801 RVA: 0x0002103C File Offset: 0x0002003C
		public bool IncludeInternalMembers { get; set; }

		/// <summary>
		/// Gets/Sets the cancellation token used by the assembly loader.
		/// </summary>
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00021045 File Offset: 0x00020045
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x0002104D File Offset: 0x0002004D
		public CancellationToken CancellationToken { get; set; }

		/// <summary>
		/// Gets/Sets the documentation provider that is used to retrieve the XML documentation for all members.
		/// </summary>
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00021056 File Offset: 0x00020056
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x0002105E File Offset: 0x0002005E
		public IDocumentationProvider DocumentationProvider { get; set; }

		/// <summary>
		/// Gets/Sets the interning provider.
		/// </summary>
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00021067 File Offset: 0x00020067
		// (set) Token: 0x06000AF7 RID: 2807 RVA: 0x0002106F File Offset: 0x0002006F
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

		// Token: 0x06000AF8 RID: 2808
		public abstract IUnresolvedAssembly LoadAssemblyFile(string fileName);

		// Token: 0x040003BB RID: 955
		[CLSCompliant(false)]
		protected InterningProvider interningProvider = new SimpleInterningProvider();
	}
}
