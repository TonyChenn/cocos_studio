using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model.Event
{
	// Token: 0x02000076 RID: 118
	public class CreateSerialFrameEventArgs
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060003FA RID: 1018 RVA: 0x00013538 File Offset: 0x00011738
		// (set) Token: 0x060003FB RID: 1019 RVA: 0x0001354F File Offset: 0x0001174F
		public ReadOnlyCollection<ResourceItem> SerialResources { get; private set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060003FC RID: 1020 RVA: 0x00013558 File Offset: 0x00011758
		// (set) Token: 0x060003FD RID: 1021 RVA: 0x0001356F File Offset: 0x0001176F
		public VisualObject ObjectSerialOn { get; private set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00013578 File Offset: 0x00011778
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x0001358F File Offset: 0x0001178F
		public int StartFrameIndex { get; private set; }

		// Token: 0x06000400 RID: 1024 RVA: 0x00013598 File Offset: 0x00011798
		public CreateSerialFrameEventArgs(VisualObject objectSerialOn, IEnumerable<ResourceItem> serialResources)
		{
			this.StartFrameIndex = 0;
			this.ObjectSerialOn = objectSerialOn;
			this.SerialResources = serialResources.ToList<ResourceItem>().AsReadOnly();
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x000135C5 File Offset: 0x000117C5
		public CreateSerialFrameEventArgs(VisualObject objectSerialOn, IEnumerable<ResourceItem> serialResources, int startFrameIndex)
		{
			this.StartFrameIndex = startFrameIndex;
			this.ObjectSerialOn = objectSerialOn;
			this.SerialResources = serialResources.ToList<ResourceItem>().AsReadOnly();
		}
	}
}
