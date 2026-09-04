using System;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.UndoManager;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000008 RID: 8
	internal class CommandServcie : ICommandService
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00005589 File Offset: 0x00003789
		public void Initialize(IGLView glView)
		{
			this.selectServcie = SelectService.Instance;
			this.eventAggregator = Services.EventsService;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000055A4 File Offset: 0x000037A4
		public virtual void DeleteObject()
		{
			using (CompositeTask.Run("CommandService.DeleteObject", this.selectServcie))
			{
				this.eventAggregator.GetEvent<DeleteVisualObjectsEvent>().Publish(this.selectServcie.SelectedParentObjectList.ToList<VisualObject>().AsReadOnly());
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00005610 File Offset: 0x00003810
		public virtual void CopyObject()
		{
			this.eventAggregator.GetEvent<CopyVisualObjectsEvent>().Publish(this.selectServcie.SelectedParentObjectList.ToList<VisualObject>().AsReadOnly());
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000563C File Offset: 0x0000383C
		public virtual void PasteObject(PasteObjectsChangeEventArgs args)
		{
			using (CompositeTask.Run("CommandService.PasteObject", this.selectServcie))
			{
				this.eventAggregator.GetEvent<PasteVisualObjectsEvent>().Publish(args);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00005694 File Offset: 0x00003894
		public virtual void CutObject()
		{
			using (CompositeTask.Run("CommandService.CutObject", this.selectServcie))
			{
				this.eventAggregator.GetEvent<CutVisualObjectsEvent>().Publish(this.selectServcie.SelectedParentObjectList.ToList<VisualObject>().AsReadOnly());
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00005700 File Offset: 0x00003900
		public virtual void RotateObject(float rotation)
		{
			foreach (VisualObject visualObject in this.selectServcie.SelectedObjectList)
			{
				visualObject.Rotation += rotation;
			}
		}

		// Token: 0x04000016 RID: 22
		protected SelectService selectServcie;

		// Token: 0x04000017 RID: 23
		protected IEventAggregator eventAggregator;
	}
}
