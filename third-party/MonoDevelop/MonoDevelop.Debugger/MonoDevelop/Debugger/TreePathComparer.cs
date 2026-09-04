using System;
using System.Collections.Generic;
using Gtk;

namespace MonoDevelop.Debugger
{
	public class TreePathComparer : IComparer<TreePath>
	{
		private bool reversed;

		public TreePathComparer(bool reversed)
		{
			this.reversed = reversed;
		}

		private static int TreePathCompare(TreePath x, TreePath y)
		{
			int num = Math.Min(x.Depth, y.Depth);
			for (int i = 0; i < num; i++)
			{
				if (x.Indices[i] < y.Indices[i])
				{
					return -1;
				}
				if (x.Indices[i] > y.Indices[i])
				{
					return 1;
				}
			}
			if (x.Depth < y.Depth)
			{
				return -1;
			}
			if (x.Depth > y.Depth)
			{
				return 1;
			}
			return 0;
		}

		public int Compare(TreePath x, TreePath y)
		{
			if (!reversed)
			{
				return TreePathCompare(x, y);
			}
			return TreePathCompare(y, x);
		}
	}
}
