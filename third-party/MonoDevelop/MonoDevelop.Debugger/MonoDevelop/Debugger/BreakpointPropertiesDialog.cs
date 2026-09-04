using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GLib;
using Gdk;
using Gtk;
using ICSharpCode.NRefactory.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem.Implementation;
using Mono.Debugging.Client;
using MonoDevelop.Components;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Ide;
using MonoDevelop.Ide.CodeCompletion;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;
using Xwt;
using Xwt.Drawing;

namespace MonoDevelop.Debugger
{
	internal sealed class BreakpointPropertiesDialog : Xwt.Dialog
	{
		private class ImageViewWithTooltip : Xwt.Widget
		{
			private string tip;

			private Xwt.Drawing.Image icon;

			private Xwt.ImageView image;

			private TooltipPopoverWindow tooltipWindow;

			private bool mouseOver;

			public string ToolTip
			{
				get
				{
					return tip;
				}
				set
				{
					tip = value;
					if (tooltipWindow != null)
					{
						if (!string.IsNullOrEmpty(tip))
						{
							tooltipWindow.Text = value;
						}
						else
						{
							HideTooltip();
						}
					}
					else if (!string.IsNullOrEmpty(tip) && mouseOver)
					{
						ShowTooltip();
					}
				}
			}

			public Xwt.Drawing.Image Image
			{
				get
				{
					return icon;
				}
				set
				{
					icon = value;
					image.Image = icon;
				}
			}

			public ImageViewWithTooltip(Xwt.Drawing.Image icon)
			{
				this.icon = icon;
				image = new Xwt.ImageView(icon);
				base.Content = image;
				base.MouseEntered += HandleEnterNotifyEvent;
				base.MouseExited += HandleLeaveNotifyEvent;
			}

			[ConnectBefore]
			private void HandleLeaveNotifyEvent(object sender, EventArgs e)
			{
				mouseOver = false;
				HideTooltip();
			}

			[ConnectBefore]
			private void HandleEnterNotifyEvent(object sender, EventArgs e)
			{
				mouseOver = true;
				ShowTooltip();
			}

			private bool ShowTooltip()
			{
				if (!string.IsNullOrEmpty(tip))
				{
					HideTooltip();
					tooltipWindow = new TooltipPopoverWindow();
					tooltipWindow.ShowArrow = true;
					tooltipWindow.Text = tip;
					Xwt.Rectangle screenBounds = base.ScreenBounds;
					tooltipWindow.ShowPopup((Gtk.Widget)Toolkit.CurrentEngine.GetNativeWidget(this), new Gdk.Rectangle((int)(base.ParentWindow.X - screenBounds.X), (int)(base.ParentWindow.Y - screenBounds.Y), (int)screenBounds.Width, (int)screenBounds.Height), PopupPosition.Bottom);
				}
				return false;
			}

			private void HideTooltip()
			{
				if (tooltipWindow != null)
				{
					tooltipWindow.Destroy();
					tooltipWindow = null;
				}
			}

			protected override void Dispose(bool disposing)
			{
				HideTooltip();
				base.Dispose(disposing);
			}
		}

		private class ParsedLocation
		{
			private int line;

			private int column;

			public string Warning { get; private set; }

			public bool IsValid => Warning == "";

			public string FileName { get; private set; }

			public int Line => line;

			public int Column => column;

			public void Update(string location)
			{
				if (string.IsNullOrWhiteSpace(location))
				{
					Warning = GettextCatalog.GetString("Enter location");
					return;
				}
				string[] array = location.Split(':');
				if (!File.Exists(array[0]))
				{
					if (array.Length <= 1 || !File.Exists(array[0] + ":" + array[1]))
					{
						Warning = GettextCatalog.GetString("File does not exist");
						return;
					}
					string[] array2 = new string[array.Length - 1];
					array2[0] = array[0] + ":" + array[1];
					for (int i = 2; i < array.Length; i++)
					{
						array2[i - 1] = array[i];
					}
					array = array2;
				}
				if (array.Length < 2)
				{
					Warning = GettextCatalog.GetString("Missing ':' for line declaration");
					return;
				}
				FileName = array[0];
				if (!int.TryParse(array[1], out line))
				{
					Warning = GettextCatalog.GetString("Line is not a number");
					return;
				}
				if (array.Length > 2 && !int.TryParse(array[2], out column))
				{
					Warning = GettextCatalog.GetString("Column is not a number");
					return;
				}
				column = 1;
				Warning = "";
			}

