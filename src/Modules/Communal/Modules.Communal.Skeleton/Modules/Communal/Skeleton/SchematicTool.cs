using System;
using CocoStudio.Core;
using CocoStudio.UserStatistics;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	// Token: 0x0200001B RID: 27
	internal class SchematicTool : BaseTool
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00006114 File Offset: 0x00004314
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00006117 File Offset: 0x00004317
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.TreeRelationShip.png");
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00006123 File Offset: 0x00004323
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_ViewTooltip;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000103 RID: 259 RVA: 0x0000612A File Offset: 0x0000432A
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.T;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000104 RID: 260 RVA: 0x0000612E File Offset: 0x0000432E
		public override ToolType Type
		{
			get
			{
				return ToolType.Button;
			}
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00006131 File Offset: 0x00004331
		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			if (!base.Enabled && this.window != null)
			{
				this.window.Destroy();
				this.window = null;
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000615B File Offset: 0x0000435B
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString().ToUpperInvariant() == this.ShortcutKey.ToString())
			{
				this.ShowWindow();
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00006194 File Offset: 0x00004394
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			Tracker.Add(ViewRegions.UIMainTool, "SkeletonView", "", "");
			this.ShowWindow();
			args.RetVal = true;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x000061C0 File Offset: 0x000043C0
		private void ShowWindow()
		{
			if (this.window == null)
			{
				this.window = new SkeletonGraphDialog();
				this.window.WindowPosition = WindowPosition.CenterOnParent;
				this.window.Destroyed += this.Window_Destroyed;
			}
			this.window.TransientFor = Services.MainWindow;
			this.window.Show();
			this.window.Present();
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00006229 File Offset: 0x00004429
		private void Window_Destroyed(object sender, EventArgs e)
		{
			this.window.Destroyed -= this.Window_Destroyed;
			this.window = null;
		}

		// Token: 0x0400004A RID: 74
		private SkeletonGraphDialog window;
	}
}
