using System;
using System.Text;
using CocoStudio.Core;
using Gtk;

namespace Modules.Communal.CocosAdapter.Platform
{
	internal abstract class BasePlatform : IPlatform, IComparable
	{
		public abstract EnumPlatform PlatformType { get; }

		public virtual int Order
		{
			get
			{
				return 99;
			}
		}

		protected abstract string PlatformName { get; }

		public virtual bool IsShowConsoleWhenRun
		{
			get
			{
				return false;
			}
		}

		public virtual string GetDisplayName(EnumOperationType opType)
		{
			return string.Empty;
		}

		public virtual bool CanShow(EnumOperationType opType)
		{
			return opType == EnumOperationType.Package || opType == EnumOperationType.Run;
		}

		public bool CanExecute(EnumOperationType opType, PackageParams prms)
		{
			return Services.ProjectsService.CurrentSolution != null && (opType == EnumOperationType.Package || opType == EnumOperationType.Run) && this.OnCanExecute(opType, prms);
		}

		protected virtual bool OnCanExecute(EnumOperationType opType, PackageParams prms)
		{
			return true;
		}

		public bool Execute(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			return this.OnExecuteInitialize(opType, prms, monitor) && this.OnExecute(opType, prms, monitor);
		}

		protected virtual bool OnExecuteInitialize(EnumOperationType opType, PackageParams prms, CocosMonitor monitor)
		{
			return true;
		}

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

		protected abstract string OnCreateConsoleArguments(EnumOperationType opType, PackageParams prms);

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
