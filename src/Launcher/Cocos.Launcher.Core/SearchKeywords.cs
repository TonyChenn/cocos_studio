using System;
using System.Collections.Generic;
using System.ComponentModel;
using Gtk;

namespace Cocos.Launcher.Core
{
	[ToolboxItem(true)]
	public class SearchKeywords : TreeView
	{
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

		public SearchKeywords()
		{
			this.Initialize();
		}

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

		internal object GetSelectedItem()
		{
			TreeModel treeModel;
			TreeIter iter;
			base.Selection.GetSelected(out treeModel, out iter);
			return treeModel.GetValue(iter, 0);
		}

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

		internal void Restore()
		{
			this.Models = new List<string>();
		}

		private ListStore store;

		private List<string> models = new List<string>();
	}
}
