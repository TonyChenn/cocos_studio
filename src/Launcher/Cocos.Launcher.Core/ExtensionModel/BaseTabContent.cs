using System;
using CocoStudio.Basic;
using Gtk;

namespace Cocos.Launcher.Core.ExtensionModel
{
	public abstract class BaseTabContent : ITabContent
	{
		public virtual int Order
		{
			get
			{
				return int.MaxValue;
			}
		}

		public virtual Widget Content { get; protected set; }

		public void Initialize(ITabHead tabHead)
		{
			try
			{
				this.OnInitialize(tabHead);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent Initialize failed.", exception);
			}
		}

		protected virtual void OnInitialize(ITabHead tabHead)
		{
		}

		public void Activated(SwitchTabInfo switchTabInfo)
		{
			try
			{
				this.OnActivated(switchTabInfo);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent activated failed.", exception);
			}
		}

		protected virtual void OnActivated(SwitchTabInfo switchTabInfo)
		{
		}

		public void Deactivated()
		{
			try
			{
				this.OnDeactivated();
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent Deactivated failed.", exception);
			}
		}

		protected virtual void OnDeactivated()
		{
		}

		public void Search(string url)
		{
			try
			{
				this.OnSearch(url);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("TabContent Search failed.", exception);
			}
		}

		protected virtual void OnSearch(string url)
		{
		}
	}
}
