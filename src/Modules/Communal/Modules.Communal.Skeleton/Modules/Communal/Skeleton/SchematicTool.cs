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
	internal class SchematicTool : BaseTool
	{
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.TreeRelationShip.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_ViewTooltip;
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.T;
			}
		}

		public override ToolType Type
		{
			get
			{
				return ToolType.Button;
			}
		}

		protected override void OnEnabledChanged()
		{
			base.OnEnabledChanged();
			if (!base.Enabled && this.window != null)
			{
				this.window.Destroy();
				this.window = null;
			}
		}

		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString().ToUpperInvariant() == this.ShortcutKey.ToString())
			{
				this.ShowWindow();
			}
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			Tracker.Add(ViewRegions.UIMainTool, "SkeletonView", "", "");
			this.ShowWindow();
			args.RetVal = true;
		}

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

		private void Window_Destroyed(object sender, EventArgs e)
		{
			this.window.Destroyed -= this.Window_Destroyed;
			this.window = null;
		}

		private SkeletonGraphDialog window;
	}
}
