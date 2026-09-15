using System;
using System.Collections;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Projects
{
	[DataModelExtension]
	[Extension(typeof(IUserData))]
	public class FilePathData : IExtendedDataItem, IDataModel, ICustomDataItem, IUserData
	{
		public ResourceFile File { get; set; }

		private FilePathData()
		{
		}

		public FilePathData(ResourceFile resourceFile)
		{
			this.File = resourceFile;
		}

		public DataCollection Serialize(ITypeSerializer handler)
		{
			SerializationContext serializationContext = handler.SerializationContext;
			if (this.File == null)
			{
				this.relativePath = null;
			}
			else
			{
				FilePath name = ((FilePath)handler.SerializationContext.BaseFile).ParentDirectory.Combine(new string[]
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

		public void Deserialize(ITypeSerializer handler, DataCollection data)
		{
			handler.Deserialize(this, data);
			this.File = (ProjectsService.Instance.CurrentResourceGroup.FindResourceItem(this.relativePath) as ResourceFile);
		}

		public static implicit operator FilePathData(ResourceFile file)
		{
			return new FilePathData(file);
		}

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

		[JsonProperty(PropertyName = "Path")]
		[ItemProperty("Path")]
		private string relativePath;

		private Hashtable hashtable;
	}
}
