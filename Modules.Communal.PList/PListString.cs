using System;
using System.Collections.Generic;
using System.Text;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x02000013 RID: 19
	public class PListString : PListElement<string>
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00003DFC File Offset: 0x00001FFC
		public override string Tag
		{
			get
			{
				return this.m_IsUTF16 ? "ustring" : "string";
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00003E24 File Offset: 0x00002024
		public override byte TypeCode
		{
			get
			{
				return (byte)(this.m_IsUTF16 ? 6 : 5);
			}
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003E44 File Offset: 0x00002044
		public PListString()
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003E4F File Offset: 0x0000204F
		public PListString(string value)
		{
			this.Value = value;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00003E64 File Offset: 0x00002064
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00003E7C File Offset: 0x0000207C
		public override string Value
		{
			get
			{
				return this.m_Value;
			}
			set
			{
				this.m_Value = value;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00003E86 File Offset: 0x00002086
		protected override void Parse(string value)
		{
			this.Value = value;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00003E94 File Offset: 0x00002094
		protected override string ToXmlString()
		{
			return this.Value;
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003EAC File Offset: 0x000020AC
		public override void ReadBinary(PListBinaryReader reader)
		{
			byte[] array = new byte[reader.CurrentElementLength * ((reader.CurrentElementTypeCode == 5) ? 1 : 2)];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			Encoding encoding = (reader.CurrentElementTypeCode == 5) ? Encoding.UTF8 : Encoding.BigEndianUnicode;
			this.Value = encoding.GetString(array);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00003F1C File Offset: 0x0000211C
		public override int GetPListElementLength()
		{
			return this.Value.Length;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00003F3C File Offset: 0x0000213C
		public override void WriteBinary(PListBinaryWriter writer)
		{
			Encoding encoding = this.m_IsUTF16 ? Encoding.BigEndianUnicode : Encoding.UTF8;
			byte[] bytes = encoding.GetBytes(this.Value);
			writer.BaseStream.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x04000016 RID: 22
		private static byte[] s_UTF8Bytes = new byte[]
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19,
			20,
			21,
			22,
			23,
			24,
			25,
			26,
			27,
			28,
			29,
			30,
			31,
			32,
			33,
			34,
			35,
			36,
			37,
			38,
			39,
			40,
			41,
			42,
			43,
			44,
			45,
			46,
			47,
			48,
			49,
			50,
			51,
			52,
			53,
			54,
			55,
			56,
			57,
			58,
			59,
			60,
			61,
			62,
			63,
			64,
			65,
			66,
			67,
			68,
			69,
			70,
			71,
			72,
			73,
			74,
			75,
			76,
			77,
			78,
			79,
			80,
			81,
			82,
			83,
			84,
			85,
			86,
			87,
			88,
			89,
			90,
			91,
			92,
			93,
			94,
			95,
			96,
			97,
			98,
			99,
			100,
			101,
			102,
			103,
			104,
			105,
			106,
			107,
			108,
			109,
			110,
			111,
			112,
			113,
			114,
			115,
			116,
			117,
			118,
			119,
			120,
			121,
			122,
			123,
			124,
			125,
			126,
			127,
			128,
			129,
			130,
			131,
			132,
			133,
			134,
			135,
			136,
			137,
			138,
			139,
			140,
			141,
			142,
			143,
			144,
			145,
			146,
			147,
			148,
			149,
			150,
			151,
			152,
			153,
			154,
			155,
			156,
			157,
			158,
			159,
			160,
			161,
			162,
			163,
			164,
			165,
			166,
			167,
			168,
			169,
			170,
			171,
			172,
			173,
			174,
			175,
			176,
			177,
			178,
			179,
			180,
			181,
			182,
			183,
			184,
			185,
			186,
			187,
			188,
			189,
			190,
			191,
			192,
			193,
			194,
			195,
			196,
			197,
			198,
			199,
			200,
			201,
			202,
			203,
			204,
			205,
			206,
			207,
			208,
			209,
			210,
			211,
			212,
			213,
			214,
			215,
			216,
			217,
			218,
			219,
			220,
			221,
			222,
			223,
			224,
			225,
			226,
			227,
			228,
			229,
			230,
			231,
			232,
			233,
			234,
			235,
			236,
			237,
			238,
			239,
			240,
			241,
			242,
			243,
			244,
			245,
			246,
			247,
			248,
			249,
			250,
			251,
			252,
			253,
			254,
			byte.MaxValue
		};

		// Token: 0x04000017 RID: 23
		private static HashSet<char> s_UTF8Chars = new HashSet<char>(Encoding.UTF8.GetChars(PListString.s_UTF8Bytes));

		// Token: 0x04000018 RID: 24
		private string m_Value;

		// Token: 0x04000019 RID: 25
		private bool m_IsUTF16;
	}
}
