using System;
using Gdk;
using Pango;

namespace Gtk
{
	public static class GtkWidgetExtend
	{
		private const double PangoScaleFactor = 1024.0;

		public static void SetFontSize(this Widget widget, double fontSize)
		{
			if (widget != null)
			{
				widget.ModifyFont(new FontDescription
				{
					AbsoluteSize = fontSize * PangoScaleFactor
				});
			}
		}

		public static void RemoveAll(this Container box)
		{
			if (box != null)
			{
				foreach (Widget widget in box.Children)
				{
					box.Remove(widget);
				}
			}
		}

		public static void RemoveChild(this Bin bin)
		{
			if (bin != null)
			{
				if (bin.Child != null)
				{
					bin.Remove(bin.Child);
				}
			}
		}

		public static void AddChild(this Bin bin, Widget widget)
		{
			if (bin != null && bin.Child == null)
			{
				bin.Add(widget);
			}
		}

		public static void RemoveAllText(this ComboBox comboBox)
		{
			for (int i = comboBox.Model.IterNChildren(); i >= 0; i--)
			{
				comboBox.RemoveText(0);
			}
		}

		public static void InsertTextAt(this ListStore listStore, int position, string text)
		{
			if (listStore != null && position >= 0)
			{
				TreeIter iter = listStore.Insert(0);
				listStore.SetValue(iter, 0, text);
			}
		}

		public static int TextCount(this ComboBox comboBox)
		{
			int result;
			if (comboBox == null)
			{
				result = 0;
			}
			else
			{
				result = comboBox.Model.IterNChildren();
			}
			return result;
		}

		public static void SetNormalBg(this Widget widget, Gdk.Color bgColor)
		{
			if (widget != null)
			{
				widget.ModifyBg(StateType.Normal, bgColor);
			}
		}
	}
}
