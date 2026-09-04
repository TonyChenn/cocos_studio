using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gtk;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;
using MonoDevelop.Refactoring;
using Xwt;

namespace MonoDevelop.CodeIssues
{
	public class CodeIssuePadControl : Xwt.VBox
	{
		private const int UpdatePeriod = 500;

		private const int BatchChoiceCount = 5;

		private readonly Xwt.TreeView view = new Xwt.TreeView();

		private readonly DataField<string> textField = new DataField<string>();

		private readonly DataField<IIssueTreeNode> nodeField = new DataField<IIssueTreeNode>();

		private readonly Xwt.Button runButton = new Xwt.Button("Run");

		private readonly Xwt.Button cancelButton = new Xwt.Button("Cancel");

		private readonly IssueGroup rootGroup;

		private readonly Xwt.TreeStore store;

		private readonly ISet<IIssueTreeNode> syncedNodes = new HashSet<IIssueTreeNode>();

		private readonly Dictionary<IIssueTreeNode, TreePosition> nodePositions = new Dictionary<IIssueTreeNode, TreePosition>();

		private bool runPeriodicUpdate;

		private readonly object queueLock = new object();

		private readonly Queue<IIssueTreeNode> updateQueue = new Queue<IIssueTreeNode>();

		private IJobContext currentJobContext;

		private static readonly Type[] groupingProviders = new Type[5]
		{
			typeof(CategoryGroupingProvider),
			typeof(ProviderGroupingProvider),
			typeof(SeverityGroupingProvider),
			typeof(ProjectGroupingProvider),
			typeof(FileGroupingProvider)
		};

		private bool handledByPress;

		public IJobContext CurrentJobContext
		{
			get
			{
				return currentJobContext;
			}
			set
			{
				currentJobContext = value;
				bool flag = currentJobContext != null;
				runButton.Sensitive = !flag;
				cancelButton.Sensitive = flag;
			}
		}

		public CodeIssuePadControl()
		{
			Xwt.HBox hBox = new Xwt.HBox();
			runButton.Image = ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Execute, Gtk.IconSize.Menu);
			runButton.Clicked += StartAnalyzation;
			hBox.PackStart(runButton);
			cancelButton.Image = ImageService.GetIcon(MonoDevelop.Ide.Gui.Stock.Stop, Gtk.IconSize.Menu);
			cancelButton.Clicked += StopAnalyzation;
			cancelButton.Sensitive = false;
			hBox.PackStart(cancelButton);
			CategoryGroupingProvider nextProvider = new CategoryGroupingProvider
			{
				Next = new ProviderGroupingProvider()
			};
			rootGroup = new IssueGroup(nextProvider, "root group");
			GroupingProviderChainControl widget = new GroupingProviderChainControl(rootGroup, groupingProviders);
			hBox.PackStart(widget);
			PackStart(hBox);
			store = new Xwt.TreeStore(textField, nodeField);
			view.DataSource = store;
			view.HeadersVisible = false;
			view.Columns.Add("Name", textField);
			view.SelectionMode = Xwt.SelectionMode.Multiple;
			view.RowActivated += OnRowActivated;
			view.RowExpanding += OnRowExpanding;
			view.ButtonPressed += HandleButtonPressed;
			view.ButtonReleased += HandleButtonReleased;
			PackStart(view, expand: true);
			IIssueTreeNode issueTreeNode = rootGroup;
			EventHandler<IssueGroupEventArgs> value = delegate
			{
				Xwt.Application.Invoke(delegate
				{
					ClearSiblingNodes(store.GetFirstNode());
					store.Clear();
					foreach (IIssueTreeNode child in ((IIssueTreeNode)rootGroup).Children)
					{
						TreeNavigator navigator = store.AddNode();
						SetNode(navigator, child);
						SyncNode(navigator);
					}
				});
			};
			issueTreeNode.ChildrenInvalidated += value;
			issueTreeNode.ChildAdded += HandleRootChildAdded;
			IdeApp.Workspace.LastWorkspaceItemClosed += HandleLastWorkspaceItemClosed;
		}

