using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Gdk;
using ICSharpCode.NRefactory.Completion;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Projects;

namespace CocoStudio.LuaBinding
{
	public class LuaTextEditorCompletion : CompletionTextEditorExtension
	{
		private string[] Globals = new string[151]
		{
			"_G\tand", "_G\tbreak", "_G\tdo", "_G\telse", "_G\telseif", "_G\tend", "_G\tfalse", "_G\tfor", "_G\tfunction", "_G\tgoto",
			"_G\tif", "_G\tin", "_G\tlocal", "_G\tnil", "_G\tnot", "_G\tor", "_G\trepeat", "_G\treturn", "_G\tthen", "_G\ttrue",
			"_G\tuntil", "_G\twhile", "_G\tassert\t(value [, message])", "_G\tcollectgarbage\t(opt [, arg])", "_G\tdofile\t(filename)", "_G\terror\t(message [, level])", "_G\tgetmetatable\t(object)", "_G\tipairs(\t(table)", "_G\tload\t(func [, chunkname])", "_G\tloadfile\t([filename])",
			"_G\tloadstring\t(string [, chunkname])", "_G\tnext\t(table [, index])", "_G\tpairs\t(table)", "_G\tpcall\t(func, ...)", "_G\tprint\t(...)", "_G\trawequal\t(v1, v2)", "_G\trawget\t(table, index)", "_G\trawset\t(table, index, value)", "_G\tselect\t(index, ...)", "_G\tsetmetatable\t(table, metatable)",
			"_G\ttonumber\t(value [, base])", "_G\ttostring\t(value)", "_G\ttype\t(value)", "_G\tunpack\t(list [, i [, j]])", "_G\txpcall\t(func, err)", "_G\t_G\t{}", "_G\t_VERSION\t#", "_G\tcoroutine\t{}", "coroutine\tcreate\t(func)", "coroutine\tresume\t(co [, val1, ...])",
			"coroutine\trunning\t()", "coroutine\tstatus\t(co)", "coroutine\twrap\t(func)", "coroutine\tyield\t(...)", "_G\tdebug\t{}", "debug\tdebug\t()", "debug\tgethook\t([thread])", "debug\tgetinfo\t([thread,] function [, what])", "debug\tgetlocal\t([thread,] level, local)", "debug\tgetmetatable\t(object)",
			"debug\tgetregistry\t()", "debug\tgetupvalue\t(func, up)", "debug\tsethook\t([thread,] hook, mask [, count])", "debug\tsetlocal\t([thread,] level, local, value)", "debug\tsetmetatable\t(object, table)", "debug\tsetupvalue\t(func, up, value)", "debug\ttraceback\t([thread,] [message] [, level])", "_G\tio\t{}", "io\tclose\t([file])", "io\tflush\t()",
			"io\tinput\t([file])", "io\tlines\t([filename])", "io\topen\t(filename [, mode])", "io\toutput\t([file])", "io\tpopen\t(prog [, mode])", "io\tread\t(...)", "io\ttempfile\t()", "io\ttype\t(obj)", "io\twrite\t(...)", "_G\tmath\t{}",
			"math\tabs\t(x)", "math\tacos\t(x)", "math\tatan\t(x)", "math\tatan2\t(x, y)", "math\tceil\t(x)", "math\tcosh\t(x)", "math\tdeg\t(x)", "math\texp\t(x)", "math\tfloor\t(x)", "math\tfmod\t(x, y)",
			"math\tfrexp\t(x)", "math\thuge\t#", "math\tldexp\t(m, e)", "math\tlog\t(x)", "math\tlog10\t(x)", "math\tmax\t(x, ...)", "math\tmin\t(x, ...)", "math\tmodf\t(x)", "math\tpi\t#", "math\tpow\t(x, y)",
			"math\trad\t(x)", "math\trandom\t([m [, n]])", "math\trandomseed\t(x)", "math\tsin\t(x)", "math\tsinh\t(x)", "math\tsqrt\t(x)", "math\ttan\t(x)", "math\ttanh\t(x)", "_G\tos\t{}", "os\tclock\t()",
			"os\tdate\t([format [, time]])", "os\tdifftime\t(t2, t1)", "os\texecute\t([command])", "os\texit\t([code])", "os\tgetenv\t(varname)", "os\tremove\t(filename)", "os\trename\t(oldname, newname)", "os\tsetlocale\t(locale [, category])", "os\ttime\t([table])", "os\ttmpname\t()",
			"_G\tpackage\t{}", "_G\tmodule\t(name [, ...])", "_G\trequire\t(modname)", "package\tcpath\t#", "package\tloaded\t#", "package\tloaders\t#", "package\tloadlib\t(libname, funcname)", "package\tpath\t#", "package\tpreload\t#", "package\tseeall\t(module)",
			"_G\tstring\t{}", "string\tbyte\t(string [, from [, to]])", "string\tchar\t(...)", "string\tdump\t(function)", "string\tfind\t(string, pattern [, init [, plain]])", "string\tformat\t(string, ...)", "string\tgmatch\t(string, pattern)", "string\tgsub\t(string, pattern, repl [, n])", "string\tlen\t(string)", "string\tlower\t(string)",
			"string\tmatch\t(string, pattern [, init])", "string\trep\t(string, count)", "string\treverse\t(string)", "string\tsub\t(string, from [, to])", "string\tupper\t(string)", "_G\ttable\t{}", "table\tconcat\t(table [, sep [, i [, j]]])", "table\tinsert\t(table, [pos,] value)", "table\tmaxn\t(table)", "table\tremove\t(table [, pos])",
			"table\tsort\t(table [, comp])"
		};

