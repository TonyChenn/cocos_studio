using System;
using CocoStudio.Core;
using CocoStudio.Model.Interface;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Mono.Addins;

namespace CocoStudio.Model
{
	// Token: 0x020000DB RID: 219
	[Extension(Type = typeof(IDragOperation))]
	public class DragOperation2D : DragOperation
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x0001B3E4 File Offset: 0x000195E4
		protected override AbstractNodeObject CreateObjectFromFile(ResourceItem resourceFile)
		{
			AbstractNodeObject abstractNodeObject = null;
			if (resourceFile is ImageFile || resourceFile is PlistImageFile)
			{
				abstractNodeObject = new SpriteObject(resourceFile as ResourceFile);
			}
			else if (resourceFile is CocosItem)
			{
				CocosItem cocosItem = resourceFile as CocosItem;
				if (cocosItem.IsGameFile())
				{
					abstractNodeObject = new FileNodeObject(cocosItem);
				}
			}
			else if (resourceFile is AudioFile)
			{
				abstractNodeObject = new SimpleAudioObject(resourceFile as ResourceFile);
			}
			if (abstractNodeObject != null)
			{
				abstractNodeObject.Name = Services.ProjectOperations.CurrentSelectedProject.CreateObjectName(abstractNodeObject, "");
			}
			return abstractNodeObject;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001B49C File Offset: 0x0001969C
		protected override void AddChildToTarget(AbstractNodeObject childNode, PointF coord, AbstractNodeObject target)
		{
			if (childNode != null && target != null)
			{
				string taskName = base.GetType().Name + "CreateObject";
				using (CompositeTask.Run(taskName, null))
				{
					PointF scene = GameWindow.Current.ConvertControlToScene(coord);
					childNode.Position = target.TransformSceneForChild(scene);
					target.Children.Add(childNode);
				}
			}
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x0001B528 File Offset: 0x00019728
		public override bool CanHandle(CocosItem project)
		{
			return project.Is2DFile();
		}
	}
}
