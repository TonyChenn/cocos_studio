using System;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.ViewModel
{
	public static class GameFileNest
	{
		public static string CheckNest(this CocosItem cocosItem, CocosItem primaryFile = null)
		{
			string fileType = Services.ProjectOperations.CurrentSelectedProject.GetFileType();
			string arg = "";
			string result;
			if (NodeType.Scene3D.ToString() == fileType)
			{
				result = LanguageInfo.MessageBox206_NestedSence3DError;
			}
			else if (NodeType.Scene.ToString() == cocosItem.GetFileType() || NodeType.Scene3D.ToString() == cocosItem.GetFileType())
			{
				if (NodeType.Scene.ToString() == fileType)
				{
					arg = LanguageInfo.NewFile_Scene;
				}
				else if (NodeType.Layer.ToString() == fileType)
				{
					arg = LanguageInfo.NewFile_Layer;
				}
				else if (NodeType.Node.ToString() == fileType)
				{
					arg = LanguageInfo.Display_Component_Entity;
				}
				result = string.Format(LanguageInfo.MessageBox206_NestedSenceError, arg);
			}
			else
			{
				if (primaryFile != null)
				{
					if (cocosItem.HasReferencedItem(primaryFile))
					{
						return string.Format(LanguageInfo.Output_ProjectRefLoop, cocosItem.FileName.FileName);
					}
				}
				result = null;
			}
			return result;
		}
	}
}
