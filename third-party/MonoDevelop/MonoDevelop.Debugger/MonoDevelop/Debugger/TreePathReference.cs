using System;
using Gtk;

namespace MonoDevelop.Debugger
{
	public sealed class TreePathReference : IDisposable
	{
		private int[] indices;

		private TreePath path;

		public TreeModel Model { get; private set; }

		public TreePath Path
		{
			get
			{
				if (path == null && indices != null)
				{
					path = new TreePath(indices);
				}
				return path;
			}
		}

		public bool IsValid
		{
			get
			{
				if (Model != null)
				{
					return indices != null;
				}
				return false;
			}
		}

		public TreePathReference(TreeModel model, TreePath path)
		{
			model.RowsReordered += HandleRowsReordered;
			model.RowInserted += HandleRowInserted;
			model.RowDeleted += HandleRowDeleted;
			indices = path.Indices;
			this.path = path;
			Model = model;
		}

		private void HandleRowsReordered(object o, RowsReorderedArgs args)
		{
			int num = Model.IterNChildren(args.Iter);
			int depth = args.Path.Depth;
			if (num < 2 || !args.Path.IsAncestor(Path) || indices.Length <= depth)
			{
				return;
			}
			for (int i = 0; i < num; i++)
			{
				if (args.NewChildOrder[i] == indices[depth])
				{
					indices[depth] = i;
					break;
				}
			}
		}

		private void HandleRowInserted(object o, RowInsertedArgs args)
		{
			int[] array = args.Path.Indices;
			int i;
			for (i = 0; i < array.Length - 1 && i < indices.Length - 1; i++)
			{
				if (array[i] > indices[i])
				{
					return;
				}
				if (array[i] < indices[i])
				{
					break;
				}
			}
			if (array[i] <= indices[i])
			{
				indices[i]++;
				path = null;
			}
		}

		private void HandleRowDeleted(object o, RowDeletedArgs args)
		{
			int[] array = args.Path.Indices;
			for (int i = 0; i < array.Length && i < indices.Length; i++)
			{
				if (array[i] > indices[i])
				{
					return;
				}
				if (array[i] < indices[i])
				{
					indices[i]--;
					path = null;
					return;
				}
			}
			if (array.Length <= indices.Length)
			{
				Invalidate();
			}
		}

		private void Invalidate()
		{
			if (Model != null)
			{
				Model.RowsReordered -= HandleRowsReordered;
				Model.RowInserted -= HandleRowInserted;
				Model.RowDeleted -= HandleRowDeleted;
				Model = null;
			}
			indices = null;
			path = null;
		}

		public void Dispose()
		{
			Invalidate();
		}
	}
}
