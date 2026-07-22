using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Event
{
	// Token: 0x020000AC RID: 172
	public class SelectedVisualObjectsChangeEventArgs
	{
		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001852C File Offset: 0x0001672C
		// (set) Token: 0x06000587 RID: 1415 RVA: 0x00018543 File Offset: 0x00016743
		public ReadOnlyCollection<VisualObject> SelectedObject { get; private set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000588 RID: 1416 RVA: 0x0001854C File Offset: 0x0001674C
		// (set) Token: 0x06000589 RID: 1417 RVA: 0x00018563 File Offset: 0x00016763
		public ReadOnlyCollection<VisualObject> SelectedParentObject { get; private set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001856C File Offset: 0x0001676C
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x00018583 File Offset: 0x00016783
		public string SourceName { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001858C File Offset: 0x0001678C
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x000185A3 File Offset: 0x000167A3
		public bool Handled { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x000185AC File Offset: 0x000167AC
		// (set) Token: 0x0600058F RID: 1423 RVA: 0x000185C3 File Offset: 0x000167C3
		public bool IsDoInNow { get; set; }

		// Token: 0x06000590 RID: 1424 RVA: 0x000185CC File Offset: 0x000167CC
		public SelectedVisualObjectsChangeEventArgs(IEnumerable<VisualObject> selectedObject, IEnumerable<VisualObject> selectedParentObject, bool isdoinnow = false)
		{
			this.SelectedObject = selectedObject.ToList<VisualObject>().AsReadOnly();
			this.SelectedParentObject = selectedParentObject.ToList<VisualObject>().AsReadOnly();
			this.IsDoInNow = isdoinnow;
		}
	}
}
