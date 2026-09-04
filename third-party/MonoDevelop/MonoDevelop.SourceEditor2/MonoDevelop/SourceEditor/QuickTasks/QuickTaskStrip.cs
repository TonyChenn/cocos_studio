using System;
using System.Collections.Generic;
using System.Linq;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.Refactoring;
using Mono.TextEditor;
using MonoDevelop.Components.Commands;
using MonoDevelop.Core;
using MonoDevelop.Ide;

namespace MonoDevelop.SourceEditor.QuickTasks
{
	public class QuickTaskStrip : VBox
	{
		internal enum HoverMode
		{
			NextMessage,
			NextWarning,
			NextError
		}

		public static readonly PropertyWrapper<bool> EnableFancyFeatures;

		public static readonly bool MergeScrollBarAndQuickTasks;

		private Adjustment adj;

		private TextEditor textEditor;

		private ScrollBarMode mode;

		private Dictionary<IQuickTaskProvider, List<QuickTask>> providerTasks = new Dictionary<IQuickTaskProvider, List<QuickTask>>();

		private Dictionary<IUsageProvider, List<Usage>> providerUsages = new Dictionary<IUsageProvider, List<Usage>>();

		private Widget mapMode;

		public Adjustment VAdjustment
		{
			get
			{
				return adj;
			}
			set
			{
				adj = value;
			}
		}

		public TextEditor TextEditor
		{
			get
			{
				return textEditor;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				textEditor = value;
				SetupMode();
			}
		}

		public ScrollBarMode ScrollBarMode
		{
			get
			{
				return mode;
			}
			set
			{
				mode = value;
				PropertyService.Set("ScrollBar.Mode", value);
				SetupMode();
			}
		}

		public IEnumerable<QuickTask> AllTasks
		{
			get
			{
				if (providerTasks == null)
				{
					yield break;
				}
				foreach (List<QuickTask> tasks in providerTasks.Values)
				{
					foreach (QuickTask item in tasks)
					{
						yield return item;
					}
				}
			}
		}

		public IEnumerable<Usage> AllUsages
		{
			get
			{
				if (providerUsages == null)
				{
					yield break;
				}
				foreach (List<Usage> tasks in providerUsages.Values)
				{
					foreach (Usage item in tasks)
					{
						yield return item;
					}
				}
			}
		}

		public event EventHandler TaskProviderUpdated;

		static QuickTaskStrip()
		{
			EnableFancyFeatures = new PropertyWrapper<bool>("MonoDevelop.AnalysisCore.AnalysisEnabled", defaultValue: false);
			MergeScrollBarAndQuickTasks = !Platform.IsMac;
			EnableFancyFeatures.Changed += delegate
			{
				PropertyService.Set("ScrollBar.Mode", ScrollBarMode.Overview);
			};
		}

		public QuickTaskStrip()
		{
			ScrollBarMode = PropertyService.Get("ScrollBar.Mode", ScrollBarMode.Overview);
			PropertyService.AddPropertyHandler("ScrollBar.Mode", ScrollBarModeChanged);
			EnableFancyFeatures.Changed += HandleChanged;
			base.Events |= EventMask.ButtonPressMask;
		}

		private void HandleChanged(object sender, EventArgs e)
		{
			SetupMode();
		}

