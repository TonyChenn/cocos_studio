using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000057 RID: 87
	[ToolboxItem(true)]
	public class SearchKeywords : TreeView
	{
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x0000BB64 File Offset: 0x00009D64
		// (set) Token: 0x060002F2 RID: 754 RVA: 0x0000BB6C File Offset: 0x00009D6C
		public List<string> Models
		{
			get
			{
				return this.models;
			}
			set
			{
				if (this.models == value)
				{
					return;
				}
				this.models = value;
				this.store.Clear();
				foreach (string text in value)
				{
					this.store.AppendValues(new object[]
					{
						text
					});
				}
				base.ShowAll();
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000BBF0 File Offset: 0x00009DF0
		public SearchKeywords()
		{
			this.Initialize();
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000BC0C File Offset: 0x00009E0C
		private void Initialize()
		{
			base.Name = "DarkTreeView";
			base.HeadersVisible = false;
			this.store = new ListStore(new Type[]
			{
				typeof(string)
			});
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			CellRendererText cell = new CellRendererText();
			treeViewColumn.PackStart(cell, true);
			treeViewColumn.AddAttribute(cell, "text", 0);
			base.AppendColumn(treeViewColumn);
			base.Model = this.store;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000BC80 File Offset: 0x00009E80
		internal object GetSelectedItem()
		{
			TreeModel treeModel;
			TreeIter iter;
			base.Selection.GetSelected(out treeModel, out iter);
			return treeModel.GetValue(iter, 0);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000BCA8 File Offset: 0x00009EA8
		internal void GoBackItem()
		{
			TreeIter iter;
			if (!base.Selection.GetSelected(out iter))
			{
				return;
			}
			TreePath path = this.store.GetPath(iter);
			path.Prev();
			base.Selection.SelectPath(path);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000BCE8 File Offset: 0x00009EE8
		internal void GoNextItem()
		{
			TreeIter iter;
			bool selected = base.Selection.GetSelected(out iter);
			TreePath treePath = TreePath.NewFirst();
			if (selected)
			{
				treePath = this.store.GetPath(iter);
				treePath.Next();
			}
			base.Selection.SelectPath(treePath);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000BD2B File Offset: 0x00009F2B
		internal void Restore()
		{
			this.Models = new List<string>();
		}

		// Token: 0x04000117 RID: 279
		private ListStore store;

		// Token: 0x04000118 RID: 280
		private List<string> models = new List<string>();
	}
}
