using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.Events;
using CocoStudio.Core.ExtensionModel;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Event;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gdk;
using Gtk;
using Modules.Communal.MultiLanguage;
using Modules.Communal.Render.Model;
using Mono.Addins;
using Mono.Unix;
using MonoDevelop.Core;
using Stetic;

namespace Modules.Communal.Status
{
	[Extension(Path = "/CocoStudio/Ide/MainStatus")]
	public class StatusUC : Bin, IMainWindowPart, IStatusBar, IService
	{
		protected virtual void Build()
		{
			Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "Modules.Communal.Status.StatusUC";
			this.eventbox_bg = new EventBox();
			this.eventbox_bg.Name = "eventbox_bg";
			this.alignment_main = new Alignment(0.5f, 0.5f, 1f, 1f);
			this.alignment_main.Name = "alignment_main";
			this.alignment_main.LeftPadding = 10U;
			this.alignment_main.RightPadding = 10U;
			this.hbox_main = new HBox();
			this.hbox_main.Name = "hbox_main";
			this.hbox_main.Spacing = 22;
			this.hbox_warningIcon = new HBox();
			this.hbox_warningIcon.Name = "hbox_warningIcon";
			this.hbox_main.Add(this.hbox_warningIcon);
			Box.BoxChild boxChild = (Box.BoxChild)this.hbox_main[this.hbox_warningIcon];
			boxChild.Position = 0;
			boxChild.Expand = false;
			this.hbox_canvas = new HBox();
			this.hbox_canvas.Name = "hbox_canvas";
			this.hbox_canvas.Spacing = 4;
			this.hscale_Zoom = new HScale(null);
			this.hscale_Zoom.WidthRequest = 180;
			this.hscale_Zoom.HeightRequest = 24;
			this.hscale_Zoom.CanFocus = true;
			this.hscale_Zoom.Name = "hscale_Zoom";
			this.hscale_Zoom.Adjustment.Lower = 10.0;
			this.hscale_Zoom.Adjustment.Upper = 500.0;
			this.hscale_Zoom.Adjustment.PageIncrement = 10.0;
			this.hscale_Zoom.Adjustment.StepIncrement = 1.0;
			this.hscale_Zoom.DrawValue = false;
			this.hscale_Zoom.Digits = 0;
			this.hscale_Zoom.ValuePos = PositionType.Top;
			this.hbox_canvas.Add(this.hscale_Zoom);
			Box.BoxChild boxChild2 = (Box.BoxChild)this.hbox_canvas[this.hscale_Zoom];
			boxChild2.Position = 0;
			this.combobox_Zoom = ComboBoxEntry.NewText();
			this.combobox_Zoom.WidthRequest = 76;
			this.combobox_Zoom.HeightRequest = 24;
			this.combobox_Zoom.Name = "combobox_Zoom";
			this.hbox_canvas.Add(this.combobox_Zoom);
			Box.BoxChild boxChild3 = (Box.BoxChild)this.hbox_canvas[this.combobox_Zoom];
			boxChild3.Position = 1;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			this.hbox_main.Add(this.hbox_canvas);
			Box.BoxChild boxChild4 = (Box.BoxChild)this.hbox_main[this.hbox_canvas];
			boxChild4.PackType = PackType.End;
			boxChild4.Position = 1;
			boxChild4.Expand = false;
			this.hbox_scale = new HBox();
			this.hbox_scale.Name = "hbox_scale";
			this.hbox_scale.Spacing = 4;
			this.label_ScaleName = new Label();
			this.label_ScaleName.Name = "label_ScaleName";
			this.label_ScaleName.LabelProp = Catalog.GetString("缩放");
			this.hbox_scale.Add(this.label_ScaleName);
			Box.BoxChild boxChild5 = (Box.BoxChild)this.hbox_scale[this.label_ScaleName];
			boxChild5.Position = 0;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			this.label_ScaleValue = new Label();
			this.label_ScaleValue.Name = "label_ScaleValue";
			this.hbox_scale.Add(this.label_ScaleValue);
			Box.BoxChild boxChild6 = (Box.BoxChild)this.hbox_scale[this.label_ScaleValue];
			boxChild6.Position = 1;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			this.hbox_main.Add(this.hbox_scale);
			Box.BoxChild boxChild7 = (Box.BoxChild)this.hbox_main[this.hbox_scale];
			boxChild7.PackType = PackType.End;
			boxChild7.Position = 2;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			this.hbox_rotate = new HBox();
			this.hbox_rotate.Name = "hbox_rotate";
			this.hbox_rotate.Spacing = 4;
			this.label_RotationName = new Label();
			this.label_RotationName.Name = "label_RotationName";
			this.label_RotationName.LabelProp = Catalog.GetString("旋转");
			this.hbox_rotate.Add(this.label_RotationName);
			Box.BoxChild boxChild8 = (Box.BoxChild)this.hbox_rotate[this.label_RotationName];
			boxChild8.Position = 0;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			this.label_RotationValue = new Label();
			this.label_RotationValue.Name = "label_RotationValue";
			this.hbox_rotate.Add(this.label_RotationValue);
			Box.BoxChild boxChild9 = (Box.BoxChild)this.hbox_rotate[this.label_RotationValue];
			boxChild9.Position = 1;
			boxChild9.Expand = false;
			boxChild9.Fill = false;
			this.hbox_main.Add(this.hbox_rotate);
			Box.BoxChild boxChild10 = (Box.BoxChild)this.hbox_main[this.hbox_rotate];
			boxChild10.PackType = PackType.End;
			boxChild10.Position = 3;
			boxChild10.Expand = false;
			boxChild10.Fill = false;
			this.hbox_pos = new HBox();
			this.hbox_pos.Name = "hbox_pos";
			this.hbox_pos.Spacing = 4;
			this.label_PositionName = new Label();
			this.label_PositionName.Name = "label_PositionName";
			this.label_PositionName.LabelProp = Catalog.GetString("位置");
			this.hbox_pos.Add(this.label_PositionName);
			Box.BoxChild boxChild11 = (Box.BoxChild)this.hbox_pos[this.label_PositionName];
			boxChild11.Position = 0;
			boxChild11.Expand = false;
			boxChild11.Fill = false;
			this.label_PositionValue = new Label();
			this.label_PositionValue.Name = "label_PositionValue";
			this.hbox_pos.Add(this.label_PositionValue);
			Box.BoxChild boxChild12 = (Box.BoxChild)this.hbox_pos[this.label_PositionValue];
			boxChild12.Position = 1;
			boxChild12.Expand = false;
			boxChild12.Fill = false;
			this.hbox_main.Add(this.hbox_pos);
			Box.BoxChild boxChild13 = (Box.BoxChild)this.hbox_main[this.hbox_pos];
			boxChild13.PackType = PackType.End;
			boxChild13.Position = 4;
			boxChild13.Expand = false;
			boxChild13.Fill = false;
			this.label_GUIName = new Label();
			this.label_GUIName.Name = "label_GUIName";
			this.hbox_main.Add(this.label_GUIName);
			Box.BoxChild boxChild14 = (Box.BoxChild)this.hbox_main[this.label_GUIName];
			boxChild14.PackType = PackType.End;
			boxChild14.Position = 5;
			boxChild14.Expand = false;
			boxChild14.Fill = false;
			this.alignment_main.Add(this.hbox_main);
			this.eventbox_bg.Add(this.alignment_main);
			base.Add(this.eventbox_bg);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			base.Hide();
		}

