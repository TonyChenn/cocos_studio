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
	// Token: 0x02000138 RID: 312
	[DisplayName("Display_Component_Sprite")]
	[ControlGroup("Control_BaseObject", 0)]
	[ModelExtension(true, 2)]
	[EngineClassName("Sprite")]
	public class SpriteObject : NodeObject, IFlipped, IBlendFunc
	{
		// Token: 0x06000B92 RID: 2962 RVA: 0x0002D8F8 File Offset: 0x0002BAF8
		private CSSprite GetInnerWidget()
		{
			return (CSSprite)this.innerNode;
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x0002D915 File Offset: 0x0002BB15
		public SpriteObject()
		{
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x0002D927 File Offset: 0x0002BB27
		public SpriteObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x0002D93A File Offset: 0x0002BB3A
		public SpriteObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0002D94D File Offset: 0x0002BB4D
		protected override void CreateCSObject()
		{
			this.innerNode = new CSSprite();
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x0002D95C File Offset: 0x0002BB5C
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

		// Token: 0x06000B98 RID: 2968 RVA: 0x0002D9A4 File Offset: 0x0002BBA4
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.Filp = new FilpValue(false, false);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x0002D9D7 File Offset: 0x0002BBD7
		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65519;
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0002D9E8 File Offset: 0x0002BBE8
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0002DA00 File Offset: 0x0002BC00
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

		// Token: 0x06000B9C RID: 2972 RVA: 0x0002DA03 File Offset: 0x0002BC03
		internal override void OnResourcePropertyChanged()
		{
			this.RaisePropertyChanged<SizeF>(() => this.Size, false);
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000B9D RID: 2973 RVA: 0x0002DA44 File Offset: 0x0002BC44
		// (set) Token: 0x06000B9E RID: 2974 RVA: 0x0002DA8C File Offset: 0x0002BC8C
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

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0002DB98 File Offset: 0x0002BD98
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x0002DBBB File Offset: 0x0002BDBB
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

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x0002DBD8 File Offset: 0x0002BDD8
		// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x0002DBF8 File Offset: 0x0002BDF8
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

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x0002DC58 File Offset: 0x0002BE58
		// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x0002DC78 File Offset: 0x0002BE78
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

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x0002DCE4 File Offset: 0x0002BEE4
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x0002DD04 File Offset: 0x0002BF04
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

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x0002DD70 File Offset: 0x0002BF70
		public bool IsReverse
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002DD84 File Offset: 0x0002BF84
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

		// Token: 0x040004E7 RID: 1255
		private ResourceFile file = null;
	}
}
