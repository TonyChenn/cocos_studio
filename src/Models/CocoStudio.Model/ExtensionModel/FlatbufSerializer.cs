using System;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Model.ExtensionModel
{
	[Extension(typeof(IGameFileSerializer))]
	[SerializerExtension(true)]
	internal class FlatbufSerializer : BaseCocosFileSerializer
	{
		protected override string OnGetID()
		{
			return "Serializer_FlatBuffers";
		}

		protected override string OnGetLabel()
		{
			return LanguageInfo.ProjSetting_csbFile;
		}

		public override string Description
		{
			get
			{
				return LanguageInfo.ProjSetting_csbInfo;
			}
		}

		protected override int DisplayIndex
		{
			get
			{
				return 0;
			}
		}

		protected override string OnSerialize(PublishInfo info, GameFile gameFile)
		{
			FilePath filePath = info.DestinationFilePath;
			string sourceFilePath = info.SourceFilePath;
			string des = filePath.ChangeExtension(".csb");
			string res = Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory;
			return CSCocosHelp.ConvertToBinByFlat(des, sourceFilePath, res);
		}

		public const string BinaryFileExtension = ".csb";
	}
}
