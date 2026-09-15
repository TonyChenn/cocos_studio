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
	internal class CanvasWidget : BaseToolbarWidget
	{
		public override Widget GtkWidget
		{
			get
			{
				return this.mainHBox;
			}
		}

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

		public CanvasWidget()
		{
			this.ViewModel = new CanvasViewModel(Services.EventsService, this);
			this.InitWidget();
			this.InitEvent();
			this.RefreshCanvasComboBox();
		}

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

		private void InitEvent()
		{
			this.mainComboBox.Changed += this.CanvasComboBoxChangedHandler;
			this.buttonHorizon.CheckChanged += this.HorizonButtonCheckedChangeHandler;
			Option.UserConfig.PropertyChanged += this.UserConfigPropertyChangedHandler;
		}

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

		public override void OnProjectChanged(ProjectsOperations.ProjectEventArgs args)
		{
			this.ViewModel.OnProjectChanged(args);
		}

		public override void OnSolutionClosed(SolutionEventArgs args)
		{
			this.ViewModel.OnSolutionClosed(args);
		}

		public override void OnSolutionChanged(SolutionEventArgs args)
		{
			this.ViewModel.OnSolutionChanged(args);
		}

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

		private void HorizonButtonCheckedChangeHandler(object o, EventArgs args)
		{
			this.ReverseScreen();
		}

		private void UserConfigPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
		{
			this.RefreshCanvasComboBox();
		}

		private HBox mainHBox;

		private ComboBox mainComboBox;

		private IconRadioButton buttonHorizon;

		private IconRadioButton buttonVertical;

		private CanvasViewModel ViewModel;

		private ResolutionConfig preResolution = Option.UserConfig.ResolutionList[0];
	}
}
