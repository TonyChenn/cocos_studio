using System;
using System.ComponentModel;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F6 RID: 246
	[DisplayName("Display_Component_Entity")]
	[ControlGroup("Control_BaseObject", 0)]
	[EngineClassName("Node")]
	[ModelExtension(true, 13)]
	public class SingleNodeObject : NodeObject
	{
		// Token: 0x060008C0 RID: 2240 RVA: 0x0002307F File Offset: 0x0002127F
		public SingleNodeObject()
		{
		}

		// Token: 0x060008C1 RID: 2241 RVA: 0x0002308A File Offset: 0x0002128A
		public SingleNodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00023096 File Offset: 0x00021296
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon("Object.png");
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x000230B0 File Offset: 0x000212B0
		protected internal override string GetNamePrefix()
		{
			return "Node_";
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x000230C7 File Offset: 0x000212C7
		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65511;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060008C5 RID: 2245 RVA: 0x000230D8 File Offset: 0x000212D8
		// (set) Token: 0x060008C6 RID: 2246 RVA: 0x000230F0 File Offset: 0x000212F0
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("CallBack_ClassName")]
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

		// Token: 0x0400033C RID: 828
		private string customClassName;
	}
}
