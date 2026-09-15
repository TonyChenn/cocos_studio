using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Core;
using Gdk;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	public class PropertyGridUC : EventBox, IPropertyGrid, IService
	{
		public IReadOnlyList<object> SelectedObjects
		{
			get
			{
				return this._selectedObjects;
			}
			set
			{
				if (value != null && this._selectedObjects != null && this._selectedObjects.Count != 0)
				{
					if (this._selectedObjects.Count == value.Count && this._selectedObjects.Except(value).Count<object>() == 0)
					{
						this._selectedObjects = value;
						return;
					}
				}
				this.UnregisterPropertyChangeEvent(this._selectedObjects);
				this._selectedObjects = value;
				if (this._selectedObjects == null)
				{
					this._selectedObjects = new List<object>();
				}
				this.ShowSelectedObjects(this._selectedObjects);
				this.RegisterPropertyChangeEvent(this._selectedObjects);
			}
		}

		public bool IsShowTitle { get; set; }

		public PropertyGridUC()
		{
			base.WidthRequest = 360;
			this.IsShowTitle = true;
			base.WidgetFlags |= WidgetFlags.AppPaintable;
			base.Events |= EventMask.PointerMotionMask;
			Services.RegisterService<IPropertyGrid>(this);
			this.normalMainWidget = this.InitNormalWidget();
			this.plistMainWidget = this.InitPlistWidget();
		}

		private Widget InitNormalWidget()
		{
			VBox vbox = new VBox();
			vbox.Show();
			this.titleWidget = new TitleWidget();
			vbox.PackStart(this.titleWidget, false, false, 0U);
			this.titleWidget.Show();
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.HScrollbar.Visible = true;
			scrolledWindow.VScrollbar.Visible = true;
			vbox.PackStart(scrolledWindow);
			scrolledWindow.Show();
			EventBox eventBox = new EventBox();
			scrolledWindow.AddWithViewport(eventBox);
			eventBox.Show();
			this.propGroupVbox = new VBox();
			eventBox.Add(this.propGroupVbox);
			this.propGroupVbox.Show();
			return vbox;
		}

		private Widget InitPlistWidget()
		{
			this.plistPropVbox = new VBox();
			this.plistPropVbox.Spacing = (int)PropertyPadStyle.mainRowSpacing;
			this.plistPropVbox.Show();
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment.TopPadding = PropertyPadStyle.rightPadding;
			alignment.RightPadding = PropertyPadStyle.rightPadding;
			alignment.Add(this.plistPropVbox);
			alignment.Show();
			ScrolledWindow scrolledWindow = new ScrolledWindow();
			scrolledWindow.HScrollbar.Visible = true;
			scrolledWindow.VScrollbar.Visible = true;
			scrolledWindow.AddWithViewport(alignment);
			scrolledWindow.Show();
			return scrolledWindow;
		}

		public List<IPropertyEditor> GetEditors()
		{
			return this.propertyEditors.Values.ToList<IPropertyEditor>();
		}

		public IPropertyEditor GetEditor(string propertyName)
		{
			IPropertyEditor result;
			if (string.IsNullOrEmpty(propertyName))
			{
				result = null;
			}
			else if (this.propertyEditors.ContainsKey(propertyName))
			{
				result = this.propertyEditors[propertyName];
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void ForceRefresh()
		{
			foreach (IPropertyEditor propertyEditor in this.propertyEditors.Values)
			{
				BaseEditor baseEditor = (BaseEditor)propertyEditor;
				baseEditor.ForceRefresh();
			}
		}

		private void ShowSelectedObjects(IReadOnlyList<object> selectObjs)
		{
			this.Reset(selectObjs);
			if (selectObjs != null && selectObjs.Count != 0)
			{
				this.propertyEditors = PropertyManager.Instance.GetEditorList(selectObjs);
				if (this.propertyEditors.Count != 0)
				{
					List<IPropertyEditor> list = this.propertyEditors.Values.ToList<IPropertyEditor>();
					list.Sort();
					foreach (IEditorController editorController in PropertyManager.Instance.CurrentControllers)
					{
						editorController.RefreshEditor(selectObjs, string.Empty);
					}
					if (this.IsShowTitle)
					{
						IPropertyEditor propertyEditor = this.propertyEditors["Name"];
						Widget widget;
						if (selectObjs.Count == 1)
						{
							widget = propertyEditor.EditorWidget;
						}
						else
						{
							widget = new Entry();
							widget.Sensitive = false;
						}
						this.titleWidget.ShowTitle(selectObjs, widget);
						foreach (IPropertyEditor propertyEditor2 in list)
						{
							if (!propertyEditor2.PropertyItem.Name.Equals("Name"))
							{
								PropertyGroup group2 = this.GetGroup(propertyEditor2.Group);
								group2.Add(propertyEditor2);
							}
						}
						using (Dictionary<string, PropertyGroup>.ValueCollection.Enumerator enumerator3 = this.propertyGroups.Values.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								PropertyGroup group = enumerator3.Current;
								group.RefreshView();
								group.ExpanderWidget.Expanded = (this.shrinkGroupList.FirstOrDefault((string w) => w == group.DisplayName) == null);
							}
						}
						this.ChangeMainWidget(this.normalMainWidget);
						this.propGroupVbox.QueueResize();
					}
					else
					{
						foreach (IPropertyEditor propertyEditor2 in list)
						{
							this.plistPropVbox.PackStart(propertyEditor2.EditorWidget, false, false, 0U);
						}
						this.ChangeMainWidget(this.plistMainWidget);
					}
				}
			}
		}

		private void Reset(IReadOnlyList<object> selectObjs)
		{
			if (selectObjs.Count == 0)
			{
				this.ChangeMainWidget(null);
			}
			this.titleWidget.Clear();
			this.plistPropVbox.RemoveAll();
			foreach (PropertyGroup propertyGroup in this.propertyGroups.Values)
			{
				propertyGroup.Clear();
			}
			foreach (IPropertyEditor propertyEditor in this.propertyEditors.Values)
			{
				if (!propertyEditor.CanCaching)
				{
					propertyEditor.EditorWidget.Destroy();
				}
			}
			this.propertyEditors.Clear();
		}

		private PropertyGroup GetGroup(string groupName)
		{
			PropertyGroup propertyGroup;
			PropertyGroup result;
			if (this.propertyGroups.TryGetValue(groupName, out propertyGroup))
			{
				result = propertyGroup;
			}
			else
			{
				propertyGroup = new PropertyGroup(groupName);
				propertyGroup.ExpanderWidget.ExpandChanged += this.GroupExpandChangedHandler;
				this.propertyGroups.Add(groupName, propertyGroup);
				this.propGroupVbox.PackStart(propertyGroup.ExpanderWidget, false, false, 0U);
				List<PropertyGroup> list = this.propertyGroups.Values.ToList<PropertyGroup>();
				list.Sort();
				for (int i = 0; i < list.Count; i++)
				{
					Box.BoxChild boxChild = this.propGroupVbox[list[i].ExpanderWidget] as Box.BoxChild;
					boxChild.Position = i;
				}
				result = propertyGroup;
			}
			return result;
		}

		private void ChangeMainWidget(Widget newMainWidget)
		{
			Widget child = base.Child;
			if (child != newMainWidget)
			{
				if (child != null)
				{
					base.Remove(child);
				}
				if (newMainWidget != null)
				{
					base.Add(newMainWidget);
					newMainWidget.Show();
				}
			}
		}

		private void RegisterPropertyChangeEvent(IReadOnlyList<object> objs)
		{
			INotifyPropertyChanged notifyPropertyChanged;
			INotifyPropertyChanged notifyPropertyChanged2;
			this.GetKeyObjects(objs, out notifyPropertyChanged, out notifyPropertyChanged2);
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged += this.PropertyChangedHandler;
			}
			if (notifyPropertyChanged2 != null)
			{
				notifyPropertyChanged2.PropertyChanged += this.PropertyChangedHandler;
			}
		}

		private void UnregisterPropertyChangeEvent(IReadOnlyList<object> objs)
		{
			INotifyPropertyChanged notifyPropertyChanged;
			INotifyPropertyChanged notifyPropertyChanged2;
			this.GetKeyObjects(objs, out notifyPropertyChanged, out notifyPropertyChanged2);
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.PropertyChangedHandler;
			}
			if (notifyPropertyChanged2 != null)
			{
				notifyPropertyChanged2.PropertyChanged -= this.PropertyChangedHandler;
			}
		}

		private void GetKeyObjects(IReadOnlyList<object> selectedObjs, out INotifyPropertyChanged obj1, out INotifyPropertyChanged obj2)
		{
			obj1 = null;
			obj2 = null;
			if (selectedObjs != null && selectedObjs.Count != 0)
			{
				if (selectedObjs.Count == 1)
				{
					obj1 = (selectedObjs[0] as INotifyPropertyChanged);
				}
				else
				{
					obj1 = (selectedObjs[0] as INotifyPropertyChanged);
					obj2 = (selectedObjs[selectedObjs.Count - 1] as INotifyPropertyChanged);
				}
			}
		}

		private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			foreach (IPropertyEditor propertyEditor in this.propertyEditors.Values)
			{
				propertyEditor.HandlePropertyChanged(e);
			}
			foreach (IEditorController editorController in PropertyManager.Instance.CurrentControllers)
			{
				if (editorController.CorrespondProperties.Contains(e.PropertyName))
				{
					editorController.RefreshEditor(this.SelectedObjects, e.PropertyName);
				}
			}
			foreach (PropertyGroup propertyGroup in this.propertyGroups.Values)
			{
				propertyGroup.RefreshView();
			}
		}

		private void GroupExpandChangedHandler(object sender, ExpandEvent e)
		{
			if (e.Expand)
			{
				this.shrinkGroupList.RemoveAll((string w) => w == e.Name);
			}
			else
			{
				this.shrinkGroupList.Add(e.Name);
			}
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			base.CanFocus = true;
			base.HasFocus = true;
			base.HasFocus = false;
			base.CanFocus = false;
			return base.OnButtonPressEvent(evnt);
		}

		private Widget normalMainWidget;

		private TitleWidget titleWidget;

		private VBox propGroupVbox;

		private Widget plistMainWidget;

		private VBox plistPropVbox;

		private Dictionary<string, IPropertyEditor> propertyEditors = new Dictionary<string, IPropertyEditor>();

		private Dictionary<string, PropertyGroup> propertyGroups = new Dictionary<string, PropertyGroup>();

		private List<string> shrinkGroupList = new List<string>();

		private IReadOnlyList<object> _selectedObjects;
	}
}
