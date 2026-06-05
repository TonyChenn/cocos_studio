using System;
using System.Collections.Generic;
using System.IO;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x020000E6 RID: 230
	public class BinaryConfigurationWriter
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x00020D80 File Offset: 0x0001EF80
		public void Write(Stream stream, DataNode data)
		{
			BinaryWriter writer = new BinaryWriter(stream);
			this.Write(writer, data);
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00020D9C File Offset: 0x0001EF9C
		public void Write(BinaryWriter writer, DataNode data)
		{
			this.Write(writer, new Dictionary<string, int>(), data);
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00020DAC File Offset: 0x0001EFAC
		private void Write(BinaryWriter writer, Dictionary<string, int> nameTable, DataNode data)
		{
			if (data is DataValue)
			{
				writer.Write(1);
				this.WriteString(writer, nameTable, data.Name);
				this.WriteString(writer, nameTable, ((DataValue)data).Value);
				return;
			}
			if (data is DataItem)
			{
				writer.Write(2);
				this.WriteString(writer, nameTable, data.Name);
				DataItem dataItem = (DataItem)data;
				writer.Write(dataItem.ItemData.Count);
				foreach (object obj in dataItem.ItemData)
				{
					DataNode data2 = (DataNode)obj;
					this.Write(writer, nameTable, data2);
				}
			}
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00020E70 File Offset: 0x0001F070
		private void WriteString(BinaryWriter writer, Dictionary<string, int> nameTable, string str)
		{
			int num;
			if (!nameTable.TryGetValue(str, out num))
			{
				num = nameTable.Count + 1;
				nameTable[str] = num;
				writer.Write(-num);
				writer.Write(str);
				return;
			}
			writer.Write(num);
		}

		// Token: 0x04000297 RID: 663
		public static BinaryConfigurationWriter DefaultWriter = new BinaryConfigurationWriter();
	}
}
