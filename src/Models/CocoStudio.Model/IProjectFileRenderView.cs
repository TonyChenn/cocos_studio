using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;

namespace CocoStudio.Model
{
	public interface IProjectFileRenderView
	{
		void ChangeView(CanvasObject canvas, CocosItem project);
	}
}
