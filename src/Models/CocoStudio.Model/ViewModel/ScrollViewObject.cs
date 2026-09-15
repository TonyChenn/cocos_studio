using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Interface;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[ModelExtension(true, 51)]
	[DisplayName("Display_Component_UIScrollview")]
	[ControlGroup("Control_Container", 2)]
	[EngineClassName("ScrollView")]
	public class ScrollViewObject : PanelObject, ICallBackEvent
	{
		private CSScrollView GetInnerWidget()
		{
			return (CSScrollView)this.innerNode;
		}

		public ScrollViewObject()
		{
		}

		public ScrollViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSScrollView();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				base.SingleColor = Color.FromArgb(255, 255, 150, 100);
				base.FirstColor = Color.FromArgb(255, 255, 150, 100);
				base.EndColor = Color.FromArgb(255, 255, 255, 255);
				base.ComboBoxType = PanelColorFillType.Color_solid;
				this.ScrollDirectionType = ScrollViewDirectionType.Vertical;
				this.InnerNodeSize = new SizeValue(200, 300);
			}
		}

		[Category("Group_PosAndSize")]
		[RequestOperationMode(OperationMask.SizeFlag)]
		[Editor(typeof(UISizeEditor), typeof(UISizeEditor))]
		[DisplayName("grid_sudoku")]
		[Browsable(true)]
		[LayoutRefresh]
		[PropertyOrder(16)]
		[UndoProperty]
		public override SizeF Size
		{
			get
			{
				return this.GetCSVisual().GetSize();
			}
			set
			{
				value.Width = (float)Math.Floor((double)value.Width);
				value.Height = (float)Math.Floor((double)value.Height);
				int num = this.InnerNodeSize.Width;
				int num2 = this.InnerNodeSize.Height;
				if ((float)num < value.Width)
				{
					num = (int)value.Width;
				}
				if ((float)num2 < value.Height)
				{
					num2 = (int)value.Height;
				}
				if (num != this.InnerNodeSize.Width || num2 != this.InnerNodeSize.Height)
				{
					this.InnerNodeSize = new SizeValue(num, num2);
				}
				this.GetCSVisual().SetSize(value);
				this.RaisePropertyChanged<SizeF>(() => this.Size);
			}
		}

		[UndoProperty]
		[Description("Description_ScrollAreaWidth")]
		[PropertyOrder(40)]
		[Editor(typeof(ScrollAreaSizeEditor), typeof(ScrollAreaSizeEditor))]
		[DisplayName("Description_ScrollAreaWidth")]
		[Category("Group_Feature")]
		[Browsable(true)]
		public virtual SizeValue InnerNodeSize
		{
			get
			{
				return new SizeValue((int)this.GetInnerWidget().GetInnerSize().Width, (int)this.GetInnerWidget().GetInnerSize().Height);
			}
			set
			{
				if (this.GetInnerWidget().GetInnerSize().Width != (float)value.Width || this.GetInnerWidget().GetInnerSize().Height != (float)value.Height)
				{
					this.GetInnerWidget().SetInnerSize(new SizeF((float)value.Width, (float)value.Height));
					this.RaisePropertyChanged<SizeValue>(() => this.InnerNodeSize);
				}
			}
		}

		[PropertyOrder(41)]
		[Description("Description_ScrollDirection")]
		[Category("Group_Feature")]
		[UndoProperty]
		[DisplayName("Description_ScrollDirection")]
		public virtual ScrollViewDirectionType ScrollDirectionType
		{
			get
			{
				return (ScrollViewDirectionType)this.GetInnerWidget().GetDirectionType();
			}
			set
			{
				this.GetInnerWidget().SetDirectionType((int)value);
				this.RaisePropertyChanged<ScrollViewDirectionType>(() => this.ScrollDirectionType);
			}
		}

		[UndoProperty]
		[PropertyOrder(39)]
		[DisplayName("Display_OpenResilience")]
		[Description("Display_OpenResilience")]
		[Category("Group_Feature")]
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

		internal override void InsertChild(int index, AbstractNodeObject nObject)
		{
			base.InsertChild(index, nObject);
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			ScrollViewObject scrollViewObject = cObject as ScrollViewObject;
			if (scrollViewObject != null)
			{
				scrollViewObject.ScrollDirectionType = this.ScrollDirectionType;
				scrollViewObject.InnerNodeSize = this.InnerNodeSize;
				scrollViewObject.IsBounceEnabled = this.IsBounceEnabled;
			}
		}

		public override PointF TransformSceneForChild(PointF scene)
		{
			return this.GetInnerWidget().TransformToSelfInner(scene);
		}

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}
	}
}
