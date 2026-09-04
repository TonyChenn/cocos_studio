using System;
using System.Collections.Generic;

namespace Modules.Communal.PList.Internal
{
	// Token: 0x02000008 RID: 8
	internal class PListElementFactory
	{
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000049 RID: 73 RVA: 0x00003040 File Offset: 0x00001240
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

		// Token: 0x0600004A RID: 74 RVA: 0x00003070 File Offset: 0x00001270
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

		// Token: 0x0600004B RID: 75 RVA: 0x00003138 File Offset: 0x00001338
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

		// Token: 0x0600004C RID: 76 RVA: 0x000031CC File Offset: 0x000013CC
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

		// Token: 0x0600004D RID: 77 RVA: 0x00003230 File Offset: 0x00001430
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

		// Token: 0x0600004E RID: 78 RVA: 0x000032B8 File Offset: 0x000014B8
		public IPListElement Create(string tag)
		{
			if (this.m_PListElementTags.ContainsKey(tag))
			{
				return (IPListElement)Activator.CreateInstance(this.m_PListElementTags[tag]);
			}
			throw new PListFormatException(string.Format("Unknown PList - Tag ({0})", tag));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003304 File Offset: 0x00001504
		public IPListElement CreateLengthElement(int length)
		{
			return new PListInteger((long)length);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003320 File Offset: 0x00001520
		public IPListElement CreateKeyElement(string key)
		{
			return new PListString(key);
		}

		// Token: 0x0400000B RID: 11
		private static PListElementFactory s_Instance;

		// Token: 0x0400000C RID: 12
		private Dictionary<string, Type> m_PListElementTags = new Dictionary<string, Type>();

		// Token: 0x0400000D RID: 13
		private Dictionary<byte, Type> m_PListElementTypeCodes = new Dictionary<byte, Type>();
	}
}
