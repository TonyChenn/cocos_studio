using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render3D.Model.Tool;
using Xwt.Drawing;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x02000009 RID: 9
	internal class RotateTool : Object3DTool
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002D0C File Offset: 0x00000F0C
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Rotation.png");
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002D18 File Offset: 0x00000F18
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Rotate + " (E)";
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00002D29 File Offset: 0x00000F29
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.E;
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002D2D File Offset: 0x00000F2D
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected && this.controlObject != null)
			{
				this.controlObject.Operate = ControlNode3D.Opt.Rotate;
			}
		}
	}
}
