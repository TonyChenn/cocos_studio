using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gdk;
using GLib;
using Gtk;
using MonoDevelop.Components;
using Xwt.GtkBackend;

namespace Modules.Communal.ResourcePanel
{
	internal class ResourceGridView : ScrolledWindow
	{
		private const int ThumbnailSize = 40;
		private const uint LoadIntervalMilliseconds = 16U;
		private const int LoadBatchSize = 8;
		private const int LoadTimeBudgetMilliseconds = 6;
		private const int ImageColumn = 0;
		private const int TextColumn = 1;
		private const int ResourceColumn = 2;
		private const int TooltipColumn = 3;

		public ResourceFolder CurrentFolder { get; private set; }

		public bool HasSelection
		{
			get
			{
				return this.iconView.SelectedItems.Length > 0;
			}
		}

		public ResourceGridView(ResourceWidget resourceWidget)
		{
			this.resourceWidget = resourceWidget;
			this.store = new ListStore(typeof(Pixbuf), typeof(string), typeof(ResourceItem), typeof(string));
			this.iconView = new IconView(this.store);
			this.iconView.Orientation = Gtk.Orientation.Vertical;
			this.iconView.SelectionMode = SelectionMode.Multiple;
			this.iconView.ItemWidth = 64;
			this.iconView.Columns = -1;
			this.iconView.Margin = 4;
			this.iconView.RowSpacing = 4;
			this.iconView.ColumnSpacing = 2;
			this.iconView.Spacing = 1;
			this.iconView.TooltipColumn = ResourceGridView.TooltipColumn;
			this.iconView.PixbufColumn = ResourceGridView.ImageColumn;
			this.iconView.TextColumn = ResourceGridView.TextColumn;
			this.pixbufRenderer = this.iconView.Cells.OfType<CellRendererPixbuf>().First();
			this.textRenderer = this.iconView.Cells.OfType<CellRendererText>().First();
			this.textRenderer.SingleParagraphMode = false;
			this.textRenderer.WrapMode = Pango.WrapMode.WordChar;
			this.textRenderer.WrapWidth = 60;
			this.textRenderer.FixedHeightFromFont = 2;
			this.textRenderer.Ellipsize = Pango.EllipsizeMode.None;
			this.iconView.SelectionChanged += this.IconView_SelectionChanged;
			this.iconView.ItemActivated += this.IconView_ItemActivated;
			this.iconView.ButtonReleaseEvent += this.IconView_ButtonReleaseEvent;
			this.iconView.DragBegin += this.IconView_DragBegin;
			this.textRenderer.EditingStarted += this.TextRenderer_EditingStarted;
			this.textRenderer.Edited += this.TextRenderer_Edited;
			this.textRenderer.EditingCanceled += this.TextRenderer_EditingCanceled;
			this.iconView.EnableModelDragSource(ModifierType.Button1Mask, ResourceGridView.dragTargets, DragAction.Copy | DragAction.Move | DragAction.Link);
			Gtk.Drag.DestSet(this.iconView, DestDefaults.All, ResourceGridView.fileDropTargets, DragAction.Copy);
			this.iconView.DragDataReceived += this.IconView_DragDataReceived;
			base.Add(this.iconView);
		}

		public void SetCurrentFolder(ResourceFolder folder)
		{
			this.CurrentFolder = folder;
		}

