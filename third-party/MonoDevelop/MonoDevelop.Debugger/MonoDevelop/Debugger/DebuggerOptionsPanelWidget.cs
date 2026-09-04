using System;
using System.ComponentModel;
using Gtk;
using Mono.Debugging.Client;
using Mono.Unix;
using Stetic;

namespace MonoDevelop.Debugger
{
	[ToolboxItem(true)]
	public class DebuggerOptionsPanelWidget : Bin
	{
		private DebuggerSessionOptions options;

		private VBox vbox3;

		private CheckButton checkProjectCodeOnly;

		private CheckButton checkStepOverPropertiesAndOperators;

		private CheckButton checkAllowEval;

		private Alignment alignmentAllowToString;

		private CheckButton checkAllowToString;

		private CheckButton checkShowBaseGroup;

		private CheckButton checkGroupPrivate;

		private CheckButton checkGroupStatic;

		private Table tableEval;

		private Label label3;

		private Label labelEvalTimeout;

		private SpinButton spinTimeout;

		public DebuggerOptionsPanelWidget()
		{
			Build();
			options = DebuggingService.GetUserOptions();
			checkProjectCodeOnly.Active = options.ProjectAssembliesOnly;
			checkStepOverPropertiesAndOperators.Active = options.StepOverPropertiesAndOperators;
			checkAllowEval.Active = options.EvaluationOptions.AllowTargetInvoke;
			checkAllowToString.Active = options.EvaluationOptions.AllowToStringCalls;
			checkShowBaseGroup.Active = !options.EvaluationOptions.FlattenHierarchy;
			checkGroupPrivate.Active = options.EvaluationOptions.GroupPrivateMembers;
			checkGroupStatic.Active = options.EvaluationOptions.GroupStaticMembers;
			checkAllowToString.Sensitive = checkAllowEval.Active;
			spinTimeout.Value = options.EvaluationOptions.EvaluationTimeout;
		}

		public void Store()
		{
			EvaluationOptions evaluationOptions = options.EvaluationOptions;
			evaluationOptions.AllowTargetInvoke = checkAllowEval.Active;
			evaluationOptions.AllowToStringCalls = checkAllowToString.Active;
			evaluationOptions.FlattenHierarchy = !checkShowBaseGroup.Active;
			evaluationOptions.GroupPrivateMembers = checkGroupPrivate.Active;
			evaluationOptions.GroupStaticMembers = checkGroupStatic.Active;
			evaluationOptions.EvaluationTimeout = (int)spinTimeout.Value;
			options.StepOverPropertiesAndOperators = checkStepOverPropertiesAndOperators.Active;
			options.ProjectAssembliesOnly = checkProjectCodeOnly.Active;
			options.EvaluationOptions = evaluationOptions;
			DebuggingService.SetUserOptions(options);
		}