		private double CanvasZoom
		{
			get
			{
				return this.hscale_Zoom.Value;
			}
			set
			{
				if (this.hscale_Zoom.Value != value)
				{
					this.hscale_Zoom.Value = value;
				}
			}
		}

		public bool Is3DCocosItem
		{
			get
			{
				return this.is3DCocosItem;
			}
			set
			{
				if (this.is3DCocosItem != value)
				{
					this.is3DCocosItem = value;
					if (!value)
					{
						this.hbox_canvas.Show();
					}
					else
					{
						this.hbox_canvas.Hide();
					}
				}
			}
		}

		public StatusUC()
		{
			this.Build();
			this.eventbox_bg.ModifyBg(StateType.Normal, WindowStyle.WindowBgColor);
			this.eventAggregator = Services.EventsService;
			base.HeightRequest = 26;
			this.InitView();
			this.InitEvent();
			this.InitValue();
			this.ReadLanuageConfigFile();
			base.ShowAll();
			Services.RegisterService<IStatusBar>(this);
		}

		private void InitView()
		{
			foreach (string text in this.ZoomDefaultList)
			{
				this.combobox_Zoom.AppendText(text);
			}
			this.button_reset = new IconButton(ImageIcon.GetIcon("Modules.Communal.Status.Image.reset.png"));
			this.hbox_canvas.PackStart(this.button_reset, false, false, 0U);
		}

