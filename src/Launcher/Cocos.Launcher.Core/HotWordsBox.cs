using System;
using Cocos.Launcher.Control;
using Gtk;

// Token: 0x02000059 RID: 89
public class HotWordsBox : EventBox
{
	// Token: 0x170000A1 RID: 161
	// (get) Token: 0x06000307 RID: 775 RVA: 0x0000C141 File Offset: 0x0000A341
	public string Text
	{
		get
		{
			return this.lable.Text;
		}
	}

	// Token: 0x06000308 RID: 776 RVA: 0x0000C150 File Offset: 0x0000A350
	public HotWordsBox(string text)
	{
		this.lable = new Label();
		this.lable.Text = text;
		this.lable.Xpad = 10;
		this.lable.Ypad = 4;
		this.SetNormalStyle();
		base.Add(this.lable);
		base.EnterNotifyEvent += this.HotWordsBox_EnterNotifyEvent;
		base.LeaveNotifyEvent += this.HotWordsBox_LeaveNotifyEvent;
		base.ShowAll();
	}

	// Token: 0x06000309 RID: 777 RVA: 0x0000C1CF File Offset: 0x0000A3CF
	public void SetNormalStyle()
	{
		this.lable.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
		base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLeftColor);
	}

	// Token: 0x0600030A RID: 778 RVA: 0x0000C1F8 File Offset: 0x0000A3F8
	private void HotWordsBox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
	{
		this.SetNormalStyle();
	}

	// Token: 0x0600030B RID: 779 RVA: 0x0000C200 File Offset: 0x0000A400
	private void HotWordsBox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
	{
		this.lable.ModifyFg(StateType.Normal, ConstantConfig.Colors.MainContentColor);
		base.ModifyBg(StateType.Normal, ConstantConfig.Colors.TabFontPressColor);
	}

	// Token: 0x0400011B RID: 283
	private Label lable;
}
