using System;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000095 RID: 149
	public class PlayControlWidget : EventBox
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001607C File Offset: 0x0001427C
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x00016093 File Offset: 0x00014293
		public Button PlayButton { get; private set; }

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001609C File Offset: 0x0001429C
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x000160B3 File Offset: 0x000142B3
		public Button StopButton { get; private set; }

		// Token: 0x0600051C RID: 1308 RVA: 0x000160BC File Offset: 0x000142BC
		public PlayControlWidget()
		{
			this.ContentTable = new Table(1U, 2U, false);
			this.PlayButton = new Button();
			this.PlayButton.WidthRequest = 60;
			this.PlayButton.HeightRequest = 25;
			this.PlayButton.Label = LanguageInfo.Command_Play;
			this.ContentTable.ColumnSpacing = 6U;
			this.StopButton = new Button();
			this.StopButton.WidthRequest = 60;
			this.StopButton.HeightRequest = 25;
			this.StopButton.Label = LanguageInfo.Command_Stop;
			this.ContentTable.Attach(this.PlayButton, 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Expand, 0U, 0U);
			this.ContentTable.Attach(this.StopButton, 1U, 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Expand, 0U, 0U);
			this.PlayButton.Show();
			this.StopButton.Show();
			base.Add(this.ContentTable);
			this.ContentTable.Show();
			this.PlayButton.Clicked += this.play_Clicked;
			this.StopButton.Clicked += this.stop_Clicked;
			base.ShowAll();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000161FC File Offset: 0x000143FC
		private void stop_Clicked(object sender, EventArgs e)
		{
			if (this.Stop != null)
			{
				this.Stop(this, null);
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00016228 File Offset: 0x00014428
		private void play_Clicked(object sender, EventArgs e)
		{
			if (this.Play != null)
			{
				this.Play(this, null);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600051F RID: 1311 RVA: 0x00016254 File Offset: 0x00014454
		// (remove) Token: 0x06000520 RID: 1312 RVA: 0x00016290 File Offset: 0x00014490
		public event EventHandler Play;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000521 RID: 1313 RVA: 0x000162CC File Offset: 0x000144CC
		// (remove) Token: 0x06000522 RID: 1314 RVA: 0x00016308 File Offset: 0x00014508
		public event EventHandler Stop;

		// Token: 0x04000254 RID: 596
		private Table ContentTable;
	}
}
