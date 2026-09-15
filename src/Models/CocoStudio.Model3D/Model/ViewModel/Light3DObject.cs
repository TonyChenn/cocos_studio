using System;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[ModelExtension(true, 100, EnumModelType.ThreeDimensional)]
	[ControlGroup("Control_3DControl", 0)]
	[DisplayName("Display_Component_Light3D")]
	public class Light3DObject : Node3DObject
	{
		public Light3DObject()
		{
		}

		public Light3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSLight();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
		}

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

		private CSLight GetInnerObject()
		{
			return this.innerNode as CSLight;
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

		public bool HitTestControlNode(MouseEventArgs args)
		{
			return this.GetInnerObject().HitControlPoint(args.Point);
		}

		protected override void OnMouseDown(MouseEventArgs args)
		{
			if (base.Recorder.IsAutoRecord)
			{
				base.Recorder.Stop(true);
			}
			this.GetInnerObject().OnMouseDown(args.Point);
		}

		protected override void OnMouseUp(MouseEventArgs args)
		{
			this.GetInnerObject().OnMouseUp(args.Point);
			if (!base.Recorder.IsAutoRecord)
			{
				base.Recorder.Start(true, false);
			}
		}

		protected override void OnMouseMove(MouseEventArgs args)
		{
			this.GetInnerObject().OnMouseMove(args.Point);
			this.RaisePropertyChanged<float>(() => this.Range);
			this.RaisePropertyChanged<float>(() => this.OuterAngle);
		}

		public void RefreshLightState(bool enabled)
		{
			this.GetInnerObject().RefreshLightState(enabled);
		}

		internal void RefreshLightIndex(int index)
		{
			this.GetInnerObject().RefreshLightIndex(index);
		}

		internal override void AfterAdded()
		{
			base.AfterAdded();
			Light3DHelper.Instance.AddLight(this);
		}

		internal override void BeforeRemoved()
		{
			base.BeforeRemoved();
			Light3DHelper.Instance.RemoveLight(this);
		}
	}
}
