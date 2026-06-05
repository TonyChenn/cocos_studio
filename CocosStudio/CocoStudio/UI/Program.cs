using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using GLib;
using Gtk;
using Modules.Communal.MutualEditor;
using Modules.Communal.StartAutoRecover;

namespace CocoStudio.UI
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x0000208C File Offset: 0x0000028C
		[STAThread]
		private static void Main(string[] args)
		{
			Services.IntinalizeCompleted += delegate(EventArgs e)
			{
				StartRecoverService.Instance.InitializeEvent();
			};
			Starter.Initialize(EnumApp.Studio, null);
			if (StartInfoService.Instance.PreCheckArgs(args))
			{
				Starter.Run();
				Services.TaskService.Clear(null);
				Services.MainWindow.Closing += delegate(object s, CancelEventArgs e)
				{
					MutualCore.Instance.Dispose();
				};
				GLib.Timeout.Add(1000U, delegate
				{
					StartInfoService.Instance.OpenArgsProjSln();
					return false;
				});
				Application.Run();
			}
		}
	}
}
