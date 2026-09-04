using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200006F RID: 111
	internal class UISizeEditor : BaseEditor
	{
		// Token: 0x060003CF RID: 975 RVA: 0x00012740 File Offset: 0x00010940
		static UISizeEditor()
		{
			UISizeEditor.correspondProperties.Add("LayoutState");
			UISizeEditor.correspondProperties.Add("PercentWidthEnable");
			UISizeEditor.correspondProperties.Add("PercentHeightEnable");
			UISizeEditor.correspondProperties.Add("Size");
			UISizeEditor.correspondProperties.Add("PreSize");
			UISizeEditor.correspondProperties.Add("InnerNodeSize");
			UISizeEditor.correspondProperties.Add("IsCustomSize");
			UISizeEditor.correspondProperties.Add("LabelText");
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060003D0 RID: 976 RVA: 0x000127D8 File Offset: 0x000109D8
		// (set) Token: 0x060003D1 RID: 977 RVA: 0x000127F0 File Offset: 0x000109F0
		public bool ShowSwitchButton
		{
			get
			{
				return this._showSwitchButton;
			}
			set
			{
				this._showSwitchButton = value;
				if (!this.CanShowSwitchButton)
				{
					this.SetUnitButtonVisible(false);
				}
				else
				{
					this.SetUnitButtonVisible(this._showSwitchButton);
				}
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00012828 File Offset: 0x00010A28
		// (set) Token: 0x060003D3 RID: 979 RVA: 0x00012840 File Offset: 0x00010A40
		public bool CanShowSwitchButton
		{
			get
			{
				return this._canShowSwitchButton;
			}
			set
			{
				this._canShowSwitchButton = value;
				this.ShowSwitchButton = this.ShowSwitchButton;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060003D4 RID: 980 RVA: 0x00012858 File Offset: 0x00010A58
		// (set) Token: 0x060003D5 RID: 981 RVA: 0x00012878 File Offset: 0x00010A78
		public bool IsXPercent
		{
			get
			{
				return this.xUnitBtn.IsPercent;
			}
			set
			{
				this.xUnitBtn.IsPercent = value;
				if (value)
				{
					this.xMainEntry.UnitText = "%";
				}
				else
				{
					this.xMainEntry.UnitText = LanguageInfo.NewFile_Pixel;
				}
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003D6 RID: 982 RVA: 0x000128C0 File Offset: 0x00010AC0
		// (set) Token: 0x060003D7 RID: 983 RVA: 0x000128E0 File Offset: 0x00010AE0
		public bool IsYPercent
		{
			get
			{
				return this.yUnitBtn.IsPercent;
			}
			set
			{
				this.yUnitBtn.IsPercent = value;
				if (value)
				{
					this.yMainEntry.UnitText = "%";
				}
				else
				{
					this.yMainEntry.UnitText = LanguageInfo.NewFile_Pixel;
				}
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00012928 File Offset: 0x00010B28
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0001293C File Offset: 0x00010B3C
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00012950 File Offset: 0x00010B50
		public override bool IsMultiLine
		{
			get
			{
				return this.innerWidget != null && this.innerWidget is VBox;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00012988 File Offset: 0x00010B88
		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			this.xInnerEntry.MinValue = 0;
			this.xUnitBtn = new ChangeUnitButton();
			this.xMainEntry = new FullEntryShell("X", this.xInnerEntry, LanguageInfo.NewFile_Pixel, this.xUnitBtn);
			this.xMainEntry.HeightRequest = 25;
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			this.yInnerEntry.MinValue = 0;
			this.yUnitBtn = new ChangeUnitButton();
			this.yMainEntry = new FullEntryShell("Y", this.yInnerEntry, LanguageInfo.NewFile_Pixel, this.yUnitBtn);
			this.yMainEntry.HeightRequest = 25;
			HBoxProp2D hboxProp2D = new HBoxProp2D();
			hboxProp2D.Spacing = 4;
			hboxProp2D.PackStart(this.xMainEntry);
			hboxProp2D.PackStart(this.yMainEntry);
			hboxProp2D.ShowAll();
			Widget widget = this.CreateResetButton();
			Widget widget2;
			if (widget == null)
			{
				widget2 = hboxProp2D;
			}
			else
			{
				VBox vbox = new VBox();
				vbox.Spacing = (int)PropertyPadStyle.mainRowSpacing;
				vbox.PackStart(hboxProp2D);
				vbox.PackStart(widget);
				widget2 = vbox;
			}
			widget2.ShowAll();
			base.SetControl();
			this.xInnerEntry.EntryValueChanged += this.XEntryValueChangedHandler;
			this.yInnerEntry.EntryValueChanged += this.YEntryValueChangedHandler;
			this.xUnitBtn.UnitChanged += this.XUnitChangedHandler;
			this.yUnitBtn.UnitChanged += this.YUnitChangedHandler;
			return widget2;
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00012B3C File Offset: 0x00010D3C
		private Widget CreateResetButton()
		{
			foreach (object obj in PropertyItem.Objects)
			{
				if (!(obj is IResetSize))
				{
					return null;
				}
			}
			Button button = new Button(LanguageInfo.Property_ResetSize);
			button.WidthRequest = 90;
			button.Clicked += this.ResetButtonClickedHandler;
			HBox hbox = new HBox();
			hbox.PackStart(button, false, false, 0U);
			return hbox;
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00012BE8 File Offset: 0x00010DE8
		protected override void OnReset()
		{
			base.OnReset();
			this.CanShowSwitchButton = true;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00012BFC File Offset: 0x00010DFC
		protected override void OnSetControl()
		{
			NodeObject nodeObject = PropertyItem.FirstObject as NodeObject;
			SizeF size = nodeObject.Size;
			SizeF preSize = nodeObject.PreSize;
			bool flag = nodeObject.PercentWidthEnable;
			bool flag2 = nodeObject.PercentHeightEnable;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<NodeObject, NodeObject, bool> func = (NodeObject a, NodeObject b) => a.PercentWidthEnable == b.PercentWidthEnable;
				if (base.IsWhipNode<NodeObject>(func))
				{
					flag = false;
				}
				Func<NodeObject, NodeObject, bool> func2 = (NodeObject a, NodeObject b) => a.PercentHeightEnable == b.PercentHeightEnable;
				if (base.IsWhipNode<NodeObject>(func2))
				{
					flag2 = false;
				}
				Func<NodeObject, NodeObject, bool> func3 = (NodeObject a, NodeObject b) => a.Size.Width == b.Size.Width;
				Func<NodeObject, NodeObject, bool> func4 = (NodeObject a, NodeObject b) => a.Size.Height == b.Size.Height;
				Func<NodeObject, NodeObject, bool> func5 = (NodeObject a, NodeObject b) => a.PreSize.Width == b.PreSize.Width;
				Func<NodeObject, NodeObject, bool> func6 = (NodeObject a, NodeObject b) => a.PreSize.Height == b.PreSize.Height;
				Func<NodeObject, NodeObject, bool> func7;
				if (flag)
				{
					func7 = func5;
				}
				else
				{
					func7 = func3;
				}
				if (base.IsWhipNode<NodeObject>(func7))
				{
					this.xInnerEntry.SetToSubState();
				}
				else
				{
					this.xInnerEntry.Value = (flag ? (preSize.Width * 100f) : size.Width);
				}
				if (flag2)
				{
					func7 = func6;
				}
				else
				{
					func7 = func4;
				}
				if (base.IsWhipNode<NodeObject>(func7))
				{
					this.yInnerEntry.SetToSubState();
				}
				else
				{
					this.yInnerEntry.Value = (flag2 ? (preSize.Height * 100f) : size.Height);
				}
			}
			else
			{
				this.xInnerEntry.Value = (flag ? (preSize.Width * 100f) : size.Width);
				this.yInnerEntry.Value = (flag2 ? (preSize.Height * 100f) : size.Height);
			}
			if (this.IsXPercent != flag)
			{
				this.IsXPercent = flag;
			}
			if (this.IsYPercent != flag2)
			{
				this.IsYPercent = flag2;
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x00012E6C File Offset: 0x0001106C
		private void SetUnitButtonVisible(bool isVisible)
		{
			if (isVisible)
			{
				this.xMainEntry.AdditionWidget = this.xUnitBtn;
				this.xUnitBtn.State = StateType.Normal;
				this.yMainEntry.AdditionWidget = this.yUnitBtn;
				this.yUnitBtn.State = StateType.Normal;
			}
			else
			{
				bool isPercent = this.xUnitBtn.IsPercent;
				this.xMainEntry.AdditionWidget = null;
				this.xUnitBtn.IsPercent = false;
				if (isPercent != this.xUnitBtn.IsPercent)
				{
					this.XUnitChangedHandler(this, new EventArgs());
				}
				bool isPercent2 = this.yUnitBtn.IsPercent;
				this.yMainEntry.AdditionWidget = null;
				this.yUnitBtn.IsPercent = false;
				if (isPercent2 != this.yUnitBtn.IsPercent)
				{
					this.YUnitChangedHandler(this, new EventArgs());
				}
			}
			base.SetControl();
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00012F5C File Offset: 0x0001115C
		private void XUnitChangedHandler(object sender, EventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					NodeObject nodeObject = PropertyItem.Objects[i] as NodeObject;
					if (nodeObject != null)
					{
						nodeObject.PercentWidthEnable = this.xUnitBtn.IsPercent;
					}
				}
			}
			if (this.xUnitBtn.IsPercent)
			{
				this.xMainEntry.UnitText = "%";
			}
			else
			{
				this.xMainEntry.UnitText = LanguageInfo.NewFile_Pixel;
			}
			base.SetControl();
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00013020 File Offset: 0x00011220
		private void YUnitChangedHandler(object sender, EventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					NodeObject nodeObject = PropertyItem.Objects[i] as NodeObject;
					if (nodeObject != null)
					{
						nodeObject.PercentHeightEnable = this.yUnitBtn.IsPercent;
					}
				}
			}
			if (this.yUnitBtn.IsPercent)
			{
				this.yMainEntry.UnitText = "%";
			}
			else
			{
				this.yMainEntry.UnitText = LanguageInfo.NewFile_Pixel;
			}
			base.SetControl();
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x000130E4 File Offset: 0x000112E4
		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					NodeObject nodeObject = PropertyItem.Objects[i] as NodeObject;
					if (nodeObject != null)
					{
						if (this.xUnitBtn.IsPercent)
						{
							SizeF preSize = nodeObject.PreSize;
							preSize.Width = e.Value * 0.01f;
							nodeObject.PreSize = preSize;
						}
						else
						{
							SizeF size = nodeObject.Size;
							size.Width = e.Value;
							nodeObject.Size = size;
						}
					}
				}
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000131C0 File Offset: 0x000113C0
		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					NodeObject nodeObject = PropertyItem.Objects[i] as NodeObject;
					if (nodeObject != null)
					{
						if (this.xUnitBtn.IsPercent)
						{
							SizeF preSize = nodeObject.PreSize;
							preSize.Height = e.Value * 0.01f;
							nodeObject.PreSize = preSize;
						}
						else
						{
							SizeF size = nodeObject.Size;
							size.Height = e.Value;
							nodeObject.Size = size;
						}
					}
				}
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0001329C File Offset: 0x0001149C
		private void ResetButtonClickedHandler(object sender, EventArgs e)
		{
			using (CompositeTask.Run("ResetSize", null))
			{
				foreach (object obj in PropertyItem.Objects)
				{
					IResetSize resetSize = obj as IResetSize;
					if (resetSize != null)
					{
						resetSize.ResetSize();
					}
				}
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0001333C File Offset: 0x0001153C
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (UISizeEditor.correspondProperties.Contains(e.PropertyName))
			{
				base.SetControl();
			}
		}

		// Token: 0x040001F3 RID: 499
		private static List<string> correspondProperties = new List<string>();

		// Token: 0x040001F4 RID: 500
		private FullEntryShell xMainEntry;

		// Token: 0x040001F5 RID: 501
		private FullEntryShell yMainEntry;

		// Token: 0x040001F6 RID: 502
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x040001F7 RID: 503
		private NoUndoNumEntry yInnerEntry;

		// Token: 0x040001F8 RID: 504
		private ChangeUnitButton xUnitBtn;

		// Token: 0x040001F9 RID: 505
		private ChangeUnitButton yUnitBtn;

		// Token: 0x040001FA RID: 506
		private bool _showSwitchButton;

		// Token: 0x040001FB RID: 507
		private bool _canShowSwitchButton = true;
	}
}
