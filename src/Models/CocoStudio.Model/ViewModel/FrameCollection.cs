using System;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.Model.Interface;

namespace CocoStudio.Model.ViewModel
{
	public class FrameCollection : ObservableCollection<Frame>
	{
		protected FrameCollection()
		{
		}

		public FrameCollection(ITimeline iTimeline)
		{
			this.collectionTimeline = iTimeline;
		}

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

		protected override void RemoveItem(int index)
		{
			Frame frame = base.Items[index];
			frame.Timeline = null;
			base.RemoveItem(index);
		}

		protected new void Move(int oldIndex, int newIndex)
		{
			throw new InvalidOperationException("can not move frame in FrameCollection ");
		}

		private ITimeline collectionTimeline;
	}
}
