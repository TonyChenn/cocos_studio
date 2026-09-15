using System;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Model.ExtensionModel
{
	internal class ProtobufSerializer : BaseCocosFileSerializer
	{
		protected override string OnGetID()
		{
			return "Serializer_ProtocolBuffers";
		}

		protected override string OnGetLabel()
		{
			return "Protocol Buffers";
		}

		protected override string OnSerialize(PublishInfo info, GameFile gameFile)
		{
			FilePath filePath = info.DestinationFilePath;
			string sourceFilePath = info.SourceFilePath;
			string des = filePath.ChangeExtension(".csb");
			string res = Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory;
			return CSCocosHelp.ConvertToBinProto(des, sourceFilePath, res);
		}

		private const string displayName = "Protocol Buffers";
	}
}
