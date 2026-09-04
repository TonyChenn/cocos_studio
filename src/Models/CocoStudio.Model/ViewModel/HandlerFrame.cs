using System;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000E4 RID: 228
	public class HandlerFrame : Frame
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001D20C File Offset: 0x0001B40C
		// (set) Token: 0x06000734 RID: 1844 RVA: 0x0001D223 File Offset: 0x0001B423
		public ObservableCollection<Frame> Frames { get; set; }

		// Token: 0x06000735 RID: 1845 RVA: 0x0001D22C File Offset: 0x0001B42C
		public HandlerFrame()
		{
			this.Frames = new ObservableCollection<Frame>();
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001D244 File Offset: 0x0001B444
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0001D257 File Offset: 0x0001B457
		public override bool Tween
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0001D25C File Offset: 0x0001B45C
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x0001D274 File Offset: 0x0001B474
		public override int FrameIndex
		{
			get
			{
				return this.frameIndex;
			}
			set
			{
				using (CompositeTask.Run(" ChangedFrameIndex", null))
				{
					foreach (Frame frame in this.Frames.ToList<Frame>())
					{
						frame.FrameIndex = this.frameIndex;
					}
					int num = this.frameIndex;
					this.frameIndex = value;
				}
			}
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0001D31C File Offset: 0x0001B51C
		internal override void SetInnerFrameIndex(int index)
		{
			this.frameIndex = index;
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001D328 File Offset: 0x0001B528
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x0001D394 File Offset: 0x0001B594
		public override bool Select
		{
			get
			{
				bool result = true;
				foreach (Frame frame in this.Frames)
				{
					if (!frame.Select)
					{
						result = false;
					}
				}
				return result;
			}
			set
			{
				base.Select = value;
				foreach (Frame frame in this.Frames)
				{
					frame.Select = value;
				}
			}
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001D3F8 File Offset: 0x0001B5F8
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
		}

		// Token: 0x040002FE RID: 766
		private int frameIndex;
	}
}
