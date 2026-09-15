using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using CocoStudio.Projects.Visiter;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[Extension(typeof(ICocosFileContent))]
	[DataItem("GameProjectContent")]
	[JsonObject(MemberSerialization.OptIn)]
	public class GameFileContent : ICocosFileContent, ICocosFile, IInitialize, ICocosItem
	{
		public bool IsLoaded
		{
			get
			{
				return this.isLoaded;
			}
		}

		public bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		public CocosFile CocosFile { get; set; }

		public Dictionary<Type, int> TypeIndex { get; private set; }

		public HashSet<string> Names { get; set; }

		public int ObjectsCount
		{
			get
			{
				if (this.IsLoaded)
				{
					this.objectsCount = this.Names.Count<string>();
				}
				else if (this.objectsCount == -1)
				{
					if (this.Content == null)
					{
						throw new ArgumentNullException("Content is null. {0}", this.CocosFile.FileName);
					}
					this.objectsCount = this.GetObjects(this.Content.ObjectData).Count;
				}
				return this.objectsCount;
			}
			private set
			{
				this.objectsCount = value;
			}
		}

		public CameraData SceneCamera { get; set; }

		public AbstractNodeObject RootVisualObject { get; private set; }

		public TimelineAction TimelineAction { get; private set; }

		[JsonProperty]
		[ItemProperty]
		public GameFileData Content { get; set; }

		public GameFileContent()
		{
			this.TypeIndex = new Dictionary<Type, int>();
			this.Names = new HashSet<string>();
			this.Content = new GameFileData();
		}

		public GameFileContent(NodeType type)
		{
			this.TypeIndex = new Dictionary<Type, int>();
			this.Names = new HashSet<string>();
			IProjectFileCreator projectFileCreator = ProjectFileTemplateService.GetProjectFileCreator(type);
			if (projectFileCreator == null)
			{
				LogConfig.Logger.Error(string.Format("未找到{0}类型对应的项目文件创建视图", type));
				this.Content = new GameFileData();
			}
			else
			{
				this.Content = projectFileCreator.CreateGameProjectData();
			}
		}

		private List<AbstractNodeObjectData> GetObjects(AbstractNodeObjectData data)
		{
			List<AbstractNodeObjectData> list = new List<AbstractNodeObjectData>();
			list.Add(data);
			if (data.Children != null)
			{
				foreach (AbstractNodeObjectData data2 in data.Children)
				{
					list.AddRange(this.GetObjects(data2));
				}
			}
			return list;
		}

		public void Load(IProgressMonitor monitor)
		{
			if (!this.isLoaded)
			{
				GameFileLoadResult gameFileLoadResult = GameFileLoader.LoadProject(this.Content);
				gameFileLoadResult.RootObject.IsSelected = false;
				this.RootVisualObject = gameFileLoadResult.RootObject;
				this.TimelineAction = gameFileLoadResult.TimelineAction;
				this.TimelineAction.InitWithRootNode(this.RootVisualObject);
				this.Names = gameFileLoadResult.Names;
				this.isLoaded = true;
			}
		}

		public void Save(IProgressMonitor monitor)
		{
			if (this.isLoaded)
			{
				this.Content = GameFileLoader.SaveProject(this.RootVisualObject, this.TimelineAction);
			}
		}

		public void Initialize(IProgressMonitor monitor)
		{
			this.objectsCount = -1;
			if (this.CocosFile.Type == NodeType.Scene.ToString())
			{
				this.Content.ObjectData.Size = Services.ProjectOperations.CurrentSelectedSolution.GetSceneSize();
			}
		}

		public void UnLoad(IProgressMonitor monitor)
		{
			this.isLoaded = false;
			this.RootVisualObject = null;
			this.TimelineAction = null;
			this.TypeIndex.Clear();
			this.Names.Clear();
		}

		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
			GameFileLoader.FindProjectNodeToReload(this.RootVisualObject);
		}

		public bool HasReferencedItem(CocosItem item)
		{
			bool result;
			if (this.IsLoaded)
			{
				result = GameFileContent.CheckReferenceFileInObject(this.RootVisualObject, item);
			}
			else if (this.Content == null || this.Content.ObjectData == null)
			{
				result = false;
			}
			else
			{
				ResourceData resourceData = item.GetResourceData();
				result = GameFileContent.CheckReferenceFileInObjectData(this.Content.ObjectData, item, resourceData);
			}
			return result;
		}

		private static bool CheckReferenceFileInObject(AbstractNodeObject parentNode, CocosItem cocosItem)
		{
			bool result;
			if (parentNode == null)
			{
				result = false;
			}
			else
			{
				FileNodeObject fileNodeObject = parentNode as FileNodeObject;
				if (fileNodeObject != null)
				{
					ResourceFile fileData = fileNodeObject.FileData;
					if (fileData == cocosItem)
					{
						return true;
					}
					if (fileNodeObject.Project != null)
					{
						bool flag = fileNodeObject.Project.HasReferencedItem(cocosItem);
						if (flag)
						{
							return true;
						}
					}
				}
				if (parentNode.Children != null)
				{
					foreach (AbstractNodeObject parentNode2 in parentNode.Children)
					{
						bool flag2 = GameFileContent.CheckReferenceFileInObject(parentNode2, cocosItem);
						if (flag2)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		private static bool CheckReferenceFileInObjectData(AbstractNodeObjectData parentNode, CocosItem project, ResourceData projectPath)
		{
			bool result;
			if (parentNode == null)
			{
				result = false;
			}
			else
			{
				FileNodeObjectData fileNodeObjectData = parentNode as FileNodeObjectData;
				if (fileNodeObjectData != null)
				{
					ResourceItemData fileData = fileNodeObjectData.FileData;
					if (projectPath.Equals(fileData))
					{
						return true;
					}
					if (fileData != null)
					{
						CocosItem cocosItem = Services.ProjectOperations.FindResourceItem(fileData) as CocosItem;
						if (cocosItem != null)
						{
							bool flag = cocosItem.HasReferencedItem(project);
							if (flag)
							{
								return true;
							}
						}
					}
				}
				if (parentNode.Children != null)
				{
					foreach (AbstractNodeObjectData parentNode2 in parentNode.Children)
					{
						bool flag2 = GameFileContent.CheckReferenceFileInObjectData(parentNode2, project, projectPath);
						if (flag2)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return GameFileContent.SearchResourceItemData(this.Content);
		}

		private static HashSet<ResourceData> SearchResourceItemData(GameFileData projectData)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			HashSet<ResourceData> hashSet2 = GameFileContent.ScanObjectData(projectData.ObjectData);
			if (hashSet2 != null)
			{
				hashSet.UnionWith(hashSet2);
			}
			hashSet2 = GameFileContent.ScanAnimationData(projectData.Animation);
			if (hashSet2 != null)
			{
				hashSet.UnionWith(hashSet2);
			}
			return hashSet;
		}

		private static HashSet<ResourceData> ScanObjectData(AbstractNodeObjectData parentNodeData)
		{
			IEnumerable<PropertyAccessorHandler> resourceProperties = parentNodeData.GetResourceProperties();
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			if (resourceProperties != null)
			{
				foreach (PropertyAccessorHandler propertyAccessorHandler in resourceProperties)
				{
					ResourceItemData resourceItemData = propertyAccessorHandler.GetValue(parentNodeData, null) as ResourceItemData;
					if (resourceItemData != null)
					{
						hashSet.Add(resourceItemData);
					}
				}
			}
			if (parentNodeData.Children != null)
			{
				foreach (AbstractNodeObjectData parentNodeData2 in parentNodeData.Children)
				{
					hashSet.UnionWith(GameFileContent.ScanObjectData(parentNodeData2));
				}
			}
			return hashSet;
		}

		private static HashSet<ResourceData> ScanAnimationData(TimelineActionData timelineActionData)
		{
			HashSet<ResourceData> result;
			if (timelineActionData == null)
			{
				result = null;
			}
			else
			{
				HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
				foreach (TimelineData timelineData in timelineActionData.Timelines)
				{
					if (timelineData.Frames.Count != 0 && timelineData.Frames[0] is TextureFrameData)
					{
						foreach (FrameData frameData in timelineData.Frames)
						{
							TextureFrameData textureFrameData = (TextureFrameData)frameData;
							if (textureFrameData.TextureFile != null)
							{
								hashSet.Add(textureFrameData.TextureFile);
							}
						}
					}
				}
				result = hashSet;
			}
			return result;
		}

		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection)
		{
			bool flag = GameFileContent.UpdateResourcesInObjectData(this.Content.ObjectData, changedResourceCollection);
			bool flag2 = GameFileContent.UpdateResourcesInAnimationData(this.Content.Animation, changedResourceCollection);
			return flag || flag2;
		}

		private static bool UpdateResourcesInObjectData(AbstractNodeObjectData parentNodeData, ChangedResourceCollection changedResourceCollection)
		{
			IEnumerable<PropertyAccessorHandler> resourceProperties = parentNodeData.GetResourceProperties();
			bool result;
			if (resourceProperties == null)
			{
				result = false;
			}
			else
			{
				bool flag = false;
				foreach (PropertyAccessorHandler propertyAccessorHandler in resourceProperties)
				{
					ResourceItemData resourceItemData = propertyAccessorHandler.GetValue(parentNodeData, null) as ResourceItemData;
					if (!(resourceItemData == null))
					{
						ResourceFile resourceFile = null;
						if (changedResourceCollection.TryGetValue(resourceItemData, out resourceFile))
						{
							if (resourceFile != null)
							{
								resourceItemData.Update(resourceFile.GetResourceData());
							}
							else
							{
								propertyAccessorHandler.SetValue(parentNodeData, ResourceItemData.DefaultMarker, null);
							}
							if (!flag)
							{
								flag = true;
							}
						}
					}
				}
				if (parentNodeData.Children != null)
				{
					foreach (AbstractNodeObjectData parentNodeData2 in parentNodeData.Children)
					{
						bool flag2 = GameFileContent.UpdateResourcesInObjectData(parentNodeData2, changedResourceCollection);
						if (!flag && flag2)
						{
							flag = flag2;
						}
					}
				}
				result = flag;
			}
			return result;
		}

		private static bool UpdateResourcesInAnimationData(TimelineActionData timelineActionData, ChangedResourceCollection changedResourceCollection)
		{
			bool flag = false;
			foreach (TimelineData timelineData in timelineActionData.Timelines)
			{
				if (timelineData.Frames.Count != 0 && timelineData.Frames[0] is TextureFrameData)
				{
					foreach (FrameData frameData in timelineData.Frames)
					{
						TextureFrameData textureFrameData = (TextureFrameData)frameData;
						ResourceItemData textureFile = textureFrameData.TextureFile;
						if (!(textureFile == null))
						{
							ResourceFile resourceFile = null;
							if (changedResourceCollection.TryGetValue(textureFile, out resourceFile))
							{
								if (resourceFile != null)
								{
									textureFile.Update(resourceFile.GetResourceData());
								}
								else
								{
									textureFrameData.TextureFile = null;
								}
								if (!flag)
								{
									flag = true;
								}
							}
						}
					}
				}
			}
			return flag;
		}

		private bool isLoaded;

		private int objectsCount = -1;
	}
}
