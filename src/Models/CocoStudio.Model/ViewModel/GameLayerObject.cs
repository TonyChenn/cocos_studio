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
	[DisplayName("Property_LayerFile")]
	[EngineClassName("__LayerRGBA")]
	public class GameLayerObject : AbstractNodeObject
	{
		private CSLayer GetInnerObject()
		{
			return (CSLayer)this.innerNode;
		}

		public GameLayerObject()
		{
		}

		public GameLayerObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSLayer();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.TouchEnable = true;
		}

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

		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return objectData != null && base.Parent == null;
		}

		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new ResourceExtender(this),
				new LayoutExtender(this)
			});
		}

		private string customClassName;
	}
}
