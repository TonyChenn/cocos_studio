using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[DisplayName("Property_NodeFile")]
	[EngineClassName("Node")]
	public class GameNodeObject : AbstractNodeObject
	{
		public GameNodeObject()
		{
		}

		public GameNodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode2D();
		}

		private CSNode2D GetInnerObject()
		{
			return this.innerNode as CSNode2D;
		}

		[LayoutRefresh]
		public override SizeF Size
		{
			get
			{
				return this.GetCSVisual().GetSize();
			}
			set
			{
				if (value != null)
				{
					this.GetCSVisual().SetSize(value);
					this.RaisePropertyChanged<SizeF>(() => this.Size);
				}
			}
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

		[DisplayName("CallBack_ClassName")]
		[Browsable(true)]
		[Category("Group_Advanced")]
		[Editor(typeof(CallBackPropertyRootEditor), typeof(CallBackPropertyRootEditor))]
		[UndoProperty]
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
