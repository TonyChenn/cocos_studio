using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Components;
using Xwt.Drawing;

namespace Modules.Communal.PropertyGrid
{
	// Token: 0x02000022 RID: 34
	internal class TitleWidget : TitleMainEventBox
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x0000629C File Offset: 0x0000449C
		public TitleWidget()
		{
			Widget child = this.CreateLeftWidget();
			Widget widget = this.CreateRightWidget();
			EventBox eventBox = new EventBox();
			eventBox.WidthRequest = 1;
			eventBox.SetNormalBg(WindowStyle.WindowLineColor);
			HBox hbox = new HBox();
			hbox.Spacing = 15;
			hbox.PackStart(child, false, false, 0U);
			hbox.PackStart(eventBox, false, false, 0U);
			hbox.PackStart(widget);
			base.Add(hbox);
			base.ShowAll();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006318 File Offset: 0x00004518
		private Widget CreateLeftWidget()
		{
			this.iconImageView = new ImageView();
			this.nameFstLineLabel = new Label();
			this.nameSndLineLabel = new Label();
			VBox vbox = new VBox();
			vbox.PackStart(this.nameFstLineLabel, false, false, 0U);
			vbox.PackStart(this.nameSndLineLabel, false, false, 0U);
			VBox vbox2 = new VBox();
			vbox2.WidthRequest = 75;
			vbox2.Spacing = 3;
			vbox2.PackStart(this.iconImageView);
			vbox2.PackStart(vbox, false, false, 0U);
			return vbox2;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000063A4 File Offset: 0x000045A4
		private Widget CreateRightWidget()
		{
			this.rightTable = new Table(2U, 2U, false);
			this.rightTable.RowSpacing = PropertyPadStyle.mainRowSpacing;
			this.rightTable.ColumnSpacing = PropertyPadStyle.mainColumnSpacing;
			Label label = new Label(LanguageInfo.Display_Name);
			Label label2 = new Label(LanguageInfo.Display_ExportType);
			this.engineTypeLabel = new Label(" Object");
			label.Xalign = (label2.Xalign = 1f);
			this.engineTypeLabel.Xalign = 0f;
			this.rightTable.Attach(label, 0U, 1U, 0U, 1U, AttachOptions.Fill, (AttachOptions)0, 0U, 0U);
			this.rightTable.Attach(label2, 0U, 1U, 1U, 2U, AttachOptions.Fill, (AttachOptions)0, 1U, 1U);
			this.rightTable.Attach(this.engineTypeLabel, 1U, 2U, 1U, 2U, AttachOptions.Fill, (AttachOptions)0, 0U, 0U);
			return this.rightTable;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00006480 File Offset: 0x00004680
		public void Clear()
		{
			if (this.lastEditorWidget != null && this.rightTable.Children.Contains(this.lastEditorWidget))
			{
				this.rightTable.Remove(this.lastEditorWidget);
			}
			this.lastEditorWidget = null;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000064D0 File Offset: 0x000046D0
		public void ShowTitle(IReadOnlyList<object> selectObjs, Widget nameEditorWidget)
		{
			this.iconImageView.Image = this.GetIconImage(selectObjs);
			this.SetDisplayNameWidget(selectObjs);
			this.rightTable.Attach(nameEditorWidget, 1U, 2U, 0U, 1U);
			nameEditorWidget.Show();
			this.lastEditorWidget = nameEditorWidget;
			this.engineTypeLabel.Text = string.Format(" {0}", this.GetTypeName(selectObjs));
			base.Show();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0000653C File Offset: 0x0000473C
		private Xwt.Drawing.Image GetIconImage(IReadOnlyList<object> selectObjs)
		{
			string text = "Multi";
			if (selectObjs.Count == 1)
			{
				text = selectObjs[0].GetType().Name;
				text = text.Substring(0, text.Length - 6);
			}
			string resourceID = string.Format("CocoStudio.DefaultResource.ComponentResource.{0}.png", text);
			return ImageIcon.GetCustomControlIcon(resourceID);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000065A0 File Offset: 0x000047A0
		private void SetDisplayNameWidget(IReadOnlyList<object> selectObjs)
		{
			string displayName = this.GetDisplayName(selectObjs);
			this.nameFstLineLabel.Text = displayName;
			this.nameFstLineLabel.Show();
			this.nameSndLineLabel.Hide();
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000065E0 File Offset: 0x000047E0
		private string GetDisplayName(IReadOnlyList<object> selectObjs)
		{
			string result;
			if (selectObjs.Count > 1)
			{
				if (selectObjs.Count > 999)
				{
					result = LanguageInfo.Property_ManyObject;
				}
				else
				{
					result = string.Format(LanguageInfo.Property_Object, selectObjs.Count);
				}
			}
			else
			{
				object obj = selectObjs[0];
				object[] customAttributes = obj.GetType().GetCustomAttributes(false);
				foreach (object obj2 in customAttributes)
				{
					DisplayNameAttribute displayNameAttribute = obj2 as DisplayNameAttribute;
					if (displayNameAttribute != null)
					{
						string text = displayNameAttribute.DisplayName;
						if (text == "Property_NodeFile")
						{
							CocosItem file = Services.Workbench.ActiveDocument.File;
							GameFile gameFile = file.CocosFile as GameFile;
							if (gameFile != null && gameFile.Type == "Scene")
							{
								text = "Property_SceneFile";
							}
						}
						return LanguageOption.GetValueBykey(text);
					}
				}
				result = "Object";
			}
			return result;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00006710 File Offset: 0x00004910
		private string GetTypeName(IReadOnlyList<object> selectObjs)
		{
			string result;
			if (selectObjs == null || selectObjs.Count == 0)
			{
				result = "None";
			}
			else if (selectObjs.Count > 1)
			{
				result = "Multy Objects";
			}
			else
			{
				Type type = selectObjs[0].GetType();
				object[] customAttributes = type.GetCustomAttributes(typeof(EngineClassNameAttribute), false);
				EngineClassNameAttribute engineClassNameAttribute = customAttributes.FirstOrDefault<object>() as EngineClassNameAttribute;
				if (engineClassNameAttribute == null)
				{
					result = type.Name;
				}
				else
				{
					result = engineClassNameAttribute.CocoType;
				}
			}
			return result;
		}

		// Token: 0x0400005F RID: 95
		private ImageView iconImageView;

		// Token: 0x04000060 RID: 96
		private Label nameFstLineLabel;

		// Token: 0x04000061 RID: 97
		private Label nameSndLineLabel;

		// Token: 0x04000062 RID: 98
		private Table rightTable;

		// Token: 0x04000063 RID: 99
		private Label engineTypeLabel;

		// Token: 0x04000064 RID: 100
		private Widget lastEditorWidget;
	}
}