		private string[] ProjectGlobals = new string[0];

		private DateTime? LastModified = null;

		private readonly Regex rx_is_local = new Regex("^\\s*(local|for)\\s+((([A-z_][A-z0-9_]*))(\\s*,\\s*([A-z_][A-z0-9_]*))*)?\\s*$", RegexOptions.Compiled);

		private readonly Regex rx_is_function = new Regex("^.*(local\\s+)?function(\\s+([A-z0-9_\\.]*))?\\s*(\\([A-z0-9_, ]*)?$", RegexOptions.Compiled);

		private readonly Regex rx_is_comment = new Regex("^.*--.*$", RegexOptions.Compiled);

		private readonly Regex rx_in_string = new Regex("(?<!\\\\)\"", RegexOptions.Compiled);

		private readonly Regex rx_is_number = new Regex("(?<![A-z_][0-9\\.]*)[0-9\\.]+$", RegexOptions.Compiled);

		private bool wasnil = true;

		private readonly Regex rx_locals = new Regex("(?<tabs>[ \\t]*)local\\s+(function\\s+(?<func_name>[A-z_][A-z0-9_]*)\\s*\\((?<func_args>.*)\\)|(?<vars>([A-z_][A-z0-9_]*))(\\s*,\\s*([A-z_][A-z0-9_]*))*)", RegexOptions.Compiled);

		private readonly Regex rx_args_and_for = new Regex("(?<tabs>[ \\t]*)((local\\s+)?function\\s+([A-z][A-z0-9]+)?\\((?<vars>.+)\\)|for\\s+(?<vars>.+)\\s+(in\\s+|=))", RegexOptions.Compiled);

		private Regex rx_is_keyword = new Regex("(and|break|do|else|elseif|end|for|function|if|local|nil|not|or|repeat|return|then|until|while)$", RegexOptions.Compiled);

		private static bool ValidVarnameChar(char x)
		{
			if (!char.IsLetterOrDigit(x))
			{
				return x == '_';
			}
			return true;
		}

