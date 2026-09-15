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
	public class SetStyleMenuItem : MenuItem, IObjectMenuItem
	{
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

		public SetStyleMenuItem(string[] filetype, string name, string title) : base(title)
		{
			base.Name = name;
			this.FileType = filetype;
			base.ButtonReleaseEvent += new ButtonReleaseEventHandler(this.SetStyle_Click);
		}

		private void SetStyle_Click(object sender, EventArgs e)
		{
			Menu menu = base.Parent as Menu;
			menu.Deactivate();
			this.SetWidgetStyle();
		}

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

		public string SendSetStyleMessage()
		{
			return this.TriggerObject.GetType().Name;
		}

		public void UpdateMenuItemState()
		{
		}

		private VisualObject triggerObject;

		private string[] FileType;
	}
}
