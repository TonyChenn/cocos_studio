using System;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.Model.Interface;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000CF RID: 207
	public class FrameCollection : ObservableCollection<Frame>
	{
		// Token: 0x06000672 RID: 1650 RVA: 0x0001A04C File Offset: 0x0001824C
		protected FrameCollection()
		{
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0001A057 File Offset: 0x00018257
		public FrameCollection(ITimeline iTimeline)
		{
			this.collectionTimeline = iTimeline;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001A06C File Offset: 0x0001826C
		public Frame BinarySearch(int frameIndex)
		{
			if (base.Count != 0)
			{
				int i = 0;
				int num = this.Count<Frame>() - 1;
				while (i <= num)
				{
					int num2 = i + num >> 1;
					Frame frame = base[num2];
					int frameIndex2 = frame.FrameIndex;
					if (frameIndex == frameIndex2)
					{
						return frame;
					}
					if (frameIndex > frameIndex2)
					{
						i = num2 + 1;
					}
					else
					{
						num = num2 - 1;
					}
				}
			}
			return null;
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0001A0F8 File Offset: 0x000182F8
		protected override void InsertItem(int index, Frame frame)
		{
			int i = 0;
			if (base.Count != 0)
			{
				int frameIndex = frame.FrameIndex;
				int num = base.Count - 1;
				while (i <= num)
				{
					int num2 = i + num >> 1;
					int frameIndex2 = base.Items[num2].FrameIndex;
					if (frameIndex == frameIndex2)
					{
						i = num2;
						this.RemoveItem(i);
						break;
					}
					if (frameIndex > frameIndex2)
					{
						i = num2 + 1;
					}
					else
					{
						num = num2 - 1;
					}
				}
			}
			frame.Timeline = this.collectionTimeline;
			base.InsertItem(i, frame);
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0001A1A4 File Offset: 0x000183A4
		protected override void RemoveItem(int index)
		{
			Frame frame = base.Items[index];
			frame.Timeline = null;
			base.RemoveItem(index);
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001A1CF File Offset: 0x000183CF
		protected new void Move(int oldIndex, int newIndex)
		{
			throw new InvalidOperationException("can not move frame in FrameCollection ");
		}

		// Token: 0x040002CB RID: 715
		private ITimeline collectionTimeline;
	}
}