		private void InitEvent()
		{
			this.hscale_Zoom.ValueChanged += this.hscale_Zoom_ValueChanged;
			this.combobox_Zoom.Changed += this.combobox_Zoom_Changed;
			this.combobox_Zoom.Entry.WidgetEvent += this.Entry_WidgetEvent;
			this.combobox_Zoom.Entry.Changed += this.Entry_Changed;
			this.combobox_Zoom.Entry.KeyReleaseEvent += this.Entry_KeyReleaseEvent;
			this.combobox_Zoom.Entry.FocusOutEvent += this.combobox_Zoom_FocusOutEvent;
			this.combobox_Zoom.Entry.ScrollEvent += this.Entry_ScrollEvent;
			this.button_reset.Clicked += new EventHandler<ButtonReleaseEventArgs>(this.button_reset_Clicked);
			GlobalCommand.ResetCanvasCmd.Execute += this.ResetCanvasCmd_Execute;
			GlobalCommand.ResetCanvasCmd2.Execute += this.ResetCanvasCmd_Execute;
			this.eventAggregator.GetEvent<SelectedVisualObjectsChangeEvent>().Subscribe(new Action<SelectedVisualObjectsChangeEventArgs>(this.SelectedObjectsChangeEventHandle));
			this.eventAggregator.GetEvent<CanvasZoomChangeEvent>().Subscribe(new Action<CanvasZoomChangeEventArgs>(this.CanvasZoomChangeEventHandle));
			Services.ProjectOperations.CurrentProjectChanged += this.ProjectOperations_CurrentDocumentChanged;
			Services.ProjectOperations.CurrentSelectedSolutionChanged += this.ProjectOperations_CurrentSelectedSolutionChanged;
		}

		private void InitValue()
		{
			this.CanvasZoom = 100.0;
			this.combobox_Zoom.Active = 4;
			this.hbox_canvas.Sensitive = false;
		}

		private void ReadLanuageConfigFile()
		{
			this.label_ScaleName.LabelProp = string.Empty;
			this.label_RotationName.LabelProp = string.Empty;
			this.label_PositionName.LabelProp = string.Empty;
			string arg = Platform.IsWindows ? "Ctrl + Num0" : "Command + Num0";
			this.button_reset.TooltipText = string.Format("{0} ({1})", LanguageInfo.StatusSlider_btn_Reset, arg);
		}

		void IStatusBar.ShowWarningInfo(IStatusWarningInfo info)
		{
			WarningIconWidget warningIconWidget = new WarningIconWidget(info);
			this.hbox_warningIcon.RemoveAll();
			this.hbox_warningIcon.PackStart(warningIconWidget);
			warningIconWidget.Show();
		}

		void IStatusBar.HideWarningInfo()
		{
			this.hbox_warningIcon.RemoveAll();
		}

		private void SetDefaultValue()
		{
			this.label_GUIName.Text = string.Empty;
			this.label_PositionValue.Text = string.Empty;
			this.label_RotationValue.Text = string.Empty;
			this.label_ScaleValue.Text = string.Empty;
			this.label_PositionName.Text = string.Empty;
			this.label_RotationName.Text = string.Empty;
			this.label_ScaleName.Text = string.Empty;
		}

		private void SetComboboxValue(double newCanvasZoom)
		{
			this.isImport = false;
			this.combobox_Zoom.Entry.Text = ((int)newCanvasZoom).ToString() + "%";
			this.isImport = true;
		}

