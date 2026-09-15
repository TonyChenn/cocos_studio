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
	internal class UISizeEditor : BaseEditor
	{
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

		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		public override bool IsMultiLine
		{
			get
			{
				return this.innerWidget != null && this.innerWidget is VBox;
			}
		}

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

		protected override void OnReset()
		{
			base.OnReset();
			this.CanShowSwitchButton = true;
		}

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

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (UISizeEditor.correspondProperties.Contains(e.PropertyName))
			{
				base.SetControl();
			}
		}

		private static List<string> correspondProperties = new List<string>();

		private FullEntryShell xMainEntry;

		private FullEntryShell yMainEntry;

		private NoUndoNumEntry xInnerEntry;

		private NoUndoNumEntry yInnerEntry;

		private ChangeUnitButton xUnitBtn;

		private ChangeUnitButton yUnitBtn;

		private bool _showSwitchButton;

		private bool _canShowSwitchButton = true;
	}
}
