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
	[JsonObject(MemberSerialization.OptIn)]
	[DataInclude(typeof(ResourceItem))]
	public class ResourceGroup : SolutionEntityItem, IInitialize
	{
		[ItemProperty("RootFolder")]
		[JsonProperty(PropertyName = "RootFolder")]
		public ResourceFolder RootFolder { get; private set; }

		private ResourceGroup()
		{
		}

		public ResourceGroup(Solution parentSolution)
		{
			base.ParentFolder = parentSolution.RootFolder;
			base.ParentSolution = parentSolution;
			this.RootFolder = new ResourceFolder(parentSolution.ItemDirectory);
		}

		protected override void OnSave(IProgressMonitor monitor)
		{
			ICocosFile rootFolder = this.RootFolder;
			rootFolder.Save(monitor);
		}

		void IInitialize.Initialize(IProgressMonitor monitor)
		{
			ICocosFile rootFolder = this.RootFolder;
			rootFolder.Initialize(monitor);
		}

		bool IInitialize.IsAutoInitialize
		{
			get
			{
				return true;
			}
		}

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

		public HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor)
		{
			ICocosFile rootFolder = this.RootFolder;
			return rootFolder.GetUsedResources(monitor);
		}

		public HashSet<ResourceData> GetAllResources()
		{
			return this.GetSolutionAllResources(this.RootFolder);
		}

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
