using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gtk;
using Mono.Addins;

namespace CocoStudio.Model.Interface
{
	[TypeExtensionPoint]
	public interface IDragOperation
	{
		void DragEnter(DragMotionArgs e, VisualObject target);

		void DragLeave(DragMotionArgs e, VisualObject target);

		bool DragOver(DragMotionArgs e, VisualObject target);

		void DragDrop(DragDropArgs e, VisualObject target);

		bool CanHandle(CocosItem project);
	}
}
