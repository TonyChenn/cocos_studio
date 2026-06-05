using System;
using Gdk;
using Pango;

namespace Gtk
{
	// Token: 0x02000059 RID: 89
	public static class GtkWidgetExtend
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x00008844 File Offset: 0x00006A44
		public static void SetFontSize(this Widget widget, double fontSize)
		{
			if (widget != null)
			{
				widget.ModifyFont(new FontDescription
				{
					AbsoluteSize = fontSize * Scale.PangoScale
				});
			}
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000887C File Offset: 0x00006A7C
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

		// Token: 0x060001E4 RID: 484 RVA: 0x000088C0 File Offset: 0x00006AC0
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

		// Token: 0x060001E5 RID: 485 RVA: 0x000088FC File Offset: 0x00006AFC
		public static void AddChild(this Bin bin, Widget widget)
		{
			if (bin != null && bin.Child == null)
			{
				bin.Add(widget);
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008928 File Offset: 0x00006B28
		public static void RemoveAllText(this ComboBox comboBox)
		{
			for (int i = comboBox.Model.IterNChildren(); i >= 0; i--)
			{
				comboBox.RemoveText(0);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000895C File Offset: 0x00006B5C
		public static void InsertTextAt(this ListStore listStore, int position, string text)
		{
			if (listStore != null && position >= 0)
			{
				TreeIter iter = listStore.Insert(0);
				listStore.SetValue(iter, 0, text);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008990 File Offset: 0x00006B90
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

		// Token: 0x060001E9 RID: 489 RVA: 0x000089BC File Offset: 0x00006BBC
		public static void SetNormalBg(this Widget widget, Gdk.Color bgColor)
		{
			if (widget != null)
			{
				widget.ModifyBg(StateType.Normal, bgColor);
			}
		}
	}
}
