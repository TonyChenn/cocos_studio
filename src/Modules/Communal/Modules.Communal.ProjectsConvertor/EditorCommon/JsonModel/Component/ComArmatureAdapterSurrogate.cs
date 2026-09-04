using System;
using System.IO;
using System.Runtime.Serialization;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Projects;
using Modules.Communal.ProjectsConvertor;
using Mono.Addins;

namespace EditorCommon.JsonModel.Component
{
	// Token: 0x0200000F RID: 15
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComArmatureAdapterSurrogate : ComRenderSurrogate
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000045DF File Offset: 0x000027DF
		// (set) Token: 0x0600008C RID: 140 RVA: 0x000045E7 File Offset: 0x000027E7
		[DataMember]
		public string selectedactionname { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000045F0 File Offset: 0x000027F0
		// (set) Token: 0x0600008E RID: 142 RVA: 0x000045F8 File Offset: 0x000027F8
		[DataMember]
		public bool isShowColliderRect { get; set; }

		// Token: 0x0600008F RID: 143 RVA: 0x00004601 File Offset: 0x00002801
		protected ComArmatureAdapterSurrogate()
		{
			this.classname = "CCArmature";
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004614 File Offset: 0x00002814
		public override void SetValue(object obj)
		{
			base.SetValue(obj);
			string path = Services.ProjectOperations.CurrentSelectedSolution.BaseDirectory;
			string text = Path.Combine(path, "CocosStudio".ToLower());
			try
			{
				string text2 = Path.Combine(text, base.fileData.path);
				string a = Path.GetExtension(text2).ToUpper();
				if (a == ".JSON" || a == ".EXPORTJSON")
				{
					CocosItem cocosItem = ProjectsConvertorHelper.BuildCSD(text2, text, JsonProjType.animation, false);
					if (cocosItem != null)
					{
						FileNodeObjectData fileNodeObjectData = obj as FileNodeObjectData;
						fileNodeObjectData.FileData = new ResourceItemData(EnumResourceType.Normal, Path.GetFileName(cocosItem.FileName), "");
					}
				}
			}
			catch (Exception ex)
			{
				LogConfig.Output.Info(ex.ToString(), true);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000046E8 File Offset: 0x000028E8
		protected override object CreateModelObject()
		{
			return new FileNodeObjectData();
		}
	}
}
