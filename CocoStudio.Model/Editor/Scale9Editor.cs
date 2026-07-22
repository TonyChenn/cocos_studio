using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000068 RID: 104
	internal class Scale9Editor : BaseEditor
	{
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0000F524 File Offset: 0x0000D724
		public override bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600036F RID: 879 RVA: 0x0000F538 File Offset: 0x0000D738
		public override bool IsMultiLine
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x0000F54C File Offset: 0x0000D74C
		protected override Widget OnCreateWidget()
		{
			this.table = new Table(3U, 3U, false);
			this.topEntry = new NoUndoNumEntry();
			this.bottomEntry = new NoUndoNumEntry();
			this.leftEntry = new NoUndoNumEntry();
			this.rightEntry = new NoUndoNumEntry();
			this.scale9 = new Scale9EventBox();
			this.checkButton = new CheckButtonEx();
			this.checkButton.Label = "";
			this.topTable = new Table(1U, 2U, false);
			this.noticeIcon = new TooltipIcon();
			this.noticeIcon.Text = LanguageInfo.Display_Scale9Notification;
			this.alignNotice = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.leftEntry.WidthRequest = (this.rightEntry.WidthRequest = (this.topEntry.WidthRequest = (this.bottomEntry.WidthRequest = 30)));
			this.leftEntry.DecimalPlaces = (this.rightEntry.DecimalPlaces = (this.topEntry.DecimalPlaces = (this.bottomEntry.DecimalPlaces = 0)));
			this.leftEntry.MinValue = (this.rightEntry.MinValue = (this.topEntry.MinValue = (this.bottomEntry.MinValue = 0)));
			this.topTable.Attach(this.checkButton, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.topTable.Attach(this.alignNotice, 1U, 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.topTable, 0U, 3U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.scale9, 0U, 1U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable = new Table(1U, 3U, false);
			this.rightTable = new Table(3U, 1U, false);
			this.bottomTable.Attach(this.leftEntry, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.Attach(this.rightEntry, 2U, 3U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.bottomTable.Attach(new Label
			{
				WidthRequest = 50
			}, 1U, 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.rightTable.Attach(this.topEntry, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.rightTable.Attach(this.bottomEntry, 0U, 1U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.rightTable.Attach(new Label
			{
				HeightRequest = 60
			}, 0U, 1U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.bottomTable, 0U, 1U, 2U, 3U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(this.rightTable, 1U, 2U, 1U, 2U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.Attach(new Label(), 2U, 3U, 1U, 2U, AttachOptions.Expand | AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.table.ShowAll();
			this.Scale9Object = (PropertyItem.FirstObject as IScale9);
			this.SetScale9MaxValue();
			base.SetControl();
			this.scale9.Sensitive = (this.bottomTable.Sensitive = (this.rightTable.Sensitive = this.checkButton.Active));
			this.leftEntry.EntryValueChanged += this.Entry_EntryValueChanged;
			this.rightEntry.EntryValueChanged += this.Entry_EntryValueChanged;
			this.topEntry.EntryValueChanged += this.Entry_EntryValueChanged;
			this.bottomEntry.EntryValueChanged += this.Entry_EntryValueChanged;
			this.checkButton.Clicked += this.checkButton_Clicked;
			Scale9EventBox scale9EventBox = this.scale9;
			scale9EventBox.Scale9EventChanged = (EventHandler<Scale9EventArgs>)Delegate.Combine(scale9EventBox.Scale9EventChanged, new EventHandler<Scale9EventArgs>(this.Scale9_EventChanged));
			return this.table;
		}

		// Token: 0x06000371 RID: 881 RVA: 0x0000F940 File Offset: 0x0000DB40
		private void Scale9_EventChanged(object sender, Scale9EventArgs e)
		{
			using (base.GetLock(true))
			{
				switch (e.Type)
				{
				case CurrentRange.Left:
					this.leftEntry.Value = (float)(e.Left - 2.0) / 100f * this.Scale9Object.ResourceSize.Width;
					this.Scale9Object.LeftEage = (int)this.leftEntry.Value;
					break;
				case CurrentRange.Right:
					this.rightEntry.Value = (float)(102.0 - e.Right) / 100f * this.Scale9Object.ResourceSize.Width;
					this.Scale9Object.RightEage = (int)this.rightEntry.Value;
					break;
				case CurrentRange.Top:
					this.topEntry.Value = (float)(e.Top - 2.0) / 100f * this.Scale9Object.ResourceSize.Height;
					this.Scale9Object.TopEage = (int)this.topEntry.Value;
					break;
				case CurrentRange.Bottom:
					this.bottomEntry.Value = (float)(102.0 - e.Bottom) / 100f * this.Scale9Object.ResourceSize.Height;
					this.Scale9Object.BottomEage = (int)this.bottomEntry.Value;
					break;
				}
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000FAEC File Offset: 0x0000DCEC
		private void Entry_EntryValueChanged(object sender, EntryIntEventArgs e)
		{
			if (this.Scale9Object != null)
			{
				EntryIntEx entryIntEx = sender as EntryIntEx;
				using (base.GetLock(true))
				{
					if (entryIntEx == this.topEntry)
					{
						this.Scale9Object.TopEage = (int)e.Value;
						this.scale9.DrawingArea.TopRange = (double)this.Scale9Object.TopEage / (double)((this.Scale9Object.ResourceSize.Height == 0f) ? 1f : this.Scale9Object.ResourceSize.Height) * 100.0 + 2.0;
						if (this.scale9.DrawingArea.TopRange >= this.scale9.DrawingArea.BottomRange)
						{
							this.scale9.DrawingArea.BottomRange = this.scale9.DrawingArea.TopRange;
							this.bottomEntry.Value = (float)(102.0 - this.scale9.DrawingArea.BottomRange) / 100f * this.Scale9Object.ResourceSize.Height;
						}
					}
					else if (entryIntEx == this.bottomEntry)
					{
						this.Scale9Object.BottomEage = (int)e.Value;
						this.scale9.DrawingArea.BottomRange = 102.0 - (double)this.Scale9Object.BottomEage / (double)((this.Scale9Object.ResourceSize.Height == 0f) ? 1f : this.Scale9Object.ResourceSize.Height) * 100.0;
						if (this.scale9.DrawingArea.BottomRange <= this.scale9.DrawingArea.TopRange)
						{
							this.scale9.DrawingArea.TopRange = this.scale9.DrawingArea.BottomRange;
							this.topEntry.Value = (float)(this.scale9.DrawingArea.TopRange - 2.0) / 100f * this.Scale9Object.ResourceSize.Height;
						}
					}
					else if (entryIntEx == this.leftEntry)
					{
						this.Scale9Object.LeftEage = (int)e.Value;
						this.scale9.DrawingArea.LeftRange = (double)this.Scale9Object.LeftEage / (double)((this.Scale9Object.ResourceSize.Width == 0f) ? 1f : this.Scale9Object.ResourceSize.Width) * 100.0 + 2.0;
						if (this.scale9.DrawingArea.LeftRange >= this.scale9.DrawingArea.RightRange)
						{
							this.scale9.DrawingArea.RightRange = this.scale9.DrawingArea.LeftRange;
							this.rightEntry.Value = (float)(102.0 - this.scale9.DrawingArea.RightRange) / 100f * this.Scale9Object.ResourceSize.Width;
						}
					}
					else if (entryIntEx == this.rightEntry)
					{
						this.Scale9Object.RightEage = (int)e.Value;
						this.scale9.DrawingArea.RightRange = 102.0 - (double)this.Scale9Object.RightEage / (double)((this.Scale9Object.ResourceSize.Width == 0f) ? 1f : this.Scale9Object.ResourceSize.Width) * 100.0;
						if (this.scale9.DrawingArea.RightRange <= this.scale9.DrawingArea.LeftRange)
						{
							this.scale9.DrawingArea.LeftRange = this.scale9.DrawingArea.RightRange;
							this.leftEntry.Value = (float)(this.scale9.DrawingArea.TopRange - 2.0) / 100f * this.Scale9Object.ResourceSize.Width;
						}
					}
					this.scale9.DrawingArea.QueueDraw();
				}
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		private void checkButton_Clicked(object sender, EventArgs e)
		{
			using (base.GetLock(true))
			{
				this.Scale9Object.Scale9Enable = (this.scale9.Sensitive = (this.bottomTable.Sensitive = (this.rightTable.Sensitive = this.checkButton.Active)));
				if (this.checkButton.Active && this.Scale9Object.LeftEage == 0 && this.Scale9Object.RightEage == 0 && this.Scale9Object.TopEage == 0 && this.Scale9Object.BottomEage == 0)
				{
					this.RefreshScale(true);
				}
			}
			base.ReportUserData("9Slice");
		}

		// Token: 0x06000374 RID: 884 RVA: 0x0001009C File Offset: 0x0000E29C
		private void SetScale9MaxValue()
		{
			if (this.Scale9Object != null && this.Scale9Object.ResourceSize != null)
			{
				this.RefreshScale(false);
				this.leftEntry.MaxValue = (this.rightEntry.MaxValue = (int)this.Scale9Object.ResourceSize.Width);
				this.topEntry.MaxValue = (this.bottomEntry.MaxValue = (int)this.Scale9Object.ResourceSize.Height);
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00010130 File Offset: 0x0000E330
		private void RefreshScale(bool forceRefresh = false)
		{
			if (forceRefresh)
			{
				this.RefreshScaleData(forceRefresh);
			}
			else
			{
				using (base.GetLock(true))
				{
					this.RefreshScaleData(forceRefresh);
				}
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001018C File Offset: 0x0000E38C
		private void RefreshScaleData(bool forceRefresh = false)
		{
			int num = (int)(this.Scale9Object.ResourceSize.Width * 0.33f);
			int num2 = (int)(this.Scale9Object.ResourceSize.Height * 0.33f);
			if (this.leftEntry.Value > (float)num || forceRefresh)
			{
				this.leftEntry.Value = (float)num;
				this.Scale9Object.LeftEage = num;
			}
			if (this.rightEntry.Value > (float)num || forceRefresh)
			{
				this.rightEntry.Value = (float)num;
				this.Scale9Object.RightEage = num;
			}
			if (this.topEntry.Value > (float)num2 || forceRefresh)
			{
				this.topEntry.Value = (float)num2;
				this.Scale9Object.TopEage = num2;
			}
			if (this.bottomEntry.Value > (float)num2 || forceRefresh)
			{
				this.bottomEntry.Value = (float)num2;
				this.Scale9Object.BottomEage = num2;
			}
			this.SetSacleDrawingArea();
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000102B4 File Offset: 0x0000E4B4
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FileData" || e.PropertyName == "NormalFileData")
			{
				this.SetScale9MaxValue();
				base.SetControl();
			}
			else if (e.PropertyName == "LeftEage" || e.PropertyName == "RightEage" || e.PropertyName == "TopEage" || e.PropertyName == "BottomEage" || e.PropertyName == "Scale9Enable")
			{
				base.SetControl();
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00010370 File Offset: 0x0000E570
		protected override void OnSetControl()
		{
			if (this.Scale9Object != null)
			{
				this.leftEntry.Value = (float)this.Scale9Object.LeftEage;
				this.rightEntry.Value = (float)this.Scale9Object.RightEage;
				this.topEntry.Value = (float)this.Scale9Object.TopEage;
				this.bottomEntry.Value = (float)this.Scale9Object.BottomEage;
				this.SetSacleDrawingArea();
				this.checkButton.Active = this.Scale9Object.Scale9Enable;
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00010414 File Offset: 0x0000E614
		private void SetSacleDrawingArea()
		{
			if (this.Scale9Object.ResourceSize.Height == 0f)
			{
				this.scale9.DrawingArea.TopRange = (this.scale9.DrawingArea.BottomRange = 0.0);
				this.scale9.DrawingArea.CanDraw = false;
				if (this.alignNotice.Child == null)
				{
					this.alignNotice.Add(this.noticeIcon);
					this.noticeIcon.Show();
				}
			}
			else
			{
				this.scale9.DrawingArea.TopRange = (double)this.Scale9Object.TopEage / (double)this.Scale9Object.ResourceSize.Height * 100.0 + 2.0;
				this.scale9.DrawingArea.BottomRange = 102.0 - (double)this.Scale9Object.BottomEage / (double)this.Scale9Object.ResourceSize.Height * 100.0;
				this.scale9.DrawingArea.CanDraw = true;
				this.alignNotice.RemoveChild();
			}
			if (this.Scale9Object.ResourceSize.Width == 0f)
			{
				this.scale9.DrawingArea.LeftRange = (this.scale9.DrawingArea.RightRange = 0.0);
				this.scale9.DrawingArea.CanDraw = false;
				if (this.alignNotice.Child == null)
				{
					this.alignNotice.Add(this.noticeIcon);
					this.noticeIcon.Show();
				}
			}
			else
			{
				this.scale9.DrawingArea.LeftRange = (double)this.Scale9Object.LeftEage / (double)this.Scale9Object.ResourceSize.Width * 100.0 + 2.0;
				this.scale9.DrawingArea.RightRange = 102.0 - (double)this.Scale9Object.RightEage / (double)this.Scale9Object.ResourceSize.Width * 100.0;
				this.scale9.DrawingArea.CanDraw = true;
				this.alignNotice.RemoveChild();
			}
		}

		// Token: 0x040001AF RID: 431
		private const float scale9DrawArea = 100f;

		// Token: 0x040001B0 RID: 432
		private const float maxDrawArea = 102f;

		// Token: 0x040001B1 RID: 433
		private const float minDrawArea = 2f;

		// Token: 0x040001B2 RID: 434
		private Table table;

		// Token: 0x040001B3 RID: 435
		private CheckButtonEx checkButton;

		// Token: 0x040001B4 RID: 436
		private NoUndoNumEntry topEntry;

		// Token: 0x040001B5 RID: 437
		private NoUndoNumEntry bottomEntry;

		// Token: 0x040001B6 RID: 438
		private NoUndoNumEntry leftEntry;

		// Token: 0x040001B7 RID: 439
		private NoUndoNumEntry rightEntry;

		// Token: 0x040001B8 RID: 440
		private Scale9EventBox scale9;

		// Token: 0x040001B9 RID: 441
		private IScale9 Scale9Object = null;

		// Token: 0x040001BA RID: 442
		private Table bottomTable;

		// Token: 0x040001BB RID: 443
		private Table rightTable;

		// Token: 0x040001BC RID: 444
		private Table topTable;

		// Token: 0x040001BD RID: 445
		private Alignment alignNotice;

		// Token: 0x040001BE RID: 446
		private TooltipIcon noticeIcon;
	}
}
