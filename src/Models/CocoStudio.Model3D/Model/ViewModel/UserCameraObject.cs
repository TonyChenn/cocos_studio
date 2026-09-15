using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Model.Event;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[ModelExtension(true, 102, EnumModelType.ThreeDimensional)]
	[ControlGroup("Control_3DControl", 0)]
	[DisplayName("Display_Component_Camera")]
	public class UserCameraObject : Node3DObject, ISkyBox
	{
		private CSUserCamera GetUserCamera()
		{
			return (CSUserCamera)this.innerNode;
		}

		public UserCameraObject()
		{
			Services.EventsService.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChange));
		}

		public UserCameraObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSUserCamera();
		}

		protected override void InitData(bool useScript)
		{
			this.UserCameraFlagMode = CameraFlag.USER1;
			this.innerNode.SetPosition3D(Point3F.Empty);
			this.LeftImage = new ImageFile(UserCameraObjectData.DefaultFile);
			this.RightImage = (this.UpImage = (this.DownImage = (this.ForwardImage = (this.BackImage = this.LeftImage))));
			base.InitData(useScript);
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			UserCameraObject userCameraObject = cObject as UserCameraObject;
			if (userCameraObject == null)
			{
				return;
			}
			userCameraObject.Fov = this.Fov;
			userCameraObject.ViewSize = this.ViewSize;
			userCameraObject.ClipPlane = this.ClipPlane;
			userCameraObject.UserCameraFlagMode = this.UserCameraFlagMode;
			userCameraObject.LeftImage = this.LeftImage;
			userCameraObject.RightImage = this.RightImage;
			userCameraObject.UpImage = this.UpImage;
			userCameraObject.DownImage = this.DownImage;
			userCameraObject.ForwardImage = this.ForwardImage;
			userCameraObject.BackImage = this.BackImage;
			userCameraObject.SkyBoxEnabled = this.SkyBoxEnabled;
		}

		protected internal CSCamera GetCSCamera()
		{
			return this.GetUserCamera().GetCSCamera();
		}

		protected internal CSSkyBox GetCSSkyBox()
		{
			return this.GetUserCamera().GetCSSkyBox();
		}

		[Category("Group_Routine")]
		[Browsable(false)]
		public override Point3F Scale3D
		{
			get
			{
				return base.Scale3D;
			}
			set
			{
				base.Scale3D = value;
			}
		}

		[Category("Group_Routine")]
		[Browsable(false)]
		public override int CameraFlagMode
		{
			get
			{
				return base.CameraFlagMode;
			}
			set
			{
				base.CameraFlagMode = value;
			}
		}

		[Browsable(false)]
		[Category("Group_Routine")]
		public override int Alpha
		{
			get
			{
				return base.Alpha;
			}
			set
			{
				base.Alpha = value;
			}
		}

		[Browsable(false)]
		public override Color CColor
		{
			get
			{
				return base.CColor;
			}
			set
			{
				base.CColor = value;
			}
		}

		public int CameraMask { get; set; }

		[Browsable(true)]
		[PropertyOrder(18)]
		[UndoProperty]
		[Editor(typeof(FovEditor), typeof(FovEditor))]
		[DisplayName("Display_Fov")]
		[Category("Group_Routine")]
		public float Fov
		{
			get
			{
				return this.GetCSCamera().GetFov();
			}
			set
			{
				this.GetCSCamera().SetFov(value);
				this.GetUserCamera().ResetFrustum();
				this.RaisePropertyChanged<float>(() => this.Fov);
			}
		}

		[PropertyOrder(19)]
		[Editor(typeof(ViewSizeEditor), typeof(ViewSizeEditor))]
		[UndoProperty]
		[DisplayName("Display_ViewSize")]
		[Category("Group_Routine")]
		[Browsable(false)]
		public SizeF ViewSize
		{
			get
			{
				return this.GetCSCamera().GetScreenSize();
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.GetCSCamera().SetScreenSize(value);
				this.GetUserCamera().ResetFrustum();
				this.RaisePropertyChanged<SizeF>(() => this.ViewSize);
			}
		}

		[Editor(typeof(ClipPlaneEditor), typeof(ClipPlaneEditor))]
		[UndoProperty]
		[DisplayName("Display_ClipPlane")]
		[Category("Group_Routine")]
		[Browsable(true)]
		[PropertyOrder(20)]
		public PointF ClipPlane
		{
			get
			{
				return new PointF(this.GetCSCamera().GetNearPlane(), this.GetCSCamera().GetFarPlane());
			}
			set
			{
				if (value == null)
				{
					return;
				}
				this.GetCSCamera().SetFarPlane(value.Y);
				this.GetCSCamera().SetNearPlane(value.X);
				this.GetUserCamera().ResetFrustum();
				this.RaisePropertyChanged<PointF>(() => this.ClipPlane);
			}
		}

		[DisplayName("Display_Camera_Flag")]
		[Category("Group_Routine")]
		[DefaultValue("")]
		[UndoProperty]
		[PropertyOrder(21)]
		public CameraFlag UserCameraFlagMode
		{
			get
			{
				return (CameraFlag)this.cameraFlag;
			}
			set
			{
				this.cameraFlag = (uint)value;
				this.RefreshCameraFlag(this._skyEnabeld);
				this.RaisePropertyChanged<CameraFlag>(() => this.UserCameraFlagMode);
			}
		}

		public uint CameraFlagData { get; set; }

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

		[Editor(typeof(SkyBoxImageEditor), typeof(SkyBoxImageEditor))]
		[PropertyOrder(114)]
		[DisplayName("Display_SkyBox_Image")]
		[Category("Group_SkyBox")]
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

		[DisplayName("Display_SkyBox_Enabled")]
		[Browsable(true)]
		[PropertyOrder(113)]
		[UndoProperty]
		[Category("Group_SkyBox")]
		public bool SkyBoxEnabled
		{
			get
			{
				return this._skyEnabeld;
			}
			set
			{
				this._skyEnabeld = value;
				this.RefreshCameraFlag(this._skyEnabeld);
				this.GetCSSkyBox().SetEnabled(this._skyEnabeld);
				this.RaisePropertyChanged<bool>(() => this.SkyBoxEnabled);
			}
		}

		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DisplayName("Display_SkyBox_Left")]
		[UndoProperty]
		[DefaultValue(null)]
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

		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[DisplayName("Display_SkyBox_Right")]
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

		[DefaultValue(null)]
		[DisplayName("Display_SkyBox_Up")]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[UndoProperty]
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
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[DefaultValue(null)]
		[DisplayName("Display_SkyBox_Down")]
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

		[DefaultValue(null)]
		[DisplayName("Display_SkyBox_Front")]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
		[UndoProperty]
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
		[DefaultValue(null)]
		[DisplayName("Display_SkyBox_Back")]
		[ResourceFilter(new string[]
		{
			"png",
			"jpg"
		})]
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

		private void OnCanvasSizeChange(CanvasSizeChangeEventArgs obj)
		{
			this.ViewSize = obj.NewSize;
		}

		private void RefreshCameraFlag(bool skyEnabled)
		{
			this.CameraFlagData = this.cameraFlag;
			if (!skyEnabled)
			{
				this.CameraFlagData |= 1024U;
			}
			this.GetUserCamera().SetCameraMask(this.CameraFlagData);
		}

		private uint cameraFlag = 2U;

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
