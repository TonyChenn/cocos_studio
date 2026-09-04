using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200002A RID: 42
	public class Slice3DObject : Node3DObject, IFlipped
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x000068A0 File Offset: 0x00004AA0
		public Slice3DObject()
		{
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x000068A8 File Offset: 0x00004AA8
		public Slice3DObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
			this.Name = this.FileData.FileName.FileNameWithoutExtension;
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x000068DB File Offset: 0x00004ADB
		public Slice3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000068E4 File Offset: 0x00004AE4
		protected override void CreateCSObject()
		{
			this.innerNode = new CSSlice3D();
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000068F1 File Offset: 0x00004AF1
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00006904 File Offset: 0x00004B04
		private CSSlice3D GetInnerWidget()
		{
			return (CSSlice3D)this.innerNode;
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00006911 File Offset: 0x00004B11
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0000691C File Offset: 0x00004B1C
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[Description("Description_File")]
		[DisplayName("Display_ImageResource")]
		[Category("Group_Feature")]
		[Browsable(true)]
		[PropertyOrder(82)]
		[UndoProperty]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
		public ResourceFile FileData
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
				if (this.file == null || this.file.DataError != null)
				{
					this.file = new MeshFile(Slice3DObjectData.DefaultFile);
				}
				this.GetInnerWidget().SetFileData(this.file.GetResourceData());
				string taskName = base.GetType().Name + "FileData";
				using (CompositeTask.Run(taskName, null))
				{
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000069A8 File Offset: 0x00004BA8
		// (set) Token: 0x060001AC RID: 428 RVA: 0x000069C4 File Offset: 0x00004BC4
		[Browsable(true)]
		[Category("Group_Routine")]
		[UndoProperty]
		[Editor(typeof(SliceSizeEditor), typeof(SliceSizeEditor))]
		[DefaultValue(null)]
		[DisplayName("Display_ImageSize")]
		[PropertyOrder(23)]
		public SizeF SliceSize
		{
			get
			{
				return this.GetInnerWidget().GetSliceSize();
			}
			set
			{
				this.GetInnerWidget().SetSliceSize(value);
				this.RaisePropertyChanged<SizeF>(() => this.SliceSize);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00006A17 File Offset: 0x00004C17
		// (set) Token: 0x060001AE RID: 430 RVA: 0x00006A2A File Offset: 0x00004C2A
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

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00006A44 File Offset: 0x00004C44
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x00006A54 File Offset: 0x00004C54
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

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00006AB5 File Offset: 0x00004CB5
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x00006AC4 File Offset: 0x00004CC4
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

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00006B25 File Offset: 0x00004D25
		public bool IsReverse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00006B28 File Offset: 0x00004D28
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x00006B44 File Offset: 0x00004D44
		[UndoProperty]
		[Browsable(true)]
		[DisplayName("Display_AnimationSwitch")]
		[Category("Group_Routine")]
		[DefaultValue(false)]
		[PropertyOrder(24)]
		public virtual bool AnimationSwitch
		{
			get
			{
				return this.GetInnerWidget().getUVActive();
			}
			set
			{
				if (this.GetInnerWidget().getUVActive() != value)
				{
					this.GetInnerWidget().setUVActive(value);
					this.RaisePropertyChanged<bool>(() => this.AnimationSwitch);
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00006BA8 File Offset: 0x00004DA8
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00006BC4 File Offset: 0x00004DC4
		[DefaultValue(null)]
		[UndoProperty]
		[Editor(typeof(AnimationSpeedEditor), typeof(AnimationSpeedEditor))]
		[DisplayName("Display_AnimationSpeed")]
		[Category("Group_Routine")]
		[Browsable(true)]
		[PropertyOrder(25)]
		public PointF AnimationSpeed
		{
			get
			{
				return this.GetInnerWidget().GetAnimationSpeed();
			}
			set
			{
				this.GetInnerWidget().SetAnimationSpeed(value);
				this.RaisePropertyChanged<PointF>(() => this.AnimationSpeed);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x00006C18 File Offset: 0x00004E18
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00006C34 File Offset: 0x00004E34
		[Category("Group_Routine")]
		[UndoProperty]
		[DisplayName("Display_TextureSwitch")]
		[DefaultValue(false)]
		[PropertyOrder(26)]
		[Browsable(true)]
		public virtual bool TextureSwitch
		{
			get
			{
				return this.GetInnerWidget().GetTextureActive();
			}
			set
			{
				if (this.GetInnerWidget().GetTextureActive() != value)
				{
					this.GetInnerWidget().SetTextureActive(value);
					this.RaisePropertyChanged<bool>(() => this.TextureSwitch);
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00006C98 File Offset: 0x00004E98
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00006CB4 File Offset: 0x00004EB4
		[UndoProperty]
		[PropertyOrder(27)]
		[Editor(typeof(TextureAnimationEditor), typeof(TextureAnimationEditor))]
		[DefaultValue(null)]
		[DisplayName("Display_TextureRowAndColumn")]
		[Category("Group_Routine")]
		[Browsable(true)]
		public PointF TextureAnimation
		{
			get
			{
				return this.GetInnerWidget().GetRowAndColumn();
			}
			set
			{
				this.GetInnerWidget().SetRowAndColumn(value);
				this.RaisePropertyChanged<PointF>(() => this.TextureAnimation);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00006D07 File Offset: 0x00004F07
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00006D14 File Offset: 0x00004F14
		[DisplayName("Display_Framerate")]
		[UndoProperty]
		[Editor(typeof(FrameRateEditor), typeof(FrameRateEditor))]
		[Category("Group_Routine")]
		[Browsable(true)]
		[PropertyOrder(28)]
		public float framerate
		{
			get
			{
				return this.GetInnerWidget().GetFramerate();
			}
			set
			{
				this.GetInnerWidget().SetFramerate(value);
				this.RaisePropertyChanged<float>(() => this.framerate);
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00006D67 File Offset: 0x00004F67
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00006D74 File Offset: 0x00004F74
		[UndoProperty]
		[DefaultValue("")]
		[DisplayName("Display_BillBoardMode")]
		[Category("Group_Routine")]
		[PropertyOrder(29)]
		public BillBoardMode SliceBillBoardMode
		{
			get
			{
				return (BillBoardMode)this.GetInnerWidget().getBillBoardMode();
			}
			set
			{
				this.GetInnerWidget().SetBillBoardMode((int)value);
				this.RaisePropertyChanged<BillBoardMode>(() => this.SliceBillBoardMode);
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00006DC8 File Offset: 0x00004FC8
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			Slice3DObject slice3DObject = cObject as Slice3DObject;
			if (slice3DObject == null)
			{
				return;
			}
			slice3DObject.FileData = this.FileData;
			slice3DObject.FlipX = this.FlipX;
			slice3DObject.FlipY = this.FlipY;
			slice3DObject.SliceSize = this.SliceSize;
		}

		// Token: 0x0400008C RID: 140
		private ResourceFile file;
	}
}
