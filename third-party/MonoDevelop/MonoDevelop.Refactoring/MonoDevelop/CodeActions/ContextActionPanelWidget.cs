using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using MonoDevelop.CodeIssues;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Refactoring;
using Pango;
using Stetic;

namespace MonoDevelop.CodeActions
{
	internal class ContextActionPanelWidget : Bin
	{
		private readonly string mimeType;

		private readonly TreeStore treeStore = new TreeStore(typeof(string), typeof(bool), typeof(CodeActionProvider), typeof(string));

		private readonly Dictionary<CodeActionProvider, bool> providerStates = new Dictionary<CodeActionProvider, bool>();

		private VBox vbox1;

		private HBox hbox1;

		private SearchEntry searchentryFilter;

		private ScrolledWindow GtkScrolledWindow;

		private TreeView treeviewContextActions;

		private void GetAllProviderStates()
		{
			string text = PropertyService.Get("ContextActions." + mimeType, "");
			foreach (CodeActionProvider item in RefactoringService.ContextAddinNodes.Where((CodeActionProvider n) => n.MimeType == mimeType))
			{
				providerStates[item] = text.IndexOf(item.IdString, StringComparison.Ordinal) < 0;
			}
		}

		public ContextActionPanelWidget(string mimeType)
		{
			this.mimeType = mimeType;
			Build();
			TreeView treeView = treeviewContextActions;
			SizeAllocatedHandler value = delegate
			{
				if (treeviewContextActions.Selection.GetSelected(out var iter))
				{
					TreePath path = treeviewContextActions.Model.GetPath(iter);
					treeviewContextActions.ScrollToCell(path, treeviewContextActions.Columns[0], use_align: false, 0f, 0f);
				}
			};
			treeView.SizeAllocated += value;
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			searchentryFilter.ForceFilterButtonVisible = true;
			searchentryFilter.RoundedShape = true;
			searchentryFilter.HasFrame = true;
			searchentryFilter.Ready = true;
			searchentryFilter.Visible = true;
			searchentryFilter.Entry.Changed += ApplyFilter;
			CellRendererToggle cellRendererToggle = new CellRendererToggle();
			cellRendererToggle.Toggled += delegate(object o, ToggledArgs args)
			{
				if (treeStore.GetIterFromString(out var iter, args.Path))
				{
					CodeActionProvider key = (CodeActionProvider)treeStore.GetValue(iter, 2);
					providerStates[key] = !providerStates[key];
					treeStore.SetValue(iter, 1, providerStates[key]);
				}
			};
			treeViewColumn.PackStart(cellRendererToggle, expand: false);
			treeViewColumn.AddAttribute(cellRendererToggle, "active", 1);
			CellRendererText cell = new CellRendererText
			{
				Ellipsize = EllipsizeMode.End
			};
			treeViewColumn.PackStart(cell, expand: true);
			treeViewColumn.AddAttribute(cell, "markup", 0);
			treeviewContextActions.AppendColumn(treeViewColumn);
			treeviewContextActions.HeadersVisible = false;
			treeviewContextActions.Model = treeStore;
			GetAllProviderStates();
			FillTreeStore(null);
			treeviewContextActions.TooltipColumn = 3;
			treeviewContextActions.HasTooltip = true;
		}

		private void ApplyFilter(object sender, EventArgs e)
		{
			FillTreeStore(searchentryFilter.Entry.Text.Trim());
		}

		public void FillTreeStore(string filter)
		{
			treeStore.Clear();
			IOrderedEnumerable<CodeActionProvider> orderedEnumerable = providerStates.Keys.Where((CodeActionProvider node) => string.IsNullOrEmpty(filter) || node.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) > 0).OrderBy((CodeActionProvider n) => n.Title, StringComparer.Ordinal);
			foreach (CodeActionProvider item in orderedEnumerable)
			{
				string title = item.Title;
				CodeIssuePanelWidget.MarkupSearchResult(filter, ref title);
				treeStore.AppendValues(title, providerStates[item], item, item.Description);
			}
		}

		public void ApplyChanges()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<CodeActionProvider, bool> providerState in providerStates)
			{
				if (!providerState.Value)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(",");
					}
					stringBuilder.Append(providerState.Key.IdString);
				}
			}
			PropertyService.Set("ContextActions." + mimeType, stringBuilder.ToString());
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.CodeActions.ContextActionPanelWidget";
			vbox1 = new VBox();
			vbox1.Name = "vbox1";
			vbox1.Spacing = 6;
			hbox1 = new HBox();
			hbox1.Name = "hbox1";
			hbox1.Spacing = 6;
			searchentryFilter = new SearchEntry();
			searchentryFilter.Name = "searchentryFilter";
			searchentryFilter.ForceFilterButtonVisible = false;
			searchentryFilter.HasFrame = false;
			searchentryFilter.RoundedShape = false;
			searchentryFilter.IsCheckMenu = false;
			searchentryFilter.ActiveFilterID = 0;
			searchentryFilter.Ready = true;
			searchentryFilter.HasFocus = false;
			hbox1.Add(searchentryFilter);
			Box.BoxChild boxChild = (Box.BoxChild)hbox1[searchentryFilter];
			boxChild.Position = 0;
			vbox1.Add(hbox1);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox1[hbox1];
			boxChild2.Position = 0;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			GtkScrolledWindow = new ScrolledWindow();
			GtkScrolledWindow.Name = "GtkScrolledWindow";
			GtkScrolledWindow.ShadowType = ShadowType.In;
			treeviewContextActions = new TreeView();
			treeviewContextActions.CanFocus = true;
			treeviewContextActions.Name = "treeviewContextActions";
			GtkScrolledWindow.Add(treeviewContextActions);
			vbox1.Add(GtkScrolledWindow);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox1[GtkScrolledWindow];
			boxChild3.Position = 1;
			Add(vbox1);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
		}
	}
}
