using System;
using System.Text;
using CocoStudio.Core;
using Gtk;

namespace Modules.Communal.CocosAdapter.Platform
{
	// Token: 0x02000010 RID: 16
	internal abstract class BasePlatform : IPlatform, IComparable
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000066 RID: 102
		public abstract EnumPlatform PlatformType { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00003490 File Offset: 0x00001690
		public virtual int Order
		{
			get
			{
				return 99;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000068 RID: 104
		protected abstract string PlatformName { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003494 File Offset: 0x00001694
		public virtual bool IsShowConsoleWhenRun
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003497 File Offset: 0x00001697
		public virtual string GetDisplayName(EnumOperationType opType)
		{
			return string.Empty;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000349E File Offset: 0x0000169E
		public virtual bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Package || opType == EnumOperationType.Run;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000034AB File Offset: 0x000016AB
		public bool CanExecute(EnumOperationType opType, PackageParams prms)
		{
			return Services.ProjectsService.CurrentSolution != null && (opType == EnumOperationType.Package || opType == EnumOperationType.Run) && this.OnCanExecute(opType, prms);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000034CD File Offset: 0x000016CD
		protected virtual bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return true;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000034D0 File Offset: 0x000016D0
		public bool Execute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			return this.OnExecuteInitialize(opType, prms, monitor) && this.OnExecute(opType, prms, monitor);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000034E8 File Offset: 0x000016E8
		protected virtual bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			return true;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000034EC File Offset: 0x000016EC
		protected virtual bool OnExecute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			string cmd = this.OnCreateConsoleArguments(opType, prms);
			CocosPythonTool cocosPythonTool = new CocosPythonTool(monitor);
			bool showConsole = false;
			if (opType == EnumOperationType.Run)
			{
				showConsole = this.IsShowConsoleWhenRun;
			}
			return cocosPythonTool.RunPython(prms.EngineInfo, cmd, showConsole);
		}

		// Token: 0x06000071 RID: 113
		protected abstract string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms);

		// Token: 0x06000072 RID: 114 RVA: 0x00003524 File Offset: 0x00001724
		protected string CreateGeneralArguments(EnumOperationType opType, string sourcePath, bool isDebug = false)
		{
			if (opType != EnumOperationType.Package && opType != EnumOperationType.Run)
			{
				return string.Empty;
			}
			string value = " compile";
			if (opType == EnumOperationType.Run)
			{
				value = " run";
			}
			string arg = "release";
			if (isDebug)
			{
				arg = "debug";
			}
			string arg2 = Services.ProjectsService.CurrentSolution.PackageDirectory.Replace("\\", "/");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			stringBuilder.Append(string.Format(" -s \"{0}\"", sourcePath));
			stringBuilder.Append(string.Format(" -p {0}", this.PlatformName));
			if (opType == EnumOperationType.Package)
			{
				stringBuilder.Append(string.Format(" -m {0}", arg));
				stringBuilder.Append(string.Format(" -o \"{0}\"", arg2));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000035E4 File Offset: 0x000017E4
		public int CompareTo(object obj)
		{
			IPlatform platform = obj as IPlatform;
			if (platform != null)
			{
				return this.Order.CompareTo(platform.Order);
			}
			return this.Order.CompareTo(-1);
		}
	}
}
