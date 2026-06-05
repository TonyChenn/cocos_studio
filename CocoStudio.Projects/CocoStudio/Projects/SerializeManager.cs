using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;

namespace CocoStudio.Projects
{
	// Token: 0x02000071 RID: 113
	internal class SerializeManager : ISerializeManager
	{
		// Token: 0x06000380 RID: 896 RVA: 0x0000C8A4 File Offset: 0x0000AAA4
		internal SerializeManager()
		{
			this.serializerList = new List<IGameFileSerializer>();
			IGameFileSerializer[] extensionObjects = AddinManager.GetExtensionObjects<IGameFileSerializer>();
			foreach (IGameFileSerializer gameFileSerializer in extensionObjects)
			{
				this.serializerList.Add(gameFileSerializer);
				BaseCocosFileSerializer baseCocosFileSerializer = gameFileSerializer as BaseCocosFileSerializer;
				if (baseCocosFileSerializer != null)
				{
					object[] customAttributes = baseCocosFileSerializer.GetType().GetCustomAttributes(typeof(SerializerExtensionAttribute), false);
					if (customAttributes.Length > 0)
					{
						baseCocosFileSerializer.IsDefault = ((SerializerExtensionAttribute)customAttributes[0]).IsDefault;
					}
				}
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000381 RID: 897 RVA: 0x0000C92C File Offset: 0x0000AB2C
		// (set) Token: 0x06000382 RID: 898 RVA: 0x0000C970 File Offset: 0x0000AB70
		public IGameFileSerializer CurrentSerializer
		{
			get
			{
				Solution currentSolution = ProjectsService.Instance.CurrentSolution;
				if (currentSolution == null)
				{
					return this.DefaultSerializer;
				}
				string customSerializer = currentSolution.Config.CustomSerializer;
				IGameFileSerializer serializerById = this.GetSerializerById(customSerializer);
				if (serializerById != null)
				{
					return serializerById;
				}
				return this.DefaultSerializer;
			}
			set
			{
				if (value == null)
				{
					return;
				}
				Solution currentSolution = ProjectsService.Instance.CurrentSolution;
				if (currentSolution == null)
				{
					return;
				}
				currentSolution.Config.CustomSerializer = value.ID;
				currentSolution.Config.Save();
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000383 RID: 899 RVA: 0x0000C9AC File Offset: 0x0000ABAC
		public IGameFileSerializer DefaultSerializer
		{
			get
			{
				Solution currentSolution = ProjectsService.Instance.CurrentSolution;
				string id;
				if (currentSolution == null)
				{
					id = "Serializer_FlatBuffers";
				}
				else
				{
					id = currentSolution.Config.DefaultSerializer;
				}
				IGameFileSerializer serializerById = this.GetSerializerById(id);
				if (serializerById == null)
				{
					return this.GetSerializerById("Serializer_FlatBuffers");
				}
				return serializerById;
			}
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000C9F3 File Offset: 0x0000ABF3
		public IEnumerable<IGameFileSerializer> GetSerializerList()
		{
			return this.serializerList.ToList<IGameFileSerializer>();
		}

		// Token: 0x06000385 RID: 901 RVA: 0x0000CA00 File Offset: 0x0000AC00
		private IGameFileSerializer GetSerializerById(string id)
		{
			foreach (IGameFileSerializer gameFileSerializer in this.serializerList)
			{
				if (gameFileSerializer.ID.Equals(id))
				{
					return gameFileSerializer;
				}
			}
			return null;
		}

		// Token: 0x040000D6 RID: 214
		private List<IGameFileSerializer> serializerList;
	}
}
