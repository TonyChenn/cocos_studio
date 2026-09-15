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
	[DisplayName("Property_Scene3DFile")]
	[EngineClassName("Node")]
	public class GameNode3DObject : GameNodeObject, ISkyBox
	{
		public GameNode3DObject()
		{
		}

		public GameNode3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSNode2D();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.innerSkybox = new CSSkyBox();
			GameWindow.Current.GetSceneObject().GetCamera().InnerSkyBox = this.innerSkybox;
			this.LeftImage = new ImageFile(UserCameraObjectData.DefaultFile);
			this.RightImage = (this.UpImage = (this.DownImage = (this.ForwardImage = (this.BackImage = this.LeftImage))));
		}

		private CSSkyBox GetCSSkyBox()
		{
			return this.innerSkybox;
		}

		public void ResetDefaultCameraBrush()
		{
			GameWindow.Current.GetSceneObject().GetCamera().InnerSkyBox = this.innerSkybox;
		}

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

		public bool SkyBoxValid { get; set; }

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

		public void ResetSkyBox()
		{
			this.GetCSSkyBox().ResetSkyBox();
		}

		public void RefreshSkyBox()
		{
			this.GetCSSkyBox().RefreshSkyBox(this.leftFile.GetResourceData(), this.rightFile.GetResourceData(), this.topFile.GetResourceData(), this.bottomFile.GetResourceData(), this.forwardFile.GetResourceData(), this.backFile.GetResourceData());
		}

		private CSSkyBox innerSkybox;

		private string _error = string.Empty;

		private bool _skyEnabeld;

		private ResourceFile leftFile;

		private ResourceFile rightFile;

		private ResourceFile topFile;

		private ResourceFile bottomFile;

		private ResourceFile forwardFile;

		private ResourceFile backFile;
	}
}
