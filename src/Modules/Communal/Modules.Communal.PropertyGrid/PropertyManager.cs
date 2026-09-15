using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CocoStudio.Core;
using Mono.Addins;

namespace Modules.Communal.PropertyGrid
{
	public class PropertyManager
	{
		public IPropertyFilter CurrentFilter { get; private set; }

		public IReadOnlyList<IEditorController> CurrentControllers
		{
			get
			{
				return this._currentControllers;
			}
		}

		public static PropertyManager Instance { get; private set; }

		public static void Initialize()
		{
			if (PropertyManager.Instance == null)
			{
				PropertyManager.Instance = new PropertyManager();
			}
		}

		internal PropertyManager()
		{
			this.editorInstances = new Dictionary<Tuple<Type, string>, BaseEditor>();
			this.defaultEditors = new Dictionary<Type, Type>();
			this.defaultEditors.Add(typeof(string), typeof(DefaultEditor));
			this.defaultEditors.Add(typeof(bool), typeof(BoolEditor));
			this.defaultEditors.Add(typeof(Enum), typeof(EnumEditor));
			this.defaultEditors.Add(typeof(int), typeof(IntEditor));
			this.defaultEditors.Add(typeof(float), typeof(SingleEditor));
			this.defaultEditors.Add(typeof(double), typeof(SingleEditor));
			this.defaultEditors.Add(typeof(Color), typeof(ColorEditor));
			this.propertyFilterList = new List<IPropertyFilter>();
			IPropertyFilter[] extensionObjects = AddinManager.GetExtensionObjects<IPropertyFilter>();
			foreach (IPropertyFilter propertyFilter in extensionObjects)
			{
				this.propertyFilterList.Add(propertyFilter);
				if (propertyFilter.CanHandle())
				{
					this.CurrentFilter = propertyFilter;
				}
			}
			this.controllerList = new List<IEditorController>();
			this._currentControllers = new List<IEditorController>();
			IEditorController[] extensionObjects2 = AddinManager.GetExtensionObjects<IEditorController>();
			foreach (IEditorController editorController in extensionObjects2)
			{
				this.controllerList.Add(editorController);
				if (editorController.CanHandle())
				{
					this._currentControllers.Add(editorController);
				}
			}
			Services.Workbench.ActiveDocumentChanged += this.ActiveDocumentChangedHandler;
		}

		internal Dictionary<string, IPropertyEditor> GetEditorList(IReadOnlyList<object> selectObjs)
		{
			Dictionary<string, IPropertyEditor> dictionary = new Dictionary<string, IPropertyEditor>();
			Dictionary<string, IPropertyEditor> result;
			if (selectObjs == null || selectObjs.Count == 0)
			{
				result = dictionary;
			}
			else
			{
				List<Type> list = new List<Type>();
				foreach (object obj in selectObjs)
				{
					Type type = obj.GetType();
					if (!list.Contains(type))
					{
						list.Add(type);
					}
				}
				List<PropertyDescriptor> propertyDescriptors = this.GetPropertyDescriptors(list[0]);
				for (int i = 1; i < list.Count; i++)
				{
					List<PropertyDescriptor> propertyDescriptors2 = this.GetPropertyDescriptors(list[i]);
					for (int j = propertyDescriptors.Count - 1; j >= 0; j--)
					{
						PropertyDescriptor originProp = propertyDescriptors[j];
						PropertyDescriptor propertyDescriptor = propertyDescriptors2.FirstOrDefault((PropertyDescriptor w) => w.Name == originProp.Name);
						if (propertyDescriptor == null)
						{
							propertyDescriptors.RemoveAt(j);
						}
					}
				}
				PropertyItem.Objects = selectObjs;
				foreach (PropertyDescriptor propertyDescriptor2 in propertyDescriptors)
				{
					if (this.CurrentFilter == null || this.CurrentFilter.CanShow(selectObjs, propertyDescriptor2.Name))
					{
						PropertyItem propItem = new PropertyItem(propertyDescriptor2);
						BaseEditor editor = this.GetEditor(list[0], propItem, propertyDescriptor2);
						editor.Visible = (editor.Enable = true);
						if (selectObjs.Count <= 1 || editor.SupportMultiSelect)
						{
							dictionary.Add(propertyDescriptor2.Name, editor);
						}
					}
				}
				result = dictionary;
			}
			return result;
		}

