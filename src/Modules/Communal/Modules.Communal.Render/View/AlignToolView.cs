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
	internal class AlignToolView : HBox
	{
		public AlignToolView()
		{
			this.eventAggregator = Services.EventsService;
			this.InitView();
			this.InitEvent();
		}

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

		private void ArrangePublish(int count)
		{
			if (this.eventAggregator != null)
			{
				AlignVisualObjectsEvent @event = this.eventAggregator.GetEvent<AlignVisualObjectsEvent>();
				@event.Publish(count);
			}
		}

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

		private void button_verticalIsometry_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(9);
		}

		private void button_horizontalIsomertry_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(8);
		}

		private void button_bottom_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(7);
		}

		private void button_vertical_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(6);
		}

		private void button_top_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(5);
		}

		private void button_right_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(4);
		}

		private void button_horizontal_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(3);
		}

		private void button_left_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(2);
		}

		private void button_Center_Clicked(object sender, EventArgs e)
		{
			this.ArrangePublish(1);
		}

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

		private void ProjectOperations_CurrentDocumentChanged(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			this.AlignIsShow(0);
		}

		private IEventAggregator eventAggregator;

		private IconButton button_horizontalIsomertry;

		private IconButton button_verticalIsometry;

		private IconButton button_bottom;

		private IconButton button_horizontal;

		private IconButton button_top;

		private IconButton button_right;

		private IconButton button_vertical;

		private IconButton button_left;

		private IconButton button_Center;
	}
}
