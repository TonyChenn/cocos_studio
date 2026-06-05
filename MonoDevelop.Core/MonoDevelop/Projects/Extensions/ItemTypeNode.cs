using System;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x02000190 RID: 400
	public abstract class ItemTypeNode : ExtensionNode
	{
		// Token: 0x06000F78 RID: 3960 RVA: 0x0003A188 File Offset: 0x00038388
		public ItemTypeNode()
		{
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x0003A190 File Offset: 0x00038390
		public ItemTypeNode(string guid, string extension, string import)
		{
			this.guid = guid;
			this.extension = extension;
			this.import = import;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000F7A RID: 3962 RVA: 0x0003A1AD File Offset: 0x000383AD
		public string Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x0003A1B5 File Offset: 0x000383B5
		public string Extension
		{
			get
			{
				return this.extension;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000F7C RID: 3964 RVA: 0x0003A1BD File Offset: 0x000383BD
		public string Import
		{
			get
			{
				return this.import;
			}
		}

		// Token: 0x06000F7D RID: 3965
		public abstract bool CanHandleItem(SolutionEntityItem item);

		// Token: 0x06000F7E RID: 3966 RVA: 0x0003A1C8 File Offset: 0x000383C8
		public virtual void InitializeHandler(SolutionEntityItem item)
		{
			MSBuildHandler msbuildHandler = this.CreateHandler<MSBuildHandler>(null, null);
			msbuildHandler.Item = item;
			item.SetItemHandler(msbuildHandler);
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x0003A1EC File Offset: 0x000383EC
		public virtual bool CanHandleFile(string fileName, string typeGuid)
		{
			return (typeGuid != null && string.Compare(typeGuid, this.guid, true) == 0) || (!string.IsNullOrEmpty(this.extension) && System.IO.Path.GetExtension(fileName) == "." + this.extension);
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x0003A23C File Offset: 0x0003843C
		protected T CreateHandler<T>(string fileName, string itemGuid) where T : MSBuildHandler
		{
			MSBuildHandler msbuildHandler = this.OnCreateHandler(fileName, itemGuid);
			if (!(msbuildHandler is T))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Error while creating a MSBuildHandler. Expected an object of type '",
					typeof(T).FullName,
					", found type '",
					msbuildHandler.GetType()
				}));
			}
			return (T)((object)msbuildHandler);
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x0003A2A0 File Offset: 0x000384A0
		protected virtual MSBuildHandler OnCreateHandler(string fileName, string itemGuid)
		{
			MSBuildHandler msbuildHandler;
			if (!string.IsNullOrEmpty(this.handlerType))
			{
				msbuildHandler = (base.Addin.CreateInstance(this.handlerType, true) as MSBuildHandler);
				if (msbuildHandler == null)
				{
					throw new InvalidOperationException("Type '" + this.handlerType + "' must be a subclass of 'MonoDevelop.Projects.Formats.MSBuild.MSBuildHandler'");
				}
				if (msbuildHandler is MSBuildProjectHandler)
				{
					((MSBuildProjectHandler)msbuildHandler).Initialize(this.Guid, this.Import, itemGuid);
				}
				else
				{
					msbuildHandler.Initialize(this.Guid, itemGuid);
				}
			}
			else
			{
				msbuildHandler = new MSBuildProjectHandler(this.Guid, this.Import, itemGuid);
			}
			return msbuildHandler;
		}

		// Token: 0x06000F82 RID: 3970
		public abstract SolutionEntityItem LoadSolutionItem(IProgressMonitor monitor, string fileName, MSBuildFileFormat expectedFormat, string itemGuid);

		// Token: 0x0400047A RID: 1146
		[NodeAttribute(Required = true)]
		private string guid;

		// Token: 0x0400047B RID: 1147
		[NodeAttribute]
		private string extension;

		// Token: 0x0400047C RID: 1148
		[NodeAttribute]
		private string import;

		// Token: 0x0400047D RID: 1149
		[NodeAttribute]
		private string handlerType;
	}
}
