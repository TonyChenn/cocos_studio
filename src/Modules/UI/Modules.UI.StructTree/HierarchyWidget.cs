using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CocoStudio.Core;
using CocoStudio.Core.Commands;
using CocoStudio.Core.View;
using CocoStudio.Model.Event;
using CocoStudio.Model.Interface;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Animation;
using Modules.Animation.Model;
using Modules.Animation.StructTree;
using Modules.Communal.Render.Model;
using MonoDevelop.Components.Commands;
using MonoDevelop.Ide.Codons;
using MonoDevelop.Ide.Gui;

namespace Modules.UI.StructTree
{
	public class HierarchyWidget : EventBox, ICommandRouter
	{
		private const string AnimationPadId = "Modules.Animation.AnimationPad";

		public HierarchyWidget()
		{
			base.Name = "Modules.UI.StructTree.HierarchyWidget";
			base.WidthRequest = 250;

			this.treeWidget = new TreeWidget();
			this.DisableDuplicateSerialFrameHandler();

			this.scrolledWindow = new ScrolledWindow();
			this.scrolledWindow.HscrollbarPolicy = PolicyType.Automatic;
			this.scrolledWindow.VscrollbarPolicy = PolicyType.Automatic;
			this.scrolledWindow.Add(this.treeWidget);
			base.Add(this.scrolledWindow);

			this.treeWidget.TreeWidetSelectionChangedEvent += this.OnTreeWidgetSelectionChanged;
			this.treeWidget.CopyNodeEvent += this.OnCopyNode;
			this.treeWidget.PasteNodeEvent += this.OnPasteNode;
			this.selectedVisualObjectsChangeEvent = Services.EventsService.GetEvent<SelectedVisualObjectsChangeEvent>();
			this.selectedVisualObjectsChangeEvent.Subscribe(this.OnSelectedVisualObjectsChanged);

			base.ShowAll();
			GLib.Idle.Add(this.InitializeAnimationModel);
		}

		public void DetachEvents()
		{
			if (this.eventsDetached)
			{
				return;
			}
			this.eventsDetached = true;
			this.treeWidget.TreeWidetSelectionChangedEvent -= this.OnTreeWidgetSelectionChanged;
			this.treeWidget.CopyNodeEvent -= this.OnCopyNode;
			this.treeWidget.PasteNodeEvent -= this.OnPasteNode;
			this.selectedVisualObjectsChangeEvent.Unsubscribe(this.OnSelectedVisualObjectsChanged);
		}

		private bool InitializeAnimationModel()
		{
			if (this.eventsDetached)
			{
				return false;
			}
			CocoStudio.Core.View.Pad animationPad = Services.Workbench.Pads.FirstOrDefault((CocoStudio.Core.View.Pad pad) => pad.Id == HierarchyWidget.AnimationPadId);
			if (animationPad == null)
			{
				return false;
			}

			Widget animationWidget = animationPad.CurrentWidget();
			if (animationWidget == null)
			{
				PadCodon animationPadCodon = animationPad.Content as PadCodon;
				if (animationPadCodon != null)
				{
					IPadContent animationPadContent = animationPadCodon.InitializePadContent(Services.MainWindow.GetPadWindow(animationPadCodon));
					animationWidget = animationPadContent.Control;
				}
			}

			AnimationUC animationControl = HierarchyWidget.FindChild<AnimationUC>(animationWidget);
			if (Services.Workbench.ActiveDocument != null && StructTreeModel.Instance.IterNChildren() == 0 && animationControl != null)
			{
				MethodInfo documentChanged = typeof(AnimationUC).GetMethod("DocumentChanged", BindingFlags.Instance | BindingFlags.NonPublic);
				if (documentChanged != null)
				{
					documentChanged.Invoke(animationControl, new object[] { this, EventArgs.Empty });
				}
			}

			this.SelectObjects(SelectService.Instance.SelectedObjectList);
			return false;
		}

		private static T FindChild<T>(Widget widget) where T : Widget
		{
			if (widget == null)
			{
				return null;
			}
			T result = widget as T;
			if (result != null)
			{
				return result;
			}
			Container container = widget as Container;
			if (container != null)
			{
				foreach (Widget child in container.Children)
				{
					result = HierarchyWidget.FindChild<T>(child);
					if (result != null)
					{
						return result;
					}
				}
			}
			return null;
		}

		private void DisableDuplicateSerialFrameHandler()
		{
			MethodInfo method = typeof(TreeWidget).GetMethod("CreateSerialFrameEventHandled", BindingFlags.Instance | BindingFlags.NonPublic);
			if (method == null)
			{
				throw new MissingMethodException(typeof(TreeWidget).FullName, "CreateSerialFrameEventHandled");
			}
			Action<CreateSerialFrameEventArgs> handler = (Action<CreateSerialFrameEventArgs>)Delegate.CreateDelegate(typeof(Action<CreateSerialFrameEventArgs>), this.treeWidget, method);
			Services.EventsService.GetEvent<CreateSerialFrameEvent>().Unsubscribe(handler);
		}

