using System;
using Gdk;
using Gtk;

namespace CocoStudio.Model.Editor
{
	public class Scale9EventBox : EventBox
	{
		public Scale9DrawingArea DrawingArea
		{
			get
			{
				return this.widget;
			}
		}

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

		private bool IsChosen(double range, double value)
		{
			return Math.Abs(value - range) < 4.0;
		}

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

		protected override bool OnButtonReleaseEvent(EventButton evnt)
		{
			this.mousePress = false;
			this.widget.IsPress = false;
			base.QueueDraw();
			return base.OnButtonReleaseEvent(evnt);
		}

		private void LeaveNotifyHandler(object o, LeaveNotifyEventArgs args)
		{
			base.GdkWindow.Cursor = null;
		}

		private const int minValue = 2;

		private const int maxValue = 102;

		private Scale9DrawingArea widget;

		private CurrentRange currentChoiceRange = CurrentRange.None;

		private bool mousePress = false;

		public EventHandler<Scale9EventArgs> Scale9EventChanged;
	}
}