		private void SetupMode()
		{
			if (adj == null || textEditor == null)
			{
				return;
			}
			if (mapMode != null)
			{
				mapMode.Destroy();
				mapMode = null;
			}
			if ((bool)EnableFancyFeatures)
			{
				switch (ScrollBarMode)
				{
				case ScrollBarMode.Overview:
					mapMode = new QuickTaskOverviewMode(this);
					PackStart(mapMode, expand: true, fill: true, 0u);
					break;
				case ScrollBarMode.Minimap:
					mapMode = new QuickTaskMiniMapMode(this);
					PackStart(mapMode, expand: true, fill: true, 0u);
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
			ShowAll();
		}

		protected override void OnDestroyed()
		{
			base.OnDestroyed();
			adj = null;
			textEditor = null;
			providerTasks = null;
			PropertyService.RemovePropertyHandler("ScrollBar.Mode", ScrollBarModeChanged);
			EnableFancyFeatures.Changed -= HandleChanged;
		}

		private void ScrollBarModeChanged(object sender, PropertyChangedEventArgs args)
		{
			ScrollBarMode scrollBarMode = (ScrollBarMode)args.NewValue;
			ScrollBarMode = scrollBarMode;
		}

		public void Update(IQuickTaskProvider provider)
		{
			if (providerTasks != null)
			{
				providerTasks[provider] = new List<QuickTask>(provider.QuickTasks);
				OnTaskProviderUpdated(EventArgs.Empty);
			}
		}

		public void Update(IUsageProvider provider)
		{
			if (providerTasks != null)
			{
				providerUsages[provider] = new List<Usage>(provider.Usages);
				OnTaskProviderUpdated(EventArgs.Empty);
			}
		}

		protected virtual void OnTaskProviderUpdated(EventArgs e)
		{
			TaskProviderUpdated?.Invoke(this, e);
		}

		protected override bool OnButtonPressEvent(EventButton evnt)
		{
			if (evnt.Button == 3)
			{
				IdeApp.CommandService.ShowContextMenu(this, evnt, "/MonoDevelop/SourceEditor2/ContextMenu/Scrollbar");
			}
			return base.OnButtonPressEvent(evnt);
		}

		[CommandHandler(ScrollbarCommand.Top)]
		internal void GotoTop()
		{
			VAdjustment.Value = VAdjustment.Lower;
		}

		[CommandHandler(ScrollbarCommand.Bottom)]
		internal void GotoBottom()
		{
			VAdjustment.Value = Math.Max(VAdjustment.Lower, VAdjustment.Upper - VAdjustment.PageSize / 2.0);
		}

		[CommandHandler(ScrollbarCommand.PgUp)]
		internal void GotoPgUp()
		{
			VAdjustment.Value = Math.Max(VAdjustment.Lower, VAdjustment.Value - VAdjustment.PageSize);
		}

		[CommandHandler(ScrollbarCommand.PgDown)]
		internal void GotoPgDown()
		{
			VAdjustment.Value = Math.Min(VAdjustment.Upper, VAdjustment.Value + VAdjustment.PageSize);
		}

		[CommandUpdateHandler(ScrollbarCommand.ShowTasks)]
		internal void UpdateShowMap(CommandInfo info)
		{
			info.Visible = EnableFancyFeatures;
			info.Checked = ScrollBarMode == ScrollBarMode.Overview;
		}

		[CommandHandler(ScrollbarCommand.ShowTasks)]
		internal void ShowMap()
		{
			ScrollBarMode = ScrollBarMode.Overview;
		}

		[CommandUpdateHandler(ScrollbarCommand.ShowMinimap)]
		internal void UpdateShowFull(CommandInfo info)
		{
			info.Visible = EnableFancyFeatures;
			info.Checked = ScrollBarMode == ScrollBarMode.Minimap;
		}

		[CommandHandler(ScrollbarCommand.ShowMinimap)]
		internal void ShowFull()
		{
			ScrollBarMode = ScrollBarMode.Minimap;
		}

		internal QuickTask SearchNextTask(HoverMode mode)
		{
			TextLocation textLocation = TextEditor.Caret.Location;
			QuickTask quickTask = null;
			foreach (QuickTask item in AllTasks.OrderBy((QuickTask t) => t.Location))
			{
				bool flag = item.Location > textLocation;
				if (mode == HoverMode.NextMessage || (mode == HoverMode.NextWarning && item.Severity == Severity.Warning) || (mode == HoverMode.NextError && item.Severity == Severity.Error))
				{
					if (flag)
					{
						return item;
					}
					if (quickTask == null)
					{
						quickTask = item;
					}
				}
			}
			return quickTask;
		}

		internal QuickTask SearchPrevTask(HoverMode mode)
		{
			TextLocation textLocation = TextEditor.Caret.Location;
			QuickTask quickTask = null;
			foreach (QuickTask item in AllTasks.OrderByDescending((QuickTask t) => t.Location))
			{
				bool flag = item.Location < textLocation;
				if (mode == HoverMode.NextMessage || (mode == HoverMode.NextWarning && item.Severity == Severity.Warning) || (mode == HoverMode.NextError && item.Severity == Severity.Error))
				{
					if (flag)
					{
						return item;
					}
					if (quickTask == null)
					{
						quickTask = item;
					}
				}
			}
			return quickTask;
		}

		internal void GotoTask(QuickTask quickTask)
		{
			if (quickTask != null)
			{
				int line = quickTask.Location.Line;
				if (line >= 1 && line < TextEditor.LineCount)
				{
					TextEditor.Caret.Location = new TextLocation(line, Math.Max(1, quickTask.Location.Column));
					TextEditor.CenterToCaret();
					TextEditor.StartCaretPulseAnimation();
					TextEditor.GrabFocus();
				}
			}
		}
	}
}