		private void OnTreeWidgetSelectionChanged(List<ITimeline> selectedItems)
		{
			HashSet<VisualObject> selectedObjects = new HashSet<VisualObject>();
			foreach (ITimeline item in selectedItems)
			{
				VisualObject visualObject = item as VisualObject;
				if (visualObject != null)
				{
					selectedObjects.Add(visualObject);
				}
				else
				{
					Timeline timeline = item as Timeline;
					if (timeline != null)
					{
						selectedObjects.Add(timeline.Node);
					}
				}
			}

			this.selectedVisualObjectsChangeEvent.Unsubscribe(this.OnSelectedVisualObjectsChanged);
			try
			{
				this.selectedVisualObjectsChangeEvent.Publish(new SelectedVisualObjectsChangeEventArgs(selectedObjects, this.treeWidget.SelectedParentObjects));
			}
			finally
			{
				this.selectedVisualObjectsChangeEvent.Subscribe(this.OnSelectedVisualObjectsChanged);
			}
		}

		private void OnSelectedVisualObjectsChanged(SelectedVisualObjectsChangeEventArgs args)
		{
			this.SelectObjects(args.SelectedObject);
		}

		private void SelectObjects(IEnumerable<VisualObject> objects)
		{
			this.treeWidget.SetIgorePublishSelectEvent(true);
			this.treeWidget.EnableTreeSelectionFunc = false;
			this.treeWidget.SelfSelectionChanged = false;
			this.treeWidget.Selection.UnselectAll();

			HashSet<VisualObject> selectedObjects = new HashSet<VisualObject>(objects ?? Enumerable.Empty<VisualObject>());
			foreach (VisualObject selectedObject in selectedObjects)
			{
				TreeIter iter = StructTreeModel.Instance.GetItemIter(selectedObject);
				if (!iter.Equals(TreeIter.Zero))
				{
					TreePath path = StructTreeModel.Instance.GetItemFilteredPath(selectedObject);
					if (path != null)
					{
						this.treeWidget.ExpandToPath(path);
					}
				}
			}

			TreePath firstSelectedPath = this.treeWidget.SelectNodes(selectedObjects);
			this.treeWidget.EnableTreeSelectionFunc = true;
			this.treeWidget.SetIgorePublishSelectEvent(false);
			this.treeWidget.SelectedObject = selectedObjects.Count == 1 ? selectedObjects.FirstOrDefault() as AbstractNodeObject : null;
			this.treeWidget.QueueDraw();

			if (firstSelectedPath != null)
			{
				GLib.Timeout.Add(250U, delegate
				{
					if (!this.eventsDetached)
					{
						this.treeWidget.ScrollToPath(firstSelectedPath);
					}
					return false;
				});
			}
		}

		private void OnCopyNode()
		{
			List<VisualObject> selectedObjects = this.treeWidget.SelectedItems.OfType<AbstractNodeObject>().Cast<VisualObject>().ToList<VisualObject>();
			Services.EventsService.GetEvent<CopyVisualObjectsEvent>().Publish(selectedObjects.AsReadOnly());
		}

		private void OnPasteNode()
		{
			Services.EventsService.GetEvent<PasteVisualObjectsEvent>().Publish(null);
		}

		private List<VisualObject> GetSelectedObjects()
		{
			return this.treeWidget.SelectedParentObjects.ToList<VisualObject>();
		}

		[CommandHandler(CmdEnum.DeleteCmd)]
		[CommandHandler(CmdEnum.DeleteCmd2)]
		private void DeleteSelectedObjects()
		{
			Services.EventsService.GetEvent<DeleteVisualObjectsEvent>().Publish(this.GetSelectedObjects().AsReadOnly());
		}

		[CommandUpdateHandler(CmdEnum.DeleteCmd)]
		[CommandUpdateHandler(CmdEnum.DeleteCmd2)]
		private void UpdateDeleteCommand(CommandInfo info)
		{
			info.Enabled = !this.treeWidget.IsRanameStatus;
			info.Bypass = this.treeWidget.IsRanameStatus;
		}

		[CommandHandler(CmdEnum.CopyCmd)]
		private void CopySelectedObjects()
		{
			Services.EventsService.GetEvent<CopyVisualObjectsEvent>().Publish(this.GetSelectedObjects().AsReadOnly());
		}

		[CommandHandler(CmdEnum.CutCmd)]
		private void CutSelectedObjects()
		{
			Services.EventsService.GetEvent<CutVisualObjectsEvent>().Publish(this.GetSelectedObjects().AsReadOnly());
		}

		[CommandHandler(CmdEnum.PasteCmd)]
		private void PasteObjects()
		{
			this.OnPasteNode();
		}

		[CommandUpdateHandler(CmdEnum.CopyCmd)]
		[CommandUpdateHandler(CmdEnum.CutCmd)]
		[CommandUpdateHandler(CmdEnum.PasteCmd)]
		private void UpdateClipboardCommand(CommandInfo info)
		{
			info.Enabled = !this.treeWidget.IsRanameStatus;
			info.Bypass = this.treeWidget.IsRanameStatus;
		}

		object ICommandRouter.GetNextCommandTarget()
		{
			return this.treeWidget.ContextMenu;
		}

		private readonly TreeWidget treeWidget;

		private readonly ScrolledWindow scrolledWindow;

		private readonly SelectedVisualObjectsChangeEvent selectedVisualObjectsChangeEvent;

		private bool eventsDetached;
	}
}
