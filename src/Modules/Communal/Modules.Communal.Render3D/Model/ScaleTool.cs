using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render3D.Model.Tool;
using Xwt.Drawing;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x0200000A RID: 10
	internal class ScaleTool : Object3DTool
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00002D59 File Offset: 0x00000F59
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Scale.png");
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00002D65 File Offset: 0x00000F65
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Scale + " (R)";
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002D76 File Offset: 0x00000F76
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.R;
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D7A File Offset: 0x00000F7A
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected && this.controlObject != null)
			{
				this.controlObject.Operate = ControlNode3D.Opt.Scale;
			}
		}
	}
}
