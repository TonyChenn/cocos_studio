using System;
using System.ComponentModel;
using System.Reflection;
using CocoStudio.UndoManager;
using CocoStudio.UserStatistics;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Modules.Communal.PropertyGrid
{
	public abstract class BaseEditor : IPropertyEditor, IComparable<IPropertyEditor>
	{
		public string DisplayName { get; private set; }

		public string Group { get; private set; }

		public int Order { get; private set; }

		public PropertyItem PropertyItem { get; private set; }

		public Widget EditorWidget { get; private set; }

		public virtual bool IsShowLabel
		{
			get
			{
				return true;
			}
		}

		public virtual bool IsMultiLine
		{
			get
			{
				return false;
			}
		}

		public virtual bool SupportMultiSelect
		{
			get
			{
				return false;
			}
		}

		public virtual bool CanCaching
		{
			get
			{
				return false;
			}
		}

		public bool Visible
		{
			get
			{
				return this._visible;
			}
			set
			{
				this._visible = value;
				if (this.EditorWidget != null)
				{
					this.EditorWidget.Visible = this._visible;
				}
			}
		}

		public bool Enable
		{
			get
			{
				return this._enable;
			}
			set
			{
				this._enable = value;
				this.OnSetSensitive(this._enable);
			}
		}

		public bool IsSettingControl
		{
			get
			{
				return this.PropertyItem.Values.IsSettingControl;
			}
			private set
			{
				this.PropertyItem.Values.IsSettingControl = value;
			}
		}

		public bool IsSettingValue
		{
			get
			{
				return this.PropertyItem.Values.IsSettingValue;
			}
			private set
			{
				this.PropertyItem.Values.IsSettingValue = value;
			}
		}

		public BaseEditor()
		{
			this.DisplayName = "Default Property";
			this.Group = "Group_Custom";
			this.Order = -10;
		}

		public void Initialize(PropertyItem propItem)
		{
			if (propItem != null)
			{
				this.PropertyItem = propItem;
				DisplayNameAttribute displayNameAttribute = propItem.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
				if (displayNameAttribute != null)
				{
					this.DisplayName = LanguageOption.GetValueBykey(displayNameAttribute.DisplayName);
				}
				PropertyOrderAttribute propertyOrderAttribute = propItem.Attributes[typeof(PropertyOrderAttribute)] as PropertyOrderAttribute;
				if (propertyOrderAttribute != null)
				{
					this.Order = propertyOrderAttribute.Order;
				}
				CategoryAttribute categoryAttribute = propItem.Attributes[typeof(CategoryAttribute)] as CategoryAttribute;
				if (categoryAttribute != null)
				{
					this.Group = categoryAttribute.Category;
				}
				this.innerWidget = this.OnCreateWidget();
				if (this.IsShowLabel)
				{
					HBox hbox = new HBox();
					hbox.Spacing = (int)PropertyPadStyle.mainColumnSpacing;
					Widget widget = this.CreatePropertyLabel();
					hbox.PackStart(widget, false, false, 0U);
					hbox.PackStart(this.innerWidget);
					widget.Show();
					this.innerWidget.Show();
					this.EditorWidget = hbox;
				}
				else
				{
					this.EditorWidget = this.innerWidget;
				}
			}
		}

		public void Reset(PropertyItem propItem)
		{
			this.PropertyItem = propItem;
			PropertyOrderAttribute propertyOrderAttribute = propItem.Attributes[typeof(PropertyOrderAttribute)] as PropertyOrderAttribute;
			if (propertyOrderAttribute != null)
			{
				this.Order = propertyOrderAttribute.Order;
			}
			CategoryAttribute categoryAttribute = propItem.Attributes[typeof(CategoryAttribute)] as CategoryAttribute;
			if (categoryAttribute != null)
			{
				this.Group = categoryAttribute.Category;
			}
			if (this.EditorWidget.Parent != null)
			{
				Gtk.Container container = this.EditorWidget.Parent as Gtk.Container;
				container.Remove(this.EditorWidget);
			}
			this.Visible = true;
			this.OnReset();
		}

		protected virtual void OnReset()
		{
			this.SetControl();
		}

		protected abstract Widget OnCreateWidget();

		private Widget CreatePropertyLabel()
		{
			Label label = new Label(this.DisplayName);
			label.WidthRequest = PropertyPadStyle.propertyLabelWidth;
			label.Xalign = 1f;
			label.Show();
			Widget result;
			if (this.IsMultiLine)
			{
				VBox vbox = new VBox();
				uint padding = 4U;
				if (Platform.IsMac)
				{
					padding = 6U;
				}
				vbox.PackStart(label, false, false, padding);
				result = vbox;
			}
			else
			{
				result = label;
			}
			return result;
		}

		internal void ForceRefresh()
		{
			this.SetControl();
		}

		public virtual void HandlePropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.PropertyItem.Name)
			{
				this.SetControl();
			}
		}

		protected void SetControl()
		{
			if (!this.IsSettingValue)
			{
				this.IsSettingControl = true;
				this.OnSetControl();
				this.IsSettingControl = false;
			}
		}

		protected abstract void OnSetControl();

		protected virtual void OnSetSensitive(bool isSensitive)
		{
			if (this.innerWidget != null)
			{
				this.innerWidget.Sensitive = this._enable;
			}
		}

		protected void UpdatePropertyValue(object value, PropertyInfo propInfo = null)
		{
			if (!this.IsSettingControl)
			{
				this.IsSettingValue = true;
				if (PropertyItem.Objects.Count > 1)
				{
					using (CompositeTask.Run("SetValue", null))
					{
						this.SetValues(value, propInfo);
					}
				}
				else
				{
					this.SetValues(value, propInfo);
				}
				this.IsSettingValue = false;
			}
		}

		private void SetValues(object value, PropertyInfo propInfo)
		{
			if (propInfo == null)
			{
				foreach (object obj in PropertyItem.Objects)
				{
					this.PropertyItem.Values[obj] = value;
				}
			}
			else
			{
				foreach (object obj in PropertyItem.Objects)
				{
					PropertyInfo property = obj.GetType().GetProperty(propInfo.Name);
					property.SetValue(obj, value, null);
				}
			}
		}

		protected bool CheckIsSameValue()
		{
			bool result;
			if (PropertyItem.Objects.Count == 1)
			{
				result = true;
			}
			else
			{
				object firstValue = this.PropertyItem.FirstValue;
				for (int i = 1; i < PropertyItem.Objects.Count; i++)
				{
					object obj = this.PropertyItem.Values[i];
					if (firstValue == null)
					{
						if (obj != null)
						{
							return false;
						}
					}
					else if (!firstValue.Equals(obj))
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		protected bool IsWhip<T>(Func<T, T, bool> Func = null)
		{
			bool result;
			if (PropertyItem.Objects.Count == 1)
			{
				result = false;
			}
			else
			{
				T arg = (T)((object)this.PropertyItem.FirstValue);
				for (int i = 1; i < PropertyItem.Objects.Count; i++)
				{
					T t = (T)((object)this.PropertyItem.Values[i]);
					if (Func != null)
					{
						if (!Func(arg, t))
						{
							return true;
						}
					}
					else if (!arg.Equals(t))
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		protected bool IsWhipNode<T>(Func<T, T, bool> Func)
		{
			T arg = (T)((object)PropertyItem.FirstObject);
			for (int i = 1; i < PropertyItem.Objects.Count; i++)
			{
				if (Func != null)
				{
					if (!Func(arg, (T)((object)PropertyItem.Objects[i])))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected void ReportUserData(string featureName)
		{
			Tracker.Add(ViewRegions.PropertyUC, featureName, "", "");
		}

		public IDisposable GetLock(bool isGroupTask = true)
		{
			if (this.propertyChangeLock == null)
			{
				this.propertyChangeLock = new BaseEditor.PropertyChangeLock(this);
			}
			return this.propertyChangeLock.Lock(isGroupTask);
		}

		public int CompareTo(IPropertyEditor other)
		{
			return this.Order.CompareTo(other.Order);
		}

		protected Widget innerWidget;

		private bool _visible;

		private bool _enable;

		private BaseEditor.PropertyChangeLock propertyChangeLock = null;

		private class PropertyChangeLock : IDisposable
		{
			public PropertyChangeLock(BaseEditor editor)
			{
				this.editor = editor;
			}

			public void Dispose()
			{
				if (this.taskLock != null)
				{
					this.taskLock.Dispose();
				}
				this.editor.IsSettingValue = false;
			}

			internal IDisposable Lock(bool isGroupTask)
			{
				this.editor.IsSettingValue = true;
				if (isGroupTask)
				{
					this.taskLock = CompositeTask.Run("SetValue", null);
				}
				return this;
			}

			private BaseEditor editor;

			private CompositeTask taskLock;
		}
	}
}
