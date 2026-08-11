using System;
using CocoStudio.Core.ExtensionModel;
using Mono.Addins;

namespace CocosStudio.ExternalImport.StudioPlugin
{
	[Extension(Type = typeof(ICommandHandle))]
	public sealed class ExternalImportCommandHandle : ICommandHandle
	{
		public void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			this.initialized = true;
			ExternalImportServer.Instance.Start();
			AppDomain.CurrentDomain.ProcessExit += this.OnProcessExit;
		}

		private void OnProcessExit(object sender, EventArgs e)
		{
			AppDomain.CurrentDomain.ProcessExit -= this.OnProcessExit;
			ExternalImportServer.Instance.Dispose();
		}

		private bool initialized;
	}
}
