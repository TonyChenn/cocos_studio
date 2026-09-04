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
	// Token: 0x02000017 RID: 23
	internal class HideSkinTool : BaseTool
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00005AC8 File Offset: 0x00003CC8
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.Hide.png");
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_HideSkin + " (P)";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000DD RID: 221 RVA: 0x00005AE5 File Offset: 0x00003CE5
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.P;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00005AE9 File Offset: 0x00003CE9
		public override ToolType Type
		{
			get
			{
				return ToolType.Toggle;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x00005AF4 File Offset: 0x00003CF4
		// (set) Token: 0x060000E1 RID: 225 RVA: 0x00005AFB File Offset: 0x00003CFB
		public static bool ToolHideAllSkins { get; private set; }

		// Token: 0x060000E2 RID: 226 RVA: 0x00005B03 File Offset: 0x00003D03
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.HideSkins();
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005B14 File Offset: 0x00003D14
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
