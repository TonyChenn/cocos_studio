using System;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using Gdk;
using Gtk;

namespace CocoStudio.Model.ViewModel
{
	public class CameraObject : VisualObject
	{
		internal override CSVisualObject GetCSVisual()
		{
			return this.csCamera;
		}

		public CameraObject(CSSceneCamera camera)
		{
			this.csCamera = camera;
			camera.SetMoveSpeed(10f, false);
			camera.SetMoveAccelerate(16f);
			this.userCameraPreview = new UserCameraPreviewObject(camera.GetUserCameraPreview());
		}

		public CSSkyBox InnerSkyBox
		{
			get
			{
				return this.csCamera.GetSkyBox();
			}
			set
			{
				this.csCamera.SetSkyBox(value);
			}
		}

		public float Distance
		{
			get
			{
				return (this.distance > 0f) ? this.distance : (-this.distance);
			}
		}

		public CSComControlNode3D controlNode { get; set; }

		public void UpdateSelectFrustum(RectF rect)
		{
			this.csCamera.UpdateFrustumPlane(rect);
		}

		public System.Drawing.Color GetPickColor(PointF point)
		{
			return this.csCamera.GetPickColor(point);
		}

		public void MoveTo(VisualObject control)
		{
			this.csCamera.MoveTo(control.GetCSVisual());
		}

		public Point3F ConvertControlToWorld3D(PointF screen, float distance)
		{
			return this.csCamera.ScreenToWorld(screen, distance);
		}

		public void SetCurrentProject(GameFileContent game)
		{
			if (this.project != game)
			{
				this.UpdateProjectCamera();
				this.project = game;
			}
			if (game != null)
			{
				CameraData sceneCamera = game.SceneCamera;
				if (sceneCamera != null && sceneCamera.Position != null && sceneCamera.Rotation != null)
				{
					this.csCamera.SetPosition3D(sceneCamera.Position);
					this.csCamera.SetRotation3D(sceneCamera.Rotation);
				}
				else
				{
					Point3F position3D = new Point3F(6f, 6f, 6f);
					Point3F rotation3D = new Point3F(-30f, 45f, 0f);
					this.csCamera.SetPosition3D(position3D);
					this.csCamera.SetRotation3D(rotation3D);
				}
			}
			this.GetUserCameraPreview().Rander(false);
		}

		public void UpdateProjectCamera()
		{
			if (this.project != null)
			{
				this.project.SceneCamera = new CameraData(this.csCamera.GetPosition3D(), this.csCamera.GetRotation3D());
			}
		}

		public UserCameraPreviewObject GetUserCameraPreview()
		{
			return this.userCameraPreview;
		}

		public void OnChangeCameraView(PointF screenPoint)
		{
			this.csCamera.OnChangeViewpoint(screenPoint);
		}

		public void SetRectDrawNode(DrawNodeObject drawNode)
		{
			this.csCamera.SetRectDrawNode(drawNode.GetCSVisual() as CSDrawNode);
		}

		public bool isMouseButtonDown(MouseButton button)
		{
			return button >= (MouseButton)0 && (this.buttonFlag & 1U << (int)button) != 0U;
		}

		private void UpdateCamera()
		{
			if (this.project != null)
			{
				if (this.controlNode != null)
				{
					this.controlNode.RefreshSelectable();
				}
			}
		}

