using System;
using MonoDevelop.Components.Commands;

namespace CocoStudio.Core.ExtensionModel
{
	internal class CcsCmdHandler : CommandHandler
	{
		public CcsCmdHandler(MenuHandler handler)
		{
			this.Handler = handler;
		}

		protected override void Run()
		{
			this.Handler.InternalRun();
		}

		protected override void Run(object dataItem)
		{
			this.Handler.InternalRun(dataItem);
		}

		protected override void Update(CommandInfo info)
		{
			MenuInfo info2 = new MenuInfo(info);
			this.Handler.InternalUpdate(info2);
		}

		protected override void Update(CommandArrayInfo info)
		{
			MenuArrayInfo info2 = new MenuArrayInfo(info);
			this.Handler.InternalUpdate(info2);
		}

		private MenuHandler Handler;
	}
}
