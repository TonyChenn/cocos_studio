using System;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using Gdk;
using Gtk;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000EE RID: 238
	public class CameraObject : VisualObject
	{
		// Token: 0x060007CD RID: 1997 RVA: 0x0001F170 File Offset: 0x0001D370
		internal override CSVisualObject GetCSVisual()
		{
			return this.csCamera;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0001F188 File Offset: 0x0001D388
		public CameraObject(CSSceneCamera camera)
		{
			this.csCamera = camera;
			camera.SetMoveSpeed(10f, false);
			camera.SetMoveAccelerate(16f);
			this.userCameraPreview = new UserCameraPreviewObject(camera.GetUserCameraPreview());
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x060007CF RID: 1999 RVA: 0x0001F1F4 File Offset: 0x0001D3F4
		// (set) Token: 0x060007D0 RID: 2000 RVA: 0x0001F211 File Offset: 0x0001D411
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

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x0001F224 File Offset: 0x0001D424
		public float Distance
		{
			get
			{
				return (this.distance > 0f) ? this.distance : (-this.distance);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0001F254 File Offset: 0x0001D454
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0001F26B File Offset: 0x0001D46B
		public CSComControlNode3D controlNode { get; set; }

		// Token: 0x060007D4 RID: 2004 RVA: 0x0001F274 File Offset: 0x0001D474
		public void UpdateSelectFrustum(RectF rect)
		{
			this.csCamera.UpdateFrustumPlane(rect);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x0001F284 File Offset: 0x0001D484
		public System.Drawing.Color GetPickColor(PointF point)
		{
			return this.csCamera.GetPickColor(point);
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x0001F2A2 File Offset: 0x0001D4A2
		public void MoveTo(VisualObject control)
		{
			this.csCamera.MoveTo(control.GetCSVisual());
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x0001F2B8 File Offset: 0x0001D4B8
		public Point3F ConvertControlToWorld3D(PointF screen, float distance)
		{
			return this.csCamera.ScreenToWorld(screen, distance);
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x0001F2D8 File Offset: 0x0001D4D8
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

		// Token: 0x060007D9 RID: 2009 RVA: 0x0001F3B0 File Offset: 0x0001D5B0
		public void UpdateProjectCamera()
		{
			if (this.project != null)
			{
				this.project.SceneCamera = new CameraData(this.csCamera.GetPosition3D(), this.csCamera.GetRotation3D());
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x0001F3F4 File Offset: 0x0001D5F4
		public UserCameraPreviewObject GetUserCameraPreview()
		{
			return this.userCameraPreview;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x0001F40C File Offset: 0x0001D60C
		public void OnChangeCameraView(PointF screenPoint)
		{
			this.csCamera.OnChangeViewpoint(screenPoint);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x0001F41C File Offset: 0x0001D61C
		public void SetRectDrawNode(DrawNodeObject drawNode)
		{
			this.csCamera.SetRectDrawNode(drawNode.GetCSVisual() as CSDrawNode);
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x0001F438 File Offset: 0x0001D638
		public bool isMouseButtonDown(MouseButton button)
		{
			return button >= (MouseButton)0 && (this.buttonFlag & 1U << (int)button) != 0U;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001F470 File Offset: 0x0001D670
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

		// Token: 0x060007DF RID: 2015 RVA: 0x0001F4AC File Offset: 0x0001D6AC
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

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001F71C File Offset: 0x0001D91C
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

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001F81C File Offset: 0x0001DA1C
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

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001F89C File Offset: 0x0001DA9C
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

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001F8EB File Offset: 0x0001DAEB
		public void AddMoveFlag(CSSceneCamera.MoveFlag mFlag, bool isGlobal)
		{
			this.csCamera.AddMoveFlag((ushort)mFlag, isGlobal);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001F8FD File Offset: 0x0001DAFD
		public void RemoveMoveFlag(CSSceneCamera.MoveFlag mFlag)
		{
			this.csCamera.RemoveFlag((ushort)mFlag);
		}

		// Token: 0x0400031D RID: 797
		private const float zoomFactor = 1f;

		// Token: 0x0400031E RID: 798
		private const float MaxDistance = 50f;

		// Token: 0x0400031F RID: 799
		private CSSceneCamera csCamera;

		// Token: 0x04000320 RID: 800
		private PointF mousePoint = new PointF();

		// Token: 0x04000321 RID: 801
		private float distance = 10f;

		// Token: 0x04000322 RID: 802
		private uint wheelTime = 0U;

		// Token: 0x04000323 RID: 803
		private uint buttonFlag = 0U;

		// Token: 0x04000324 RID: 804
		private GameFileContent project;

		// Token: 0x04000325 RID: 805
		private UserCameraPreviewObject userCameraPreview;
	}
}
