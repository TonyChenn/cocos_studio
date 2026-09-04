using System;
using Cairo;
using Gdk;
using Gtk;
using Mono.TextEditor;
using MonoDevelop.Components;

namespace MonoDevelop.Debugger
{
	internal class OverlayMessageWindow : EventBox
	{
		private const int border = 8;

		public Func<int> SizeFunc;

		private TextEditor textEditor;

		private int wRequest = -1;

		public OverlayMessageWindow()
		{
			base.AppPaintable = true;
		}

		public void ShowOverlay(TextEditor textEditor)
		{
			this.textEditor = textEditor;
			ShowAll();
			textEditor.AddTopLevelWidget(this, 0, 0);
			textEditor.SizeAllocated += HandleSizeAllocated;
			TextEditor.EditorContainerChild editorContainerChild = (TextEditor.EditorContainerChild)textEditor[this];
			editorContainerChild.FixedPosition = true;
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			if (textEditor != null)
			{
				textEditor.SizeAllocated -= HandleSizeAllocated;
				textEditor = null;
			}
		}

		protected override void OnSizeRequested(ref Requisition requisition)
		{
			base.OnSizeRequested(ref requisition);
			if (wRequest > 0)
			{
				requisition.Width = wRequest;
			}
		}

		protected override void OnSizeAllocated(Gdk.Rectangle allocation)
		{
			base.OnSizeAllocated(allocation);
			Resize(allocation);
		}

		private void HandleSizeAllocated(object o, SizeAllocatedArgs args)
		{
			if (SizeFunc != null)
			{
				int num = Math.Min(SizeFunc(), textEditor.Allocation.Width - 16);
				if (num != wRequest)
				{
					wRequest = num;
					QueueResize();
				}
			}
			else if (base.Allocation.Width > textEditor.Allocation.Width - 16 && textEditor.Allocation.Width - 16 > 0)
			{
				QueueResize();
			}
			Resize(base.Allocation);
		}

		private void Resize(Gdk.Rectangle alloc)
		{
			textEditor.MoveTopLevelWidget(this, (textEditor.Allocation.Width - alloc.Width) / 2, textEditor.Allocation.Height - alloc.Height - 8);
		}

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			using (Context context = CairoHelper.Create(evnt.Window))
			{
				context.LineWidth = 1.0;
				context.Rectangle(0.0, 0.0, base.Allocation.Width, base.Allocation.Height);
				context.SetSourceColor(textEditor.ColorStyle.NotificationText.Background);
				context.Fill();
				context.RoundedRectangle(0.0, 0.0, base.Allocation.Width, base.Allocation.Height, 3.0);
				context.SetSourceColor(textEditor.ColorStyle.NotificationText.Background);
				context.FillPreserve();
				context.SetSourceColor(textEditor.ColorStyle.NotificationBorder.Color);
				context.Stroke();
			}
			return base.OnExposeEvent(evnt);
		}
	}
}
