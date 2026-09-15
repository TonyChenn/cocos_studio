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
	internal class HideBoneTool : BaseTool
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
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.HideBone.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_HideSkeleton + " (H)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.H;
			}
		}

		public override ToolType Type
		{
			get
			{
				return ToolType.Toggle;
			}
		}

		public static bool ToolHideAllBones { get; private set; }

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.HideBones();
		}

		private void HideBones()
		{
			HideBoneTool.ToolHideAllBones = base.IsSelected;
			foreach (DocumentExtend documentExtend in Services.Workbench.Documents)
			{
				SkeletonObject skeletonObject = documentExtend.File.GetRootNode() as SkeletonObject;
				if (skeletonObject != null)
				{
					skeletonObject.SetHideAllBones(base.IsSelected);
				}
			}
		}
	}
}
