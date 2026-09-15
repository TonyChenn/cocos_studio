using System;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.TexturePacker.Model
{
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class PlistCocosFileTemplate : BaseProjectFileTemplate
	{
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Plist;
			}
		}

		public override NodeType FileType
		{
			get
			{
				return NodeType.Plist;
			}
		}

		public override int Order
		{
			get
			{
				return 3;
			}
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_PlistDes;
			}
		}

		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.plist.png";
		}

		public override string FileExtension
		{
			get
			{
				return ".csi";
			}
		}

		public override int MaxSize
		{
			get
			{
				return -1;
			}
		}
	}
}
