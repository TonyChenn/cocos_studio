using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using MonoDevelop.Components;
using MonoDevelop.Components.Commands;
using MonoDevelop.Components.Docking;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Commands;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	public class Toolbox : VBox, IPropertyPadProvider, IToolboxConfiguration
	{
		private ToolboxService toolboxService;

		private ItemToolboxNode selectedNode;

		private ToolboxWidget toolboxWidget;

		private ScrolledWindow scrolledWindow;

		private ToggleButton catToggleButton;

		private ToggleButton compactModeToggleButton;

		private SearchEntry filterEntry;

		private PadFontChanger fontChanger;

		private IPadWindow container;

		private Dictionary<string, int> categoryPriorities = new Dictionary<string, int>();

		private Button toolboxAddButton;

		private static readonly string GtkWidgetDomain = GettextCatalog.GetString("GTK# Widgets");

		private Dictionary<string, Category> categories = new Dictionary<string, Category>();

		public bool AllowEditingComponents
		{
			get
			{
				return toolboxAddButton.Visible;
			}
			set
			{
				toolboxAddButton.Visible = value;
			}
		}

		public Toolbox(ToolboxService toolboxService, IPadWindow container)
		{
			Toolbox toolbox = this;
			this.toolboxService = toolboxService;
			this.container = container;
			DockItemToolbar toolbar = container.GetToolbar(PositionType.Top);
			filterEntry = new SearchEntry();
			filterEntry.Ready = true;
			filterEntry.HasFrame = true;
			filterEntry.WidthRequest = 150;
			filterEntry.Changed += filterTextChanged;
			filterEntry.Show();
			toolbar.Add(filterEntry, fill: true);
			catToggleButton = new ToggleButton();
			catToggleButton.Image = new Gtk.Image(MonoDevelop.Ide.Gui.Stock.GroupByCategory, IconSize.Menu);
			catToggleButton.Toggled += toggleCategorisation;
			catToggleButton.TooltipText = GettextCatalog.GetString("Show categories");
			toolbar.Add(catToggleButton);
			compactModeToggleButton = new ToggleButton();
			compactModeToggleButton.Image = new ImageView(ImageService.GetIcon("md-compact-display", IconSize.Menu));
			compactModeToggleButton.Toggled += ToggleCompactMode;
			compactModeToggleButton.TooltipText = GettextCatalog.GetString("Use compact display");
			toolbar.Add(compactModeToggleButton);
			toolboxAddButton = new Button(new Gtk.Image(MonoDevelop.Ide.Gui.Stock.Add, IconSize.Menu));
			toolbar.Add(toolboxAddButton);
			toolboxAddButton.TooltipText = GettextCatalog.GetString("Add toolbox items");
			toolboxAddButton.Clicked += toolboxAddButton_Clicked;
			toolbar.ShowAll();
			toolboxWidget = new ToolboxWidget();
			ToolboxWidget obj = toolboxWidget;
			EventHandler value = delegate
			{
				toolbox.selectedNode = ((toolbox.toolboxWidget.SelectedItem != null) ? (toolbox.toolboxWidget.SelectedItem.Tag as ItemToolboxNode) : null);
				toolboxService.SelectItem(toolbox.selectedNode);
			};
			obj.SelectedItemChanged += value;
			toolboxWidget.DragBegin += delegate(object sender, DragBeginArgs e)
			{
				if (toolbox.toolboxWidget.SelectedItem != null)
				{
					toolbox.toolboxWidget.HideTooltipWindow();
					toolboxService.DragSelectedItem(toolbox.toolboxWidget, e.Context);
				}
			};
			toolboxWidget.ActivateSelectedItem += delegate
			{
				toolboxService.UseSelectedItem();
			};
			fontChanger = new PadFontChanger(toolboxWidget, toolboxWidget.SetCustomFont, toolboxWidget.QueueResize);
			toolboxWidget.DoPopupMenu = ShowPopup;
			scrolledWindow = new CompactScrolledWindow();
			PackEnd(scrolledWindow, expand: true, fill: true, 0u);
			base.FocusChain = new Widget[1] { scrolledWindow };
			scrolledWindow.ShadowType = ShadowType.None;
			scrolledWindow.VscrollbarPolicy = PolicyType.Automatic;
			scrolledWindow.HscrollbarPolicy = PolicyType.Never;
			scrolledWindow.WidthRequest = 150;
			scrolledWindow.Add(toolboxWidget);
			toolboxService.ToolboxContentsChanged += delegate
			{
				Refresh();
			};
			toolboxService.ToolboxConsumerChanged += delegate
			{
				Refresh();
			};
			Refresh();
			toolboxWidget.ShowCategories = (catToggleButton.Active = true);
			compactModeToggleButton.Active = PropertyService.Get("ToolboxIsInCompactMode", defaultValue: false);
			toolboxWidget.IsListMode = !compactModeToggleButton.Active;
			ShowAll();
		}

		private void ToggleCompactMode(object sender, EventArgs e)
		{
			toolboxWidget.IsListMode = !compactModeToggleButton.Active;
			PropertyService.Set("ToolboxIsInCompactMode", compactModeToggleButton.Active);
		}

		private void toggleCategorisation(object sender, EventArgs e)
		{
			toolboxWidget.ShowCategories = catToggleButton.Active;
		}

		private void filterTextChanged(object sender, EventArgs e)
		{
			foreach (Category category in toolboxWidget.Categories)
			{
				bool flag = false;
				foreach (Item item in category.Items)
				{
					item.IsVisible = ((ItemToolboxNode)item.Tag).Filter(filterEntry.Entry.Text);
					flag |= item.IsVisible;
				}
				category.IsVisible = flag;
			}
			toolboxWidget.QueueDraw();
			toolboxWidget.QueueResize();
		}

		private void toolboxAddButton_Clicked(object sender, EventArgs e)
		{
			toolboxService.AddUserItems();
		}

		private void ShowPopup(EventButton evt)
		{
			if (AllowEditingComponents)
			{
				CommandEntrySet entrySet = IdeApp.CommandService.CreateCommandEntrySet("/MonoDevelop/DesignerSupport/ToolboxItemContextMenu");
				IdeApp.CommandService.ShowContextMenu(this, evt, entrySet, this);
			}
		}

		[CommandHandler(EditCommands.Delete)]
		internal void OnDeleteItem()
		{
			if (MessageService.Confirm(GettextCatalog.GetString("Are you sure you want to remove the selected Item?"), AlertButton.Delete))
			{
				toolboxService.RemoveUserItem(selectedNode);
			}
		}

		[CommandUpdateHandler(EditCommands.Delete)]
		internal void OnUpdateDeleteItem(CommandInfo info)
		{
			info.Enabled = selectedNode != null && (selectedNode.ItemDomain != GtkWidgetDomain || (selectedNode.Category != "Widgets" && selectedNode.Category != "Container"));
		}

		private void AddItems(IEnumerable<ItemToolboxNode> nodes)
		{
			foreach (ItemToolboxNode node in nodes)
			{
				Item item = new Item(node);
				if (!categories.ContainsKey(node.Category))
				{
					Category category = new Category(node.Category);
					if (!categoryPriorities.TryGetValue(node.Category, out var value))
					{
						value = -1;
					}
					category.Priority = value;
					categories[node.Category] = category;
				}
				if (item.Text != null)
				{
					categories[node.Category].Add(item);
				}
			}
		}

		public void Refresh()
		{
			DispatchService.AssertGuiThread();
			if (toolboxService.Initializing)
			{
				toolboxWidget.CustomMessage = GettextCatalog.GetString("Initializing...");
				return;
			}
			ConfigureToolbar();
			toolboxWidget.CustomMessage = null;
			categories.Clear();
			AddItems(toolboxService.GetCurrentToolboxItems());
			Gtk.Drag.SourceUnset(toolboxWidget);
			toolboxWidget.ClearCategories();
			List<Category> list = categories.Values.ToList();
			list.Sort((Category a, Category b) => (a.Priority == b.Priority) ? a.Text.CompareTo(b.Text) : a.Priority.CompareTo(b.Priority));
			list.Reverse();
			foreach (Category item in list)
			{
				item.IsExpanded = true;
				toolboxWidget.AddCategory(item);
			}
			toolboxWidget.QueueResize();
			TargetEntry[] currentDragTargetTable = toolboxService.GetCurrentDragTargetTable();
			if (currentDragTargetTable != null)
			{
				Gtk.Drag.SourceSet(toolboxWidget, ModifierType.Button1Mask, currentDragTargetTable, DragAction.Copy | DragAction.Move);
			}
			compactModeToggleButton.Visible = toolboxWidget.CanIconizeToolboxCategories;
		}

		private void ConfigureToolbar()
		{
			categoryPriorities.Clear();
			toolboxAddButton.Visible = true;
			toolboxService.Customize(container, this);
		}

		protected override void OnDestroyed()
		{
			if (fontChanger != null)
			{
				fontChanger.Dispose();
				fontChanger = null;
			}
			base.OnDestroyed();
		}

		object IPropertyPadProvider.GetActiveComponent()
		{
			return selectedNode;
		}

		object IPropertyPadProvider.GetProvider()
		{
			return selectedNode;
		}

		void IPropertyPadProvider.OnEndEditing(object obj)
		{
		}

		void IPropertyPadProvider.OnChanged(object obj)
		{
		}

		public void SetCategoryPriority(string category, int priority)
		{
			categoryPriorities[category] = priority;
		}
	}
}
