using System;
using CocoStudio.Model.ViewModel;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render3D.Model.Tool;
using Xwt.Drawing;

namespace Modules.Communal.Render3D.Model
{
	internal class RotateTool : Object3DTool
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Rotation.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Rotate + " (E)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.E;
			}
		}

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
