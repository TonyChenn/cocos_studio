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
	[ModelExtension(true, 52)]
	[DisplayName("Display_Component_UIListview")]
	[EngineClassName("ListView")]
	[ControlGroup("Control_Container", 2)]
	public class ListViewObject : PanelObject, IListViewType
	{
		private CSListView GetInnerWidget()
		{
			return (CSListView)this.innerNode;
		}

		public ListViewObject()
		{
		}

		public ListViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSListView();
		}

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

		internal override void RemoveChild(AbstractNodeObject nObject)
		{
			base.RemoveChild(nObject);
			nObject.InitOperation();
		}

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

		public override bool CanReceiveDragResource(ResourceInfoDragData objectData, bool showLog)
		{
			if (showLog)
			{
				LogConfig.Output.Info(LanguageInfo.ListViewOutputMessage, true);
			}
			return false;
		}

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

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private ListViewHorizontal horizontalType = ListViewHorizontal.Align_Left;

		private ListViewVertical verticalType = ListViewVertical.Align_Top;
	}
}
