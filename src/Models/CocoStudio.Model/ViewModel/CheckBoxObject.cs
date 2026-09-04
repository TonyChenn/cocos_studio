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
	// Token: 0x0200010A RID: 266
	[DisplayName("Display_Component_UICheckBox")]
	[ModelExtension(true, 1)]
	[ControlGroup("ComToolPad", 1)]
	[EngineClassName("CheckBox")]
	public class CheckBoxObject : WidgetObject, IDisplayState, ICallBackEvent, IResetSize
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x00024CB4 File Offset: 0x00022EB4
		private CSCheckBox GetInnerWidget()
		{
			return (CSCheckBox)this.innerNode;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00024CD1 File Offset: 0x00022ED1
		public CheckBoxObject()
		{
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00024D06 File Offset: 0x00022F06
		public CheckBoxObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x00024D3C File Offset: 0x00022F3C
		protected override void CreateCSObject()
		{
			this.innerNode = new CSCheckBox();
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00024D4C File Offset: 0x00022F4C
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

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00024DE4 File Offset: 0x00022FE4
		// (set) Token: 0x06000936 RID: 2358 RVA: 0x00024DFC File Offset: 0x00022FFC
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

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00024E60 File Offset: 0x00023060
		// (set) Token: 0x06000938 RID: 2360 RVA: 0x00024E80 File Offset: 0x00023080
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

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x00024EEC File Offset: 0x000230EC
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x00024F29 File Offset: 0x00023129
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

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x00024F34 File Offset: 0x00023134
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x00024F65 File Offset: 0x00023165
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

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x0600093D RID: 2365 RVA: 0x00024F70 File Offset: 0x00023170
		// (set) Token: 0x0600093E RID: 2366 RVA: 0x00024FB8 File Offset: 0x000231B8
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

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x00025034 File Offset: 0x00023234
		// (set) Token: 0x06000940 RID: 2368 RVA: 0x0002507C File Offset: 0x0002327C
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

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x000250F8 File Offset: 0x000232F8
		// (set) Token: 0x06000942 RID: 2370 RVA: 0x00025140 File Offset: 0x00023340
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

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x000251BC File Offset: 0x000233BC
		// (set) Token: 0x06000944 RID: 2372 RVA: 0x00025204 File Offset: 0x00023404
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

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x00025280 File Offset: 0x00023480
		// (set) Token: 0x06000946 RID: 2374 RVA: 0x000252C8 File Offset: 0x000234C8
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

		// Token: 0x06000947 RID: 2375 RVA: 0x00025344 File Offset: 0x00023544
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

		// Token: 0x06000948 RID: 2376 RVA: 0x000253D8 File Offset: 0x000235D8
		protected internal override bool IsCanChangeSize()
		{
			return true;
		}

		// Token: 0x04000431 RID: 1073
		private bool isNormal = true;

		// Token: 0x04000432 RID: 1074
		private ResourceFile normalBackFile = null;

		// Token: 0x04000433 RID: 1075
		private ResourceFile pressedBackFile = null;

		// Token: 0x04000434 RID: 1076
		private ResourceFile disableBackFile = null;

		// Token: 0x04000435 RID: 1077
		private ResourceFile nodeNormalFile = null;

		// Token: 0x04000436 RID: 1078
		private ResourceFile nodeDisableFile = null;
	}
}