			public void Update(Breakpoint bp)
			{
				Update(bp.FileName, bp.Line, bp.Column);
			}

			public void Update(string filePath, int line, int column)
			{
				if (!File.Exists(filePath))
				{
					Warning = GettextCatalog.GetString("File does not exist");
				}
				else
				{
					Warning = "";
				}
				FileName = filePath;
				this.line = line;
				this.column = column;
			}

			public Breakpoint ToBreakpoint()
			{
				if (!IsValid)
				{
					throw new InvalidOperationException("Location is invalid.");
				}
				return new Breakpoint(FileName, line, column);
			}

			public override string ToString()
			{
				return FileName + ":" + line + ":" + column;
			}
		}

		private DialogButton buttonOk;

		private bool editing;

		private Xwt.HBox hboxFunction = new Xwt.HBox
		{
			MarginLeft = 18.0
		};

		private Xwt.HBox hboxLocation = new Xwt.HBox();

		private Xwt.HBox hboxException = new Xwt.HBox();

		private Xwt.HBox hboxCondition = new Xwt.HBox();

		private Xwt.VBox vboxException = new Xwt.VBox
		{
			MarginLeft = 18.0
		};

		private Xwt.VBox vboxLocation = new Xwt.VBox
		{
			MarginLeft = 18.0
		};

		private readonly Xwt.RadioButton breakpointActionPause = new Xwt.RadioButton(GettextCatalog.GetString("Pause the program"));

		private readonly Xwt.RadioButton breakpointActionPrint = new Xwt.RadioButton(GettextCatalog.GetString("Print a message and continue"));

		private readonly Xwt.RadioButton stopOnFunction = new Xwt.RadioButton(GettextCatalog.GetString("When a function is entered"));

		private readonly Xwt.RadioButton stopOnLocation = new Xwt.RadioButton(GettextCatalog.GetString("When a location is reached"));

		private readonly Xwt.RadioButton stopOnException = new Xwt.RadioButton(GettextCatalog.GetString("When an exception is thrown"));

		private readonly TextEntry entryFunctionName = new TextEntry
		{
			PlaceholderText = GettextCatalog.GetString("e.g. System.Object.ToString")
		};

		private readonly TextEntry entryLocationFile = new TextEntry
		{
			PlaceholderText = GettextCatalog.GetString("e.g. Program.cs:15:5")
		};

		private readonly TextEntryWithCodeCompletion entryExceptionType = new TextEntryWithCodeCompletion
		{
			PlaceholderText = GettextCatalog.GetString("e.g. System.InvalidOperationException")
		};

		private readonly TextEntry entryConditionalExpression = new TextEntry
		{
			PlaceholderText = GettextCatalog.GetString("e.g. colorName == \"Red\"")
		};

		private readonly TextEntry entryPrintExpression = new TextEntry
		{
			PlaceholderText = GettextCatalog.GetString("e.g. Value of 'name' is {name}")
		};

		private readonly ImageViewWithTooltip warningFunction = new ImageViewWithTooltip(ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Warning, Gtk.IconSize.Menu));

