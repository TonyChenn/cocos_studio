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
	public class Slice3DObject : Node3DObject, IFlipped
	{
		public Slice3DObject()
		{
		}

		public Slice3DObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
			this.Name = this.FileData.FileName.FileNameWithoutExtension;
		}

		public Slice3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSSlice3D();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		private CSSlice3D GetInnerWidget()
		{
			return (CSSlice3D)this.innerNode;
		}

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

		public bool IsReverse
		{
			get
			{
				return false;
			}
		}

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

		private ResourceFile file;
	}
}
