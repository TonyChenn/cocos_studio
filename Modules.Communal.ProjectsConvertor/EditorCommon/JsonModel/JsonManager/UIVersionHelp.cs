using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using Newtonsoft.Json.Linq;

namespace EditorCommon.JsonModel.JsonManager
{
	// Token: 0x02000033 RID: 51
	public class UIVersionHelp
	{
		// Token: 0x0600036B RID: 875 RVA: 0x00008C34 File Offset: 0x00006E34
		public static UIVersion IsNeedToConvert(string jsonData)
		{
			JObject jobject = null;
			try
			{
				jobject = JObject.Parse(jsonData);
			}
			catch (Exception ex)
			{
				LogConfig.Output.Error(ex.ToString());
				return UIVersion.undefine;
			}
			JToken jtoken = jobject["widgetTree"];
			if (jtoken != null)
			{
				return UIVersion.windows;
			}
			JToken jtoken2 = jobject["nodeTree"];
			if (jtoken2 != null)
			{
				return UIVersion.beta;
			}
			return UIVersion.undefine;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00008C98 File Offset: 0x00006E98
		public static string ConvertToNewJsonData(string jsonData)
		{
			UIVersion uiversion = UIVersionHelp.IsNeedToConvert(jsonData);
			if (uiversion == UIVersion.undefine)
			{
				return jsonData;
			}
			string text = jsonData;
			if (uiversion == UIVersion.windows)
			{
				text = text.Replace("#SPEditorCommon", "#EditorCommon");
				text = text.Replace("ComGUITextButtonSurrogate", "ButtonSurrogate");
				text = text.Replace("ComGUIDragPanelSurrogate", "ScrollViewSurrogate");
				text = text.Replace("ComGUITextAreaSurrogate", "LabelSurrogate");
				text = text.Replace("ComGUILayoutSurrogate", "LayoutSurrogate");
				text = text.Replace("ComGUIButtonSurrogate", "ButtonSurrogate");
				text = text.Replace("ComGUICheckBoxSurrogate", "CheckBoxSurrogate");
				text = text.Replace("ComGUIImageViewSurrogate", "ImageViewSurrogate");
				text = text.Replace("ComGUILabelAtlasSurrogate", "LabelAtlasSurrogate");
				text = text.Replace("ComGUILabelBMFontSurrogate", "LabelBMFontSurrogate");
				text = text.Replace("ComGUILabelSurrogate", "LabelSurrogate");
				text = text.Replace("ComGUIListViewSurrogate", "ListViewSurrogate");
				text = text.Replace("ComGUILoadingBarSurrogate", "LoadingBarSurrogate");
				text = text.Replace("ComGUINodeContainerSurrogate", "NodeContainerSurrogate");
				text = text.Replace("ComGUIPageViewSurrogate", "PageViewSurrogate");
				text = text.Replace("ComGUIPanelSurrogate", "PanelSurrogate");
				text = text.Replace("ComGUIScrollViewSurrogate", "ScrollViewSurrogate");
				text = text.Replace("ComGUISliderSurrogate", "SliderSurrogate");
				text = text.Replace("ComGUITextFieldSurrogate", "TextFieldSurrogate");
				text = text.Replace("widgetTree", "nodeTree");
				text = text.Replace("__type", "$type");
				text = text.Replace("actiontag", "actionTag");
				text = text.Replace("LayoutSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.LayoutSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("ButtonSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.ButtonSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("CheckBoxSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.CheckBoxSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("ImageViewSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.ImageViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("LabelAtlasSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.LabelAtlasSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("LabelBMFontSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.LabelBMFontSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("LabelSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.LabelSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("ListViewSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.ListViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("LoadingBarSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.LoadingBarSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("NodeContainerSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.NodeContainerSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("PageViewSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.PageViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("PanelSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.PanelSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("ScrollViewSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.ScrollViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("SliderSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.SliderSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("TextFieldSurrogate:#EditorCommon.JsonModel.Component.GUI", "EditorCommon.JsonModel.Component.GUI.TextFieldSurrogate, Modules.Communal.ProjectsConvertor");
			}
			else if (uiversion == UIVersion.beta)
			{
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.RootGUISurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.RootGUISurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.LayoutSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.LayoutSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.ButtonSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.ButtonSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.CheckBoxSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.CheckBoxSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.ImageViewSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.ImageViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.LabelAtlasSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.LabelAtlasSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.LabelBMFontSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.LabelBMFontSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.LabelSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.LabelSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.ListViewSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.ListViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.LoadingBarSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.LoadingBarSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.NodeContainerSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.NodeContainerSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("PEditorCommon.JsonModel.Component.GUI.PageViewSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.PageViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.PanelSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.PanelSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.ScrollViewSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.ScrollViewSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.SliderSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.SliderSurrogate, Modules.Communal.ProjectsConvertor");
				text = text.Replace("EditorCommon.JsonModel.Component.GUI.TextFieldSurrogate, EditorCommon", "EditorCommon.JsonModel.Component.GUI.TextFieldSurrogate, Modules.Communal.ProjectsConvertor");
			}
			return text;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0000904C File Offset: 0x0000724C
		public static void ConvertToNewAnimationData(string jsonData)
		{
			JObject jobject = null;
			try
			{
				jobject = JObject.Parse(jsonData);
			}
			catch (Exception ex)
			{
				LogConfig.Output.Error(ex.ToString());
				return;
			}
			JToken jtoken = jobject["animation"];
			if (jtoken != null)
			{
				JToken jtoken2 = jtoken["actionlist"];
				if (jtoken2 == null)
				{
					return;
				}
				foreach (JToken jtoken3 in ((IEnumerable<JToken>)jtoken2))
				{
					if (jtoken3 != null)
					{
						JToken jtoken4 = jtoken3["actionnodelist"];
						if (jtoken4 != null)
						{
							foreach (JToken jtoken5 in ((IEnumerable<JToken>)jtoken4))
							{
								if (jtoken5 != null)
								{
									JToken jtoken6 = jtoken5["ActionTag"];
								}
							}
						}
					}
				}
				return;
			}
		}
	}
}