		private void UpdateProjectGlobals()
		{
			if (!document.HasProject)
			{
				return;
			}
			Project project = document.Project;
			string path = project.GetAbsoluteChildPath(".project_globals");
			if (!File.Exists(path))
			{
				return;
			}
			if (LastModified.HasValue)
			{
				DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc(path);
				DateTime? lastModified = LastModified;
				if (!(lastWriteTimeUtc > lastModified))
				{
					return;
				}
			}
			LastModified = File.GetLastWriteTimeUtc(path);
			ProjectGlobals = File.ReadAllLines(path);
		}

		public override bool CanRunCompletionCommand()
		{
			string lineText = base.Editor.GetLineText(base.Editor.Caret.Line);
			string input = lineText.Substring(0, Math.Min(base.Editor.Caret.Column - 1, lineText.Length));
			if (rx_is_local.IsMatch(input) || rx_is_function.IsMatch(input))
			{
				return false;
			}
			if (rx_is_comment.IsMatch(input))
			{
				return false;
			}
			MatchCollection matchCollection = rx_in_string.Matches(input);
			if (matchCollection.Count % 2 == 1)
			{
				return false;
			}
			if (rx_is_number.IsMatch(input))
			{
				return false;
			}
			return true;
		}

		public override ICompletionDataList HandleCodeCompletion(CodeCompletionContext completionContext, char completionChar, ref int triggerWordLength)
		{
			if (!CanRunCompletionCommand())
			{
				return null;
			}
			if (completionChar == '(' || completionChar == ')' || completionChar == '[' || completionChar == ']' || completionChar == '{' || completionChar == '}' || completionChar == '"' || completionChar == '\'' || completionChar == ';' || completionChar == '=' || completionChar == ' ' || completionChar == '\t' || completionChar == ',')
			{
				_ = 40;
				return null;
			}
			CompletionDataList ret = new CompletionDataList();
			string fullcontext = "";
			bool flag = false;
			if (completionContext.TriggerOffset > 1)
			{
				completionContext.TriggerOffset = document.Editor.Caret.Offset;
				int num = completionContext.TriggerOffset - 1;
				while (num > 1)
				{
					char charAt = document.Editor.GetCharAt(num);
					num--;
					if (charAt == '.' || charAt == ':' || charAt == ',' || !ValidVarnameChar(charAt))
					{
						if (charAt == '.' || charAt == ':')
						{
							flag = true;
						}
						break;
					}
				}
				triggerWordLength = completionContext.TriggerOffset - num - 2;
				completionContext.TriggerWordLength = triggerWordLength;
				bool flag2 = false;
				bool flag3 = false;
				while (num > 1 && flag)
				{
					char charAt2 = document.Editor.GetCharAt(num);
					if (char.IsWhiteSpace(charAt2))
					{
						flag2 = true;
					}
					else
					{
						if (flag2 && !flag3 && charAt2 != '.' && charAt2 != ':')
						{
							break;
						}
						if (charAt2 == '.' || charAt2 == ':')
						{
							flag3 = true;
							fullcontext = charAt2 + fullcontext;
						}
						else
						{
							if (!ValidVarnameChar(charAt2))
							{
								break;
							}
							flag3 = false;
							flag2 = false;
							fullcontext = charAt2 + fullcontext;
						}
					}
					num--;
				}
			}
			if (completionChar == '.' && string.IsNullOrWhiteSpace(fullcontext.Trim(".".ToCharArray())))
			{
				return null;
			}
			if (fullcontext.Trim() == "")
			{
				fullcontext = "_G";
			}
			else
			{
				fullcontext = fullcontext.TrimEnd(".".ToCharArray());
				if (fullcontext.StartsWith("_G."))
				{
					fullcontext = fullcontext.Substring("_G.".Length);
				}
			}
			CompletionCategory cat = null;
			Action<string> action = delegate(string line)
			{
				if (!line.Trim().StartsWith("#") && !string.IsNullOrWhiteSpace(line))
				{
					string[] array = line.Split("\t".ToCharArray());
					string text = ((array.Length >= 1) ? array[0] : "");
					string text2 = ((array.Length >= 2) ? array[1] : "");
					string text3 = ((array.Length >= 3) ? array[2] : "");
					if (text == fullcontext)
					{
						string text4 = Stock.Method;
						string text5 = text3;
						switch (text5)
						{
						case "":
							text4 = "md-keyword";
							text5 = "";
							break;
						case "{}":
							text4 = Stock.NameSpace;
							text5 = "";
							break;
						case "#":
							text4 = Stock.Literal;
							text5 = "";
							break;
						}
						CompletionData completionData = ret.Add(text2 + text5, text4, "", text2);
						completionData.CompletionCategory = cat;
					}
				}
			};
			UpdateProjectGlobals();
			cat = new GlobalCompletionCategory();
			string[] globals = Globals;
			foreach (string obj in globals)
			{
				action(obj);
			}
			string[] projectGlobals = ProjectGlobals;
			foreach (string obj2 in projectGlobals)
			{
				action(obj2);
			}
			cat = new LocalCompletionCategory();
			cat.DisplayText = "Locals";
			foreach (string documentLocal in GetDocumentLocals())
			{
				action(documentLocal);
			}
			return ret;
		}

