using Gtk;
using Stetic;

namespace MonoDevelop.SourceEditor
{
	internal class PrintSettingsWidget : Bin
	{
		public PrintSettingsWidget()
		{
			Build();
		}

		protected virtual void Build()
		{
			Stetic.Gui.Initialize(this);
			Stetic.BinContainer.Attach(this);
			base.Name = "MonoDevelop.SourceEditor.PrintSettingsWidget";
			if (base.Child != null)
			{
				base.Child.ShowAll();
			}
			Hide();
		}
	}
}
