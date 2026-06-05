using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x020000E7 RID: 231
	public class BinaryConfigurationReader
	{
		// Token: 0x06000816 RID: 2070 RVA: 0x00020EC4 File Offset: 0x0001F0C4
		public DataNode Read(Stream stream)
		{
			return this.Read(new BinaryReader(stream), new Dictionary<int, string>());
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00020ED7 File Offset: 0x0001F0D7
		public DataNode Read(BinaryReader reader)
		{
			return this.Read(reader, new Dictionary<int, string>());
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x00020EE8 File Offset: 0x0001F0E8
		private DataNode Read(BinaryReader reader, Dictionary<int, string> nameTable)
		{
			byte b = reader.ReadByte();
			if (b == 1)
			{
				string name = this.ReadString(reader, nameTable);
				string value = this.ReadString(reader, nameTable);
				return new DataValue(name, value);
			}
			if (b == 2)
			{
				DataItem dataItem = new DataItem();
				dataItem.Name = this.ReadString(reader, nameTable);
				int num = reader.ReadInt32();
				while (num-- > 0)
				{
					dataItem.ItemData.Add(this.Read(reader, nameTable));
				}
				return dataItem;
			}
			throw new InvalidOperationException("Unknown node type: " + b);
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00020F70 File Offset: 0x0001F170
		private string ReadString(BinaryReader reader, Dictionary<int, string> nameTable)
		{
			int num = reader.ReadInt32();
			if (num < 0)
			{
				string text = reader.ReadString();
				nameTable[-num] = text;
				return text;
			}
			return nameTable[num];
		}

		// Token: 0x04000298 RID: 664
		public static BinaryConfigurationReader DefaultReader = new BinaryConfigurationReader();
	}
}