		private void GameObjectInfo_2D(VisualObject gameObject)
		{
			this.label_GUIName.Text = gameObject.DisplayName;
			this.label_PositionName.Text = LanguageInfo.TextBlock_Position;
			this.label_RotationName.Text = LanguageInfo.MainTool_Rotation;
			this.label_ScaleName.Text = LanguageInfo.MainTool_Scale;
			float validPointValue = this.GetValidPointValue(gameObject.Position.X);
			float validPointValue2 = this.GetValidPointValue(gameObject.Position.Y);
			this.label_PositionValue.Text = string.Format("X: {0}   Y: {1}", validPointValue.ToString("0.00"), validPointValue2.ToString("0.00"));
			this.label_RotationValue.Text = gameObject.Rotation.ToString("0.00");
			string arg = this.GetValidPointValue(gameObject.Scale.ScaleX * 100f).ToString("0.00") + "%";
			string arg2 = this.GetValidPointValue(gameObject.Scale.ScaleY * 100f).ToString("0.00") + "%";
			this.label_ScaleValue.Text = string.Format("X: {0}   Y: {1}", arg, arg2);
		}

		private void GameObjectInfo_3D(Node3DObject node3DObject)
		{
			if (node3DObject != null)
			{
				this.label_GUIName.Text = node3DObject.DisplayName;
				this.label_PositionName.Text = LanguageInfo.TextBlock_Position;
				this.label_RotationName.Text = LanguageInfo.MainTool_Rotation;
				this.label_ScaleName.Text = LanguageInfo.MainTool_Scale;
				this.label_PositionValue.Text = string.Format("X: {0}   Y: {1}   Z: {2}", this.GetValidPointValue(node3DObject.Position3D.X).ToString("0.000"), this.GetValidPointValue(node3DObject.Position3D.Y).ToString("0.000"), this.GetValidPointValue(node3DObject.Position3D.Z).ToString("0.000"));
				this.label_RotationValue.Text = string.Format("X: {0}   Y: {1}   Z: {2}", this.GetValidPointValue(node3DObject.Rotation3D.X).ToString("0.000"), this.GetValidPointValue(node3DObject.Rotation3D.Y).ToString("0.000"), this.GetValidPointValue(node3DObject.Rotation3D.Z).ToString("0.000"));
				this.label_ScaleValue.Text = string.Format("X: {0}   Y: {1}   Z: {2}      ", this.GetValidPointValue(node3DObject.Scale3D.X).ToString("0.000"), this.GetValidPointValue(node3DObject.Scale3D.Y).ToString("0.000"), this.GetValidPointValue(node3DObject.Scale3D.Z).ToString("0.000"));
			}
		}

		private float GetValidPointValue(float value)
		{
			float result;
			if (value > 9999999f)
			{
				result = 9999999f;
			}
			else if (value < -9999999f)
			{
				result = -9999999f;
			}
			else
			{
				result = value;
			}
			return result;
		}

		private void EntrySetZoom()
		{
			if (!this.combobox_Zoom.Entry.Text.Contains("%"))
			{
				if (string.IsNullOrEmpty(this.combobox_Zoom.Entry.Text))
				{
					this.combobox_Zoom.Entry.Text = this.CanvasZoom.ToString();
				}
				else
				{
					if ((double)int.Parse(this.combobox_Zoom.Entry.Text) < this.minZoom)
					{
						this.combobox_Zoom.Entry.Text = this.minZoom.ToString();
					}
					else if ((double)int.Parse(this.combobox_Zoom.Entry.Text) > this.maxZoom)
					{
						this.combobox_Zoom.Entry.Text = this.maxZoom.ToString();
					}
					this.CanvasZoom = Convert.ToDouble(this.combobox_Zoom.Entry.Text);
					this.SetComboboxValue(this.CanvasZoom);
				}
			}
		}

		private void ResetCanvasCmd_Execute(object sender, CommandRunArgs e)
		{
			this.CanvasZoom = 100.0;
			if (ViewModeManager.Instance.Current != null)
			{
				ViewModeManager.Instance.Current.ResetView();
			}
		}

		private void button_reset_Clicked(object sender, EventArgs e)
		{
			GlobalCommand.ResetCanvasCmd.RaiseExecute(null);
		}

		private void Entry_WidgetEvent(object o, WidgetEventArgs args)
		{
			EventType type = args.Event.Type;
			if (type == EventType.ButtonPress)
			{
				if (this.combobox_Zoom.Entry.Text.Contains("%"))
				{
					this.combobox_Zoom.Entry.Text = this.combobox_Zoom.Entry.Text.Substring(0, this.combobox_Zoom.Entry.Text.Length - 1);
				}
			}
		}

