using System;
using System.IO;

namespace MonoDevelop.Projects.Utility
{
	// Token: 0x0200023B RID: 571
	public class ByteOrderMark
	{
		// Token: 0x06001523 RID: 5411 RVA: 0x00056671 File Offset: 0x00054871
		private ByteOrderMark(string name, byte[] bytes)
		{
			this.Bytes = bytes;
			this.Name = name;
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001524 RID: 5412 RVA: 0x00056687 File Offset: 0x00054887
		// (set) Token: 0x06001525 RID: 5413 RVA: 0x0005668F File Offset: 0x0005488F
		public string Name { get; private set; }

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001526 RID: 5414 RVA: 0x00056698 File Offset: 0x00054898
		// (set) Token: 0x06001527 RID: 5415 RVA: 0x000566A0 File Offset: 0x000548A0
		public byte[] Bytes { get; private set; }

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001528 RID: 5416 RVA: 0x000566A9 File Offset: 0x000548A9
		public int Length
		{
			get
			{
				return this.Bytes.Length;
			}
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x000566B4 File Offset: 0x000548B4
		public static ByteOrderMark GetByName(string name)
		{
			for (int i = 0; i < ByteOrderMark.table.Length; i++)
			{
				if (ByteOrderMark.table[i].Name == name)
				{
					return ByteOrderMark.table[i];
				}
			}
			return null;
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000566F0 File Offset: 0x000548F0
		public static bool TryParse(byte[] buffer, int available, out ByteOrderMark bom)
		{
			if (buffer.Length >= 2)
			{
				for (int i = 0; i < ByteOrderMark.table.Length; i++)
				{
					bool flag = true;
					if (available >= ByteOrderMark.table[i].Bytes.Length)
					{
						for (int j = 0; j < ByteOrderMark.table[i].Bytes.Length; j++)
						{
							if (buffer[j] != ByteOrderMark.table[i].Bytes[j])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							bom = ByteOrderMark.table[i];
							return true;
						}
					}
				}
			}
			bom = null;
			return false;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0005676C File Offset: 0x0005496C
		public static bool TryParse(Stream stream, out ByteOrderMark bom)
		{
			byte[] array = new byte[4];
			int available;
			if ((available = stream.Read(array, 0, array.Length)) < 2)
			{
				bom = null;
				return false;
			}
			return ByteOrderMark.TryParse(array, available, out bom);
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x000567EC File Offset: 0x000549EC
		// Note: this type is marked as 'beforefieldinit'.
		static ByteOrderMark()
		{
			ByteOrderMark[] array = new ByteOrderMark[14];
			array[0] = new ByteOrderMark("UTF-8", new byte[]
			{
				239,
				187,
				191
			});
			array[1] = new ByteOrderMark("UTF-32BE", new byte[]
			{
				0,
				0,
				254,
				byte.MaxValue
			});
			ByteOrderMark[] array2 = array;
			int num = 2;
			string name = "UTF-32LE";
			byte[] array3 = new byte[4];
			array3[0] = byte.MaxValue;
			array3[1] = 254;
			array2[num] = new ByteOrderMark(name, array3);
			array[3] = new ByteOrderMark("UTF-16BE", new byte[]
			{
				254,
				byte.MaxValue
			});
			array[4] = new ByteOrderMark("UTF-16LE", new byte[]
			{
				byte.MaxValue,
				254
			});
			array[5] = new ByteOrderMark("UTF-7", new byte[]
			{
				43,
				47,
				118,
				56
			});
			array[6] = new ByteOrderMark("UTF-7", new byte[]
			{
				43,
				47,
				118,
				57
			});
			array[7] = new ByteOrderMark("UTF-7", new byte[]
			{
				43,
				47,
				118,
				43
			});
			array[8] = new ByteOrderMark("UTF-7", new byte[]
			{
				43,
				47,
				118,
				47
			});
			array[9] = new ByteOrderMark("UTF-1", new byte[]
			{
				247,
				100,
				76
			});
			array[10] = new ByteOrderMark("UTF-EBCDIC", new byte[]
			{
				221,
				115,
				102,
				115
			});
			array[11] = new ByteOrderMark("SCSU", new byte[]
			{
				14,
				254,
				byte.MaxValue
			});
			array[12] = new ByteOrderMark("BOCU-1", new byte[]
			{
				251,
				238,
				40
			});
			array[13] = new ByteOrderMark("GB18030", new byte[]
			{
				132,
				49,
				149,
				51
			});
			ByteOrderMark.table = array;
		}

		// Token: 0x04000661 RID: 1633
		private static readonly ByteOrderMark[] table;
	}
}
