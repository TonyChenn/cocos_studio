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
	// Token: 0x02000114 RID: 276
	[ModelExtension(true, 51)]
	[DisplayName("Display_Component_UIScrollview")]
	[ControlGroup("Control_Container", 2)]
	[EngineClassName("ScrollView")]
	public class ScrollViewObject : PanelObject, ICallBackEvent
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x00028E3C File Offset: 0x0002703C
		private CSScrollView GetInnerWidget()
		{
			return (CSScrollView)this.innerNode;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00028E59 File Offset: 0x00027059
		public ScrollViewObject()
		{
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00028E64 File Offset: 0x00027064
		public ScrollViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00028E70 File Offset: 0x00027070
		protected override void CreateCSObject()
		{
			this.innerNode = new CSScrollView();
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00028E80 File Offset: 0x00027080
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

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x00028F20 File Offset: 0x00027120
		// (set) Token: 0x06000A45 RID: 2629 RVA: 0x00028F40 File Offset: 0x00027140
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

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0002903C File Offset: 0x0002723C
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x00029078 File Offset: 0x00027278
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

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0002911C File Offset: 0x0002731C
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x0002913C File Offset: 0x0002733C
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

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x00029194 File Offset: 0x00027394
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x000291B4 File Offset: 0x000273B4
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

		// Token: 0x06000A4C RID: 2636 RVA: 0x0002921E File Offset: 0x0002741E
		internal override void InsertChild(int index, AbstractNodeObject nObject)
		{
			base.InsertChild(index, nObject);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0002922C File Offset: 0x0002742C
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

		// Token: 0x06000A4E RID: 2638 RVA: 0x00029280 File Offset: 0x00027480
		public override PointF TransformSceneForChild(PointF scene)
		{
			return this.GetInnerWidget().TransformToSelfInner(scene);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000292A0 File Offset: 0x000274A0
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}
	}
}