		private void HandleLastWorkspaceItemClosed(object sender, EventArgs e)
		{
			ClearState();
		}

		private void ClearState()
		{
			store.Clear();
			rootGroup.ClearStatistics();
			rootGroup.EnableProcessing();
			syncedNodes.Clear();
			nodePositions.Clear();
			lock (queueLock)
			{
				updateQueue.Clear();
			}
		}

		private void StartPeriodicUpdate()
		{
			runPeriodicUpdate = true;
			Xwt.Application.TimeoutInvoke(500, RunPeriodicUpdate);
		}

		private void ProcessUpdateQueue()
		{
			IList<IIssueTreeNode> list;
			lock (queueLock)
			{
				list = new List<IIssueTreeNode>(updateQueue);
				updateQueue.Clear();
			}
			foreach (IIssueTreeNode item in list)
			{
				if (!nodePositions.TryGetValue(item, out var value))
				{
					continue;
				}
				TreeNavigator navigatorAt = store.GetNavigatorAt(value);
				if (!item.Visible)
				{
					nodePositions.Remove(item);
					if (syncedNodes.Contains(item))
					{
						syncedNodes.Remove(item);
					}
					ClearChildNodes(navigatorAt);
					navigatorAt.Remove();
					continue;
				}
				UpdateText(navigatorAt, item);
				if (!syncedNodes.Contains(item) && item.HasVisibleChildren)
				{
					if (navigatorAt.MoveToChild())
					{
						navigatorAt.MoveToParent();
					}
					else
					{
						AddDummyChild(navigatorAt);
					}
				}
			}
		}

		private bool RunPeriodicUpdate()
		{
			ProcessUpdateQueue();
			return runPeriodicUpdate;
		}

		private void EndPeriodicUpdate()
		{
			runPeriodicUpdate = false;
		}

		private void HandleRootChildAdded(object sender, IssueTreeNodeEventArgs e)
		{
			Xwt.Application.Invoke(delegate
			{
				TreeNavigator navigator = store.AddNode();
				SetNode(navigator, e.Child);
				SyncNode(navigator);
			});
		}

		private void StartAnalyzation(object sender, EventArgs e)
		{
			Solution currentSelectedSolution = IdeApp.ProjectOperations.CurrentSelectedSolution;
			if (currentSelectedSolution != null)
			{
				ClearState();
				SolutionAnalysisJob solutionAnalysisJob = new SolutionAnalysisJob(currentSelectedSolution);
				solutionAnalysisJob.CodeIssueAdded += HandleCodeIssueAdded;
				solutionAnalysisJob.Completed += delegate
				{
					CurrentJobContext = null;
				};
				CurrentJobContext = RefactoringService.QueueCodeIssueAnalysis(solutionAnalysisJob, "Analyzing solution");
				StartPeriodicUpdate();
			}
		}

		private void HandleCodeIssueAdded(object sender, CodeIssueEventArgs e)
		{
			foreach (CodeIssue codeIssue in e.CodeIssues)
			{
				IssueSummary issue = IssueSummary.FromCodeIssue(e.File, e.Provider, codeIssue);
				rootGroup.AddIssue(issue);
			}
		}

		private void StopAnalyzation(object sender, EventArgs e)
		{
			if (CurrentJobContext != null)
			{
				CurrentJobContext.CancelJob();
				CurrentJobContext = null;
			}
			EndPeriodicUpdate();
		}

		private void SetNode(TreeNavigator navigator, IIssueTreeNode node)
		{
			if (navigator == null)
			{
				throw new ArgumentNullException("navigator");
			}
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			navigator.SetValue(nodeField, node);
			TreePosition position = navigator.CurrentPosition;
			nodePositions.Add(node, position);
			node.ChildAdded += delegate(object sender, IssueTreeNodeEventArgs e)
			{
				Xwt.Application.Invoke(delegate
				{
					TreeNavigator navigatorAt = store.GetNavigatorAt(position);
					navigatorAt.AddChild();
					SetNode(navigatorAt, e.Child);
					SyncNode(navigatorAt);
				});
			};
			node.ChildrenInvalidated += delegate
			{
				Xwt.Application.Invoke(delegate
				{
					SyncNode(store.GetNavigatorAt(position));
				});
			};
			node.TextChanged += delegate(object sender, IssueGroupEventArgs e)
			{
				lock (queueLock)
				{
					if (!updateQueue.Contains(e.Node))
					{
						updateQueue.Enqueue(e.Node);
					}
				}
			};
			node.VisibleChanged += delegate(object sender, IssueGroupEventArgs e)
			{
				lock (queueLock)
				{
					if (!updateQueue.Contains(e.Node))
					{
						updateQueue.Enqueue(e.Node);
					}
				}
			};
		}

