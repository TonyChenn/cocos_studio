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
	// Token: 0x02000006 RID: 6
	internal class CreateBoneGroupTool : BaseTool, ISkeletonTool
	{
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002B20 File Offset: 0x00000D20
		public override bool HasSeparator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002B23 File Offset: 0x00000D23
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return this.currentTool.Icon;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002B30 File Offset: 0x00000D30
		public override string Tooltip
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002B37 File Offset: 0x00000D37
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.C;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002B3B File Offset: 0x00000D3B
		public override Widget CustomWidget
		{
			get
			{
				return this.radioButton;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002B44 File Offset: 0x00000D44
		// (set) Token: 0x0600002E RID: 46 RVA: 0x00002B60 File Offset: 0x00000D60
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

		// Token: 0x0600002F RID: 47 RVA: 0x00002BE0 File Offset: 0x00000DE0
		public CreateBoneGroupTool()
		{
			this.subToolList = new List<CreateBoneTool>();
			this.subToolList.Add(new CreateBindingBoneTool());
			this.subToolList.Add(new CreateBoneTool());
			this.radioButton = this.CreateRadioButton(this.subToolList);
			RadioItem curItem = this.radioButton.SelectedItem;
			this.currentTool = this.subToolList.FirstOrDefault((CreateBoneTool w) => w.Tooltip == curItem.ID);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C64 File Offset: 0x00000E64
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

		// Token: 0x06000031 RID: 49 RVA: 0x00002D0C File Offset: 0x00000F0C
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

		// Token: 0x06000032 RID: 50 RVA: 0x00002DF4 File Offset: 0x00000FF4
		private void SelectedRadioItemChangedHandler(object sender, EventArgs e)
		{
			RadioItem curItem = this.radioButton.SelectedItem;
			this.currentTool = this.subToolList.FirstOrDefault((CreateBoneTool w) => w.Tooltip == curItem.ID);
			this.OnSelectedChanged();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002E3B File Offset: 0x0000103B
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			if (base.IsSelected && args.Event.State == ModifierType.ShiftMask)
			{
				this.radioButton.SwitchToNext();
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002E5E File Offset: 0x0000105E
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			this.currentTool.OnMouseDown(args);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002E6C File Offset: 0x0000106C
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			this.currentTool.OnMouseMove(args);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002E7A File Offset: 0x0000107A
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			this.currentTool.OnMouseUp(args);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002E88 File Offset: 0x00001088
		public override void OnMouseEnter(EnterNotifyEventArgs args)
		{
			base.OnMouseEnter(args);
			BaseTool.SetCursor(Cursors.ArrowMoveAnchor);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002E9B File Offset: 0x0000109B
		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			this.currentTool.OnKeyUp(args);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002EA9 File Offset: 0x000010A9
		public void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			this.currentTool.OnSelectObjectsChangeEvent(args);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002EB7 File Offset: 0x000010B7
		public void OnRefreshControlDraw()
		{
			this.currentTool.OnRefreshControlDraw();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002EC4 File Offset: 0x000010C4
		public void OnCanvasZoomedChangedEvent()
		{
			this.currentTool.ReCalculatePoints();
		}

		// Token: 0x0400000B RID: 11
		private CompositeRadioButton radioButton;

		// Token: 0x0400000C RID: 12
		private List<CreateBoneTool> subToolList;

		// Token: 0x0400000D RID: 13
		private CreateBoneTool currentTool;
	}
}
