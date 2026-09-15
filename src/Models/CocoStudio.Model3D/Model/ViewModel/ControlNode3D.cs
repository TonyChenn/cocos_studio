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
	public class ControlNode3D : VisualObject
	{
		public event EventHandler CoordinateSystemChanged;

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

		public ControlNode3D()
		{
			SceneObject sceneObject = GameWindow.Current.GetSceneObject();
			this.controlCamera = sceneObject.GetCamera();
			this.innerNode = new CSComControlNode3D();
			this.DumyObject = new Dumy3DObject();
			sceneObject.AddChild(this);
			sceneObject.AddChild(this.DumyObject);
		}

		private CSComControlNode3D GetComControl()
		{
			return this.innerNode;
		}

		internal override CSVisualObject GetCSVisual()
		{
			return this.innerNode;
		}

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

		public Dumy3DObject DumyObject { get; private set; }

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

		public override HitTestResult HitTest(PointF point)
		{
			if (this.isActive && this.GetComControl().OnSelect(point))
			{
				return new HitTestResult(point, this.Visible);
			}
			return null;
		}

		public void MoveCameraToSelected()
		{
			if (this.SelectObjectList.Count == 0)
			{
				return;
			}
			this.controlCamera.MoveTo(this.DumyObject);
		}

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

		private void RefreshSpaceButtonState()
		{
			if (this.CoordinateSystemChanged != null)
			{
				this.CoordinateSystemChanged(this, new EventArgs());
			}
		}

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

		private bool isSelect;

		private bool isActive;

		private CameraObject controlCamera;

		private CSComControlNode3D innerNode;

		private List<VisualObject> selectObjectList = new List<VisualObject>();

		private static ControlNode3D _instance;

		private ControlNode3D.PivotPoint pivotpoint;

		private ControlNode3D.Space space;

		private bool isContorlMoved;

		public enum Opt
		{
			Invalid,
			Translate,
			Rotate,
			Scale,
			Move
		}

		public enum PivotPoint
		{
			Pivot,
			Center
		}

		public enum Space
		{
			Local,
			Global
		}
	}
}
