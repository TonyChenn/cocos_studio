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
	// Token: 0x0200008B RID: 139
	[Extension(typeof(ICocosFileContent))]
	[DataItem("GameProjectContent")]
	[JsonObject(MemberSerialization.OptIn)]
	public class GameFileContent : ICocosFileContent, ICocosFile, IInitialize, ICocosItem
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00014244 File Offset: 0x00012444
		public bool IsLoaded
		{
			get
			{
				return this.isLoaded;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001425C File Offset: 0x0001245C
		public bool IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00014270 File Offset: 0x00012470
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00014287 File Offset: 0x00012487
		public CocosFile CocosFile { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x00014290 File Offset: 0x00012490
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x000142A7 File Offset: 0x000124A7
		public Dictionary<Type, int> TypeIndex { get; private set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x000142B0 File Offset: 0x000124B0
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x000142C7 File Offset: 0x000124C7
		public HashSet<string> Names { get; set; }

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x000142D0 File Offset: 0x000124D0
		// (set) Token: 0x060004BC RID: 1212 RVA: 0x00014365 File Offset: 0x00012565
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x00014370 File Offset: 0x00012570
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x00014387 File Offset: 0x00012587
		public CameraData SceneCamera { get; set; }

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x00014390 File Offset: 0x00012590
		// (set) Token: 0x060004C0 RID: 1216 RVA: 0x000143A7 File Offset: 0x000125A7
		public AbstractNodeObject RootVisualObject { get; private set; }

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x000143B0 File Offset: 0x000125B0
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x000143C7 File Offset: 0x000125C7
		public TimelineAction TimelineAction { get; private set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000143D0 File Offset: 0x000125D0
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x000143E7 File Offset: 0x000125E7
		[JsonProperty]
		[ItemProperty]
		public GameFileData Content { get; set; }

		// Token: 0x060004C5 RID: 1221 RVA: 0x000143F0 File Offset: 0x000125F0
		public GameFileContent()
		{
			this.TypeIndex = new Dictionary<Type, int>();
			this.Names = new HashSet<string>();
			this.Content = new GameFileData();
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00014428 File Offset: 0x00012628
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

		// Token: 0x060004C7 RID: 1223 RVA: 0x000144AC File Offset: 0x000126AC
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

		// Token: 0x060004C8 RID: 1224 RVA: 0x00014534 File Offset: 0x00012734
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

		// Token: 0x060004C9 RID: 1225 RVA: 0x000145AC File Offset: 0x000127AC
		public void Save(IProgressMonitor monitor)
		{
			if (this.isLoaded)
			{
				this.Content = GameFileLoader.SaveProject(this.RootVisualObject, this.TimelineAction);
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000145E0 File Offset: 0x000127E0
		public void Initialize(IProgressMonitor monitor)
		{
			this.objectsCount = -1;
			if (this.CocosFile.Type == NodeType.Scene.ToString())
			{
				this.Content.ObjectData.Size = Services.ProjectOperations.CurrentSelectedSolution.GetSceneSize();
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00014639 File Offset: 0x00012839
		public void UnLoad(IProgressMonitor monitor)
		{
			this.isLoaded = false;
			this.RootVisualObject = null;
			this.TimelineAction = null;
			this.TypeIndex.Clear();
			this.Names.Clear();
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001466B File Offset: 0x0001286B
		public void ReloadReferencedItem(IProgressMonitor monitor)
		{
			GameFileLoader.FindProjectNodeToReload(this.RootVisualObject);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001467C File Offset: 0x0001287C
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

		// Token: 0x060004CE RID: 1230 RVA: 0x000146EC File Offset: 0x000128EC
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

		// Token: 0x060004CF RID: 1231 RVA: 0x000147F0 File Offset: 0x000129F0
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

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001490C File Offset: 0x00012B0C
		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			return GameFileContent.SearchResourceItemData(this.Content);
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001492C File Offset: 0x00012B2C
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

		// Token: 0x060004D2 RID: 1234 RVA: 0x00014980 File Offset: 0x00012B80
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

		// Token: 0x060004D3 RID: 1235 RVA: 0x00014A88 File Offset: 0x00012C88
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

		// Token: 0x060004D4 RID: 1236 RVA: 0x00014BA0 File Offset: 0x00012DA0
		public bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection)
		{
			bool flag = GameFileContent.UpdateResourcesInObjectData(this.Content.ObjectData, changedResourceCollection);
			bool flag2 = GameFileContent.UpdateResourcesInAnimationData(this.Content.Animation, changedResourceCollection);
			return flag || flag2;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00014BE0 File Offset: 0x00012DE0
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

		// Token: 0x060004D6 RID: 1238 RVA: 0x00014D50 File Offset: 0x00012F50
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

		// Token: 0x04000234 RID: 564
		private bool isLoaded;

		// Token: 0x04000235 RID: 565
		private int objectsCount = -1;
	}
}
