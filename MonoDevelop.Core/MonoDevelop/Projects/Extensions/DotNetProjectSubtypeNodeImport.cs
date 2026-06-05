using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200019C RID: 412
	internal class DotNetProjectSubtypeNodeImport : ExtensionNode
	{
		// Token: 0x06000FCF RID: 4047 RVA: 0x0003ABB2 File Offset: 0x00038DB2
		protected override void Read(NodeElement elem)
		{
			this.IsAdd = (elem.NodeName == "AddImport");
			base.Read(elem);
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0003ABD1 File Offset: 0x00038DD1
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x0003ABD9 File Offset: 0x00038DD9
		[NodeAttribute("language")]
		public string Language { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0003ABE2 File Offset: 0x00038DE2
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x0003ABEA File Offset: 0x00038DEA
		[NodeAttribute("projects")]
		public string Projects { get; set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0003ABF3 File Offset: 0x00038DF3
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x0003ABFB File Offset: 0x00038DFB
		public bool IsAdd { get; private set; }
	}
}
