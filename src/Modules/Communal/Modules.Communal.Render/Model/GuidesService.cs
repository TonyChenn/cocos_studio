using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gdk;
using Gtk;
using Modules.Communal.Render.View;
using MonoDevelop.Components;

namespace Modules.Communal.Render.Model
{
	public class GuidesService : BaseObject, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl, IDocumentEventHandler
	{
		private void InitializeCommand()
		{
			GlobalCommand.GuidesCmd.Execute += this.GuidesCmd_Execute;
			GlobalCommand.GuidesCmd.Update += this.GuidesCmd_Update;
			GlobalCommand.LockGuidesCmd.Execute += this.LockGuidesCmd_Execute;
			GlobalCommand.LockGuidesCmd.Update += this.LockGuidesCmd_Update;
			GlobalCommand.ClearGuidesCmd.Execute += this.ClearGuidesCmd_Execute;
			GlobalCommand.ClearGuidesCmd.Update += this.ClearGuidesCmd_Update;
			GlobalCommand.NewGuidesCmd.Execute += this.NewGuidesCmd_Execute;
			GlobalCommand.NewGuidesCmd.Update += this.NewGuidesCmd_Update;
			GlobalCommand.AbsorptionGuidesCmd.Update += this.AbsorptionGuidesCmd_Update;
			GlobalCommand.AbsorptionGuidesCmd.Execute += this.AbsorptionGuidesCmd_Execute;
		}

		private bool CanShowGuides()
		{
			CocosItem file = Services.Workbench.ActiveDocument.File;
			bool result = true;
			if (file == null || file.CocosFile.Type == NodeType.Plist.ToString() || file.CocosFile.Type == NodeType.Scene3D.ToString())
			{
				result = false;
			}
			return result;
		}

		private void GuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.Visible = !this.Visible;
		}

