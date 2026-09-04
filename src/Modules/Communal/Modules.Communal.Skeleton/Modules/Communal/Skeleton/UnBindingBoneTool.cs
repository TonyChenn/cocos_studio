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
	// Token: 0x0200001D RID: 29
	internal class UnBindingBoneTool : BaseTool
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000115 RID: 277 RVA: 0x000064E1 File Offset: 0x000046E1
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("Modules.Communal.Skeleton.Images.UnLink.png");
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000116 RID: 278 RVA: 0x000064ED File Offset: 0x000046ED
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.Skeleton_Unbinding + " (U)";
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000117 RID: 279 RVA: 0x000064FE File Offset: 0x000046FE
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.U;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006502 File Offset: 0x00004702
		public override ToolType Type
		{
			get
			{
				return ToolType.Button;
			}
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00006508 File Offset: 0x00004708
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (args.Event.Key.ToString().ToUpperInvariant() == this.ShortcutKey.ToString())
			{
				UnBindingBoneTool.UnBindingBones();
				args.RetVal = true;
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00006557 File Offset: 0x00004757
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			UnBindingBoneTool.UnBindingBones();
			args.RetVal = true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000656C File Offset: 0x0000476C
		public static void UnBindingBones()
		{
			string text = UnBindingBoneTool.UnBindingSelectedNodes();
			if (!string.IsNullOrEmpty(text))
			{
				LogConfig.TipWithOutConsole.Info(text, false);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00006594 File Offset: 0x00004794
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

		// Token: 0x0600011D RID: 285 RVA: 0x000066D4 File Offset: 0x000048D4
		public static bool CanNodeUnBinding(AbstractNodeObject node)
		{
			bool result = true;
			if (node == null || node.Parent == null || node.Parent == Services.Workbench.ActiveDocument.File.GetRootNode())
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00006710 File Offset: 0x00004910
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
