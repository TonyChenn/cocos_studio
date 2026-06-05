using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000244 RID: 580
	public class ExecutionTargetGroup : ExecutionTarget, IList<ExecutionTarget>, ICollection<ExecutionTarget>, IEnumerable<ExecutionTarget>, IEnumerable
	{
		// Token: 0x06001553 RID: 5459 RVA: 0x00057005 File Offset: 0x00055205
		public ExecutionTargetGroup(string name, string id)
		{
			this.targets = new List<ExecutionTarget>();
			this.name = name;
			this.id = id;
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x06001554 RID: 5460 RVA: 0x00057026 File Offset: 0x00055226
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x06001555 RID: 5461 RVA: 0x0005702E File Offset: 0x0005522E
		public override string Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00057036 File Offset: 0x00055236
		public int IndexOf(ExecutionTarget target)
		{
			return this.targets.IndexOf(target);
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00057044 File Offset: 0x00055244
		public void Insert(int index, ExecutionTarget target)
		{
			target.ParentGroup = this;
			this.targets.Insert(index, target);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x0005705C File Offset: 0x0005525C
		public void RemoveAt(int index)
		{
			ExecutionTarget executionTarget = this.targets[index];
			executionTarget.ParentGroup = null;
			this.targets.RemoveAt(index);
		}

		// Token: 0x17000485 RID: 1157
		public ExecutionTarget this[int index]
		{
			get
			{
				return this.targets[index];
			}
			set
			{
				ExecutionTarget executionTarget = this.targets[index];
				executionTarget.ParentGroup = null;
				this.targets[index] = value;
				value.ParentGroup = this;
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x000570CD File Offset: 0x000552CD
		public void Add(ExecutionTarget target)
		{
			target.ParentGroup = this;
			this.targets.Add(target);
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x000570E4 File Offset: 0x000552E4
		public void Clear()
		{
			foreach (ExecutionTarget executionTarget in this.targets)
			{
				executionTarget.ParentGroup = null;
			}
			this.targets.Clear();
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00057144 File Offset: 0x00055344
		public bool Contains(ExecutionTarget target)
		{
			return this.targets.Contains(target);
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00057152 File Offset: 0x00055352
		public void CopyTo(ExecutionTarget[] array, int arrayIndex)
		{
			this.targets.CopyTo(array, arrayIndex);
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00057161 File Offset: 0x00055361
		public bool Remove(ExecutionTarget target)
		{
			target.ParentGroup = null;
			return this.targets.Remove(target);
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x00057176 File Offset: 0x00055376
		public int Count
		{
			get
			{
				return this.targets.Count;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x06001561 RID: 5473 RVA: 0x00057183 File Offset: 0x00055383
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x00057186 File Offset: 0x00055386
		public IEnumerator<ExecutionTarget> GetEnumerator()
		{
			return this.targets.GetEnumerator();
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00057198 File Offset: 0x00055398
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.targets.GetEnumerator();
		}

		// Token: 0x0400066B RID: 1643
		private List<ExecutionTarget> targets;

		// Token: 0x0400066C RID: 1644
		private string name;

		// Token: 0x0400066D RID: 1645
		private string id;
	}
}
