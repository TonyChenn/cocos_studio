using System;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.TexturePacker.Model
{
	// Token: 0x02000012 RID: 18
	[Extension(typeof(BaseProjectFileTemplate))]
	internal class PlistCocosFileTemplate : BaseProjectFileTemplate
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004F68 File Offset: 0x00003168
		public override string LabelName
		{
			get
			{
				return LanguageInfo.NewFile_Plist;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004F6F File Offset: 0x0000316F
		public override NodeType FileType
		{
			get
			{
				return NodeType.Plist;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004F72 File Offset: 0x00003172
		public override int Order
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004F75 File Offset: 0x00003175
		public override string Description
		{
			get
			{
				return LanguageInfo.NewFile_PlistDes;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00004F7C File Offset: 0x0000317C
		protected override string OnGetIconResource()
		{
			return "CocoStudio.DefaultResource.Images.ProjectFile.plist.png";
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004F83 File Offset: 0x00003183
		public override string FileExtension
		{
			get
			{
				return ".csi";
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004F8A File Offset: 0x0000318A
		public override int MaxSize
		{
			get
			{
				return -1;
			}
		}
	}
}
