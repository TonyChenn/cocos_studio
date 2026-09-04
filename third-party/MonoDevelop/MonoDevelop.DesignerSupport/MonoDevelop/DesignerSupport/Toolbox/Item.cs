using System;
using Gtk;
using MonoDevelop.Ide;
using Xwt.Drawing;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public class Item : IComparable<Item>
	{
		private static Xwt.Drawing.Image defaultIcon;

		private Xwt.Drawing.Image icon;

		private string text;

		private string tooltip;

		private object tag;

		private bool isVisible = true;

		private ItemToolboxNode node;

		public string Tooltip
		{
			get
			{
				if (node != null)
				{
					if (!string.IsNullOrEmpty(node.Description))
					{
						return node.Description;
					}
					return node.Name;
				}
				return tooltip;
			}
		}

		public Xwt.Drawing.Image Icon
		{
			get
			{
				if (node != null)
				{
					return node.Icon;
				}
				return icon ?? DefaultIcon;
			}
		}

		private static Xwt.Drawing.Image DefaultIcon
		{
			get
			{
				if (defaultIcon == null)
				{
					defaultIcon = ImageService.GetIcon(Stock.MissingImage, IconSize.Menu);
				}
				return defaultIcon;
			}
		}

		public string Text
		{
			get
			{
				if (node != null)
				{
					return node.Name;
				}
				return text;
			}
		}

		public bool IsVisible
		{
			get
			{
				return isVisible;
			}
			set
			{
				isVisible = value;
			}
		}

		public object Tag => node ?? tag;

		public Item(ItemToolboxNode node)
		{
			this.node = node;
		}

		public Item(string text)
			: this(null, text, null)
		{
		}

		public Item(Xwt.Drawing.Image icon, string text)
			: this(icon, text, null)
		{
		}

		public Item(Xwt.Drawing.Image icon, string text, string tooltip)
			: this(icon, text, tooltip, null)
		{
		}

		public Item(Xwt.Drawing.Image icon, string text, string tooltip, object tag)
		{
			this.icon = icon;
			this.text = text;
			this.tooltip = tooltip;
			this.tag = tag;
		}

		public virtual int CompareTo(Item other)
		{
			if (other == null)
			{
				return -1;
			}
			return Text.CompareTo(other.Text);
		}
	}
}
