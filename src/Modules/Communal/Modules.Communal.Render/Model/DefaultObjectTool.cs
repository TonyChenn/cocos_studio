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
	internal class DefaultObjectTool : BaseObjectTool
	{
		public override Xwt.Drawing.Image Icon
		{
			get
			{
				return ImageIcon.GetIcon("CocoStudio.DefaultResource.Images.Toolbar.Arrow.png");
			}
		}

		public override string Tooltip
		{
			get
			{
				return LanguageInfo.MainTool2DWidget_normal + " (W)";
			}
		}

		public override Gdk.Key ShortcutKey
		{
			get
			{
				return Gdk.Key.w;
			}
		}

		protected override void OnSelectedChanged()
		{
			base.OnSelectedChanged();
			if (base.IsSelected)
			{
				BaseTool.SetCursor(Cursors.Arrow);
			}
		}

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

		public override void Initialize()
		{
			this.rotateRightPixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.CursorImage.RotateRight.png");
			this.stretchHorizonPixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.CursorImage.StretchHorizon.png");
			this.stretchSizeHorizonPixbuf = ImageIcon.GetPixbuf("CocoStudio.DefaultResource.CursorImage.StretchSizeHorizon.png");
			this.controlNode = new ControlNode(GameWindow.Current.GetCanvasObject());
			this.controlNode.Visible = false;
		}

		public override void Load()
		{
			base.Load();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
		}

		public override void UnLoad()
		{
			base.UnLoad();
			SelectedVisualObjectsChangeEvent @event = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			@event.Unsubscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.OnSelectObjectsChangeEvent));
		}

		private void OnSelectObjectsChangeEvent(SelectedVisualObjectsChangeEventArgs args)
		{
			ControlNode controlNode = this.controlNode as ControlNode;
			controlNode.SelectedObjectsChanged(args.SelectedObject, args.SelectedParentObject);
		}

		private Pixbuf stretchHorizonPixbuf = null;

		private Pixbuf rotateRightPixbuf = null;

		private Pixbuf stretchSizeHorizonPixbuf = null;
	}
}
