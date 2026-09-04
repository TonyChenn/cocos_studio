using System;
using System.Linq;
using CocoStudio.Core.Commands;
using CocoStudio.Model.DataModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.ExtensionModel;
using Modules.Communal.Skeleton;
using Mono.Addins;
using MonoDevelop.Components.Commands;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000010 RID: 16
	[Extension(typeof(IObjectContextMenu))]
	public class ContextMenuSkeleton : BaseMenu
	{
		// Token: 0x06000074 RID: 116 RVA: 0x000045FC File Offset: 0x000027FC
		protected override void InitMenuItem()
		{
			this.menuItemUnBind = MenuCreator.CreateMenuItem(GlobalCommand.SkeletonUnBind, false, LanguageInfo.Skeleton_Unbinding);
			this.menuItemRename = MenuCreator.CreateMenuItem(GlobalCommand.RenameCmd, false, LanguageInfo.Command_Rename);
			base.InitMenuItem();
			base.Add(this.menuItemUnBind);
			base.Add(this.menuItemRename);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000465C File Offset: 0x0000285C
		public override void CanShow(ContextMenuShowingArgs args)
		{
			if ((args != null && args.ClickPoint != null) || base.SelectedObjectList.Count > 1)
			{
				if (base.Children.Contains(this.menuItemRename))
				{
					base.Remove(this.menuItemRename);
				}
			}
			else if (!base.Children.Contains(this.menuItemRename))
			{
				base.Append(this.menuItemRename);
			}
			base.ShowAll();
			base.CanShow(args);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000046D5 File Offset: 0x000028D5
		[CommandHandler(CmdEnum.SkeletonUnBind)]
		public void SkeletonUnBindCmd()
		{
			UnBindingBoneTool.UnBindingBones();
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000046DC File Offset: 0x000028DC
		[CommandUpdateHandler(CmdEnum.SkeletonUnBind)]
		public void SkeletonUnBindCanExtentCmd(CommandInfo info)
		{
			info.Enabled = UnBindingBoneTool.CanNodesUnBinding(base.SelectedObjectList);
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000046EF File Offset: 0x000028EF
		public override string Type
		{
			get
			{
				return NodeType.Skeleton.ToString();
			}
		}

		// Token: 0x04000023 RID: 35
		private MenuItem menuItemUnBind;

		// Token: 0x04000024 RID: 36
		private MenuItem menuItemRename;
	}
}