		public void Refresh(string filterText)
		{
			ResourceFolder rootFolder = this.GetRootFolder();
			TreeIter currentFolderIter;
			if (this.CurrentFolder == null || !this.resourceWidget.TreeView.Builder.GetFirstNode(this.CurrentFolder, out currentFolderIter))
			{
				this.CurrentFolder = rootFolder;
			}
			List<ResourceItem> items = this.GetItems(rootFolder, filterText).OrderBy((ResourceItem item) => item is ResourceFolder ? 0 : 1).ThenBy((ResourceItem item) => item.Name, StringComparer.CurrentCultureIgnoreCase).ToList<ResourceItem>();
			List<ResourceItem> selectedItems = this.resourceWidget.TreeView.Builder.GetCurrentSelectes();
			this.ClearItems();
			if (items.Count == 0)
			{
				this.SetSelectedItems(selectedItems);
				return;
			}
			ListStore nextStore = new ListStore(typeof(Pixbuf), typeof(string), typeof(ResourceItem), typeof(string));
			Dictionary<ResourceItem, TreePath> nextItemPaths = new Dictionary<ResourceItem, TreePath>();
			this.loadingStore = nextStore;
			int loadVersion = this.loadVersion;
			int itemIndex = 0;
			this.loadTimerId = GLib.Timeout.Add(ResourceGridView.LoadIntervalMilliseconds, delegate
			{
				if (loadVersion != this.loadVersion || this.loadingStore != nextStore)
				{
					return false;
				}
				Stopwatch stopwatch = Stopwatch.StartNew();
				int batchCount = 0;
				do
				{
					ResourceItem resourceItem = items[itemIndex++];
					Pixbuf thumbnail = this.CreateThumbnail(resourceItem);
					TreeIter iter = nextStore.AppendValues(thumbnail, resourceItem.Name, resourceItem, GLib.Markup.EscapeText(resourceItem.FullPath));
					nextItemPaths[resourceItem] = nextStore.GetPath(iter);
					batchCount++;
				}
				while (itemIndex < items.Count && batchCount < ResourceGridView.LoadBatchSize && stopwatch.ElapsedMilliseconds < ResourceGridView.LoadTimeBudgetMilliseconds);
				if (itemIndex < items.Count)
				{
					return true;
				}
				this.loadTimerId = 0U;
				this.loadingStore = null;
				ListStore previousStore = this.store;
				this.store = nextStore;
				this.itemPaths = nextItemPaths;
				this.synchronizingSelection = true;
				this.iconView.Model = this.store;
				this.synchronizingSelection = false;
				previousStore.Dispose();
				this.SetSelectedItems(selectedItems);
				this.iconView.ShowAll();
				return false;
			});
		}

		public void Clear()
		{
			this.CurrentFolder = null;
			this.InvalidateThumbnails();
		}

		public void InvalidateThumbnails()
		{
			this.ClearItems();
			foreach (Pixbuf pixbuf in this.thumbnailCache.Values)
			{
				pixbuf.Dispose();
			}
			this.thumbnailCache.Clear();
		}

		public void StartLabelEdit()
		{
			TreePath[] selectedItems = this.iconView.SelectedItems;
			if (selectedItems == null || selectedItems.Length != 1)
			{
				return;
			}
			TreeIter iter;
			if (!this.store.GetIter(out iter, selectedItems[0]))
			{
				return;
			}
			this.editingResource = this.store.GetValue(iter, ResourceGridView.ResourceColumn) as ResourceItem;
			this.textRenderer.Editable = true;
			this.iconView.SetCursor(selectedItems[0], this.textRenderer, true);
		}

		private ResourceFolder GetRootFolder()
		{
			Solution solution = Services.ProjectOperations.CurrentSelectedSolution;
			return solution == null ? null : solution.GetRootFolder();
		}

		private IEnumerable<ResourceItem> GetItems(ResourceFolder rootFolder, string filterText)
		{
			if (rootFolder == null)
			{
				return Enumerable.Empty<ResourceItem>();
			}
			if (string.IsNullOrWhiteSpace(filterText))
			{
				return this.CurrentFolder == null ? Enumerable.Empty<ResourceItem>() : this.CurrentFolder.Items;
			}
			List<ResourceItem> result = new List<ResourceItem>();
			this.FindItems(rootFolder, filterText, result);
			return result;
		}

		private void FindItems(ResourceFolder folder, string filterText, IList<ResourceItem> result)
		{
			foreach (ResourceItem resourceItem in folder.Items)
			{
				if (resourceItem.Name.IndexOf(filterText, StringComparison.CurrentCultureIgnoreCase) >= 0)
				{
					result.Add(resourceItem);
				}
				ResourceFolder childFolder = resourceItem as ResourceFolder;
				if (childFolder != null)
				{
					this.FindItems(childFolder, filterText, result);
				}
			}
		}

