using System;
using System.Collections.Generic;
using System.Linq;
using MonoDevelop.Core;
using Xwt;

namespace MonoDevelop.CodeIssues
{
	public class GroupingProviderChainControl : HBox
	{
		private class GroupingProvider : IGroupingProvider
		{
			private readonly IssueGroup rootGroup;

			private IGroupingProvider next;

			public IGroupingProvider Next
			{
				get
				{
					return next;
				}
				set
				{
					next = value;
					rootGroup.GroupingProvider = value;
				}
			}

			public bool SupportsNext => true;

			public event EventHandler<GroupingProviderEventArgs> NextChanged;

			public GroupingProvider(IssueGroup rootGroup)
			{
				this.rootGroup = rootGroup;
				next = rootGroup.GroupingProvider;
			}

			public IssueGroup GetIssueGroup(IssueGroup parent, IssueSummary issue)
			{
				throw new NotImplementedException();
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}

		private readonly IList<Type> availableProviders;

		private readonly IList<ComboBox> providerPickers = new List<ComboBox>();

		private readonly IList<Label> labels = new List<Label>();

		public IGroupingProvider RootGroupingProvider { get; private set; }

		public GroupingProviderChainControl(IssueGroup rootGroup, IEnumerable<Type> providers)
		{
			RootGroupingProvider = new GroupingProvider(rootGroup);
			availableProviders = providers.ToList();
			BuildUi();
		}

		private void BuildUi()
		{
			Clear();
			Label label = new Label("Group by:");
			labels.Add(label);
			PackStart(label);
			BuildProviderSelectors(RootGroupingProvider, RootGroupingProvider.Next);
		}

		private void BuildProviderSelectors(IGroupingProvider previousProvider, IGroupingProvider selectedProvider)
		{
			ComboBox combo = MakeSelector(selectedProvider);
			combo.SelectionChanged += delegate
			{
				Type type = combo.SelectedItem as Type;
				if (!(type == null))
				{
					IGroupingProvider groupingProvider = (IGroupingProvider)Activator.CreateInstance(type);
					if (groupingProvider.SupportsNext && selectedProvider.SupportsNext)
					{
						groupingProvider.Next = selectedProvider.Next;
					}
					previousProvider.Next = groupingProvider;
					BuildUi();
				}
			};
			providerPickers.Add(combo);
			PackStart(combo);
			if (selectedProvider.SupportsNext)
			{
				PackStart(new Label("then by"));
				BuildProviderSelectors(selectedProvider, selectedProvider.Next);
			}
		}

		private ComboBox MakeSelector(IGroupingProvider selectedProvider)
		{
			ComboBox comboBox = new ComboBox();
			comboBox.Items.Add(typeof(NullGroupingProvider), "Nothing");
			comboBox.Items.Add(ItemSeparator.Instance);
			foreach (Type availableProvider in availableProviders)
			{
				GroupingDescriptionAttribute groupingDescriptionAttribute = (GroupingDescriptionAttribute)availableProvider.GetCustomAttributes(inherit: false).FirstOrDefault((object attr) => attr is GroupingDescriptionAttribute);
				if (groupingDescriptionAttribute == null)
				{
					LoggingService.LogWarning("Grouping provider '{0}' does not have a metadata attribute, ignoring provider.", availableProvider.FullName);
				}
				else
				{
					comboBox.Items.Add(availableProvider, groupingDescriptionAttribute.Title);
				}
			}
			if (selectedProvider != null)
			{
				comboBox.SelectedItem = selectedProvider.GetType();
			}
			else
			{
				comboBox.SelectedItem = typeof(NullGroupingProvider);
			}
			return comboBox;
		}
	}
}
