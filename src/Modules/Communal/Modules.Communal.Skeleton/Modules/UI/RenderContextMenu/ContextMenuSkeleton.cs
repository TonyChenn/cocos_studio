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
	[Extension(typeof(IObjectContextMenu))]
	public class ContextMenuSkeleton : BaseMenu
	{
		protected override void InitMenuItem()
		{
			this.menuItemUnBind = MenuCreator.CreateMenuItem(GlobalCommand.SkeletonUnBind, false, LanguageInfo.Skeleton_Unbinding);
			this.menuItemRename = MenuCreator.CreateMenuItem(GlobalCommand.RenameCmd, false, LanguageInfo.Command_Rename);
			base.InitMenuItem();
			base.Add(this.menuItemUnBind);
			base.Add(this.menuItemRename);
		}

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

		[CommandHandler(CmdEnum.SkeletonUnBind)]
		public void SkeletonUnBindCmd()
		{
			UnBindingBoneTool.UnBindingBones();
		}

		[CommandUpdateHandler(CmdEnum.SkeletonUnBind)]
		public void SkeletonUnBindCanExtentCmd(CommandInfo info)
		{
			info.Enabled = UnBindingBoneTool.CanNodesUnBinding(base.SelectedObjectList);
		}

		public override string Type
		{
			get
			{
				return NodeType.Skeleton.ToString();
			}
		}

		private MenuItem menuItemUnBind;

		private MenuItem menuItemRename;
	}
}
