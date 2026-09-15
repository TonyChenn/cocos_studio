using System;
using System.Collections.ObjectModel;
using System.Linq;
using CocoStudio.UndoManager;

namespace CocoStudio.Model.ViewModel
{
	public class HandlerFrame : Frame
	{
		public ObservableCollection<Frame> Frames { get; set; }

		public HandlerFrame()
		{
			this.Frames = new ObservableCollection<Frame>();
		}

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

		internal override void SetInnerFrameIndex(int index)
		{
			this.frameIndex = index;
		}

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

		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
		}

		private int frameIndex;
	}
}
