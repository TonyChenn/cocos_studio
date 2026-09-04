using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using GLib;
using Gdk;
using Gtk;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.DesignerSupport.Toolbox;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	public class ToolboxService
	{
		private static readonly string toolboxLoaderPath = "/MonoDevelop/DesignerSupport/ToolboxLoaders";

		private static readonly string toolboxProviderPath = "/MonoDevelop/DesignerSupport/ToolboxProviders";

		private ToolboxConfiguration config;

		private int initializing;

		private List<IToolboxLoader> loaders = new List<IToolboxLoader>();

		private List<IToolboxDynamicProvider> dynamicProviders = new List<IToolboxDynamicProvider>();

		private IToolboxConsumer currentConsumer;

		private ItemToolboxNode selectedItem;

		private IToolboxCustomizer customizer;

		private Document oldActiveDoc;

		private Project oldProject;

		private bool configChanged;

		private IToolboxDynamicProvider viewProvider;

		private static string ToolboxConfigFile => UserProfile.Current.LocalConfigDir.Combine("Toolbox.xml");

		public bool Initializing => initializing > 0;

		private ToolboxConfiguration Configuration
		{
			get
			{
				if (config == null)
				{
					try
					{
						if (File.Exists(ToolboxConfigFile))
						{
							config = ToolboxConfiguration.LoadFromFile(ToolboxConfigFile);
						}
						else
						{
							config = new ToolboxConfiguration();
						}
					}
					catch (Exception ex)
					{
						LoggingService.LogError(ex.ToString());
						config = new ToolboxConfiguration();
					}
				}
				return config;
			}
		}

		internal IEnumerable<ItemToolboxNode> UserItems => Configuration.ItemList;

		public IToolboxConsumer CurrentConsumer
		{
			get
			{
				return currentConsumer;
			}
			private set
			{
				currentConsumer = value;
				OnToolboxConsumerChanged(currentConsumer);
			}
		}

		public ItemToolboxNode SelectedItem => selectedItem;

		public event EventHandler ToolboxContentsChanged;

		public event ToolboxConsumerChangedHandler ToolboxConsumerChanged;

		public event ToolboxSelectionChangedHandler ToolboxSelectionChanged;

		public event ToolboxUsedHandler ToolboxUsed;

		internal ToolboxService()
		{
			if (IdeApp.Workbench != null)
			{
				IdeApp.Workbench.ActiveDocumentChanged += onActiveDocChanged;
			}
			AddinManager.AddExtensionNodeHandler(toolboxLoaderPath, OnLoaderExtensionChanged);
			AddinManager.AddExtensionNodeHandler(toolboxProviderPath, OnProviderExtensionChanged);
			if (IdeApp.Workbench != null)
			{
				onActiveDocChanged(null, null);
			}
		}

		private void OnLoaderExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				loaders.Add((IToolboxLoader)args.ExtensionObject);
			}
			else if (args.Change == ExtensionChange.Remove)
			{
				loaders.Remove((IToolboxLoader)args.ExtensionObject);
			}
		}

		private void OnProviderExtensionChanged(object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				if (args.ExtensionObject is IToolboxDynamicProvider toolboxDynamicProvider)
				{
					dynamicProviders.Add((IToolboxDynamicProvider)args.ExtensionObject);
					toolboxDynamicProvider.ItemsChanged += OnProviderItemsChanged;
				}
				if (args.ExtensionObject is IToolboxDefaultProvider provider)
				{
					RegisterDefaultToolboxProvider(provider);
				}
			}
			else if (args.Change == ExtensionChange.Remove && args.ExtensionObject is IToolboxDynamicProvider toolboxDynamicProvider2)
			{
				toolboxDynamicProvider2.ItemsChanged -= OnProviderItemsChanged;
				dynamicProviders.Remove(toolboxDynamicProvider2);
			}
			OnToolboxContentsChanged();
		}

		private void OnProviderItemsChanged(object s, EventArgs args)
		{
			OnToolboxContentsChanged();
		}

		public void AddUserItems()
		{
			ComponentSelectorDialog componentSelectorDialog = new ComponentSelectorDialog(currentConsumer);
			componentSelectorDialog.Fill();
			MessageService.ShowCustomDialog(componentSelectorDialog);
		}

		private void AddUserItems(IList<ItemToolboxNode> nodes)
		{
			foreach (ItemToolboxNode node in nodes)
			{
				bool flag = false;
				foreach (ItemToolboxNode item in Configuration.ItemList)
				{
					if (node.Equals(item))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					Configuration.ItemList.Add(node);
				}
			}
		}

		public void RemoveUserItem(ItemToolboxNode node)
		{
			Configuration.ItemList.Remove(node);
			SaveConfiguration();
			OnToolboxContentsChanged();
		}

		public void RegisterDefaultToolboxProvider(IToolboxDefaultProvider provider)
		{
			string fullName = provider.GetType().FullName;
			if (Configuration.LoadedDefaultProviders.Contains(fullName))
			{
				return;
			}
			Configuration.LoadedDefaultProviders.Add(fullName);
			initializing++;
			OnToolboxContentsChanged();
			ThreadPool.QueueUserWorkItem(delegate
			{
				List<ItemToolboxNode> nodes = new List<ItemToolboxNode>();
				try
				{
					IEnumerable<ItemToolboxNode> defaultItems = provider.GetDefaultItems();
					if (defaultItems != null)
					{
						nodes.AddRange(defaultItems);
					}
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error getting default items from a IToolboxDefaultProvider", ex);
				}
				LoaderContext loaderContext = null;
				try
				{
					IEnumerable<string> defaultFiles = provider.GetDefaultFiles();
					if (defaultFiles != null)
					{
						loaderContext = new LoaderContext();
						foreach (string item in defaultFiles)
						{
							nodes.AddRange(GetFileItems(loaderContext, item));
						}
					}
				}
				finally
				{
					loaderContext?.Dispose();
				}
				DispatchService.GuiDispatch(delegate
				{
					AddUserItems(nodes);
					initializing--;
					SaveConfiguration();
					OnToolboxContentsChanged();
				});
			});
		}

		public void ResetToolboxContents()
		{
			config = new ToolboxConfiguration();
			object[] extensionObjects = AddinManager.GetExtensionObjects(toolboxProviderPath);
			foreach (object obj in extensionObjects)
			{
				if (obj is IToolboxDefaultProvider provider)
				{
					RegisterDefaultToolboxProvider(provider);
				}
			}
		}

		internal void Customize(IPadWindow padWindow, IToolboxConfiguration config)
		{
			if (customizer != null)
			{
				customizer.Customize(padWindow, config);
			}
		}

		internal IList<ItemToolboxNode> GetFileItems(LoaderContext ctx, string fileName)
		{
			List<ItemToolboxNode> list = new List<ItemToolboxNode>();
			foreach (IToolboxLoader loader in loaders)
			{
				bool flag = false;
				string[] fileTypes = loader.FileTypes;
				foreach (string value in fileTypes)
				{
					if (fileName.EndsWith(value))
					{
						flag = true;
						break;
					}
				}
				if (flag && fileName.EndsWith(loader.FileTypes[0]))
				{
					try
					{
						IList<ItemToolboxNode> collection = loader.Load(ctx, fileName);
						list.AddRange(collection);
					}
					catch (Exception ex)
					{
						LoggingService.LogError(ex.ToString());
					}
				}
			}
			return list;
		}

		~ToolboxService()
		{
			AddinManager.RemoveExtensionNodeHandler(toolboxLoaderPath, OnLoaderExtensionChanged);
			AddinManager.RemoveExtensionNodeHandler(toolboxProviderPath, OnLoaderExtensionChanged);
		}

		private void SaveConfiguration()
		{
			if (config != null)
			{
				try
				{
					config.SaveContents(ToolboxConfigFile);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error saving toolbox configuration.", ex);
				}
			}
		}

		public IList<ItemToolboxNode> GetCurrentToolboxItems()
		{
			return GetToolboxItems(CurrentConsumer);
		}

		public TargetEntry[] GetCurrentDragTargetTable()
		{
			if (CurrentConsumer != null)
			{
				return CurrentConsumer.DragTargets;
			}
			return new TargetEntry[0];
		}

		public IList<ItemToolboxNode> GetToolboxItems(IToolboxConsumer consumer)
		{
			List<ItemToolboxNode> list = new List<ItemToolboxNode>();
			Hashtable hashtable = new Hashtable();
			if (consumer != null)
			{
				foreach (ItemToolboxNode item in Configuration.ItemList)
				{
					if (!(item is UnknownToolboxNode) && !hashtable.Contains(item) && IsSupported(item, consumer))
					{
						list.Add(item);
						hashtable.Add(item, item);
					}
				}
				foreach (IToolboxDynamicProvider dynamicProvider in dynamicProviders)
				{
					IEnumerable<ItemToolboxNode> dynamicItems = dynamicProvider.GetDynamicItems(consumer);
					if (dynamicItems == null)
					{
						continue;
					}
					foreach (ItemToolboxNode item2 in dynamicItems)
					{
						if (!hashtable.Contains(item2))
						{
							list.Add(item2);
							hashtable.Add(item2, item2);
						}
					}
				}
			}
			return list;
		}

		internal void UpdateUserItems(IEnumerable<ItemToolboxNode> newItems)
		{
			Configuration.ItemList.Clear();
			Configuration.ItemList.AddRange(newItems);
			SaveConfiguration();
			OnToolboxContentsChanged();
		}

		public void SelectItem(ItemToolboxNode item)
		{
			selectedItem = item;
			OnToolboxSelectionChanged(item);
		}

		public void UseSelectedItem()
		{
			if (CurrentConsumer != null && selectedItem != null)
			{
				CurrentConsumer.ConsumeItem(selectedItem);
				OnToolboxUsed(CurrentConsumer, selectedItem);
			}
		}

		public void DragSelectedItem(Widget source, DragContext ctx)
		{
			if (CurrentConsumer == null || selectedItem == null)
			{
				return;
			}
			try
			{
				CurrentConsumer.DragItem(selectedItem, source, ctx);
				OnToolboxUsed(CurrentConsumer, selectedItem);
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error dragging toolbox item.", ex);
			}
		}

		private void onActiveDocChanged(object o, EventArgs e)
		{
			if (oldActiveDoc != null)
			{
				oldActiveDoc.ViewChanged -= OnViewChanged;
			}
			if (oldProject != null)
			{
				oldProject.Modified -= onProjectConfigChanged;
			}
			oldActiveDoc = IdeApp.Workbench.ActiveDocument;
			oldProject = ((oldActiveDoc != null) ? oldActiveDoc.Project : null);
			if (oldActiveDoc != null)
			{
				oldActiveDoc.ViewChanged += OnViewChanged;
			}
			if (oldProject != null)
			{
				oldProject.Modified += onProjectConfigChanged;
			}
			OnViewChanged(null, null);
		}

		private void OnViewChanged(object sender, EventArgs args)
		{
			if (viewProvider != null)
			{
				dynamicProviders.Remove(viewProvider);
				viewProvider.ItemsChanged -= OnProviderItemsChanged;
			}
			if (IdeApp.Workbench.ActiveDocument != null && IdeApp.Workbench.ActiveDocument.ActiveView != null)
			{
				CurrentConsumer = IdeApp.Workbench.ActiveDocument.ActiveView.GetContent<IToolboxConsumer>();
				viewProvider = IdeApp.Workbench.ActiveDocument.ActiveView.GetContent<IToolboxDynamicProvider>();
				customizer = IdeApp.Workbench.ActiveDocument.ActiveView.GetContent<IToolboxCustomizer>();
				if (viewProvider != null)
				{
					dynamicProviders.Add(viewProvider);
					viewProvider.ItemsChanged += OnProviderItemsChanged;
					OnToolboxContentsChanged();
				}
			}
			else
			{
				CurrentConsumer = null;
				viewProvider = null;
				customizer = null;
			}
		}

		private void onProjectConfigChanged(object sender, EventArgs args)
		{
			if (!configChanged)
			{
				configChanged = true;
				GLib.Timeout.Add(500u, delegate
				{
					configChanged = false;
					OnToolboxContentsChanged();
					return false;
				});
			}
		}

		protected virtual void OnToolboxContentsChanged()
		{
			if (ToolboxContentsChanged != null)
			{
				ToolboxContentsChanged(this, new EventArgs());
			}
		}

		protected virtual void OnToolboxConsumerChanged(IToolboxConsumer consumer)
		{
			if (ToolboxConsumerChanged != null)
			{
				ToolboxConsumerChanged(this, new ToolboxConsumerChangedEventArgs(consumer));
			}
		}

		protected virtual void OnToolboxSelectionChanged(ItemToolboxNode item)
		{
			if (ToolboxSelectionChanged != null)
			{
				ToolboxSelectionChanged(this, new ToolboxSelectionChangedEventArgs(item));
			}
		}

		protected virtual void OnToolboxUsed(IToolboxConsumer consumer, ItemToolboxNode item)
		{
			if (ToolboxUsed != null)
			{
				ToolboxUsed(this, new ToolboxUsedEventArgs(consumer, item));
			}
		}

		public bool IsSupported(ItemToolboxNode node, IToolboxConsumer consumer)
		{
			if (consumer is ICustomFilteringToolboxConsumer)
			{
				return ((ICustomFilteringToolboxConsumer)consumer).SupportsItem(node);
			}
			if (node.ItemFilters == null || node.ItemFilters.Count == 0)
			{
				return true;
			}
			ToolboxItemFilterAttribute[] toolboxFilterAttributes = consumer.ToolboxFilterAttributes;
			foreach (ToolboxItemFilterAttribute desFa in toolboxFilterAttributes)
			{
				if (!FilterPermitted(node, desFa, node.ItemFilters, consumer))
				{
					return false;
				}
			}
			foreach (ToolboxItemFilterAttribute itemFilter in node.ItemFilters)
			{
				if (!FilterPermitted(node, itemFilter, consumer.ToolboxFilterAttributes, consumer))
				{
					return false;
				}
			}
			return true;
		}

		private bool FilterPermitted(ItemToolboxNode node, ToolboxItemFilterAttribute desFa, ICollection<ToolboxItemFilterAttribute> filterAgainst, IToolboxConsumer consumer)
		{
			switch (desFa.FilterType)
			{
			case ToolboxItemFilterType.Allow:
				return true;
			case ToolboxItemFilterType.Custom:
				return consumer.CustomFilterSupports(node);
			case ToolboxItemFilterType.Prevent:
				foreach (ToolboxItemFilterAttribute item in filterAgainst)
				{
					if (desFa.Match(item))
					{
						return false;
					}
				}
				return true;
			case ToolboxItemFilterType.Require:
				foreach (ToolboxItemFilterAttribute item2 in filterAgainst)
				{
					if (desFa.Match(item2) && desFa.FilterType != ToolboxItemFilterType.Prevent)
					{
						return true;
					}
				}
				return false;
			default:
				throw new InvalidOperationException("Unexpected ToolboxItemFilterType value.");
			}
		}

		internal ComponentIndex GetComponentIndex(IProgressMonitor monitor)
		{
			ComponentIndex componentIndex = ComponentIndex.Load();
			HashSet<string> hashSet = new HashSet<string>();
			List<ComponentIndexFile> list = new List<ComponentIndexFile>();
			List<ComponentIndexFile> list2 = new List<ComponentIndexFile>();
			foreach (ComponentIndexFile file in componentIndex.Files)
			{
				hashSet.Add(file.FileName);
				if (!File.Exists(file.FileName))
				{
					list2.Add(file);
				}
				if (file.NeedsUpdate)
				{
					list.Add(file);
				}
				if (monitor.IsCancelRequested)
				{
					return componentIndex;
				}
			}
			foreach (TargetRuntime targetRuntime in Runtime.SystemAssemblyService.GetTargetRuntimes())
			{
				foreach (SystemAssembly assembly in targetRuntime.AssemblyContext.GetAssemblies())
				{
					if (hashSet.Add(assembly.Location))
					{
						ComponentIndexFile item = new ComponentIndexFile(assembly.Location);
						componentIndex.Files.Add(item);
						list.Add(item);
					}
					if (monitor.IsCancelRequested)
					{
						return componentIndex;
					}
				}
			}
			foreach (ComponentIndexFile item2 in list2)
			{
				componentIndex.Files.Remove(item2);
			}
			if (list.Count > 0)
			{
				monitor.BeginTask(GettextCatalog.GetString("Looking for components..."), list.Count);
				LoaderContext loaderContext = new LoaderContext();
				try
				{
					foreach (ComponentIndexFile item3 in list)
					{
						item3.Update(loaderContext);
						monitor.Step(1);
						if (monitor.IsCancelRequested)
						{
							return componentIndex;
						}
					}
				}
				finally
				{
					loaderContext.Dispose();
					monitor.EndTask();
				}
			}
			if (list.Count > 0 || list2.Count > 0)
			{
				componentIndex.Save();
			}
			return componentIndex;
		}
	}
}
