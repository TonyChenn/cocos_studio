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
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class ComGUIAdapterSurrogate : ComRenderSurrogate
	{
		public ComGUIAdapterSurrogate()
		{
			this.classname = "GUIComponent";
		}

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

		protected override object CreateModelObject()
		{
			return new FileNodeObjectData();
		}
	}
}
