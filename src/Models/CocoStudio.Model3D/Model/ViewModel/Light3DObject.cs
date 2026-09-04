using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000024 RID: 36
	[ModelExtension(true, 100, EnumModelType.ThreeDimensional)]
	[ControlGroup("Control_3DControl", 0)]
	[DisplayName("Display_Component_Light3D")]
	public class Light3DObject : Node3DObject
	{
		// Token: 0x06000158 RID: 344 RVA: 0x00005C32 File Offset: 0x00003E32
		public Light3DObject()
		{
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005C3A File Offset: 0x00003E3A
		public Light3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005C43 File Offset: 0x00003E43
		protected override void CreateCSObject()
		{
			this.innerNode = new CSLight();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005C50 File Offset: 0x00003E50
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005C5C File Offset: 0x00003E5C
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			Light3DObject light3DObject = cObject as Light3DObject;
			if (light3DObject == null)
			{
				return;
			}
			light3DObject.Type = this.Type;
			light3DObject.Enable = this.Enable;
			light3DObject.Flag = this.Flag;
			light3DObject.Intensity = this.Intensity;
			light3DObject.Range = this.Range;
			light3DObject.OuterAngle = this.OuterAngle;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005CC3 File Offset: 0x00003EC3
		private CSLight GetInnerObject()
		{
			return this.innerNode as CSLight;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00005CD0 File Offset: 0x00003ED0
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00005CD8 File Offset: 0x00003ED8
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

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00005CE1 File Offset: 0x00003EE1
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00005CE9 File Offset: 0x00003EE9
		[Category("Group_Routine")]
		[Browsable(false)]
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

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00005CF2 File Offset: 0x00003EF2
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00005D00 File Offset: 0x00003F00
		[UndoProperty]
		[Browsable(true)]
		[PropertyOrder(30)]
		[Category("Group_Feature")]
		[DisplayName("Display_Component_LightType")]
		public LightType Type
		{
			get
			{
				return (LightType)this.GetInnerObject().GetLightType();
			}
			set
			{
				this.GetInnerObject().SetLightType((int)value);
				this.RaisePropertyChanged<LightType>(() => this.Type);
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00005D53 File Offset: 0x00003F53
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00005D60 File Offset: 0x00003F60
		[Category("Group_Feature")]
		[PropertyOrder(31)]
		[UndoProperty]
		[DisplayName("Display_Component_LightFlag")]
		[Browsable(true)]
		public LightFlag Flag
		{
			get
			{
				return (LightFlag)this.GetInnerObject().GetLightFlag();
			}
			set
			{
				this.GetInnerObject().SetLightFlag((int)value);
				this.RaisePropertyChanged<LightFlag>(() => this.Flag);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00005DB3 File Offset: 0x00003FB3
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00005DC0 File Offset: 0x00003FC0
		[DisplayName("Display_Component_Intensity")]
		[PropertyOrder(33)]
		[UndoProperty]
		[ValueRange(0, 10, 0.1f, 1f)]
		[Category("Group_Feature")]
		[Browsable(true)]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[ConstructParams(new object[]
		{
			true,
			false,
			false,
			""
		})]
		public float Intensity
		{
			get
			{
				return this.GetInnerObject().GetIntensity();
			}
			set
			{
				this.GetInnerObject().SetIntensity(value);
				this.RaisePropertyChanged<float>(() => this.Intensity);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00005E13 File Offset: 0x00004013
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00005E20 File Offset: 0x00004020
		[PropertyOrder(32)]
		[Browsable(true)]
		[UndoProperty]
		[DisplayName("Display_Component_LightEnable")]
		[Category("Group_Feature")]
		public bool Enable
		{
			get
			{
				return this.GetInnerObject().IsEnabled();
			}
			set
			{
				this.GetInnerObject().SetEnabled(value);
				this.RaisePropertyChanged<bool>(() => this.Enable);
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00005E73 File Offset: 0x00004073
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00005E80 File Offset: 0x00004080
		[Browsable(true)]
		[ValueRange(0, 2147483647, 1f, 10f)]
		[Category("Group_Feature")]
		[UndoProperty]
		[PropertyOrder(34)]
		[DisplayName("Display_Component_Range")]
		public float Range
		{
			get
			{
				return this.GetInnerObject().GetRange();
			}
			set
			{
				this.GetInnerObject().SetRange(value);
				this.RaisePropertyChanged<float>(() => this.Range);
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00005ED3 File Offset: 0x000040D3
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00005EE0 File Offset: 0x000040E0
		[ValueRange(1, 179, 1f, 10f)]
		[Editor(typeof(SliderEditor), typeof(SliderEditor))]
		[DisplayName("Display_Component_Angle")]
		[Category("Group_Feature")]
		[Browsable(true)]
		[ConstructParams(new object[]
		{
			true,
			false,
			false,
			""
		})]
		[PropertyOrder(35)]
		[UndoProperty]
		public float OuterAngle
		{
			get
			{
				return this.GetInnerObject().GetOuterAngle();
			}
			set
			{
				this.GetInnerObject().SetOuterAngle(value);
				this.RaisePropertyChanged<float>(() => this.OuterAngle);
			}
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00005F33 File Offset: 0x00004133
		public bool HitTestControlNode(MouseEventArgs args)
		{
			return this.GetInnerObject().HitControlPoint(args.Point);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00005F46 File Offset: 0x00004146
		protected override void OnMouseDown(MouseEventArgs args)
		{
			if (base.Recorder.IsAutoRecord)
			{
				base.Recorder.Stop(true);
			}
			this.GetInnerObject().OnMouseDown(args.Point);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00005F73 File Offset: 0x00004173
		protected override void OnMouseUp(MouseEventArgs args)
		{
			this.GetInnerObject().OnMouseUp(args.Point);
			if (!base.Recorder.IsAutoRecord)
			{
				base.Recorder.Start(true, false);
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00005FA4 File Offset: 0x000041A4
		protected override void OnMouseMove(MouseEventArgs args)
		{
			this.GetInnerObject().OnMouseMove(args.Point);
			this.RaisePropertyChanged<float>(() => this.Range);
			this.RaisePropertyChanged<float>(() => this.OuterAngle);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006037 File Offset: 0x00004237
		public void RefreshLightState(bool enabled)
		{
			this.GetInnerObject().RefreshLightState(enabled);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00006045 File Offset: 0x00004245
		internal void RefreshLightIndex(int index)
		{
			this.GetInnerObject().RefreshLightIndex(index);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00006053 File Offset: 0x00004253
		internal override void AfterAdded()
		{
			base.AfterAdded();
			Light3DHelper.Instance.AddLight(this);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00006066 File Offset: 0x00004266
		internal override void BeforeRemoved()
		{
			base.BeforeRemoved();
			Light3DHelper.Instance.RemoveLight(this);
		}
	}
}
