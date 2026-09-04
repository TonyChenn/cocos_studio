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
	// Token: 0x0200001F RID: 31
	[Extension(Type = typeof(IDragOperation))]
	public class DragOperation3D : DragOperation
	{
		// Token: 0x060000FA RID: 250 RVA: 0x00004F9C File Offset: 0x0000319C
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

		// Token: 0x060000FB RID: 251 RVA: 0x00005040 File Offset: 0x00003240
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

		// Token: 0x060000FC RID: 252 RVA: 0x000050A4 File Offset: 0x000032A4
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

		// Token: 0x060000FD RID: 253 RVA: 0x000050F1 File Offset: 0x000032F1
		public override bool CanHandle(CocosItem item)
		{
			return item.Is3DFile();
		}
	}
}