		public void OnMouseMove(PointF point, MouseButton e, bool isCameraMoveMode)
		{
			if (this.isMouseButtonDown(MouseButton.Right))
			{
				SizeF screenSize = this.csCamera.GetScreenSize();
				float fov = this.csCamera.GetFov();
				float degree = (this.mousePoint.X - point.X) / screenSize.Width * fov * 3.1415927f / 180f;
				float degree2 = (this.mousePoint.Y - point.Y) / screenSize.Height * fov * 3.1415927f / 180f;
				this.csCamera.Rotate3D(Point3F.UnitY, degree, CSVisualObject.TransformSpace.TS_WORLD);
				this.csCamera.Rotate3D(Point3F.UnitX, degree2, CSVisualObject.TransformSpace.TS_LOCAL);
				this.UpdateCamera();
			}
			else if (this.isMouseButtonDown(MouseButton.Middle) || (this.isMouseButtonDown(MouseButton.Left) && isCameraMoveMode))
			{
				Point3F pz = this.csCamera.ScreenToWorld(point, this.Distance);
				Point3F point3F = this.csCamera.ScreenToWorld(this.mousePoint, this.Distance);
				point3F.Subtract(pz);
				this.csCamera.MoveRelative(point3F, CSVisualObject.TransformSpace.TS_WORLD);
				this.UpdateCamera();
			}
			else if (this.isMouseButtonDown(MouseButton.Left) && !isCameraMoveMode)
			{
				if (KeyboardExtend.IsModifyKeyPressed(ModifierType.Mod1Mask))
				{
					SizeF screenSize = this.csCamera.GetScreenSize();
					float fov = this.csCamera.GetFov();
					float degree = (this.mousePoint.X - point.X) / screenSize.Width * fov * 3.1415927f / 180f;
					float degree2 = (this.mousePoint.Y - point.Y) / screenSize.Height * fov * 3.1415927f / 180f;
					Point3F lookAt = this.csCamera.GetLookAt();
					this.csCamera.MoveRelative(new Point3F(0f, 0f, -this.distance), CSVisualObject.TransformSpace.TS_LOCAL);
					this.csCamera.Rotate3D(Point3F.UnitY, degree, CSVisualObject.TransformSpace.TS_WORLD);
					this.csCamera.Rotate3D(Point3F.UnitX, degree2, CSVisualObject.TransformSpace.TS_LOCAL);
					this.csCamera.MoveRelative(new Point3F(0f, 0f, this.distance), CSVisualObject.TransformSpace.TS_LOCAL);
					this.UpdateCamera();
				}
			}
			this.mousePoint.X = point.X;
			this.mousePoint.Y = point.Y;
		}

		public void OnMouseWheel(ScrollDirection direction, uint time)
		{
			if (this.wheelTime != 0U && time - this.wheelTime > 500U)
			{
				if (this.distance < 0f)
				{
					this.distance = -this.distance;
				}
			}
			switch (direction)
			{
			case ScrollDirection.Up:
				this.distance -= 1f;
				this.csCamera.MoveDirect(1f);
				this.wheelTime = time;
				break;
			case ScrollDirection.Down:
				this.distance += 1f;
				this.csCamera.MoveDirect(-1f);
				this.wheelTime = time;
				break;
			}
			if (Math.Abs(this.distance) > 50f)
			{
				this.distance = ((this.distance > 0f) ? 50f : -50f);
			}
			this.UpdateCamera();
		}

		public void OnMouseDown(PointF point, MouseButton mouseButton)
		{
			this.mousePoint.X = point.X;
			this.mousePoint.Y = point.Y;
			this.csCamera.StartMove();
			if (mouseButton > (MouseButton)0)
			{
				this.buttonFlag |= 1U << (int)mouseButton;
			}
			if (mouseButton == MouseButton.Right)
			{
				this.csCamera.StartMove();
				this.UpdateCamera();
			}
		}

		public void OnMouseUp(MouseButton mouseButton)
		{
			if (mouseButton > (MouseButton)0)
			{
				this.buttonFlag &= ~(1U << (int)mouseButton);
			}
			if (mouseButton == MouseButton.Right)
			{
				this.csCamera.Paused();
				this.UpdateCamera();
			}
		}

		public void AddMoveFlag(CSSceneCamera.MoveFlag mFlag, bool isGlobal)
		{
			this.csCamera.AddMoveFlag((ushort)mFlag, isGlobal);
		}

		public void RemoveMoveFlag(CSSceneCamera.MoveFlag mFlag)
		{
			this.csCamera.RemoveFlag((ushort)mFlag);
		}

		private const float zoomFactor = 1f;

		private const float MaxDistance = 50f;

		private CSSceneCamera csCamera;

		private PointF mousePoint = new PointF();

		private float distance = 10f;

		private uint wheelTime = 0U;

		private uint buttonFlag = 0U;

		private GameFileContent project;

		private UserCameraPreviewObject userCameraPreview;
	}
}
