using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Interface;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[DisplayName("Display_Component_UICheckBox")]
	[ModelExtension(true, 1)]
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("CheckBox")]
	public class CheckBoxObject : WidgetObject, IDisplayState, ICallBackEvent, IResetSize
	{
		private CSCheckBox GetInnerWidget()
		{
			return (CSCheckBox)this.innerNode;
		}

		public CheckBoxObject()
		{
		}

		public CheckBoxObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSCheckBox();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.NormalBackFileData = ResourceFile.DefaultMarker;
				this.NodeNormalFileData = ResourceFile.DefaultMarker;
				if (!Option.UserConfig.IsSimplifyDefaultRes)
				{
					this.PressedBackFileData = ResourceFile.DefaultMarker;
					this.DisableBackFileData = ResourceFile.DefaultMarker;
					this.NodeDisableFileData = ResourceFile.DefaultMarker;
				}
				else
				{
					this.PressedBackFileData = null;
					this.DisableBackFileData = null;
					this.NodeDisableFileData = null;
				}
				this.CheckedState = true;
				this.TouchEnable = true;
			}
		}

		[Category("Group_Feature")]
		[UndoProperty]
		[DefaultValue(true)]
		[PropertyOrder(90)]
		[Editor(typeof(CheckBoxEditor), typeof(CheckBoxEditor))]
		[DisplayName("Display_State")]
		public virtual bool DisplayState
		{
			get
			{
				return this.isNormal;
			}
			set
			{
				this.isNormal = value;
				this.GetInnerWidget().ChangeState(this.isNormal);
				this.RaisePropertyChanged<bool>(() => this.DisplayState);
			}
		}

		[DisplayName("Display_Select_UnSelect")]
		[Category("Group_Feature")]
		[PropertyOrder(91)]
		[DefaultValue(true)]
		[UndoProperty]
		public virtual bool CheckedState
		{
			get
			{
				return this.GetInnerWidget().GetChecked();
			}
			set
			{
				if (this.GetInnerWidget().GetChecked() != value)
				{
					this.GetInnerWidget().SetChecked(value);
					this.RaisePropertyChanged<bool>(() => this.CheckedState);
				}
			}
		}

		[DisplayName("Display_ImageResources")]
		[PropertyOrder(88)]
		[Editor(typeof(ResourceGroupEditor), typeof(ResourceGroupEditor))]
		[Category("Group_Feature")]
		public List<string> ResourceValue
		{
			get
			{
				return new List<string>
				{
					"NormalBackFileData",
					"PressedBackFileData",
					"DisableBackFileData"
				};
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		[PropertyOrder(89)]
		[Category("Group_Feature")]
		[Editor(typeof(ResourceGroupEditor), typeof(ResourceGroupEditor))]
		[DisplayName("Display_MarkResource")]
		public List<string> ResourceNodeValue
		{
			get
			{
				return new List<string>
				{
					"NodeNormalFileData",
					"NodeDisableFileData"
				};
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		[UndoProperty]
		[DefaultValue(null)]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("ContexMenu_BackgroundNormal")]
		[ResourceFilter(true, true, new string[]
		{
			"png",
			"jpg"
		})]
		public ResourceFile NormalBackFileData
		{
			get
			{
				if (this.normalBackFile == null)
				{
					this.normalBackFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetNormalGroundFile()) as ResourceFile);
				}
				return this.normalBackFile;
			}
			set
			{
				this.normalBackFile = value;
				ImageFile defaultFile = new ImageFile(CheckBoxObjectData.Default_Normal);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.normalBackFile, defaultFile, true);
				this.GetInnerWidget().SetNormalGroudFile(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.NormalBackFileData);
			}
		}

		[UndoProperty]
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[IgnoreResize]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("ContexMenu_BackgroundPressed")]
		public ResourceFile PressedBackFileData
		{
			get
			{
				if (this.pressedBackFile == null)
				{
					this.pressedBackFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetPressedGroundFile()) as ResourceFile);
				}
				return this.pressedBackFile;
			}
			set
			{
				this.pressedBackFile = value;
				ImageFile defaultFile = new ImageFile(CheckBoxObjectData.Default_Press);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.pressedBackFile, defaultFile, true);
				this.GetInnerWidget().SetPressedGroudFile(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.PressedBackFileData);
			}
		}

		[DisplayName("ContexMenu_BackgroundDisabled")]
		[DefaultValue(null)]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		[UndoProperty]
		public ResourceFile DisableBackFileData
		{
			get
			{
				if (this.disableBackFile == null)
				{
					this.disableBackFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetDisabledGroundFile()) as ResourceFile);
				}
				return this.disableBackFile;
			}
			set
			{
				this.disableBackFile = value;
				ImageFile defaultFile = new ImageFile(CheckBoxObjectData.Default_Disable);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.disableBackFile, defaultFile, true);
				this.GetInnerWidget().SetDisabledGroudFile(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.DisableBackFileData);
			}
		}

		[DisplayName("ContexMenu_CheckNormal")]
		[DefaultValue(null)]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[UndoProperty]
		[ResourceFilter(true, true, new string[]
		{
			"png",
			"jpg"
		})]
		public ResourceFile NodeNormalFileData
		{
			get
			{
				if (this.nodeNormalFile == null)
				{
					this.nodeNormalFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetNormalNodeFile()) as ResourceFile);
				}
				return this.nodeNormalFile;
			}
			set
			{
				this.nodeNormalFile = value;
				ImageFile defaultFile = new ImageFile(CheckBoxObjectData.Default_NodeNormal);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.nodeNormalFile, defaultFile, true);
				this.GetInnerWidget().SetNormalNodeFile(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.NodeNormalFileData);
			}
		}

		[UndoProperty]
		[Editor(typeof(ResourceImageEditor), typeof(ResourceImageEditor))]
		[DisplayName("ContexMenu_CheckDisabled")]
		[ResourceFilter(false, true, new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		public ResourceFile NodeDisableFileData
		{
			get
			{
				if (this.nodeDisableFile == null)
				{
					this.nodeDisableFile = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetDisabledNodeFile()) as ResourceFile);
				}
				return this.nodeDisableFile;
			}
			set
			{
				this.nodeDisableFile = value;
				ImageFile defaultFile = new ImageFile(CheckBoxObjectData.Default_NodeDisable);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.nodeDisableFile, defaultFile, true);
				this.GetInnerWidget().SetDisabledNodeFile(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.NodeDisableFileData);
			}
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			CheckBoxObject checkBoxObject = cObject as CheckBoxObject;
			if (checkBoxObject != null)
			{
				checkBoxObject.NormalBackFileData = this.NormalBackFileData;
				checkBoxObject.PressedBackFileData = this.PressedBackFileData;
				checkBoxObject.DisableBackFileData = this.DisableBackFileData;
				checkBoxObject.NodeNormalFileData = this.NodeNormalFileData;
				checkBoxObject.NodeDisableFileData = this.NodeDisableFileData;
				checkBoxObject.CheckedState = this.CheckedState;
				checkBoxObject.DisplayState = this.DisplayState;
				checkBoxObject.Size = this.Size;
			}
		}

		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		private bool isNormal = true;

		private ResourceFile normalBackFile = null;

		private ResourceFile pressedBackFile = null;

		private ResourceFile disableBackFile = null;

		private ResourceFile nodeNormalFile = null;

		private ResourceFile nodeDisableFile = null;
	}
}
