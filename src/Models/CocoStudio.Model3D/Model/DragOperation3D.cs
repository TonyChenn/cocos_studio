using System;
using CocoStudio.Core;
using CocoStudio.Model.Interface;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace CocoStudio.Model
{
	[Extension(Type = typeof(IDragOperation))]
	public class DragOperation3D : DragOperation
	{
		protected override string CanDragDropArgs(DragDropArgs e)
		{
			ResourceInfoDragData resourceInfoDragData = e.Context.GetDragData() as ResourceInfoDragData;
			CocosFile cocosFile = Services.ProjectOperations.CurrentSelectedProject.CocosFile;
			foreach (ResourceItem resourceItem in resourceInfoDragData.Items)
			{
				CocosItem cocosItem = resourceItem as CocosItem;
				if (cocosItem != null)
				{
					return LanguageInfo.MessageBox206_NestedSence3DError;
				}
				if (!(resourceItem is MeshFile) && !(resourceItem is PuFile) && !(resourceItem is Sprite3DFile))
				{
					return LanguageInfo.MessageBox245_2DObjectIn3DScene;
				}
			}
			return null;
		}

		protected override AbstractNodeObject CreateObjectFromFile(ResourceItem resourceFile)
		{
			AbstractNodeObject abstractNodeObject = null;
			if (resourceFile is MeshFile || resourceFile is Sprite3DFile)
			{
				return new Sprite3DObject(resourceFile as ResourceFile);
			}
			if (resourceFile is PuFile)
			{
				return new Particle3DObject(resourceFile as ResourceFile);
			}
			if (abstractNodeObject != null)
			{
				abstractNodeObject.Name = Services.ProjectOperations.CurrentSelectedProject.CreateObjectName(abstractNodeObject, "");
			}
			return abstractNodeObject;
		}

		protected override void AddChildToTarget(AbstractNodeObject childNode, PointF coord, AbstractNodeObject target)
		{
			if (childNode == null || target == null)
			{
				return;
			}
			Node3DObject node3DObject = childNode as Node3DObject;
			if (node3DObject != null)
			{
				CameraObject camera = GameWindow.Current.GetSceneObject().GetCamera();
				node3DObject.Position3D = camera.ConvertControlToWorld3D(coord, camera.Distance);
				target.Children.Add(node3DObject);
			}
		}

		public override bool CanHandle(CocosItem item)
		{
			return item.Is3DFile();
		}
	}
}
