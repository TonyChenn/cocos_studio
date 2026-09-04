using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000DF RID: 223
	[DisplayName("Property_NodeFile")]
	[EngineClassName("Node")]
	public class GameNodeObject : AbstractNodeObject
	{
		// Token: 0x0600070B RID: 1803 RVA: 0x0001CCE4 File Offset: 0x0001AEE4
		public GameNodeObject()
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001CCEF File Offset: 0x0001AEEF
		public GameNodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001CCFB File Offset: 0x0001AEFB
		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode2D();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001CD0C File Offset: 0x0001AF0C
		private CSNode2D GetInnerObject()
		{
			return this.innerNode as CSNode2D;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x0001CD2C File Offset: 0x0001AF2C
		// (set) Token: 0x06000710 RID: 1808 RVA: 0x0001CD4C File Offset: 0x0001AF4C
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

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0001CDB0 File Offset: 0x0001AFB0
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0001CDC8 File Offset: 0x0001AFC8
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

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0001CDD4 File Offset: 0x0001AFD4
		// (set) Token: 0x06000714 RID: 1812 RVA: 0x0001CDEC File Offset: 0x0001AFEC
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

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x0001CDF8 File Offset: 0x0001AFF8
		// (set) Token: 0x06000716 RID: 1814 RVA: 0x0001CE10 File Offset: 0x0001B010
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

		// Token: 0x06000717 RID: 1815 RVA: 0x0001CE60 File Offset: 0x0001B060
		public override bool CanReceiveDragObject(ModelDragData objectData, bool showLog)
		{
			return objectData != null && base.Parent == null;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001CE94 File Offset: 0x0001B094
		protected override void OnBindingRecorder()
		{
			ExtenderFactory.Binding(this, new BaseExtender[]
			{
				new ResourceExtender(this),
				new LayoutExtender(this)
			});
		}

		// Token: 0x040002F5 RID: 757
		private string customClassName;
	}
}
