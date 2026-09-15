using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Ide.Codons;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.View
{
	public class Pad
	{
		internal Pad(MainWindow mainWindow, PadCodon content)
		{
			this.window = mainWindow.GetPadWindow(content);
			this.window.PadHidden += delegate(object param0, EventArgs param1)
			{
				this.IsOpenedAutomatically = false;
			};
			this.content = content;
			this.mainWindow = mainWindow;
		}

		internal PadCodon InternalContent
		{
			get
			{
				return this.content;
			}
		}

		public object Content
		{
			get
			{
				return this.content;
			}
		}

		public string Title
		{
			get
			{
				return this.window.Title;
			}
		}

		public IconId Icon
		{
			get
			{
				return this.window.Icon;
			}
		}

		public string Id
		{
			get
			{
				return this.window.Id;
			}
		}

		public bool IsOpenedAutomatically { get; set; }

		public string[] Categories
		{
			get
			{
				if (this.categories == null)
				{
					CategoryNode categoryNode = this.content.Parent as CategoryNode;
					if (categoryNode == null)
					{
						this.categories = new string[]
						{
							GettextCatalog.GetString("Pads")
						};
					}
					else
					{
						List<string> list = new List<string>();
						while (categoryNode != null)
						{
							list.Insert(0, categoryNode.Name);
							categoryNode = (categoryNode.Parent as CategoryNode);
						}
						this.categories = list.ToArray();
					}
				}
				return this.categories;
			}
		}

		public void BringToFront()
		{
			this.BringToFront(false);
		}

		public void BringToFront(bool grabFocus)
		{
			this.mainWindow.BringToFront(this.content);
			this.window.Activate(grabFocus);
		}

		public bool AutoHide
		{
			get
			{
				return this.window.AutoHide;
			}
			set
			{
				this.window.AutoHide = value;
			}
		}

		public bool Visible
		{
			get
			{
				return this.window.Visible;
			}
			set
			{
				this.window.Visible = value;
			}
		}

		public bool Sticky
		{
			get
			{
				return this.window.Sticky;
			}
			set
			{
				this.window.Sticky = value;
			}
		}

		internal IPadWindow Window
		{
			get
			{
				return this.window;
			}
		}

		internal IMementoCapable GetMementoCapable()
		{
			throw new NotImplementedException();
		}

		public void Destroy()
		{
			this.Visible = false;
			this.mainWindow.RemovePad(this.content);
		}

		private IPadWindow window;

		private PadCodon content;

		private MainWindow mainWindow;

		private string[] categories;
	}
}
