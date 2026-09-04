using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Ide.Codons;
using MonoDevelop.Ide.Gui;

namespace CocoStudio.Core.View
{
	// Token: 0x0200004F RID: 79
	public class Pad
	{
		// Token: 0x0600030E RID: 782 RVA: 0x0000E2C0 File Offset: 0x0000C4C0
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

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600030F RID: 783 RVA: 0x0000E314 File Offset: 0x0000C514
		internal PadCodon InternalContent
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0000E32C File Offset: 0x0000C52C
		public object Content
		{
			get
			{
				return this.content;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000311 RID: 785 RVA: 0x0000E344 File Offset: 0x0000C544
		public string Title
		{
			get
			{
				return this.window.Title;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000E364 File Offset: 0x0000C564
		public IconId Icon
		{
			get
			{
				return this.window.Icon;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000313 RID: 787 RVA: 0x0000E384 File Offset: 0x0000C584
		public string Id
		{
			get
			{
				return this.window.Id;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000E3A4 File Offset: 0x0000C5A4
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000E3BB File Offset: 0x0000C5BB
		public bool IsOpenedAutomatically { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000316 RID: 790 RVA: 0x0000E3C4 File Offset: 0x0000C5C4
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

		// Token: 0x06000317 RID: 791 RVA: 0x0000E467 File Offset: 0x0000C667
		public void BringToFront()
		{
			this.BringToFront(false);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000E472 File Offset: 0x0000C672
		public void BringToFront(bool grabFocus)
		{
			this.mainWindow.BringToFront(this.content);
			this.window.Activate(grabFocus);
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000E494 File Offset: 0x0000C694
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000E4B1 File Offset: 0x0000C6B1
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600031B RID: 795 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
		// (set) Token: 0x0600031C RID: 796 RVA: 0x0000E4E1 File Offset: 0x0000C6E1
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000E511 File Offset: 0x0000C711
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600031F RID: 799 RVA: 0x0000E524 File Offset: 0x0000C724
		internal IPadWindow Window
		{
			get
			{
				return this.window;
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000E53C File Offset: 0x0000C73C
		internal IMementoCapable GetMementoCapable()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E544 File Offset: 0x0000C744
		public void Destroy()
		{
			this.Visible = false;
			this.mainWindow.RemovePad(this.content);
		}

		// Token: 0x04000166 RID: 358
		private IPadWindow window;

		// Token: 0x04000167 RID: 359
		private PadCodon content;

		// Token: 0x04000168 RID: 360
		private MainWindow mainWindow;

		// Token: 0x04000169 RID: 361
		private string[] categories;
	}
}
