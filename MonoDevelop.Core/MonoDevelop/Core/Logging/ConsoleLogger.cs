using System;

namespace MonoDevelop.Core.Logging
{
	// Token: 0x02000055 RID: 85
	public class ConsoleLogger : ILogger
	{
		// Token: 0x060002C3 RID: 707 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public void Log(LogLevel level, string message)
		{
			string arg;
			switch (level)
			{
			case LogLevel.Fatal:
				arg = "FATAL ERROR";
				if (this.useColour)
				{
					ConsoleCrayon.ForegroundColor = ConsoleColor.Yellow;
					ConsoleCrayon.BackgroundColor = ConsoleColor.Red;
					goto IL_AF;
				}
				goto IL_AF;
			case LogLevel.Error:
				arg = "ERROR";
				if (this.useColour)
				{
					ConsoleCrayon.ForegroundColor = ConsoleColor.Red;
					ConsoleCrayon.BackgroundColor = ConsoleColor.Yellow;
					goto IL_AF;
				}
				goto IL_AF;
			case (LogLevel)3:
				break;
			case LogLevel.Warn:
				arg = "WARNING";
				if (this.useColour)
				{
					ConsoleCrayon.ForegroundColor = ConsoleColor.Red;
					goto IL_AF;
				}
				goto IL_AF;
			default:
				if (level != LogLevel.Info)
				{
					if (level == LogLevel.Debug)
					{
						arg = "DEBUG";
						if (this.useColour)
						{
							ConsoleCrayon.ForegroundColor = ConsoleColor.Blue;
							goto IL_AF;
						}
						goto IL_AF;
					}
				}
				else
				{
					arg = "INFO";
					if (this.useColour)
					{
						ConsoleCrayon.ForegroundColor = ConsoleColor.Green;
						goto IL_AF;
					}
					goto IL_AF;
				}
				break;
			}
			arg = "LOG";
			IL_AF:
			Console.Write("{0} [{1}]:", arg, DateTime.Now.ToString("u"));
			if (this.useColour)
			{
				ConsoleCrayon.ResetColor();
			}
			Console.WriteLine(" " + message);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000AED6 File Offset: 0x000090D6
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x0000AEDE File Offset: 0x000090DE
		public EnabledLoggingLevel EnabledLevel
		{
			get
			{
				return this.enabledLevel;
			}
			set
			{
				this.enabledLevel = value;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000AEE7 File Offset: 0x000090E7
		public string Name
		{
			get
			{
				return "ConsoleLogger";
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000AEEE File Offset: 0x000090EE
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000AEF8 File Offset: 0x000090F8
		public bool UseColour
		{
			get
			{
				return this.useColour;
			}
			set
			{
				this.useColour = false;
				if (value)
				{
					try
					{
						ConsoleCrayon.ForegroundColor = ConsoleColor.Red;
						ConsoleCrayon.ResetColor();
						this.useColour = true;
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0400010A RID: 266
		private EnabledLoggingLevel enabledLevel = EnabledLoggingLevel.UpToInfo;

		// Token: 0x0400010B RID: 267
		private bool useColour;
	}
}
