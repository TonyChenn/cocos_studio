using System;
using CocoStudio.Basic;
using Gtk;

namespace Modules.Communal.NewSolution
{
	// Token: 0x02000021 RID: 33
	public class GroupTypeContent : IRadioItemContent
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000FF RID: 255 RVA: 0x000083D6 File Offset: 0x000065D6
		// (set) Token: 0x06000100 RID: 256 RVA: 0x000083DE File Offset: 0x000065DE
		public RadioGroup Group { get; private set; }

		// Token: 0x06000101 RID: 257 RVA: 0x000083E8 File Offset: 0x000065E8
		public GroupTypeContent(RadioGroup group, string title)
		{
			this.Group = group;
			this.label_title = new Label(title);
			this.label_title.WidthRequest = 114;
			this.label_title.HeightRequest = 34;
			this.label_title.SetFontSize(12.0);
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000843C File Offset: 0x0000663C
		public Widget GetGtkWidget()
		{
			return this.label_title;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00008444 File Offset: 0x00006644
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

		// Token: 0x040000C4 RID: 196
		private Label label_title;
	}
}
