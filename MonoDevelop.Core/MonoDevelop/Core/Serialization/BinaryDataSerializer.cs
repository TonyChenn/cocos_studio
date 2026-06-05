using System;
using System.IO;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x020000E5 RID: 229
	public class BinaryDataSerializer
	{
		// Token: 0x06000804 RID: 2052 RVA: 0x00020C49 File Offset: 0x0001EE49
		public BinaryDataSerializer(DataContext ctx) : this(new DataSerializer(ctx))
		{
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00020C57 File Offset: 0x0001EE57
		public BinaryDataSerializer(DataSerializer serializer)
		{
			this.serializer = serializer;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00020C66 File Offset: 0x0001EE66
		public void Serialize(string file, object obj)
		{
			this.Serialize(file, obj, null);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00020C74 File Offset: 0x0001EE74
		public void Serialize(string file, object obj, Type type)
		{
			using (Stream stream = File.OpenWrite(file))
			{
				this.Serialize(stream, obj, type);
			}
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00020CB0 File Offset: 0x0001EEB0
		public void Serialize(Stream stream, object obj)
		{
			this.Serialize(stream, obj, null);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00020CBB File Offset: 0x0001EEBB
		public void Serialize(Stream stream, object obj, Type type)
		{
			this.Serialize(new BinaryWriter(stream), obj, type);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00020CCB File Offset: 0x0001EECB
		public void Serialize(BinaryWriter writer, object obj)
		{
			this.Serialize(writer, obj, null);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00020CD8 File Offset: 0x0001EED8
		public void Serialize(BinaryWriter writer, object obj, Type type)
		{
			DataNode data = this.serializer.Serialize(obj, type);
			BinaryConfigurationWriter.DefaultWriter.Write(writer, data);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00020D00 File Offset: 0x0001EF00
		public object Deserialize(string fileName, Type type)
		{
			object result;
			using (Stream stream = File.OpenRead(fileName))
			{
				result = this.Deserialize(stream, type);
			}
			return result;
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00020D3C File Offset: 0x0001EF3C
		public object Deserialize(Stream stream, Type type)
		{
			return this.Deserialize(new BinaryReader(stream), type);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00020D4C File Offset: 0x0001EF4C
		public object Deserialize(BinaryReader reader, Type type)
		{
			DataNode data = BinaryConfigurationReader.DefaultReader.Read(reader);
			return this.serializer.Deserialize(type, data);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00020D72 File Offset: 0x0001EF72
		public SerializationContext SerializationContext
		{
			get
			{
				return this.serializer.SerializationContext;
			}
		}

		// Token: 0x04000296 RID: 662
		private DataSerializer serializer;
	}
}
