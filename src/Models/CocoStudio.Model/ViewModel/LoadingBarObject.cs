using System;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000112 RID: 274
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("LoadingBar")]
	[ModelExtension(true, 6)]
	[DisplayName("Display_Component_UILoadingBar")]
	public class LoadingBarObject : WidgetObject, IResetSize
	{
		// Token: 0x06000A14 RID: 2580 RVA: 0x00028588 File Offset: 0x00026788
		private CSLoadingBar GetInnerWidget()
		{
			return (CSLoadingBar)this.innerNode;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x000285A5 File Offset: 0x000267A5
		public LoadingBarObject()
		{
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000285DA File Offset: 0x000267DA
		public LoadingBarObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00028610 File Offset: 0x00026810
		protected override void CreateCSObject()
		{
			this.innerNode = new CSLoadingBar();
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00028620 File Offset: 0x00026820
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.ImageFileData = null;
				this.ProgressInfo = 80;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00028650 File Offset: 0x00026850
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x00028698 File Offset: 0x00026898
		[DisplayName("Display_ImageResources")]
		[PropertyOrder(73)]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[Category("Group_Feature")]
		[UndoProperty]
		public ResourceFile ImageFileData
		{
			get
			{
				if (this.file == null)
				{
					this.file = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetFileData()) as ResourceFile);
				}
				return this.file;
			}
			set
			{
				this.file = value;
				ImageFile defaultFile = new ImageFile(LoadingBarObjectData.DefaultFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
				this.GetInnerWidget().SetFileData(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.ImageFileData);
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00028714 File Offset: 0x00026914
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x00028734 File Offset: 0x00026934
		[UndoProperty]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[ValueRange(0, 100, 1f, 10f)]
		[DisplayName("Display_Progress")]
		[Category("Group_Feature")]
		[PropertyOrder(74)]
		public int ProgressInfo
		{
			get
			{
				return this.GetInnerWidget().GetProgressPercent();
			}
			set
			{
				int num = value;
				if (num < 0)
				{
					num = 0;
				}
				else if (num > 100)
				{
					num = 100;
				}
				this.GetInnerWidget().SetProgressPercent(num);
				this.RaisePropertyChanged<int>(() => this.ProgressInfo);
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x000287B0 File Offset: 0x000269B0
		// (set) Token: 0x06000A1E RID: 2590 RVA: 0x000287D0 File Offset: 0x000269D0
		[UndoProperty]
		[Category("Group_Feature")]
		[DisplayName("Display_ProgressType")]
		[PropertyOrder(75)]
		[DefaultValue(null)]
		public LoadingBarDirectionType ProgressType
		{
			get
			{
				return (LoadingBarDirectionType)this.GetInnerWidget().GetProgressType();
			}
			set
			{
				this.GetInnerWidget().SetProgressType((int)value);
				this.RaisePropertyChanged<LoadingBarDirectionType>(() => this.ProgressType);
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x00028828 File Offset: 0x00026A28
		// (set) Token: 0x06000A20 RID: 2592 RVA: 0x00028840 File Offset: 0x00026A40
		[Category("Display_Sudoku")]
		[UndoProperty]
		[LayoutRefresh]
		[Browsable(false)]
		[PropertyOrder(37)]
		[DisplayName("Display_Sudoku")]
		public virtual bool Scale9Enable
		{
			get
			{
				return this._scale9Enabled;
			}
			set
			{
				this._scale9Enabled = value;
				this.GetInnerWidget().SetScale9Enabled(value);
				this.RaisePropertyChanged<bool>(() => this.Scale9Enable);
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x000288A0 File Offset: 0x00026AA0
		// (set) Token: 0x06000A22 RID: 2594 RVA: 0x000288C0 File Offset: 0x00026AC0
		[UndoProperty]
		public virtual int LeftEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Left();
			}
			set
			{
				this._left = value;
				this.GetInnerWidget().SetScale9Left(this._left);
				this.RaisePropertyChanged<int>(() => this.LeftEage);
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x00028924 File Offset: 0x00026B24
		// (set) Token: 0x06000A24 RID: 2596 RVA: 0x00028944 File Offset: 0x00026B44
		[UndoProperty]
		public virtual int RightEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Right();
			}
			set
			{
				this._right = value;
				this.GetInnerWidget().SetScale9Right(this._right);
				this.RaisePropertyChanged<int>(() => this.RightEage);
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x000289A8 File Offset: 0x00026BA8
		// (set) Token: 0x06000A26 RID: 2598 RVA: 0x000289C8 File Offset: 0x00026BC8
		[UndoProperty]
		public virtual int TopEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Top();
			}
			set
			{
				this._top = value;
				this.GetInnerWidget().SetScale9Top(this._top);
				this.RaisePropertyChanged<int>(() => this.TopEage);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00028A2C File Offset: 0x00026C2C
		// (set) Token: 0x06000A28 RID: 2600 RVA: 0x00028A4C File Offset: 0x00026C4C
		[UndoProperty]
		public virtual int BottomEage
		{
			get
			{
				return this.GetInnerWidget().GetScale9Bottom();
			}
			set
			{
				this._bottom = value;
				this.GetInnerWidget().SetScale9Bottom(this._bottom);
				this.RaisePropertyChanged<int>(() => this.BottomEage);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x00028AB0 File Offset: 0x00026CB0
		public SizeF ResourceSize
		{
			get
			{
				return this.GetInnerWidget().GetWidgetAutoSize();
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x00028AD0 File Offset: 0x00026CD0
		// (set) Token: 0x06000A2B RID: 2603 RVA: 0x00028AED File Offset: 0x00026CED
		public virtual int Scale9OriginX
		{
			get
			{
				return this.GetInnerWidget().GetScale9OriginX();
			}
			set
			{
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x00028AF0 File Offset: 0x00026CF0
		// (set) Token: 0x06000A2D RID: 2605 RVA: 0x00028B0D File Offset: 0x00026D0D
		public virtual int Scale9OriginY
		{
			get
			{
				return this.GetInnerWidget().GetScale9OriginY();
			}
			set
			{
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x00028B10 File Offset: 0x00026D10
		// (set) Token: 0x06000A2F RID: 2607 RVA: 0x00028B2D File Offset: 0x00026D2D
		public virtual int Scale9Width
		{
			get
			{
				return this.GetInnerWidget().GetScale9Width();
			}
			set
			{
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x00028B30 File Offset: 0x00026D30
		// (set) Token: 0x06000A31 RID: 2609 RVA: 0x00028B4D File Offset: 0x00026D4D
		public virtual int Scale9Height
		{
			get
			{
				return this.GetInnerWidget().GetScale9Height();
			}
			set
			{
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x00028B50 File Offset: 0x00026D50
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			LoadingBarObject loadingBarObject = cObject as LoadingBarObject;
			if (loadingBarObject != null)
			{
				loadingBarObject.ImageFileData = this.ImageFileData;
				loadingBarObject.ProgressInfo = this.ProgressInfo;
				loadingBarObject.ProgressType = this.ProgressType;
				loadingBarObject.Scale9Enable = this.Scale9Enable;
				loadingBarObject.LeftEage = this.LeftEage;
				loadingBarObject.RightEage = this.RightEage;
				loadingBarObject.TopEage = this.TopEage;
				loadingBarObject.BottomEage = this.BottomEage;
				loadingBarObject.Size = this.Size;
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x00028BF0 File Offset: 0x00026DF0
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x0400045A RID: 1114
		private ResourceFile file = null;

		// Token: 0x0400045B RID: 1115
		private bool _scale9Enabled = false;

		// Token: 0x0400045C RID: 1116
		private int _left = 0;

		// Token: 0x0400045D RID: 1117
		private int _right = 0;

		// Token: 0x0400045E RID: 1118
		private int _top = 0;

		// Token: 0x0400045F RID: 1119
		private int _bottom = 0;
	}
}
