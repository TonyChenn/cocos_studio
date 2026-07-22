using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000107 RID: 263
	public class WidgetObject : NodeObject, IStretchSize
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x00023234 File Offset: 0x00021434
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0002324B File Offset: 0x0002144B
		[Browsable(false)]
		public CSWidget CustomWidgetInstance { get; private set; }

		// Token: 0x060008CF RID: 2255 RVA: 0x00023254 File Offset: 0x00021454
		private CSWidget GetInnerWidget()
		{
			return this.innerNode as CSWidget;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00023274 File Offset: 0x00021474
		protected void SetDefaultSizeType(bool isCustom)
		{
			if (this.GetInnerWidget() != null)
			{
				this.GetInnerWidget().SetCustomSizeEnabled(isCustom);
			}
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x000232A1 File Offset: 0x000214A1
		public WidgetObject()
		{
			this.SetDefaultSizeType(true);
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x000232CD File Offset: 0x000214CD
		public WidgetObject(ScriptFileData fileData) : base(fileData)
		{
			this.SetDefaultSizeType(true);
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000232FA File Offset: 0x000214FA
		protected override void CreateCSObject()
		{
			this.innerNode = new CSWidget();
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060008D4 RID: 2260 RVA: 0x00023308 File Offset: 0x00021508
		// (set) Token: 0x060008D5 RID: 2261 RVA: 0x00023328 File Offset: 0x00021528
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

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060008D6 RID: 2262 RVA: 0x00023394 File Offset: 0x00021594
		// (set) Token: 0x060008D7 RID: 2263 RVA: 0x000233AC File Offset: 0x000215AC
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

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x000233FC File Offset: 0x000215FC
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x00023414 File Offset: 0x00021614
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

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x00023464 File Offset: 0x00021664
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x00023484 File Offset: 0x00021684
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

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00023530 File Offset: 0x00021730
		// (set) Token: 0x060008DD RID: 2269 RVA: 0x00023550 File Offset: 0x00021750
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

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060008DE RID: 2270 RVA: 0x00023634 File Offset: 0x00021834
		// (set) Token: 0x060008DF RID: 2271 RVA: 0x00023654 File Offset: 0x00021854
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

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00023700 File Offset: 0x00021900
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00023720 File Offset: 0x00021920
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

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x000237CC File Offset: 0x000219CC
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x000237E4 File Offset: 0x000219E4
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

		// Token: 0x060008E4 RID: 2276 RVA: 0x000237F0 File Offset: 0x000219F0
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

		// Token: 0x060008E5 RID: 2277 RVA: 0x00023883 File Offset: 0x00021A83
		public override void ResetSize()
		{
			this.Size = this.GetInnerWidget().GetWidgetAutoSize();
		}

		// Token: 0x04000419 RID: 1049
		private EnumCallBack callBackType = EnumCallBack.None;

		// Token: 0x0400041A RID: 1050
		protected string callBackName = "";

		// Token: 0x0400041B RID: 1051
		private bool canStretch = true;
	}
}
