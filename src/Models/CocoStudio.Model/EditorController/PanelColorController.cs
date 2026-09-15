using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class PanelColorController : Base2DController
	{
		public PanelColorController()
		{
			base.AddCorrespondProperty("ComboBoxType");
		}

		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("BackColorAlpha");
			IPropertyEditor editor2 = service.GetEditor("SingleColor");
			IPropertyEditor editor3 = service.GetEditor("FirstColor");
			IPropertyEditor editor4 = service.GetEditor("EndColor");
			IPropertyEditor editor5 = service.GetEditor("ColorAngle");
			if (editor != null && editor2 != null && editor3 != null && editor4 != null && editor5 != null)
			{
				int num = -1;
				foreach (object obj in selectedObjs)
				{
					PanelObject panelObject = obj as PanelObject;
					if (panelObject == null)
					{
						num = -1;
						break;
					}
					if (num == -1)
					{
						num = panelObject.ComboBoxIndex;
					}
					else if (num != panelObject.ComboBoxIndex)
					{
						num = -1;
						break;
					}
				}
				if (num == 1)
				{
					editor.Visible = true;
					editor2.Visible = true;
					editor3.Visible = false;
					editor4.Visible = false;
					editor5.Visible = false;
				}
				else if (num == 2)
				{
					editor.Visible = true;
					editor2.Visible = false;
					editor3.Visible = true;
					editor4.Visible = true;
					editor5.Visible = true;
				}
				else
				{
					editor.Visible = false;
					editor2.Visible = false;
					editor3.Visible = false;
					editor4.Visible = false;
					editor5.Visible = false;
				}
			}
		}
	}
}
