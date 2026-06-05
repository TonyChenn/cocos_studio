using System;
using System.Xml;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x0200008B RID: 139
	public class XmlElementDataType : DataType
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x0000FD61 File Offset: 0x0000DF61
		public XmlElementDataType() : base(typeof(XmlElement))
		{
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0000FD74 File Offset: 0x0000DF74
		protected internal override DataNode OnSerialize(SerializationContext serCtx, object mapData, object value)
		{
			XmlElement elem = (XmlElement)value;
			XmlConfigurationReader xmlConfigurationReader = new XmlConfigurationReader();
			return xmlConfigurationReader.Read(elem);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0000FD98 File Offset: 0x0000DF98
		protected internal override object OnDeserialize(SerializationContext serCtx, object mapData, DataNode data)
		{
			XmlConfigurationWriter xmlConfigurationWriter = new XmlConfigurationWriter();
			XmlDocument doc = new XmlDocument();
			return xmlConfigurationWriter.Write(doc, data);
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0000FDB9 File Offset: 0x0000DFB9
		public override bool IsSimpleType
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		public override bool CanCreateInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0000FDBF File Offset: 0x0000DFBF
		public override bool CanReuseInstance
		{
			get
			{
				return false;
			}
		}
	}
}
