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
	// Token: 0x02000009 RID: 9
	public abstract class BaseEditor : IPropertyEditor, IComparable<IPropertyEditor>
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000024 RID: 36 RVA: 0x000021B0 File Offset: 0x000003B0
		// (set) Token: 0x06000025 RID: 37 RVA: 0x000021C7 File Offset: 0x000003C7
		public string DisplayName { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000021D0 File Offset: 0x000003D0
		// (set) Token: 0x06000027 RID: 39 RVA: 0x000021E7 File Offset: 0x000003E7
		public string Group { get; private set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000021F0 File Offset: 0x000003F0
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002207 File Offset: 0x00000407
		public int Order { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002210 File Offset: 0x00000410
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00002227 File Offset: 0x00000427
		public PropertyItem PropertyItem { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002230 File Offset: 0x00000430
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00002247 File Offset: 0x00000447
		public Widget EditorWidget { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002250 File Offset: 0x00000450
		public virtual bool IsShowLabel
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002264 File Offset: 0x00000464
		public virtual bool IsMultiLine
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002278 File Offset: 0x00000478
		public virtual bool SupportMultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000228C File Offset: 0x0000048C
		public virtual bool CanCaching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000032 RID: 50 RVA: 0x000022A0 File Offset: 0x000004A0
		// (set) Token: 0x06000033 RID: 51 RVA: 0x000022B8 File Offset: 0x000004B8
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

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000022EC File Offset: 0x000004EC
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002304 File Offset: 0x00000504
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

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0000231C File Offset: 0x0000051C
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0000233E File Offset: 0x0000053E
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002354 File Offset: 0x00000554
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002376 File Offset: 0x00000576
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

		// Token: 0x0600003A RID: 58 RVA: 0x0000238B File Offset: 0x0000058B
		public BaseEditor()
		{
			this.DisplayName = "Default Property";
			this.Group = "Group_Custom";
			this.Order = -10;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000023C0 File Offset: 0x000005C0
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

		// Token: 0x0600003C RID: 60 RVA: 0x00002500 File Offset: 0x00000700
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

		// Token: 0x0600003D RID: 61 RVA: 0x000025B7 File Offset: 0x000007B7
		protected virtual void OnReset()
		{
			this.SetControl();
		}

		// Token: 0x0600003E RID: 62
		protected abstract Widget OnCreateWidget();

		// Token: 0x0600003F RID: 63 RVA: 0x000025C4 File Offset: 0x000007C4
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

		// Token: 0x06000040 RID: 64 RVA: 0x0000263A File Offset: 0x0000083A
		internal void ForceRefresh()
		{
			this.SetControl();
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002644 File Offset: 0x00000844
		public virtual void HandlePropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == this.PropertyItem.Name)
			{
				this.SetControl();
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002678 File Offset: 0x00000878
		protected void SetControl()
		{
			if (!this.IsSettingValue)
			{
				this.IsSettingControl = true;
				this.OnSetControl();
				this.IsSettingControl = false;
			}
		}

		// Token: 0x06000043 RID: 67
		protected abstract void OnSetControl();

		// Token: 0x06000044 RID: 68 RVA: 0x000026AC File Offset: 0x000008AC
		protected virtual void OnSetSensitive(bool isSensitive)
		{
			if (this.innerWidget != null)
			{
				this.innerWidget.Sensitive = this._enable;
			}
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000026DC File Offset: 0x000008DC
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

		// Token: 0x06000046 RID: 70 RVA: 0x00002768 File Offset: 0x00000968
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

		// Token: 0x06000047 RID: 71 RVA: 0x00002840 File Offset: 0x00000A40
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

		// Token: 0x06000048 RID: 72 RVA: 0x000028DC File Offset: 0x00000ADC
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

		// Token: 0x06000049 RID: 73 RVA: 0x00002990 File Offset: 0x00000B90
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

		// Token: 0x0600004A RID: 74 RVA: 0x000029F2 File Offset: 0x00000BF2
		protected void ReportUserData(string featureName)
		{
			Tracker.Add(ViewRegions.PropertyUC, featureName, "", "");
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A08 File Offset: 0x00000C08
		public IDisposable GetLock(bool isGroupTask = true)
		{
			if (this.propertyChangeLock == null)
			{
				this.propertyChangeLock = new BaseEditor.PropertyChangeLock(this);
			}
			return this.propertyChangeLock.Lock(isGroupTask);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002A44 File Offset: 0x00000C44
		public int CompareTo(IPropertyEditor other)
		{
			return this.Order.CompareTo(other.Order);
		}

		// Token: 0x04000008 RID: 8
		protected Widget innerWidget;

		// Token: 0x04000009 RID: 9
		private bool _visible;

		// Token: 0x0400000A RID: 10
		private bool _enable;

		// Token: 0x0400000B RID: 11
		private BaseEditor.PropertyChangeLock propertyChangeLock = null;

		// Token: 0x0200000A RID: 10
		private class PropertyChangeLock : IDisposable
		{
			// Token: 0x0600004D RID: 77 RVA: 0x00002A6A File Offset: 0x00000C6A
			public PropertyChangeLock(BaseEditor editor)
			{
				this.editor = editor;
			}

			// Token: 0x0600004E RID: 78 RVA: 0x00002A7C File Offset: 0x00000C7C
			public void Dispose()
			{
				if (this.taskLock != null)
				{
					this.taskLock.Dispose();
				}
				this.editor.IsSettingValue = false;
			}

			// Token: 0x0600004F RID: 79 RVA: 0x00002AB0 File Offset: 0x00000CB0
			internal IDisposable Lock(bool isGroupTask)
			{
				this.editor.IsSettingValue = true;
				if (isGroupTask)
				{
					this.taskLock = CompositeTask.Run("SetValue", null);
				}
				return this;
			}

			// Token: 0x04000011 RID: 17
			private BaseEditor editor;

			// Token: 0x04000012 RID: 18
			private CompositeTask taskLock;
		}
	}
}
