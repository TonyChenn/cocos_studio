using System;
using System.Collections.Generic;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x0200001C RID: 28
	internal class PropertyGroup : IComparable<PropertyGroup>
	{
		// Token: 0x060000BA RID: 186 RVA: 0x00004654 File Offset: 0x00002854
		static PropertyGroup()
		{
			PropertyGroup.defaultGroupSet.Add("Group_PosAndSize", 0);
			PropertyGroup.defaultGroupSet.Add("Group_Routine", 1);
			PropertyGroup.defaultGroupSet.Add("Display_Sudoku", 2);
			PropertyGroup.defaultGroupSet.Add("Group_Feature", 3);
			PropertyGroup.defaultGroupSet.Add("Group_SkyBox", 4);
			PropertyGroup.defaultGroupSet.Add("Group_Advanced", 5);
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000BB RID: 187 RVA: 0x000046D4 File Offset: 0x000028D4
		// (set) Token: 0x060000BC RID: 188 RVA: 0x000046EB File Offset: 0x000028EB
		public CustomExpender ExpanderWidget { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000BD RID: 189 RVA: 0x000046F4 File Offset: 0x000028F4
		// (set) Token: 0x060000BE RID: 190 RVA: 0x0000470B File Offset: 0x0000290B
		public string Category { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004714 File Offset: 0x00002914
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x0000472B File Offset: 0x0000292B
		public string DisplayName { get; private set; }

		// Token: 0x060000C1 RID: 193 RVA: 0x00004734 File Offset: 0x00002934
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

		// Token: 0x060000C2 RID: 194 RVA: 0x0000484F File Offset: 0x00002A4F
		public void Clear()
		{
			this.propertyEditorList.Clear();
			this.mainVbox.RemoveAll();
			this.ExpanderWidget.Hide();
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004876 File Offset: 0x00002A76
		public void Add(IPropertyEditor editor)
		{
			this.propertyEditorList.Add(editor);
			this.mainVbox.PackStart(editor.EditorWidget, false, false, 0U);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000489C File Offset: 0x00002A9C
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

		// Token: 0x060000C5 RID: 197 RVA: 0x00004944 File Offset: 0x00002B44
		public int CompareTo(PropertyGroup other)
		{
			return this.indexText.CompareTo(other.indexText);
		}

		// Token: 0x04000039 RID: 57
		private static readonly Dictionary<string, int> defaultGroupSet = new Dictionary<string, int>();

		// Token: 0x0400003A RID: 58
		private VBox mainVbox;

		// Token: 0x0400003B RID: 59
		private string indexText;

		// Token: 0x0400003C RID: 60
		private List<IPropertyEditor> propertyEditorList = new List<IPropertyEditor>();
	}
}
