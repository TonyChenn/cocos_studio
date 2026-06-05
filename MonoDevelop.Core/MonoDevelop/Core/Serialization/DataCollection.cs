using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public class DataCollection : IEnumerable
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000D02B File Offset: 0x0000B22B
		protected List<DataNode> List
		{
			get
			{
				if (this.list == null)
				{
					this.list = new List<DataNode>();
				}
				return this.list;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600034F RID: 847 RVA: 0x0000D046 File Offset: 0x0000B246
		public int Count
		{
			get
			{
				if (this.list != null)
				{
					return this.list.Count;
				}
				return 0;
			}
		}

		// Token: 0x170000A3 RID: 163
		public virtual DataNode this[int n]
		{
			get
			{
				return this.List[n];
			}
			set
			{
				this.List[n] = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		public virtual DataNode this[string name]
		{
			get
			{
				DataCollection dataCollection;
				int num = this.FindData(name, out dataCollection, false);
				if (num != -1)
				{
					return dataCollection.List[num];
				}
				return null;
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x0000D0A8 File Offset: 0x0000B2A8
		private int FindData(string name, out DataCollection colec, bool buildTree)
		{
			if (this.list == null)
			{
				colec = null;
				return -1;
			}
			if (name.IndexOf('/') == -1)
			{
				for (int i = 0; i < this.list.Count; i++)
				{
					DataNode dataNode = this.list[i];
					if (dataNode.Name == name)
					{
						colec = this;
						return i;
					}
				}
				colec = this;
				return -1;
			}
			string[] array = name.Split(new char[]
			{
				'/'
			});
			int result = -1;
			colec = this;
			DataNode dataNode2 = null;
			for (int j = 0; j < array.Length; j++)
			{
				if (j > 0)
				{
					DataItem dataItem = dataNode2 as DataItem;
					if (dataItem != null)
					{
						colec = dataItem.ItemData;
					}
					else
					{
						if (!buildTree)
						{
							colec = null;
							return -1;
						}
						dataItem = new DataItem();
						dataItem.Name = array[j - 1];
						colec.Add(dataItem);
						colec = dataItem.ItemData;
					}
				}
				result = -1;
				for (int k = 0; k < colec.List.Count; k++)
				{
					dataNode2 = colec.List[k];
					if (dataNode2.Name == array[j])
					{
						result = k;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0000D1D1 File Offset: 0x0000B3D1
		public virtual IEnumerator GetEnumerator()
		{
			if (this.list != null)
			{
				return this.list.GetEnumerator();
			}
			return Type.EmptyTypes.GetEnumerator();
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0000D1F8 File Offset: 0x0000B3F8
		public void AddRange(DataCollection col)
		{
			foreach (object obj in col)
			{
				DataNode entry = (DataNode)obj;
				this.Add(entry);
			}
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0000D24C File Offset: 0x0000B44C
		public virtual void Add(DataNode entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.List.Add(entry);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0000D268 File Offset: 0x0000B468
		public virtual void Insert(int index, DataNode entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			this.List.Insert(index, entry);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000D288 File Offset: 0x0000B488
		public virtual void Add(DataNode entry, string itemPath)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			DataCollection dataCollection;
			this.FindData(itemPath + "/", out dataCollection, true);
			dataCollection.List.Add(entry);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		public virtual void Remove(DataNode entry)
		{
			if (this.list != null)
			{
				this.list.Remove(entry);
			}
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000D2DC File Offset: 0x0000B4DC
		public DataNode Extract(string name)
		{
			DataCollection dataCollection;
			int num = this.FindData(name, out dataCollection, false);
			if (num != -1)
			{
				DataNode result = dataCollection.List[num];
				dataCollection.list.RemoveAt(num);
				return result;
			}
			return null;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0000D314 File Offset: 0x0000B514
		public int IndexOf(DataNode entry)
		{
			if (this.list == null)
			{
				return -1;
			}
			return this.list.IndexOf(entry);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0000D32C File Offset: 0x0000B52C
		public virtual void Clear()
		{
			if (this.list != null)
			{
				this.list.Clear();
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0000D344 File Offset: 0x0000B544
		public void Merge(DataCollection col)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in col)
			{
				DataNode dataNode = (DataNode)obj;
				DataNode dataNode2 = this[dataNode.Name];
				if (dataNode2 == null)
				{
					arrayList.Add(dataNode);
				}
				else if (dataNode is DataItem && dataNode2 is DataItem)
				{
					((DataItem)dataNode2).ItemData.Merge(((DataItem)dataNode).ItemData);
				}
			}
			foreach (object obj2 in arrayList)
			{
				DataNode entry = (DataNode)obj2;
				this.Add(entry);
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x0000D484 File Offset: 0x0000B684
		public void Sort(Dictionary<string, int> nameToPosition)
		{
			this.list.Sort(delegate(DataNode x, DataNode y)
			{
				int maxValue;
				if (!nameToPosition.TryGetValue(x.Name, out maxValue))
				{
					maxValue = int.MaxValue;
				}
				int maxValue2;
				if (!nameToPosition.TryGetValue(y.Name, out maxValue2))
				{
					maxValue2 = int.MaxValue;
				}
				return maxValue.CompareTo(maxValue2);
			});
		}

		// Token: 0x04000126 RID: 294
		private List<DataNode> list = new List<DataNode>();
	}
}
