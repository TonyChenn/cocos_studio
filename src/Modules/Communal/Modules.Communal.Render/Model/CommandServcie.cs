using System;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;

namespace Modules.Communal.Render.Model
{
	internal class CommandServcie : ICommandService
	{
		public void Initialize(IGLView glView)
		{
			this.selectServcie = SelectService.Instance;
			this.eventAggregator = Services.EventsService;
		}

		public virtual void DeleteObject()
		{
			using (CompositeTask.Run("CommandService.DeleteObject", this.selectServcie))
			{
				this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Publish(this.selectServcie.SelectedParentObjectList.ToList<VisualObject>().AsReadOnly());
			}
		}

		public virtual void CopyObject()
		{
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Publish(this.selectServcie.SelectedParentObjectList.ToList<VisualObject>().AsReadOnly());
		}

		public virtual void PasteObject(PasteObjectsChangeEventArgs args)
		{
			using (CompositeTask.Run("CommandService.PasteObject", this.selectServcie))
			{
				this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Publish(args);
			}
		}

		public virtual void CutObject()
		{
			using (CompositeTask.Run("CommandService.CutObject", this.selectServcie))
			{
				this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Publish(this.selectServcie.SelectedParentObjectList.ToList<VisualObject>().AsReadOnly());
			}
		}

		public virtual void RotateObject(float rotation)
		{
			foreach (VisualObject visualObject in this.selectServcie.SelectedObjectList)
			{
				visualObject.Rotation += rotation;
			}
		}

		protected SelectService selectServcie;

		protected IEventAggregator eventAggregator;
	}
}
