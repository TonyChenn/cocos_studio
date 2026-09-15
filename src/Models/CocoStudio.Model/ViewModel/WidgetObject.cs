using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	public class WidgetObject : NodeObject, IStretchSize
	{
		[Browsable(false)]
		public CSWidget CustomWidgetInstance { get; private set; }

		private CSWidget GetInnerWidget()
		{
			return this.innerNode as CSWidget;
		}

		protected void SetDefaultSizeType(bool isCustom)
		{
			if (this.GetInnerWidget() != null)
			{
				this.GetInnerWidget().SetCustomSizeEnabled(isCustom);
			}
		}

		public WidgetObject()
		{
			this.SetDefaultSizeType(true);
		}

		public WidgetObject(ScriptFileData fileData) : base(fileData)
		{
			this.SetDefaultSizeType(true);
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSWidget();
		}

		[UndoProperty]
		[Browsable(true)]
		[DisplayName("Display_CanUse")]
		[Category("Group_Routine")]
		[DefaultValue(true)]
		[PropertyOrder(2)]
		public virtual bool TouchEnable
		{
			get
			{
				return this.GetInnerWidget().GetTouchEnabled();
			}
			set
			{
				if (this.GetInnerWidget().GetTouchEnabled() != value)
				{
					this.GetInnerWidget().SetTouchEnabled(value);
					this.RaisePropertyChanged<bool>(() => this.TouchEnable);
				}
			}
		}

		[PropertyOrder(1001)]
		[UndoProperty]
		[DisplayName("CallBack_Method")]
		[Editor(typeof(CallBackPropertyEditor), typeof(CallBackPropertyEditor))]
		[Browsable(true)]
		[Category("Group_Advanced")]
		public override EnumCallBack CallBackType
		{
			get
			{
				return this.callBackType;
			}
			set
			{
				this.callBackType = value;
				this.RaisePropertyChanged<EnumCallBack>(() => this.CallBackType);
			}
		}

		[UndoProperty]
		public override string CallBackName
		{
			get
			{
				return this.callBackName;
			}
			set
			{
				this.callBackName = value;
				this.RaisePropertyChanged<string>(() => this.CallBackName);
			}
		}

		[UndoProperty]
		public override bool PercentWidthEnable
		{
			get
			{
				return this.GetInnerWidget().GetPercentWidthEnable();
			}
			set
			{
				this.GetInnerWidget().SetPercentWidthEnable(value);
				string taskName = base.GetType().Name + "PercentWidthEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.StretchWidthEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.PercentWidthEnable);
				}
			}
		}

		[UndoProperty]
		public override bool PercentHeightEnable
		{
			get
			{
				return this.GetInnerWidget().GetPercentHeightEnable();
			}
			set
			{
				this.GetInnerWidget().SetPercentHeightEnable(value);
				this.RaisePropertyChanged<bool>(() => this.PercentHeightEnable);
				string taskName = base.GetType().Name + "PercentHeightEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.StretchHeightEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.PercentHeightEnable);
				}
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual bool StretchWidthEnable
		{
			get
			{
				return this.GetInnerWidget().GetStretchWidthEnable();
			}
			set
			{
				this.GetInnerWidget().SetStretchWidthEnable(value);
				string taskName = base.GetType().Name + "StretchWidthEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.PercentWidthEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.StretchWidthEnable);
				}
			}
		}

		[LayoutRefresh]
		[UndoProperty]
		public virtual bool StretchHeightEnable
		{
			get
			{
				return this.GetInnerWidget().GetStretchHeightEnable();
			}
			set
			{
				this.GetInnerWidget().SetStretchHeightEnable(value);
				string taskName = base.GetType().Name + "StretchHeightEnable";
				using (CompositeTask.Run(taskName, null))
				{
					if (value)
					{
						this.PercentHeightEnable = false;
					}
					this.RaisePropertyChanged<bool>(() => this.StretchHeightEnable);
				}
			}
		}

		public virtual bool CanShowStretch
		{
			get
			{
				return this.canStretch;
			}
			set
			{
				this.canStretch = value;
			}
		}

		protected override void SetValue(object cObject)
		{
			WidgetObject widgetObject = cObject as WidgetObject;
			if (widgetObject.GetInnerWidget() != null)
			{
				widgetObject.GetInnerWidget().CloneWidgetCustomProperty(this.GetInnerWidget());
			}
			base.SetValue(cObject);
			WidgetObject widgetObject2 = cObject as WidgetObject;
			if (widgetObject2 != null)
			{
				widgetObject2.TouchEnable = this.TouchEnable;
				widgetObject2.CallBackName = this.CallBackName;
				widgetObject2.CallBackType = this.CallBackType;
				widgetObject2.StretchWidthEnable = this.StretchWidthEnable;
				widgetObject2.StretchHeightEnable = this.StretchHeightEnable;
			}
		}

		public override void ResetSize()
		{
			this.Size = this.GetInnerWidget().GetWidgetAutoSize();
		}

		private EnumCallBack callBackType = EnumCallBack.None;

		protected string callBackName = "";

		private bool canStretch = true;
	}
}
