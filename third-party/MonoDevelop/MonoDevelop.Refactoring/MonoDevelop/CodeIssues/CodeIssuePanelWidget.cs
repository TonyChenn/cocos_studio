using System;
using System.Collections.Generic;
using System.Linq;
using Cairo;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.Refactoring;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Refactoring;
using MonoDevelop.SourceEditor.QuickTasks;
using Pango;
using Stetic;
using Xwt.Drawing;

namespace MonoDevelop.CodeIssues
{
	internal class CodeIssuePanelWidget : Bin
	{
		private class CustomCellRenderer : CellRendererCombo
		{
			public Xwt.Drawing.Image Icon { get; set; }

			protected override void Render(Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
			{
				int num = 10;
				Gdk.Rectangle cell_area2 = new Gdk.Rectangle(cell_area.X + num, cell_area.Y, cell_area.Width - num, cell_area.Height);
				using (Cairo.Context s = Gdk.CairoHelper.Create(window))
				{
					s.DrawImage(widget, Icon, cell_area.X - 4, (double)cell_area.Y + Math.Round(((double)cell_area.Height - Icon.Height) / 2.0));
				}
				base.Render(window, widget, background_area, cell_area2, expose_area, flags);
			}
		}

		private readonly string mimeType;

		private readonly TreeStore treeStore = new TreeStore(typeof(string), typeof(BaseCodeIssueProvider), typeof(string));

		private readonly Dictionary<BaseCodeIssueProvider, Severity> severities = new Dictionary<BaseCodeIssueProvider, Severity>();

		private readonly Dictionary<BaseCodeIssueProvider, bool> enableState = new Dictionary<BaseCodeIssueProvider, bool>();

		private readonly Dictionary<string, TreeIter> categories = new Dictionary<string, TreeIter>();

		private VBox vbox1;

		private HBox hbox1;

		private SearchEntry searchentryFilter;

		private ScrolledWindow GtkScrolledWindow;

		private TreeView treeviewInspections;

		private void GetAllSeverities()
		{
			foreach (CodeIssueProvider inspector in RefactoringService.GetInspectors(mimeType))
			{
				severities[inspector] = inspector.GetSeverity();
				enableState[inspector] = inspector.GetIsEnabled();
				if (!inspector.HasSubIssues)
				{
					continue;
				}
				foreach (BaseCodeIssueProvider subIssue in inspector.SubIssues)
				{
					severities[subIssue] = subIssue.GetSeverity();
					enableState[subIssue] = subIssue.GetIsEnabled();
				}
			}
		}

		public void SelectCodeIssue(string idString)
		{
			if (treeStore.GetIterFirst(out var iter))
			{
				SelectCodeIssue(idString, iter);
			}
		}

		private bool SelectCodeIssue(string idString, TreeIter iter)
		{
			do
			{
				if (treeStore.GetValue(iter, 1) is BaseCodeIssueProvider baseCodeIssueProvider && baseCodeIssueProvider.IdString == idString)
				{
					treeviewInspections.ExpandToPath(treeStore.GetPath(iter));
					treeviewInspections.Selection.SelectIter(iter);
					return true;
				}
				if (!treeStore.IterChildren(out var iter2, iter))
				{
					continue;
				}
				do
				{
					if (SelectCodeIssue(idString, iter2))
					{
						return true;
					}
				}
				while (treeStore.IterNext(ref iter2));
			}
			while (treeStore.IterNext(ref iter));
			return false;
		}

