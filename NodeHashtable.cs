using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Modules.Communal.ResourcePanel
{
	// Token: 0x02000027 RID: 39
	internal class NodeHashtable : Dictionary<object, object>
	{
		// Token: 0x06000108 RID: 264 RVA: 0x00004F40 File Offset: 0x00003140
		public NodeHashtable() : base(new NodeHashtable.NodeComparer())
		{
			this.nodeComparer = (NodeHashtable.NodeComparer)base.Comparer;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00004F5E File Offset: 0x0000315E
		public void RegisterByRefType(Type type)
		{
			this.nodeComparer.byRefTypes.Add(type);
		}

		// Token: 0x04000050 RID: 80
		private NodeHashtable.NodeComparer nodeComparer;

		// Token: 0x02000028 RID: 40
		private class NodeComparer : IEqualityComparer<object>
		{
			// Token: 0x0600010A RID: 266 RVA: 0x00004F72 File Offset: 0x00003172
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				if (this.CompareByRef(x.GetType()))
				{
					return x == y;
				}
				return x.Equals(y);
			}

			// Token: 0x0600010B RID: 267 RVA: 0x00004F8E File Offset: 0x0000318E
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				if (this.CompareByRef(obj.GetType()))
				{
					return RuntimeHelpers.GetHashCode(obj);
				}
				return obj.GetHashCode();
			}

			// Token: 0x0600010C RID: 268 RVA: 0x00004FAC File Offset: 0x000031AC
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

			// Token: 0x04000051 RID: 81
			public HashSet<Type> byRefTypes = new HashSet<Type>();

			// Token: 0x04000052 RID: 82
			public Dictionary<Type, bool> typeData = new Dictionary<Type, bool>();
		}
	}
}
