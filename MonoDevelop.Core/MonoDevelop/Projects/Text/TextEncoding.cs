using System;
using System.Collections.Generic;
using System.Text;
using MonoDevelop.Core;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001FE RID: 510
	public class TextEncoding
	{
		// Token: 0x0600135B RID: 4955 RVA: 0x0004F898 File Offset: 0x0004DA98
		internal TextEncoding(string id, string name)
		{
			this.id = id;
			this.name = name;
			this.codePage = -1;
			try
			{
				Encoding encoding = Encoding.GetEncoding(id);
				if (encoding != null)
				{
					this.codePage = encoding.CodePage;
				}
			}
			catch
			{
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x0600135C RID: 4956 RVA: 0x0004F8EC File Offset: 0x0004DAEC
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x0600135D RID: 4957 RVA: 0x0004F8F4 File Offset: 0x0004DAF4
		public string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600135E RID: 4958 RVA: 0x0004F8FC File Offset: 0x0004DAFC
		public int CodePage
		{
			get
			{
				return this.codePage;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x0600135F RID: 4959 RVA: 0x0004F904 File Offset: 0x0004DB04
		public static TextEncoding[] SupportedEncodings
		{
			get
			{
				if (TextEncoding.supported == null)
				{
					TextEncoding.supported = new TextEncoding[TextEncoding.encodings.GetUpperBound(0)];
					for (int i = 0; i < TextEncoding.encodings.GetUpperBound(0); i++)
					{
						TextEncoding.supported[i] = new TextEncoding(TextEncoding.encodings[i, 0], TextEncoding.encodings[i, 1]);
					}
				}
				return TextEncoding.supported ?? new TextEncoding[0];
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06001360 RID: 4960 RVA: 0x0004F978 File Offset: 0x0004DB78
		// (set) Token: 0x06001361 RID: 4961 RVA: 0x0004FA98 File Offset: 0x0004DC98
		public static TextEncoding[] ConversionEncodings
		{
			get
			{
				if (TextEncoding.conversion == null)
				{
					string text = PropertyService.Get<string>("MonoDevelop.Projects.Text.ConversionEncodings", "");
					if (text.Length == 0)
					{
						text = TextEncoding.DefaultEncoding;
						foreach (string text2 in TextEncoding.defaultEncodings)
						{
							if (text.IndexOf(text2) == -1)
							{
								text = text + " " + text2;
							}
						}
					}
					List<string> list = new List<string>(text.Split(new char[]
					{
						' '
					}));
					List<TextEncoding> list2 = new List<TextEncoding>();
					if (!list.Contains("UTF-16"))
					{
						list.Insert((list.Count > 0) ? 1 : 0, "UTF-16");
					}
					foreach (string text3 in list)
					{
						TextEncoding encoding = TextEncoding.GetEncoding(text3);
						if (encoding != null)
						{
							list2.Add(encoding);
						}
					}
					TextEncoding.conversion = list2.ToArray();
				}
				return TextEncoding.conversion ?? new TextEncoding[0];
			}
			set
			{
				TextEncoding.conversion = value;
				string text = "";
				foreach (TextEncoding textEncoding in TextEncoding.conversion)
				{
					text = text + textEncoding.Id + " ";
				}
				PropertyService.Set("MonoDevelop.Projects.Text.ConversionEncodings", text.Trim());
				PropertyService.SaveProperties();
			}
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x0004FAF0 File Offset: 0x0004DCF0
		public static TextEncoding GetEncoding(string id)
		{
			foreach (TextEncoding textEncoding in TextEncoding.SupportedEncodings)
			{
				if (textEncoding.Id == id)
				{
					return textEncoding;
				}
			}
			return null;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x0004FB2C File Offset: 0x0004DD2C
		public static TextEncoding GetEncoding(int codePage)
		{
			foreach (TextEncoding textEncoding in TextEncoding.SupportedEncodings)
			{
				if (textEncoding.CodePage == codePage)
				{
					return textEncoding;
				}
			}
			return null;
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06001364 RID: 4964 RVA: 0x0004FB61 File Offset: 0x0004DD61
		public static string DefaultEncoding
		{
			get
			{
				return "UTF-8";
			}
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0004FB68 File Offset: 0x0004DD68
		// Note: this type is marked as 'beforefieldinit'.
		static TextEncoding()
		{
			string[,] array = new string[64, 2];
			array[0, 0] = "ISO-8859-1";
			array[0, 1] = GettextCatalog.GetString("Western");
			array[1, 0] = "ISO-8859-2";
			array[1, 1] = GettextCatalog.GetString("Central European");
			array[2, 0] = "ISO-8859-3";
			array[2, 1] = GettextCatalog.GetString("South European");
			array[3, 0] = "ISO-8859-4";
			array[3, 1] = GettextCatalog.GetString("Baltic");
			array[4, 0] = "ISO-8859-5";
			array[4, 1] = GettextCatalog.GetString("Cyrillic");
			array[5, 0] = "ISO-8859-6";
			array[5, 1] = GettextCatalog.GetString("Arabic");
			array[6, 0] = "ISO-8859-7";
			array[6, 1] = GettextCatalog.GetString("Greek");
			array[7, 0] = "ISO-8859-8";
			array[7, 1] = GettextCatalog.GetString("Hebrew Visual");
			array[8, 0] = "ISO-8859-8-I";
			array[8, 1] = GettextCatalog.GetString("Hebrew");
			array[9, 0] = "ISO-8859-9";
			array[9, 1] = GettextCatalog.GetString("Turkish");
			array[10, 0] = "ISO-8859-10";
			array[10, 1] = GettextCatalog.GetString("Nordic");
			array[11, 0] = "ISO-8859-13";
			array[11, 1] = GettextCatalog.GetString("Baltic");
			array[12, 0] = "ISO-8859-14";
			array[12, 1] = GettextCatalog.GetString("Celtic");
			array[13, 0] = "ISO-8859-15";
			array[13, 1] = GettextCatalog.GetString("Western");
			array[14, 0] = "ISO-8859-16";
			array[14, 1] = GettextCatalog.GetString("Romanian");
			array[15, 0] = "UTF-7";
			array[15, 1] = GettextCatalog.GetString("Unicode");
			array[16, 0] = "UTF-8";
			array[16, 1] = GettextCatalog.GetString("Unicode");
			array[17, 0] = "UTF-16";
			array[17, 1] = GettextCatalog.GetString("Unicode");
			array[18, 0] = "UTF-16BE";
			array[18, 1] = GettextCatalog.GetString("Unicode");
			array[19, 0] = "UTF-16LE";
			array[19, 1] = GettextCatalog.GetString("Unicode");
			array[20, 0] = "UTF-32";
			array[20, 1] = GettextCatalog.GetString("Unicode");
			array[21, 0] = "UCS-2";
			array[21, 1] = GettextCatalog.GetString("Unicode");
			array[22, 0] = "UCS-4";
			array[22, 1] = GettextCatalog.GetString("Unicode");
			array[23, 0] = "ARMSCII-8";
			array[23, 1] = GettextCatalog.GetString("Armenian");
			array[24, 0] = "BIG5";
			array[24, 1] = GettextCatalog.GetString("Chinese Traditional");
			array[25, 0] = "BIG5-HKSCS";
			array[25, 1] = GettextCatalog.GetString("Chinese Traditional");
			array[26, 0] = "CP866";
			array[26, 1] = GettextCatalog.GetString("Cyrillic/Russian");
			array[27, 0] = "EUC-JP";
			array[27, 1] = GettextCatalog.GetString("Japanese");
			array[28, 0] = "EUC-JP-MS";
			array[28, 1] = GettextCatalog.GetString("Japanese");
			array[29, 0] = "CP932";
			array[29, 1] = GettextCatalog.GetString("Japanese");
			array[30, 0] = "EUC-KR";
			array[30, 1] = GettextCatalog.GetString("Korean");
			array[31, 0] = "EUC-TW";
			array[31, 1] = GettextCatalog.GetString("Chinese Traditional");
			array[32, 0] = "GB18030";
			array[32, 1] = GettextCatalog.GetString("Chinese Simplified");
			array[33, 0] = "GB2312";
			array[33, 1] = GettextCatalog.GetString("Chinese Simplified");
			array[34, 0] = "GBK";
			array[34, 1] = GettextCatalog.GetString("Chinese Simplified");
			array[35, 0] = "GEORGIAN-ACADEMY";
			array[35, 1] = GettextCatalog.GetString("Georgian");
			array[36, 0] = "HZ";
			array[36, 1] = GettextCatalog.GetString("Chinese Simplified");
			array[37, 0] = "IBM850";
			array[37, 1] = GettextCatalog.GetString("Western");
			array[38, 0] = "IBM852";
			array[38, 1] = GettextCatalog.GetString("Central European");
			array[39, 0] = "IBM855";
			array[39, 1] = GettextCatalog.GetString("Cyrillic");
			array[40, 0] = "IBM857";
			array[40, 1] = GettextCatalog.GetString("Turkish");
			array[41, 0] = "IBM862";
			array[41, 1] = GettextCatalog.GetString("Hebrew");
			array[42, 0] = "IBM864";
			array[42, 1] = GettextCatalog.GetString("Arabic");
			array[43, 0] = "ISO-2022-JP";
			array[43, 1] = GettextCatalog.GetString("Japanese");
			array[44, 0] = "ISO-2022-KR";
			array[44, 1] = GettextCatalog.GetString("Korean");
			array[45, 0] = "ISO-IR-111";
			array[45, 1] = GettextCatalog.GetString("Cyrillic");
			array[46, 0] = "JOHAB";
			array[46, 1] = GettextCatalog.GetString("Korean");
			array[47, 0] = "KOI8R";
			array[47, 1] = GettextCatalog.GetString("Cyrillic");
			array[48, 0] = "KOI8-R";
			array[48, 1] = GettextCatalog.GetString("Cyrillic");
			array[49, 0] = "KOI8U";
			array[49, 1] = GettextCatalog.GetString("Cyrillic/Ukrainian");
			array[50, 0] = "SHIFT_JIS";
			array[50, 1] = GettextCatalog.GetString("Japanese");
			array[51, 0] = "TCVN";
			array[51, 1] = GettextCatalog.GetString("Vietnamese");
			array[52, 0] = "TIS-620";
			array[52, 1] = GettextCatalog.GetString("Thai");
			array[53, 0] = "UHC";
			array[53, 1] = GettextCatalog.GetString("Korean");
			array[54, 0] = "VISCII";
			array[54, 1] = GettextCatalog.GetString("Vietnamese");
			array[55, 0] = "WINDOWS-1250";
			array[55, 1] = GettextCatalog.GetString("Central European");
			array[56, 0] = "WINDOWS-1251";
			array[56, 1] = GettextCatalog.GetString("Cyrillic");
			array[57, 0] = "WINDOWS-1252";
			array[57, 1] = GettextCatalog.GetString("Western");
			array[58, 0] = "WINDOWS-1253";
			array[58, 1] = GettextCatalog.GetString("Greek");
			array[59, 0] = "WINDOWS-1254";
			array[59, 1] = GettextCatalog.GetString("Turkish");
			array[60, 0] = "WINDOWS-1255";
			array[60, 1] = GettextCatalog.GetString("Hebrew");
			array[61, 0] = "WINDOWS-1256";
			array[61, 1] = GettextCatalog.GetString("Arabic");
			array[62, 0] = "WINDOWS-1257";
			array[62, 1] = GettextCatalog.GetString("Baltic");
			array[63, 0] = "WINDOWS-1258";
			array[63, 1] = GettextCatalog.GetString("Vietnamese");
			TextEncoding.encodings = array;
		}

		// Token: 0x040005A5 RID: 1445
		private static TextEncoding[] supported;

		// Token: 0x040005A6 RID: 1446
		private static TextEncoding[] conversion;

		// Token: 0x040005A7 RID: 1447
		private string name;

		// Token: 0x040005A8 RID: 1448
		private string id;

		// Token: 0x040005A9 RID: 1449
		private int codePage;

		// Token: 0x040005AA RID: 1450
		private static string[] defaultEncodings = new string[]
		{
			"UTF-8",
			"ISO-8859-15",
			"UTF-16"
		};

		// Token: 0x040005AB RID: 1451
		private static string[,] encodings;
	}
}
