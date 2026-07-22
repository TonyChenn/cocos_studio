using System;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Visiter;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F2 RID: 242
	[DisplayName("Property_LayerFile")]
	[EngineClassName("__LayerRGBA")]
	public class GameLayerObject : AbstractNodeObject
	{
		// Token: 0x06000842 RID: 2114 RVA: 0x00020FD8 File Offset: 0x0001F1D8
		private CSLayer GetInnerObject()
		{
			return (CSLayer)this.innerNode;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00020FF5 File Offset: 0x0001F1F5
		public GameLayerObject()
		{
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00021000 File Offset: 0x0001F200
		public GameLayerObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002100C File Offset: 0x0001F20C
		protected override void CreateCSObject()
		{
			this.innerNode = new CSLayer();
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0002101A File Offset: 0x0001F21A
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.TouchEnable = true;
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x00021030 File Offset: 0x0001F230
		// (set) Token: 0x06000848 RID: 2120 RVA: 0x00021048 File Offset: 0x0001F248
		[Browsable(false)]
		public override int Tag
		{
			get
			{
				return base.Tag;
			}
			set
			{
				base.Tag = value;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00021054 File Offset: 0x0001F254
		// (set) Token: 0x0600084A RID: 2122 RVA: 0x0002106C File Offset: 0x0001F26C
		[Browsable(false)]
		public override string FrameEvent
		{
			get
			{
				return base.FrameEvent;
			}
			set
			{
				base.FrameEvent = value;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x00021078 File Offset: 0x0001F278
		// (set) Token: 0x0600084C RID: 2124 RVA: 0x00021098 File Offset: 0x0001F298
		[DisplayName("Display_CanUse")]
		[DefaultValue(false)]
		[Browsable(false)]
		[PropertyOrder(2)]
		[Category("Group_Routine")]
		[UndoProperty]
		public virtual bool TouchEnable
		{
			get
			{
				return this.GetInnerObject().IsTouchEnabled();
			}
			set
			{
				if (this.GetInnerObject().IsTouchEnabled() != value)
				{
					this.GetInnerObject().SetTouchEnabled(value);
					this.RaisePropertyChanged<bool>(() => this.TouchEnable);
				}
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00021104 File Offset: 0x0001F304
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0002111C File Offset: 0x0001F31C
		[Editor(typeof(LayerSizeEditor), typeof(LayerSizeEditor))]
		[Category("Group_PosAndSize")]
		[LayoutRefresh]
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Layer_size")]
		[PropertyOrder(0)]
		public virtual SizeF LayerSize
		{
			get
			{
				return base.Size;
			}
			set
			{
				this.GetCSVisual().SetSize(value);
				if (Services.ProjectOperations.CurrentSelectedProject.GetRootNode() == this)
				{
					GameWindow.Current.GetCanvasObject().Size = value;
				}
				this.RaisePropertyChanged<SizeF>(() => this.LayerSize);
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x000211A0 File Offset: 0x0001F3A0
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x000211B8 File Offset: 0x0001F3B8
		[Browsable(true)]
		[DisplayName("CallBack_ClassName")]
		[UndoProperty]
		[Category("Group_Advanced")]
		[Editor(typeof(CallBackPropertyRootEditor), typeof(CallBackPropertyRootEditor))]
		public override string CustomClassName
		{
			get
			{
				return this.customClassName;
			}
			set
			{
				this.customClassName = value;
				this.RaisePropertyChanged<string>(() => this.CustomClassName);
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00021208 File Offset: 0x0001F408
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			GameLayerObject gameLayerObject = cObject as GameLayerObject;
			if (gameLayerObject != null)
			{
				gameLayerObject.TouchEnable = this.TouchEnable;
				gameLayerObject.CustomClassName = this.CustomClassName;
				gameLayerObject.LayerSize = this.LayerSize;
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0002125C File Offset: 0x0001F45C
		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return objectData != null && base.Parent == null;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00021290 File Offset: 0x0001F490
		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new ResourceExtender(this),
				new LayoutExtender(this)
			});
		}

		// Token: 0x0400032B RID: 811
		private string customClassName;
	}
}