		private void ClearSiblingNodes(TreeNavigator navigator)
		{
			if (navigator.CurrentPosition == null)
			{
				return;
			}
			do
			{
				IIssueTreeNode value = navigator.GetValue(nodeField);
				if (value != null)
				{
					if (syncedNodes.Contains(value))
					{
						syncedNodes.Remove(value);
					}
					if (nodePositions.ContainsKey(value))
					{
						nodePositions.Remove(value);
					}
				}
				ClearChildNodes(navigator);
			}
			while (navigator.MoveNext());
		}

		private void ClearChildNodes(TreeNavigator navigator)
		{
			if (navigator.MoveToChild())
			{
				ClearSiblingNodes(navigator);
				navigator.MoveToParent();
			}
		}

		private void SyncNode(TreeNavigator navigator, bool forceExpansion = false)
		{
			IIssueTreeNode value = navigator.GetValue(nodeField);
			UpdateText(navigator, value);
			bool flag = forceExpansion || view.IsRowExpanded(navigator.CurrentPosition);
			ClearChildNodes(navigator);
			syncedNodes.Remove(value);
			navigator.RemoveChildren();
			if (!value.HasVisibleChildren)
			{
				return;
			}
			if (flag)
			{
				foreach (IIssueTreeNode item in value.Children.Where((IIssueTreeNode child) => child.Visible))
				{
					navigator.AddChild();
					SetNode(navigator, item);
					SyncNode(navigator);
					navigator.MoveToParent();
				}
			}
			else
			{
				AddDummyChild(navigator);
			}
			if (flag)
			{
				syncedNodes.Add(value);
				view.ExpandRow(navigator.CurrentPosition, expandChildren: false);
			}
		}

		private void UpdateText(TreeNavigator navigator, IIssueTreeNode node)
		{
			navigator.SetValue(textField, node.Text);
		}

		private void AddDummyChild(TreeNavigator navigator)
		{
			navigator.AddChild();
			navigator.SetValue(textField, "Loading...");
			navigator.MoveToParent();
		}

		private EventHandler<IssueGroupEventArgs> GetChildrenInvalidatedHandler(TreePosition position)
		{
			return delegate
			{
				Xwt.Application.Invoke(delegate
				{
					bool flag = view.IsRowExpanded(position);
					TreeNavigator navigatorAt = store.GetNavigatorAt(position);
					navigatorAt.RemoveChildren();
					SyncNode(navigatorAt, flag);
					if (flag)
					{
						view.ExpandRow(position, expandChildren: false);
					}
				});
			};
		}

		private void OnRowActivated(object sender, TreeViewRowEventArgs e)
		{
			TreePosition position = e.Position;
			IIssueTreeNode value = store.GetNavigatorAt(position).GetValue(nodeField);
			if (value is IssueSummary issueSummary)
			{
				DomRegion region = issueSummary.Region;
				IdeApp.Workbench.OpenDocument(region.FileName, region.BeginLine, region.BeginColumn);
			}
			else if (!view.IsRowExpanded(position))
			{
				view.ExpandRow(position, expandChildren: false);
			}
			else
			{
				view.CollapseRow(position);
			}
		}

		private void OnRowExpanding(object sender, TreeViewRowEventArgs e)
		{
			TreeNavigator navigatorAt = store.GetNavigatorAt(e.Position);
			IIssueTreeNode value = navigatorAt.GetValue(nodeField);
			if (!syncedNodes.Contains(value))
			{
				SyncNode(navigatorAt, forceExpansion: true);
			}
		}