		public override void RunCompletionCommand()
		{
			wasnil = false;
			base.RunCompletionCommand();
		}

		public override ParameterDataProvider HandleParameterCompletion(CodeCompletionContext completionContext, char completionChar)
		{
			if (wasnil && completionChar == '\0')
			{
				return null;
			}
			ParameterDataProvider parameterDataProvider = _HandleParameterCompletion(completionContext, completionChar);
			wasnil = parameterDataProvider == null;
			return parameterDataProvider;
		}

		private ParameterDataProvider _HandleParameterCompletion(CodeCompletionContext completionContext, char completionChar)
		{
			int triggerLine = completionContext.TriggerLine;
			int triggerLineOffset = completionContext.TriggerLineOffset;
			string lineText = base.Document.Editor.GetLineText(triggerLine);
			lineText = lineText.Substring(0, Math.Min(triggerLineOffset, lineText.Length));
			int num = lineText.Length - 1;
			int num2 = 0;
			bool flag = false;
			while (num > 0)
			{
				switch (lineText[num])
				{
				case ')':
					num2++;
					break;
				case '(':
					num2--;
					flag = num2 < 0;
					break;
				}
				if (flag)
				{
					break;
				}
				num--;
			}
			if (num <= 0)
			{
				return null;
			}
			lineText = lineText.Substring(0, num);
			bool flag2 = true;
			string name = "";
			string context = "";
			int length = lineText.Length;
			while (length-- > 0)
			{
				char c = lineText[length];
				if (flag2)
				{
					if (c == '.' || c == ':')
					{
						flag2 = false;
						continue;
					}
					if (!ValidVarnameChar(c))
					{
						break;
					}
					name = c + name;
				}
				else
				{
					if (c != '.' && c != ':' && !ValidVarnameChar(c))
					{
						break;
					}
					context = c + context;
				}
			}
			if (context == "")
			{
				context = "_G";
			}
			string args = "";
			Action<string> action = delegate(string line2)
			{
				if (!line2.Trim().StartsWith("#") && !string.IsNullOrWhiteSpace(line2))
				{
					string[] array = line2.Split("\t".ToCharArray());
					string text = ((array.Length >= 1) ? array[0] : "");
					string text2 = ((array.Length >= 2) ? array[1] : "");
					string text3 = ((array.Length >= 3) ? array[2] : "");
					if (text == context && text2 == name)
					{
						args = text3;
					}
				}
			};
			UpdateProjectGlobals();
			string[] globals = Globals;
			foreach (string obj in globals)
			{
				action(obj);
			}
			string[] projectGlobals = ProjectGlobals;
			foreach (string obj2 in projectGlobals)
			{
				action(obj2);
			}
			foreach (string documentLocal in GetDocumentLocals())
			{
				action(documentLocal);
			}
			if (args == "")
			{
				return null;
			}
			args = args.Trim("()".ToCharArray());
			return new LuaParameterDataProvider(name, args);
		}

