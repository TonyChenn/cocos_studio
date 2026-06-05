using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Mono.Addins;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Core
{
	// Token: 0x02000002 RID: 2
	public static class StringParserService
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static Dictionary<string, object> Properties
		{
			get
			{
				return StringParserService.properties;
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000021A0 File Offset: 0x000003A0
		static StringParserService()
		{
			StringParserService.stringGenerators.Add("DATE", delegate(string tag, string format)
			{
				if (format.Length == 0)
				{
					return DateTime.Today.ToShortDateString();
				}
				return DateTime.Today.ToString(format);
			});
			StringParserService.stringGenerators.Add("TIME", delegate(string tag, string format)
			{
				if (format.Length == 0)
				{
					return DateTime.Now.ToShortTimeString();
				}
				return DateTime.Now.ToString(format);
			});
			StringParserService.stringGenerators.Add("YEAR", (string tag, string format) => DateTime.Today.Year.ToString(format));
			StringParserService.stringGenerators.Add("MONTH", (string tag, string format) => DateTime.Today.Month.ToString(format));
			StringParserService.stringGenerators.Add("DAY", (string tag, string format) => DateTime.Today.Day.ToString(format));
			StringParserService.stringGenerators.Add("HOUR", (string tag, string format) => DateTime.Now.Hour.ToString(format));
			StringParserService.stringGenerators.Add("MINUTE", (string tag, string format) => DateTime.Now.Minute.ToString(format));
			StringParserService.stringGenerators.Add("SECOND", (string tag, string format) => DateTime.Now.Second.ToString(format));
			StringParserService.stringGenerators.Add("USER", (string tag, string format) => Environment.UserName);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002375 File Offset: 0x00000575
		public static string Parse(string input)
		{
			return StringParserService.Parse(input, StringParserService.DefaultStringTagModel);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002384 File Offset: 0x00000584
		public static void Parse(ref string[] inputs)
		{
			for (int i = inputs.GetLowerBound(0); i <= inputs.GetUpperBound(0); i++)
			{
				inputs[i] = StringParserService.Parse(inputs[i], null);
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023BC File Offset: 0x000005BC
		private static string Replace(string tag, IStringTagModel customTags)
		{
			int num = tag.IndexOf(':');
			string text;
			string text2;
			if (num != -1)
			{
				text = tag.Substring(0, num);
				text2 = tag.Substring(num + 1);
			}
			else
			{
				text = tag;
				text2 = string.Empty;
			}
			tag = tag.ToUpperInvariant();
			object value = customTags.GetValue(tag);
			if (value != null)
			{
				return StringParserService.FormatValue(value, text2);
			}
			if (StringParserService.properties.ContainsKey(tag))
			{
				return StringParserService.FormatValue(StringParserService.properties[tag], text2);
			}
			StringParserService.GenerateString generateString;
			if (StringParserService.stringGenerators.TryGetValue(text, out generateString))
			{
				return generateString(text, text2);
			}
			string a;
			if (text2.Length > 0 && (a = text.ToUpper()) != null)
			{
				if (!(a == "ENV"))
				{
					if (!(a == "PROPERTY"))
					{
						goto IL_126;
					}
				}
				else
				{
					using (IDictionaryEnumerator enumerator = Environment.GetEnvironmentVariables().GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							if (dictionaryEntry.Key.ToString().ToUpper() == text2.ToUpper())
							{
								return dictionaryEntry.Value.ToString();
							}
						}
						goto IL_126;
					}
				}
				return PropertyService.Get<string>(text2);
			}
			IL_126:
			return null;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002504 File Offset: 0x00000704
		private static string FormatValue(object val, string format)
		{
			if (format.Length == 0)
			{
				return val.ToString();
			}
			if (val is DateTime)
			{
				return ((DateTime)val).ToString(format);
			}
			if (val is int)
			{
				return ((int)val).ToString(format);
			}
			if (val is uint)
			{
				return ((uint)val).ToString(format);
			}
			if (val is long)
			{
				return ((long)val).ToString(format);
			}
			if (val is ulong)
			{
				return ((ulong)val).ToString(format);
			}
			if (val is short)
			{
				return ((short)val).ToString(format);
			}
			if (val is ushort)
			{
				return ((ushort)val).ToString(format);
			}
			if (val is byte)
			{
				return ((byte)val).ToString(format);
			}
			if (val is sbyte)
			{
				return ((sbyte)val).ToString(format);
			}
			if (val is decimal)
			{
				return ((decimal)val).ToString(format);
			}
			if (val is float)
			{
				return ((float)val).ToString(format);
			}
			if (val is double)
			{
				return ((double)val).ToString(format);
			}
			if (val is string)
			{
				if (format == "upper")
				{
					return val.ToString().ToUpper();
				}
				if (format == "lower")
				{
					return val.ToString().ToLower();
				}
			}
			return val.ToString();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002688 File Offset: 0x00000888
		public static string Parse<T>(string input, Dictionary<string, T> customTags)
		{
			return StringParserService.Parse(input, new DictionaryStringTagModel<T>(customTags));
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002698 File Offset: 0x00000898
		public static string Parse(string input, string[,] customTags)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
			if (customTags != null)
			{
				for (int i = 0; i < customTags.GetLength(0); i++)
				{
					dictionary.Add(customTags[i, 0].ToUpper(), customTags[i, 1]);
				}
			}
			return StringParserService.Parse<object>(input, dictionary);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000026E8 File Offset: 0x000008E8
		public static string Parse(string input, IStringTagModel customTags)
		{
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] == '$')
				{
					i++;
					int index;
					if (i >= input.Length || (index = StringParserService.OpenBraces.IndexOf(input[i])) == -1)
					{
						stringBuilder.Append('$');
						continue;
					}
					i++;
					int num = i;
					while (i < input.Length && input[i] != StringParserService.CloseBraces[index])
					{
						i++;
					}
					string text = input.Substring(num, i - num);
					char c = StringParserService.CloseBraces[index];
					char c2 = StringParserService.OpenBraces[index];
					string text2;
					if ((text2 = StringParserService.Replace(text, customTags)) == null)
					{
						text2 = string.Format("${0}{1}{2}", c2, text, (i < input.Length) ? c.ToString() : "");
					}
					string value = text2;
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append(input[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002978 File Offset: 0x00000B78
		public static IEnumerable<IStringTagProvider> GetProviders()
		{
			foreach (IStringTagProvider provider in AddinManager.GetExtensionObjects(typeof(IStringTagProvider)))
			{
				yield return provider;
			}
			yield break;
		}

		// Token: 0x04000001 RID: 1
		private static Dictionary<string, object> properties = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x04000002 RID: 2
		private static Dictionary<string, StringParserService.GenerateString> stringGenerators = new Dictionary<string, StringParserService.GenerateString>(StringComparer.InvariantCultureIgnoreCase);

		// Token: 0x04000003 RID: 3
		private static StringTagModel DefaultStringTagModel = new StringTagModel();

		// Token: 0x04000004 RID: 4
		private static string OpenBraces = "{(";

		// Token: 0x04000005 RID: 5
		private static string CloseBraces = "})";

		// Token: 0x02000003 RID: 3
		// (Invoke) Token: 0x06000015 RID: 21
		private delegate string GenerateString(string tag, string format);
	}
}