		private static string GetDescription(Severity severity)
		{
			switch (severity)
			{
			case Severity.None:
				return GettextCatalog.GetString("Do not show");
			case Severity.Error:
				return GettextCatalog.GetString("Error");
			case Severity.Warning:
				return GettextCatalog.GetString("Warning");
			case Severity.Hint:
				return GettextCatalog.GetString("Hint");
			case Severity.Suggestion:
				return GettextCatalog.GetString("Suggestion");
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private Xwt.Drawing.Image GetIcon(Severity severity)
		{
			switch (severity)
			{
			case Severity.Error:
				return QuickTaskOverviewMode.ErrorImage;
			case Severity.Warning:
				return QuickTaskOverviewMode.WarningImage;
			case Severity.Suggestion:
			case Severity.Hint:
				return QuickTaskOverviewMode.SuggestionImage;
			default:
				return QuickTaskOverviewMode.OkImage;
			}
		}

		public void FillInspectors(string filter)
		{
			categories.Clear();
			treeStore.Clear();
			IOrderedEnumerable<IGrouping<string, CodeIssueProvider>> orderedEnumerable = (from node in severities.Keys.OfType<CodeIssueProvider>()
				where string.IsNullOrEmpty(filter) || node.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) > 0
				group node by node.Category).OrderBy((IGrouping<string, CodeIssueProvider> g) => g.Key, StringComparer.Ordinal);
			foreach (IGrouping<string, CodeIssueProvider> item in orderedEnumerable)
			{
				TreeIter treeIter = treeStore.AppendValues("<b>" + item.Key + "</b>", null, null);
				categories[item.Key] = treeIter;
				foreach (CodeIssueProvider item2 in item.OrderBy((CodeIssueProvider n) => n.Title, StringComparer.Ordinal))
				{
					string title = item2.Title;
					MarkupSearchResult(filter, ref title);
					TreeIter parent = treeStore.AppendValues(treeIter, title, item2, item2.Description);
					if (!item2.HasSubIssues)
					{
						continue;
					}
					foreach (BaseCodeIssueProvider subIssue in item2.SubIssues)
					{
						title = subIssue.Title;
						MarkupSearchResult(filter, ref title);
						treeStore.AppendValues(parent, title, subIssue, subIssue.Description);
					}
				}
			}
			treeviewInspections.ExpandAll();
		}

		public static void MarkupSearchResult(string filter, ref string title)
		{
			if (!string.IsNullOrEmpty(filter))
			{
				int num = title.IndexOf(filter, StringComparison.OrdinalIgnoreCase);
				if (num >= 0)
				{
					title = Markup.EscapeText(title.Substring(0, num)) + "<span bgcolor=\"yellow\">" + Markup.EscapeText(title.Substring(num, filter.Length)) + "</span>" + Markup.EscapeText(title.Substring(num + filter.Length));
					return;
				}
			}
			title = Markup.EscapeText(title);
		}

		public CodeIssuePanelWidget(string mimeType)
		{
			this.mimeType = mimeType;
			Build();
			TreeView treeView = treeviewInspections;
			SizeAllocatedHandler value = delegate
			{
				if (treeviewInspections.Selection.GetSelected(out var iter))
				{
					TreePath path = treeviewInspections.Model.GetPath(iter);
					treeviewInspections.ScrollToCell(path, treeviewInspections.Columns[0], use_align: false, 0f, 0f);
				}
			};
			treeView.SizeAllocated += value;
			treeviewInspections.TooltipColumn = 2;
			treeviewInspections.HasTooltip = true;
			CellRendererToggle toggleRenderer = new CellRendererToggle();
			toggleRenderer.Toggled += delegate(object o, ToggledArgs args)
			{
				if (treeStore.GetIterFromString(out var iter, args.Path))
				{
					BaseCodeIssueProvider key = (BaseCodeIssueProvider)treeStore.GetValue(iter, 1);
					enableState[key] = !enableState[key];
				}
			};
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			treeviewInspections.AppendColumn(treeViewColumn);
			treeViewColumn.PackStart(toggleRenderer, expand: false);
			treeViewColumn.Sizing = TreeViewColumnSizing.Autosize;
			treeViewColumn.SetCellDataFunc(toggleRenderer, delegate(TreeViewColumn treeColumn, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
			{
				BaseCodeIssueProvider baseCodeIssueProvider = (BaseCodeIssueProvider)model.GetValue(iter, 1);
				if (baseCodeIssueProvider == null)
				{
					toggleRenderer.Visible = false;
				}
				else
				{
					toggleRenderer.Visible = true;
					toggleRenderer.Active = enableState[baseCodeIssueProvider];
				}
			});
			CellRendererText cell = new CellRendererText
			{
				Ellipsize = EllipsizeMode.End
			};
			treeViewColumn.PackStart(cell, expand: true);
			treeViewColumn.AddAttribute(cell, "markup", 0);
			treeViewColumn.Expand = true;
			searchentryFilter.ForceFilterButtonVisible = true;
			searchentryFilter.RoundedShape = true;
			searchentryFilter.HasFrame = true;
			searchentryFilter.Ready = true;
			searchentryFilter.Visible = true;
			searchentryFilter.Entry.Changed += ApplyFilter;
			CustomCellRenderer comboRenderer = new CustomCellRenderer
			{
				Alignment = Pango.Alignment.Center
			};
			TreeViewColumn treeViewColumn2 = treeviewInspections.AppendColumn("Severity", comboRenderer);
			treeViewColumn2.Sizing = TreeViewColumnSizing.GrowOnly;
			treeViewColumn2.MinWidth = 100;
			treeViewColumn2.Expand = false;
			ListStore comboBoxStore = new ListStore(typeof(string), typeof(Severity));
			comboBoxStore.AppendValues(GetDescription(Severity.Error), Severity.Error);
			comboBoxStore.AppendValues(GetDescription(Severity.Warning), Severity.Warning);
			comboBoxStore.AppendValues(GetDescription(Severity.Hint), Severity.Hint);
			comboBoxStore.AppendValues(GetDescription(Severity.Suggestion), Severity.Suggestion);
			comboRenderer.Model = comboBoxStore;
			comboRenderer.Mode = CellRendererMode.Activatable;
			comboRenderer.TextColumn = 0;
			comboRenderer.Editable = true;
			comboRenderer.HasEntry = false;
			comboRenderer.Edited += delegate(object o, EditedArgs args)
			{
				if (treeStore.GetIterFromString(out var iter, args.Path) && comboBoxStore.GetIterFirst(out var iter2))
				{
					do
					{
						if ((string)comboBoxStore.GetValue(iter2, 0) == args.NewText)
						{
							BaseCodeIssueProvider key = (BaseCodeIssueProvider)treeStore.GetValue(iter, 1);
							Severity value2 = (Severity)comboBoxStore.GetValue(iter2, 1);
							severities[key] = value2;
							break;
						}
					}
					while (comboBoxStore.IterNext(ref iter2));
				}
			};
			treeViewColumn2.SetCellDataFunc(comboRenderer, delegate(TreeViewColumn treeColumn, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
			{
				BaseCodeIssueProvider baseCodeIssueProvider = (BaseCodeIssueProvider)model.GetValue(iter, 1);
				if (baseCodeIssueProvider == null)
				{
					comboRenderer.Visible = false;
				}
				else
				{
					Severity severity = severities[baseCodeIssueProvider];
					comboRenderer.Visible = true;
					comboRenderer.Text = GetDescription(severity);
					comboRenderer.Icon = GetIcon(severity);
				}
			});
			treeviewInspections.HeadersVisible = false;
			treeviewInspections.Model = treeStore;
			GetAllSeverities();
			FillInspectors(null);
		}

		private void ApplyFilter(object sender, EventArgs e)
		{
			FillInspectors(searchentryFilter.Entry.Text.Trim());
		}

		public void ApplyChanges()
		{
			foreach (KeyValuePair<BaseCodeIssueProvider, Severity> severity in severities)
			{
				severity.Key.SetSeverity(severity.Value);
			}
			foreach (KeyValuePair<BaseCodeIssueProvider, bool> item in enableState)
			{
				item.Key.SetIsEnabled(item.Value);
			}
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.CodeIssues.CodeIssuePanelWidget";
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
			treeviewInspections = new TreeView();
			treeviewInspections.CanFocus = true;
			treeviewInspections.Name = "treeviewInspections";
			GtkScrolledWindow.Add(treeviewInspections);
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
