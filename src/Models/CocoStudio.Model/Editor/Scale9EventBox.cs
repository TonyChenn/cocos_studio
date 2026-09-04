using System;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	// Token: 0x0200006A RID: 106
	public class Scale9EventBox : EventBox
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000394 RID: 916 RVA: 0x00011218 File Offset: 0x0000F418
		public Scale9DrawingArea DrawingArea
		{
			get
			{
				return this.widget;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00011230 File Offset: 0x0000F430
		public Scale9EventBox()
		{
			this.widget = new Scale9DrawingArea();
			base.Add(this.widget);
			base.ShowAll();
			base.WidthRequest = 140;
			base.HeightRequest = 140;
			base.Events |= EventMask.PointerMotionMask;
			base.LeaveNotifyEvent += this.LeaveNotifyHandler;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000112B0 File Offset: 0x0000F4B0
		private CurrentRange GetCurrentRange(double x, double y)
		{
			CurrentRange result;
			if (this.IsChosen(this.widget.LeftRange, x))
			{
				result = CurrentRange.Left;
			}
			else if (this.IsChosen(this.widget.RightRange, x))
			{
				result = CurrentRange.Right;
			}
			else if (this.IsChosen(this.widget.TopRange, y))
			{
				result = CurrentRange.Top;
			}
			else if (this.IsChosen(this.widget.BottomRange, y))
			{
				result = CurrentRange.Bottom;
			}
			else
			{
				result = CurrentRange.None;
			}
			return result;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0001133C File Offset: 0x0000F53C
		private bool IsChosen(double range, double value)
		{
			return Math.Abs(value - range) < 4.0;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00011370 File Offset: 0x0000F570
		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			bool result;
			if (this.currentChoiceRange == CurrentRange.None)
			{
				result = base.OnButtonPressEvent(evnt);
			}
			else
			{
				this.mousePress = true;
				this.widget.IsPress = true;
				this.widget.CurrentSelect = this.currentChoiceRange;
				base.QueueDraw();
				result = base.OnButtonPressEvent(evnt);
			}
			return result;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000113D0 File Offset: 0x0000F5D0
		protected override bool OnMotionNotifyEvent(EventMotion evnt)
		{
			if (this.mousePress)
			{
				double num = evnt.X;
				double num2 = evnt.Y;
				if (num <= 2.0)
				{
					num = 2.0;
				}
				if (num >= 102.0)
				{
					num = 102.0;
				}
				if (num2 <= 2.0)
				{
					num2 = 2.0;
				}
				if (num2 >= 102.0)
				{
					num2 = 102.0;
				}
				if (this.currentChoiceRange == CurrentRange.Left)
				{
					if (num > this.widget.RightRange)
					{
						this.currentChoiceRange = CurrentRange.Right;
						this.widget.LeftRange = this.widget.RightRange;
						this.widget.RightRange = num;
					}
					else
					{
						this.widget.LeftRange = num;
					}
				}
				else if (this.currentChoiceRange == CurrentRange.Right)
				{
					if (num < this.widget.LeftRange)
					{
						this.currentChoiceRange = CurrentRange.Left;
						this.widget.RightRange = this.widget.LeftRange;
						this.widget.LeftRange = num;
					}
					else
					{
						this.widget.RightRange = num;
					}
				}
				else if (this.currentChoiceRange == CurrentRange.Top)
				{
					if (num2 > this.widget.BottomRange)
					{
						this.currentChoiceRange = CurrentRange.Bottom;
						this.widget.TopRange = this.widget.BottomRange;
						this.widget.BottomRange = num2;
					}
					else
					{
						this.widget.TopRange = num2;
					}
				}
				else if (this.currentChoiceRange == CurrentRange.Bottom)
				{
					if (num2 < this.widget.TopRange)
					{
						this.currentChoiceRange = CurrentRange.Top;
						this.widget.BottomRange = this.widget.TopRange;
						this.widget.TopRange = num2;
					}
					else
					{
						this.widget.BottomRange = num2;
					}
				}
				this.widget.CurrentSelect = this.currentChoiceRange;
				if (this.Scale9EventChanged != null)
				{
					this.Scale9EventChanged(this, new Scale9EventArgs(this.currentChoiceRange, this.widget.LeftRange, this.widget.RightRange, this.widget.TopRange, this.widget.BottomRange));
				}
				base.QueueDraw();
			}
			else
			{
				this.currentChoiceRange = this.GetCurrentRange(evnt.X, evnt.Y);
				if (this.currentChoiceRange == CurrentRange.None)
				{
					base.GdkWindow.Cursor = null;
				}
				else if (this.currentChoiceRange == CurrentRange.Left || this.currentChoiceRange == CurrentRange.Right)
				{
					base.GdkWindow.Cursor = new Cursor(CursorType.SbHDoubleArrow);
				}
				else if (this.currentChoiceRange == CurrentRange.Top || this.currentChoiceRange == CurrentRange.Bottom)
				{
					base.GdkWindow.Cursor = new Cursor(CursorType.SbVDoubleArrow);
				}
			}
			return base.OnMotionNotifyEvent(evnt);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00011734 File Offset: 0x0000F934
		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			this.mousePress = false;
			this.widget.IsPress = false;
			base.QueueDraw();
			return base.OnButtonReleaseEvent(evnt);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00011768 File Offset: 0x0000F968
		private void LeaveNotifyHandler(object o, LeaveNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = null;
		}

		// Token: 0x040001D4 RID: 468
		private const int minValue = 2;

		// Token: 0x040001D5 RID: 469
		private const int maxValue = 102;

		// Token: 0x040001D6 RID: 470
		private Scale9DrawingArea widget;

		// Token: 0x040001D7 RID: 471
		private CurrentRange currentChoiceRange = CurrentRange.None;

		// Token: 0x040001D8 RID: 472
		private bool mousePress = false;

		// Token: 0x040001D9 RID: 473
		public EventHandler<Scale9EventArgs> Scale9EventChanged;
	}
}
