using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	internal class UnBindingBoneTool : BaseTool
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.UnLink.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_Unbinding + " (U)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.U;
			}
		}

		public override ToolType Type
		{
			get
			{
				return ToolType.Button;
			}
		}

		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString().ToUpperInvariant() == this.ShortcutKey.ToString())
			{
				UnBindingBoneTool.UnBindingBones();
				args.RetVal = true;
			}
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			UnBindingBoneTool.UnBindingBones();
			args.RetVal = true;
		}

		public static void UnBindingBones()
		{
			string text = UnBindingBoneTool.UnBindingSelectedNodes();
			if (!string.IsNullOrEmpty(text))
			{
				LogConfig.TipWithOutConsole.Info(text, false);
			}
		}

		public static string UnBindingSelectedNodes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			IReadOnlyList<VisualObject> selectedObjectList = SelectService.Instance.SelectedObjectList;
			IReadOnlyList<VisualObject> selectedParentObjectList = SelectService.Instance.SelectedParentObjectList;
			SkeletonObject skeletonObject = Services.Workbench.ActiveDocument.File.GetRootNode() as SkeletonObject;
			if (skeletonObject == null || selectedObjectList.Count == 0)
			{
				return string.Empty;
			}
			using (CompositeTask.Run("UnBindingBoneTool.UnBindingBones", null))
			{
				foreach (VisualObject visualObject in selectedObjectList)
				{
					AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
					if (abstractNodeObject != null && abstractNodeObject.Parent != null && abstractNodeObject.Parent != skeletonObject)
					{
						abstractNodeObject.Parent.Children.Remove(abstractNodeObject);
						skeletonObject.Children.Add(abstractNodeObject);
						stringBuilder.AppendFormat("[{0}] ", abstractNodeObject.Name);
					}
				}
				SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
				@event.Publish(new SelectedVisualObjectsChangeEventArgs(selectedObjectList, selectedParentObjectList, false));
			}
			if (stringBuilder.Length != 0)
			{
				stringBuilder.Append(LanguageInfo.Skeleton_UnBindingSuccess);
			}
			return stringBuilder.ToString();
		}

		public static bool CanNodeUnBinding(AbstractNodeObject node)
		{
			bool result = true;
			if (node == null || node.Parent == null || node.Parent == Services.Workbench.ActiveDocument.File.GetRootNode())
			{
				result = false;
			}
			return result;
		}

		public static bool CanNodesUnBinding(IEnumerable<VisualObject> node)
		{
			if (node == null || node.Count<VisualObject>() == 0)
			{
				return false;
			}
			foreach (VisualObject visualObject in node)
			{
				bool flag = UnBindingBoneTool.CanNodeUnBinding((AbstractNodeObject)visualObject);
				if (flag)
				{
					return flag;
				}
			}
			return false;
		}
	}
}
