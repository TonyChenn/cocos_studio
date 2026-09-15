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
	internal class TitleWidget : TitleMainEventBox
	{
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

		public void Clear()
		{
			if (this.lastEditorWidget != null && this.rightTable.Children.Contains(this.lastEditorWidget))
			{
				this.rightTable.Remove(this.lastEditorWidget);
			}
			this.lastEditorWidget = null;
		}

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

		private void SetDisplayNameWidget(IReadOnlyList<object> selectObjs)
		{
			string displayName = this.GetDisplayName(selectObjs);
			this.nameFstLineLabel.Text = displayName;
			this.nameFstLineLabel.Show();
			this.nameSndLineLabel.Hide();
		}

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

		private ImageView iconImageView;

		private Label nameFstLineLabel;

		private Label nameSndLineLabel;

		private Table rightTable;

		private Label engineTypeLabel;

		private Widget lastEditorWidget;
	}
}
