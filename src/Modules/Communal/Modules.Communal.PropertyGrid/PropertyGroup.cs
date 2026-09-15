using System;
using System.Collections.Generic;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.PropertyGrid
{
	internal class PropertyGroup : IComparable<PropertyGroup>
	{
		static PropertyGroup()
		{
			PropertyGroup.defaultGroupSet.Add("Group_PosAndSize", 0);
			PropertyGroup.defaultGroupSet.Add("Group_Routine", 1);
			PropertyGroup.defaultGroupSet.Add("Display_Sudoku", 2);
			PropertyGroup.defaultGroupSet.Add("Group_Feature", 3);
			PropertyGroup.defaultGroupSet.Add("Group_SkyBox", 4);
			PropertyGroup.defaultGroupSet.Add("Group_Advanced", 5);
		}

		public CustomExpender ExpanderWidget { get; private set; }

		public string Category { get; private set; }

		public string DisplayName { get; private set; }

		public PropertyGroup(string category)
		{
			this.Category = category;
			this.DisplayName = LanguageOption.GetValueBykey(this.Category);
			if (PropertyGroup.defaultGroupSet.ContainsKey(category))
			{
				this.indexText = string.Format("{0}-{1}", PropertyGroup.defaultGroupSet[category], category);
			}
			else
			{
				this.indexText = string.Format("99-{0}", category);
			}
			this.ExpanderWidget = new CustomExpender(this.DisplayName);
			this.ExpanderWidget.Expanded = true;
			this.mainVbox = new VBox();
			this.mainVbox.Spacing = (int)PropertyPadStyle.mainRowSpacing;
			Alignment alignment = new Alignment(0.5f, 0.5f, 1f, 1f);
			alignment.TopPadding = PropertyPadStyle.mainRowSpacing;
			alignment.RightPadding = PropertyPadStyle.rightPadding;
			alignment.Add(this.mainVbox);
			this.mainVbox.Show();
			this.ExpanderWidget.Add(alignment);
			alignment.Show();
		}

		public void Clear()
		{
			this.propertyEditorList.Clear();
			this.mainVbox.RemoveAll();
			this.ExpanderWidget.Hide();
		}

		public void Add(IPropertyEditor editor)
		{
			this.propertyEditorList.Add(editor);
			this.mainVbox.PackStart(editor.EditorWidget, false, false, 0U);
		}

		public void RefreshView()
		{
			if (this.propertyEditorList == null)
			{
				this.ExpanderWidget.Hide();
			}
			else
			{
				bool visible = false;
				if (this.propertyEditorList != null)
				{
					foreach (IPropertyEditor propertyEditor in this.propertyEditorList)
					{
						if (propertyEditor.Visible)
						{
							visible = true;
							break;
						}
					}
					this.ExpanderWidget.Visible = visible;
				}
			}
		}

		public int CompareTo(PropertyGroup other)
		{
			return this.indexText.CompareTo(other.indexText);
		}

		private static readonly Dictionary<string, int> defaultGroupSet = new Dictionary<string, int>();

		private VBox mainVbox;

		private string indexText;

		private List<IPropertyEditor> propertyEditorList = new List<IPropertyEditor>();
	}
}
