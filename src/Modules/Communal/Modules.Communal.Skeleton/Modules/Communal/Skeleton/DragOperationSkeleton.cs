using System;
using CocoStudio.Core;
using CocoStudio.Model;
using CocoStudio.Model.Interface;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using CocoStudio.Projects;
using Gtk;
using Modules.Communal.MultiLanguage;
using Mono.Addins;

namespace Modules.Communal.Skeleton
{
	[Extension(typeof(IDragOperation))]
	public class DragOperationSkeleton : DragOperation
	{
		protected override string CanDragDropArgs(DragDropArgs e)
		{
			ResourceInfoDragData resourceInfoDragData = e.Context.GetDragData() as ResourceInfoDragData;
			foreach (ResourceItem resourceItem in resourceInfoDragData.Items)
			{
				CocosItem cocosItem = resourceItem as CocosItem;
				if (cocosItem != null)
				{
					return LanguageInfo.MessageBox206_NestedSkeletonError;
				}
			}
			return null;
		}

		protected override AbstractNodeObject CreateObjectFromFile(ResourceItem resourceFile)
		{
			AbstractNodeObject abstractNodeObject = null;
			if (resourceFile is ImageFile || resourceFile is PlistImageFile)
			{
				abstractNodeObject = new SpriteObject(resourceFile as ResourceFile);
			}
			else if (resourceFile is AudioFile)
			{
				abstractNodeObject = new SimpleAudioObject(resourceFile as ResourceFile);
			}
			if (abstractNodeObject != null)
			{
				abstractNodeObject.Name = Services.Workbench.ActiveDocument.File.CreateObjectName(abstractNodeObject, "");
			}
			return abstractNodeObject;
		}

		protected override void AddChildToTarget(AbstractNodeObject childNode, PointF coord, AbstractNodeObject target)
		{
			if (childNode == null || target == null)
			{
				return;
			}
			NodeObject nodeObject = childNode as NodeObject;
			if (nodeObject != null)
			{
				PointF scene = GameWindow.Current.ConvertControlToScene(coord);
				nodeObject.Position = target.TransformSceneForChild(scene);
			}
			target.Children.Add(childNode);
		}

		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.IsSkeletonFile();
		}
	}
}
