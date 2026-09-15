using System;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	internal class HideSkinTool : BaseTool
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Hide.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_HideSkin + " (P)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.P;
			}
		}

		public override ToolType Type
		{
			get
			{
				return ToolType.Toggle;
			}
		}

		public static bool ToolHideAllSkins { get; private set; }

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.HideSkins();
		}

		private void HideSkins()
		{
			HideSkinTool.ToolHideAllSkins = base.IsSelected;
			foreach (DocumentExtend documentExtend in Services.Workbench.Documents)
			{
				SkeletonObject skeletonObject = documentExtend.File.GetRootNode() as SkeletonObject;
				if (skeletonObject != null)
				{
					skeletonObject.SetHideAllSkins(base.IsSelected);
				}
			}
		}
	}
}
