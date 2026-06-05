using System;
using System.Collections.Generic;
using System.Text;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// Builds a process argument string.
	/// </summary>
	// Token: 0x02000212 RID: 530
	public class ProcessArgumentBuilder
	{
		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x060013EA RID: 5098 RVA: 0x00052D61 File Offset: 0x00050F61
		// (set) Token: 0x060013EB RID: 5099 RVA: 0x00052D69 File Offset: 0x00050F69
		public string ProcessPath { get; private set; }

		// Token: 0x060013EC RID: 5100 RVA: 0x00052D72 File Offset: 0x00050F72
		public ProcessArgumentBuilder()
		{
		}

		// Token: 0x060013ED RID: 5101 RVA: 0x00052D85 File Offset: 0x00050F85
		public ProcessArgumentBuilder(string processPath)
		{
			this.ProcessPath = processPath;
		}

		/// <summary>
		/// Adds an argument without escaping or quoting.
		/// </summary>
		// Token: 0x060013EE RID: 5102 RVA: 0x00052D9F File Offset: 0x00050F9F
		public void Add(string argument)
		{
			if (this.sb.Length > 0)
			{
				this.sb.Append(' ');
			}
			this.sb.Append(argument);
		}

		/// <summary>
		/// Adds multiple arguments without escaping or quoting.
		/// </summary>
		// Token: 0x060013EF RID: 5103 RVA: 0x00052DCC File Offset: 0x00050FCC
		public void Add(params string[] args)
		{
			foreach (string argument in args)
			{
				this.Add(argument);
			}
		}

		/// <summary>
		/// Adds a formatted argument, quoting and escaping as necessary.
		/// </summary>
		// Token: 0x060013F0 RID: 5104 RVA: 0x00052DF4 File Offset: 0x00050FF4
		public void AddQuotedFormat(string argumentFormat, params object[] values)
		{
			this.AddQuoted(string.Format(argumentFormat, values));
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00052E03 File Offset: 0x00051003
		public void AddQuotedFormat(string argumentFormat, object val0)
		{
			this.AddQuoted(string.Format(argumentFormat, val0));
		}

		/// <summary>Adds an argument, quoting and escaping as necessary.</summary>
		/// <remarks>The .NET process class does not support escaped 
		/// arguments, only quoted arguments with escaped quotes.</remarks>
		// Token: 0x060013F2 RID: 5106 RVA: 0x00052E14 File Offset: 0x00051014
		public void AddQuoted(string argument)
		{
			if (argument == null)
			{
				return;
			}
			if (this.sb.Length > 0)
			{
				this.sb.Append(' ');
			}
			this.sb.Append('"');
			ProcessArgumentBuilder.AppendEscaped(this.sb, "\\\"", argument);
			this.sb.Append('"');
		}

		/// <summary>
		/// Adds multiple arguments, quoting and escaping each as necessary.
		/// </summary>
		// Token: 0x060013F3 RID: 5107 RVA: 0x00052E70 File Offset: 0x00051070
		public void AddQuoted(params string[] args)
		{
			foreach (string argument in args)
			{
				this.AddQuoted(argument);
			}
		}

		/// <summary>Quotes a string, escaping if necessary.</summary>
		/// <remarks>The .NET process class does not support escaped 
		/// arguments, only quoted arguments with escaped quotes.</remarks>
		// Token: 0x060013F4 RID: 5108 RVA: 0x00052E98 File Offset: 0x00051098
		public static string Quote(string s)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('"');
			ProcessArgumentBuilder.AppendEscaped(stringBuilder, "\\\"", s);
			stringBuilder.Append('"');
			return stringBuilder.ToString();
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x00052ECF File Offset: 0x000510CF
		public override string ToString()
		{
			return this.sb.ToString();
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x00052EDC File Offset: 0x000510DC
		private static void AppendEscaped(StringBuilder sb, string escapeChars, string s)
		{
			foreach (char value in s)
			{
				if (escapeChars.IndexOf(value) > -1)
				{
					sb.Append('\\');
				}
				sb.Append(value);
			}
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x00052F20 File Offset: 0x00051120
		private static string GetArgument(StringBuilder builder, string buf, int startIndex, out int endIndex, out Exception ex)
		{
			bool flag = false;
			char c = '\0';
			int i = startIndex;
			builder.Clear();
			char c2 = buf[startIndex];
			char c3;
			if (c2 != '"')
			{
				if (c2 == '\'')
				{
					c3 = '\'';
					i++;
				}
				else
				{
					c3 = '\0';
				}
			}
			else
			{
				c3 = '"';
				i++;
			}
			while (i < buf.Length)
			{
				c = buf[i];
				if (c == c3 && !flag)
				{
					i++;
					break;
				}
				if (c == '\\')
				{
					flag = true;
				}
				else if (flag)
				{
					builder.Append(c);
					flag = false;
				}
				else
				{
					if (c3 == '\0' && (c == ' ' || c == '\t'))
					{
						break;
					}
					if (c3 == '\0' && (c == '\'' || c == '"'))
					{
						string value = builder.ToString();
						string argument;
						if ((argument = ProcessArgumentBuilder.GetArgument(builder, buf, i, out endIndex, out ex)) == null)
						{
							return null;
						}
						i = endIndex;
						builder.Clear();
						builder.Append(value);
						builder.Append(argument);
						continue;
					}
					else
					{
						builder.Append(c);
					}
				}
				i++;
			}
			if (flag || (c3 != '\0' && c != c3))
			{
				ex = new FormatException(flag ? "Incomplete escape sequence." : "No matching quote found.");
				endIndex = -1;
				return null;
			}
			endIndex = i;
			ex = null;
			return builder.ToString();
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x0005303C File Offset: 0x0005123C
		private static bool TryParse(string commandline, out string[] argv, out Exception ex)
		{
			StringBuilder builder = new StringBuilder();
			List<string> list = new List<string>();
			for (int i = 0; i < commandline.Length; i++)
			{
				char c = commandline[i];
				if (c != ' ' && c != '\t')
				{
					int num;
					string argument;
					if ((argument = ProcessArgumentBuilder.GetArgument(builder, commandline, i, out num, out ex)) == null)
					{
						argv = null;
						return false;
					}
					list.Add(argument);
					i = num;
				}
			}
			argv = list.ToArray();
			ex = null;
			return true;
		}

		// Token: 0x060013F9 RID: 5113 RVA: 0x000530A8 File Offset: 0x000512A8
		public static bool TryParse(string commandline, out string[] argv)
		{
			Exception ex;
			return ProcessArgumentBuilder.TryParse(commandline, out argv, out ex);
		}

		// Token: 0x060013FA RID: 5114 RVA: 0x000530C0 File Offset: 0x000512C0
		public static string[] Parse(string commandline)
		{
			string[] result;
			Exception ex;
			if (!ProcessArgumentBuilder.TryParse(commandline, out result, out ex))
			{
				throw ex;
			}
			return result;
		}

		// Token: 0x040005FC RID: 1532
		private const string escapeDoubleQuoteCharsStr = "\\\"";

		// Token: 0x040005FD RID: 1533
		private StringBuilder sb = new StringBuilder();
	}
}
