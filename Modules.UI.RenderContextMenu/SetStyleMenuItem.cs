using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Modules.Communal.MultiLanguage;

namespace Modules.UI.RenderContextMenu
{
	// Token: 0x02000022 RID: 34
	public class SetStyleMenuItem : MenuItem, IObjectMenuItem
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000109 RID: 265 RVA: 0x00006E10 File Offset: 0x00005010
		// (set) Token: 0x06000108 RID: 264 RVA: 0x00006DEC File Offset: 0x00004FEC
		public VisualObject TriggerObject
		{
			get
			{
				return this.triggerObject;
			}
			set
			{
				if (this.triggerObject != value)
				{
					this.triggerObject = value;
				}
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00006E28 File Offset: 0x00005028
		public SetStyleMenuItem(string[] filetype, string name, string title) : base(title)
		{
			base.Name = name;
			this.FileType = filetype;
			base.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.SetStyle_Click);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00006E58 File Offset: 0x00005058
		private void SetStyle_Click(object sender, EventArgs e)
		{
			Menu menu = base.Parent as Menu;
			menu.Deactivate();
			this.SetWidgetStyle();
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00006E80 File Offset: 0x00005080
		public static async Task<ResourceItem> addSourceItem(string[] fileTypes = null)
		{
			string[] selectPath = FileChooserDialogModel.GetOpenFilePath(fileTypes, LanguageInfo.MessageBox_Content96, true, Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory.ToString()).FileNames;
			ResourceItem result2;
			if (selectPath == null)
			{
				result2 = null;
			}
			else
			{
				ResourceItem result = null;
				ResourceFolder folder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
				IList<ResourceItem> res = await Services.ProjectOperations.ImportResourcesAsync(folder, selectPath, null);
				if (res != null && res.Count > 0)
				{
					result = res[0];
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00006ECC File Offset: 0x000050CC
		public ResourceFolder GetRootFolder(Solution solution)
		{
			if (solution != null)
			{
				ResourceGroup resourceGroup = solution.RootFolder.Items[0] as ResourceGroup;
				if (resourceGroup != null)
				{
					return resourceGroup.RootFolder;
				}
			}
			return null;
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00006F14 File Offset: 0x00005114
		public async void SetWidgetStyle()
		{
			if (this.TriggerObject == null)
			{
				throw new InvalidOperationException("Trigger widget was not detected.");
			}
			ResourceItem resourcepath = await SetStyleMenuItem.addSourceItem(this.FileType);
			if (resourcepath != null)
			{
				if (resourcepath is PlistImageFolder)
				{
					MessageBox.Show(LanguageInfo.MessageBox190_FileError, MessageBoxImage.Other, null, null);
				}
				else
				{
					this.SendSetStyleMessage();
					using (CompositeTask.Run("设置资源", null))
					{
						PropertyInfo property = this.TriggerObject.GetType().GetProperty(base.Name);
						if (property != null)
						{
							property.SetValue(this.TriggerObject, resourcepath as ResourceFile, null);
							IPlayControl playControl = this.TriggerObject as IPlayControl;
							if (playControl != null)
							{
								playControl.IsPlaying = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00006F50 File Offset: 0x00005150
		public string SendSetStyleMessage()
		{
			return this.TriggerObject.GetType().Name;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00006F72 File Offset: 0x00005172
		public void UpdateMenuItemState()
		{
		}

		// Token: 0x04000077 RID: 119
		private VisualObject triggerObject;

		// Token: 0x04000078 RID: 120
		private string[] FileType;
	}
}