		public override ICompletionDataList CodeCompletionCommand(CodeCompletionContext completionContext)
		{
			int triggerWordLength = 0;
			return HandleCodeCompletion(completionContext, '\0', ref triggerWordLength);
		}

		private List<string> GetDocumentLocals()
		{
			List<string> list = new List<string>();
			string textBetween = base.Editor.GetTextBetween(0, base.Editor.Caret.Offset);
			MatchCollection matchCollection = rx_locals.Matches(textBetween);
			Match[] array = new Match[matchCollection.Count];
			int num = 1;
			foreach (Match item in matchCollection)
			{
				array[array.Length - num] = item;
				num++;
			}
			int num2 = -1;
			Match[] array2 = array;
			foreach (Match match2 in array2)
			{
				int length = match2.Groups["tabs"].Value.Replace("\t", "    ").Length;
				if (num2 == -1 || length < num2)
				{
					num2 = length;
				}
				else if (length > num2)
				{
					continue;
				}
				if (match2.Groups["vars"].Success)
				{
					string value = match2.Groups["vars"].Value;
					string[] array3 = value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
					foreach (string text in array3)
					{
						list.Add($"_G\t{text.Trim()}\t#");
					}
				}
				else
				{
					string value2 = match2.Groups["func_name"].Value;
					string value3 = match2.Groups["func_args"].Value;
					if (!string.IsNullOrWhiteSpace(value2))
					{
						list.Add($"_G\t{value2.Trim()}\t({value3.Trim()})");
					}
				}
			}
			MatchCollection matchCollection2 = rx_args_and_for.Matches(textBetween);
			Match[] array4 = new Match[matchCollection2.Count];
			int num3 = 1;
			foreach (Match item2 in matchCollection2)
			{
				array4[array4.Length - num3] = item2;
				num3++;
			}
			int num4 = -1;
			Match[] array5 = array4;
			foreach (Match match4 in array5)
			{
				int num5 = match4.Groups["tabs"].Value.Replace("\t", "    ").Length + 4;
				if (num4 == -1 || num5 < num4)
				{
					num4 = num5;
				}
				else if (num5 > num4)
				{
					continue;
				}
				string[] array6 = match4.Groups["vars"].Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
				if (array6.Length > 0)
				{
					string[] array7 = array6;
					foreach (string text2 in array7)
					{
						list.Add($"_G\t{text2.Trim()}\t#");
					}
				}
			}
			return list.Distinct().ToList();
		}

		public override bool KeyPress(Key key, char keyChar, ModifierType modifier)
		{
			if (keyChar == ' ' && modifier == ModifierType.None && base.CompletionWidget != null)
			{
				CompletionWindowManager.PreProcessKeyEvent(Key.Tab, '\t', ModifierType.None);
				CompletionWindowManager.PostProcessKeyEvent(Key.Tab, '\t', ModifierType.None);
				base.CompletionWidget.CurrentCodeCompletionContext.TriggerWordLength = 0;
			}
			bool result = base.KeyPress(key, keyChar, modifier);
			string lineText = base.Editor.GetLineText(base.Editor.Caret.Line);
			string input = lineText.Substring(0, Math.Min(lineText.Length, base.Editor.Caret.Column));
			if (rx_is_keyword.IsMatch(input))
			{
				CompletionWindowManager.HideWindow();
			}
			return result;
		}

		public override int GetCurrentParameterIndex(int startOffset)
		{
			int num = startOffset;
			int num2 = 1;
			int num3 = 0;
			while (num > 1)
			{
				switch (document.Editor.GetCharAt(num))
				{
				case ')':
					num3++;
					break;
				case '(':
					num3--;
					if (num3 < 0)
					{
						return num2 + 1;
					}
					break;
				case ',':
					if (num3 == 0)
					{
						num2++;
					}
					break;
				}
				num--;
			}
			return base.GetCurrentParameterIndex(startOffset);
		}
	}
}
