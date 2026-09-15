using System;
using Cocos.Launcher.Control;
using Gtk;

public class HotWordsBox : EventBox
{
	public string Text
	{
		get
		{
			return this.lable.Text;
		}
	}

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

	public void SetNormalStyle()
	{
		this.lable.ModifyFg(StateType.Normal, ConstantConfig.Colors.ContentLabelColor1);
		base.ModifyBg(StateType.Normal, ConstantConfig.Colors.MainLeftColor);
	}

	private void HotWordsBox_LeaveNotifyEvent(object o, LeaveNotifyEventArgs args)
	{
		this.SetNormalStyle();
	}

	private void HotWordsBox_EnterNotifyEvent(object o, EnterNotifyEventArgs args)
	{
		this.lable.ModifyFg(StateType.Normal, ConstantConfig.Colors.MainContentColor);
		base.ModifyBg(StateType.Normal, ConstantConfig.Colors.TabFontPressColor);
	}

	private Label lable;
}
