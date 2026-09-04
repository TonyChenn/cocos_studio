using System;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Model.ExtensionModel
{
	// Token: 0x02000080 RID: 128
	internal class ProtobufSerializer : BaseCocosFileSerializer
	{
		// Token: 0x0600047F RID: 1151 RVA: 0x00013C28 File Offset: 0x00011E28
		protected override string OnGetID()
		{
			return "Serializer_ProtocolBuffers";
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00013C40 File Offset: 0x00011E40
		protected override string OnGetLabel()
		{
			return "Protocol Buffers";
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00013C58 File Offset: 0x00011E58
		protected override string OnSerialize(PublishInfo info, GameFile gameFile)
		{
			FilePath filePath = info.DestinationFilePath;
			string sourceFilePath = info.SourceFilePath;
			string des = filePath.ChangeExtension(".csb");
			string res = Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory;
			return CSCocosHelp.ConvertToBinProto(des, sourceFilePath, res);
		}

		// Token: 0x04000225 RID: 549
		private const string displayName = "Protocol Buffers";
	}
}