		private readonly ImageViewWithTooltip warningLocation = new ImageViewWithTooltip(ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Warning, Gtk.IconSize.Menu));

		private readonly ImageViewWithTooltip warningException = new ImageViewWithTooltip(ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Warning, Gtk.IconSize.Menu));

		private readonly ImageViewWithTooltip warningCondition = new ImageViewWithTooltip(ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Warning, Gtk.IconSize.Menu));

		private readonly ImageViewWithTooltip warningPrintExpression = new ImageViewWithTooltip(ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Warning, Gtk.IconSize.Menu));

		private readonly Xwt.SpinButton ignoreHitCount = new Xwt.SpinButton();

		private readonly Xwt.ComboBox ignoreHitType = new Xwt.ComboBox();

		private readonly Xwt.ComboBox conditionalHitType = new Xwt.ComboBox();

		private readonly CheckBox checkIncludeSubclass = new CheckBox(GettextCatalog.GetString("Include subclasses"));

		private readonly Xwt.Label printMessageTip = new Xwt.Label(GettextCatalog.GetString("Place simple C# expressions within {} to interpolate them."))
		{
			Sensitive = false,
			TextAlignment = Xwt.Alignment.End
		};

		private readonly Xwt.Label conditionalExpressionTip = new Xwt.Label(GettextCatalog.GetString("A C# boolean expression. Scope is local to the breakpoint."))
		{
			Sensitive = false,
			TextAlignment = Xwt.Alignment.End
		};

		private ParsedLocation breakpointLocation = new ParsedLocation();

		private BreakEvent be;

		private string[] parsedParamTypes;

		private string parsedFunction;

		private readonly HashSet<string> classes = new HashSet<string>();

		public BreakpointPropertiesDialog(BreakEvent be, BreakpointType breakpointType)
		{
			this.be = be;
			LoadExceptionList();
			Initialize();
			SetInitialData();
			SetLayout();
			if (be == null)
			{
				switch (breakpointType)
				{
				case BreakpointType.Location:
					stopOnLocation.Active = true;
					entryLocationFile.SetFocus();
					break;
				case BreakpointType.Function:
					stopOnFunction.Active = true;
					entryFunctionName.SetFocus();
					break;
				case BreakpointType.Catchpoint:
					stopOnException.Active = true;
					entryExceptionType.SetFocus();
					break;
				}
			}
		}

		private void Initialize()
		{
			base.Title = GettextCatalog.GetString((be == null) ? "Create a Breakpoint" : "Edit Breakpoint");
			string label = GettextCatalog.GetString((be == null) ? "Create" : "Apply");
			RadioButtonGroup radioButtonGroup = new RadioButtonGroup();
			breakpointActionPause.Group = radioButtonGroup;
			breakpointActionPrint.Group = radioButtonGroup;
			RadioButtonGroup radioButtonGroup2 = new RadioButtonGroup();
			stopOnFunction.Group = radioButtonGroup2;
			stopOnLocation.Group = radioButtonGroup2;
			stopOnException.Group = radioButtonGroup2;
			ignoreHitType.Items.Add(HitCountMode.None, GettextCatalog.GetString("Reset condition"));
			ignoreHitType.Items.Add(HitCountMode.LessThan, GettextCatalog.GetString("When hit count is less than"));
			ignoreHitType.Items.Add(HitCountMode.LessThanOrEqualTo, GettextCatalog.GetString("When hit count is less than or equal to"));
			ignoreHitType.Items.Add(HitCountMode.EqualTo, GettextCatalog.GetString("When hit count is equal to"));
			ignoreHitType.Items.Add(HitCountMode.GreaterThan, GettextCatalog.GetString("When hit count is greater than"));
			ignoreHitType.Items.Add(HitCountMode.GreaterThanOrEqualTo, GettextCatalog.GetString("When hit count is greater than or equal to"));
			ignoreHitType.Items.Add(HitCountMode.MultipleOf, GettextCatalog.GetString("When hit count is a multiple of"));
			ignoreHitCount.IncrementValue = 1.0;
			ignoreHitCount.Digits = 0;
			ignoreHitCount.ClimbRate = 1.0;
			ignoreHitCount.MinimumValue = 0.0;
			ignoreHitCount.MaximumValue = 2147483647.0;
			conditionalHitType.Items.Add(ConditionalHitWhen.ResetCondition, GettextCatalog.GetString("Reset condition"));
			conditionalHitType.Items.Add(ConditionalHitWhen.ConditionIsTrue, GettextCatalog.GetString("And the following condition is true"));
			conditionalHitType.Items.Add(ConditionalHitWhen.ExpressionChanges, GettextCatalog.GetString("And the following expression changes"));
			buttonOk = new DialogButton(label, Command.Ok)
			{
				Sensitive = false
			};
			radioButtonGroup2.ActiveRadioButtonChanged += OnUpdateControls;
			entryFunctionName.Changed += OnUpdateControls;
			entryLocationFile.Changed += OnUpdateControls;
			entryConditionalExpression.Changed += OnUpdateControls;
			ignoreHitType.SelectionChanged += OnUpdateControls;
			conditionalHitType.SelectionChanged += OnUpdateControls;
			breakpointActionPause.ActiveChanged += OnUpdateControls;
			breakpointActionPrint.ActiveChanged += OnUpdateControls;
			entryFunctionName.Changed += OnUpdateText;
			entryLocationFile.Changed += OnUpdateText;
			entryExceptionType.Changed += OnUpdateText;
			entryPrintExpression.Changed += OnUpdateText;
			buttonOk.Clicked += OnSave;
			CompletionWindowManager.WindowShown += HandleCompletionWindowShown;
			CompletionWindowManager.WindowClosed += HandleCompletionWindowClosed;
		}

		private void HandleCompletionWindowClosed(object sender, EventArgs e)
		{
			if (Toolkit.CurrentEngine.GetNativeWidget(vboxLocation) is Gtk.Widget widget && widget.Toplevel is Gtk.Window window)
			{
				window.Modal = true;
			}
		}

		private void HandleCompletionWindowShown(object sender, EventArgs e)
		{
			if (Toolkit.CurrentEngine.GetNativeWidget(vboxLocation) is Gtk.Widget widget && widget.Toplevel is Gtk.Window window)
			{
				window.Modal = false;
			}
		}

		private void SetInitialFunctionBreakpointData(FunctionBreakpoint fb)
		{
			stopOnLocation.Visible = false;
			vboxLocation.Visible = false;
			stopOnException.Visible = false;
			vboxException.Visible = false;
			stopOnFunction.Active = true;
			if (fb.ParamTypes != null)
			{
				entryFunctionName.Text = fb.FunctionName + " (" + string.Join(", ", fb.ParamTypes) + ")";
			}
			else
			{
				entryFunctionName.Text = fb.FunctionName;
			}
		}

		private void SetInitialBreakpointData(Breakpoint bp)
		{
			stopOnFunction.Visible = false;
			hboxFunction.Visible = false;
			stopOnException.Visible = false;
			vboxException.Visible = false;
			stopOnLocation.Active = true;
			breakpointLocation.Update(bp);
			entryLocationFile.Text = breakpointLocation.ToString();
			Project project = null;
			if (!string.IsNullOrEmpty(bp.FileName))
			{
				project = IdeApp.Workspace.GetProjectsContainingFile(bp.FileName).FirstOrDefault();
			}
			if (project != null)
			{
				SolutionEntityItem startupItem = project.ParentSolution.StartupItem;
				entryConditionalExpression.Sensitive = DebuggingService.IsFeatureSupported(project, DebuggerFeatures.ConditionalBreakpoints) || DebuggingService.IsFeatureSupported(startupItem, DebuggerFeatures.ConditionalBreakpoints);
				bool sensitive = DebuggingService.IsFeatureSupported(project, DebuggerFeatures.Tracepoints) || DebuggingService.IsFeatureSupported(startupItem, DebuggerFeatures.Tracepoints);
				breakpointActionPause.Sensitive = sensitive;
				entryPrintExpression.Sensitive = sensitive;
			}
		}

		private void SetInitialCatchpointData(Catchpoint cp)
		{
			stopOnFunction.Visible = false;
			hboxFunction.Visible = false;
			stopOnLocation.Visible = false;
			vboxLocation.Visible = false;
			stopOnException.Active = true;
			entryExceptionType.Text = cp.ExceptionName;
			checkIncludeSubclass.Active = cp.IncludeSubclasses;
		}

		private void SetInitialData()
		{
			if (be != null)
			{
				editing = true;
				if (be.HitCountMode == HitCountMode.None)
				{
					ignoreHitType.SelectedItem = HitCountMode.GreaterThanOrEqualTo;
					ignoreHitCount.Value = 0.0;
				}
				else
				{
					ignoreHitType.SelectedItem = be.HitCountMode;
					ignoreHitCount.Value = be.HitCount;
				}
				if ((be.HitAction & HitAction.Break) == HitAction.Break)
				{
					breakpointActionPause.Active = true;
				}
				else
				{
					breakpointActionPrint.Active = true;
					entryPrintExpression.Text = be.TraceExpression;
				}
				entryConditionalExpression.Text = be.ConditionExpression ?? "";
				conditionalHitType.SelectedItem = ((!be.BreakIfConditionChanges) ? ConditionalHitWhen.ConditionIsTrue : ConditionalHitWhen.ExpressionChanges);
				if (be is FunctionBreakpoint initialFunctionBreakpointData)
				{
					SetInitialFunctionBreakpointData(initialFunctionBreakpointData);
				}
				else if (be is Breakpoint initialBreakpointData)
				{
					SetInitialBreakpointData(initialBreakpointData);
				}
				if (be is Catchpoint initialCatchpointData)
				{
					SetInitialCatchpointData(initialCatchpointData);
				}
			}
			else
			{
				ignoreHitType.SelectedItem = HitCountMode.GreaterThanOrEqualTo;
				conditionalHitType.SelectedItem = ConditionalHitWhen.ConditionIsTrue;
				checkIncludeSubclass.Active = true;
				if (IdeApp.Workbench.ActiveDocument != null && IdeApp.Workbench.ActiveDocument.Editor != null && IdeApp.Workbench.ActiveDocument.FileName != FilePath.Null)
				{
					breakpointLocation.Update(IdeApp.Workbench.ActiveDocument.FileName, IdeApp.Workbench.ActiveDocument.Editor.Caret.Line, IdeApp.Workbench.ActiveDocument.Editor.Caret.Column);
					entryLocationFile.Text = breakpointLocation.ToString();
					stopOnLocation.Active = true;
				}
			}
		}

		private void SaveFunctionBreakpoint(FunctionBreakpoint fb)
		{
			fb.FunctionName = parsedFunction;
			fb.ParamTypes = parsedParamTypes;
		}

		private void SaveBreakpoint(Breakpoint bp)
		{
			bp.SetColumn(breakpointLocation.Column);
			bp.SetLine(breakpointLocation.Line);
		}

		private void OnSave(object sender, EventArgs e)
		{
			if (be == null)
			{
				if (stopOnFunction.Active)
				{
					be = new FunctionBreakpoint("", "C#");
				}
				else if (stopOnLocation.Active)
				{
					be = breakpointLocation.ToBreakpoint();
				}
				else
				{
					if (!stopOnException.Active)
					{
						return;
					}
					be = new Catchpoint(entryExceptionType.Text, checkIncludeSubclass.Active);
				}
			}
			if (be is FunctionBreakpoint fb)
			{
				SaveFunctionBreakpoint(fb);
			}
			if (be is Breakpoint bp)
			{
				SaveBreakpoint(bp);
			}
			if ((HitCountMode)ignoreHitType.SelectedItem == HitCountMode.GreaterThanOrEqualTo && (int)ignoreHitCount.Value == 0)
			{
				be.HitCountMode = HitCountMode.None;
			}
			else
			{
				be.HitCountMode = (HitCountMode)ignoreHitType.SelectedItem;
			}
			be.HitCount = ((be.HitCountMode != HitCountMode.None) ? ((int)ignoreHitCount.Value) : 0);
			if (!string.IsNullOrWhiteSpace(entryConditionalExpression.Text))
			{
				be.ConditionExpression = entryConditionalExpression.Text;
				be.BreakIfConditionChanges = conditionalHitType.SelectedItem.Equals(ConditionalHitWhen.ExpressionChanges);
			}
			else
			{
				be.ConditionExpression = null;
			}
			if (breakpointActionPrint.Active)
			{
				be.HitAction = HitAction.PrintExpression;
				be.TraceExpression = entryPrintExpression.Text;
			}
			else
			{
				be.HitAction = HitAction.Break;
			}
			be.CommitChanges();
		}

		private void OnUpdateControls(object sender, EventArgs e)
		{
			if (ignoreHitType.SelectedItem != null && (HitCountMode)ignoreHitType.SelectedItem == HitCountMode.None)
			{
				ignoreHitType.SelectedItem = HitCountMode.GreaterThanOrEqualTo;
				ignoreHitCount.Value = 0.0;
			}
			if (conditionalHitType.SelectedItem != null && (ConditionalHitWhen)conditionalHitType.SelectedItem == ConditionalHitWhen.ResetCondition)
			{
				conditionalHitType.SelectedItem = ConditionalHitWhen.ConditionIsTrue;
				entryConditionalExpression.Text = "";
			}
			hboxFunction.Sensitive = stopOnFunction.Active && DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints) && !editing;
			hboxLocation.Sensitive = stopOnLocation.Active && DebuggingService.IsFeatureSupported(DebuggerFeatures.Breakpoints) && !editing;
			hboxException.Sensitive = stopOnException.Active && DebuggingService.IsFeatureSupported(DebuggerFeatures.Catchpoints) && !editing;
			checkIncludeSubclass.Sensitive = stopOnException.Active && !editing;
			hboxCondition.Sensitive = DebuggingService.IsFeatureSupported(DebuggerFeatures.ConditionalBreakpoints);
			entryPrintExpression.Sensitive = breakpointActionPrint.Active && DebuggingService.IsFeatureSupported(DebuggerFeatures.Tracepoints);
			buttonOk.Sensitive = CheckValidity();
		}

		private void OnUpdateText(object sender, EventArgs e)
		{
			buttonOk.Sensitive = CheckValidity();
		}

		private bool CheckValidity()
		{
			warningFunction.Hide();
			warningLocation.Hide();
			warningException.Hide();
			warningCondition.Hide();
			warningPrintExpression.Hide();
			bool result = true;
			if (breakpointActionPrint.Active && string.IsNullOrWhiteSpace(entryPrintExpression.Text))
			{
				warningPrintExpression.Show();
				warningPrintExpression.ToolTip = GettextCatalog.GetString("Trace expression not specified");
				result = false;
			}
			if (stopOnFunction.Active)
			{
				string text = entryFunctionName.Text.Trim();
				if (stopOnFunction.Active)
				{
					if (text.Length == 0)
					{
						warningFunction.Show();
						warningFunction.ToolTip = GettextCatalog.GetString("Function name not specified");
						result = false;
					}
					if (!TryParseFunction(text, out parsedFunction, out parsedParamTypes))
					{
						warningFunction.Show();
						warningFunction.ToolTip = GettextCatalog.GetString("Invalid function syntax");
						result = false;
					}
				}
			}
			else if (stopOnLocation.Active)
			{
				breakpointLocation.Update(entryLocationFile.Text);
				if (!breakpointLocation.IsValid)
				{
					warningLocation.Show();
					warningLocation.ToolTip = breakpointLocation.Warning;
					result = false;
				}
			}
			else if (stopOnException.Active && !classes.Contains(entryExceptionType.Text))
			{
				warningException.Show();
				warningException.ToolTip = GettextCatalog.GetString("Exception not identified");
				result = false;
			}
			return result;
		}

		private static bool TryParseFunction(string signature, out string function, out string[] paramTypes)
		{
			int num = signature.IndexOf('(');
			int num2 = signature.IndexOf(')');
			if (num == -1 && num2 == -1)
			{
				function = signature;
				paramTypes = null;
				return true;
			}
			if (num2 != signature.Length - 1)
			{
				paramTypes = null;
				function = null;
				return false;
			}
			function = signature.Substring(0, num).Trim();
			num++;
			if (!FunctionBreakpoint.TryParseParameters(signature, num, num2, out paramTypes))
			{
				paramTypes = null;
				function = null;
				return false;
			}
			return true;
		}

		private void LoadExceptionList()
		{
			classes.Add("System.Exception");
			if (IdeApp.ProjectOperations.CurrentSelectedProject != null)
			{
				ICompilation compilation = TypeSystemService.GetCompilation(IdeApp.ProjectOperations.CurrentSelectedProject);
				foreach (ITypeDefinition subTypeDefinition in compilation.FindType(typeof(Exception)).GetSubTypeDefinitions())
				{
					classes.Add(subTypeDefinition.ReflectionName);
				}
			}
			else
			{
				IUnresolvedAssembly unresolvedAssembly = TypeSystemService.LoadAssemblyContext(Runtime.SystemAssemblyService.CurrentRuntime, TargetFramework.Default, typeof(Uri).Assembly.Location);
				IUnresolvedAssembly unresolvedAssembly2 = TypeSystemService.LoadAssemblyContext(Runtime.SystemAssemblyService.CurrentRuntime, TargetFramework.Default, typeof(object).Assembly.Location);
				if (unresolvedAssembly != null && unresolvedAssembly2 != null)
				{
					SimpleCompilation compilation2 = new SimpleCompilation(unresolvedAssembly, unresolvedAssembly2);
					foreach (ITypeDefinition subTypeDefinition2 in compilation2.FindType(typeof(Exception)).GetSubTypeDefinitions())
					{
						classes.Add(subTypeDefinition2.ReflectionName);
					}
				}
			}
			entryExceptionType.SetCodeCompletionList(classes.ToList());
		}

		public BreakEvent GetBreakEvent()
		{
			return be;
		}

		private void SetLayout()
		{
			Xwt.VBox vBox = new Xwt.VBox();
			vBox.MinWidth = 450.0;
			vBox.PackStart(new Xwt.Label(GettextCatalog.GetString("Breakpoint Action"))
			{
				Font = vBox.Font.WithWeight(FontWeight.Bold)
			});
			Xwt.VBox vBox2 = new Xwt.VBox();
			vBox2.MarginLeft = 12.0;
			Xwt.VBox vBox3 = vBox2;
			vBox3.PackStart(breakpointActionPause);
			vBox3.PackStart(breakpointActionPrint);
			Xwt.HBox hBox = new Xwt.HBox();
			hBox.MarginLeft = 18.0;
			Xwt.HBox hBox2 = hBox;
			hBox2.PackStart(entryPrintExpression, expand: true);
			hBox2.PackStart(warningPrintExpression);
			vBox3.PackStart(hBox2);
			vBox3.PackEnd(printMessageTip);
			vBox.PackStart(vBox3);
			vBox.PackStart(new Xwt.Label(GettextCatalog.GetString("When to Take Action"))
			{
				Font = vBox.Font.WithWeight(FontWeight.Bold)
			});
			Xwt.VBox vBox4 = new Xwt.VBox();
			vBox4.MarginLeft = 12.0;
			Xwt.VBox vBox5 = vBox4;
			vBox5.PackStart(stopOnFunction);
			hboxFunction.PackStart(entryFunctionName, expand: true);
			hboxFunction.PackEnd(warningFunction);
			vBox5.PackStart(hboxFunction);
			vBox5.PackStart(stopOnException);
			hboxException = new Xwt.HBox();
			hboxException.PackStart(entryExceptionType, expand: true);
			hboxException.PackEnd(warningException);
			vboxException.PackStart(hboxException);
			vboxException.PackStart(checkIncludeSubclass);
			vBox5.PackStart(vboxException);
			vBox5.PackStart(stopOnLocation);
			hboxLocation.PackStart(entryLocationFile, expand: true);
			hboxLocation.PackStart(warningLocation);
			vboxLocation.PackEnd(hboxLocation);
			vBox5.PackStart(vboxLocation);
			vBox.PackStart(vBox5);
			vBox.PackStart(new Xwt.Label(GettextCatalog.GetString("Advanced Conditions"))
			{
				Font = vBox.Font.WithWeight(FontWeight.Bold)
			});
			Xwt.VBox vBox6 = new Xwt.VBox();
			vBox6.MarginLeft = 30.0;
			Xwt.VBox vBox7 = vBox6;
			Xwt.HBox hBox3 = new Xwt.HBox();
			hBox3.PackStart(ignoreHitType, expand: true);
			hBox3.PackStart(ignoreHitCount);
			vBox7.PackStart(hBox3);
			vBox7.PackStart(conditionalHitType);
			hboxCondition = new Xwt.HBox();
			hboxCondition.PackStart(entryConditionalExpression, expand: true);
			hboxCondition.PackStart(warningCondition);
			vBox7.PackStart(hboxCondition);
			vBox7.PackEnd(conditionalExpressionTip);
			vBox.PackStart(vBox7);
			base.Buttons.Add(new DialogButton(Command.Cancel));
			base.Buttons.Add(buttonOk);
			base.Content = vBox;
			if (IdeApp.Workbench != null)
			{
				Gtk.Widget parent = ((Gtk.Widget)Toolkit.CurrentEngine.GetNativeWidget(vBox)).Parent;
				while (parent != null && !(parent is Gtk.Window))
				{
					parent = parent.Parent;
				}
				if (parent is Gtk.Window)
				{
					((Gtk.Window)parent).TransientFor = IdeApp.Workbench.RootWindow;
				}
			}
			OnUpdateControls(null, null);
		}

		protected override void Dispose(bool disposing)
		{
			CompletionWindowManager.WindowShown -= HandleCompletionWindowShown;
			CompletionWindowManager.WindowClosed -= HandleCompletionWindowClosed;
			Dispose(disposing);
		}
	}
}
