using System;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200018D RID: 397
	[ExtensionNode(Description = "A language binding. The specified class must implement MonoDevelop.Projects.ILanguageBinding")]
	internal class LanguageBindingCodon : TypeExtensionNode
	{
		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0003A117 File Offset: 0x00038317
		// (set) Token: 0x06000F6F RID: 3951 RVA: 0x0003A11F File Offset: 0x0003831F
		public string[] Supportedextensions
		{
			get
			{
				return this.supportedExtensions;
			}
			set
			{
				this.supportedExtensions = value;
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0003A128 File Offset: 0x00038328
		public ILanguageBinding LanguageBinding
		{
			get
			{
				return (ILanguageBinding)base.GetInstance();
			}
		}

		// Token: 0x04000476 RID: 1142
		[NodeAttribute("supportedextensions", "File extensions supported by this binding (to be shown in the Open File dialog)")]
		private string[] supportedExtensions;
	}
}