		private Pixbuf CreateThumbnail(ResourceItem resourceItem)
		{
			Pixbuf cachedThumbnail;
			if (this.thumbnailCache.TryGetValue(resourceItem, out cachedThumbnail))
			{
				return cachedThumbnail;
			}
			Pixbuf source = null;
			if (!(resourceItem is ResourceFolder))
			{
				try
				{
					PreviewImageInfo previewImageInfo = resourceItem.PreviewImageInfo;
					source = previewImageInfo == null ? null : previewImageInfo.Image;
				}
				catch (Exception)
				{
				}
			}
			if (source == null)
			{
				source = this.resourceWidget.GetResourceIcon(resourceItem);
			}
			if (source == null || source.Width <= 0 || source.Height <= 0)
			{
				return null;
			}
			double scale = Math.Min((double)ResourceGridView.ThumbnailSize / source.Width, (double)ResourceGridView.ThumbnailSize / source.Height);
			int width = Math.Max(1, (int)Math.Round(source.Width * scale));
			int height = Math.Max(1, (int)Math.Round(source.Height * scale));
			Pixbuf thumbnail = new Pixbuf(Colorspace.Rgb, true, 8, ResourceGridView.ThumbnailSize, ResourceGridView.ThumbnailSize);
			thumbnail.Fill(0U);
			using (Pixbuf scaled = source.ScaleSimple(width, height, InterpType.Bilinear))
			{
				scaled.CopyArea(0, 0, width, height, thumbnail, (ResourceGridView.ThumbnailSize - width) / 2, (ResourceGridView.ThumbnailSize - height) / 2);
			}
			this.thumbnailCache[resourceItem] = thumbnail;
			return thumbnail;
		}

		private void SetSelectedItems(IEnumerable<ResourceItem> selectedItems)
		{
			this.synchronizingSelection = true;
			this.iconView.UnselectAll();
			TreePath firstSelectedPath = null;
			if (selectedItems != null)
			{
				foreach (ResourceItem resourceItem in selectedItems)
				{
					TreePath path;
					if (this.itemPaths.TryGetValue(resourceItem, out path))
					{
						this.iconView.SelectPath(path);
						if (firstSelectedPath == null)
						{
							firstSelectedPath = path;
						}
					}
				}
			}
			if (firstSelectedPath != null)
			{
				this.iconView.ScrollToPath(firstSelectedPath, 0.5f, 0.5f);
			}
			this.synchronizingSelection = false;
		}

		private List<ResourceItem> GetSelectedItems()
		{
			List<ResourceItem> result = new List<ResourceItem>();
			foreach (TreePath path in this.iconView.SelectedItems)
			{
				TreeIter iter;
				if (this.store.GetIter(out iter, path))
				{
					ResourceItem resourceItem = this.store.GetValue(iter, ResourceGridView.ResourceColumn) as ResourceItem;
					if (resourceItem != null)
					{
						result.Add(resourceItem);
					}
				}
			}
			return result;
		}

		private void ClearItems()
		{
			this.CancelPendingLoad();
			this.synchronizingSelection = true;
			this.store.Clear();
			this.itemPaths.Clear();
			this.synchronizingSelection = false;
		}

		private void CancelPendingLoad()
		{
			this.loadVersion++;
			if (this.loadTimerId != 0U)
			{
				Source.Remove(this.loadTimerId);
				this.loadTimerId = 0U;
			}
			if (this.loadingStore != null)
			{
				this.loadingStore.Dispose();
				this.loadingStore = null;
			}
		}

		private void IconView_SelectionChanged(object sender, EventArgs e)
		{
			if (!this.synchronizingSelection)
			{
				this.resourceWidget.SelectGridResources(this.GetSelectedItems());
			}
		}

		private void IconView_ItemActivated(object o, ItemActivatedArgs args)
		{
			TreeIter iter;
			if (!this.store.GetIter(out iter, args.Path))
			{
				return;
			}
			ResourceItem resourceItem = this.store.GetValue(iter, ResourceGridView.ResourceColumn) as ResourceItem;
			ResourceFolder folder = resourceItem as ResourceFolder;
			if (folder != null)
			{
				this.resourceWidget.NavigateGridTo(folder);
				return;
			}
			this.resourceWidget.OpenGridResource(resourceItem);
		}

		[ConnectBefore]
		private void IconView_ButtonReleaseEvent(object o, ButtonReleaseEventArgs args)
		{
			if (!args.Event.IsContextMenuButton())
			{
				return;
			}
			TreePath path = this.iconView.GetPathAtPos((int)args.Event.X, (int)args.Event.Y);
			if (path != null && !this.iconView.PathIsSelected(path))
			{
				this.iconView.UnselectAll();
				this.iconView.SelectPath(path);
			}
			this.iconView.GrabFocus();
			GtkWorkarounds.ShowContextMenu(ResourceMenu.ContextMenu, this.iconView, args.Event);
		}

