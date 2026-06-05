using System;
using System.IO;
using System.Xml;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000088 RID: 136
	public class XmlDataSerializer
	{
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0000F4C2 File Offset: 0x0000D6C2
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x0000F4CA File Offset: 0x0000D6CA
		public bool StoreAllInElements { get; set; }

		// Token: 0x06000470 RID: 1136 RVA: 0x0000F4D3 File Offset: 0x0000D6D3
		public XmlDataSerializer(DataContext ctx) : this(new DataSerializer(ctx))
		{
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000F4E1 File Offset: 0x0000D6E1
		public XmlDataSerializer(DataSerializer serializer)
		{
			this.serializer = serializer;
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000F4F0 File Offset: 0x0000D6F0
		public void Serialize(string file, object obj)
		{
			this.Serialize(file, obj, null);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000F4FC File Offset: 0x0000D6FC
		public void Serialize(string file, object obj, Type type)
		{
			using (StreamWriter streamWriter = new StreamWriter(file))
			{
				this.Serialize(streamWriter, obj, type);
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000F538 File Offset: 0x0000D738
		public void Serialize(TextWriter writer, object obj)
		{
			this.Serialize(writer, obj, null);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000F544 File Offset: 0x0000D744
		public void Serialize(TextWriter writer, object obj, Type type)
		{
			this.Serialize(new XmlTextWriter(writer)
			{
				Formatting = Formatting.Indented
			}, obj, type);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000F568 File Offset: 0x0000D768
		public void Serialize(XmlWriter writer, object obj)
		{
			this.Serialize(writer, obj, null);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000F574 File Offset: 0x0000D774
		public void Serialize(XmlWriter writer, object obj, Type type)
		{
			DataNode data = this.serializer.Serialize(obj, type);
			new XmlConfigurationWriter
			{
				StoreAllInElements = this.StoreAllInElements
			}.Write(writer, data);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000F5AC File Offset: 0x0000D7AC
		public object Deserialize(string fileName, Type type)
		{
			object result;
			using (StreamReader streamReader = new StreamReader(fileName))
			{
				result = this.Deserialize(streamReader, type);
			}
			return result;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000F5E8 File Offset: 0x0000D7E8
		public object Deserialize(TextReader reader, Type type)
		{
			return this.Deserialize(new XmlTextReader(reader), type);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		public object Deserialize(XmlReader reader, Type type)
		{
			DataNode data = XmlConfigurationReader.DefaultReader.Read(reader);
			return this.serializer.Deserialize(type, data);
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0000F61E File Offset: 0x0000D81E
		public SerializationContext SerializationContext
		{
			get
			{
				return this.serializer.SerializationContext;
			}
		}

		// Token: 0x0400017B RID: 379
		private DataSerializer serializer;
	}
}
