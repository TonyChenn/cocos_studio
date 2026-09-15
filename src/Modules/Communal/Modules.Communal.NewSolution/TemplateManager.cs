using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.NewSolution
{
	internal class TemplateManager
	{
		public static TemplateManager Instance { get; private set; } = new TemplateManager();

		static TemplateManager()
		{
			try
			{
				ISolutionTemplate[] extensionObjects = AddinManager.GetExtensionObjects<ISolutionTemplate>();
				foreach (ISolutionTemplate item in extensionObjects)
				{
					TemplateManager.slnTemplateList.Add(item);
				}
				TemplateManager.slnTemplateList.Sort();
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("初始化新建项目模板时出错", exception);
			}
		}

		public List<RadioItemWidget> GetSolutionTemplates()
		{
			List<ISolutionTemplate> list = new List<ISolutionTemplate>();
			foreach (ISolutionTemplate solutionTemplate in TemplateManager.slnTemplateList)
			{
				if (solutionTemplate.Enable)
				{
					list.Add(solutionTemplate);
				}
			}
			List<SampleInfo> sampleInfoList = this.GetSampleInfoList();
			sampleInfoList.Sort();
			foreach (SampleInfo sampleInfo in sampleInfoList)
			{
				ISolutionTemplate solutionTemplate2 = new SampleSolutionTemplate(sampleInfo);
				if (solutionTemplate2.Enable)
				{
					list.Add(solutionTemplate2);
				}
			}
			List<RadioItemWidget> list2 = new List<RadioItemWidget>();
			foreach (ISolutionTemplate slnTemplate in list)
			{
				RadioItemWidget item = this.CreateTemplateItem(slnTemplate);
				list2.Add(item);
			}
			return list2;
		}

		public List<RadioItemWidget> GetSoltuionGroups(List<RadioItemWidget> templateList)
		{
			Dictionary<EnumTemplateGroup, RadioGroup> dictionary = new Dictionary<EnumTemplateGroup, RadioGroup>();
			List<RadioGroup> list = new List<RadioGroup>();
			RadioGroup radioGroup = new RadioGroup(LanguageInfo.NewSolution_GroupAll);
			list.Add(radioGroup);
			foreach (RadioItemWidget radioItemWidget in templateList)
			{
				radioGroup.AddItem(radioItemWidget);
				ISolutionTemplate solutionTemplate = radioItemWidget.Tag as ISolutionTemplate;
				RadioGroup radioGroup2;
				if (dictionary.ContainsKey(solutionTemplate.Group))
				{
					radioGroup2 = dictionary[solutionTemplate.Group];
				}
				else
				{
					string key = "NewSolution_Group" + solutionTemplate.Group;
					string valueBykey = LanguageOption.GetValueBykey(key);
					radioGroup2 = new RadioGroup(valueBykey);
					list.Add(radioGroup2);
					dictionary[solutionTemplate.Group] = radioGroup2;
				}
				radioGroup2.AddItem(radioItemWidget);
			}
			List<RadioItemWidget> list2 = new List<RadioItemWidget>();
			foreach (RadioGroup radioGroup3 in list)
			{
				RadioItemWidget radioItemWidget2 = new RadioItemWidget();
				this.SetGroupTypeWidgetStyle(radioItemWidget2);
				radioItemWidget2.SetContent(new GroupTypeContent(radioGroup3, radioGroup3.Name));
				list2.Add(radioItemWidget2);
			}
			return list2;
		}

		private List<SampleInfo> GetSampleInfoList()
		{
			HashSet<SampleInfo> samplesFromDir = this.GetSamplesFromDir(Option.DefaultSamplesDir);
			HashSet<SampleInfo> samplesFromDir2 = this.GetSamplesFromDir(Option.DownloadSamplesDir);
			samplesFromDir.UnionWith(samplesFromDir2);
			return samplesFromDir.ToList<SampleInfo>();
		}

		private HashSet<SampleInfo> GetSamplesFromDir(string dir)
		{
			HashSet<SampleInfo> hashSet = new HashSet<SampleInfo>();
			if (!Directory.Exists(dir))
			{
				return hashSet;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(dir);
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
			{
				string name = directoryInfo2.Name;
				string fullName = directoryInfo2.FullName;
				SampleInfo sampleInfo = SampleInfo.CreateInstance(directoryInfo2.Name, directoryInfo2.FullName);
				if (sampleInfo != null && sampleInfo.SampleVersion <= Option.EditorVersion)
				{
					hashSet.Add(sampleInfo);
				}
			}
			return hashSet;
		}

		private RadioItemWidget CreateTemplateItem(ISolutionTemplate slnTemplate)
		{
			RadioItemWidget radioItemWidget = new RadioItemWidget();
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				radioItemWidget.NormalBgColor = NewSolutionStyles.White;
				radioItemWidget.SelectedBgColor = NewSolutionStyles.White;
				radioItemWidget.HoverBgColor = NewSolutionStyles.Launcher_BgGray;
				radioItemWidget.BorderColor = NewSolutionStyles.Launcher_Blue;
			}
			else
			{
				radioItemWidget.NormalBgColor = NewSolutionStyles.Studio_BgGray;
				radioItemWidget.SelectedBgColor = NewSolutionStyles.Studio_HoverDarkGray;
				radioItemWidget.HoverBgColor = NewSolutionStyles.Studio_HoverDarkGray;
				radioItemWidget.BorderColor = NewSolutionStyles.Studio_Blue;
			}
			radioItemWidget.Tag = slnTemplate;
			SolutionTypeContent content = new SolutionTypeContent(slnTemplate.Info);
			radioItemWidget.SetContent(content);
			return radioItemWidget;
		}

		private void SetGroupTypeWidgetStyle(RadioItemWidget itemWidget)
		{
			itemWidget.HideBorder();
			if (Option.CurrentApp == EnumApp.Launcher)
			{
				itemWidget.NormalBgColor = NewSolutionStyles.White;
				itemWidget.SelectedBgColor = NewSolutionStyles.Launcher_Blue;
				itemWidget.HoverBgColor = NewSolutionStyles.Launcher_HoverGray;
				return;
			}
			itemWidget.NormalBgColor = NewSolutionStyles.Studio_BgGray;
			itemWidget.SelectedBgColor = NewSolutionStyles.Studio_Blue;
			itemWidget.HoverBgColor = NewSolutionStyles.Studio_HoverDarkGray;
		}

		private static List<ISolutionTemplate> slnTemplateList = new List<ISolutionTemplate>();
	}
}
