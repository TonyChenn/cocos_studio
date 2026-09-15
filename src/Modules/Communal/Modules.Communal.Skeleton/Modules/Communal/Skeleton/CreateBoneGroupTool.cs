using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Model.Event;
using Gdk;
using Gtk;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Skeleton
{
	internal class CreateBoneGroupTool : BaseTool, ISkeletonTool
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
				return this.currentTool.Icon;
			}
		}

		public override string Tooltip
		{
			get
			{
				return string.Empty;
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.C;
			}
		}

		public override Widget CustomWidget
		{
			get
			{
				return this.radioButton;
			}
		}

		public override ToolGroup Group
		{
			get
			{
				BaseTool baseTool = this.currentTool;
				return baseTool.Group;
			}
			set
			{
				foreach (ITool tool in this.subToolList)
				{
					BaseTool baseTool = tool as BaseTool;
					if (baseTool != null)
					{
						baseTool.Group = value;
					}
				}
			}
		}

		public CreateBoneGroupTool()
		{
			this.subToolList = new List<CreateBoneTool>();
			this.subToolList.Add(new CreateBindingBoneTool());
			this.subToolList.Add(new CreateBoneTool());
			this.radioButton = this.CreateRadioButton(this.subToolList);
			RadioItem curItem = this.radioButton.SelectedItem;
			this.currentTool = this.subToolList.FirstOrDefault((CreateBoneTool w) => w.Tooltip == curItem.ID);
		}

		private CompositeRadioButton CreateRadioButton(List<CreateBoneTool> toolList)
		{
			List<RadioItem> list = new List<RadioItem>();
			foreach (CreateBoneTool createBoneTool in toolList)
			{
				string tooltip = string.Format("{0} ({1})", createBoneTool.Tooltip, createBoneTool.ShortcutKey);
				RadioItem item = new RadioItem(createBoneTool.Tooltip, createBoneTool.Tooltip, createBoneTool.Icon, tooltip);
				list.Add(item);
			}
			CompositeRadioButton compositeRadioButton = new CompositeRadioButton(list, 0);
			compositeRadioButton.SelectedRadioItemChanged += this.SelectedRadioItemChangedHandler;
			return compositeRadioButton;
		}

		protected override void OnSelectedChanged()
		{
			if (base.IsSelected)
			{
				BaseTool.SetCursor(Cursors.ArrowMoveAnchor);
				BoneControlObject.Instance.ResetAsOrigin();
				this.Group.GetTool<HideBoneTool>().IsSelected = false;
				this.Group.GetTool<HideBoneTool>().Enabled = false;
			}
			else
			{
				BaseTool.SetCursor(Cursors.Arrow);
				this.Group.GetTool<HideBoneTool>().Enabled = true;
			}
			foreach (ITool tool in this.subToolList)
			{
				if (tool != this.currentTool)
				{
					tool.IsSelected = false;
				}
			}
			this.currentTool.IsSelected = base.IsSelected;
		}

		private void SelectedRadioItemChangedHandler(object sender, EventArgs e)
		{
			RadioItem curItem = this.radioButton.SelectedItem;
			this.currentTool = this.subToolList.FirstOrDefault((CreateBoneTool w) => w.Tooltip == curItem.ID);
			this.OnSelectedChanged();
		}

		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (base.IsSelected && args.Event.State == ModifierType.ShiftMask)
			{
				this.radioButton.SwitchToNext();
			}
		}

		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			this.currentTool.OnMouseDown(args);
		}

		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			this.currentTool.OnMouseMove(args);
		}

		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			this.currentTool.OnMouseUp(args);
		}

		public override void OnMouseEnter(EnterNotifyEventArgs args)
		{
			base.OnMouseEnter(args);
			BaseTool.SetCursor(Cursors.ArrowMoveAnchor);
		}

		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			this.currentTool.OnKeyUp(args);
		}

		public void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			this.currentTool.OnSelectObjectsChangeEvent(args);
		}

		public void OnRefreshControlDraw()
		{
			this.currentTool.OnRefreshControlDraw();
		}

		public void OnCanvasZoomedChangedEvent()
		{
			this.currentTool.ReCalculatePoints();
		}

		private CompositeRadioButton radioButton;

		private List<CreateBoneTool> subToolList;

		private CreateBoneTool currentTool;
	}
}