		private void HandleButtonReleased(object sender, ButtonEventArgs e)
		{
			if (e.Button == PointerButton.Right && !handledByPress)
			{
				TreePosition[] selectedRows = view.SelectedRows;
				if (selectedRows.Length <= 1)
				{
					ShowBatchFixContextMenu(e.X, e.Y, view.SelectedRows);
				}
			}
		}

		private void HandleButtonPressed(object sender, ButtonEventArgs e)
		{
			if (e.Button == PointerButton.Right)
			{
				TreePosition[] selectedRows = view.SelectedRows;
				if (selectedRows.Length > 1)
				{
					ShowBatchFixContextMenu(e.X, e.Y, selectedRows);
					e.Handled = true;
					handledByPress = true;
				}
				else
				{
					handledByPress = false;
				}
			}
		}

		private void UpdateParents(TreeNavigator navigator)
		{
			do
			{
				IIssueTreeNode value = navigator.GetValue(nodeField);
				UpdateText(navigator, value);
			}
			while (navigator.MoveToParent());
		}

		private void ShowBatchFixContextMenu(double x, double y, IEnumerable<TreePosition> rows)
		{
			IOrderedEnumerable<IGrouping<string, IssueSummary>> source = from issue in (from issue in (from node3 in (from row in rows
							select store.GetNavigatorAt(row).GetValue(nodeField) into node1
							where node1 != null
							select node1).SelectMany((IIssueTreeNode node2) => node2.AllChildren.Union(new IIssueTreeNode[1] { node2 }))
						where node3.Visible
						select node3).OfType<IssueSummary>()
					where issue.Actions.Any((ActionSummary a) => a.Batchable)
					select issue).Distinct()
				group issue by issue.InspectorIdString into @group
				orderby -@group.Count()
				select @group;
			List<IGrouping<string, IssueSummary>> list = source.Take(5).ToList();
			if (!list.Any())
			{
				return;
			}
			if (list.Count == 1)
			{
				CreateIssueMenu(list.First()).Popup(view, x, y);
				return;
			}
			Xwt.Menu menu = new Xwt.Menu();
			foreach (IGrouping<string, IssueSummary> item in list)
			{
				Xwt.MenuItem menuItem = new Xwt.MenuItem(item.First().ProviderTitle);
				menuItem.SubMenu = CreateIssueMenu(item);
				menu.Items.Add(menuItem);
			}
			menu.Popup(view, x, y);
		}

		private Xwt.Menu CreateIssueMenu(IEnumerable<IssueSummary> issues)
		{
			IList<IssueSummary> source = (issues as IList<IssueSummary>) ?? issues.ToList();
			Xwt.Menu menu = new Xwt.Menu();
			IEnumerable<IGrouping<object, ActionSummary>> enumerable = from action in source.SelectMany((IssueSummary issue) => issue.Actions)
				group action by action.SiblingKey;
			foreach (IGrouping<object, ActionSummary> item in enumerable)
			{
				IGrouping<object, ActionSummary> actionGroup = item;
				Xwt.MenuItem menuItem = new Xwt.MenuItem(actionGroup.First().Title);
				menuItem.Clicked += delegate
				{
					ThreadPool.QueueUserWorkItem(delegate
					{
						try
						{
							using (IProgressMonitor monitor = IdeApp.Workbench.ProgressMonitors.GetStatusProgressMonitor("Applying fixes", null, showErrorDialogs: false))
							{
								BatchFixer batchFixer = new BatchFixer(new ExactIssueMatcher(), monitor);
								IEnumerable<ActionSummary> enumerable2 = batchFixer.TryFixIssues(actionGroup);
								foreach (ActionSummary item2 in enumerable2)
								{
									((IIssueTreeNode)item2.IssueSummary).Visible = false;
								}
							}
							Xwt.Application.Invoke(delegate
							{
								ProcessUpdateQueue();
							});
						}
						catch (Exception ex)
						{
							LoggingService.LogInternalError(ex);
						}
					});
				};
				menu.Items.Add(menuItem);
			}
			return menu;
		}
	}
}
