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
	public static class GameFileVisiter
	{
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

		public static AbstractNodeObjectData GetRootNodeData(this CocosItem cocosItem)
		{
			return cocosItem.GetGameContent().Content.ObjectData;
		}

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

		public static bool Is3DFile(this CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Scene3D.ToString();
		}

		public static bool IsSkeletonFile(this CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Skeleton.ToString();
		}

		public static bool Is2DFile(this CocosItem cocosItem)
		{
			return cocosItem.ContentType == NodeType.Scene.ToString() || cocosItem.ContentType == NodeType.Layer.ToString() || cocosItem.ContentType == NodeType.Node.ToString();
		}

		public static bool IsGameFile(this CocosItem cocosItem)
		{
			return cocosItem.GetGameFile() != null;
		}

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

		public static void AddName(this CocosItem cocosItem, string name)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			if (gameContent != null)
			{
				gameContent.Names.Add(name);
			}
		}

		public static void RemoveName(this CocosItem cocosItem, string name)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			if (gameContent != null)
			{
				gameContent.Names.Remove(name);
			}
		}

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

		private static bool IsObjectNameExist(this CocosItem cocosItem, string name)
		{
			GameFileContent gameContent = cocosItem.GetGameContent();
			return gameContent != null && gameContent.Names.Contains(name);
		}

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
