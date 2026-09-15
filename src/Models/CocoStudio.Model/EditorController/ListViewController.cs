using System;
using System.Collections.Generic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using Modules.Communal.PropertyGrid;
using Mono.Addins;

namespace CocoStudio.Model.EditorController
{
	[Extension(typeof(IEditorController))]
	internal class ListViewController : Base2DController
	{
		public ListViewController()
		{
			base.AddCorrespondProperty("DirectionType");
		}

		public override void RefreshEditor(IReadOnlyList<object> selectedObjs, string propertyName)
		{
			IPropertyGrid service = Services.GetService<IPropertyGrid>();
			IPropertyEditor editor = service.GetEditor("HorizontalType");
			IPropertyEditor editor2 = service.GetEditor("VerticalType");
			if (editor != null && editor2 != null)
			{
				ListViewDirectionType? listViewDirectionType = null;
				foreach (object obj in selectedObjs)
				{
					ListViewObject listViewObject = obj as ListViewObject;
					if (listViewObject == null)
					{
						listViewDirectionType = null;
						break;
					}
					if (listViewDirectionType == null)
					{
						listViewDirectionType = new ListViewDirectionType?(listViewObject.DirectionType);
					}
					else if (listViewDirectionType != listViewObject.DirectionType)
					{
						listViewDirectionType = null;
						break;
					}
				}
				if (listViewDirectionType == null)
				{
					editor.Visible = false;
					editor2.Visible = false;
				}
				else if (listViewDirectionType == ListViewDirectionType.Horizontal)
				{
					editor.Visible = false;
					editor2.Visible = true;
				}
				else if (listViewDirectionType == ListViewDirectionType.Vertical)
				{
					editor.Visible = true;
					editor2.Visible = false;
				}
			}
		}
	}
}
