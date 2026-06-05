using System;
using System.Runtime.InteropServices;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x02000058 RID: 88
	internal static class ConsoleCrayon
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002D5 RID: 725 RVA: 0x0000B09C File Offset: 0x0000929C
		// (set) Token: 0x060002D6 RID: 726 RVA: 0x0000B0A3 File Offset: 0x000092A3
		public static ConsoleColor ForegroundColor
		{
			get
			{
				return ConsoleCrayon.foreground_color;
			}
			set
			{
				ConsoleCrayon.foreground_color = value;
				ConsoleCrayon.SetColor(ConsoleCrayon.foreground_color, true);
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002D7 RID: 727 RVA: 0x0000B0B6 File Offset: 0x000092B6
		// (set) Token: 0x060002D8 RID: 728 RVA: 0x0000B0BD File Offset: 0x000092BD
		public static ConsoleColor BackgroundColor
		{
			get
			{
				return ConsoleCrayon.background_color;
			}
			set
			{
				ConsoleCrayon.background_color = value;
				ConsoleCrayon.SetColor(ConsoleCrayon.background_color, false);
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000B0D0 File Offset: 0x000092D0
		public static void ResetColor()
		{
			if (ConsoleCrayon.XtermColors)
			{
				Console.Write(ConsoleCrayon.GetAnsiResetControlCode());
				return;
			}
			if (Environment.OSVersion.Platform != PlatformID.Unix && !ConsoleCrayon.RuntimeIsMono)
			{
				Console.ResetColor();
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000B100 File Offset: 0x00009300
		private static void SetColor(ConsoleColor color, bool isForeground)
		{
			if (color < ConsoleColor.Black || color > ConsoleColor.White)
			{
				throw new ArgumentOutOfRangeException("color", "Not a ConsoleColor value.");
			}
			if (ConsoleCrayon.XtermColors)
			{
				Console.Write(ConsoleCrayon.GetAnsiColorControlCode(color, isForeground));
				return;
			}
			if (Environment.OSVersion.Platform != PlatformID.Unix && !ConsoleCrayon.RuntimeIsMono)
			{
				if (isForeground)
				{
					Console.ForegroundColor = color;
					return;
				}
				Console.BackgroundColor = color;
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000B160 File Offset: 0x00009360
		private static int TranslateColor(ConsoleColor desired, out bool light)
		{
			switch (desired)
			{
			case ConsoleColor.Black:
				light = false;
				return 0;
			case ConsoleColor.DarkBlue:
				light = false;
				return 4;
			case ConsoleColor.DarkGreen:
				light = false;
				return 2;
			case ConsoleColor.DarkCyan:
				light = false;
				return 6;
			case ConsoleColor.DarkRed:
				light = false;
				return 1;
			case ConsoleColor.DarkMagenta:
				light = false;
				return 5;
			case ConsoleColor.DarkYellow:
				light = false;
				return 3;
			case ConsoleColor.Gray:
				light = false;
				return 7;
			case ConsoleColor.DarkGray:
				light = true;
				return 0;
			case ConsoleColor.Blue:
				light = true;
				return 4;
			case ConsoleColor.Green:
				light = true;
				return 2;
			case ConsoleColor.Cyan:
				light = true;
				return 6;
			case ConsoleColor.Red:
				light = true;
				return 1;
			case ConsoleColor.Magenta:
				light = true;
				return 5;
			case ConsoleColor.Yellow:
				light = true;
				return 3;
			}
			light = true;
			return 7;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000B208 File Offset: 0x00009408
		private static string GetAnsiColorControlCode(ConsoleColor colour, bool isForeground)
		{
			bool flag;
			int num = ConsoleCrayon.TranslateColor(colour, out flag) + (isForeground ? 30 : 40) + (flag ? 60 : 0);
			return string.Format("\u001b[{0}m", num);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000B241 File Offset: 0x00009441
		private static string GetAnsiResetControlCode()
		{
			return "\u001b[0m";
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002DE RID: 734 RVA: 0x0000B248 File Offset: 0x00009448
		public static bool XtermColors
		{
			get
			{
				if (ConsoleCrayon.xterm_colors == null)
				{
					ConsoleCrayon.DetectXtermColors();
				}
				return ConsoleCrayon.xterm_colors.Value;
			}
		}

		// Token: 0x060002DF RID: 735
		[DllImport("libc", EntryPoint = "isatty")]
		private static extern int _isatty(int fd);

		// Token: 0x060002E0 RID: 736 RVA: 0x0000B268 File Offset: 0x00009468
		private static bool isatty(int fd)
		{
			bool result;
			try
			{
				result = (ConsoleCrayon._isatty(fd) == 1);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000B298 File Offset: 0x00009498
		private static void DetectXtermColors()
		{
			bool flag = false;
			string environmentVariable;
			if ((environmentVariable = Environment.GetEnvironmentVariable("TERM")) != null)
			{
				if (!(environmentVariable == "xterm") && !(environmentVariable == "linux"))
				{
					if (environmentVariable == "xterm-color")
					{
						flag = true;
					}
				}
				else if (Environment.GetEnvironmentVariable("COLORTERM") != null)
				{
					flag = true;
				}
			}
			ConsoleCrayon.xterm_colors = new bool?(flag && ConsoleCrayon.isatty(1) && ConsoleCrayon.isatty(2));
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000B30E File Offset: 0x0000950E
		public static bool RuntimeIsMono
		{
			get
			{
				if (ConsoleCrayon.runtime_is_mono == null)
				{
					ConsoleCrayon.runtime_is_mono = new bool?(Type.GetType("System.MonoType") != null);
				}
				return ConsoleCrayon.runtime_is_mono.Value;
			}
		}

		// Token: 0x0400010F RID: 271
		private static ConsoleColor foreground_color;

		// Token: 0x04000110 RID: 272
		private static ConsoleColor background_color;

		// Token: 0x04000111 RID: 273
		private static bool? xterm_colors = null;

		// Token: 0x04000112 RID: 274
		private static bool? runtime_is_mono;
	}
}