		private void IconView_DragBegin(object o, DragBeginArgs args)
		{
			this.resourceWidget.TreeView.Builder.SetDragData(args.Context);
		}

		private void IconView_DragDataReceived(object o, DragDataReceivedArgs args)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null || !this.IsExternalFileDrop(args.Context) || args.SelectionData.Type == null)
			{
				return;
			}
			IEnumerable<string> fileArray = args.SelectionData.GetFileArray().FileArray;
			if (fileArray == null || !fileArray.Any())
			{
				return;
			}
			ResourceFolder folder = this.GetImportTargetFolder(args.X, args.Y);
			this.resourceWidget.TreeView.Builder.ImportResources(fileArray.ToArray<string>(), folder);
		}

		private bool IsExternalFileDrop(DragContext context)
		{
			return context.DragProtocol == DragProtocol.Win32Dropfiles || context.GetSourceWidget() == null;
		}

		private ResourceFolder GetImportTargetFolder(int x, int y)
		{
			TreePath path = this.iconView.GetPathAtPos(x, y);
			if (path != null)
			{
				TreeIter iter;
				if (this.store.GetIter(out iter, path))
				{
					ResourceFolder folder = this.store.GetValue(iter, ResourceGridView.ResourceColumn) as ResourceFolder;
					if (folder != null)
					{
						return folder;
					}
				}
			}
			ResourceFolder rootFolder = this.GetRootFolder();
			return this.CurrentFolder ?? rootFolder;
		}

		private void TextRenderer_EditingStarted(object o, EditingStartedArgs args)
		{
			Entry entry = args.Editable as Entry;
			ResourceItem resourceItem = this.editingResource;
			if (entry == null || resourceItem == null)
			{
				return;
			}
			Idle.Add(delegate
			{
				if (resourceItem is ResourceFile)
				{
					int length = System.IO.Path.GetFileNameWithoutExtension(resourceItem.FullPath).Length;
					entry.SelectRegion(0, length);
				}
				else
				{
					entry.SelectRegion(0, entry.Text.Length);
				}
				return false;
			});
		}

		private void TextRenderer_Edited(object o, EditedArgs args)
		{
			this.textRenderer.Editable = false;
			ResourceItem resourceItem = this.editingResource;
			this.editingResource = null;
			if (resourceItem == null)
			{
				return;
			}
			string newName = args.NewText;
			if (resourceItem is ResourceFile && string.Equals(System.IO.Path.GetExtension(newName), System.IO.Path.GetExtension(resourceItem.FullPath), StringComparison.CurrentCultureIgnoreCase))
			{
				newName = System.IO.Path.GetFileNameWithoutExtension(newName);
			}
			this.resourceWidget.RenameGridResource(resourceItem, newName);
		}

		private void TextRenderer_EditingCanceled(object sender, EventArgs e)
		{
			this.textRenderer.Editable = false;
			this.editingResource = null;
		}

		protected override void OnDestroyed()
		{
			this.ClearItems();
			this.InvalidateThumbnails();
			base.OnDestroyed();
		}

		private static TargetEntry[] dragTargets = new TargetEntry[]
		{
			DragTargetType.FileDropTarget,
			DragTargetType.CocoStudioTarget
		};

		private static TargetEntry[] fileDropTargets = new TargetEntry[]
		{
			DragTargetType.FileDropTarget
		};

		private ResourceWidget resourceWidget;
		private ListStore store;
		private IconView iconView;
		private CellRendererPixbuf pixbufRenderer;
		private CellRendererText textRenderer;
		private ResourceItem editingResource;
		private bool synchronizingSelection;
		private int loadVersion;
		private uint loadTimerId;
		private ListStore loadingStore;
		private Dictionary<ResourceItem, TreePath> itemPaths = new Dictionary<ResourceItem, TreePath>();
		private Dictionary<ResourceItem, Pixbuf> thumbnailCache = new Dictionary<ResourceItem, Pixbuf>();
	}
}
