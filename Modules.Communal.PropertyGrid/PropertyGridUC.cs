using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Core;
using Gdk;
using Gtk;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200001E RID: 30
	public class PropertyGridUC : EventBox, IPropertyGrid, IService
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004980 File Offset: 0x00002B80
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004998 File Offset: 0x00002B98
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

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004A50 File Offset: 0x00002C50
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004A67 File Offset: 0x00002C67
		public bool IsShowTitle { get; set; }

		// Token: 0x060000CC RID: 204 RVA: 0x00004A70 File Offset: 0x00002C70
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

		// Token: 0x060000CD RID: 205 RVA: 0x00004AFC File Offset: 0x00002CFC
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

		// Token: 0x060000CE RID: 206 RVA: 0x00004BAC File Offset: 0x00002DAC
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

		// Token: 0x060000CF RID: 207 RVA: 0x00004C5C File Offset: 0x00002E5C
		public List<IPropertyEditor> GetEditors()
		{
			return this.propertyEditors.Values.ToList<IPropertyEditor>();
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004C80 File Offset: 0x00002E80
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

		// Token: 0x060000D1 RID: 209 RVA: 0x00004CC8 File Offset: 0x00002EC8
		public void ForceRefresh()
		{
			foreach (IPropertyEditor propertyEditor in this.propertyEditors.Values)
			{
				BaseEditor baseEditor = (BaseEditor)propertyEditor;
				baseEditor.ForceRefresh();
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00004D5C File Offset: 0x00002F5C
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

		// Token: 0x060000D3 RID: 211 RVA: 0x0000502C File Offset: 0x0000322C
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

		// Token: 0x060000D4 RID: 212 RVA: 0x00005124 File Offset: 0x00003324
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

		// Token: 0x060000D5 RID: 213 RVA: 0x000051F0 File Offset: 0x000033F0
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

		// Token: 0x060000D6 RID: 214 RVA: 0x0000523C File Offset: 0x0000343C
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

		// Token: 0x060000D7 RID: 215 RVA: 0x0000528C File Offset: 0x0000348C
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

		// Token: 0x060000D8 RID: 216 RVA: 0x000052DC File Offset: 0x000034DC
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

		// Token: 0x060000D9 RID: 217 RVA: 0x00005350 File Offset: 0x00003550
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

		// Token: 0x060000DA RID: 218 RVA: 0x000054AC File Offset: 0x000036AC
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

		// Token: 0x060000DB RID: 219 RVA: 0x0000550C File Offset: 0x0000370C
		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			base.CanFocus = true;
			base.HasFocus = true;
			base.HasFocus = false;
			base.CanFocus = false;
			return base.OnButtonPressEvent(evnt);
		}

		// Token: 0x04000040 RID: 64
		private Widget normalMainWidget;

		// Token: 0x04000041 RID: 65
		private TitleWidget titleWidget;

		// Token: 0x04000042 RID: 66
		private VBox propGroupVbox;

		// Token: 0x04000043 RID: 67
		private Widget plistMainWidget;

		// Token: 0x04000044 RID: 68
		private VBox plistPropVbox;

		// Token: 0x04000045 RID: 69
		private Dictionary<string, IPropertyEditor> propertyEditors = new Dictionary<string, IPropertyEditor>();

		// Token: 0x04000046 RID: 70
		private Dictionary<string, PropertyGroup> propertyGroups = new Dictionary<string, PropertyGroup>();

		// Token: 0x04000047 RID: 71
		private List<string> shrinkGroupList = new List<string>();

		// Token: 0x04000048 RID: 72
		private IReadOnlyList<object> _selectedObjects;
	}
}
