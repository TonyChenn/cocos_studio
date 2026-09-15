using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	internal class ThreeNumberEditor : BaseEditor
	{
		public override bool SupportMultiSelect
		{
			get
			{
				return true;
			}
		}

		public ThreeNumberEditor()
		{
			this.xHeadText = "X";
			this.yHeadText = "Y";
			this.zHeadText = "Z";
		}

		public ThreeNumberEditor(string xHead, string yHead, string zHead)
		{
			this.xHeadText = xHead;
			this.yHeadText = yHead;
			this.zHeadText = zHead;
		}

		protected override Widget OnCreateWidget()
		{
			this.xInnerEntry = new NoUndoNumEntry();
			this.xInnerEntry.DecimalPlaces = 3;
			FullEntryShell widget = EntryShellBuilder.CreateShell(this.xHeadText, this.xInnerEntry);
			this.yInnerEntry = new NoUndoNumEntry();
			this.yInnerEntry.DecimalPlaces = 3;
			FullEntryShell widget2 = EntryShellBuilder.CreateShell(this.yHeadText, this.yInnerEntry);
			this.zInnerEntry = new NoUndoNumEntry();
			this.zInnerEntry.DecimalPlaces = 3;
			FullEntryShell widget3 = EntryShellBuilder.CreateShell(this.zHeadText, this.zInnerEntry);
			HBox hbox = new HBox();
			hbox.Spacing = 4;
			hbox.PackStart(widget);
			hbox.PackStart(widget2);
			hbox.PackStart(widget3);
			hbox.ShowAll();
			base.SetControl();
			this.xInnerEntry.EntryValueChanged += this.XEntryValueChangedHandler;
			this.yInnerEntry.EntryValueChanged += this.YEntryValueChangedHandler;
			this.zInnerEntry.EntryValueChanged += this.ZEntryValueChangedHandler;
			return hbox;
		}

		protected override void OnSetControl()
		{
			Point3F point3F = (Point3F)base.PropertyItem.Values[0];
			this.xInnerEntry.Value = point3F.X;
			this.yInnerEntry.Value = point3F.Y;
			this.zInnerEntry.Value = point3F.Z;
		}

		protected virtual void OnXValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.X = e.Value;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		protected virtual void OnYValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.Y = e.Value;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		protected virtual void OnZValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.Z = e.Value;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		protected void CompareNumber(Func<Node3DObject, Node3DObject, bool> funcX, Func<Node3DObject, Node3DObject, bool> funcY, Func<Node3DObject, Node3DObject, bool> funcZ)
		{
			Point3F point3F = (Point3F)base.PropertyItem.Values[0];
			if (PropertyItem.Objects.Count <= 1)
			{
				object firstObject = PropertyItem.FirstObject;
				this.xInnerEntry.Value = point3F.X;
				this.yInnerEntry.Value = point3F.Y;
				this.zInnerEntry.Value = point3F.Z;
				return;
			}
			if (base.IsWhipNode<Node3DObject>(funcX))
			{
				this.xInnerEntry.SetToSubState();
			}
			else
			{
				this.xInnerEntry.Value = point3F.X;
			}
			if (base.IsWhipNode<Node3DObject>(funcY))
			{
				this.yInnerEntry.SetToSubState();
			}
			else
			{
				this.yInnerEntry.Value = point3F.Y;
			}
			if (base.IsWhipNode<Node3DObject>(funcZ))
			{
				this.zInnerEntry.SetToSubState();
				return;
			}
			this.zInnerEntry.Value = point3F.Z;
		}

		private void XEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnXValueChanged(e);
		}

		private void YEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnYValueChanged(e);
		}

		private void ZEntryValueChangedHandler(object sender, EntryIntEventArgs e)
		{
			this.OnZValueChanged(e);
		}

		protected NoUndoNumEntry xInnerEntry;

		protected NoUndoNumEntry yInnerEntry;

		protected NoUndoNumEntry zInnerEntry;

		private string xHeadText;

		private string yHeadText;

		private string zHeadText;
	}
}
