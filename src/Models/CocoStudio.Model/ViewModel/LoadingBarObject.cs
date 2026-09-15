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
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("LoadingBar")]
	[ModelExtension(true, 6)]
	[DisplayName("Display_Component_UILoadingBar")]
	public class LoadingBarObject : WidgetObject, IResetSize
	{
		private CSLoadingBar GetInnerWidget()
		{
			return (CSLoadingBar)this.innerNode;
		}

		public LoadingBarObject()
		{
		}

		public LoadingBarObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSLoadingBar();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.ImageFileData = null;
				this.ProgressInfo = 80;
			}
		}

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
