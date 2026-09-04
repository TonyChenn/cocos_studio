using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Model;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Projects
{
	// Token: 0x0200008A RID: 138
	[JsonObject(MemberSerialization.OptIn)]
	[DataInclude(typeof(ResourceItem))]
	public class ResourceGroup : SolutionEntityItem, IInitialize
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600043A RID: 1082 RVA: 0x0000DCE9 File Offset: 0x0000BEE9
		// (set) Token: 0x0600043B RID: 1083 RVA: 0x0000DCF1 File Offset: 0x0000BEF1
		[ItemProperty("RootFolder")]
		[JsonProperty(PropertyName = "RootFolder")]
		public ResourceFolder RootFolder { get; private set; }

		// Token: 0x0600043C RID: 1084 RVA: 0x0000DCFA File Offset: 0x0000BEFA
		private ResourceGroup()
		{
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000DD02 File Offset: 0x0000BF02
		public ResourceGroup(Solution parentSolution)
		{
			base.ParentFolder = parentSolution.RootFolder;
			base.ParentSolution = parentSolution;
			this.RootFolder = new ResourceFolder(parentSolution.ItemDirectory);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000DD30 File Offset: 0x0000BF30
		protected override void OnSave(IProgressMonitor monitor)
		{
			ICocosFile rootFolder = this.RootFolder;
			rootFolder.Save(monitor);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000DD4C File Offset: 0x0000BF4C
		void IInitialize.Initialize(IProgressMonitor monitor)
		{
			ICocosFile rootFolder = this.RootFolder;
			rootFolder.Initialize(monitor);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x0000DD67 File Offset: 0x0000BF67
		bool IInitialize.IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000DD6C File Offset: 0x0000BF6C
		public ResourceItem FindResourceItem(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return null;
			}
			filePath = FileService.MakePathSeparatorsNative(filePath);
			if (!Path.IsPathRooted(filePath))
			{
				filePath = Path.Combine(this.RootFolder.BaseDirectory, filePath);
			}
			return this.FindResourceItem(this.RootFolder, filePath);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000DDE0 File Offset: 0x0000BFE0
		public ResourceItem FindResourceItem(ResourceData resourceData)
		{
			if (resourceData == null)
			{
				return null;
			}
			if (resourceData.Type == EnumResourceType.Normal || resourceData.Type == EnumResourceType.MarkedSubImage)
			{
				return this.FindResourceItem(resourceData.Path);
			}
			if (resourceData.Type == EnumResourceType.PlistSubImage)
			{
				ResourceFolder resourceFolder = this.FindResourceItem(resourceData.Plist) as ResourceFolder;
				if (resourceFolder != null)
				{
					return resourceFolder.Items.FirstOrDefault((ResourceItem a) => a.Name.Equals(resourceData.Path, StringComparison.OrdinalIgnoreCase));
				}
			}
			else
			{
				if (resourceData.Type == EnumResourceType.Default)
				{
					return ResourceFile.DefaultMarker;
				}
				if (resourceData.Type == EnumResourceType.Addin)
				{
					return new AddinsResourceFile(resourceData);
				}
			}
			return null;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
		public ResourceItem FindResourceItem(ResourceItem parentItem, FilePath fullPath)
		{
			if (parentItem.FullPath.Equals(fullPath, StringComparison.OrdinalIgnoreCase))
			{
				return parentItem;
			}
			if (!fullPath.IsChildPathOf(parentItem.FullPath))
			{
				return null;
			}
			FilePath name2 = fullPath.ToRelative(parentItem.FullPath);
			Stack<string> stack = new Stack<string>();
			while (name2 != "" || !string.IsNullOrWhiteSpace(name2.FileName))
			{
				stack.Push(name2.FileName);
				name2 = name2.ParentDirectory;
			}
			if (parentItem is ResourceFolder)
			{
				while (stack.Count != 0)
				{
					string name = stack.Pop();
					ResourceFolder resourceFolder = parentItem as ResourceFolder;
					if (resourceFolder != null)
					{
						parentItem = resourceFolder.Items.SingleOrDefault((ResourceItem n) => n.Name == name);
					}
					if (parentItem == null)
					{
						return null;
					}
				}
			}
			return parentItem;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			ICocosFile rootFolder = this.RootFolder;
			return rootFolder.GetUsedResources(monitor);
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000DFBB File Offset: 0x0000C1BB
		public HashSet<ResourceData> GetAllResources()
		{
			return this.GetSolutionAllResources(this.RootFolder);
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		private HashSet<ResourceData> GetSolutionAllResources(ResourceItem resItem)
		{
			HashSet<ResourceData> hashSet = new HashSet<ResourceData>();
			ResourceFolder resourceFolder = resItem as ResourceFolder;
			if (resourceFolder != null)
			{
				using (IEnumerator<ResourceItem> enumerator = resourceFolder.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ResourceItem resItem2 = enumerator.Current;
						HashSet<ResourceData> solutionAllResources = this.GetSolutionAllResources(resItem2);
						hashSet.UnionWith(solutionAllResources);
					}
					return hashSet;
				}
			}
			if (!(resItem is CocosItem))
			{
				hashSet.Add(resItem.GetResourceData());
			}
			return hashSet;
		}
	}
}
