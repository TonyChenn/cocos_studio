using System;
using System.Collections.Generic;
using MonoDevelop.Ide.CodeCompletion;

namespace CocoStudio.LuaBinding
{
	public class LuaParameterDataProvider : ParameterDataProvider
	{
		private readonly List<string> Overloads;

		private readonly string FuncName;

		private readonly string FuncArgs;

		public override int Count => Overloads.Count;

		public static List<string> Unpack(string input)
		{
			int num = 0;
			string text = "";
			List<Tuple<int, List<string>>> list = new List<Tuple<int, List<string>>>();
			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];
				if (c == '[')
				{
					int num2 = i;
					num = 1;
					for (i++; i < input.Length; i++)
					{
						if (num <= 0)
						{
							break;
						}
						if (input[i] == '[')
						{
							num++;
						}
						if (input[i] == ']')
						{
							num--;
						}
					}
					i--;
					string input2 = input.Substring(num2 + 1, i - (num2 + 1));
					int length = text.Length;
					list.Add(new Tuple<int, List<string>>(length, Unpack(input2)));
				}
				else
				{
					text += c;
				}
			}
			List<string> list2 = new List<string>();
			if (list.Count == 0)
			{
				list2.Add(text);
				return list2;
			}
			int[] array = new int[list.Count];
			int[] array2 = new int[list.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = 0;
			}
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = list[i].Item2.Count;
			}
			bool flag = false;
			while (!flag)
			{
				string text2 = text;
				int i = array.Length;
				while (i-- > 0)
				{
					if (array[i] != 0)
					{
						int item = list[i].Item1;
						string value = list[i].Item2[array[i] - 1];
						text2 = text2.Insert(item, value);
					}
				}
				text2 = text2.Trim(" \t,".ToCharArray());
				List<string> list3 = new List<string>(text2.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
				i = list3.Count;
				while (i-- > 0)
				{
					if (string.IsNullOrWhiteSpace(list3[i]))
					{
						list3.RemoveAt(i);
					}
				}
				string text3 = "";
				text2 = "";
				foreach (string item2 in list3)
				{
					text2 = text2 + text3 + item2.Trim();
					text3 = ", ";
				}
				list2.Add(text2);
				array[0]++;
				for (i = 0; i < array.Length; i++)
				{
					if (array[i] > array2[i])
					{
						if (i + 1 >= array.Length)
						{
							flag = true;
							break;
						}
						array[i] = 0;
						array[i + 1]++;
					}
				}
			}
			return list2;
		}

		public LuaParameterDataProvider(string funcname, string args)
			: base(1)
		{
			Overloads = Unpack(args);
			FuncName = funcname;
			FuncArgs = args;
		}

		public override bool AllowParameterList(int overload)
		{
			return overload < Count;
		}

		public override int GetParameterCount(int overload)
		{
			return Overloads[overload].Split(",".ToCharArray()).Length;
		}

		public override string GetParameterName(int overload, int currentParameter)
		{
			return Overloads[overload].Split(",".ToCharArray())[currentParameter].Trim();
		}

		private string GetHeading(int overload, string[] parameterDescription, int currentParameter)
		{
			return "HEADING";
		}

		private string GetDescription(int overload, int currentParameter)
		{
			return "DESCRIPT";
		}

		public override TooltipInformation CreateTooltipInformation(int overload, int currentParameter, bool smartWrap)
		{
			TooltipInformation tooltipInformation = new TooltipInformation();
			string[] array = Overloads[overload].Split(",".ToCharArray());
			string text = "";
			string arg = "";
			int num = 1;
			string[] array2 = array;
			foreach (string arg2 in array2)
			{
				text = ((num != currentParameter) ? (text + $"{arg}{arg2}") : (text + $"{arg}<b><i>{arg2}</i></b>"));
				arg = ", ";
				num++;
			}
			tooltipInformation.SignatureMarkup = FuncName + "(" + FuncArgs + ")";
			tooltipInformation.AddCategory("Parameters", $"{FuncName}( {text} )");
			return tooltipInformation;
		}
	}
}
