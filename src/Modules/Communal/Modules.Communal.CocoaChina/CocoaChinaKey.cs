using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Modules.Communal.CocoaChina
{
	public static class CocoaChinaKey
	{
		public static int ConvertDateTimeInt(DateTime time)
		{
			DateTime d = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
			double totalSeconds = (time - d).TotalSeconds;
			return (int)totalSeconds - 60;
		}

		public static string UniversalTime
		{
			get
			{
				return CocoaChinaKey.ConvertDateTimeInt(DateTime.Now).ToString();
			}
		}

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

		public const string CocosKey = "f2fb1076691c445a46b25e1fcc9e95f2";

		public const string CocosSecret = "3f579e7429443a3ec0a9062e27766c72";

		public const string RedirectClientID = "10";
	}
}