		private void combobox_Zoom_Changed(object sender, EventArgs e)
		{
			if ((sender as ComboBox).ActiveText != null && !this.isRecivingMsg && !this.isVersaRecivingMsg && !this.combobox_Zoom.Entry.IsFocus)
			{
				string activeText = (sender as ComboBox).ActiveText;
				if (activeText.Contains("%"))
				{
					double canvasZoom = Convert.ToDouble(activeText.Substring(0, activeText.Length - 1));
					this.CanvasZoom = canvasZoom;
				}
			}
		}

		private void Entry_Changed(object sender, EventArgs e)
		{
			if (this.combobox_Zoom.Entry.IsFocus && this.combobox_Zoom.Entry.Text.Length > 0 && this.isImport)
			{
				for (int i = 0; i < this.combobox_Zoom.Entry.Text.Length; i++)
				{
					if (this.combobox_Zoom.Entry.Text[i] < '0' || this.combobox_Zoom.Entry.Text[i] > '9' || i > 3)
					{
						this.combobox_Zoom.Entry.Text = this.combobox_Zoom.Entry.Text.Remove(i, 1);
						if (!string.IsNullOrWhiteSpace(this.combobox_Zoom.Entry.Text) && !this.combobox_Zoom.Entry.Text.Contains("%"))
						{
							double canvasZoom = Convert.ToDouble(this.combobox_Zoom.Entry.Text);
							this.CanvasZoom = canvasZoom;
						}
					}
				}
			}
		}

		private void combobox_Zoom_FocusOutEvent(object o, FocusOutEventArgs args)
		{
			this.EntrySetZoom();
		}

		private void Entry_KeyReleaseEvent(object o, KeyReleaseEventArgs args)
		{
			if ((args.Event.Key == Gdk.Key.Return || args.Event.Key == Gdk.Key.KP_Enter) && this.combobox_Zoom.Entry.IsFocus)
			{
				this.EntrySetZoom();
			}
		}

		private void hscale_Zoom_ValueChanged(object sender, EventArgs e)
		{
			HScale hscale = sender as HScale;
			float zoomDelta = (float)((hscale.Value - this.oldCanvasZoom) / 100.0);
			this.oldCanvasZoom = hscale.Value;
			if (!this.isRecivingMsg)
			{
				this.isVersaRecivingMsg = true;
				this.eventAggregator.GetEvent<CanvasZoomChangeEvent>().Publish(new CanvasZoomChangeEventArgs(zoomDelta));
				this.SetComboboxValue(hscale.Value);
				this.isVersaRecivingMsg = false;
			}
		}

		private void ProjectOperations_CurrentDocumentChanged(object sender, ProjectsOperations.ProjectEventArgs e)
		{
			if (this.SelectedObjects != null && this.SelectedObjects.Count == 1)
			{
				this.SelectedObjects[0].PropertyChanged -= this.gameObject_PropertyChanged;
			}
			if (e.Project != null)
			{
				CocosFile cocosFile = e.Project.CocosFile;
				if (cocosFile != null)
				{
					this.SetDefaultValue();
					if (cocosFile.Type.Equals(NodeType.Plist.ToString()) && this.isShow)
					{
						this.isShow = false;
						this.eventbox_bg.Remove(this.alignment_main);
						base.ShowAll();
					}
					else if (!cocosFile.Type.Equals(NodeType.Plist.ToString()) && !this.isShow)
					{
						this.isShow = true;
						this.eventbox_bg.Add(this.alignment_main);
						base.ShowAll();
					}
				}
				if (e.Project.Is3DFile())
				{
					this.Is3DCocosItem = true;
				}
				else
				{
					this.Is3DCocosItem = false;
					this.SetDefaultValue();
				}
			}
		}

		private void ProjectOperations_CurrentSelectedSolutionChanged(object sender, SolutionEventArgs e)
		{
			if (e.Solution == null)
			{
				this.CanvasZoom = 100.0;
				this.SetDefaultValue();
				this.hbox_canvas.Sensitive = false;
			}
			else
			{
				this.hbox_canvas.Sensitive = true;
			}
		}

		private void CanvasZoomChangeEventHandle(CanvasZoomChangeEventArgs obj)
		{
			this.isRecivingMsg = true;
			double num = this.CanvasZoom + (double)(obj.ZoomDelta * 100f);
			if (!this.isVersaRecivingMsg)
			{
				if (num < this.minZoom)
				{
					this.CanvasZoom = this.minZoom;
				}
				else if (num > this.maxZoom)
				{
					this.CanvasZoom = this.maxZoom;
				}
				else
				{
					this.CanvasZoom = num;
				}
				this.SetComboboxValue(this.CanvasZoom);
			}
			this.isRecivingMsg = false;
		}

