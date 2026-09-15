using System;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model.Editor
{
	public class PlayControlWidget : EventBox
	{
		public Button PlayButton { get; private set; }

		public Button StopButton { get; private set; }

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

		private void stop_Clicked(object sender, EventArgs e)
		{
			if (this.Stop != null)
			{
				this.Stop(this, null);
			}
		}

		private void play_Clicked(object sender, EventArgs e)
		{
			if (this.Play != null)
			{
				this.Play(this, null);
			}
		}

		public event EventHandler Play;

		public event EventHandler Stop;

		private Table ContentTable;
	}
}
