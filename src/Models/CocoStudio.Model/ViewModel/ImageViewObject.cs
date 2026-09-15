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
	[ModelExtension(true, 3)]
	[EngineClassName("ImageView")]
	[ControlGroup("ComToolPad", 1)]
	[DisplayName("Display_Component_UIImageView")]
	public class ImageViewObject : WidgetObject, IScale9, IResetSize, IFlipped
	{
		private CSImageView GetInnerWidget()
		{
			return (CSImageView)this.innerNode;
		}

		public ImageViewObject()
		{
		}

		public ImageViewObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSImageView();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.Filp = new FilpValue(false, false);
				this.FileData = null;
			}
		}

		protected internal override string GetNamePrefix()
		{
			return "Image_";
		}

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
				return true;
			}
		}

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

		public SizeF ResourceSize
		{
			get
			{
				return this.GetInnerWidget().GetWidgetAutoSize();
			}
		}

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

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private ResourceFile file = null;

		private bool _scale9Enabled = false;

		private int _left = 0;

		private int _right = 0;

		private int _top = 0;

		private int _bottom = 0;
	}
}