		private void SelectedObjectsChangeEventHandle(SelectedVisualObjectsChangeEventArgs obj)
		{
			if (this.SelectedObjects != null && this.SelectedObjects.Count == 1)
			{
				VisualObject visualObject = this.SelectedObjects[0];
				this.SelectedObjects[0].PropertyChanged -= this.gameObject_PropertyChanged;
			}
			this.SelectedObjects = obj.SelectedObject;
			if (obj.SelectedObject.Count == 1)
			{
				if (!this.Is3DCocosItem)
				{
					VisualObject visualObject = obj.SelectedObject[0];
					this.GameObjectInfo_2D(visualObject);
					visualObject.PropertyChanged += this.gameObject_PropertyChanged;
				}
				else
				{
					Node3DObject node3DObject = obj.SelectedObject[0] as Node3DObject;
					if (node3DObject == null)
					{
						this.SetDefaultValue();
						if (obj.SelectedObject.Count > 1)
						{
							this.label_GUIName.Text = string.Format(LanguageInfo.StatusBar_SelectedCount, obj.SelectedObject.Count);
						}
					}
					else
					{
						this.GameObjectInfo_3D(node3DObject);
						node3DObject.PropertyChanged += this.gameObject_PropertyChanged;
					}
				}
			}
			else
			{
				this.SetDefaultValue();
				if (obj.SelectedObject.Count > 1)
				{
					this.label_GUIName.Text = string.Format(LanguageInfo.StatusBar_SelectedCount, obj.SelectedObject.Count);
				}
			}
		}

		private void gameObject_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (!this.Is3DCocosItem)
			{
				if (e.PropertyName == "Position" || e.PropertyName == "Scale" || e.PropertyName == "Rotation" || e.PropertyName == "SkinPosition" || e.PropertyName == "SkinScale" || e.PropertyName == "SkinRotation" || e.PropertyName == "PropertyRotation" || e.PropertyName == "DisplayName")
				{
					VisualObject gameObject = sender as VisualObject;
					this.GameObjectInfo_2D(gameObject);
				}
			}
			else if (e.PropertyName == "Position3D" || e.PropertyName == "Scale3D" || e.PropertyName == "Rotation3D" || e.PropertyName == "DisplayName")
			{
				Node3DObject node3DObject = sender as Node3DObject;
				this.GameObjectInfo_3D(node3DObject);
			}
		}

		private void Entry_ScrollEvent(object o, ScrollEventArgs args)
		{
			double num = this.CanvasZoom;
			if (args.Event.Direction == ScrollDirection.Up)
			{
				if (num < this.maxZoom)
				{
					num += 1.0;
				}
			}
			else if (num > this.minZoom)
			{
				num -= 1.0;
			}
			this.CanvasZoom = num;
		}

		private EventBox eventbox_bg;

		private Alignment alignment_main;

		private HBox hbox_main;

		private HBox hbox_warningIcon;

		private HBox hbox_canvas;

		private HScale hscale_Zoom;

		private ComboBoxEntry combobox_Zoom;

		private HBox hbox_scale;

		private Label label_ScaleName;

		private Label label_ScaleValue;

		private HBox hbox_rotate;

		private Label label_RotationName;

		private Label label_RotationValue;

		private HBox hbox_pos;

		private Label label_PositionName;

		private Label label_PositionValue;

		private Label label_GUIName;

		private List<string> ZoomDefaultList = new List<string>
		{
			"10%",
			"20%",
			"50%",
			"75%",
			"100%",
			"150%",
			"200%",
			"400%",
			"500%"
		};

		private string projectName = string.Empty;

		private double oldCanvasZoom = 100.0;

		private double minZoom = 10.000000149011612;

		private double maxZoom = 500.0;

		private bool isRecivingMsg;

		private bool isVersaRecivingMsg;

		private bool isImport;

		private IEventAggregator eventAggregator;

		private ReadOnlyCollection<VisualObject> SelectedObjects;

		private IconButton button_reset;

		private bool isShow = true;

		private bool is3DCocosItem = false;
	}
}
