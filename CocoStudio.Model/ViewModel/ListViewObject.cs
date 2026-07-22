using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000111 RID: 273
	[ModelExtension(true, 52)]
	[DisplayName("Display_Component_UIListview")]
	[EngineClassName("ListView")]
	[ControlGroup("Control_Container", 2)]
	public class ListViewObject : PanelObject, IListViewType
	{
		// Token: 0x060009FE RID: 2558 RVA: 0x00027FD0 File Offset: 0x000261D0
		private CSListView GetInnerWidget()
		{
			return (CSListView)this.innerNode;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00027FED File Offset: 0x000261ED
		public ListViewObject()
		{
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00028006 File Offset: 0x00026206
		public ListViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00028020 File Offset: 0x00026220
		protected override void CreateCSObject()
		{
			this.innerNode = new CSListView();
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00028030 File Offset: 0x00026230
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				base.SingleColor = Color.FromArgb(255, 150, 150, 255);
				base.FirstColor = Color.FromArgb(255, 150, 150, 255);
				base.EndColor = Color.FromArgb(255, 255, 255, 255);
				base.ComboBoxType = PanelColorFillType.Color_solid;
				this.DirectionType = ListViewDirectionType.Vertical;
				this.HorizontalType = ListViewHorizontal.Align_Left;
				this.VerticalType = ListViewVertical.Align_Top;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x000280D0 File Offset: 0x000262D0
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x000280F0 File Offset: 0x000262F0
		[PropertyOrder(39)]
		[Description("Display_OpenResilience")]
		[DisplayName("Display_OpenResilience")]
		[Category("Group_Feature")]
		[UndoProperty]
		public bool IsBounceEnabled
		{
			get
			{
				return this.GetInnerWidget().GetBounceEnabled();
			}
			set
			{
				if (this.GetInnerWidget().GetBounceEnabled() != value)
				{
					this.GetInnerWidget().SetBounceEnabled(value);
					this.RaisePropertyChanged<bool>(() => this.IsBounceEnabled);
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002815C File Offset: 0x0002635C
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0002817C File Offset: 0x0002637C
		[DisplayName("Category_Component_Children_Interval")]
		[PropertyOrder(45)]
		[UndoProperty]
		[Browsable(true)]
		[ValueRange(-10000, 10000, 1f, 10f)]
		[Category("Group_Feature")]
		public int ItemMargin
		{
			get
			{
				return this.GetInnerWidget().GetItemSpace();
			}
			set
			{
				if (this.GetInnerWidget().GetItemSpace() != value)
				{
					this.GetInnerWidget().SetItemSpace(value);
					this.RaisePropertyChanged<int>(() => this.ItemMargin);
				}
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x000281E8 File Offset: 0x000263E8
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x00028208 File Offset: 0x00026408
		[PropertyOrder(42)]
		[DisplayName("Description_ScrollDirection")]
		[UndoProperty]
		[Category("Group_Feature")]
		[Browsable(true)]
		public ListViewDirectionType DirectionType
		{
			get
			{
				return (ListViewDirectionType)this.GetInnerWidget().GetDirectionType();
			}
			set
			{
				this.GetInnerWidget().SetDirectionType((int)value);
				if (value == ListViewDirectionType.Horizontal)
				{
					this.GetInnerWidget().SetGravityType((int)this.verticalType);
				}
				else if (value == ListViewDirectionType.Vertical)
				{
					this.GetInnerWidget().SetGravityType((int)this.horizontalType);
				}
				this.RaisePropertyChanged<ListViewDirectionType>(() => this.DirectionType);
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x000282A0 File Offset: 0x000264A0
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x000282B8 File Offset: 0x000264B8
		[Category("Group_Feature")]
		[PropertyOrder(43)]
		[DisplayName("UIListVIew_ChildAlignment")]
		[UndoProperty]
		public ListViewHorizontal HorizontalType
		{
			get
			{
				return this.horizontalType;
			}
			set
			{
				this.horizontalType = value;
				if (this.DirectionType == ListViewDirectionType.Vertical)
				{
					this.GetInnerWidget().SetGravityType((int)value);
				}
				this.RaisePropertyChanged<ListViewHorizontal>(() => this.HorizontalType);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00028328 File Offset: 0x00026528
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x00028340 File Offset: 0x00026540
		[PropertyOrder(44)]
		[UndoProperty]
		[DisplayName("UIListVIew_ChildAlignment")]
		[Category("Group_Feature")]
		public ListViewVertical VerticalType
		{
			get
			{
				return this.verticalType;
			}
			set
			{
				this.verticalType = value;
				if (this.DirectionType == ListViewDirectionType.Horizontal)
				{
					this.GetInnerWidget().SetGravityType((int)value);
				}
				this.RaisePropertyChanged<ListViewVertical>(() => this.VerticalType);
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000283B0 File Offset: 0x000265B0
		internal override void InsertChild(int index, AbstractNodeObject nObject)
		{
			WidgetObject widgetObject = nObject as WidgetObject;
			if (widgetObject != null)
			{
				widgetObject.HorizontalEdge = HorizontalBerthEdge.None;
				widgetObject.VerticalEdge = VerticalBerthEdge.None;
				widgetObject.StretchWidthEnable = false;
				widgetObject.StretchHeightEnable = false;
				this.GetCSVisual().InsertChild(index, nObject.GetCSVisual());
				nObject.OperationFlag = OperationMask.SizeFlag;
				nObject.Parent = this;
			}
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x00028416 File Offset: 0x00026616
		internal override void RemoveChild(AbstractNodeObject nObject)
		{
			base.RemoveChild(nObject);
			nObject.InitOperation();
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00028428 File Offset: 0x00026628
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			ListViewObject listViewObject = cObject as ListViewObject;
			if (listViewObject != null)
			{
				listViewObject.IsBounceEnabled = this.IsBounceEnabled;
				listViewObject.DirectionType = this.DirectionType;
				listViewObject.HorizontalType = this.HorizontalType;
				listViewObject.VerticalType = this.VerticalType;
				listViewObject.ItemMargin = this.ItemMargin;
			}
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00028494 File Offset: 0x00026694
		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			bool result;
			if (!base.CanReceiveDragObject(objectData, showLog))
			{
				result = false;
			}
			else if (!typeof(WidgetObject).IsAssignableFrom(objectData.MetaData.Type))
			{
				if (showLog)
				{
					LogConfig.Output.Info(LanguageInfo.ListViewOutputMessage, true);
				}
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x000284F4 File Offset: 0x000266F4
		public override bool CanReceiveDragResource(ResourceInfoDragData objectData, bool showLog)
		{
			if (showLog)
			{
				LogConfig.Output.Info(LanguageInfo.ListViewOutputMessage, true);
			}
			return false;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00028520 File Offset: 0x00026720
		public override bool CanDrop(object node, TreeViewDropPosition mode, bool copy)
		{
			if (mode == TreeViewDropPosition.IntoOrBefore || mode == TreeViewDropPosition.IntoOrAfter)
			{
				if (!(node is WidgetObject))
				{
					LogConfig.Output.Info(LanguageInfo.ListViewOutputMessage, true);
					return false;
				}
			}
			return base.CanDrop(node, mode, copy);
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x00028574 File Offset: 0x00026774
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x04000458 RID: 1112
		private ListViewHorizontal horizontalType = ListViewHorizontal.Align_Left;

		// Token: 0x04000459 RID: 1113
		private ListViewVertical verticalType = ListViewVertical.Align_Top;
	}
}
