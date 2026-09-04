using System;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x0200007E RID: 126
	[Extension(typeof(IGameFileSerializer))]
	[SerializerExtension(true)]
	internal class FlatbufSerializer : BaseCocosFileSerializer
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x00013720 File Offset: 0x00011920
		protected override string OnGetID()
		{
			return "Serializer_FlatBuffers";
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00013738 File Offset: 0x00011938
		protected override string OnGetLabel()
		{
			return LanguageInfo.ProjSetting_csbFile;
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00013750 File Offset: 0x00011950
		public override string Description
		{
			get
			{
				return LanguageInfo.ProjSetting_csbInfo;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x00013768 File Offset: 0x00011968
		protected override int DisplayIndex
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001377C File Offset: 0x0001197C
		protected override string OnSerialize(PublishInfo info, GameFile gameFile)
		{
			FilePath filePath = info.DestinationFilePath;
			string sourceFilePath = info.SourceFilePath;
			string des = filePath.ChangeExtension(".csb");
			string res = Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory;
			return CSCocosHelp.ConvertToBinByFlat(des, sourceFilePath, res);
		}

		// Token: 0x04000222 RID: 546
		public const string BinaryFileExtension = ".csb";
	}
}
