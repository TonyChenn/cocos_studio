using System;
using CocoStudio.Basic;
using Gtk;

namespace Modules.Communal.CocosAdapter
{
	internal abstract class CreateStep : ICreateStep
	{
		public bool Run(CreateParams prms, CocosMonitor monitor)
		{
			this.Monitor = monitor;
			bool result = false;
			try
			{
				result = this.OnRun(prms);
			}
			catch (Exception exception)
			{
				this.SendOutputInfo("Failed to create");
				LogConfig.Logger.Error("新建项目时出错", exception);
				result = false;
			}
			return result;
		}

		protected abstract bool OnRun(CreateParams prms);

		public bool CanCreate(CreateParams prms)
		{
			return this.OnCanCreate(prms);
		}

		protected abstract bool OnCanCreate(CreateParams prms);

		protected void SendOutputInfo(string info)
		{
			if (this.Monitor != null && !string.IsNullOrEmpty(info))
			{
				this.Monitor.SendInfo(info);
			}
		}

		protected CocosMonitor Monitor;
	}
}
