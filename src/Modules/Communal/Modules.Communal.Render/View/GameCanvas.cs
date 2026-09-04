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
	// Token: 0x02000036 RID: 54
	public class GameCanvas : EventBox, IGameCanvas, IService
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000D5D0 File Offset: 0x0000B7D0
		// (set) Token: 0x0600026C RID: 620 RVA: 0x0000D5E7 File Offset: 0x0000B7E7
		public bool IsInitialized { get; private set; }

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600026D RID: 621 RVA: 0x0000D5F0 File Offset: 0x0000B7F0
		// (remove) Token: 0x0600026E RID: 622 RVA: 0x0000D62C File Offset: 0x0000B82C
		public event EventHandler<EventArgs> InitializeCompleted;

		// Token: 0x0600026F RID: 623 RVA: 0x0000D668 File Offset: 0x0000B868
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

		// Token: 0x06000270 RID: 624 RVA: 0x0000D780 File Offset: 0x0000B980
		private void Initialize(IEventAggregator eventAggregator)
		{
			base.Shown += this.Window_Shown;
			if (eventAggregator != null)
			{
				eventAggregator.GetEvent<CanvasSizeChangeEvent>().Subscribe(new Action<CanvasSizeChangeEventArgs>(this.CanvasSizeChangeHandle));
				Services.ProjectOperations.CurrentSelectedSolutionChanged += this.ProjectOperations_CurrentSelectedSolutionChanged;
			}
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000D7DD File Offset: 0x0000B9DD
		private void Window_Shown(object sender, EventArgs e)
		{
			this.InitializeGameWindow(this.eventAggregator);
			base.Shown -= this.Window_Shown;
			base.ShowAll();
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000D808 File Offset: 0x0000BA08
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

		// Token: 0x06000273 RID: 627 RVA: 0x0000D8E8 File Offset: 0x0000BAE8
		internal bool CheckInitialized()
		{
			if (!this.IsInitialized)
			{
				this.InitializeGameWindow(Services.EventsService);
			}
			return this.IsInitialized;
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000D918 File Offset: 0x0000BB18
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

		// Token: 0x06000275 RID: 629 RVA: 0x0000D964 File Offset: 0x0000BB64
		private void CanvasSizeChangeHandle(CanvasSizeChangeEventArgs args)
		{
			if (this.GameWindow != null)
			{
				this.glView.OnCanvasSizeChanged();
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000D990 File Offset: 0x0000BB90
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

		// Token: 0x06000277 RID: 631 RVA: 0x0000D9C1 File Offset: 0x0000BBC1
		internal void SwitchView(bool isShowing)
		{
			this.glView.SwitchView(isShowing);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D9D1 File Offset: 0x0000BBD1
		internal void OnCocosItemChanged(CocosItem cocosItem)
		{
			this.SetRulerChanged(cocosItem);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		// (set) Token: 0x0600027A RID: 634 RVA: 0x0000D9F9 File Offset: 0x0000BBF9
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

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600027B RID: 635 RVA: 0x0000DA0C File Offset: 0x0000BC0C
		public GameWindow GameWindow
		{
			get
			{
				return this.glView.GameWindow;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600027C RID: 636 RVA: 0x0000DA2C File Offset: 0x0000BC2C
		// (set) Token: 0x0600027D RID: 637 RVA: 0x0000DA44 File Offset: 0x0000BC44
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

		// Token: 0x0600027E RID: 638 RVA: 0x0000DA74 File Offset: 0x0000BC74
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

		// Token: 0x0600027F RID: 639 RVA: 0x0000DB37 File Offset: 0x0000BD37
		private void InitRulerEvent()
		{
			GlobalCommand.RulerCmd.Execute += this.RulerCmd_Execute;
			GlobalCommand.RulerCmd.Update += this.RulerCmd_Update;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000DB68 File Offset: 0x0000BD68
		private void SetBackgroud(Color color)
		{
			base.ModifyBg(StateType.Normal, color);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000DB74 File Offset: 0x0000BD74
		private bool CanShowRuler(CocosItem cocosItem)
		{
			bool result = true;
			if (cocosItem == null || cocosItem.CocosFile.Type == NodeType.Plist.ToString() || cocosItem.CocosFile.Type == NodeType.Scene3D.ToString())
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000DBD4 File Offset: 0x0000BDD4
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

		// Token: 0x06000283 RID: 643 RVA: 0x0000DC10 File Offset: 0x0000BE10
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

		// Token: 0x06000284 RID: 644 RVA: 0x0000DC8F File Offset: 0x0000BE8F
		private void HideRuler()
		{
			this.hbox_ruler_H.RemoveAll();
			this.hbox_ruler_V.RemoveAll();
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000DCAC File Offset: 0x0000BEAC
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

		// Token: 0x06000286 RID: 646 RVA: 0x0000DCF4 File Offset: 0x0000BEF4
		private void RulerCmd_Update(object sender, CommandUpdateArgs e)
		{
			CocosItem file = Services.Workbench.ActiveDocument.File;
			e.Info.Enabled = this.CanShowRuler(file);
			e.Info.Checked = GuidesService.Instance.IsShowRuler;
		}

		// Token: 0x040000AD RID: 173
		private const int defaultborderWidth = 1;

		// Token: 0x040000AE RID: 174
		private GLView glView;

		// Token: 0x040000AF RID: 175
		private RulerView ruler_horzontal;

		// Token: 0x040000B0 RID: 176
		private RulerView ruler_vertical;

		// Token: 0x040000B1 RID: 177
		private IEventAggregator eventAggregator;

		// Token: 0x040000B3 RID: 179
		private bool showAnimationModeTip = false;

		// Token: 0x040000B4 RID: 180
		private HBox container;

		// Token: 0x040000B5 RID: 181
		private HBox hbox_ruler_V;

		// Token: 0x040000B6 RID: 182
		private HBox hbox_ruler_H;

		// Token: 0x040000B7 RID: 183
		private EventBox eventBox_rect;

		// Token: 0x040000B8 RID: 184
		private readonly Color controlBrush = new Color(51, 51, 51);
	}
}
