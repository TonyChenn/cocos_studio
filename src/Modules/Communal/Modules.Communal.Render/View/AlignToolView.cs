using System;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.Render.View
{
	// Token: 0x02000033 RID: 51
	internal class AlignToolView : HBox
	{
		// Token: 0x0600021E RID: 542 RVA: 0x0000C18B File Offset: 0x0000A38B
		public AlignToolView()
		{
			this.eventAggregator = Services.EventsService;
			this.InitView();
			this.InitEvent();
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000C1B0 File Offset: 0x0000A3B0
		private void InitView()
		{
			base.Spacing = 6;
			this.button_horizontalIsomertry = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.horizontalIsometry.png"));
			this.button_verticalIsometry = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.verticalIsometry.png"));
			this.button_bottom = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.bottom.png"));
			this.button_horizontal = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.horizontal.png"));
			this.button_top = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.top.png"));
			this.button_right = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.right.png"));
			this.button_vertical = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.vertical.png"));
			this.button_left = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.left.png"));
			this.button_Center = new IconButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Alignment.center.png"));
			base.PackStart(this.button_Center, false, false, 0U);
			base.PackStart(new VSeparator(), false, false, 0U);
			base.PackStart(this.button_left, false, false, 0U);
			base.PackStart(this.button_horizontal, false, false, 0U);
			base.PackStart(this.button_right, false, false, 0U);
			base.PackStart(new VSeparator(), false, false, 0U);
			base.PackStart(this.button_top, false, false, 0U);
			base.PackStart(this.button_vertical, false, false, 0U);
			base.PackStart(this.button_bottom, false, false, 0U);
			base.PackStart(new VSeparator(), false, false, 0U);
			base.PackStart(this.button_horizontalIsomertry, false, false, 0U);
			base.PackStart(this.button_verticalIsometry, false, false, 0U);
			this.InitLanguage();
			this.AlignIsShow(0);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000C350 File Offset: 0x0000A550
		private void InitEvent()
		{
			this.button_Center.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_Center_Clicked);
			this.button_left.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_left_Clicked);
			this.button_vertical.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_vertical_Clicked);
			this.button_right.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_right_Clicked);
			this.button_top.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_top_Clicked);
			this.button_horizontal.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_horizontal_Clicked);
			this.button_bottom.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_bottom_Clicked);
			this.button_horizontalIsomertry.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_horizontalIsomertry_Clicked);
			this.button_verticalIsometry.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_verticalIsometry_Clicked);
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedObjectsChangeEventHandle));
			Services.ProjectOperations.CurrentProjectChanged += this.ProjectOperations_CurrentDocumentChanged;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000C46C File Offset: 0x0000A66C
		private void InitLanguage()
		{
			this.button_Center.TooltipText = LanguageInfo.Align_Center;
			this.button_left.TooltipText = LanguageInfo.Align_Left;
			this.button_vertical.TooltipText = LanguageInfo.Align_VerticalCenter;
			this.button_right.TooltipText = LanguageInfo.Align_Right;
			this.button_top.TooltipText = LanguageInfo.Align_Top;
			this.button_horizontal.TooltipText = LanguageInfo.Align_HorizontalCenter;
			this.button_bottom.TooltipText = LanguageInfo.Align_Bottom;
			this.button_horizontalIsomertry.TooltipText = LanguageInfo.Horizontal_Equidistance;
			this.button_verticalIsometry.TooltipText = LanguageInfo.Vertical_Equidistance;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000C514 File Offset: 0x0000A714
		private void ArrangePublish(int count)
		{
			if (this.eventAggregator != null)
			{
				AlignVisualObjectsEvent @event = this.eventAggregator.GetEvent<AlignVisualObjectsEvent>();
				@event.Publish(count);
			}
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000C548 File Offset: 0x0000A748
		private void AlignIsShow(int count)
		{
			bool sensitive = count > 1;
			bool sensitive2 = count > 2;
			this.button_Center.Sensitive = sensitive;
			this.button_left.Sensitive = sensitive;
			this.button_vertical.Sensitive = sensitive;
			this.button_right.Sensitive = sensitive;
			this.button_top.Sensitive = sensitive;
			this.button_horizontal.Sensitive = sensitive;
			this.button_bottom.Sensitive = sensitive;
			this.button_horizontalIsomertry.Sensitive = sensitive2;
			this.button_verticalIsometry.Sensitive = sensitive2;
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000C5DF File Offset: 0x0000A7DF
		private void button_verticalIsometry_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(9);
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000C5EB File Offset: 0x0000A7EB
		private void button_horizontalIsomertry_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(8);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000C5F6 File Offset: 0x0000A7F6
		private void button_bottom_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(7);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000C601 File Offset: 0x0000A801
		private void button_vertical_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(6);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C60C File Offset: 0x0000A80C
		private void button_top_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(5);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C617 File Offset: 0x0000A817
		private void button_right_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(4);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C622 File Offset: 0x0000A822
		private void button_horizontal_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(3);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000C62D File Offset: 0x0000A82D
		private void button_left_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(2);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000C638 File Offset: 0x0000A838
		private void button_Center_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(1);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000C644 File Offset: 0x0000A844
		private void SelectedObjectsChangeEventHandle(SelectedVisualObjectsChangeEventArgs obj)
		{
			bool flag = true;
			foreach (VisualObject visualObject in obj.SelectedParentObject)
			{
				AbstractNodeObject abstractNodeObject = visualObject as AbstractNodeObject;
				if (abstractNodeObject != null && !abstractNodeObject.OperationFlag.HasFlag(OperationMask.AlignFlag))
				{
					flag = false;
					break;
				}
			}
			this.AlignIsShow(flag ? obj.SelectedParentObject.Count : 1);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000C6E8 File Offset: 0x0000A8E8
		private void ProjectOperations_CurrentDocumentChanged(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			this.AlignIsShow(0);
		}

		// Token: 0x04000099 RID: 153
		private IEventAggregator eventAggregator;

		// Token: 0x0400009A RID: 154
		private IconButton button_horizontalIsomertry;

		// Token: 0x0400009B RID: 155
		private IconButton button_verticalIsometry;

		// Token: 0x0400009C RID: 156
		private IconButton button_bottom;

		// Token: 0x0400009D RID: 157
		private IconButton button_horizontal;

		// Token: 0x0400009E RID: 158
		private IconButton button_top;

		// Token: 0x0400009F RID: 159
		private IconButton button_right;

		// Token: 0x040000A0 RID: 160
		private IconButton button_vertical;

		// Token: 0x040000A1 RID: 161
		private IconButton button_left;

		// Token: 0x040000A2 RID: 162
		private IconButton button_Center;
	}
}
