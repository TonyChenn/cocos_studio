using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.ViewModel.HitTest;
using Gdk;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200001B RID: 27
	public class ControlNode3D : VisualObject
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060000DF RID: 223 RVA: 0x00004764 File Offset: 0x00002964
		// (remove) Token: 0x060000E0 RID: 224 RVA: 0x0000479C File Offset: 0x0000299C
		public event EventHandler CoordinateSystemChanged;

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x000047D1 File Offset: 0x000029D1
		public static ControlNode3D Instance
		{
			get
			{
				if (ControlNode3D._instance == null)
				{
					ControlNode3D._instance = new ControlNode3D();
				}
				return ControlNode3D._instance;
			}
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x000047EC File Offset: 0x000029EC
		public ControlNode3D()
		{
			SceneObject sceneObject = GameWindow.Current.GetSceneObject();
			this.controlCamera = sceneObject.GetCamera();
			this.innerNode = new CSComControlNode3D();
			this.DumyObject = new Dumy3DObject();
			sceneObject.AddChild(this);
			sceneObject.AddChild(this.DumyObject);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000484A File Offset: 0x00002A4A
		private CSComControlNode3D GetComControl()
		{
			return this.innerNode;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004852 File Offset: 0x00002A52
		internal override CSVisualObject GetCSVisual()
		{
			return this.innerNode;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000485C File Offset: 0x00002A5C
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004876 File Offset: 0x00002A76
		public ControlNode3D.Opt Operate
		{
			get
			{
				return (ControlNode3D.Opt)this.GetComControl().GetOpt();
			}
			set
			{
				this.GetComControl().SetOpt((ControlOpt.Opt)value);
				this.InitDumyObject();
				this.GetComControl().RefreshSelectable();
				this.RefreshSpaceButtonState();
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000489B File Offset: 0x00002A9B
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x000048A3 File Offset: 0x00002AA3
		public ControlNode3D.PivotPoint OptionPoint
		{
			get
			{
				return this.pivotpoint;
			}
			set
			{
				this.pivotpoint = value;
				this.InitDumyObject();
				this.GetComControl().RefreshSelectable();
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x000048BD File Offset: 0x00002ABD
		// (set) Token: 0x060000EA RID: 234 RVA: 0x000048C8 File Offset: 0x00002AC8
		public ControlNode3D.Space OptitonSpace
		{
			get
			{
				return this.space;
			}
			set
			{
				this.space = value;
				bool flag = this.space != ControlNode3D.Space.Local;
				this.GetComControl().SetSpace(flag);
				this.InitDumyObject();
				this.GetComControl().RefreshSelectable();
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004906 File Offset: 0x00002B06
		// (set) Token: 0x060000EC RID: 236 RVA: 0x0000490E File Offset: 0x00002B0E
		public Dumy3DObject DumyObject { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004917 File Offset: 0x00002B17
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00004920 File Offset: 0x00002B20
		public List<VisualObject> SelectObjectList
		{
			get
			{
				return this.selectObjectList;
			}
			set
			{
				CSComControlNode3D comControl = this.GetComControl();
				this.controlCamera.controlNode = comControl;
				if (this.selectObjectList.Count > 0)
				{
					this.selectObjectList.LastOrDefault<VisualObject>().PropertyChanged -= this.ComControlObject_PropertyChanged;
					this.selectObjectList.Clear();
				}
				for (int i = 0; i < value.Count; i++)
				{
					if (!(value[i] is GameNode3DObject))
					{
						this.selectObjectList.Add(value[i]);
					}
				}
				if (this.selectObjectList.Count > 0)
				{
					this.isActive = true;
					this.SelectObjectList.LastOrDefault<VisualObject>().PropertyChanged += this.ComControlObject_PropertyChanged;
					this.InitDumyObject();
					this.FilterUserCamera();
					comControl.RefreshSelectable();
				}
				else
				{
					this.isActive = false;
					comControl.SetTarget(null);
					this.controlCamera.GetUserCameraPreview().Rander(false);
				}
				this.RefreshSpaceButtonState();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00004A10 File Offset: 0x00002C10
		protected override void OnMouseMove(MouseEventArgs args)
		{
			PointF point = args.Point;
			this.isContorlMoved = true;
			CSComControlNode3D comControl = this.GetComControl();
			if (KeyboardExtend.IsModifyKeyPressed(ModifierType.ShiftMask))
			{
				comControl.SelectShiftKey(true);
			}
			else
			{
				comControl.SelectShiftKey(false);
			}
			ControlResult result = comControl.OnMouseMove(point);
			this.DumyObject.EffectResult(result);
			bool center = this.OptionPoint == ControlNode3D.PivotPoint.Center;
			bool global = this.OptitonSpace == ControlNode3D.Space.Global;
			foreach (VisualObject visualObject in this.SelectObjectList)
			{
				UserCameraObject userCameraObject = visualObject as UserCameraObject;
				if (userCameraObject == null || this.Operate != ControlNode3D.Opt.Scale)
				{
					Node3DObject node = visualObject as Node3DObject;
					this.DumyObject.EffectToTarget(node, result, center, global);
				}
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00004AE4 File Offset: 0x00002CE4
		protected override void OnMouseDown(MouseEventArgs args)
		{
			try
			{
				PointF point = args.Point;
				CSComControlNode3D comControl = this.GetComControl();
				if (comControl != null)
				{
					this.isSelect = comControl.OnMouseDown(point);
					if (this.isSelect && this.selectObjectList != null)
					{
						foreach (VisualObject visualObject in this.selectObjectList)
						{
							if (visualObject.Recorder.IsAutoRecord)
							{
								visualObject.Recorder.Stop(true);
							}
							else
							{
								LogConfig.Logger.Error("don't try to start a auto record object's record");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(ex.ToString());
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00004BB0 File Offset: 0x00002DB0
		protected override void OnMouseUp(MouseEventArgs args)
		{
			try
			{
				PointF point = args.Point;
				this.isContorlMoved = false;
				CSComControlNode3D comControl = this.GetComControl();
				if (comControl != null)
				{
					comControl.OnMouseUp(point);
					if (this.isSelect && this.selectObjectList != null)
					{
						foreach (VisualObject visualObject in this.selectObjectList)
						{
							if (!visualObject.Recorder.IsAutoRecord)
							{
								visualObject.Recorder.Start(true, false);
							}
							else
							{
								LogConfig.Logger.Error("don't try to start a auto record object's record");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error(ex.ToString());
			}
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00004C80 File Offset: 0x00002E80
		public override HitTestResult HitTest(PointF point)
		{
			if (this.isActive && this.GetComControl().OnSelect(point))
			{
				return new HitTestResult(point, this.Visible);
			}
			return null;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00004CA6 File Offset: 0x00002EA6
		public void MoveCameraToSelected()
		{
			if (this.SelectObjectList.Count == 0)
			{
				return;
			}
			this.controlCamera.MoveTo(this.DumyObject);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004CC8 File Offset: 0x00002EC8
		private void InitDumyObject()
		{
			if (this.selectObjectList.Count == 0)
			{
				return;
			}
			Node3DObject node3DObject = this.selectObjectList.FirstOrDefault<VisualObject>() as Node3DObject;
			if (node3DObject == null)
			{
				return;
			}
			CSComControlNode3D comControl = this.GetComControl();
			if (this.selectObjectList.Count == 1 || this.pivotpoint == ControlNode3D.PivotPoint.Pivot)
			{
				this.DumyObject.Rotation3D = node3DObject.Rotation3D;
				this.DumyObject.Position3D = node3DObject.WorldPosition3D;
				this.DumyObject.Scale3D = new Point3F(1f, 1f, 1f);
			}
			else
			{
				Point3F point3F = new Point3F(0f, 0f, 0f);
				foreach (VisualObject visualObject in this.selectObjectList)
				{
					Node3DObject node3DObject2 = visualObject as Node3DObject;
					if (node3DObject2 != null)
					{
						point3F.Add(node3DObject2.WorldPosition3D);
					}
				}
				point3F.X /= (float)this.selectObjectList.Count;
				point3F.Y /= (float)this.selectObjectList.Count;
				point3F.Z /= (float)this.selectObjectList.Count;
				this.DumyObject.Rotation3D = node3DObject.Rotation3D;
				this.DumyObject.Position3D = point3F;
				this.DumyObject.Scale3D = new Point3F(1f, 1f, 1f);
			}
			if (this.OptitonSpace == ControlNode3D.Space.Global)
			{
				this.DumyObject.Rotation3D = new Point3F(0f, 0f, 0f);
			}
			comControl.SetTarget(this.DumyObject.GetCSVisual());
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004E8C File Offset: 0x0000308C
		private void RefreshSpaceButtonState()
		{
			if (this.CoordinateSystemChanged != null)
			{
				this.CoordinateSystemChanged(this, new EventArgs());
			}
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004EA8 File Offset: 0x000030A8
		private void FilterUserCamera()
		{
			UserCameraObject userCameraObject = null;
			for (int i = 0; i < this.selectObjectList.Count; i++)
			{
				userCameraObject = (this.selectObjectList[i] as UserCameraObject);
				if (userCameraObject != null)
				{
					break;
				}
			}
			if (userCameraObject != null)
			{
				this.controlCamera.GetUserCameraPreview().SetCurrentCamera(userCameraObject.GetCSCamera());
				this.controlCamera.GetUserCameraPreview().Rander(true);
				return;
			}
			this.controlCamera.GetUserCameraPreview().Rander(false);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004F20 File Offset: 0x00003120
		private void ComControlObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this.isContorlMoved)
			{
				return;
			}
			if (this.SelectObjectList.Count<VisualObject>() == 0)
			{
				return;
			}
			if (e.PropertyName == "Position3D" || e.PropertyName == "Rotation3D" || e.PropertyName == "Scale3D" || e.PropertyName == "Parent")
			{
				this.InitDumyObject();
			}
		}

		// Token: 0x04000063 RID: 99
		private bool isSelect;

		// Token: 0x04000064 RID: 100
		private bool isActive;

		// Token: 0x04000065 RID: 101
		private CameraObject controlCamera;

		// Token: 0x04000066 RID: 102
		private CSComControlNode3D innerNode;

		// Token: 0x04000067 RID: 103
		private List<VisualObject> selectObjectList = new List<VisualObject>();

		// Token: 0x04000069 RID: 105
		private static ControlNode3D _instance;

		// Token: 0x0400006A RID: 106
		private ControlNode3D.PivotPoint pivotpoint;

		// Token: 0x0400006B RID: 107
		private ControlNode3D.Space space;

		// Token: 0x0400006C RID: 108
		private bool isContorlMoved;

		// Token: 0x0200001C RID: 28
		public enum Opt
		{
			// Token: 0x0400006F RID: 111
			Invalid,
			// Token: 0x04000070 RID: 112
			Translate,
			// Token: 0x04000071 RID: 113
			Rotate,
			// Token: 0x04000072 RID: 114
			Scale,
			// Token: 0x04000073 RID: 115
			Move
		}

		// Token: 0x0200001D RID: 29
		public enum PivotPoint
		{
			// Token: 0x04000075 RID: 117
			Pivot,
			// Token: 0x04000076 RID: 118
			Center
		}

		// Token: 0x0200001E RID: 30
		public enum Space
		{
			// Token: 0x04000078 RID: 120
			Local,
			// Token: 0x04000079 RID: 121
			Global
		}
	}
}
