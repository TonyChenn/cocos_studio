using System;
using CocoStudio.Basic;
using Gtk;

namespace Modules.Communal.NewSolution
{
	public class GroupTypeContent : IRadioItemContent
	{
		public RadioGroup Group { get; private set; }

		public GroupTypeContent(RadioGroup group, string title)
		{
			this.Group = group;
			this.label_title = new Label(title);
			this.label_title.WidthRequest = 114;
			this.label_title.HeightRequest = 34;
			this.label_title.SetFontSize(12.0);
		}

		public Widget GetGtkWidget()
		{
			return this.label_title;
		}

		public void RefreshUI(bool isSelect, ButtonState currentState)
		{
			if (Option.CurrentApp != EnumApp.Launcher)
			{
				this.label_title.ModifyFg(StateType.Normal, NewSolutionStyles.White);
				return;
			}
			if (isSelect)
			{
				this.label_title.ModifyFg(StateType.Normal, NewSolutionStyles.White);
				return;
			}
			this.label_title.ModifyFg(StateType.Normal, NewSolutionStyles.Launcher_TextBlack);
		}

		private Label label_title;
	}
}
