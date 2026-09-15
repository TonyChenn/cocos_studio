using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class PlayControlEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.proxyCom = (PropertyItem.FirstObject as IPlayControl);
			this.widget = new PlayControlWidget();
			this.widget.Play += this.widget_Play;
			this.widget.Stop += this.widget_Stop;
			this.ControlView(this.proxyCom.HasData());
			return this.widget;
		}

		private void widget_Stop(object sender, EventArgs e)
		{
			if (this.proxyCom != null)
			{
				this.proxyCom.IsPlaying = false;
			}
		}

		private void widget_Play(object sender, EventArgs e)
		{
			if (this.proxyCom != null)
			{
				this.proxyCom.IsPlaying = true;
			}
		}

		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FileData" && this.proxyCom != null)
			{
				this.ControlView(this.proxyCom.HasData());
			}
		}

		protected override void OnSetControl()
		{
		}

		private void ControlView(bool canedit = true)
		{
			this.widget.PlayButton.Sensitive = canedit;
			this.widget.StopButton.Sensitive = canedit;
		}

		private PlayControlWidget widget;

		private IPlayControl proxyCom;
	}
}