		protected virtual void OnCheckAllowEvalToggled(object sender, EventArgs e)
		{
			checkAllowToString.Sensitive = checkAllowEval.Active;
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			BinContainer.Attach(this);
			base.Name = "MonoDevelop.Debugger.DebuggerOptionsPanelWidget";
			vbox3 = new VBox();
			vbox3.Name = "vbox3";
			vbox3.Spacing = 6;
			vbox3.BorderWidth = 9u;
			checkProjectCodeOnly = new CheckButton();
			checkProjectCodeOnly.CanFocus = true;
			checkProjectCodeOnly.Name = "checkProjectCodeOnly";
			checkProjectCodeOnly.Label = Catalog.GetString("Debug project code only; do not step into framework code.");
			checkProjectCodeOnly.Active = true;
			checkProjectCodeOnly.DrawIndicator = true;
			checkProjectCodeOnly.UseUnderline = true;
			vbox3.Add(checkProjectCodeOnly);
			Box.BoxChild boxChild = (Box.BoxChild)vbox3[checkProjectCodeOnly];
			boxChild.Position = 0;
			boxChild.Expand = false;
			boxChild.Fill = false;
			checkStepOverPropertiesAndOperators = new CheckButton();
			checkStepOverPropertiesAndOperators.CanFocus = true;
			checkStepOverPropertiesAndOperators.Name = "checkStepOverPropertiesAndOperators";
			checkStepOverPropertiesAndOperators.Label = Catalog.GetString("Step over properties and operators");
			checkStepOverPropertiesAndOperators.Active = true;
			checkStepOverPropertiesAndOperators.DrawIndicator = true;
			checkStepOverPropertiesAndOperators.UseUnderline = true;
			vbox3.Add(checkStepOverPropertiesAndOperators);
			Box.BoxChild boxChild2 = (Box.BoxChild)vbox3[checkStepOverPropertiesAndOperators];
			boxChild2.Position = 1;
			boxChild2.Expand = false;
			boxChild2.Fill = false;
			checkAllowEval = new CheckButton();
			checkAllowEval.CanFocus = true;
			checkAllowEval.Name = "checkAllowEval";
			checkAllowEval.Label = Catalog.GetString("Allow implicit property evaluation and method invocation");
			checkAllowEval.Active = true;
			checkAllowEval.DrawIndicator = true;
			checkAllowEval.UseUnderline = true;
			vbox3.Add(checkAllowEval);
			Box.BoxChild boxChild3 = (Box.BoxChild)vbox3[checkAllowEval];
			boxChild3.Position = 2;
			boxChild3.Expand = false;
			boxChild3.Fill = false;
			alignmentAllowToString = new Alignment(0f, 0.5f, 1f, 1f);
			alignmentAllowToString.Name = "alignmentAllowToString";
			alignmentAllowToString.LeftPadding = 18u;
			checkAllowToString = new CheckButton();
			checkAllowToString.CanFocus = true;
			checkAllowToString.Name = "checkAllowToString";
			checkAllowToString.Label = Catalog.GetString("Call string-conversion function on objects in variables windows");
			checkAllowToString.Active = true;
			checkAllowToString.DrawIndicator = true;
			checkAllowToString.UseUnderline = true;
			alignmentAllowToString.Add(checkAllowToString);
			vbox3.Add(alignmentAllowToString);
			Box.BoxChild boxChild4 = (Box.BoxChild)vbox3[alignmentAllowToString];
			boxChild4.Position = 3;
			boxChild4.Expand = false;
			boxChild4.Fill = false;
			checkShowBaseGroup = new CheckButton();
			checkShowBaseGroup.CanFocus = true;
			checkShowBaseGroup.Name = "checkShowBaseGroup";
			checkShowBaseGroup.Label = Catalog.GetString("Show inherited class members in a base class group");
			checkShowBaseGroup.DrawIndicator = true;
			checkShowBaseGroup.UseUnderline = true;
			vbox3.Add(checkShowBaseGroup);
			Box.BoxChild boxChild5 = (Box.BoxChild)vbox3[checkShowBaseGroup];
			boxChild5.Position = 4;
			boxChild5.Expand = false;
			boxChild5.Fill = false;
			checkGroupPrivate = new CheckButton();
			checkGroupPrivate.CanFocus = true;
			checkGroupPrivate.Name = "checkGroupPrivate";
			checkGroupPrivate.Label = Catalog.GetString("Group non-public members");
			checkGroupPrivate.DrawIndicator = true;
			checkGroupPrivate.UseUnderline = true;
			vbox3.Add(checkGroupPrivate);
			Box.BoxChild boxChild6 = (Box.BoxChild)vbox3[checkGroupPrivate];
			boxChild6.Position = 5;
			boxChild6.Expand = false;
			boxChild6.Fill = false;
			checkGroupStatic = new CheckButton();
			checkGroupStatic.CanFocus = true;
			checkGroupStatic.Name = "checkGroupStatic";
			checkGroupStatic.Label = Catalog.GetString("Group static members");
			checkGroupStatic.DrawIndicator = true;
			checkGroupStatic.UseUnderline = true;
			vbox3.Add(checkGroupStatic);
			Box.BoxChild boxChild7 = (Box.BoxChild)vbox3[checkGroupStatic];
			boxChild7.Position = 6;
			boxChild7.Expand = false;
			boxChild7.Fill = false;
			tableEval = new Table(1u, 3u, homogeneous: false);
			tableEval.Name = "tableEval";
			tableEval.RowSpacing = 6u;
			tableEval.ColumnSpacing = 6u;
			label3 = new Label();
			label3.Name = "label3";
			label3.LabelProp = Catalog.GetString("ms");
			tableEval.Add(label3);
			Table.TableChild tableChild = (Table.TableChild)tableEval[label3];
			tableChild.LeftAttach = 2u;
			tableChild.RightAttach = 3u;
			tableChild.XOptions = AttachOptions.Fill;
			tableChild.YOptions = AttachOptions.Fill;
			labelEvalTimeout = new Label();
			labelEvalTimeout.Name = "labelEvalTimeout";
			labelEvalTimeout.Xalign = 0f;
			labelEvalTimeout.LabelProp = Catalog.GetString("Evaluation Timeout:");
			tableEval.Add(labelEvalTimeout);
			Table.TableChild tableChild2 = (Table.TableChild)tableEval[labelEvalTimeout];
			tableChild2.XOptions = AttachOptions.Fill;
			tableChild2.YOptions = AttachOptions.Fill;
			spinTimeout = new SpinButton(0.0, 1000000.0, 100.0);
			spinTimeout.CanFocus = true;
			spinTimeout.Name = "spinTimeout";
			spinTimeout.Adjustment.PageIncrement = 10.0;
			spinTimeout.ClimbRate = 100.0;
			spinTimeout.Numeric = true;
			tableEval.Add(spinTimeout);
			Table.TableChild tableChild3 = (Table.TableChild)tableEval[spinTimeout];
			tableChild3.LeftAttach = 1u;
			tableChild3.RightAttach = 2u;
			tableChild3.XOptions = AttachOptions.Fill;
			tableChild3.YOptions = AttachOptions.Fill;
			vbox3.Add(tableEval);
			Box.BoxChild boxChild8 = (Box.BoxChild)vbox3[tableEval];
			boxChild8.Position = 7;
			boxChild8.Expand = false;
			boxChild8.Fill = false;
			Add(vbox3);
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
			checkAllowEval.Toggled += OnCheckAllowEvalToggled;
		}
	}
}
