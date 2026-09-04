using System;
using System.ComponentModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000094 RID: 148
	internal class PlayControlEditor : BaseEditor
	{
		// Token: 0x06000511 RID: 1297 RVA: 0x00015F3C File Offset: 0x0001413C
		protected override Widget OnCreateWidget()
		{
			this.proxyCom = (PropertyItem.FirstObject as IPlayControl);
			this.widget = new PlayControlWidget();
			this.widget.Play += this.widget_Play;
			this.widget.Stop += this.widget_Stop;
			this.ControlView(this.proxyCom.HasData());
			return this.widget;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00015FB4 File Offset: 0x000141B4
		private void widget_Stop(object sender, EventArgs e)
		{
			if (this.proxyCom != null)
			{
				this.proxyCom.IsPlaying = false;
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00015FDC File Offset: 0x000141DC
		private void widget_Play(object sender, EventArgs e)
		{
			if (this.proxyCom != null)
			{
				this.proxyCom.IsPlaying = true;
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00016004 File Offset: 0x00014204
		public override void HandlePropertyChanged(PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "FileData" && this.proxyCom != null)
			{
				this.ControlView(this.proxyCom.HasData());
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00016049 File Offset: 0x00014249
		protected override void OnSetControl()
		{
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0001604C File Offset: 0x0001424C
		private void ControlView(bool canedit = true)
		{
			this.widget.PlayButton.Sensitive = canedit;
			this.widget.StopButton.Sensitive = canedit;
		}

		// Token: 0x04000252 RID: 594
		private PlayControlWidget widget;

		// Token: 0x04000253 RID: 595
		private IPlayControl proxyCom;
	}
}
