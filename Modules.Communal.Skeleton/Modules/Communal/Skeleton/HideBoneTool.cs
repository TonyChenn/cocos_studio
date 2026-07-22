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
	// Token: 0x0200001A RID: 26
	internal class HideBoneTool : BaseTool
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x0000604F File Offset: 0x0000424F
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x00006052 File Offset: 0x00004252
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.HideBone.png");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x0000605E File Offset: 0x0000425E
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_HideSkeleton + " (H)";
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000606F File Offset: 0x0000426F
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.H;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00006073 File Offset: 0x00004273
		public override ToolType Type
		{
			get
			{
				return ToolType.Toggle;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000FC RID: 252 RVA: 0x0000607E File Offset: 0x0000427E
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00006085 File Offset: 0x00004285
		public static bool ToolHideAllBones { get; private set; }

		// Token: 0x060000FE RID: 254 RVA: 0x0000608D File Offset: 0x0000428D
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.HideBones();
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000609C File Offset: 0x0000429C
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
