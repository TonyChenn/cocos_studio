using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Mono.Options
{
	// Token: 0x020000F5 RID: 245
	public class OptionSet : KeyedCollection<string, Option>
	{
		// Token: 0x0600089C RID: 2204 RVA: 0x00021E6E File Offset: 0x0002006E
		public OptionSet() : this((string f) => f)
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00021E93 File Offset: 0x00020093
		public OptionSet(Converter<string, string> localizer)
		{
			this.localizer = localizer;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00021EB2 File Offset: 0x000200B2
		public Converter<string, string> MessageLocalizer
		{
			get
			{
				return this.localizer;
			}
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00021EBA File Offset: 0x000200BA
		protected override string GetKeyForItem(Option item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("option");
			}
			if (item.Names != null && item.Names.Length > 0)
			{
				return item.Names[0];
			}
			throw new InvalidOperationException("Option has no names!");
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00021EF0 File Offset: 0x000200F0
		protected override void InsertItem(int index, Option item)
		{
			base.InsertItem(index, item);
			this.AddImpl(item);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00021F04 File Offset: 0x00020104
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
			Option option = base.Items[index];
			for (int i = 1; i < option.Names.Length; i++)
			{
				base.Dictionary.Remove(option.Names[i]);
			}
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00021F4C File Offset: 0x0002014C
		protected override void SetItem(int index, Option item)
		{
			base.SetItem(index, item);
			this.RemoveItem(index);
			this.AddImpl(item);
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00021F64 File Offset: 0x00020164
		private void AddImpl(Option option)
		{
			if (option == null)
			{
				throw new ArgumentNullException("option");
			}
			List<string> list = new List<string>(option.Names.Length);
			try
			{
				for (int i = 1; i < option.Names.Length; i++)
				{
					base.Dictionary.Add(option.Names[i], option);
					list.Add(option.Names[i]);
				}
			}
			catch (Exception)
			{
				foreach (string key in list)
				{
					base.Dictionary.Remove(key);
				}
				throw;
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0002201C File Offset: 0x0002021C
		public new OptionSet Add(Option option)
		{
			base.Add(option);
			return this;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00022026 File Offset: 0x00020226
		public OptionSet Add(string prototype, Action<string> action)
		{
			return this.Add(prototype, null, action);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00022050 File Offset: 0x00020250
		public OptionSet Add(string prototype, string description, Action<string> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			Option item = new OptionSet.ActionOption(prototype, description, 1, delegate(OptionValueCollection v)
			{
				action(v[0]);
			});
			base.Add(item);
			return this;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0002209A File Offset: 0x0002029A
		public OptionSet Add(string prototype, OptionAction<string, string> action)
		{
			return this.Add(prototype, null, action);
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x000220C8 File Offset: 0x000202C8
		public OptionSet Add(string prototype, string description, OptionAction<string, string> action)
		{
			if (action == null)
			{
				throw new ArgumentNullException("action");
			}
			Option item = new OptionSet.ActionOption(prototype, description, 2, delegate(OptionValueCollection v)
			{
				action(v[0], v[1]);
			});
			base.Add(item);
			return this;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00022112 File Offset: 0x00020312
		public OptionSet Add<T>(string prototype, Action<T> action)
		{
			return this.Add<T>(prototype, null, action);
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0002211D File Offset: 0x0002031D
		public OptionSet Add<T>(string prototype, string description, Action<T> action)
		{
			return this.Add(new OptionSet.ActionOption<T>(prototype, description, action));
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0002212D File Offset: 0x0002032D
		public OptionSet Add<TKey, TValue>(string prototype, OptionAction<TKey, TValue> action)
		{
			return this.Add<TKey, TValue>(prototype, null, action);
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x00022138 File Offset: 0x00020338
		public OptionSet Add<TKey, TValue>(string prototype, string description, OptionAction<TKey, TValue> action)
		{
			return this.Add(new OptionSet.ActionOption<TKey, TValue>(prototype, description, action));
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00022148 File Offset: 0x00020348
		protected virtual OptionContext CreateOptionContext()
		{
			return new OptionContext(this);
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x00022150 File Offset: 0x00020350
		public List<string> Parse(IEnumerable<string> arguments)
		{
			OptionContext optionContext = this.CreateOptionContext();
			optionContext.OptionIndex = -1;
			bool flag = true;
			List<string> list = new List<string>();
			Option def = base.Contains("<>") ? base["<>"] : null;
			foreach (string text in arguments)
			{
				optionContext.OptionIndex++;
				if (text == "--")
				{
					flag = false;
				}
				else if (!flag)
				{
					OptionSet.Unprocessed(list, def, optionContext, text);
				}
				else if (!this.Parse(text, optionContext))
				{
					OptionSet.Unprocessed(list, def, optionContext, text);
				}
			}
			if (optionContext.Option != null)
			{
				optionContext.Option.Invoke(optionContext);
			}
			return list;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00022224 File Offset: 0x00020424
		private static bool Unprocessed(ICollection<string> extra, Option def, OptionContext c, string argument)
		{
			if (def == null)
			{
				extra.Add(argument);
				return false;
			}
			c.OptionValues.Add(argument);
			c.Option = def;
			c.Option.Invoke(c);
			return false;
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00022254 File Offset: 0x00020454
		protected bool GetOptionParts(string argument, out string flag, out string name, out string sep, out string value)
		{
			if (argument == null)
			{
				throw new ArgumentNullException("argument");
			}
			string text;
			value = (text = null);
			string text2;
			sep = (text2 = text);
			string text3;
			name = (text3 = text2);
			flag = text3;
			Match match = this.ValueOption.Match(argument);
			if (!match.Success)
			{
				return false;
			}
			flag = match.Groups["flag"].Value;
			name = match.Groups["name"].Value;
			if (match.Groups["sep"].Success && match.Groups["value"].Success)
			{
				sep = match.Groups["sep"].Value;
				value = match.Groups["value"].Value;
			}
			return true;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00022328 File Offset: 0x00020528
		protected virtual bool Parse(string argument, OptionContext c)
		{
			if (c.Option != null)
			{
				this.ParseValue(argument, c);
				return true;
			}
			string text;
			string text2;
			string str;
			string text3;
			if (!this.GetOptionParts(argument, out text, out text2, out str, out text3))
			{
				return false;
			}
			if (base.Contains(text2))
			{
				Option option = base[text2];
				c.OptionName = text + text2;
				c.Option = option;
				switch (option.OptionValueType)
				{
				case OptionValueType.None:
					c.OptionValues.Add(text2);
					c.Option.Invoke(c);
					break;
				case OptionValueType.Optional:
				case OptionValueType.Required:
					this.ParseValue(text3, c);
					break;
				}
				return true;
			}
			return this.ParseBool(argument, text2, c) || this.ParseBundledValue(text, string.Concat(new string[]
			{
				text2 + str + text3
			}), c);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x000223F8 File Offset: 0x000205F8
		private void ParseValue(string option, OptionContext c)
		{
			if (option != null)
			{
				foreach (string item in (c.Option.ValueSeparators != null) ? option.Split(c.Option.ValueSeparators, StringSplitOptions.None) : new string[]
				{
					option
				})
				{
					c.OptionValues.Add(item);
				}
			}
			if (c.OptionValues.Count == c.Option.MaxValueCount || c.Option.OptionValueType == OptionValueType.Optional)
			{
				c.Option.Invoke(c);
				return;
			}
			if (c.OptionValues.Count > c.Option.MaxValueCount)
			{
				throw new OptionException(this.localizer(string.Format("Error: Found {0} option values when expecting {1}.", c.OptionValues.Count, c.Option.MaxValueCount)), c.OptionName);
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x000224E0 File Offset: 0x000206E0
		private bool ParseBool(string option, string n, OptionContext c)
		{
			string key;
			if (n.Length >= 1 && (n[n.Length - 1] == '+' || n[n.Length - 1] == '-') && base.Contains(key = n.Substring(0, n.Length - 1)))
			{
				Option option2 = base[key];
				string item = (n[n.Length - 1] == '+') ? option : null;
				c.OptionName = option;
				c.Option = option2;
				c.OptionValues.Add(item);
				option2.Invoke(c);
				return true;
			}
			return false;
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00022578 File Offset: 0x00020778
		private bool ParseBundledValue(string f, string n, OptionContext c)
		{
			if (f != "-")
			{
				return false;
			}
			int i = 0;
			while (i < n.Length)
			{
				string text = f + n[i].ToString();
				string key = n[i].ToString();
				if (!base.Contains(key))
				{
					if (i == 0)
					{
						return false;
					}
					throw new OptionException(string.Format(this.localizer("Cannot bundle unregistered option '{0}'."), text), text);
				}
				else
				{
					Option option = base[key];
					switch (option.OptionValueType)
					{
					case OptionValueType.None:
						OptionSet.Invoke(c, text, n, option);
						i++;
						break;
					case OptionValueType.Optional:
					case OptionValueType.Required:
					{
						string text2 = n.Substring(i + 1);
						c.Option = option;
						c.OptionName = text;
						this.ParseValue((text2.Length != 0) ? text2 : null, c);
						return true;
					}
					default:
						throw new InvalidOperationException("Unknown OptionValueType: " + option.OptionValueType);
					}
				}
			}
			return true;
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x0002267A File Offset: 0x0002087A
		private static void Invoke(OptionContext c, string name, string value, Option option)
		{
			c.OptionName = name;
			c.Option = option;
			c.OptionValues.Add(value);
			option.Invoke(c);
		}

		// Token: 0x060008B6 RID: 2230 RVA: 0x000226A0 File Offset: 0x000208A0
		public void WriteOptionDescriptions(TextWriter o)
		{
			foreach (Option option in this)
			{
				int num = 0;
				if (this.WriteOptionPrototype(o, option, ref num))
				{
					if (num < 29)
					{
						o.Write(new string(' ', 29 - num));
					}
					else
					{
						o.WriteLine();
						o.Write(new string(' ', 29));
					}
					bool flag = false;
					string value = new string(' ', 31);
					foreach (string value2 in OptionSet.GetLines(this.localizer(OptionSet.GetDescription(option.Description))))
					{
						if (flag)
						{
							o.Write(value);
						}
						o.WriteLine(value2);
						flag = true;
					}
				}
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x0002279C File Offset: 0x0002099C
		private bool WriteOptionPrototype(TextWriter o, Option p, ref int written)
		{
			string[] names = p.Names;
			int i = OptionSet.GetNextOptionIndex(names, 0);
			if (i == names.Length)
			{
				return false;
			}
			if (names[i].Length == 1)
			{
				OptionSet.Write(o, ref written, "  -");
				OptionSet.Write(o, ref written, names[0]);
			}
			else
			{
				OptionSet.Write(o, ref written, "      --");
				OptionSet.Write(o, ref written, names[0]);
			}
			for (i = OptionSet.GetNextOptionIndex(names, i + 1); i < names.Length; i = OptionSet.GetNextOptionIndex(names, i + 1))
			{
				OptionSet.Write(o, ref written, ", ");
				OptionSet.Write(o, ref written, (names[i].Length == 1) ? "-" : "--");
				OptionSet.Write(o, ref written, names[i]);
			}
			if (p.OptionValueType == OptionValueType.Optional || p.OptionValueType == OptionValueType.Required)
			{
				if (p.OptionValueType == OptionValueType.Optional)
				{
					OptionSet.Write(o, ref written, this.localizer("["));
				}
				OptionSet.Write(o, ref written, this.localizer("=" + OptionSet.GetArgumentName(0, p.MaxValueCount, p.Description)));
				string str = (p.ValueSeparators != null && p.ValueSeparators.Length > 0) ? p.ValueSeparators[0] : " ";
				for (int j = 1; j < p.MaxValueCount; j++)
				{
					OptionSet.Write(o, ref written, this.localizer(str + OptionSet.GetArgumentName(j, p.MaxValueCount, p.Description)));
				}
				if (p.OptionValueType == OptionValueType.Optional)
				{
					OptionSet.Write(o, ref written, this.localizer("]"));
				}
			}
			return true;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x0002292B File Offset: 0x00020B2B
		private static int GetNextOptionIndex(string[] names, int i)
		{
			while (i < names.Length && names[i] == "<>")
			{
				i++;
			}
			return i;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0002294A File Offset: 0x00020B4A
		private static void Write(TextWriter o, ref int n, string s)
		{
			n += s.Length;
			o.Write(s);
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00022960 File Offset: 0x00020B60
		private static string GetArgumentName(int index, int maxIndex, string description)
		{
			if (description == null)
			{
				if (maxIndex != 1)
				{
					return "VALUE" + (index + 1);
				}
				return "VALUE";
			}
			else
			{
				string[] array;
				if (maxIndex == 1)
				{
					array = new string[]
					{
						"{0:",
						"{"
					};
				}
				else
				{
					array = new string[]
					{
						"{" + index + ":"
					};
				}
				for (int i = 0; i < array.Length; i++)
				{
					int num = 0;
					int num2;
					do
					{
						num2 = description.IndexOf(array[i], num);
					}
					while (num2 >= 0 && num != 0 && description[num++ - 1] == '{');
					if (num2 != -1)
					{
						int num3 = description.IndexOf("}", num2);
						if (num3 != -1)
						{
							return description.Substring(num2 + array[i].Length, num3 - num2 - array[i].Length);
						}
					}
				}
				if (maxIndex != 1)
				{
					return "VALUE" + (index + 1);
				}
				return "VALUE";
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00022A60 File Offset: 0x00020C60
		private static string GetDescription(string description)
		{
			if (description == null)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(description.Length);
			int num = -1;
			int i = 0;
			while (i < description.Length)
			{
				char c = description[i];
				if (c != ':')
				{
					switch (c)
					{
					case '{':
						if (i == num)
						{
							stringBuilder.Append('{');
							num = -1;
						}
						else if (num < 0)
						{
							num = i + 1;
						}
						break;
					case '|':
						goto IL_BE;
					case '}':
						if (num < 0)
						{
							if (i + 1 == description.Length || description[i + 1] != '}')
							{
								throw new InvalidOperationException("Invalid option description: " + description);
							}
							i++;
							stringBuilder.Append("}");
						}
						else
						{
							stringBuilder.Append(description.Substring(num, i - num));
							num = -1;
						}
						break;
					default:
						goto IL_BE;
					}
				}
				else
				{
					if (num < 0)
					{
						goto IL_BE;
					}
					num = i + 1;
				}
				IL_D0:
				i++;
				continue;
				IL_BE:
				if (num < 0)
				{
					stringBuilder.Append(description[i]);
					goto IL_D0;
				}
				goto IL_D0;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00022D5C File Offset: 0x00020F5C
		private static IEnumerable<string> GetLines(string description)
		{
			if (string.IsNullOrEmpty(description))
			{
				yield return string.Empty;
			}
			else
			{
				int length = 50;
				int start = 0;
				int end;
				do
				{
					end = OptionSet.GetLineEnd(start, length, description);
					char c = description[end - 1];
					if (char.IsWhiteSpace(c))
					{
						end--;
					}
					bool writeContinuation = end != description.Length && !OptionSet.IsEolChar(c);
					string line = description.Substring(start, end - start) + (writeContinuation ? "-" : "");
					yield return line;
					start = end;
					if (char.IsWhiteSpace(c))
					{
						start++;
					}
					length = 48;
				}
				while (end < description.Length);
			}
			yield break;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00022D79 File Offset: 0x00020F79
		private static bool IsEolChar(char c)
		{
			return !char.IsLetterOrDigit(c);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00022D84 File Offset: 0x00020F84
		private static int GetLineEnd(int start, int length, string description)
		{
			int num = Math.Min(start + length, description.Length);
			int num2 = -1;
			for (int i = start; i < num; i++)
			{
				if (description[i] == '\n')
				{
					return i + 1;
				}
				if (OptionSet.IsEolChar(description[i]))
				{
					num2 = i + 1;
				}
			}
			if (num2 == -1 || num == description.Length)
			{
				return num;
			}
			return num2;
		}

		// Token: 0x040002BC RID: 700
		private const int OptionWidth = 29;

		// Token: 0x040002BD RID: 701
		private Converter<string, string> localizer;

		// Token: 0x040002BE RID: 702
		private readonly Regex ValueOption = new Regex("^(?<flag>--|-|/)(?<name>[^:=]+)((?<sep>[:=])(?<value>.*))?$");

		// Token: 0x020000F6 RID: 246
		private sealed class ActionOption : Option
		{
			// Token: 0x060008C0 RID: 2240 RVA: 0x00022DDF File Offset: 0x00020FDF
			public ActionOption(string prototype, string description, int count, Action<OptionValueCollection> action) : base(prototype, description, count)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				this.action = action;
			}

			// Token: 0x060008C1 RID: 2241 RVA: 0x00022E01 File Offset: 0x00021001
			protected override void OnParseComplete(OptionContext c)
			{
				this.action(c.OptionValues);
			}

			// Token: 0x040002C0 RID: 704
			private Action<OptionValueCollection> action;
		}

		// Token: 0x020000F7 RID: 247
		private sealed class ActionOption<T> : Option
		{
			// Token: 0x060008C2 RID: 2242 RVA: 0x00022E14 File Offset: 0x00021014
			public ActionOption(string prototype, string description, Action<T> action) : base(prototype, description, 1)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				this.action = action;
			}

			// Token: 0x060008C3 RID: 2243 RVA: 0x00022E34 File Offset: 0x00021034
			protected override void OnParseComplete(OptionContext c)
			{
				this.action(Option.Parse<T>(c.OptionValues[0], c));
			}

			// Token: 0x040002C1 RID: 705
			private Action<T> action;
		}

		// Token: 0x020000F8 RID: 248
		private sealed class ActionOption<TKey, TValue> : Option
		{
			// Token: 0x060008C4 RID: 2244 RVA: 0x00022E53 File Offset: 0x00021053
			public ActionOption(string prototype, string description, OptionAction<TKey, TValue> action) : base(prototype, description, 2)
			{
				if (action == null)
				{
					throw new ArgumentNullException("action");
				}
				this.action = action;
			}

			// Token: 0x060008C5 RID: 2245 RVA: 0x00022E73 File Offset: 0x00021073
			protected override void OnParseComplete(OptionContext c)
			{
				this.action(Option.Parse<TKey>(c.OptionValues[0], c), Option.Parse<TValue>(c.OptionValues[1], c));
			}

			// Token: 0x040002C2 RID: 706
			private OptionAction<TKey, TValue> action;
		}
	}
}