		private void GuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
			e.Info.Checked = this.Visible;
		}

		private void LockGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.isLock = !this.isLock;
		}

		private void LockGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
			e.Info.Checked = this.isLock;
		}

		private void ClearGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.RemoveAll();
		}

		private void ClearGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = (this.CanShowGuides() && (this.verticalList.Count > 0 || this.horizontalList.Count > 0));
		}

		private void NewGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			NewGuidesDialog newGuidesDialog = new NewGuidesDialog();
			newGuidesDialog.Show();
		}

		private void NewGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
		}

		private void AbsorptionGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.isAbsorption = !this.isAbsorption;
		}

		private void AbsorptionGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
			e.Info.Checked = this.isAbsorption;
		}

		[UndoProperty]
		public GuidesList VerticalList
		{
			get
			{
				return this.verticalList;
			}
			set
			{
				this.verticalList = value;
			}
		}

		[UndoProperty]
		public GuidesList HorizontalList
		{
			get
			{
				return this.horizontalList;
			}
			set
			{
				this.horizontalList = value;
			}
		}

		public CanvasObject CanvasObject { get; private set; }

		public bool IsShowRuler
		{
			get
			{
				return this.isShowRuler;
			}
			set
			{
				this.isShowRuler = value;
			}
		}

		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
				this.innerService.SetVisible(value);
			}
		}

		public System.Drawing.Color LineColor
		{
			get
			{
				return this.lineColor;
			}
			set
			{
				this.lineColor = value;
				if (this.innerService != null)
				{
					this.innerService.SetColor(value);
				}
			}
		}

		private GuidesService()
		{
		}

		public static GuidesService Instance { get; set; } = new GuidesService();

		public void Initialize(IGLView glView)
		{
			if (!GuidesService.isInitialized)
			{
				GuidesService.isInitialized = true;
				this.innerService = CSGuidesService.GetInstance();
				this.glView = glView;
				this.CanvasObject = glView.GameWindow.GetCanvasObject();
				this.verticalList = new GuidesList(this.innerService);
				this.horizontalList = new GuidesList(this.innerService);
				this.InitializeCommand();
				if (!string.IsNullOrEmpty(Option.UserConfig.GuidesColor))
				{
					int argb;
					bool flag = int.TryParse(Option.UserConfig.GuidesColor, out argb);
					if (flag)
					{
						this.LineColor = System.Drawing.Color.FromArgb(argb);
					}
				}
				base.BindingRecorder(null);
			}
		}

		public void Add(GuidesObject line)
		{
			GuidesList guidesList;
			if (line.Direction == Orientation.Horizontal)
			{
				guidesList = this.horizontalList;
			}
			else
			{
				guidesList = this.verticalList;
			}
			guidesList.Add(line);
		}

		public void Sort()
		{
			this.horizontalList.Sort();
			this.verticalList.Sort();
		}

		public void Remove(GuidesObject line)
		{
			if (line.Direction == Orientation.Horizontal)
			{
				this.horizontalList.Remove(line);
			}
			else
			{
				this.verticalList.Remove(line);
			}
		}

		public void RemoveAll()
		{
			using (CompositeTask.Run("RemoveAll Guides", null))
			{
				for (int i = this.horizontalList.Count - 1; i >= 0; i--)
				{
					this.horizontalList.Remove(this.horizontalList[i]);
				}
				for (int i = this.verticalList.Count - 1; i >= 0; i--)
				{
					this.verticalList.Remove(this.verticalList[i]);
				}
			}
		}

		public void Clear()
		{
			this.verticalList.Clear();
			this.horizontalList.Clear();
		}

		public void DragNewGuides(GuidesObject line)
		{
			this.SetCursor(line);
			this.lastHitLine = line;
			this.AddHoldLine(0.0, 0.0);
		}

		public void DragFinished(bool isDeleted)
		{
			if (this.lastHitLine != null)
			{
				if (!isDeleted)
				{
					this.lastHitLine.Position = this.holdLine.Position;
					this.lastHitLine.Direction = this.holdLine.Direction;
					this.Add(this.lastHitLine);
				}
				this.MoveFinished();
			}
		}

		internal List<DockGuidesResult> GetDockGuides(RectF rect)
		{
			List<DockGuidesResult> list = new List<DockGuidesResult>(2);
			DockGuidesResult dockGuides = GuidesHitTestHelp.GetDockGuides(this.horizontalList, rect.Top);
			DockGuidesResult dockGuides2 = GuidesHitTestHelp.GetDockGuides(this.horizontalList, rect.Bottom);
			this.CompareDockResult(list, dockGuides, dockGuides2);
			DockGuidesResult dockGuides3 = GuidesHitTestHelp.GetDockGuides(this.verticalList, rect.Left);
			DockGuidesResult dockGuides4 = GuidesHitTestHelp.GetDockGuides(this.verticalList, rect.Right);
			this.CompareDockResult(list, dockGuides4, dockGuides3);
			return list;
		}

		private void CompareDockResult(List<DockGuidesResult> list, DockGuidesResult forwardResult, DockGuidesResult reverseResult)
		{
			DockGuidesResult dockGuidesResult = null;
			if (forwardResult != null && reverseResult != null)
			{
				dockGuidesResult = ((forwardResult.Distance < reverseResult.Distance) ? forwardResult : reverseResult);
			}
			else if (forwardResult != null)
			{
				dockGuidesResult = forwardResult;
			}
			else if (reverseResult != null)
			{
				dockGuidesResult = reverseResult;
			}
			if (dockGuidesResult != null)
			{
				list.Add(dockGuidesResult);
			}
		}

		private void MoveLine(MotionNotifyEventArgs args)
		{
			this.MoveHoldLine(args);
			this.MoveTooltip(args);
		}

		private void MoveHoldLine(MotionNotifyEventArgs args)
		{
			int num;
			int num2;
			args.Event.Window.GetOrigin(out num, out num2);
			Gdk.Point p = new Gdk.Point(num + (int)args.Event.X, num2 + (int)args.Event.Y);
			CocoStudio.Model.PointF sencePoint = this.glView.ConvertScreenToScene(p);
			CocoStudio.Model.PointF pointF = this.glView.GameWindow.GetCanvasObject().TransformToSelf(sencePoint);
			float position = pointF.X;
			if (this.holdLine.Direction == Orientation.Horizontal)
			{
				position = pointF.Y;
			}
			this.holdLine.Position = position;
			this.lastMousePosition = pointF;
		}

		private void MoveTooltip(MotionNotifyEventArgs args)
		{
			this.tipWindow.Text = this.holdLine.ToString();
			this.tipWindow.MoveWindow(this.glView as Widget, args.Event.Window, args.Event.X, args.Event.Y);
		}

		private void MoveFinished()
		{
			if (this.lastHitLine != null)
			{
				this.lastHitLine = null;
				this.lastMousePosition = null;
				this.holdLine = null;
				this.innerService.SetHoldGuides(null);
				this.tipWindow.Visible = false;
				this.tipWindow = null;
			}
		}

		private GuidesObject HitTest(double x, double y)
		{
			CocoStudio.Model.PointF sencePoint = this.glView.ConvertControlToScene(new CocoStudio.Model.PointF((float)x, (float)y));
			CocoStudio.Model.PointF pointF = this.CanvasObject.TransformToSelf(sencePoint);
			GuidesObject guidesObject = GuidesHitTestHelp.HitTest(this.verticalList, pointF.X);
			GuidesObject result;
			if (guidesObject != null)
			{
				result = guidesObject;
			}
			else
			{
				guidesObject = GuidesHitTestHelp.HitTest(this.horizontalList, pointF.Y);
				result = guidesObject;
			}
			return result;
		}

		private void SetCursor(GuidesObject line)
		{
			if (line == null)
			{
				this.glView.Cursor = Cursors.Arrow;
			}
			else
			{
				switch (line.Direction)
				{
				case Orientation.Horizontal:
					this.glView.Cursor = Cursors.HorizontalCursor;
					break;
				case Orientation.Vertical:
					this.glView.Cursor = Cursors.VerticalCursor;
					break;
				}
			}
		}

		private void AddHoldLine(double x, double y)
		{
			this.holdLine = (this.lastHitLine.Clone() as GuidesObject);
			this.innerService.SetHoldGuides(this.holdLine.GetNode());
			this.tipWindow = new GuidesTipWindow();
			this.tipWindow.HeightRequest = 24;
			int num = this.IsShowRuler ? -8 : 10;
			this.tipWindow.ShowPopup(this.glView as Widget, new Gdk.Rectangle((int)x + num, (int)y + 10, 0, 0), PopupPosition.Left);
			this.tipWindow.Text = this.holdLine.ToString();
		}

		private void RemoveHoldLine()
		{
			if (this.lastHitLine != null && this.holdLine != null)
			{
				using (CompositeTask.Run("Move guides.", null))
				{
					this.Remove(this.lastHitLine);
					this.lastHitLine.Position = this.holdLine.Position;
					this.lastHitLine.Direction = this.holdLine.Direction;
					this.Add(this.lastHitLine);
				}
			}
		}

		public void OnMouseMove(MotionNotifyEventArgs args)
		{
			if (args.Event.State.HasFlag(ModifierType.Button1Mask))
			{
				if (this.lastHitLine != null)
				{
					this.MoveLine(args);
					args.RetVal = true;
				}
				else
				{
					args.RetVal = false;
				}
			}
			else if (!this.isLock)
			{
				this.MoveFinished();
				GuidesObject guidesObject = this.HitTest(args.Event.X, args.Event.Y);
				this.SetCursor(guidesObject);
				args.RetVal = (guidesObject != null);
			}
		}

		public void OnMouseUp(ButtonReleaseEventArgs args)
		{
			if (!this.isLock && this.lastHitLine != null)
			{
				bool flag = args.Event.X < 0.0 || args.Event.X > (double)this.glView.ActualWidth || args.Event.Y < 0.0 || args.Event.Y > (double)this.glView.ActualHeight;
				if (flag)
				{
					this.Remove(this.lastHitLine);
				}
				else
				{
					this.RemoveHoldLine();
				}
				this.MoveFinished();
				args.RetVal = true;
			}
		}

		public void OnMouseDown(ButtonPressEventArgs args)
		{
			if (!this.isLock)
			{
				GuidesObject guidesObject = this.HitTest(args.Event.X, args.Event.Y);
				this.SetCursor(guidesObject);
				if (guidesObject != null)
				{
					this.lastHitLine = guidesObject;
					this.AddHoldLine(args.Event.X, args.Event.Y + 12.0);
					args.RetVal = true;
				}
				else
				{
					args.RetVal = false;
				}
			}
		}

		public void OnMouseEnter(EnterNotifyEventArgs args)
		{
		}

		public void OnMouseLeave(LeaveNotifyEventArgs args)
		{
		}

		public void OnMouseWheel(ScrollEventArgs args)
		{
		}

		public void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
		}

		public void OnMouseGestures(MouseGesturesEventArgs args)
		{
		}

		public void OnKeyDown(KeyPressEventArgs args)
		{
			if (this.holdLine != null)
			{
				if (args.Event.Key == Gdk.Key.Alt_L || args.Event.Key == Gdk.Key.Alt_R)
				{
					this.RotateLine(true);
					args.RetVal = true;
				}
			}
		}

		public void OnKeyUp(KeyReleaseEventArgs args)
		{
			if (this.holdLine != null)
			{
				if (args.Event.Key == Gdk.Key.Alt_L || args.Event.Key == Gdk.Key.Alt_R)
				{
					this.RotateLine(false);
					args.RetVal = true;
				}
			}
		}

		private void RotateLine(bool isDown)
		{
			if (!(this.lastMousePosition == null))
			{
				if ((this.lastHitLine.Direction == this.holdLine.Direction && isDown) || (this.lastHitLine.Direction != this.holdLine.Direction && !isDown))
				{
					if (this.holdLine.Direction == Orientation.Horizontal)
					{
						this.holdLine.Direction = Orientation.Vertical;
						this.holdLine.Position = this.lastMousePosition.X;
					}
					else
					{
						this.holdLine.Direction = Orientation.Horizontal;
						this.holdLine.Position = this.lastMousePosition.Y;
					}
					this.tipWindow.Text = this.holdLine.ToString();
					this.SetCursor(this.holdLine);
				}
			}
		}

		public void Activated(CocosItem cocosItem)
		{
			if (this.Visible && this.innerService != null)
			{
				this.innerService.SetVisible(true);
			}
		}

		public void Deactivated()
		{
			if (this.innerService != null)
			{
				this.innerService.SetVisible(false);
			}
		}

		private void SaveGuides()
		{
			if (this.currentCocosItem != null)
			{
				GuidesData guidesData = this.currentCocosItem.UserData.Properties["GuidesList"] as GuidesData;
				guidesData.HorizontalList = this.horizontalList.ToList<GuidesObject>();
				guidesData.VerticalList = this.verticalList.ToList<GuidesObject>();
			}
		}

		private void LoadGuides()
		{
			this.Clear();
			if (this.currentCocosItem != null)
			{
				IUserData userData = null;
				if (!this.currentCocosItem.UserData.Properties.TryGetValue("GuidesList", out userData))
				{
					userData = new GuidesData();
					this.currentCocosItem.UserData.Properties["GuidesList"] = userData;
				}
				GuidesData guidesData = userData as GuidesData;
				this.verticalList.AddRange(guidesData.VerticalList);
				this.horizontalList.AddRange(guidesData.HorizontalList);
			}
		}

		public void OnDocumentChanged(CocosItem cocosItem)
		{
			if (cocosItem != this.currentCocosItem)
			{
				this.SaveGuides();
				this.currentCocosItem = cocosItem;
				this.LoadGuides();
			}
		}

		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
			this.SaveGuides();
		}

		public void OnDocumentSaved(CocosItem cocosItem)
		{
		}

		public void OnDocumentClosed(CocosItem cocosItem)
		{
			if (cocosItem == this.currentCocosItem)
			{
				this.Clear();
				this.currentCocosItem = null;
			}
		}

		private static bool isInitialized;

		private IGLView glView;

		private CSGuidesService innerService;

		private CocoStudio.Model.PointF lastMousePosition = null;

		private GuidesTipWindow tipWindow;

		private bool isLock = false;

		private CocosItem currentCocosItem;

		private GuidesObject lastHitLine;

		private GuidesObject holdLine;

		private GuidesList verticalList;

		private GuidesList horizontalList;

		private bool isShowRuler = true;

		private bool isAbsorption = true;

		private bool visible = true;

		private System.Drawing.Color lineColor;
	}
}
