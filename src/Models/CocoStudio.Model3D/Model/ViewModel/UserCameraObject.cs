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
	// Token: 0x0200002C RID: 44
	[ModelExtension(true, 102, EnumModelType.ThreeDimensional)]
	[ControlGroup("Control_3DControl", 0)]
	[DisplayName("Display_Component_Camera")]
	public class UserCameraObject : Node3DObject, ISkyBox
	{
		// Token: 0x060001D1 RID: 465 RVA: 0x0000710B File Offset: 0x0000530B
		private CSUserCamera GetUserCamera()
		{
			return (CSUserCamera)this.innerNode;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00007118 File Offset: 0x00005318
		public UserCameraObject()
		{
			Services.EventsService.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.OnCanvasSizeChange));
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000714E File Offset: 0x0000534E
		public UserCameraObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00007169 File Offset: 0x00005369
		protected override void CreateCSObject()
		{
			this.innerNode = new CSUserCamera();
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00007178 File Offset: 0x00005378
		protected override void InitData(bool useScript)
		{
			this.UserCameraFlagMode = CameraFlag.USER1;
			this.innerNode.SetPosition3D(Point3F.Empty);
			this.LeftImage = new ImageFile(UserCameraObjectData.DefaultFile);
			this.RightImage = (this.UpImage = (this.DownImage = (this.ForwardImage = (this.BackImage = this.LeftImage))));
			base.InitData(useScript);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000071E4 File Offset: 0x000053E4
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

		// Token: 0x060001D7 RID: 471 RVA: 0x00007287 File Offset: 0x00005487
		protected internal CSCamera GetCSCamera()
		{
			return this.GetUserCamera().GetCSCamera();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00007294 File Offset: 0x00005494
		protected internal CSSkyBox GetCSSkyBox()
		{
			return this.GetUserCamera().GetCSSkyBox();
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x000072A1 File Offset: 0x000054A1
		// (set) Token: 0x060001DA RID: 474 RVA: 0x000072A9 File Offset: 0x000054A9
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001DB RID: 475 RVA: 0x000072B2 File Offset: 0x000054B2
		// (set) Token: 0x060001DC RID: 476 RVA: 0x000072BA File Offset: 0x000054BA
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001DD RID: 477 RVA: 0x000072C3 File Offset: 0x000054C3
		// (set) Token: 0x060001DE RID: 478 RVA: 0x000072CB File Offset: 0x000054CB
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001DF RID: 479 RVA: 0x000072D4 File Offset: 0x000054D4
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x000072DC File Offset: 0x000054DC
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x000072E5 File Offset: 0x000054E5
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x000072ED File Offset: 0x000054ED
		public int CameraMask { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x000072F6 File Offset: 0x000054F6
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007304 File Offset: 0x00005504
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

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007362 File Offset: 0x00005562
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00007370 File Offset: 0x00005570
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

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x000073D4 File Offset: 0x000055D4
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007400 File Offset: 0x00005600
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

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000747E File Offset: 0x0000567E
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00007488 File Offset: 0x00005688
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

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001EB RID: 491 RVA: 0x000074E2 File Offset: 0x000056E2
		// (set) Token: 0x060001EC RID: 492 RVA: 0x000074EA File Offset: 0x000056EA
		public uint CameraFlagData { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001ED RID: 493 RVA: 0x000074F3 File Offset: 0x000056F3
		// (set) Token: 0x060001EE RID: 494 RVA: 0x000074FB File Offset: 0x000056FB
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

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00007520 File Offset: 0x00005720
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x00007528 File Offset: 0x00005728
		public bool SkyBoxValid { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00007534 File Offset: 0x00005734
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000758A File Offset: 0x0000578A
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

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00007591 File Offset: 0x00005791
		// (set) Token: 0x060001F4 RID: 500 RVA: 0x0000759C File Offset: 0x0000579C
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

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x00007607 File Offset: 0x00005807
		// (set) Token: 0x060001F6 RID: 502 RVA: 0x00007610 File Offset: 0x00005810
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

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000767D File Offset: 0x0000587D
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x00007688 File Offset: 0x00005888
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x000076F5 File Offset: 0x000058F5
		// (set) Token: 0x060001FA RID: 506 RVA: 0x00007700 File Offset: 0x00005900
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

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000776D File Offset: 0x0000596D
		// (set) Token: 0x060001FC RID: 508 RVA: 0x00007778 File Offset: 0x00005978
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

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001FD RID: 509 RVA: 0x000077E5 File Offset: 0x000059E5
		// (set) Token: 0x060001FE RID: 510 RVA: 0x000077F0 File Offset: 0x000059F0
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000785D File Offset: 0x00005A5D
		// (set) Token: 0x06000200 RID: 512 RVA: 0x00007868 File Offset: 0x00005A68
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

		// Token: 0x06000201 RID: 513 RVA: 0x000078D5 File Offset: 0x00005AD5
		public void ResetSkyBox()
		{
			this.GetCSSkyBox().ResetSkyBox();
		}

		// Token: 0x06000202 RID: 514 RVA: 0x000078E4 File Offset: 0x00005AE4
		public void RefreshSkyBox()
		{
			this.GetCSSkyBox().RefreshSkyBox(this.leftFile.GetResourceData(), this.rightFile.GetResourceData(), this.topFile.GetResourceData(), this.bottomFile.GetResourceData(), this.forwardFile.GetResourceData(), this.backFile.GetResourceData());
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000793E File Offset: 0x00005B3E
		private void OnCanvasSizeChange(CanvasSizeChangeEventArgs obj)
		{
			this.ViewSize = obj.NewSize;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000794C File Offset: 0x00005B4C
		private void RefreshCameraFlag(bool skyEnabled)
		{
			this.CameraFlagData = this.cameraFlag;
			if (!skyEnabled)
			{
				this.CameraFlagData |= 1024U;
			}
			this.GetUserCamera().SetCameraMask(this.CameraFlagData);
		}

		// Token: 0x04000090 RID: 144
		private uint cameraFlag = 2U;

		// Token: 0x04000091 RID: 145
		private string _error = string.Empty;

		// Token: 0x04000092 RID: 146
		private bool _skyEnabeld;

		// Token: 0x04000093 RID: 147
		private ResourceFile leftFile;

		// Token: 0x04000094 RID: 148
		private ResourceFile rightFile;

		// Token: 0x04000095 RID: 149
		private ResourceFile topFile;

		// Token: 0x04000096 RID: 150
		private ResourceFile bottomFile;

		// Token: 0x04000097 RID: 151
		private ResourceFile forwardFile;

		// Token: 0x04000098 RID: 152
		private ResourceFile backFile;
	}
}
