using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonoDevelop.DesignerSupport.Toolbox
{
	internal class Category : Item
	{
		private bool isExpanded;

		public uint AnimationHandle;

		public int AnimationHeight;

		private List<Item> items = new List<Item>();

		private bool canIconizeItems = true;

		private bool isDropTarget;

		private bool isSorted = true;

		public bool IsExpanded
		{
			get
			{
				return isExpanded;
			}
			set
			{
				isExpanded = value;
			}
		}

		public bool AnimatingExpand { get; set; }

		public int ItemCount => items.Count;

		public ReadOnlyCollection<Item> Items => items.AsReadOnly();

		public bool CanIconizeItems
		{
			get
			{
				return canIconizeItems;
			}
			set
			{
				canIconizeItems = value;
			}
		}

		public bool IsDropTarget
		{
			get
			{
				return isDropTarget;
			}
			set
			{
				isDropTarget = value;
			}
		}

		public bool IsSorted
		{
			get
			{
				return isSorted;
			}
			set
			{
				isSorted = value;
			}
		}

		public int Priority { get; set; }

		public Category(string text)
			: base(text)
		{
		}

		public void Clear()
		{
			items.Clear();
		}

		public void Add(Item item)
		{
			items.Add(item);
			if (isSorted)
			{
				items.Sort();
			}
		}

		public void Remove(Item item)
		{
			items.Remove(item);
			if (isSorted)
			{
				items.Sort();
			}
		}

		public override string ToString()
		{
			return $"[Category: Text={base.Text}]";
		}
	}
}
