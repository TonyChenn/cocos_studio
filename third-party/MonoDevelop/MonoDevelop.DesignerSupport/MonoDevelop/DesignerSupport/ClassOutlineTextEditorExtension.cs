using System;
using System.Collections.Generic;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Components;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Content;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	public class ClassOutlineTextEditorExtension : TextEditorExtension, IOutlinedDocument
	{
		private ParsedDocument lastCU;

		private PadTreeView outlineTreeView;

		private TreeStore outlineTreeStore;

		private TreeModelSort outlineTreeModelSort;

		private Widget[] toolbarWidgets;

		private ClassOutlineNodeComparer comparer;

		private ClassOutlineSettings settings;

		private bool refreshingOutline;

		private bool disposed;

		private bool outlineReady;

		private uint refillOutlineStoreId;

		public override bool ExtendsEditor(Document doc, IEditableTextBuffer editor)
		{
			ILanguageBinding bindingPerFileName = LanguageBindingService.GetBindingPerFileName(doc.Name);
			if (bindingPerFileName != null)
			{
				return bindingPerFileName is IDotNetLanguageBinding;
			}
			return false;
		}

		public override void Initialize()
		{
			base.Initialize();
			if (base.Document != null)
			{
				base.Document.DocumentParsed += UpdateDocumentOutline;
			}
		}

		public override void Dispose()
		{
			if (!disposed)
			{
				disposed = true;
				if (base.Document != null)
				{
					base.Document.DocumentParsed -= UpdateDocumentOutline;
				}
				RemoveRefillOutlineStoreTimeout();
				lastCU = null;
				settings = null;
				comparer = null;
				base.Dispose();
			}
		}

		Widget IOutlinedDocument.GetOutlineWidget()
		{
			if (outlineTreeView != null)
			{
				return outlineTreeView;
			}
			outlineTreeStore = new TreeStore(typeof(object));
			outlineTreeModelSort = new TreeModelSort(outlineTreeStore);
			settings = ClassOutlineSettings.Load();
			comparer = new ClassOutlineNodeComparer(GetAmbience(), settings, outlineTreeModelSort);
			outlineTreeModelSort.SetSortFunc(0, comparer.CompareNodes);
			outlineTreeModelSort.SetSortColumnId(0, SortType.Ascending);
			outlineTreeView = new PadTreeView(outlineTreeStore);
			CellRendererImage cellRendererImage = new CellRendererImage();
			cellRendererImage.Xpad = 0u;
			cellRendererImage.Ypad = 0u;
			outlineTreeView.TextRenderer.Xpad = 0u;
			outlineTreeView.TextRenderer.Ypad = 0u;
			TreeViewColumn treeViewColumn = new TreeViewColumn();
			treeViewColumn.PackStart(cellRendererImage, expand: false);
			treeViewColumn.SetCellDataFunc(cellRendererImage, OutlineTreeIconFunc);
			treeViewColumn.PackStart(outlineTreeView.TextRenderer, expand: true);
			treeViewColumn.SetCellDataFunc(outlineTreeView.TextRenderer, OutlineTreeTextFunc);
			outlineTreeView.AppendColumn(treeViewColumn);
			outlineTreeView.HeadersVisible = false;
			outlineTreeView.Selection.Changed += delegate
			{
				JumpToDeclaration(focusEditor: false);
			};
			outlineTreeView.RowActivated += delegate
			{
				JumpToDeclaration(focusEditor: true);
			};
			lastCU = base.Document.ParsedDocument;
			outlineTreeView.Realized += delegate
			{
				RefillOutlineStore();
			};
			UpdateSorting();
			CompactScrolledWindow compactScrolledWindow = new CompactScrolledWindow();
			compactScrolledWindow.Add(outlineTreeView);
			compactScrolledWindow.ShowAll();
			return compactScrolledWindow;
		}

		IEnumerable<Widget> IOutlinedDocument.GetToolbarWidgets()
		{
			if (toolbarWidgets != null)
			{
				return toolbarWidgets;
			}
			ToggleButton groupToggleButton = new ToggleButton
			{
				Image = new Gtk.Image(MonoDevelop.Ide.Gui.Stock.GroupByCategory, IconSize.Menu),
				TooltipText = GettextCatalog.GetString("Group entries by type"),
				Active = settings.IsGrouped
			};
			groupToggleButton.Toggled += delegate
			{
				if (groupToggleButton.Active != settings.IsGrouped)
				{
					settings.IsGrouped = groupToggleButton.Active;
					UpdateSorting();
				}
			};
			ToggleButton sortAlphabeticallyToggleButton = new ToggleButton
			{
				Image = new Gtk.Image(MonoDevelop.Ide.Gui.Stock.SortAlphabetically, IconSize.Menu),
				TooltipText = GettextCatalog.GetString("Sort entries alphabetically"),
				Active = settings.IsSorted
			};
			sortAlphabeticallyToggleButton.Toggled += delegate
			{
				if (sortAlphabeticallyToggleButton.Active != settings.IsSorted)
				{
					settings.IsSorted = sortAlphabeticallyToggleButton.Active;
					UpdateSorting();
				}
			};
			DockToolButton dockToolButton = new DockToolButton(MonoDevelop.Ide.Gui.Stock.Options);
			dockToolButton.TooltipText = GettextCatalog.GetString("Open preferences dialog");
			DockToolButton dockToolButton2 = dockToolButton;
			dockToolButton2.Clicked += delegate
			{
				ClassOutlineSortingPreferencesDialog classOutlineSortingPreferencesDialog = new ClassOutlineSortingPreferencesDialog(settings);
				try
				{
					if (MessageService.ShowCustomDialog(classOutlineSortingPreferencesDialog) == -5)
					{
						classOutlineSortingPreferencesDialog.SaveSettings();
						comparer = new ClassOutlineNodeComparer(GetAmbience(), settings, outlineTreeModelSort);
						UpdateSorting();
					}
				}
				finally
				{
					classOutlineSortingPreferencesDialog.Destroy();
				}
			};
			return toolbarWidgets = new Widget[4]
			{
				groupToggleButton,
				sortAlphabeticallyToggleButton,
				new VSeparator(),
				dockToolButton2
			};
		}

		private void JumpToDeclaration(bool focusEditor)
		{
			if (outlineReady && outlineTreeView.Selection.GetSelected(out var iter))
			{
				object obj = ((!IsSorting()) ? outlineTreeStore.GetValue(iter, 0) : outlineTreeStore.GetValue(outlineTreeModelSort.ConvertIterToChildIter(iter), 0));
				IdeApp.ProjectOperations.JumpToDeclaration(obj as IEntity);
				if (focusEditor)
				{
					IdeApp.Workbench.ActiveDocument.Select();
				}
			}
		}

		private void OutlineTreeIconFunc(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CellRendererImage cellRendererImage = (CellRendererImage)cell;
			object value = model.GetValue(iter, 0);
			if (value is IEntity)
			{
				cellRendererImage.Image = ImageService.GetIcon(((IEntity)value).GetStockIcon(), IconSize.Menu);
			}
			else if (value is FoldingRegion)
			{
				cellRendererImage.Image = ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Add, IconSize.Menu);
			}
		}

		private void OutlineTreeTextFunc(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
		{
			CellRendererText cellRendererText = (CellRendererText)cell;
			object value = model.GetValue(iter, 0);
			Ambience ambience = GetAmbience();
			if (value is IEntity)
			{
				cellRendererText.Text = ambience.GetString((IEntity)value, OutputFlags.ClassBrowserEntries);
			}
			else if (value is FoldingRegion)
			{
				string text = ((FoldingRegion)value).Name.Trim();
				if (string.IsNullOrEmpty(text))
				{
					text = "#region";
				}
				cellRendererText.Text = text;
			}
		}

		void IOutlinedDocument.ReleaseOutlineWidget()
		{
			if (outlineTreeView != null)
			{
				ScrolledWindow scrolledWindow = (ScrolledWindow)outlineTreeView.Parent;
				scrolledWindow.Destroy();
				if (outlineTreeModelSort != null)
				{
					outlineTreeModelSort.Dispose();
					outlineTreeModelSort = null;
				}
				if (outlineTreeStore != null)
				{
					outlineTreeStore.Dispose();
					outlineTreeStore = null;
				}
				outlineTreeView = null;
				settings = null;
				Widget[] array = toolbarWidgets;
				foreach (Widget widget in array)
				{
					widget.Destroy();
				}
				toolbarWidgets = null;
				comparer = null;
			}
		}

		private void RemoveRefillOutlineStoreTimeout()
		{
			if (refillOutlineStoreId != 0)
			{
				Source.Remove(refillOutlineStoreId);
				refillOutlineStoreId = 0u;
			}
		}

		private void UpdateDocumentOutline(object sender, EventArgs args)
		{
			lastCU = base.Document.ParsedDocument;
			if (!refreshingOutline)
			{
				refreshingOutline = true;
				refillOutlineStoreId = GLib.Timeout.Add(3000u, RefillOutlineStore);
			}
		}

		private bool RefillOutlineStore()
		{
			DispatchService.AssertGuiThread();
			Threads.Enter();
			refreshingOutline = false;
			if (outlineTreeStore == null || !outlineTreeView.IsRealized)
			{
				refillOutlineStoreId = 0u;
				return false;
			}
			outlineReady = false;
			outlineTreeStore.Clear();
			if (lastCU != null)
			{
				BuildTreeChildren(outlineTreeStore, TreeIter.Zero, lastCU);
				TreeIter iter;
				if (IsSorting())
				{
					if (outlineTreeModelSort.GetIterFirst(out iter))
					{
						outlineTreeView.Selection.SelectIter(iter);
					}
				}
				else if (outlineTreeStore.GetIterFirst(out iter))
				{
					outlineTreeView.Selection.SelectIter(iter);
				}
				outlineTreeView.ExpandAll();
			}
			outlineReady = true;
			Threads.Leave();
			refillOutlineStoreId = 0u;
			return false;
		}

		private void BuildTreeChildren(TreeStore store, TreeIter parent, ParsedDocument parsedDocument)
		{
			if (parsedDocument == null)
			{
				return;
			}
			foreach (IUnresolvedTypeDefinition topLevelTypeDefinition in parsedDocument.TopLevelTypeDefinitions)
			{
				ITypeDefinition typeDefinition = document.Compilation.MainAssembly.GetTypeDefinition(topLevelTypeDefinition.FullTypeName);
				if (typeDefinition != null)
				{
					TreeIter parent2 = (parent.Equals(TreeIter.Zero) ? store.AppendValues(typeDefinition) : store.AppendValues(parent, typeDefinition));
					AddTreeClassContents(store, parent2, parsedDocument, typeDefinition, topLevelTypeDefinition);
				}
			}
		}

		private static void AddTreeClassContents(TreeStore store, TreeIter parent, ParsedDocument parsedDocument, ITypeDefinition cls, IUnresolvedTypeDefinition part)
		{
			List<object> list = new List<object>();
			if (cls.Kind != TypeKind.Delegate)
			{
				foreach (IMember member in cls.GetMembers((IUnresolvedMember m) => part.Region.FileName == m.Region.FileName && part.Region.IsInside(m.Region.Begin)))
				{
					list.Add(member);
				}
				foreach (IType nestedType in cls.GetNestedTypes((ITypeDefinition m) => part.Region.FileName == m.Region.FileName && part.Region.IsInside(m.Region.Begin)))
				{
					if (nestedType.DeclaringType == cls)
					{
						list.Add(nestedType);
					}
				}
				foreach (IMethod constructor in cls.GetConstructors((IUnresolvedMethod m) => part.Region.FileName == m.Region.FileName && part.Region.IsInside(m.Region.Begin)))
				{
					if (!constructor.IsSynthetic)
					{
						list.Add(constructor);
					}
				}
			}
			list.Sort(ClassOutlineNodeComparer.CompareRegion);
			List<FoldingRegion> list2 = new List<FoldingRegion>();
			foreach (FoldingRegion userRegion in parsedDocument.UserRegions)
			{
				if (cls.BodyRegion.IsInside(userRegion.Region.Begin) && cls.BodyRegion.IsInside(userRegion.Region.End))
				{
					list2.Add(userRegion);
				}
			}
			list2.Sort((FoldingRegion x, FoldingRegion y) => x.Region.Begin.CompareTo(y.Region.Begin));
			IEnumerator<FoldingRegion> enumerator5 = list2.GetEnumerator();
			if (!enumerator5.MoveNext())
			{
				enumerator5 = null;
			}
			FoldingRegion foldingRegion = null;
			TreeIter parent2 = parent;
			foreach (object item in list)
			{
				if (enumerator5 != null)
				{
					DomRegion region = ClassOutlineNodeComparer.GetRegion(item);
					while (enumerator5 != null && !OuterEndsAfterInner(enumerator5.Current.Region, region))
					{
						if (!enumerator5.MoveNext())
						{
							enumerator5 = null;
						}
					}
					if (enumerator5 != null && enumerator5.Current.Region.IsInside(region.Begin))
					{
						if (foldingRegion != enumerator5.Current)
						{
							parent2 = store.AppendValues(parent, enumerator5.Current);
							foldingRegion = enumerator5.Current;
						}
					}
					else
					{
						parent2 = parent;
					}
				}
				TreeIter parent3 = store.AppendValues(parent2, item);
				if (item is ITypeDefinition)
				{
					AddTreeClassContents(store, parent3, parsedDocument, (ITypeDefinition)item, part);
				}
			}
		}

		private static DomRegion GetRegion(object o)
		{
			if (o is IEntity)
			{
				IEntity entity = (IEntity)o;
				return entity.Region;
			}
			throw new InvalidOperationException(o.GetType().ToString());
		}

		private static bool OuterEndsAfterInner(DomRegion outer, DomRegion inner)
		{
			if (outer.End.Line <= 1 || outer.End.Line <= inner.End.Line)
			{
				if (outer.End.Line == inner.End.Line)
				{
					return outer.End.Column > inner.End.Column;
				}
				return false;
			}
			return true;
		}

		private void UpdateSorting()
		{
			if (IsSorting())
			{
				outlineTreeModelSort.SetSortFunc(0, comparer.CompareNodes);
				outlineTreeView.Model = outlineTreeModelSort;
			}
			else
			{
				outlineTreeView.Model = outlineTreeStore;
			}
			outlineTreeView.ExpandAll();
		}

		private bool IsSorting()
		{
			if (!settings.IsGrouped)
			{
				return settings.IsSorted;
			}
			return true;
		}
	}
}
