using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;

namespace CocoStudio.Projects
{
	internal class SerializeManager : ISerializeManager
	{
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

		public IEnumerable<IGameFileSerializer> GetSerializerList()
		{
			return this.serializerList.ToList<IGameFileSerializer>();
		}

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

		private List<IGameFileSerializer> serializerList;
	}
}
