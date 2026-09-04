using System;
using Gtk;
using Mono.Debugging.Client;
using MonoDevelop.Ide.Gui;

namespace MonoDevelop.Debugger
{
	public class ObjectValuePad : IPadContent, IDisposable
	{
		protected ObjectValueTreeView tree;

		private readonly ScrolledWindow scrolled;

		private bool needsUpdate;

		private IPadWindow container;

		private bool initialResume;

		private StackFrame lastFrame;

		private PadFontChanger fontChanger;

		public Widget Control => scrolled;

		public ObjectValuePad()
		{
			scrolled = new ScrolledWindow();
			scrolled.HscrollbarPolicy = PolicyType.Automatic;
			scrolled.VscrollbarPolicy = PolicyType.Automatic;
			tree = new ObjectValueTreeView();
			fontChanger = new PadFontChanger(tree, tree.SetCustomFont, tree.QueueResize);
			tree.AllowEditing = true;
			tree.AllowAdding = false;
			tree.HeadersVisible = true;
			tree.RulesHint = true;
			scrolled.Add(tree);
			scrolled.ShowAll();
			DebuggingService.CurrentFrameChanged += OnFrameChanged;
			DebuggingService.PausedEvent += OnDebuggerPaused;
			DebuggingService.ResumedEvent += OnDebuggerResumed;
			DebuggingService.StoppedEvent += OnDebuggerStopped;
			DebuggingService.EvaluationOptionsChanged += OnEvaluationOptionsChanged;
			needsUpdate = true;
			initialResume = true;
		}

		public void Dispose()
		{
			if (fontChanger != null)
			{
				fontChanger.Dispose();
				fontChanger = null;
				DebuggingService.CurrentFrameChanged -= OnFrameChanged;
				DebuggingService.PausedEvent -= OnDebuggerPaused;
				DebuggingService.ResumedEvent -= OnDebuggerResumed;
				DebuggingService.StoppedEvent -= OnDebuggerStopped;
				DebuggingService.EvaluationOptionsChanged -= OnEvaluationOptionsChanged;
			}
		}

		public void Initialize(IPadWindow container)
		{
			this.container = container;
			container.PadContentShown += delegate
			{
				if (needsUpdate)
				{
					OnUpdateList();
				}
			};
		}

		public void RedrawContent()
		{
		}

		public virtual void OnUpdateList()
		{
			needsUpdate = false;
			if (DebuggingService.CurrentFrame != lastFrame)
			{
				tree.Frame = DebuggingService.CurrentFrame;
			}
			lastFrame = DebuggingService.CurrentFrame;
		}

		protected virtual void OnFrameChanged(object s, EventArgs a)
		{
			if (container != null && container.ContentVisible)
			{
				OnUpdateList();
			}
			else
			{
				needsUpdate = true;
			}
		}

		protected virtual void OnDebuggerPaused(object s, EventArgs a)
		{
		}

		protected virtual void OnDebuggerResumed(object s, EventArgs a)
		{
			if (!initialResume)
			{
				tree.ChangeCheckpoint();
			}
			initialResume = false;
		}

		protected virtual void OnDebuggerStopped(object s, EventArgs a)
		{
			tree.ResetChangeTracking();
			tree.ClearAll();
			lastFrame = null;
			initialResume = true;
		}

		protected virtual void OnEvaluationOptionsChanged(object s, EventArgs a)
		{
			if (!DebuggingService.IsRunning)
			{
				lastFrame = null;
				if (container != null && container.ContentVisible)
				{
					OnUpdateList();
				}
				else
				{
					needsUpdate = true;
				}
			}
		}
	}
}
