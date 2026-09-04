using System;
using System.Collections.Generic;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000023 RID: 35
	[DisplayName("Property_Scene3DFile")]
	[EngineClassName("Node")]
	public class GameNode3DObject : GameNodeObject, ISkyBox
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00005721 File Offset: 0x00003921
		public GameNode3DObject()
		{
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005734 File Offset: 0x00003934
		public GameNode3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00005748 File Offset: 0x00003948
		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode2D();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00005758 File Offset: 0x00003958
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.innerSkybox = new CSSkyBox();
			GameWindow.Current.GetSceneObject().GetCamera().InnerSkyBox = this.innerSkybox;
			this.LeftImage = new ImageFile(UserCameraObjectData.DefaultFile);
			this.RightImage = (this.UpImage = (this.DownImage = (this.ForwardImage = (this.BackImage = this.LeftImage))));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000057D1 File Offset: 0x000039D1
		private CSSkyBox GetCSSkyBox()
		{
			return this.innerSkybox;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x000057D9 File Offset: 0x000039D9
		public void ResetDefaultCameraBrush()
		{
			GameWindow.Current.GetSceneObject().GetCamera().InnerSkyBox = this.innerSkybox;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000142 RID: 322 RVA: 0x000057F5 File Offset: 0x000039F5
		// (set) Token: 0x06000143 RID: 323 RVA: 0x000057FD File Offset: 0x000039FD
		public string SkyboxResourceError
		{
			get
			{
				return this._error;
			}
			set
			{
				this._error = value;
				this.SkyBoxValid = (this._error == string.Empty);
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00005822 File Offset: 0x00003A22
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000582A File Offset: 0x00003A2A
		public bool SkyBoxValid { get; set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00005834 File Offset: 0x00003A34
		// (set) Token: 0x06000147 RID: 327 RVA: 0x0000588A File Offset: 0x00003A8A
		[DisplayName("Display_SkyBox_Image")]
		[Editor(typeof(SkyBoxImageEditor), typeof(SkyBoxImageEditor))]
		[Category("Group_SkyBox")]
		[PropertyOrder(114)]
		public virtual List<string> ResourceValue
		{
			get
			{
				return new List<string>
				{
					"LeftImage",
					"RightImage",
					"UpImage",
					"DownImage",
					"ForwardImage",
					"BackImage"
				};
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00005891 File Offset: 0x00003A91
		// (set) Token: 0x06000149 RID: 329 RVA: 0x0000589C File Offset: 0x00003A9C
		[Category("Group_SkyBox")]
		[PropertyOrder(113)]
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Display_SkyBox_Enabled")]
		public bool SkyBoxEnabled
		{
			get
			{
				return this._skyEnabeld;
			}
			set
			{
				this._skyEnabeld = value;
				this.GetCSSkyBox().SetEnabled(this._skyEnabeld);
				this.RaisePropertyChanged<bool>(() => this.SkyBoxEnabled);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000058FB File Offset: 0x00003AFB
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00005904 File Offset: 0x00003B04
		[DisplayName("Display_SkyBox_Left")]
		[DefaultValue(null)]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		public ResourceFile LeftImage
		{
			get
			{
				return this.leftFile;
			}
			set
			{
				this.leftFile = value;
				ImageFile defaultFile = new ImageFile(UserCameraObjectData.DefaultFile);
				ResourceFile.PreprocessToEngine(ref this.leftFile, defaultFile, false);
				SkyBoxManager.CheckSkyBoxResources(this);
				this.RaisePropertyChanged<ResourceFile>(() => this.LeftImage);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00005971 File Offset: 0x00003B71
		// (set) Token: 0x0600014D RID: 333 RVA: 0x0000597C File Offset: 0x00003B7C
		[DisplayName("Display_SkyBox_Right")]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[UndoProperty]
		public ResourceFile RightImage
		{
			get
			{
				return this.rightFile;
			}
			set
			{
				this.rightFile = value;
				ImageFile defaultFile = new ImageFile(UserCameraObjectData.DefaultFile);
				ResourceFile.PreprocessToEngine(ref this.rightFile, defaultFile, false);
				SkyBoxManager.CheckSkyBoxResources(this);
				this.RaisePropertyChanged<ResourceFile>(() => this.RightImage);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000059E9 File Offset: 0x00003BE9
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000059F4 File Offset: 0x00003BF4
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[UndoProperty]
		[DisplayName("Display_SkyBox_Up")]
		[DefaultValue(null)]
		public ResourceFile UpImage
		{
			get
			{
				return this.topFile;
			}
			set
			{
				this.topFile = value;
				ImageFile defaultFile = new ImageFile(UserCameraObjectData.DefaultFile);
				ResourceFile.PreprocessToEngine(ref this.topFile, defaultFile, false);
				SkyBoxManager.CheckSkyBoxResources(this);
				this.RaisePropertyChanged<ResourceFile>(() => this.UpImage);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00005A61 File Offset: 0x00003C61
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00005A6C File Offset: 0x00003C6C
		[UndoProperty]
		[DisplayName("Display_SkyBox_Down")]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		public ResourceFile DownImage
		{
			get
			{
				return this.bottomFile;
			}
			set
			{
				this.bottomFile = value;
				ImageFile defaultFile = new ImageFile(UserCameraObjectData.DefaultFile);
				ResourceFile.PreprocessToEngine(ref this.bottomFile, defaultFile, false);
				SkyBoxManager.CheckSkyBoxResources(this);
				this.RaisePropertyChanged<ResourceFile>(() => this.DownImage);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00005AD9 File Offset: 0x00003CD9
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00005AE4 File Offset: 0x00003CE4
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[DisplayName("Display_SkyBox_Front")]
		public ResourceFile ForwardImage
		{
			get
			{
				return this.forwardFile;
			}
			set
			{
				this.forwardFile = value;
				ImageFile defaultFile = new ImageFile(UserCameraObjectData.DefaultFile);
				ResourceFile.PreprocessToEngine(ref this.forwardFile, defaultFile, false);
				SkyBoxManager.CheckSkyBoxResources(this);
				this.RaisePropertyChanged<ResourceFile>(() => this.ForwardImage);
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005B51 File Offset: 0x00003D51
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00005B5C File Offset: 0x00003D5C
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DisplayName("Display_SkyBox_Back")]
		[DefaultValue(null)]
		public ResourceFile BackImage
		{
			get
			{
				return this.backFile;
			}
			set
			{
				this.backFile = value;
				ImageFile defaultFile = new ImageFile(UserCameraObjectData.DefaultFile);
				ResourceFile.PreprocessToEngine(ref this.backFile, defaultFile, false);
				SkyBoxManager.CheckSkyBoxResources(this);
				this.RaisePropertyChanged<ResourceFile>(() => this.BackImage);
			}
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005BC9 File Offset: 0x00003DC9
		public void ResetSkyBox()
		{
			this.GetCSSkyBox().ResetSkyBox();
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005BD8 File Offset: 0x00003DD8
		public void RefreshSkyBox()
		{
			this.GetCSSkyBox().RefreshSkyBox(this.leftFile.GetResourceData(), this.rightFile.GetResourceData(), this.topFile.GetResourceData(), this.bottomFile.GetResourceData(), this.forwardFile.GetResourceData(), this.backFile.GetResourceData());
		}

		// Token: 0x0400007D RID: 125
		private CSSkyBox innerSkybox;

		// Token: 0x0400007E RID: 126
		private string _error = string.Empty;

		// Token: 0x0400007F RID: 127
		private bool _skyEnabeld;

		// Token: 0x04000080 RID: 128
		private ResourceFile leftFile;

		// Token: 0x04000081 RID: 129
		private ResourceFile rightFile;

		// Token: 0x04000082 RID: 130
		private ResourceFile topFile;

		// Token: 0x04000083 RID: 131
		private ResourceFile bottomFile;

		// Token: 0x04000084 RID: 132
		private ResourceFile forwardFile;

		// Token: 0x04000085 RID: 133
		private ResourceFile backFile;
	}
}
