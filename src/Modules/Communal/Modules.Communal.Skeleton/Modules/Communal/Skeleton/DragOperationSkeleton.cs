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
	// Token: 0x02000002 RID: 2
	[Extension(typeof(IDragOperation))]
	public class DragOperationSkeleton : DragOperation
	{
		// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
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

		// Token: 0x06000003 RID: 3 RVA: 0x000020CC File Offset: 0x000002CC
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

		// Token: 0x06000004 RID: 4 RVA: 0x00002134 File Offset: 0x00000334
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

		// Token: 0x06000005 RID: 5 RVA: 0x00002177 File Offset: 0x00000377
		public override bool CanHandle(CocosItem cocosItem)
		{
			return cocosItem.IsSkeletonFile();
		}
	}
}
