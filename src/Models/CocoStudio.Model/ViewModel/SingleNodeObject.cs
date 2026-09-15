using System;
using System.ComponentModel;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[DisplayName("Display_Component_Entity")]
	[ControlGroup("Control_BaseObject", 0)]
	[EngineClassName("Node")]
	[ModelExtension(true, 13)]
	public class SingleNodeObject : NodeObject
	{
		public SingleNodeObject()
		{
		}

		public SingleNodeObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon("Object.png");
		}

		protected internal override string GetNamePrefix()
		{
			return "Node_";
		}

		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65511;
		}

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

		private string customClassName;
	}
}
