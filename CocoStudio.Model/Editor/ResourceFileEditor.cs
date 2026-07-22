using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Core;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000097 RID: 151
	internal class ResourceFileEditor : ResourceFileBaseEditor
	{
		// Token: 0x06000532 RID: 1330 RVA: 0x00016AD0 File Offset: 0x00014CD0
		protected override void OnInitView()
		{
			base.OnInitView();
			this.fileButton = new Button(LanguageInfo.Property_ImportFile);
			this.fileButton.WidthRequest = 90;
			this.fileButton.HeightRequest = 25;
			this.fileTable.Attach(this.fileButton, 2U, 3U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 0U, 0U);
			this.fileButton.Show();
			this.fileButton.Clicked += this.fileButton_Clicked;
			if (this.filterAttr != null)
			{
				this.fileButton.TooltipText = string.Format(LanguageInfo.Display_SupportFileTypes, string.Join(",", this.filterAttr.FileFilter));
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00016B88 File Offset: 0x00014D88
		private async void fileButton_Clicked(object sender, EventArgs e)
		{
			ResourceFolder root = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			string[] fileTypes;
			if (this.filterAttr == null)
			{
				fileTypes = new string[]
				{
					"*.png",
					"*.jpg"
				};
			}
			else
			{
				fileTypes = new string[this.filterAttr.FileFilter.Length];
				for (int i = 0; i < this.filterAttr.FileFilter.Length; i++)
				{
					fileTypes[i] = "*." + this.filterAttr.FileFilter[i];
				}
			}
			string[] projectPath = FileChooserDialogModel.GetOpenFilePath(fileTypes, LanguageInfo.MessageBox_Content96, false, root.FullPath).FileNames;
			if (projectPath != null && projectPath.Count<string>() != 0)
			{
				List<ResourceItem> result = await Services.ProjectOperations.ImportResourcesAsync(root, projectPath, null);
				ResourceItem first = result.FirstOrDefault<ResourceItem>();
				ResourceFile resFile = first as ResourceFile;
				base.SetValue(resFile);
				IPlayControl control = PropertyItem.FirstObject as IPlayControl;
				if (control != null)
				{
					control.IsPlaying = true;
				}
			}
		}

		// Token: 0x04000261 RID: 609
		private Button fileButton;
	}
}
