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
	// Token: 0x0200002A RID: 42
	public class GuidesService : BaseObject, IOperateModule, IInputEventHandler, IMouseEventHandler, IKeyEventHandler, IActivateControl, IDocumentEventHandler
	{
		// Token: 0x06000155 RID: 341 RVA: 0x0000886C File Offset: 0x00006A6C
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

		// Token: 0x06000156 RID: 342 RVA: 0x00008960 File Offset: 0x00006B60
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

		// Token: 0x06000157 RID: 343 RVA: 0x000089CD File Offset: 0x00006BCD
		private void GuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.Visible = !this.Visible;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000089E0 File Offset: 0x00006BE0
		private void GuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
			e.Info.Checked = this.Visible;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00008A07 File Offset: 0x00006C07
		private void LockGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.isLock = !this.isLock;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00008A19 File Offset: 0x00006C19
		private void LockGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
			e.Info.Checked = this.isLock;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00008A40 File Offset: 0x00006C40
		private void ClearGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.RemoveAll();
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00008A4A File Offset: 0x00006C4A
		private void ClearGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = (this.CanShowGuides() && (this.verticalList.Count > 0 || this.horizontalList.Count > 0));
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00008A88 File Offset: 0x00006C88
		private void NewGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			NewGuidesDialog newGuidesDialog = new NewGuidesDialog();
			newGuidesDialog.Show();
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00008AA3 File Offset: 0x00006CA3
		private void NewGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00008AB8 File Offset: 0x00006CB8
		private void AbsorptionGuidesCmd_Execute(object sender, CommandRunArgs e)
		{
			this.isAbsorption = !this.isAbsorption;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00008ACA File Offset: 0x00006CCA
		private void AbsorptionGuidesCmd_Update(object sender, CommandUpdateArgs e)
		{
			e.Info.Enabled = this.CanShowGuides();
			e.Info.Checked = this.isAbsorption;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00008AF4 File Offset: 0x00006CF4
		// (set) Token: 0x06000162 RID: 354 RVA: 0x00008B0C File Offset: 0x00006D0C
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

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00008B18 File Offset: 0x00006D18
		// (set) Token: 0x06000164 RID: 356 RVA: 0x00008B30 File Offset: 0x00006D30
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

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00008B3C File Offset: 0x00006D3C
		// (set) Token: 0x06000166 RID: 358 RVA: 0x00008B53 File Offset: 0x00006D53
		public CanvasObject CanvasObject { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00008B5C File Offset: 0x00006D5C
		// (set) Token: 0x06000168 RID: 360 RVA: 0x00008B74 File Offset: 0x00006D74
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

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00008B80 File Offset: 0x00006D80
		// (set) Token: 0x0600016A RID: 362 RVA: 0x00008B98 File Offset: 0x00006D98
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

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600016B RID: 363 RVA: 0x00008BB0 File Offset: 0x00006DB0
		// (set) Token: 0x0600016C RID: 364 RVA: 0x00008BC8 File Offset: 0x00006DC8
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

		// Token: 0x0600016E RID: 366 RVA: 0x00008C05 File Offset: 0x00006E05
		private GuidesService()
		{
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600016F RID: 367 RVA: 0x00008C34 File Offset: 0x00006E34
		// (set) Token: 0x06000170 RID: 368 RVA: 0x00008C4A File Offset: 0x00006E4A
		public static GuidesService Instance { get; set; } = new GuidesService();

		// Token: 0x06000171 RID: 369 RVA: 0x00008C54 File Offset: 0x00006E54
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

		// Token: 0x06000172 RID: 370 RVA: 0x00008D0C File Offset: 0x00006F0C
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

		// Token: 0x06000173 RID: 371 RVA: 0x00008D48 File Offset: 0x00006F48
		public void Sort()
		{
			this.horizontalList.Sort();
			this.verticalList.Sort();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00008D64 File Offset: 0x00006F64
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

		// Token: 0x06000175 RID: 373 RVA: 0x00008DA4 File Offset: 0x00006FA4
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

		// Token: 0x06000176 RID: 374 RVA: 0x00008E58 File Offset: 0x00007058
		public void Clear()
		{
			this.verticalList.Clear();
			this.horizontalList.Clear();
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008E73 File Offset: 0x00007073
		public void DragNewGuides(GuidesObject line)
		{
			this.SetCursor(line);
			this.lastHitLine = line;
			this.AddHoldLine(0.0, 0.0);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008EA0 File Offset: 0x000070A0
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

		// Token: 0x06000179 RID: 377 RVA: 0x00008F0C File Offset: 0x0000710C
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

		// Token: 0x0600017A RID: 378 RVA: 0x00008F88 File Offset: 0x00007188
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

		// Token: 0x0600017B RID: 379 RVA: 0x00008FE5 File Offset: 0x000071E5
		private void MoveLine(MotionNotifyEventArgs args)
		{
			this.MoveHoldLine(args);
			this.MoveTooltip(args);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008FF8 File Offset: 0x000071F8
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

		// Token: 0x0600017D RID: 381 RVA: 0x000090A8 File Offset: 0x000072A8
		private void MoveTooltip(MotionNotifyEventArgs args)
		{
			this.tipWindow.Text = this.holdLine.ToString();
			this.tipWindow.MoveWindow(this.glView as Widget, args.Event.Window, args.Event.X, args.Event.Y);
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00009108 File Offset: 0x00007308
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

		// Token: 0x0600017F RID: 383 RVA: 0x00009160 File Offset: 0x00007360
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

		// Token: 0x06000180 RID: 384 RVA: 0x000091C8 File Offset: 0x000073C8
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

		// Token: 0x06000181 RID: 385 RVA: 0x00009234 File Offset: 0x00007434
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

		// Token: 0x06000182 RID: 386 RVA: 0x000092D8 File Offset: 0x000074D8
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

		// Token: 0x06000183 RID: 387 RVA: 0x00009380 File Offset: 0x00007580
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

		// Token: 0x06000184 RID: 388 RVA: 0x00009440 File Offset: 0x00007640
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

		// Token: 0x06000185 RID: 389 RVA: 0x00009508 File Offset: 0x00007708
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

		// Token: 0x06000186 RID: 390 RVA: 0x0000959E File Offset: 0x0000779E
		public void OnMouseEnter(EnterNotifyEventArgs args)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x000095A1 File Offset: 0x000077A1
		public void OnMouseLeave(LeaveNotifyEventArgs args)
		{
		}

		// Token: 0x06000188 RID: 392 RVA: 0x000095A4 File Offset: 0x000077A4
		public void OnMouseWheel(ScrollEventArgs args)
		{
		}

		// Token: 0x06000189 RID: 393 RVA: 0x000095A7 File Offset: 0x000077A7
		public void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000095AA File Offset: 0x000077AA
		public void OnMouseGestures(MouseGesturesEventArgs args)
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000095B0 File Offset: 0x000077B0
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

		// Token: 0x0600018C RID: 396 RVA: 0x00009614 File Offset: 0x00007814
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

		// Token: 0x0600018D RID: 397 RVA: 0x00009678 File Offset: 0x00007878
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

		// Token: 0x0600018E RID: 398 RVA: 0x0000976C File Offset: 0x0000796C
		public void Activated(CocosItem cocosItem)
		{
			if (this.Visible && this.innerService != null)
			{
				this.innerService.SetVisible(true);
			}
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000097A0 File Offset: 0x000079A0
		public void Deactivated()
		{
			if (this.innerService != null)
			{
				this.innerService.SetVisible(false);
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000097C8 File Offset: 0x000079C8
		private void SaveGuides()
		{
			if (this.currentCocosItem != null)
			{
				GuidesData guidesData = this.currentCocosItem.UserData.Properties["GuidesList"] as GuidesData;
				guidesData.HorizontalList = this.horizontalList.ToList<GuidesObject>();
				guidesData.VerticalList = this.verticalList.ToList<GuidesObject>();
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000982C File Offset: 0x00007A2C
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

		// Token: 0x06000192 RID: 402 RVA: 0x000098C4 File Offset: 0x00007AC4
		public void OnDocumentChanged(CocosItem cocosItem)
		{
			if (cocosItem != this.currentCocosItem)
			{
				this.SaveGuides();
				this.currentCocosItem = cocosItem;
				this.LoadGuides();
			}
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000098F9 File Offset: 0x00007AF9
		public void OnDocumentBeforeSave(CocosItem cocosItem)
		{
			this.SaveGuides();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009903 File Offset: 0x00007B03
		public void OnDocumentSaved(CocosItem cocosItem)
		{
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009908 File Offset: 0x00007B08
		public void OnDocumentClosed(CocosItem cocosItem)
		{
			if (cocosItem == this.currentCocosItem)
			{
				this.Clear();
				this.currentCocosItem = null;
			}
		}

		// Token: 0x0400005B RID: 91
		private static bool isInitialized;

		// Token: 0x0400005C RID: 92
		private IGLView glView;

		// Token: 0x0400005D RID: 93
		private CSGuidesService innerService;

		// Token: 0x0400005E RID: 94
		private CocoStudio.Model.PointF lastMousePosition = null;

		// Token: 0x0400005F RID: 95
		private GuidesTipWindow tipWindow;

		// Token: 0x04000060 RID: 96
		private bool isLock = false;

		// Token: 0x04000061 RID: 97
		private CocosItem currentCocosItem;

		// Token: 0x04000062 RID: 98
		private GuidesObject lastHitLine;

		// Token: 0x04000063 RID: 99
		private GuidesObject holdLine;

		// Token: 0x04000064 RID: 100
		private GuidesList verticalList;

		// Token: 0x04000065 RID: 101
		private GuidesList horizontalList;

		// Token: 0x04000066 RID: 102
		private bool isShowRuler = true;

		// Token: 0x04000067 RID: 103
		private bool isAbsorption = true;

		// Token: 0x04000068 RID: 104
		private bool visible = true;

		// Token: 0x04000069 RID: 105
		private System.Drawing.Color lineColor;
	}
}
