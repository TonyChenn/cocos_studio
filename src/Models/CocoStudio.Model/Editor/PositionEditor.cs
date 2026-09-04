using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200006D RID: 109
	internal class PositionEditor : BaseEditor
	{
		// Token: 0x060003AF RID: 943 RVA: 0x00011AE4 File Offset: 0x0000FCE4
		static PositionEditor()
		{
			PositionEditor.correspondProperties.Add("LayoutState");
			PositionEditor.correspondProperties.Add("AnchorPoint");
			PositionEditor.correspondProperties.Add("Position");
			PositionEditor.correspondProperties.Add("PrePosition");
			PositionEditor.correspondProperties.Add("PositionPercentXEnabled");
			PositionEditor.correspondProperties.Add("PositionPercentYEnabled");
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x00011B5C File Offset: 0x0000FD5C
		// (set) Token: 0x060003B1 RID: 945 RVA: 0x00011B74 File Offset: 0x0000FD74
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

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x00011BAC File Offset: 0x0000FDAC
		// (set) Token: 0x060003B3 RID: 947 RVA: 0x00011BC4 File Offset: 0x0000FDC4
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

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x00011BDC File Offset: 0x0000FDDC
		// (set) Token: 0x060003B5 RID: 949 RVA: 0x00011BFC File Offset: 0x0000FDFC
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

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060003B6 RID: 950 RVA: 0x00011C44 File Offset: 0x0000FE44
		// (set) Token: 0x060003B7 RID: 951 RVA: 0x00011C64 File Offset: 0x0000FE64
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

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060003B8 RID: 952 RVA: 0x00011CAC File Offset: 0x0000FEAC
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00011CC0 File Offset: 0x0000FEC0
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

		// Token: 0x060003BA RID: 954 RVA: 0x00011E0D File Offset: 0x0001000D
		protected override void OnReset()
		{
			base.OnReset();
			this.CanShowSwitchButton = true;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00011E20 File Offset: 0x00010020
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

		// Token: 0x060003BC RID: 956 RVA: 0x00012090 File Offset: 0x00010290
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

		// Token: 0x060003BD RID: 957 RVA: 0x00012180 File Offset: 0x00010380
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

		// Token: 0x060003BE RID: 958 RVA: 0x00012244 File Offset: 0x00010444
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

		// Token: 0x060003BF RID: 959 RVA: 0x00012308 File Offset: 0x00010508
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

		// Token: 0x060003C0 RID: 960 RVA: 0x000123E4 File Offset: 0x000105E4
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

		// Token: 0x060003C1 RID: 961 RVA: 0x000124C0 File Offset: 0x000106C0
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (PositionEditor.correspondProperties.Contains(e.PropertyName))
			{
				base.SetControl();
			}
		}

		// Token: 0x040001E2 RID: 482
		private static List<string> correspondProperties = new List<string>();

		// Token: 0x040001E3 RID: 483
		private FullEntryShell xMainEntry;

		// Token: 0x040001E4 RID: 484
		private FullEntryShell yMainEntry;

		// Token: 0x040001E5 RID: 485
		private NoUndoNumEntry xInnerEntry;

		// Token: 0x040001E6 RID: 486
		private NoUndoNumEntry yInnerEntry;

		// Token: 0x040001E7 RID: 487
		private ChangeUnitButton xUnitBtn;

		// Token: 0x040001E8 RID: 488
		private ChangeUnitButton yUnitBtn;

		// Token: 0x040001E9 RID: 489
		private bool _showSwitchButton;

		// Token: 0x040001EA RID: 490
		private bool _canShowSwitchButton = true;
	}
}
