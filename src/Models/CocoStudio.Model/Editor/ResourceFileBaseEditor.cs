using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class ResourceFileBaseEditor : BaseEditor
	{
		protected override Widget OnCreateWidget()
		{
			this.filterAttr = (base.PropertyItem.Attributes[typeof(ResourceFilterAttribute)] as ResourceFilterAttribute);
			this._displayName = base.PropertyItem.Name;
			this.OnInitView();
			ResourceFile resourceFile = base.PropertyItem.Values[0] as ResourceFile;
			if (resourceFile != null && !string.IsNullOrEmpty(resourceFile.Name))
			{
				this.fileLabel.Text = resourceFile.Name;
				this.fileLabel.TooltipText = resourceFile.FullPath;
				this.resetLabel.LabelText = string.Format(" {0} ", LanguageInfo.Property_Reset);
			}
			return this.resourceEventBox;
		}

		protected virtual void OnInitView()
		{
			this.resourceEventBox = new EventBox();
			Gtk.Drag.DestSet(this.resourceEventBox, DestDefaults.All, this.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
			Gtk.Drag.SourceSet(this.resourceEventBox, ModifierType.Button1Mask, this.target_tableWindows, DragAction.Copy | DragAction.Move | DragAction.Link);
			this.resourceEventBox.DragMotion += this.ResourceFileImportBase_DragMotion;
			this.resourceEventBox.DragDrop += this.resourceEventBox_DragDrop;
			this.fileTable = new Table(1U, 3U, false);
			bool supportsLocateButton = ResourceLocateButton.SupportsFileEditor(PropertyItem.FirstObject, PropertyItem.Name);
			this.fileLabel = new Label();
			this.fileLabel.Text = "";
			this.fileLabel.MaxWidthChars = 20;
			this.fileLabel.Yalign = 0.5f;
			this.resetLabel = new LabelLinkButton(null);
			this.resetLabel.Label.SetFontSize(12.0);
			this.resetLabel.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.linkLabel_LeftClicked);
			uint fileLabelColumn = 0U;
			if (supportsLocateButton)
			{
				ResourceLocateButton locateButton = new ResourceLocateButton(delegate
				{
					return PropertyItem.FirstObject.GetType().GetProperty(PropertyItem.Name).GetValue(PropertyItem.FirstObject, null) as ResourceFile;
				});
				this.fileTable.Attach(locateButton.CreateCenteredAlignment(), 0U, 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Fill, 2U, 0U);
				fileLabelColumn = 1U;
			}
			this.fileTable.Attach(this.fileLabel, fileLabelColumn, fileLabelColumn + 1U, 0U, 1U, AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 1U);
			this.fileTable.Attach(this.resetLabel, fileLabelColumn + 1U, fileLabelColumn + 2U, 0U, 1U, AttachOptions.Fill, AttachOptions.Expand | AttachOptions.Fill, 0U, 1U);
			this.resourceEventBox.Add(this.fileTable);
			this.fileTable.HeightRequest = 25;
			this.resourceEventBox.ShowAll();
		}

		protected override void OnSetControl()
		{
			this.ScenceSetValue();
		}

		private void ScenceSetValue()
		{
			ResourceFile resourceFile = PropertyItem.FirstObject.GetType().GetProperty(this._displayName).GetValue(PropertyItem.FirstObject, null) as ResourceFile;
			if (resourceFile != null && !string.IsNullOrEmpty(resourceFile.FullPath))
			{
				this.fileLabel.Text = resourceFile.Name;
				this.fileLabel.TooltipText = resourceFile.FullPath;
				this.resetLabel.LabelText = string.Format(" {0} ", LanguageInfo.Property_Reset);
			}
			else
			{
				this.fileLabel.Text = "";
				this.fileLabel.TooltipText = "";
				this.resetLabel.LabelText = "";
			}
		}

		private void resourceEventBox_DragDrop(object o, DragDropArgs args)
		{
			object dragData = args.Context.GetDragData();
			ResourceInfoDragData resourceInfoDragData = dragData as ResourceInfoDragData;
			if (resourceInfoDragData != null)
			{
				ResourceFile resourceFile = resourceInfoDragData.Items.FirstOrDefault<ResourceItem>() as ResourceFile;
				if (resourceFile != null && this.CheckResource(resourceFile))
				{
					this.SetValue(resourceFile);
				}
			}
		}

		private void ResourceFileImportBase_DragMotion(object o, DragMotionArgs args)
		{
			object dragData = args.Context.GetDragData();
			ResourceInfoDragData resourceInfoDragData = dragData as ResourceInfoDragData;
			if (resourceInfoDragData == null)
			{
				args.SetAllowDragAction((DragAction)0);
				args.RetVal = true;
			}
			else
			{
				ResourceFile resourceFile = resourceInfoDragData.Items.FirstOrDefault<ResourceItem>() as ResourceFile;
				if (resourceFile == null || !this.CheckResource(resourceFile))
				{
					args.SetAllowDragAction((DragAction)0);
					args.RetVal = true;
				}
			}
		}

		private void linkLabel_LeftClicked(object sender, EventArgs e)
		{
			List<PropertyDescriptor> propertyDescriptors = PropertyManager.Instance.GetPropertyDescriptors(PropertyItem.FirstObject.GetType());
			foreach (PropertyDescriptor propertyDescriptor in propertyDescriptors)
			{
				if (propertyDescriptor.Name == this._displayName)
				{
					using (CompositeTask.Run(base.PropertyItem.Name, null))
					{
						propertyDescriptor.ResetValue(PropertyItem.FirstObject);
					}
					this.ScenceSetValue();
					IPlayControl playControl = PropertyItem.FirstObject as IPlayControl;
					if (playControl != null)
					{
						playControl.IsPlaying = true;
					}
					break;
				}
			}
		}

		private bool CheckResource(ResourceFile file)
		{
			bool result;
			if (this.filterAttr == null)
			{
				result = false;
			}
			else
			{
				foreach (string text in this.filterAttr.FileFilter)
				{
					if (file.FileName.Extension.Remove(0, 1).ToLower() == text.ToLower())
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		protected void SetValue(ResourceFile file)
		{
			if (file != null)
			{
				if (file.DataError != null)
				{
					LogConfig.Output.Error(LanguageInfo.Dialog_ImportResource_ErrorMessage);
				}
				else
				{
					string text = this.CheckNest(file);
					if (!string.IsNullOrEmpty(text))
					{
						LogConfig.Output.Error(text, null);
					}
					else
					{
						using (CompositeTask.Run(base.PropertyItem.Name, null))
						{
							base.PropertyItem.FirstValue = file;
						}
						IPlayControl playControl = PropertyItem.FirstObject as IPlayControl;
						if (playControl != null)
						{
							playControl.IsPlaying = true;
						}
						this.ScenceSetValue();
					}
				}
			}
		}

		private string CheckNest(ResourceFile value)
		{
			CocosItem cocosItem = value as CocosItem;
			string result;
			if (cocosItem == null)
			{
				result = null;
			}
			else
			{
				CocosItem currentSelectedProject = Services.ProjectOperations.CurrentSelectedProject;
				if (currentSelectedProject == null)
				{
					result = null;
				}
				else
				{
					GameFile gameFile = currentSelectedProject.CocosFile as GameFile;
					if (gameFile == null)
					{
						result = "No Project";
					}
					else if (cocosItem.FullPath == gameFile.FileName)
					{
						result = LanguageInfo.MessageBox207_NestedSelfError;
					}
					else
					{
						result = cocosItem.CheckNest(currentSelectedProject);
					}
				}
			}
			return result;
		}

		protected Table fileTable;

		private EventBox resourceEventBox;

		private Label fileLabel;

		private LabelLinkButton resetLabel;

		private TargetEntry[] target_tableWindows = new TargetEntry[]
		{
			DragTargetType.CocoStudioTarget
		};

		private string _displayName = "";

		protected ResourceFilterAttribute filterAttr;
	}
}
