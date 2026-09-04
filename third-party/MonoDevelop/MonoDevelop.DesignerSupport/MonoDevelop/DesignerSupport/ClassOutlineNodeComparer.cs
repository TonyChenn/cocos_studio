using System.Collections.Generic;
using System.Globalization;
using Gtk;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Ide.TypeSystem;

namespace MonoDevelop.DesignerSupport
{
	internal class ClassOutlineNodeComparer : IComparer<TreeIter>
	{
		private const string DEFAULT_REGION_NAME = "region";

		private const int GROUP_INDEX_REGIONS = 0;

		private const int GROUP_INDEX_NAMESPACES = 1;

		private const int GROUP_INDEX_TYPES = 2;

		private const int GROUP_INDEX_FIELDS = 3;

		private const int GROUP_INDEX_PROPERTIES = 4;

		private const int GROUP_INDEX_EVENTS = 5;

		private const int GROUP_INDEX_METHODS = 6;

		private Ambience ambience;

		private TreeModel model;

		private ClassOutlineSettings settings;

		private int[] groupTable;

		public ClassOutlineNodeComparer(Ambience ambience, ClassOutlineSettings settings, TreeModel model)
		{
			this.ambience = ambience;
			this.settings = settings;
			this.model = model;
			BuildGroupTable();
		}

		public int Compare(TreeIter a, TreeIter b)
		{
			return CompareNodes(model, a, b);
		}

		public int CompareNodes(TreeModel model, TreeIter node1, TreeIter node2)
		{
			object value = model.GetValue(node1, 0);
			object value2 = model.GetValue(node2, 0);
			if (value == null)
			{
				if (value2 != null)
				{
					return 1;
				}
				return 0;
			}
			if (value2 == null)
			{
				return -1;
			}
			if (settings.IsGrouped)
			{
				int num = GetGroupPriority(value) - GetGroupPriority(value2);
				if (num != 0)
				{
					return num;
				}
				if (value is IMethod m)
				{
					return CompareMethods(m, (IMethod)value2, settings.IsSorted);
				}
			}
			if (settings.IsSorted)
			{
				return CompareName(value, value2);
			}
			return CompareRegion(value, value2);
		}

		private int CompareName(object o1, object o2)
		{
			int num = string.Compare(GetSortName(o1), GetSortName(o2), CultureInfo.CurrentCulture, CompareOptions.IgnoreSymbols);
			if (num == 0)
			{
				return CompareRegion(o1, o2);
			}
			return num;
		}

		private int CompareMethods(IMethod m1, IMethod m2, bool isSortingAlphabetically)
		{
			bool isConstructor = m1.IsConstructor;
			bool isConstructor2 = m2.IsConstructor;
			if (isConstructor)
			{
				if (isConstructor2)
				{
					return CompareRegion(m1, m2);
				}
				return -1;
			}
			if (isConstructor2)
			{
				return 1;
			}
			bool flag = IsFinalizer(m1);
			bool flag2 = IsFinalizer(m2);
			if (flag)
			{
				if (flag2)
				{
					return CompareRegion(m1, m2);
				}
				return -1;
			}
			if (flag2)
			{
				return 1;
			}
			if (isSortingAlphabetically)
			{
				return CompareName(m1, m2);
			}
			return CompareRegion(m1, m2);
		}

		private bool IsConstructor(object node)
		{
			if (node is IMethod)
			{
				return ((IMethod)node).IsConstructor;
			}
			return false;
		}

		private bool IsFinalizer(object node)
		{
			if (node is IMethod)
			{
				return ((IMethod)node).IsDestructor;
			}
			return false;
		}

		private void BuildGroupTable()
		{
			groupTable = new int[7];
			int num = -10;
			foreach (string item in settings.GroupOrder)
			{
				switch (item)
				{
				case "Regions":
					groupTable[0] = num++;
					break;
				case "Namespaces":
					groupTable[1] = num++;
					break;
				case "Types":
					groupTable[2] = num++;
					break;
				case "Fields":
					groupTable[3] = num++;
					break;
				case "Properties":
					groupTable[4] = num++;
					break;
				case "Events":
					groupTable[5] = num++;
					break;
				case "Methods":
					groupTable[6] = num++;
					break;
				}
			}
		}

		private int GetGroupPriority(object node)
		{
			if (node is FoldingRegion)
			{
				return groupTable[0];
			}
			if (node is string)
			{
				return groupTable[1];
			}
			if (node is IType)
			{
				return groupTable[2];
			}
			if (node is IField)
			{
				return groupTable[3];
			}
			if (node is IProperty)
			{
				return groupTable[4];
			}
			if (node is IEvent)
			{
				return groupTable[5];
			}
			if (node is IMethod)
			{
				return groupTable[6];
			}
			return 0;
		}

		private string GetSortName(object node)
		{
			if (node is IEntity)
			{
				return ambience.GetString((IEntity)node, OutputFlags.None);
			}
			if (node is FoldingRegion)
			{
				string text = ((FoldingRegion)node).Name.Trim();
				if (text.Length == 0)
				{
					text = "region";
				}
				return text;
			}
			return string.Empty;
		}

		internal static DomRegion GetRegion(object o)
		{
			if (o is IEntity entity)
			{
				if (!entity.BodyRegion.IsEmpty)
				{
					return entity.BodyRegion;
				}
				return entity.Region;
			}
			if (o is IUnresolvedEntity unresolvedEntity)
			{
				if (!unresolvedEntity.BodyRegion.IsEmpty)
				{
					return unresolvedEntity.BodyRegion;
				}
				return unresolvedEntity.Region;
			}
			if (o is FoldingRegion)
			{
				return ((FoldingRegion)o).Region;
			}
			return DomRegion.Empty;
		}

		internal static int CompareRegion(object o1, object o2)
		{
			return GetRegion(o1).Begin.CompareTo(GetRegion(o2).Begin);
		}
	}
}
