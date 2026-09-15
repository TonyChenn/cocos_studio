using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Modules.Communal.ResourcePanel
{
	internal class NodeHashtable : Dictionary<object, object>
	{
		public NodeHashtable() : base(new NodeHashtable.NodeComparer())
		{
			this.nodeComparer = (NodeHashtable.NodeComparer)base.Comparer;
		}

		public void RegisterByRefType(Type type)
		{
			this.nodeComparer.byRefTypes.Add(type);
		}

		private NodeHashtable.NodeComparer nodeComparer;

		private class NodeComparer : IEqualityComparer<object>
		{
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				if (this.CompareByRef(x.GetType()))
				{
					return x == y;
				}
				return x.Equals(y);
			}

			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				if (this.CompareByRef(obj.GetType()))
				{
					return RuntimeHelpers.GetHashCode(obj);
				}
				return obj.GetHashCode();
			}

			private bool CompareByRef(Type type)
			{
				if (this.byRefTypes.Count == 0)
				{
					return false;
				}
				bool flag;
				if (!this.typeData.TryGetValue(type, out flag))
				{
					flag = false;
					Type type2 = type;
					while (type2 != null)
					{
						if (this.byRefTypes.Contains(type2))
						{
							flag = true;
							break;
						}
						type2 = type2.BaseType;
					}
					this.typeData[type] = flag;
				}
				return flag;
			}

			public HashSet<Type> byRefTypes = new HashSet<Type>();

			public Dictionary<Type, bool> typeData = new Dictionary<Type, bool>();
		}
	}
}
