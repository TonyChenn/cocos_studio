using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Model;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Preference;

namespace Modules.UI.MainTool
{
	// Token: 0x02000004 RID: 4
	internal class CanvasWidget : BaseToolbarWidget
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002064 File Offset: 0x00000264
		public override Widget GtkWidget
		{
			get
			{
				return this.mainHBox;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000B RID: 11 RVA: 0x0000207C File Offset: 0x0000027C
		public ResolutionConfig CurrentResolution
		{
			get
			{
				ResolutionConfig result;
				if (this.mainComboBox.Active < 0 || this.mainComboBox.Active >= this.ViewModel.CanvasSizeList.Count)
				{
					LogConfig.Logger.Error("获取当前画布分辨率失败");
					result = null;
				}
				else
				{
					result = this.ViewModel.CanvasSizeList[this.mainComboBox.Active];
				}
				return result;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020F4 File Offset: 0x000002F4
		public CanvasWidget()
		{
			this.ViewModel = new CanvasViewModel(Services.EventsService, this);
			this.InitWidget();
			this.InitEvent();
			this.RefreshCanvasComboBox();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002148 File Offset: 0x00000348
		private void InitWidget()
		{
			this.mainHBox = new HBox();
			this.mainHBox.Spacing = 6;
			this.mainComboBox = ComboBox.NewText();
			this.mainComboBox.Name = "MainComboBox";
			this.mainHBox.PackStart(this.mainComboBox);
			this.buttonHorizon = new IconRadioButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.ComponentResource.Horizontal.png"));
			this.buttonHorizon.HasTooltip = true;
			this.buttonHorizon.TooltipText = LanguageInfo.Display_Horizontal;
			this.buttonVertical = new IconRadioButton(ImageIcon.GetIcon("CocoStudio.DefaultResource.ComponentResource.Vertical.png"));
			this.buttonVertical.HasTooltip = true;
			this.buttonVertical.TooltipText = LanguageInfo.Display_Vertical;
			HBox hbox = new HBox();
			hbox.PackStart(this.buttonHorizon, false, false, 0U);
			hbox.PackStart(this.buttonVertical, false, false, 0U);
			this.mainHBox.PackStart(hbox, false, false, 0U);
			this.mainHBox.ShowAll();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002244 File Offset: 0x00000444
		private void InitEvent()
		{
			this.mainComboBox.Changed += this.CanvasComboBoxChangedHandler;
			this.buttonHorizon.CheckChanged += this.HorizonButtonCheckedChangeHandler;
			Option.UserConfig.PropertyChanged += this.UserConfigPropertyChangedHandler;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x0000229C File Offset: 0x0000049C
		public void SelectResolution(ResolutionConfig newResolution)
		{
			this.mainComboBox.Changed -= this.CanvasComboBoxChangedHandler;
			if (newResolution == null)
			{
				newResolution = ResolutionConfig.CreateDefaultResolution();
			}
			int active;
			if (this.ViewModel.CanvasSizeList.Contains(newResolution))
			{
				active = this.ViewModel.CanvasSizeList.IndexOf(newResolution);
			}
			else if (Option.UserConfig.ResolutionList.Contains(newResolution))
			{
				newResolution.IsSelected = true;
				this.ViewModel.CanvasSizeList.Insert(0, newResolution);
				((ListStore)this.mainComboBox.Model).InsertTextAt(0, newResolution.GetDisplayText());
				active = 0;
				Option.UserConfig.Save();
			}
			else
			{
				if (string.IsNullOrEmpty(newResolution.Name))
				{
					newResolution.Name = "Default";
				}
				Option.UserConfig.ResolutionList.Insert(0, newResolution);
				this.ViewModel.CanvasSizeList.Insert(0, newResolution);
				((ListStore)this.mainComboBox.Model).InsertTextAt(0, newResolution.GetDisplayText());
				active = 0;
				Option.UserConfig.Save();
			}
			this.mainComboBox.Active = active;
			if (!this.preResolution.Equals(newResolution))
			{
				this.ViewModel.RaiseCanvasSizeChange(new SizeF((float)newResolution.Width, (float)newResolution.Height));
			}
			this.RefreshCanvasButtons();
			this.preResolution = (newResolution.Clone() as ResolutionConfig);
			this.mainComboBox.Changed += this.CanvasComboBoxChangedHandler;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002444 File Offset: 0x00000644
		private void RefreshCanvasComboBox()
		{
			this.mainComboBox.Changed -= this.CanvasComboBoxChangedHandler;
			int num = -1;
			this.mainComboBox.RemoveAll();
			this.ViewModel.CanvasSizeList = Option.UserConfig.ResolutionList.FindAll((ResolutionConfig w) => w.IsSelected);
			ListStore listStore = new ListStore(new Type[]
			{
				typeof(string)
			});
			for (int i = 0; i < this.ViewModel.CanvasSizeList.Count; i++)
			{
				ResolutionConfig resolutionConfig = this.ViewModel.CanvasSizeList[i];
				listStore.AppendValues(new object[]
				{
					resolutionConfig.GetDisplayText()
				});
				if (resolutionConfig == this.preResolution)
				{
					num = i;
				}
			}
			listStore.AppendValues(new object[]
			{
				LanguageInfo.MainTool_itemCustomSize
			});
			this.mainComboBox.Model = listStore;
			if (num != -1)
			{
				this.mainComboBox.Active = num;
			}
			else
			{
				this.SelectResolution(this.preResolution);
			}
			this.RefreshCanvasButtons();
			this.mainComboBox.Changed += this.CanvasComboBoxChangedHandler;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000025A0 File Offset: 0x000007A0
		private void ReverseScreen()
		{
			ResolutionConfig currentResolution = this.CurrentResolution;
			if (currentResolution != null && currentResolution.Width != currentResolution.Height)
			{
				currentResolution.Reverse();
				this.ViewModel.RaiseCanvasSizeChange(new SizeF((float)currentResolution.Width, (float)currentResolution.Height));
				ListStore listStore = this.mainComboBox.Model as ListStore;
				TreeIter iter;
				this.mainComboBox.GetActiveIter(out iter);
				listStore.SetValue(iter, 0, currentResolution.GetDisplayText());
				Option.UserConfig.Save();
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002634 File Offset: 0x00000834
		private void RefreshCanvasButtons()
		{
			if (this.CurrentResolution != null)
			{
				this.buttonHorizon.CheckChanged -= this.HorizonButtonCheckedChangeHandler;
				if (this.CurrentResolution.Width >= this.CurrentResolution.Height)
				{
					this.buttonHorizon.IsChecked = true;
				}
				else
				{
					this.buttonVertical.IsChecked = true;
				}
				this.buttonHorizon.CheckChanged += this.HorizonButtonCheckedChangeHandler;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000026BC File Offset: 0x000008BC
		public override void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
			this.ViewModel.OnProjectChanged(args);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000026CC File Offset: 0x000008CC
		public override void OnSolutionClosed(SolutionEventArgs args)
		{
			this.ViewModel.OnSolutionClosed(args);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000026DC File Offset: 0x000008DC
		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.ViewModel.OnSolutionChanged(args);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026EC File Offset: 0x000008EC
		private void CanvasComboBoxChangedHandler(object sender, EventArgs e)
		{
			if (this.mainComboBox.ActiveText != null)
			{
				if (this.mainComboBox.ActiveText.Equals(LanguageInfo.MainTool_itemCustomSize))
				{
					PreferencesDialog preferencesDialog = new PreferencesDialog(EnumPreferenceSetting.Resolution);
					int num = preferencesDialog.Run();
					preferencesDialog.Destroy();
					if (num == -5)
					{
						this.RefreshCanvasComboBox();
					}
					else
					{
						this.SelectResolution(this.preResolution);
					}
				}
				else
				{
					ResolutionConfig currentResolution = this.CurrentResolution;
					if (currentResolution != null)
					{
						this.ViewModel.RaiseCanvasSizeChange(new SizeF((float)currentResolution.Width, (float)currentResolution.Height));
						this.RefreshCanvasButtons();
						this.preResolution = (currentResolution.Clone() as ResolutionConfig);
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000027B8 File Offset: 0x000009B8
		private void HorizonButtonCheckedChangeHandler(object o, EventArgs args)
		{
			this.ReverseScreen();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000027C2 File Offset: 0x000009C2
		private void UserConfigPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			this.RefreshCanvasComboBox();
		}

		// Token: 0x04000001 RID: 1
		private HBox mainHBox;

		// Token: 0x04000002 RID: 2
		private ComboBox mainComboBox;

		// Token: 0x04000003 RID: 3
		private IconRadioButton buttonHorizon;

		// Token: 0x04000004 RID: 4
		private IconRadioButton buttonVertical;

		// Token: 0x04000005 RID: 5
		private CanvasViewModel ViewModel;

		// Token: 0x04000006 RID: 6
		private ResolutionConfig preResolution = Option.UserConfig.ResolutionList[0];
	}
}
