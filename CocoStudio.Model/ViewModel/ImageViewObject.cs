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
	// Token: 0x0200010C RID: 268
	[ModelExtension(true, 3)]
	[EngineClassName("ImageView")]
	[ControlGroup("ComToolPad", 1)]
	[DisplayName("Display_Component_UIImageView")]
	public class ImageViewObject : WidgetObject, IScale9, IResetSize, IFlipped
	{
		// Token: 0x06000955 RID: 2389 RVA: 0x00025608 File Offset: 0x00023808
		private CSImageView GetInnerWidget()
		{
			return (CSImageView)this.innerNode;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00025625 File Offset: 0x00023825
		public ImageViewObject()
		{
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0002565A File Offset: 0x0002385A
		public ImageViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x00025690 File Offset: 0x00023890
		protected override void CreateCSObject()
		{
			this.innerNode = new CSImageView();
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x000256A0 File Offset: 0x000238A0
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.Filp = new FilpValue(false, false);
				this.FileData = null;
			}
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x000256D4 File Offset: 0x000238D4
		protected internal override string GetNamePrefix()
		{
			return "Image_";
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x000256EC File Offset: 0x000238EC
		// (set) Token: 0x0600095C RID: 2396 RVA: 0x0002570F File Offset: 0x0002390F
		[Category("Group_Routine")]
		[PropertyOrder(15)]
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Display_Flip")]
		[Editor(typeof(FilpEditor), typeof(FilpEditor))]
		[DefaultValue(false)]
		public virtual FilpValue Filp
		{
			get
			{
				return new FilpValue(this.FlipX, this.FlipY);
			}
			set
			{
				this.FlipX = value.FlipX;
				this.FlipY = value.FlipY;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0002572C File Offset: 0x0002392C
		// (set) Token: 0x0600095E RID: 2398 RVA: 0x0002574C File Offset: 0x0002394C
		public virtual bool FlipY
		{
			get
			{
				return this.GetInnerWidget().GetFlipY();
			}
			set
			{
				if (this.GetInnerWidget().GetFlipY() != value)
				{
					this.GetInnerWidget().SetFlipY(value);
					this.RaisePropertyChanged<FilpValue>(() => this.Filp);
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x000257B8 File Offset: 0x000239B8
		// (set) Token: 0x06000960 RID: 2400 RVA: 0x000257D8 File Offset: 0x000239D8
		public virtual bool FlipX
		{
			get
			{
				return this.GetInnerWidget().GetFlipX();
			}
			set
			{
				if (this.GetInnerWidget().GetFlipX() != value)
				{
					this.GetInnerWidget().SetFlipX(value);
					this.RaisePropertyChanged<FilpValue>(() => this.Filp);
				}
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x00025844 File Offset: 0x00023A44
		public bool IsReverse
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00025858 File Offset: 0x00023A58
		// (set) Token: 0x06000963 RID: 2403 RVA: 0x000258A0 File Offset: 0x00023AA0
		[DisplayName("Display_ImageResource")]
		[Category("Group_Feature")]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[PropertyOrder(82)]
		[DefaultValue(null)]
		[Description("Description_File")]
		public ResourceFile FileData
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
				ImageFile defaultFile = new ImageFile(ImageViewObjectData.DefaultFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
				this.GetInnerWidget().SetFileData(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x0002591C File Offset: 0x00023B1C
		// (set) Token: 0x06000965 RID: 2405 RVA: 0x00025934 File Offset: 0x00023B34
		[Browsable(true)]
		[DisplayName("Display_Sudoku")]
		[Editor(typeof(Scale9Editor), typeof(Scale9Editor))]
		[PropertyOrder(37)]
		[Category("Display_Sudoku")]
		[LayoutRefresh]
		[UndoProperty]
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

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00025994 File Offset: 0x00023B94
		// (set) Token: 0x06000967 RID: 2407 RVA: 0x000259B4 File Offset: 0x00023BB4
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

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00025A18 File Offset: 0x00023C18
		// (set) Token: 0x06000969 RID: 2409 RVA: 0x00025A38 File Offset: 0x00023C38
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

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00025A9C File Offset: 0x00023C9C
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00025ABC File Offset: 0x00023CBC
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

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00025B20 File Offset: 0x00023D20
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00025B40 File Offset: 0x00023D40
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

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x00025BA4 File Offset: 0x00023DA4
		public SizeF ResourceSize
		{
			get
			{
				return this.GetInnerWidget().GetWidgetAutoSize();
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x00025BC4 File Offset: 0x00023DC4
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x00025BE1 File Offset: 0x00023DE1
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

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x00025BE4 File Offset: 0x00023DE4
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x00025C01 File Offset: 0x00023E01
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

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x00025C04 File Offset: 0x00023E04
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x00025C21 File Offset: 0x00023E21
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

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x00025C24 File Offset: 0x00023E24
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x00025C41 File Offset: 0x00023E41
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

		// Token: 0x06000977 RID: 2423 RVA: 0x00025C44 File Offset: 0x00023E44
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			ImageViewObject imageViewObject = cObject as ImageViewObject;
			if (imageViewObject != null)
			{
				imageViewObject.FileData = this.FileData;
				imageViewObject.FlipX = this.FlipX;
				imageViewObject.FlipY = this.FlipY;
				imageViewObject.Scale9Enable = this.Scale9Enable;
				imageViewObject.LeftEage = this.LeftEage;
				imageViewObject.RightEage = this.RightEage;
				imageViewObject.TopEage = this.TopEage;
				imageViewObject.BottomEage = this.BottomEage;
				imageViewObject.Size = this.Size;
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00025CE4 File Offset: 0x00023EE4
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x0400043B RID: 1083
		private ResourceFile file = null;

		// Token: 0x0400043C RID: 1084
		private bool _scale9Enabled = false;

		// Token: 0x0400043D RID: 1085
		private int _left = 0;

		// Token: 0x0400043E RID: 1086
		private int _right = 0;

		// Token: 0x0400043F RID: 1087
		private int _top = 0;

		// Token: 0x04000440 RID: 1088
		private int _bottom = 0;
	}
}
