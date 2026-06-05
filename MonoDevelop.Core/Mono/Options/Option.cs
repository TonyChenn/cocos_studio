using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Mono.Options
{
	// Token: 0x020000F2 RID: 242
	public abstract class Option
	{
		// Token: 0x06000881 RID: 2177 RVA: 0x0002193E File Offset: 0x0001FB3E
		protected Option(string prototype, string description) : this(prototype, description, 1)
		{
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x0002194C File Offset: 0x0001FB4C
		protected Option(string prototype, string description, int maxValueCount)
		{
			if (prototype == null)
			{
				throw new ArgumentNullException("prototype");
			}
			if (prototype.Length == 0)
			{
				throw new ArgumentException("Cannot be the empty string.", "prototype");
			}
			if (maxValueCount < 0)
			{
				throw new ArgumentOutOfRangeException("maxValueCount");
			}
			this.prototype = prototype;
			this.names = prototype.Split(new char[]
			{
				'|'
			});
			this.description = description;
			this.count = maxValueCount;
			this.type = this.ParsePrototype();
			if (this.count == 0 && this.type != OptionValueType.None)
			{
				throw new ArgumentException("Cannot provide maxValueCount of 0 for OptionValueType.Required or OptionValueType.Optional.", "maxValueCount");
			}
			if (this.type == OptionValueType.None && maxValueCount > 1)
			{
				throw new ArgumentException(string.Format("Cannot provide maxValueCount of {0} for OptionValueType.None.", maxValueCount), "maxValueCount");
			}
			if (Array.IndexOf<string>(this.names, "<>") >= 0 && ((this.names.Length == 1 && this.type != OptionValueType.None) || (this.names.Length > 1 && this.MaxValueCount > 1)))
			{
				throw new ArgumentException("The default option handler '<>' cannot require values.", "prototype");
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00021A5F File Offset: 0x0001FC5F
		public string Prototype
		{
			get
			{
				return this.prototype;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00021A67 File Offset: 0x0001FC67
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00021A6F File Offset: 0x0001FC6F
		public OptionValueType OptionValueType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00021A77 File Offset: 0x0001FC77
		public int MaxValueCount
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00021A7F File Offset: 0x0001FC7F
		public string[] GetNames()
		{
			return (string[])this.names.Clone();
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00021A91 File Offset: 0x0001FC91
		public string[] GetValueSeparators()
		{
			if (this.separators == null)
			{
				return new string[0];
			}
			return (string[])this.separators.Clone();
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00021AB4 File Offset: 0x0001FCB4
		protected static T Parse<T>(string value, OptionContext c)
		{
			Type typeFromHandle = typeof(T);
			Type type = (typeFromHandle.IsValueType && typeFromHandle.IsGenericType && !typeFromHandle.IsGenericTypeDefinition && typeFromHandle.GetGenericTypeDefinition() == typeof(Nullable<>)) ? typeFromHandle.GetGenericArguments()[0] : typeof(T);
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			T result = default(T);
			try
			{
				if (value != null)
				{
					result = (T)((object)converter.ConvertFromString(value));
				}
			}
			catch (Exception innerException)
			{
				throw new OptionException(string.Format(c.OptionSet.MessageLocalizer("Could not convert string `{0}' to type {1} for option `{2}'."), value, type.Name, c.OptionName), c.OptionName, innerException);
			}
			return result;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00021B80 File Offset: 0x0001FD80
		internal string[] Names
		{
			get
			{
				return this.names;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00021B88 File Offset: 0x0001FD88
		internal string[] ValueSeparators
		{
			get
			{
				return this.separators;
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00021B90 File Offset: 0x0001FD90
		private OptionValueType ParsePrototype()
		{
			char c = '\0';
			List<string> list = new List<string>();
			for (int i = 0; i < this.names.Length; i++)
			{
				string text = this.names[i];
				if (text.Length == 0)
				{
					throw new ArgumentException("Empty option names are not supported.", "prototype");
				}
				int num = text.IndexOfAny(Option.NameTerminator);
				if (num != -1)
				{
					this.names[i] = text.Substring(0, num);
					if (c != '\0' && c != text[num])
					{
						throw new ArgumentException(string.Format("Conflicting option types: '{0}' vs. '{1}'.", c, text[num]), "prototype");
					}
					c = text[num];
					Option.AddSeparators(text, num, list);
				}
			}
			if (c == '\0')
			{
				return OptionValueType.None;
			}
			if (this.count <= 1 && list.Count != 0)
			{
				throw new ArgumentException(string.Format("Cannot provide key/value separators for Options taking {0} value(s).", this.count), "prototype");
			}
			if (this.count > 1)
			{
				if (list.Count == 0)
				{
					this.separators = new string[]
					{
						":",
						"="
					};
				}
				else if (list.Count == 1 && list[0].Length == 0)
				{
					this.separators = null;
				}
				else
				{
					this.separators = list.ToArray();
				}
			}
			if (c != '=')
			{
				return OptionValueType.Optional;
			}
			return OptionValueType.Required;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00021CEC File Offset: 0x0001FEEC
		private static void AddSeparators(string name, int end, ICollection<string> seps)
		{
			int num = -1;
			int i = end + 1;
			while (i < name.Length)
			{
				switch (name[i])
				{
				case '{':
					if (num != -1)
					{
						throw new ArgumentException(string.Format("Ill-formed name/value separator found in \"{0}\".", name), "prototype");
					}
					num = i + 1;
					break;
				case '|':
					goto IL_78;
				case '}':
					if (num == -1)
					{
						throw new ArgumentException(string.Format("Ill-formed name/value separator found in \"{0}\".", name), "prototype");
					}
					seps.Add(name.Substring(num, i - num));
					num = -1;
					break;
				default:
					goto IL_78;
				}
				IL_91:
				i++;
				continue;
				IL_78:
				if (num == -1)
				{
					seps.Add(name[i].ToString());
					goto IL_91;
				}
				goto IL_91;
			}
			if (num != -1)
			{
				throw new ArgumentException(string.Format("Ill-formed name/value separator found in \"{0}\".", name), "prototype");
			}
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00021DB4 File Offset: 0x0001FFB4
		public void Invoke(OptionContext c)
		{
			this.OnParseComplete(c);
			c.OptionName = null;
			c.Option = null;
			c.OptionValues.Clear();
		}

		// Token: 0x0600088F RID: 2191
		protected abstract void OnParseComplete(OptionContext c);

		// Token: 0x06000890 RID: 2192 RVA: 0x00021DD6 File Offset: 0x0001FFD6
		public override string ToString()
		{
			return this.Prototype;
		}

		// Token: 0x040002B4 RID: 692
		private string prototype;

		// Token: 0x040002B5 RID: 693
		private string description;

		// Token: 0x040002B6 RID: 694
		private string[] names;

		// Token: 0x040002B7 RID: 695
		private OptionValueType type;

		// Token: 0x040002B8 RID: 696
		private int count;

		// Token: 0x040002B9 RID: 697
		private string[] separators;

		// Token: 0x040002BA RID: 698
		private static readonly char[] NameTerminator = new char[]
		{
			'=',
			':'
		};
	}
}
