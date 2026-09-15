using System;
using System.Collections.Generic;

namespace Modules.Communal.PList.Internal
{
	internal class PListElementFactory
	{
		public static PListElementFactory Instance
		{
			get
			{
				if (PListElementFactory.s_Instance == null)
				{
					PListElementFactory.s_Instance = new PListElementFactory();
				}
				return PListElementFactory.s_Instance;
			}
		}

		private PListElementFactory()
		{
			this.Register<PListDict>(new PListDict());
			this.Register<PListInteger>(new PListInteger());
			this.Register<PListReal>(new PListReal());
			this.Register<PListString>(new PListString());
			this.Register<PListArray>(new PListArray());
			this.Register<PListData>(new PListData());
			this.Register<PListDate>(new PListDate());
			this.Register<PListString>("string", 5, new PListString());
			this.Register<PListString>("ustring", 6, new PListString());
			this.Register<PListBool>("true", 0, new PListBool());
			this.Register<PListBool>("false", 0, new PListBool());
		}

		private void Register<T>(T element) where T : IPListElement, new()
		{
			if (!this.m_PListElementTags.ContainsKey(element.Tag))
			{
				this.m_PListElementTags.Add(element.Tag, element.GetType());
			}
			if (!this.m_PListElementTypeCodes.ContainsKey(element.TypeCode))
			{
				this.m_PListElementTypeCodes.Add(element.TypeCode, element.GetType());
			}
		}

		private void Register<T>(string tag, byte typeCode, T element) where T : IPListElement, new()
		{
			if (!this.m_PListElementTags.ContainsKey(tag))
			{
				this.m_PListElementTags.Add(tag, element.GetType());
			}
			if (!this.m_PListElementTypeCodes.ContainsKey(typeCode))
			{
				this.m_PListElementTypeCodes.Add(typeCode, element.GetType());
			}
		}

		public IPListElement Create(byte typeCode, int length)
		{
			IPListElement result;
			if (typeCode == 0 && length == 0)
			{
				result = new PListNull();
			}
			else if (typeCode == 0 && length == 15)
			{
				result = new PListFill();
			}
			else
			{
				if (!this.m_PListElementTypeCodes.ContainsKey(typeCode))
				{
					throw new PListFormatException(string.Format("Unknown PList - TypeCode ({0})", typeCode));
				}
				result = (IPListElement)Activator.CreateInstance(this.m_PListElementTypeCodes[typeCode]);
			}
			return result;
		}

		public IPListElement Create(string tag)
		{
			if (this.m_PListElementTags.ContainsKey(tag))
			{
				return (IPListElement)Activator.CreateInstance(this.m_PListElementTags[tag]);
			}
			throw new PListFormatException(string.Format("Unknown PList - Tag ({0})", tag));
		}

		public IPListElement CreateLengthElement(int length)
		{
			return new PListInteger((long)length);
		}

		public IPListElement CreateKeyElement(string key)
		{
			return new PListString(key);
		}

		private static PListElementFactory s_Instance;

		private Dictionary<string, Type> m_PListElementTags = new Dictionary<string, Type>();

		private Dictionary<byte, Type> m_PListElementTypeCodes = new Dictionary<byte, Type>();
	}
}
