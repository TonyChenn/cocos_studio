using System;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render.View
{
	public class GameCanvas : EventBox, IGameCanvas, IService
	{
		public bool IsInitialized { get; private set; }

		public event EventHandler<EventArgs> InitializeCompleted;

		public GameCanvas()
		{
			this.InitializeComponent();
			this.eventAggregator = Services.EventsService;
			try
			{
				Services.RegisterService<IGameCanvas>(this);
				this.glView = new GLView(this.eventAggregator);
				this.container.PackStart(this.glView, true, true, 0U);
				this.ruler_horzontal = new RulerView();
				this.ruler_horzontal.HeightRequest = 18;
				this.ruler_horzontal.RulerOrientation = Orientation.Horizontal;
				this.hbox_ruler_H.Add(this.ruler_horzontal);
				this.ruler_vertical = new RulerView();
				this.ruler_vertical.WidthRequest = 18;
				this.ruler_vertical.RulerOrientation = Orientation.Vertical;
				this.hbox_ruler_V.Add(this.ruler_vertical);
				this.Initialize(this.eventAggregator);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Initialize renderUC failed.", exception);
			}
		}

		private void Initialize(IEventAggregator eventAggregator)
		{
			base.Shown += this.Window_Shown;
			if (eventAggregator != null)
			{
				eventAggregator.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.CanvasSizeChangeHandle));
				Services.ProjectOperations.CurrentSelectedSolutionChanged += this.ProjectOperations_CurrentSelectedSolutionChanged;
			}
		}

		private void Window_Shown(object sender, EventArgs e)
		{
			this.InitializeGameWindow(this.eventAggregator);
			base.Shown -= this.Window_Shown;
			base.ShowAll();
		}

		private void InitializeGameWindow(IEventAggregator eventAggregator)
		{
			this.IsInitialized = this.glView.CreateView();
			if (this.IsInitialized && this.InitializeCompleted != null)
			{
				this.InitializeCompleted(this, new EventArgs());
			}
			if (this.glView.GameWindow != null)
			{
				if (Services.ProjectOperations.CurrentSelectedSolution != null)
				{
					this.GameWindow.SetResourcePath(Services.ProjectOperations.CurrentSelectedSolution.ItemDirectory);
				}
				this.ruler_horzontal.Initialize();
				this.ruler_vertical.Initialize();
				ViewModeManager.Instance.Initialize(this.glView);
			}
			if (eventAggregator != null)
			{
				eventAggregator.GetEvent<RenderEngineLoadedEvent>().Publish(new RenderEngineLoadedEventArgs(this.glView.GameWindow));
			}
		}

		internal bool CheckInitialized()
		{
			if (!this.IsInitialized)
			{
				this.InitializeGameWindow(Services.EventsService);
			}
			return this.IsInitialized;
		}

		private void ProjectOperations_CurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			if (this.GameWindow != null)
			{
				if (e.Solution != null)
				{
					this.GameWindow.SetResourcePath(e.Solution.ItemDirectory);
				}
			}
		}

		private void CanvasSizeChangeHandle(CanvasSizeChangeEventArgs args)
		{
			if (this.GameWindow != null)
			{
				this.glView.OnCanvasSizeChanged();
			}
		}

		private void OnShowAnimationModeTipChanged(bool isShow)
		{
			if (isShow)
			{
				this.SetBackgroud(Colors.Red);
			}
			else
			{
				this.SetBackgroud(this.controlBrush);
			}
		}

		internal void SwitchView(bool isShowing)
		{
			this.glView.SwitchView(isShowing);
		}

		internal void OnCocosItemChanged(CocosItem cocosItem)
		{
			this.SetRulerChanged(cocosItem);
		}

		internal bool NeedRedraw
		{
			get
			{
				return this.glView.NeedRedraw;
			}
			set
			{
				this.glView.NeedRedraw = value;
			}
		}

		public GameWindow GameWindow
		{
			get
			{
				return this.glView.GameWindow;
			}
		}

		public bool IsRecordAnimation
		{
			get
			{
				return this.showAnimationModeTip;
			}
			set
			{
				if (this.showAnimationModeTip != value)
				{
					this.showAnimationModeTip = value;
					this.OnShowAnimationModeTipChanged(value);
				}
			}
		}

		private void InitializeComponent()
		{
			base.ModifyBg(StateType.Normal, this.controlBrush);
			VBox vbox = new VBox();
			HBox hbox = new HBox();
			this.hbox_ruler_V = new HBox();
			this.container = new HBox();
			hbox.PackStart(this.hbox_ruler_V, false, false, 0U);
			hbox.Add(this.container);
			this.hbox_ruler_H = new HBox();
			this.eventBox_rect = new EventBox();
			this.eventBox_rect.WidthRequest = 18;
			this.hbox_ruler_H.PackStart(this.eventBox_rect, false, false, 0U);
			vbox.Add(hbox);
			vbox.PackStart(this.hbox_ruler_H, false, false, 0U);
			vbox.BorderWidth = 1U;
			base.Add(vbox);
			this.InitRulerEvent();
		}

		private void InitRulerEvent()
		{
			GlobalCommand.RulerCmd.Execute += this.RulerCmd_Execute;
			GlobalCommand.RulerCmd.Update += this.RulerCmd_Update;
		}

		private void SetBackgroud(Color color)
		{
			base.ModifyBg(StateType.Normal, color);
		}

		private bool CanShowRuler(CocosItem cocosItem)
		{
			bool result = true;
			if (cocosItem == null || cocosItem.CocosFile.Type == NodeType.Plist.ToString() || cocosItem.CocosFile.Type == NodeType.Scene3D.ToString())
			{
				result = false;
			}
			return result;
		}

		private void SetRulerChanged(CocosItem cocosItem)
		{
			if (this.CanShowRuler(cocosItem) && GuidesService.Instance.IsShowRuler)
			{
				this.ShowRuler();
			}
			else
			{
				this.HideRuler();
			}
		}

		private void ShowRuler()
		{
			if (!this.hbox_ruler_H.Children.Contains(this.ruler_horzontal))
			{
				this.hbox_ruler_H.PackStart(this.eventBox_rect, false, false, 0U);
				this.hbox_ruler_H.Add(this.ruler_horzontal);
			}
			if (!this.hbox_ruler_V.Children.Contains(this.ruler_vertical))
			{
				this.hbox_ruler_V.Add(this.ruler_vertical);
			}
		}

		private void HideRuler()
		{
			this.hbox_ruler_H.RemoveAll();
			this.hbox_ruler_V.RemoveAll();
		}

		private void RulerCmd_Execute(object sender, CommandRunArgs e)
		{
			GuidesService.Instance.IsShowRuler = !GuidesService.Instance.IsShowRuler;
			if (GuidesService.Instance.IsShowRuler)
			{
				this.ShowRuler();
			}
			else
			{
				this.HideRuler();
			}
		}

		private void RulerCmd_Update(object sender, CommandUpdateArgs e)
		{
			CocosItem file = Services.Workbench.ActiveDocument.File;
			e.Info.Enabled = this.CanShowRuler(file);
			e.Info.Checked = GuidesService.Instance.IsShowRuler;
		}

		private const int defaultborderWidth = 1;

		private GLView glView;

		private RulerView ruler_horzontal;

		private RulerView ruler_vertical;

		private IEventAggregator eventAggregator;

		private bool showAnimationModeTip = false;

		private HBox container;

		private HBox hbox_ruler_V;

		private HBox hbox_ruler_H;

		private EventBox eventBox_rect;

		private readonly Color controlBrush = new Color(51, 51, 51);
	}
}
