using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CocoStudio.Core;
using Mono.Addins;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000018 RID: 24
	public class PropertyManager
	{
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600009D RID: 157 RVA: 0x00003980 File Offset: 0x00001B80
		// (set) Token: 0x0600009E RID: 158 RVA: 0x00003997 File Offset: 0x00001B97
		public IPropertyFilter CurrentFilter { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000039A0 File Offset: 0x00001BA0
		public IReadOnlyList<IEditorController> CurrentControllers
		{
			get
			{
				return this._currentControllers;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000039B8 File Offset: 0x00001BB8
		// (set) Token: 0x060000A1 RID: 161 RVA: 0x000039CE File Offset: 0x00001BCE
		public static PropertyManager Instance { get; private set; }

		// Token: 0x060000A2 RID: 162 RVA: 0x000039D8 File Offset: 0x00001BD8
		public static void Initialize()
		{
			if (PropertyManager.Instance == null)
			{
				PropertyManager.Instance = new PropertyManager();
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003A00 File Offset: 0x00001C00
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

		// Token: 0x060000A4 RID: 164 RVA: 0x00003C18 File Offset: 0x00001E18
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

		// Token: 0x060000A5 RID: 165 RVA: 0x00003E40 File Offset: 0x00002040
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

		// Token: 0x060000A6 RID: 166 RVA: 0x00003F10 File Offset: 0x00002110
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

		// Token: 0x060000A7 RID: 167 RVA: 0x0000401C File Offset: 0x0000221C
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

		// Token: 0x060000A8 RID: 168 RVA: 0x00004100 File Offset: 0x00002300
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

		// Token: 0x04000024 RID: 36
		private Dictionary<Type, Type> defaultEditors;

		// Token: 0x04000025 RID: 37
		private Dictionary<Tuple<Type, string>, BaseEditor> editorInstances;

		// Token: 0x04000026 RID: 38
		private List<IPropertyFilter> propertyFilterList;

		// Token: 0x04000027 RID: 39
		private List<IEditorController> controllerList;

		// Token: 0x04000028 RID: 40
		private List<IEditorController> _currentControllers;
	}
}
