using System;
using System.Collections;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Projects
{
	// Token: 0x0200004E RID: 78
	[DataModelExtension]
	[Extension(typeof(IUserData))]
	public class FilePathData : IExtendedDataItem, IDataModel, ICustomDataItem, IUserData
	{
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00008509 File Offset: 0x00006709
		// (set) Token: 0x0600021E RID: 542 RVA: 0x00008511 File Offset: 0x00006711
		public ResourceFile File { get; set; }

		// Token: 0x0600021F RID: 543 RVA: 0x0000851A File Offset: 0x0000671A
		private FilePathData()
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008522 File Offset: 0x00006722
		public FilePathData(ResourceFile resourceFile)
		{
			this.File = resourceFile;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00008534 File Offset: 0x00006734
		public DataCollection Serialize(ITypeSerializer handler)
		{
			SerializationContext serializationContext = handler.SerializationContext;
			if (this.File == null)
			{
				this.relativePath = null;
			}
			else
			{
				string name = handler.SerializationContext.BaseFile.ParentDirectory.Combine(new string[]
				{
					"CocosStudio".ToLower()
				});
				this.relativePath = this.File.FileName.ToRelative(name);
				if (Path.DirectorySeparatorChar != serializationContext.DirectorySeparatorChar)
				{
					this.relativePath = this.relativePath.Replace(Path.DirectorySeparatorChar, serializationContext.DirectorySeparatorChar);
				}
			}
			return handler.Serialize(this);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x000085EF File Offset: 0x000067EF
		public void Deserialize(ITypeSerializer handler, DataCollection data)
		{
			handler.Deserialize(this, data);
			this.File = (ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(this.relativePath) as ResourceFile);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x00008619 File Offset: 0x00006819
		public static implicit operator FilePathData(ResourceFile file)
		{
			return new FilePathData(file);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00008621 File Offset: 0x00006821
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.hashtable == null)
				{
					this.hashtable = new Hashtable();
				}
				return this.hashtable;
			}
		}

		// Token: 0x04000084 RID: 132
		[JsonProperty(PropertyName = "Path")]
		[ItemProperty("Path")]
		private string relativePath;

		// Token: 0x04000085 RID: 133
		private Hashtable hashtable;
	}
}
