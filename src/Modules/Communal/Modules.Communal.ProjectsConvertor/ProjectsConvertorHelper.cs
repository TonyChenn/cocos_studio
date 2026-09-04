using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using EditorCommon.JsonModel;

namespace Modules.Communal.ProjectsConvertor
{
	// Token: 0x02000042 RID: 66
	public class ProjectsConvertorHelper
	{
		// Token: 0x060003FA RID: 1018 RVA: 0x0000A56C File Offset: 0x0000876C
		public static CocosItem BuildCSD(string jsonPath, string csdDir, JsonProjType projType, bool isBasedProject = true)
		{
			CocosItem result;
			try
			{
				if (!File.Exists(jsonPath))
				{
					result = null;
				}
				else
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(jsonPath);
					string text = Path.Combine(csdDir, fileNameWithoutExtension + ".csd");
					CocosItemCreateInfo cocosItemCreateInfo = new CocosItemCreateInfo(text, null, 0f, 0f);
					if (projType == JsonProjType.scene)
					{
						cocosItemCreateInfo.ContentType = NodeType.Scene.ToString();
					}
					else if (projType == JsonProjType.animation)
					{
						JsonFileHelp.uiAndAniJson = jsonPath;
						JsonFileHelp.isBasedProject = isBasedProject;
						cocosItemCreateInfo.ContentType = NodeType.Skeleton.ToString();
					}
					else
					{
						if (projType != JsonProjType.ui)
						{
							return null;
						}
						JsonFileHelp.uiAndAniJson = jsonPath;
						JsonFileHelp.isBasedProject = isBasedProject;
						cocosItemCreateInfo.ContentType = NodeType.Layer.ToString();
					}
					CocosItem cocosItem = Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(text) as CocosItem;
					ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
					if (cocosItem == null)
					{
						cocosItem = Services.ProjectOperations.AddNewFile(rootFolder, cocosItemCreateInfo);
					}
					GameFileContent gameContent = cocosItem.GetGameContent();
					ProjectsConvertorHelper.ConvertJson(gameContent, jsonPath, projType, isBasedProject);
					Services.ProjectOperations.Save(cocosItem);
					ProjectsConvertorHelper.projList.Add(cocosItem);
					result = cocosItem;
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(ex.ToString());
				result = null;
			}
			return result;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0000A6B8 File Offset: 0x000088B8
		private static void ConvertJson(GameFileContent gamecontent, string jsonpath, JsonProjType projType, bool isBasedProject)
		{
			ProjectsConvertorHelper.InitNodeProp(gamecontent.Content.ObjectData);
			if (projType == JsonProjType.animation)
			{
				AnimationConveter animationConveter = new AnimationConveter();
				animationConveter.ConvertJson(gamecontent.Content, jsonpath);
				return;
			}
			if (projType == JsonProjType.scene)
			{
				JsonFileHelp.ImportCanvas(jsonpath, gamecontent.Content);
				return;
			}
			if (projType == JsonProjType.ui)
			{
				JsonFileHelp.ImportUIFromFile(jsonpath, gamecontent.Content);
			}
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0000A70E File Offset: 0x0000890E
		private static void InitNodeProp(AbstractNodeObjectData node)
		{
			node.Visible = true;
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x0000A717 File Offset: 0x00008917
		public static void Clear()
		{
			ProjectsConvertorHelper.projList.Clear();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x0000A724 File Offset: 0x00008924
		public static void Refresh()
		{
			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			AddResourcesArgs payload = new AddResourcesArgs(rootFolder, ProjectsConvertorHelper.projList, true);
			Services.EventsService.GetEvent<AddResourcesEvent>().Publish(payload);
		}

		// Token: 0x040001D6 RID: 470
		private static List<ResourceItem> projList = new List<ResourceItem>();
	}
}
