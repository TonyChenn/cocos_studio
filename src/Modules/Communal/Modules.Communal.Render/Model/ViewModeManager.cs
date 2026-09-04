using System;
using CocoStudio.Basic;
using CocoStudio.Projects;
using Mono.Addins;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000032 RID: 50
	public class ViewModeManager : IDocumentEventHandler
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000212 RID: 530 RVA: 0x0000BFA4 File Offset: 0x0000A1A4
		public IViewMode Current
		{
			get
			{
				return this.currentViewMode;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0000BFCC File Offset: 0x0000A1CC
		// (set) Token: 0x06000215 RID: 533 RVA: 0x0000BFE2 File Offset: 0x0000A1E2
		public static ViewModeManager Instance { get; private set; } = new ViewModeManager();

		// Token: 0x06000216 RID: 534 RVA: 0x0000BFEA File Offset: 0x0000A1EA
		private ViewModeManager()
		{
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000BFF8 File Offset: 0x0000A1F8
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

		// Token: 0x06000218 RID: 536 RVA: 0x0000C06C File Offset: 0x0000A26C
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

		// Token: 0x06000219 RID: 537 RVA: 0x0000C0B8 File Offset: 0x0000A2B8
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

		// Token: 0x0600021A RID: 538 RVA: 0x0000C10C File Offset: 0x0000A30C
		public void OnDocumentChanged(CocosItem cocosItem)
		{
			IViewMode viewMode = ViewModeManager.GetViewMode(cocosItem);
			this.OnViewModeChanged(cocosItem, viewMode);
			if (this.currentViewMode != null)
			{
				this.currentViewMode.OnDocumentChanged(cocosItem);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000C144 File Offset: 0x0000A344
		public void OnDocumentSaved(CocosItem cocosItem)
		{
			this.currentViewMode.OnDocumentSaved(cocosItem);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000C154 File Offset: 0x0000A354
		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
			this.currentViewMode.OnDocumentBeforeSave(cocosItem);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000C164 File Offset: 0x0000A364
		public void OnDocumentClosed(CocosItem cocosItem)
		{
			IViewMode viewMode = ViewModeManager.GetViewMode(cocosItem);
			if (viewMode != null)
			{
				viewMode.OnDocumentClosed(cocosItem);
			}
		}

		// Token: 0x04000096 RID: 150
		private static IViewMode[] viewModeArray;

		// Token: 0x04000097 RID: 151
		private IViewMode currentViewMode;
	}
}