		internal List<PropertyDescriptor> GetPropertyDescriptors(Type objType)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(objType);
			PropertyDescriptorCollection properties;
			if (converter == null || !converter.GetPropertiesSupported())
			{
				properties = TypeDescriptor.GetProperties(objType);
			}
			else
			{
				properties = converter.GetProperties(objType);
			}
			List<PropertyDescriptor> list = new List<PropertyDescriptor>();
			foreach (object obj in properties)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
				if (!string.IsNullOrEmpty(propertyDescriptor.GetGroup()))
				{
					if (propertyDescriptor.GetBrowsable())
					{
						list.Add(propertyDescriptor);
					}
				}
			}
			return list;
		}

		private BaseEditor GetEditor(Type objType, PropertyItem propItem, PropertyDescriptor propDesc)
		{
			object[] args = null;
			ConstructParamsAttribute constructParamsAttribute = propDesc.Attributes[typeof(ConstructParamsAttribute)] as ConstructParamsAttribute;
			if (constructParamsAttribute != null)
			{
				args = constructParamsAttribute.ConstructParams;
			}
			Type editorType = this.GetEditorType(objType, propDesc);
			Tuple<Type, string> key = new Tuple<Type, string>(editorType, propItem.Name);
			BaseEditor result;
			if (this.editorInstances.ContainsKey(key))
			{
				BaseEditor baseEditor = this.editorInstances[key];
				baseEditor.Reset(propItem);
				result = baseEditor;
			}
			else
			{
				BaseEditor baseEditor = Activator.CreateInstance(editorType, args) as BaseEditor;
				if (baseEditor == null)
				{
					baseEditor = new DefaultEditor();
				}
				baseEditor.Initialize(propItem);
				if (baseEditor.CanCaching)
				{
					this.editorInstances[key] = baseEditor;
				}
				result = baseEditor;
			}
			return result;
		}

		private Type GetEditorType(Type objType, PropertyDescriptor propDescriptor)
		{
			Type result;
			if (objType == null || propDescriptor == null)
			{
				result = null;
			}
			else
			{
				Type type = null;
				string colorEditor = "System.Drawing.Design.ColorEditor";
				List<EditorAttribute> source = propDescriptor.Attributes.OfType<EditorAttribute>().ToList<EditorAttribute>();
				EditorAttribute editorAttribute = (from w in source
				where !w.EditorTypeName.StartsWith(colorEditor)
				select w).FirstOrDefault<EditorAttribute>();
				if (editorAttribute != null)
				{
					type = Type.GetType(editorAttribute.EditorTypeName);
				}
				else
				{
					Type propertyType = propDescriptor.PropertyType;
					if (propertyType.IsEnum)
					{
						type = this.defaultEditors[typeof(Enum)];
					}
					else if (this.defaultEditors.ContainsKey(propertyType))
					{
						type = this.defaultEditors[propertyType];
					}
				}
				result = type;
			}
			return result;
		}

		private void ActiveDocumentChangedHandler(object sender, EventArgs e)
		{
			this.CurrentFilter = null;
			foreach (IPropertyFilter propertyFilter in this.propertyFilterList)
			{
				if (propertyFilter.CanHandle())
				{
					this.CurrentFilter = propertyFilter;
					break;
				}
			}
			this._currentControllers.Clear();
			foreach (IEditorController editorController in this.controllerList)
			{
				if (editorController.CanHandle())
				{
					this._currentControllers.Add(editorController);
				}
			}
		}

		private Dictionary<Type, Type> defaultEditors;

		private Dictionary<Tuple<Type, string>, BaseEditor> editorInstances;

		private List<IPropertyFilter> propertyFilterList;

		private List<IEditorController> controllerList;

		private List<IEditorController> _currentControllers;
	}
}
