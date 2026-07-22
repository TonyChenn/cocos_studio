using System;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render3D.Model.Tool;
using Xwt.Drawing;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x0200000B RID: 11
	internal class TranslateTool : Object3DTool
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00002DA6 File Offset: 0x00000FA6
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Translate.png");
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002DB2 File Offset: 0x00000FB2
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Translate + " (W)";
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00002DC3 File Offset: 0x00000FC3
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.W;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public override void Load()
		{
			base.Load();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002DFC File Offset: 0x00000FFC
		public override void UnLoad()
		{
			base.UnLoad();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002E2C File Offset: 0x0000102C
		private void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			this.controlObject.SelectObjectList = args.SelectedParentObject.ToList<VisualObject>();
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002E44 File Offset: 0x00001044
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected && this.controlObject != null)
			{
				this.controlObject.Operate = ControlNode3D.Opt.Translate;
			}
		}
	}
}
