using System;
using CocoStudio.Basic;
using CocoStudio.Projects;
using Mono.Addins;

namespace Modules.Communal.Render.Model
{
	public class ViewModeManager : IDocumentEventHandler
	{
		public IViewMode Current
		{
			get
			{
				return this.currentViewMode;
			}
		}

		public static ViewModeManager Instance { get; private set; } = new ViewModeManager();

		private ViewModeManager()
		{
		}

		public void Initialize(IGLView glView)
		{
			try
			{
				IViewMode[] extensionObjects = AddinManager.GetExtensionObjects<IViewMode>(false);
				ViewModeManager.viewModeArray = extensionObjects;
				foreach (IViewMode viewMode in ViewModeManager.viewModeArray)
				{
					viewMode.Initialize(glView);
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Load IViewMode failed.", exception);
			}
		}

		private static IViewMode GetViewMode(CocosItem cocosItem)
		{
			foreach (IViewMode viewMode in ViewModeManager.viewModeArray)
			{
				if (viewMode.CanHandle(cocosItem))
				{
					return viewMode;
				}
			}
			return null;
		}

		private void OnViewModeChanged(CocosItem cocosItem, IViewMode newValue)
		{
			IViewMode viewMode = this.currentViewMode;
			this.currentViewMode = newValue;
			if (viewMode == null || viewMode != newValue)
			{
				if (viewMode != null)
				{
					viewMode.Deactivated();
				}
				if (newValue != null)
				{
					newValue.Activated(cocosItem);
				}
			}
		}

		public void OnDocumentChanged(CocosItem cocosItem)
		{
			IViewMode viewMode = ViewModeManager.GetViewMode(cocosItem);
			this.OnViewModeChanged(cocosItem, viewMode);
			if (this.currentViewMode != null)
			{
				this.currentViewMode.OnDocumentChanged(cocosItem);
			}
		}

		public void OnDocumentSaved(CocosItem cocosItem)
		{
			this.currentViewMode.OnDocumentSaved(cocosItem);
		}

		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
			this.currentViewMode.OnDocumentBeforeSave(cocosItem);
		}

		public void OnDocumentClosed(CocosItem cocosItem)
		{
			IViewMode viewMode = ViewModeManager.GetViewMode(cocosItem);
			if (viewMode != null)
			{
				viewMode.OnDocumentClosed(cocosItem);
			}
		}

		private static IViewMode[] viewModeArray;

		private IViewMode currentViewMode;
	}
}
