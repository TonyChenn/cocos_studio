using System;
using System.ComponentModel;
using Gdk;
using Gtk;
using Stetic;

namespace Modules.Communal.NewSolution
{
	// Token: 0x0200001B RID: 27
	[ToolboxItem(true)]
	public class RadioItemWidget : Bin, IRadioItem
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000076FC File Offset: 0x000058FC
		// (set) Token: 0x060000BB RID: 187 RVA: 0x00007704 File Offset: 0x00005904
		public bool UseBgColor { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BC RID: 188 RVA: 0x0000770D File Offset: 0x0000590D
		// (set) Token: 0x060000BD RID: 189 RVA: 0x00007715 File Offset: 0x00005915
		public Color NormalBgColor { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000771E File Offset: 0x0000591E
		// (set) Token: 0x060000BF RID: 191 RVA: 0x00007726 File Offset: 0x00005926
		public Color HoverBgColor { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000772F File Offset: 0x0000592F
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x00007737 File Offset: 0x00005937
		public Color SelectedBgColor { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00007740 File Offset: 0x00005940
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x00007748 File Offset: 0x00005948
		public Color BorderColor { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00007751 File Offset: 0x00005951
		// (set) Token: 0x060000C5 RID: 197 RVA: 0x00007759 File Offset: 0x00005959
		public bool IsSelected { get; private set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00007762 File Offset: 0x00005962
		// (set) Token: 0x060000C7 RID: 199 RVA: 0x0000776A File Offset: 0x0000596A
		public object Tag { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00007773 File Offset: 0x00005973
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x0000777B File Offset: 0x0000597B
		public IRadioItemContent GtkContent { get; private set; }

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060000CA RID: 202 RVA: 0x00007784 File Offset: 0x00005984
		// (remove) Token: 0x060000CB RID: 203 RVA: 0x000077BC File Offset: 0x000059BC
		public event EventHandler<RadioItemArgs> DoubleClicked;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060000CC RID: 204 RVA: 0x000077F4 File Offset: 0x000059F4
		// (remove) Token: 0x060000CD RID: 205 RVA: 0x0000782C File Offset: 0x00005A2C
		public event EventHandler<RadioItemArgs> Selected;

		// Token: 0x060000CE RID: 206 RVA: 0x00007864 File Offset: 0x00005A64
		public void Select()
		{
			this.IsSelected = true;
			if (this.Selected != null)
			{
				RadioItemArgs e = new RadioItemArgs(this);
				this.Selected(this, e);
			}
			this.RefreshUI();
		}

		// Token: 0x060000CF RID: 207 RVA: 0x0000789A File Offset: 0x00005A9A
		public void Unselect()
		{
			this.IsSelected = false;
			this.currentState = ButtonState.Normal;
			this.RefreshUI();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000078B0 File Offset: 0x00005AB0
		public Widget GetGtkWidget()
		{
			return this;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000078B3 File Offset: 0x00005AB3
		public RadioItemWidget()
		{
			this.Build();
			this.UseBgColor = true;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000078C8 File Offset: 0x00005AC8
		public void SetContent(IRadioItemContent content)
		{
			this.GtkContent = content;
			Widget gtkWidget = this.GtkContent.GetGtkWidget();
			this.evtbx_bg.Add(gtkWidget);
			this.GtkContent.RefreshUI(this.IsSelected, this.currentState);
			base.ShowAll();
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00007911 File Offset: 0x00005B11
		public void HideBorder()
		{
			this.evtbx_bg.BorderWidth = 0U;
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000791F File Offset: 0x00005B1F
		public void ShowBorder()
		{
			this.evtbx_bg.BorderWidth = 1U;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00007930 File Offset: 0x00005B30
		private void RefreshUI()
		{
			if (this.UseBgColor)
			{
				if (this.IsSelected)
				{
					this.evtbx_border.ModifyBg(StateType.Normal, this.BorderColor);
				}
				else
				{
					switch (this.currentState)
					{
					case ButtonState.Normal:
						this.evtbx_border.ModifyBg(StateType.Normal, this.NormalBgColor);
						break;
					case ButtonState.Hover:
						this.evtbx_border.ModifyBg(StateType.Normal, this.HoverBgColor);
						break;
					}
				}
				if (this.IsSelected)
				{
					this.evtbx_bg.ModifyBg(StateType.Normal, this.SelectedBgColor);
				}
				else
				{
					switch (this.currentState)
					{
					case ButtonState.Normal:
						this.evtbx_bg.ModifyBg(StateType.Normal, this.NormalBgColor);
						break;
					case ButtonState.Hover:
						this.evtbx_bg.ModifyBg(StateType.Normal, this.HoverBgColor);
						break;
					}
				}
			}
			this.GtkContent.RefreshUI(this.IsSelected, this.currentState);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00007A11 File Offset: 0x00005C11
		private void OnMouseEntered(object o, EnterNotifyEventArgs args)
		{
			this.isMouseIn = true;
			this.currentState = ButtonState.Hover;
			this.RefreshUI();
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00007A27 File Offset: 0x00005C27
		private void OnMouseLeaved(object o, LeaveNotifyEventArgs args)
		{
			this.isMouseIn = false;
			this.currentState = ButtonState.Normal;
			this.RefreshUI();
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00007A40 File Offset: 0x00005C40
		private void OnMousePressed(object o, ButtonPressEventArgs args)
		{
			if ((args.Event.Type == EventType.TwoButtonPress || args.Event.Type == EventType.ThreeButtonPress) && this.DoubleClicked != null)
			{
				RadioItemArgs e = new RadioItemArgs(this);
				this.DoubleClicked(this, e);
			}
			if (!this.IsSelected)
			{
				this.currentState = ButtonState.Pressed;
				this.Select();
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00007A9A File Offset: 0x00005C9A
		private void OnMouseReleased(object o, ButtonReleaseEventArgs args)
		{
			if (this.isMouseIn)
			{
				this.currentState = ButtonState.Hover;
			}
			else
			{
				this.currentState = ButtonState.Normal;
			}
			this.RefreshUI();
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00007ABC File Offset: 0x00005CBC
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.NewSolution.RadioItemWidget";
			this.evtbx_border = new EventBox();
			this.evtbx_border.Name = "evtbx_border";
			this.evtbx_bg = new EventBox();
			this.evtbx_bg.Name = "evtbx_bg";
			this.evtbx_bg.BorderWidth = 1U;
			this.evtbx_border.Add(this.evtbx_bg);
			base.Add(this.evtbx_border);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
			this.evtbx_border.LeaveNotifyEvent += this.OnMouseLeaved;
			this.evtbx_bg.ButtonPressEvent += this.OnMousePressed;
			this.evtbx_bg.ButtonReleaseEvent += this.OnMouseReleased;
			this.evtbx_bg.EnterNotifyEvent += this.OnMouseEntered;
			this.evtbx_bg.LeaveNotifyEvent += this.OnMouseLeaved;
		}

		// Token: 0x04000098 RID: 152
		private bool isMouseIn;

		// Token: 0x04000099 RID: 153
		private ButtonState currentState;

		// Token: 0x0400009C RID: 156
		private EventBox evtbx_border;

		// Token: 0x0400009D RID: 157
		private EventBox evtbx_bg;
	}
}
