using System;
using System.Linq;
using Cairo;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;

namespace Modules.Communal.Render.Model
{
	// Token: 0x02000025 RID: 37
	internal class DefaultObjectTool : BaseObjectTool
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00008068 File Offset: 0x00006268
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Arrow.png");
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00008084 File Offset: 0x00006284
		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool2DWidget_normal + " (W)";
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000139 RID: 313 RVA: 0x000080A8 File Offset: 0x000062A8
		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.w;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000080BC File Offset: 0x000062BC
		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected)
			{
				BaseTool.SetCursor(Cursors.Arrow);
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000080EC File Offset: 0x000062EC
		public override void OnMouseMove(MotionNotifyEventArgs args)
		{
			base.OnMouseMove(args);
			if (!args.CheckRetval())
			{
				if (this.HitTest(args.Event.GetPoint()))
				{
					args.RetVal = true;
				}
			}
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008138 File Offset: 0x00006338
		public override void OnMouseDoubleClick(ButtonPressEventArgs args)
		{
			if (SelectService.Instance.SelectedParentObjectList.Count<VisualObject>() == 1)
			{
				foreach (VisualObject visualObject in SelectService.Instance.SelectedParentObjectList)
				{
					MouseEventArgs args2 = EventArgsConvert.ToMouseEvent(args.Event, this);
					visualObject.MouseDoubleClick(args2);
				}
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x000081C0 File Offset: 0x000063C0
		protected override bool HitTest(PointF widgetPoint)
		{
			PointF pointF = this.ConvertCoordinate(widgetPoint);
			HitTestResult hitTestResult = this.controlNode.HitTest(pointF);
			bool result;
			if (hitTestResult == null || hitTestResult.HitVisual == null)
			{
				BaseTool.SetCursor(Cursors.Arrow);
				result = false;
			}
			else
			{
				this.ChangeCursor(hitTestResult, pointF);
				result = true;
			}
			return result;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008218 File Offset: 0x00006418
		private void ChangeCursor(HitTestResult result, PointF scenePoint)
		{
			switch (result.OperationType)
			{
			case MouseOperationType.OPERATION_NONE:
				BaseTool.SetCursor(Cursors.Arrow);
				return;
			case MouseOperationType.OPERATION_POSITION:
			{
				HitTestResult hoverVisualBetweenSelected = HitTestMode.GetHoverVisualBetweenSelected(scenePoint, SelectService.Instance.SelectedObjectList);
				if (hoverVisualBetweenSelected != null && hoverVisualBetweenSelected.HitVisual != null)
				{
					BaseTool.SetCursor(Cursors.ArrowMove);
					return;
				}
				BaseTool.SetCursor(Cursors.Arrow);
				return;
			}
			case MouseOperationType.OPERATION_ROTATION:
				BaseTool.SetCursor(new Cursor(Cursors.RotateRight.Display, this.RotatePixbuf(result.Rotate, this.rotateRightPixbuf), 7, 7));
				return;
			case MouseOperationType.OPERATION_SCALE:
				BaseTool.SetCursor(new Cursor(Cursors.StretchHorizon.Display, this.RotatePixbuf(result.Rotate, this.stretchHorizonPixbuf), 7, 7));
				return;
			case MouseOperationType.OPERATION_ANCHOR_POINT:
				BaseTool.SetCursor(Cursors.ArrowMoveAnchor);
				return;
			case MouseOperationType.OPERATION_SIZE:
				BaseTool.SetCursor(new Cursor(Cursors.StretchSizeHorizon.Display, this.RotatePixbuf(result.Rotate, this.stretchSizeHorizonPixbuf), 7, 7));
				return;
			}
			BaseTool.SetCursor(Cursors.Arrow);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008350 File Offset: 0x00006550
		private Pixbuf RotatePixbuf(float rotate, Pixbuf pixbuf)
		{
			ImageSurface imageSurface = new ImageSurface(Format.Argb32, pixbuf.Width, pixbuf.Height);
			using (Cairo.Context context = new Cairo.Context(imageSurface))
			{
				context.Translate((double)(pixbuf.Width / 2), (double)(pixbuf.Height / 2));
				context.Rotate((double)rotate);
				CairoHelper.SetSourcePixbuf(context, pixbuf, (double)(-(double)pixbuf.Width / 2), (double)(-(double)pixbuf.Height / 2));
				context.Rectangle(0.0, 0.0, (double)pixbuf.Width, (double)pixbuf.Height);
				context.Paint();
			}
			Pixbuf result = new Pixbuf(imageSurface.Data, Colorspace.Rgb, true, 8, pixbuf.Width, pixbuf.Height, imageSurface.Stride);
			imageSurface.Dispose();
			return result;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008438 File Offset: 0x00006638
		public override void Initialize()
		{
			this.rotateRightPixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.CursorImage.RotateRight.png");
			this.stretchHorizonPixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.CursorImage.StretchHorizon.png");
			this.stretchSizeHorizonPixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.CursorImage.StretchSizeHorizon.png");
			this.controlNode = new ControlNode(GameWindow.Current.GetCanvasObject());
			this.controlNode.Visible = false;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008498 File Offset: 0x00006698
		public override void Load()
		{
			base.Load();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000084CC File Offset: 0x000066CC
		public override void UnLoad()
		{
			base.UnLoad();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00008500 File Offset: 0x00006700
		private void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			ControlNode controlNode = this.controlNode as ControlNode;
			controlNode.SelectedObjectsChanged(args.SelectedObject, args.SelectedParentObject);
		}

		// Token: 0x04000050 RID: 80
		private Pixbuf stretchHorizonPixbuf = null;

		// Token: 0x04000051 RID: 81
		private Pixbuf rotateRightPixbuf = null;

		// Token: 0x04000052 RID: 82
		private Pixbuf stretchSizeHorizonPixbuf = null;
	}
}
