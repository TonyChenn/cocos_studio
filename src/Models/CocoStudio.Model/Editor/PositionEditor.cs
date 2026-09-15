using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class PositionEditor : BaseEditor
	{
		static PositionEditor()
		{
			PositionEditor.correspondProperties.Add("LayoutState");
			PositionEditor.correspondProperties.Add("AnchorPoint");
			PositionEditor.correspondProperties.Add("Position");
			PositionEditor.correspondProperties.Add("PrePosition");
			PositionEditor.correspondProperties.Add("PositionPercentXEnabled");
			PositionEditor.correspondProperties.Add("PositionPercentYEnabled");
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

		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 2;
			this.xUnitBtn = new ChangeUnitButton();
			this.xMainEntry = new FullEntryShell("X", this.xInnerEntry, LanguageInfo.NewFile_Pixel, this.xUnitBtn);
			this.xMainEntry.HeightRequest = 25;
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 2;
			this.yUnitBtn = new ChangeUnitButton();
			this.yMainEntry = new FullEntryShell("Y", this.yInnerEntry, LanguageInfo.NewFile_Pixel, this.yUnitBtn);
			this.yMainEntry.HeightRequest = 25;
			HBoxProp2D hboxProp2D = new HBoxProp2D();
			hboxProp2D.Spacing = 4;
			hboxProp2D.PackStart(this.xMainEntry);
			hboxProp2D.PackStart(this.yMainEntry);
			hboxProp2D.ShowAll();
			base.SetControl();
			this.xInnerEntry.EntryValueChanged += this.XEntryValueChangedHandler;
			this.yInnerEntry.EntryValueChanged += this.YEntryValueChangedHandler;
			this.xUnitBtn.UnitChanged += this.XUnitChangedHandler;
			this.yUnitBtn.UnitChanged += this.YUnitChangedHandler;
			return hboxProp2D;
		}

		protected override void OnReset()
		{
			base.OnReset();
			this.CanShowSwitchButton = true;
		}

		protected override void OnSetControl()
		{
			NodeObject nodeObject = PropertyItem.FirstObject as NodeObject;
			PointF position = nodeObject.Position;
			PointF prePosition = nodeObject.PrePosition;
			bool flag = nodeObject.PositionPercentXEnabled;
			bool flag2 = nodeObject.PositionPercentYEnabled;
			if (PropertyItem.Objects.Count > 1)
			{
				Func<NodeObject, NodeObject, bool> func = (NodeObject a, NodeObject b) => a.PositionPercentXEnabled == b.PositionPercentXEnabled;
				if (base.IsWhipNode<NodeObject>(func))
				{
					flag = false;
				}
				Func<NodeObject, NodeObject, bool> func2 = (NodeObject a, NodeObject b) => a.PositionPercentYEnabled == b.PositionPercentYEnabled;
				if (base.IsWhipNode<NodeObject>(func2))
				{
					flag2 = false;
				}
				Func<NodeObject, NodeObject, bool> func3 = (NodeObject a, NodeObject b) => a.Position.X == b.Position.X;
				Func<NodeObject, NodeObject, bool> func4 = (NodeObject a, NodeObject b) => a.Position.Y == b.Position.Y;
				Func<NodeObject, NodeObject, bool> func5 = (NodeObject a, NodeObject b) => a.PrePosition.X == b.PrePosition.X;
				Func<NodeObject, NodeObject, bool> func6 = (NodeObject a, NodeObject b) => a.PrePosition.Y == b.PrePosition.Y;
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
					this.xInnerEntry.Value = (flag ? (prePosition.X * 100f) : position.X);
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
					this.yInnerEntry.Value = (flag2 ? (prePosition.Y * 100f) : position.Y);
				}
			}
			else
			{
				this.xInnerEntry.Value = (flag ? (prePosition.X * 100f) : position.X);
				this.yInnerEntry.Value = (flag2 ? (prePosition.Y * 100f) : position.Y);
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
						nodeObject.PositionPercentXEnabled = this.xUnitBtn.IsPercent;
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
						nodeObject.PositionPercentYEnabled = this.yUnitBtn.IsPercent;
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
							PointF prePosition = nodeObject.PrePosition;
							prePosition.X = e.Value * 0.01f;
							nodeObject.PrePosition = prePosition;
						}
						else
						{
							PointF position = nodeObject.Position;
							position.X = e.Value;
							nodeObject.Position = position;
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
						if (this.yUnitBtn.IsPercent)
						{
							PointF prePosition = nodeObject.PrePosition;
							prePosition.Y = e.Value * 0.01f;
							nodeObject.PrePosition = prePosition;
						}
						else
						{
							PointF position = nodeObject.Position;
							position.Y = e.Value;
							nodeObject.Position = position;
						}
					}
				}
			}
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (PositionEditor.correspondProperties.Contains(e.PropertyName))
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
