using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace CocoStudio.Model.Visiter
{
	// Token: 0x02000027 RID: 39
	public static class GameFileVisiter
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00005BC8 File Offset: 0x00003DC8
		public static GameFile GetGameFile(this CocosItem cocosItem)
		{
			GameFile result;
			if (cocosItem != null)
			{
				result = (cocosItem.CocosFile as GameFile);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00005BF4 File Offset: 0x00003DF4
		public static GameFileContent GetGameContent(this CocosItem cocosItem)
		{
			GameFile gameFile = cocosItem.GetGameFile();
			GameFileContent result;
			if (gameFile != null)
			{
				result = (gameFile.Content as GameFileContent);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00005C28 File Offset: 0x00003E28
		public static AbstractNodeObjectData GetRootNodeData(this CocosItem cocosItem)
		{
			return cocosItem.GetGameContent().Content.ObjectData;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00005C4C File Offset: 0x00003E4C
		public static AbstractNodeObject GetRootNode(this CocosItem cocosItem)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			AbstractNodeObject result;
			if (gameContent != null)
			{
				result = gameContent.RootVisualObject;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00005C78 File Offset: 0x00003E78
		public static TimelineAction GetTimelineAction(this CocosItem cocosItem)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			TimelineAction result;
			if (gameContent != null)
			{
				result = gameContent.TimelineAction;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00005CA4 File Offset: 0x00003EA4
		public static bool Is3DFile(this CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Scene3D.ToString();
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00005CCC File Offset: 0x00003ECC
		public static bool IsSkeletonFile(this CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Skeleton.ToString();
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public static bool Is2DFile(this CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Scene.ToString() || cocosItem.ContentType == NodeType.Layer.ToString() || cocosItem.ContentType == NodeType.Node.ToString();
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00005D60 File Offset: 0x00003F60
		public static bool IsGameFile(this CocosItem cocosItem)
		{
			return cocosItem.GetGameFile() != null;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00005D80 File Offset: 0x00003F80
		public static string GetFileType(this CocosItem cocosItem)
		{
			GameFile gameFile = cocosItem.GetGameFile();
			string result;
			if (gameFile != null)
			{
				result = gameFile.Type;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public static int GetTypeIndex(this CocosItem cocosItem, Type objectType)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			int result;
			if (gameContent != null)
			{
				if (gameContent.TypeIndex.ContainsKey(objectType))
				{
					Dictionary<Type, int> typeIndex;
					(typeIndex = gameContent.TypeIndex)[objectType] = typeIndex[objectType] + 1;
				}
				else
				{
					gameContent.TypeIndex.Add(objectType, 1);
				}
				result = gameContent.TypeIndex[objectType];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00005E28 File Offset: 0x00004028
		public static string CreateObjectName(this CocosItem cocosItem, AbstractNodeObject node, string pasteName = "")
		{
			Type type = node.GetType();
			string prefix = node.GetNamePrefix();
			HashSet<string> names = new HashSet<string>();
			GameFileContent gameContent = cocosItem.GetGameContent();
			if (gameContent != null)
			{
				names = gameContent.Names;
			}
			if (!string.IsNullOrEmpty(pasteName))
			{
				prefix = pasteName;
			}
			return cocosItem.ObjectRename(type, names, prefix, "");
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00005E88 File Offset: 0x00004088
		private static string ObjectRename(this CocosItem cocosItem, Type objectType, HashSet<string> names, string prefix, string renameprefix = "")
		{
			int typeIndex = cocosItem.GetTypeIndex(objectType);
			string text = renameprefix + prefix + typeIndex;
			if (!RegexModel.IsValidObjectName(text))
			{
				string text2 = renameprefix + objectType.Name;
				text = text2.Substring(0, text2.Length - 6) + "_" + typeIndex;
			}
			if (names != null)
			{
				if (names.Contains(text))
				{
					return cocosItem.ObjectRename(objectType, names, prefix, renameprefix);
				}
				names.Add(text);
			}
			else
			{
				names = new HashSet<string>();
				names.Add(text);
			}
			return text;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00005F38 File Offset: 0x00004138
		public static void AddName(this CocosItem cocosItem, string name)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			if (gameContent != null)
			{
				gameContent.Names.Add(name);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00005F64 File Offset: 0x00004164
		public static void RemoveName(this CocosItem cocosItem, string name)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			if (gameContent != null)
			{
				gameContent.Names.Remove(name);
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00005F90 File Offset: 0x00004190
		public static bool IsObjectNameStandardized(this CocosItem cocosItem, string objectName)
		{
			bool result;
			if (Services.ProjectOperations.CurrentSelectedSolution == null)
			{
				result = false;
			}
			else
			{
				bool isNameStandardized = Services.ProjectOperations.CurrentSelectedSolution.Config.IsNameStandardized;
				if (isNameStandardized)
				{
					if (!RegexModel.IsValidObjectName(objectName))
					{
						MessageBox.Show(LanguageInfo.MessageBox234_ObjectNameFormatError, MessageBoxImage.Other, null, null);
						return false;
					}
					if (cocosItem.IsObjectNameExist(objectName))
					{
						MessageBox.Show(LanguageInfo.MessageBox233_ObjectNameExists, MessageBoxImage.Other, null, null);
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00006018 File Offset: 0x00004218
		private static bool IsObjectNameExist(this CocosItem cocosItem, string name)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			return gameContent != null && gameContent.Names.Contains(name);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000604C File Offset: 0x0000424C
		public static bool StandardizeSolutionObjectName()
		{
			bool result;
			try
			{
				if (Services.ProjectOperations.CurrentSelectedSolution == null)
				{
					result = false;
				}
				else
				{
					foreach (SolutionEntityItem solutionEntityItem in Services.ProjectOperations.CurrentSelectedSolution.RootFolder.Items)
					{
						ResourceGroup resourceGroup = solutionEntityItem as ResourceGroup;
						if (resourceGroup != null)
						{
							foreach (ResourceItem resourceItem in resourceGroup.RootFolder.Items)
							{
								CocosItem cocosItem = resourceItem as CocosItem;
								if (cocosItem != null)
								{
									IProgressMonitor @default = Services.ProgressMonitors.Default;
									if (!cocosItem.IsLoaded)
									{
										cocosItem.Load(@default);
										cocosItem.StandardizeObjectName();
										cocosItem.Save(@default);
										cocosItem.UnLoad(@default);
									}
									else
									{
										cocosItem.StandardizeObjectName();
									}
								}
							}
						}
					}
					result = true;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Output.Error("Exception while object name normalizing ! {0}", exception);
				result = false;
			}
			return result;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000061F4 File Offset: 0x000043F4
		public static void StandardizeObjectName(this CocosItem cocosItem)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			if (gameContent != null)
			{
				HashSet<string> names = new HashSet<string>();
				AbstractNodeObject rootNode = cocosItem.GetRootNode();
				if (rootNode != null)
				{
					GameFileVisiter.StandardizeNodeObjectName(cocosItem, rootNode, names);
				}
				gameContent.Names = names;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000623C File Offset: 0x0000443C
		private static void StandardizeNodeObjectName(CocosItem cocosItem, AbstractNodeObject o, HashSet<string> names)
		{
			if (names.Contains(o.Name) || !RegexModel.IsValidObjectName(o.Name))
			{
				o.Name = cocosItem.ObjectRename(o.GetType(), names, o.Name + "_", "Re_");
			}
			else
			{
				names.Add(o.Name);
			}
			if (o.Children != null)
			{
				foreach (AbstractNodeObject o2 in o.Children)
				{
					GameFileVisiter.StandardizeNodeObjectName(cocosItem, o2, names);
				}
			}
		}
	}
}
