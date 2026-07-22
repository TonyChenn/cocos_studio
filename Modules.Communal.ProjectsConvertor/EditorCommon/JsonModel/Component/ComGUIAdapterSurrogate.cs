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
	// Token: 0x02000011 RID: 17
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComGUIAdapterSurrogate : ComRenderSurrogate
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00004774 File Offset: 0x00002974
		public ComGUIAdapterSurrogate()
		{
			this.classname = "GUIComponent";
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004788 File Offset: 0x00002988
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
					CocosItem cocosItem = ProjectsConvertorHelper.BuildCSD(text2, text, JsonProjType.ui, false);
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

		// Token: 0x06000097 RID: 151 RVA: 0x0000485C File Offset: 0x00002A5C
		protected override object CreateModelObject()
		{
			return new FileNodeObjectData();
		}
	}
}
