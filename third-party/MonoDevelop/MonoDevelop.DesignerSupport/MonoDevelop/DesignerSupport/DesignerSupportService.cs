using System;
using System.Collections;
using Gtk;
using Mono.Addins;
using MonoDevelop.Ide;

namespace MonoDevelop.DesignerSupport
{
	public class DesignerSupportService
	{
		private PropertyPad propertyPad;

		private ToolboxService toolboxService;

		private IPropertyProvider[] providers;

		private IPropertyPadProvider lastPadProvider;

		private object lastComponent;

		private ICustomPropertyPadProvider lastCustomProvider;

		public PropertyPad PropertyPad => propertyPad;

		public ToolboxService ToolboxService
		{
			get
			{
				if (toolboxService == null)
				{
					toolboxService = new ToolboxService();
				}
				return toolboxService;
			}
		}

		internal void SetPad(PropertyPad pad)
		{
			propertyPad = pad;
			if (propertyPad == null)
			{
				return;
			}
			if (lastPadProvider != null)
			{
				object[] providersForObject = GetProvidersForObject(lastComponent, lastPadProvider.GetProvider());
				if (providersForObject.Length > 0)
				{
					propertyPad.PropertyGrid.SetCurrentObject(lastComponent, providersForObject);
				}
				else
				{
					propertyPad.BlankPad();
				}
				if (lastPadProvider is IPropertyPadCustomizer propertyPadCustomizer)
				{
					propertyPadCustomizer.Customize(pad.PadWindow, pad.PropertyGrid);
				}
				propertyPad.PropertyGrid.Changed += OnPropertyGridChanged;
			}
			else if (lastCustomProvider != null)
			{
				propertyPad.UseCustomWidget(lastCustomProvider.GetCustomPropertyWidget());
				if (lastCustomProvider is IPropertyPadCustomizer propertyPadCustomizer2)
				{
					propertyPadCustomizer2.Customize(pad.PadWindow, null);
				}
			}
		}

		private void DisposePropertyPadProvider()
		{
			if (lastPadProvider != null)
			{
				if (propertyPad != null && propertyPad.PropertyGrid != null)
				{
					propertyPad.PropertyGrid.Changed -= OnPropertyGridChanged;
				}
				lastPadProvider.OnEndEditing(lastComponent);
				lastPadProvider = null;
				lastComponent = null;
			}
		}

		private void DisposeCustomPropertyPadProvider()
		{
			if (lastCustomProvider != null)
			{
				lastCustomProvider.DisposeCustomPropertyWidget();
				lastCustomProvider = null;
			}
		}

		internal void ReSetPad()
		{
			DisposePropertyPadProvider();
			DisposeCustomPropertyPadProvider();
			if (propertyPad != null)
			{
				propertyPad.BlankPad();
			}
		}

		public void SetPadContent(IPropertyPadProvider provider)
		{
			SetPadContent(provider, null);
		}

		public void SetPadContent(IPropertyPadProvider provider, object commandRouteOrigin)
		{
			if (provider != null)
			{
				DisposeCustomPropertyPadProvider();
				object activeComponent = provider.GetActiveComponent();
				if (lastPadProvider != null && activeComponent == lastComponent)
				{
					return;
				}
				DisposePropertyPadProvider();
				lastPadProvider = provider;
				lastComponent = activeComponent;
				if (propertyPad != null)
				{
					object[] providersForObject = GetProvidersForObject(activeComponent, provider.GetProvider());
					if (providersForObject.Length > 0)
					{
						propertyPad.PropertyGrid.SetCurrentObject(activeComponent, providersForObject);
						propertyPad.CommandRouteOrigin = commandRouteOrigin;
					}
					else
					{
						propertyPad.BlankPad();
					}
					if (provider is IPropertyPadCustomizer propertyPadCustomizer)
					{
						propertyPadCustomizer.Customize(propertyPad.PadWindow, propertyPad.PropertyGrid);
					}
					propertyPad.PropertyGrid.Changed += OnPropertyGridChanged;
				}
			}
			else
			{
				ReSetPad();
			}
		}

		public void SetPadContent(ICustomPropertyPadProvider provider)
		{
			SetPadContent(provider, null);
		}

		public void SetPadContent(ICustomPropertyPadProvider provider, Widget commandRouteOrigin)
		{
			if (provider != null)
			{
				if (lastCustomProvider == provider)
				{
					return;
				}
				DisposePropertyPadProvider();
				DisposeCustomPropertyPadProvider();
				lastCustomProvider = provider;
				if (propertyPad != null)
				{
					propertyPad.UseCustomWidget(provider.GetCustomPropertyWidget());
					propertyPad.CommandRouteOrigin = commandRouteOrigin;
					if (provider is IPropertyPadCustomizer propertyPadCustomizer)
					{
						propertyPadCustomizer.Customize(propertyPad.PadWindow, null);
					}
				}
			}
			else
			{
				ReSetPad();
			}
		}

		internal object[] GetProvidersForObject(object obj, object firstProvider)
		{
			if (providers == null)
			{
				providers = (IPropertyProvider[])AddinManager.GetExtensionObjects("/MonoDevelop/DesignerSupport/PropertyProviders", typeof(IPropertyProvider), reuseCachedInstance: true);
			}
			ArrayList arrayList = new ArrayList();
			if (firstProvider != null)
			{
				arrayList.Add(firstProvider);
			}
			IPropertyProvider[] array = providers;
			foreach (IPropertyProvider propertyProvider in array)
			{
				if (propertyProvider.SupportsObject(obj))
				{
					arrayList.Add(propertyProvider.CreateProvider(obj));
				}
			}
			return arrayList.ToArray();
		}

		private void OnPropertyGridChanged(object s, EventArgs a)
		{
			if (lastPadProvider != null)
			{
				lastPadProvider.OnChanged(lastComponent);
			}
		}

		internal DesignerSupportService()
		{
			IdeApp.CommandService.RegisterCommandTargetVisitor(new PropertyPadVisitor());
			AddinManager.ExtensionChanged += OnExtensionChanged;
		}

		private void OnExtensionChanged(object s, ExtensionEventArgs args)
		{
			if (args.PathChanged("MonoDevelop/DesignerSupport/PropertyProviders"))
			{
				providers = null;
				ReSetPad();
			}
		}
	}
}
