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
	[DisplayName("Display_Component_Sprite")]
	[ControlGroup("Control_BaseObject", 0)]
	[ModelExtension(true, 2)]
	[EngineClassName("Sprite")]
	public class SpriteObject : NodeObject, IFlipped, IBlendFunc
	{
		private CSSprite GetInnerWidget()
		{
			return (CSSprite)this.innerNode;
		}

		public SpriteObject()
		{
		}

		public SpriteObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
		}

		public SpriteObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSSprite();
		}

		protected internal override string GetNamePrefix()
		{
			string result;
			if (this.FileData != null)
			{
				result = this.FileData.FileName.FileNameWithoutExtension + "_";
			}
			else
			{
				result = "";
			}
			return result;
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.Filp = new FilpValue(false, false);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65519;
		}

		[RequestOperationMode(OperationMask.SizeFlag)]
		[Editor(typeof(UISizeEditor), typeof(UISizeEditor))]
		[DisplayName("grid_sudoku")]
		[Category("Group_PosAndSize")]
		[PropertyOrder(16)]
		[Browsable(true)]
		public override SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
			}
		}

		internal override void OnResourcePropertyChanged()
		{
			this.RaisePropertyChanged<SizeF>(() => this.Size, false);
		}

		[Browsable(true)]
		[PropertyOrder(82)]
		[UndoProperty]
		[FrameProperty]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DefaultValue(null)]
		[DisplayName("Display_ImageResource")]
		[Category("Group_Feature")]
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
				BlendFuncValue blendFunc = this.GetInnerWidget().GetBlendFunc();
				this.file = value;
				ImageFile defaultFile = new ImageFile(SpriteObjectData.DefaultFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
				this.GetInnerWidget().SetFileData(resourceFile.GetResourceData());
				if (null != blendFunc)
				{
					this.GetInnerWidget().SetBlendFunc(blendFunc);
				}
				using (CompositeTask.Run("Sprite FileData", null))
				{
					this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
					this.RaisePropertyChanged<BlendFuncValue>(() => this.BlendFunc, false);
				}
			}
		}

		[DisplayName("Display_Flip")]
		[Browsable(true)]
		[PropertyOrder(15)]
		[UndoProperty]
		[Editor(typeof(FilpEditor), typeof(FilpEditor))]
		[Category("Group_Routine")]
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

		[FrameProperty]
		[Editor(typeof(BlendFuncEditor), typeof(BlendFuncEditor))]
		[DisplayName("Animation_Blend_Blend")]
		[Browsable(true)]
		[PropertyOrder(110)]
		[UndoProperty]
		[Category("Group_Feature")]
		public BlendFuncValue BlendFunc
		{
			get
			{
				return this.GetInnerWidget().GetBlendFunc();
			}
			set
			{
				if (value != null)
				{
					this.GetInnerWidget().SetBlendFunc(value);
				}
				this.RaisePropertyChanged<BlendFuncValue>(() => this.BlendFunc);
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

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			SpriteObject spriteObject = cObject as SpriteObject;
			if (spriteObject != null)
			{
				spriteObject.FileData = this.FileData;
				spriteObject.FlipX = this.FlipX;
				spriteObject.FlipY = this.FlipY;
				spriteObject.BlendFunc = (this.BlendFunc.Clone() as BlendFuncValue);
			}
		}

		private ResourceFile file = null;
	}
}
