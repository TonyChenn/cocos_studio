using System;
using System.IO;
using CocoStudio.Model;
using CocoStudio.Projects;
using CocoStudio.Projects.ExtensionModel;
using CocoStudio.Projects.ExtensionModel.Upgrade;
using Mono.Addins;

namespace Modules.Communal.CocosAdapter
{
	[Extension(Type = typeof(ISolutionUpgrader))]
	internal class SolutionUpgrader_233 : SolutionUpgrader
	{
		public override Version Version
		{
			get
			{
				return SolutionUpgrader_233.version;
			}
		}

		protected override bool OnUpgrade(Solution sln)
		{
			SizeF sizeF = this.ConvertToSize(sln.Config.SolutionSize);
			bool isLandscape = sizeF.Width > sizeF.Height;
			SolutionUpgraderHelper.UpdateConfigJson(Path.GetDirectoryName(sln.ItemDirectory), isLandscape);
			return true;
		}

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

		private static readonly Version version = new Version("2.3.2");
	}
}
