using System;
using System.Linq;
using Mono.Addins;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000247 RID: 583
	internal class UnknownProjectTypeNode : ExtensionNode
	{
		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x06001580 RID: 5504 RVA: 0x0005760F File Offset: 0x0005580F
		// (set) Token: 0x06001581 RID: 5505 RVA: 0x00057617 File Offset: 0x00055817
		[NodeAttribute("guid", Required = true)]
		public string Guid { get; set; }

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x06001582 RID: 5506 RVA: 0x00057620 File Offset: 0x00055820
		// (set) Token: 0x06001583 RID: 5507 RVA: 0x00057628 File Offset: 0x00055828
		[NodeAttribute("name", Required = true)]
		public string Name { get; set; }

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x06001584 RID: 5508 RVA: 0x00057631 File Offset: 0x00055831
		// (set) Token: 0x06001585 RID: 5509 RVA: 0x00057639 File Offset: 0x00055839
		[NodeAttribute("addin")]
		private string requiresAddin { get; set; }

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x06001586 RID: 5510 RVA: 0x00057642 File Offset: 0x00055842
		// (set) Token: 0x06001587 RID: 5511 RVA: 0x0005764A File Offset: 0x0005584A
		[NodeAttribute("platforms")]
		private string requiresPlatform { get; set; }

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001588 RID: 5512 RVA: 0x00057653 File Offset: 0x00055853
		// (set) Token: 0x06001589 RID: 5513 RVA: 0x0005765B File Offset: 0x0005585B
		[NodeAttribute("product")]
		private string requiresProduct { get; set; }

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x0600158A RID: 5514 RVA: 0x00057664 File Offset: 0x00055864
		public bool IsSolvable
		{
			get
			{
				return this.requiresProduct != null || this.requiresAddin != null;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x0600158B RID: 5515 RVA: 0x0005767C File Offset: 0x0005587C
		public bool LoadFiles
		{
			get
			{
				return this.loadFiles;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x0600158C RID: 5516 RVA: 0x00057684 File Offset: 0x00055884
		public string Extension
		{
			get
			{
				return this.extension;
			}
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0005768C File Offset: 0x0005588C
		public bool MatchesGuid(string guid)
		{
			return this.Guid.IndexOf(guid, StringComparison.OrdinalIgnoreCase) != -1;
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000576F8 File Offset: 0x000558F8
		public string GetInstructions()
		{
			if (this.instructions != null)
			{
				return BrandingService.BrandApplicationName(this.instructions);
			}
			if (this.requiresPlatform != null)
			{
				string arg;
				string[] platID;
				if (Platform.IsMac)
				{
					platID = new string[]
					{
						"mac"
					};
					arg = "OS X";
				}
				else if (Platform.IsWindows)
				{
					platID = new string[]
					{
						"win32",
						"windows",
						"win"
					};
					arg = "Windows";
				}
				else
				{
					platID = new string[]
					{
						"linux"
					};
					arg = "Linux";
				}
				string[] source = this.requiresPlatform.Split(new char[]
				{
					';'
				});
				if (!source.Any((string a) => platID.Any((string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase))))
				{
					string @string = GettextCatalog.GetString("This project type is not supported by MonoDevelop on {0}.", arg);
					return BrandingService.BrandApplicationName(@string);
				}
			}
			if (!string.IsNullOrEmpty(this.requiresProduct))
			{
				return GettextCatalog.GetString("This project type requires {0} to be installed.", this.requiresProduct);
			}
			if (!string.IsNullOrEmpty(this.requiresAddin))
			{
				return GettextCatalog.GetString("The {0} add-in is not installed.", this.requiresAddin);
			}
			if (!string.IsNullOrEmpty(this.instructions))
			{
				return BrandingService.BrandApplicationName(base.Addin.Localizer.GetString(this.instructions));
			}
			return BrandingService.BrandApplicationName(GettextCatalog.GetString("This project type is not supported by MonoDevelop."));
		}

		// Token: 0x0400067A RID: 1658
		[NodeAttribute("_instructions", Localizable = true)]
		private string instructions;

		// Token: 0x0400067B RID: 1659
		[NodeAttribute("loadFiles", "If true, MonoDevelop will show the project files in the solution pad")]
		private bool loadFiles = true;

		// Token: 0x0400067C RID: 1660
		[NodeAttribute("extension", "Extension of the project file")]
		private string extension = "";
	}
}
