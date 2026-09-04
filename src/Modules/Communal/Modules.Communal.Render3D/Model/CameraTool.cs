using System;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render;
using Modules.Communal.Render.Model;
using Xwt.Drawing;

namespace Modules.Communal.Render3D.Model
{
	// Token: 0x02000005 RID: 5
	internal class CameraTool : BaseTool, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl, IDocumentEventHandler
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000229B File Offset: 0x0000049B
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Hand.png");
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000022A7 File Offset: 0x000004A7
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Move + " (Q)";
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000022B8 File Offset: 0x000004B8
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.Q;
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022BC File Offset: 0x000004BC
		public override void Initialize()
		{
			this.camera = GameWindow.Current.GetSceneObject().GetCamera();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022D3 File Offset: 0x000004D3
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.RefreshCursor();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022E4 File Offset: 0x000004E4
		private CameraData LoadCameraData(CocosItem cocosItem)
		{
			IUserData userData = null;
			if (!cocosItem.UserData.Properties.TryGetValue("CameraData", out userData))
			{
				userData = new CameraData();
				cocosItem.UserData.Properties["CameraData"] = userData;
			}
			return userData as CameraData;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002330 File Offset: 0x00000530
		private void SaveCameraData(CocosItem cocosItem)
		{
			if (cocosItem == null)
			{
				return;
			}
			this.camera.UpdateProjectCamera();
			cocosItem.UserData.Properties["CameraData"] = cocosItem.GetGameContent().SceneCamera;
			cocosItem.SaveUserData();
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002367 File Offset: 0x00000567
		public override void OnMouseWheel(ScrollEventArgs args)
		{
			this.camera.OnMouseWheel(args.Event.Direction, args.Event.Time);
			args.RetVal = true;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002398 File Offset: 0x00000598
		public override void OnMouseDown(ButtonPressEventArgs args)
		{
			this.mousePoint = args.Event.GetPoint();
			this.defaultCursor = Cursors.Fist;
			this.RefreshCursor();
			if (this.CheckPointInHitRect(args.Event.GetPoint()))
			{
				this.camera.OnChangeCameraView(args.Event.GetPoint());
				args.RetVal = true;
				return;
			}
			if (base.IsSelected || args.Event.GetMouseButton() != MouseButton.Left)
			{
				this.camera.OnMouseDown(args.Event.GetPoint(), args.Event.GetMouseButton());
				args.RetVal = true;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002440 File Offset: 0x00000640
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			this.mousePoint = args.Event.GetPoint();
			this.RefreshCursor();
			if (!this.CheckPointInScene(args.Event.GetPoint()))
			{
				args.RetVal = true;
				return;
			}
			if (base.IsSelected || args.Event.GetMouseButton() != MouseButton.Left)
			{
				this.camera.OnMouseMove(args.Event.GetPoint(), args.Event.GetMouseButton(), base.IsSelected);
				args.RetVal = true;
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024D0 File Offset: 0x000006D0
		public override void OnMouseUp(ButtonReleaseEventArgs args)
		{
			this.mousePoint = args.Event.GetPoint();
			this.defaultCursor = Cursors.Hand;
			this.RefreshCursor();
			if (base.IsSelected || args.Event.GetMouseButton() != MouseButton.Left)
			{
				this.camera.OnMouseUp(args.Event.GetMouseButton());
				args.RetVal = true;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002538 File Offset: 0x00000738
		public override void OnKeyDown(KeyPressEventArgs args)
		{
			Gdk.Key key = args.Event.Key;
			if (key == Gdk.Key.F || key == Gdk.Key.f)
			{
				ControlNode3D.Instance.MoveCameraToSelected();
				args.RetVal = true;
				return;
			}
			bool isGlobal = false;
			CSSceneCamera.MoveFlag moveFlag = CSSceneCamera.MoveFlag.None;
			if (key == Gdk.Key.Up || key == Gdk.Key.Down || key == Gdk.Key.Left || key == Gdk.Key.Right)
			{
				isGlobal = true;
				moveFlag = this.GetGameraMoveFlag(key);
			}
			else if (args.Event.State.HasFlag(ModifierType.Button3Mask))
			{
				moveFlag = this.GetGameraMoveFlag(key);
			}
			if (moveFlag != CSSceneCamera.MoveFlag.None)
			{
				this.camera.AddMoveFlag(moveFlag, isGlobal);
				args.RetVal = true;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025E8 File Offset: 0x000007E8
		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			CSSceneCamera.MoveFlag gameraMoveFlag = this.GetGameraMoveFlag(args.Event.Key);
			this.camera.RemoveMoveFlag(gameraMoveFlag);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002614 File Offset: 0x00000814
		private CSSceneCamera.MoveFlag GetGameraMoveFlag(Gdk.Key key)
		{
			CSSceneCamera.MoveFlag result = CSSceneCamera.MoveFlag.None;
			if (key <= Gdk.Key.W)
			{
				switch (key)
				{
				case Gdk.Key.A:
					goto IL_A6;
				case Gdk.Key.B:
				case Gdk.Key.C:
					return result;
				case Gdk.Key.D:
					goto IL_AA;
				case Gdk.Key.E:
					goto IL_B3;
				default:
					switch (key)
					{
					case Gdk.Key.Q:
						goto IL_AE;
					case Gdk.Key.R:
						return result;
					case Gdk.Key.S:
						goto IL_A2;
					default:
						if (key != Gdk.Key.W)
						{
							return result;
						}
						break;
					}
					break;
				}
			}
			else if (key <= Gdk.Key.s)
			{
				switch (key)
				{
				case Gdk.Key.a:
					goto IL_A6;
				case Gdk.Key.b:
				case Gdk.Key.c:
					return result;
				case Gdk.Key.d:
					goto IL_AA;
				case Gdk.Key.e:
					goto IL_B3;
				default:
					switch (key)
					{
					case Gdk.Key.q:
						goto IL_AE;
					case Gdk.Key.r:
						return result;
					case Gdk.Key.s:
						goto IL_A2;
					default:
						return result;
					}
					break;
				}
			}
			else if (key != Gdk.Key.w)
			{
				switch (key)
				{
				case Gdk.Key.Left:
					goto IL_A6;
				case Gdk.Key.Up:
					break;
				case Gdk.Key.Right:
					goto IL_AA;
				case Gdk.Key.Down:
					goto IL_A2;
				default:
					return result;
				}
			}
			return CSSceneCamera.MoveFlag.Foward;
			IL_A2:
			return CSSceneCamera.MoveFlag.Back;
			IL_A6:
			return CSSceneCamera.MoveFlag.Left;
			IL_AA:
			return CSSceneCamera.MoveFlag.Right;
			IL_AE:
			return CSSceneCamera.MoveFlag.Up;
			IL_B3:
			result = CSSceneCamera.MoveFlag.Down;
			return result;
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026D8 File Offset: 0x000008D8
		public void Initialize(IGLView glView)
		{
			this.Initialize();
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026E0 File Offset: 0x000008E0
		public void OnDocumentChanged(CocosItem cocosItem)
		{
			this.SaveCameraData(this.currentDocument);
			GameFileContent gameContent = cocosItem.GetGameContent();
			gameContent.SceneCamera = this.LoadCameraData(cocosItem);
			this.camera.SetCurrentProject(gameContent);
			this.currentDocument = cocosItem;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002720 File Offset: 0x00000920
		public void OnDocumentSaved(CocosItem cocosItem)
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002722 File Offset: 0x00000922
		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002724 File Offset: 0x00000924
		public void OnDocumentClosed(CocosItem cocosItem)
		{
			this.SaveCameraData(cocosItem);
			this.currentDocument = null;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002734 File Offset: 0x00000934
		public void Activated(CocosItem cocosItem)
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002736 File Offset: 0x00000936
		public void Deactivated()
		{
			this.camera.SetCurrentProject(null);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002744 File Offset: 0x00000944
		public override PointF ConvertCoordinate(PointF point)
		{
			return point;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002748 File Offset: 0x00000948
		private bool CheckPointInScene(PointF scenePoint)
		{
			return scenePoint.X >= 2f && scenePoint.X - 2f <= (float)GameWindow.Current.Width && scenePoint.Y >= 2f && scenePoint.Y - 2f <= (float)GameWindow.Current.Height;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027A4 File Offset: 0x000009A4
		private bool CheckPointInHitRect(PointF scenePoint)
		{
			return scenePoint.X > (float)(GameWindow.Current.Width - 115) && scenePoint.Y < 115f;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000027CC File Offset: 0x000009CC
		private void RefreshCursor()
		{
			if (!base.IsSelected)
			{
				BaseTool.SetCursor(Cursors.Arrow);
				return;
			}
			if (this.CheckPointInHitRect(this.mousePoint))
			{
				BaseTool.SetCursor(Cursors.Arrow);
				return;
			}
			BaseTool.SetCursor(this.defaultCursor);
		}

		// Token: 0x04000005 RID: 5
		private const int validSpace = 2;

		// Token: 0x04000006 RID: 6
		private const int rectWidth = 115;

		// Token: 0x04000007 RID: 7
		private CameraObject camera;

		// Token: 0x04000008 RID: 8
		private PointF mousePoint = PointF.Empty;

		// Token: 0x04000009 RID: 9
		private Cursor defaultCursor = Cursors.Hand;

		// Token: 0x0400000A RID: 10
		private CocosItem currentDocument;
	}
}
