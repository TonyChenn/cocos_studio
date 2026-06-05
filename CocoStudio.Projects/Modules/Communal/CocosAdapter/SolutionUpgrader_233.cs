using System;
using System.IO;
using CocoStudio.Model;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using CocoStudio.Projects.ExtensionModel.Upgrade;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	// Token: 0x0200000E RID: 14
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_233 : SolutionUpgrader
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002A62 File Offset: 0x00000C62
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_233.version;
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002A6C File Offset: 0x00000C6C
		protected override bool OnUpgrade(Solution sln)
		{
			SizeF sizeF = this.ConvertToSize(sln.Config.SolutionSize);
			bool isLandscape = sizeF.Width > sizeF.Height;
			SolutionUpgraderHelper.UpdateConfigJson(Path.GetDirectoryName(sln.ItemDirectory), isLandscape);
			return true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AB4 File Offset: 0x00000CB4
		private SizeF ConvertToSize(string sizeStr)
		{
			string[] array = sizeStr.Split(new char[]
			{
				'*'
			});
			float width = Convert.ToSingle(array[0]);
			float height = Convert.ToSingle(array[1]);
			return new SizeF(width, height);
		}

		// Token: 0x0400000E RID: 14
		private static readonly Version version = new Version("2.3.2");
	}
}
