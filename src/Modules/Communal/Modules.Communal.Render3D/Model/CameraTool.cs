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
	internal class CameraTool : BaseTool, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl, IDocumentEventHandler
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Hand.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool_Move + " (Q)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.Q;
			}
		}

		public override void Initialize()
		{
			this.camera = GameWindow.Current.GetSceneObject().GetCamera();
		}

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			this.RefreshCursor();
		}

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

		public override void OnMouseWheel(ScrollEventArgs args)
		{
			this.camera.OnMouseWheel(args.Event.Direction, args.Event.Time);
			args.RetVal = true;
		}

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

		public override void OnKeyUp(KeyReleaseEventArgs args)
		{
			CSSceneCamera.MoveFlag gameraMoveFlag = this.GetGameraMoveFlag(args.Event.Key);
			this.camera.RemoveMoveFlag(gameraMoveFlag);
		}

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

		public void Initialize(IGLView glView)
		{
			this.Initialize();
		}

		public void OnDocumentChanged(CocosItem cocosItem)
		{
			this.SaveCameraData(this.currentDocument);
			GameFileContent gameContent = cocosItem.GetGameContent();
			gameContent.SceneCamera = this.LoadCameraData(cocosItem);
			this.camera.SetCurrentProject(gameContent);
			this.currentDocument = cocosItem;
		}

		public void OnDocumentSaved(CocosItem cocosItem)
		{
		}

		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
		}

		public void OnDocumentClosed(CocosItem cocosItem)
		{
			this.SaveCameraData(cocosItem);
			this.currentDocument = null;
		}

		public void Activated(CocosItem cocosItem)
		{
		}

		public void Deactivated()
		{
			this.camera.SetCurrentProject(null);
		}

		public override PointF ConvertCoordinate(PointF point)
		{
			return point;
		}

		private bool CheckPointInScene(PointF scenePoint)
		{
			return scenePoint.X >= 2f && scenePoint.X - 2f <= (float)GameWindow.Current.Width && scenePoint.Y >= 2f && scenePoint.Y - 2f <= (float)GameWindow.Current.Height;
		}

		private bool CheckPointInHitRect(PointF scenePoint)
		{
			return scenePoint.X > (float)(GameWindow.Current.Width - 115) && scenePoint.Y < 115f;
		}

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

		private const int validSpace = 2;

		private const int rectWidth = 115;

		private CameraObject camera;

		private PointF mousePoint = PointF.Empty;

		private Cursor defaultCursor = Cursors.Hand;

		private CocosItem currentDocument;
	}
}
