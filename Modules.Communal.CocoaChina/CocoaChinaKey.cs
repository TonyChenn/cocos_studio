using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x02000002 RID: 2
	public static class CocoaChinaKey
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static int ConvertDateTimeInt(DateTime time)
		{
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			double totalSeconds = (time - d).TotalSeconds;
			return (int)totalSeconds - 60;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002098 File Offset: 0x00000298
		public static string UniversalTime
		{
			get
			{
				return CocoaChinaKey.ConvertDateTimeInt(DateTime.Now).ToString();
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020BC File Offset: 0x000002BC
		public static string GetCocoaUrlEncode(this string str)
		{
			string result;
			if (!string.IsNullOrWhiteSpace(str))
			{
				str = HttpUtility.UrlEncode(str, Encoding.UTF8);
				int num = str.Count<char>();
				List<char> list = new List<char>();
				for (int i = 0; i < num; i++)
				{
					char c = str[i];
					if (c == '%')
					{
						list.Add(c);
						string text = str[i + 1].ToString();
						list.Add(str[i + 1].ToString().ToUpper()[0]);
						list.Add(str[i + 2].ToString().ToUpper()[0]);
						i += 2;
					}
					else
					{
						list.Add(c);
					}
				}
				result = new string(list.ToArray());
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x04000001 RID: 1
		public const string CocosKey = "f2fb1076691c445a46b25e1fcc9e95f2";

		// Token: 0x04000002 RID: 2
		public const string CocosSecret = "3f579e7429443a3ec0a9062e27766c72";

		// Token: 0x04000003 RID: 3
		public const string RedirectClientID = "10";
	}
}
