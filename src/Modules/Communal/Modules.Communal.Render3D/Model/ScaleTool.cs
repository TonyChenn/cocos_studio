using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render3D.Model.Tool;
using Xwt.Drawing;

namespace Modules.Communal.Render3D.Model
{
	internal class ScaleTool : Object3DTool
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Scale.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Scale + " (R)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.R;
			}
		}

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
