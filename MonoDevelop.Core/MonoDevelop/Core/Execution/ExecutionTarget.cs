using System;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// A target that can execute a command. For example, a specific device when doing mobile development
	/// </summary>
	// Token: 0x02000243 RID: 579
	public abstract class ExecutionTarget
	{
		// Token: 0x06001546 RID: 5446 RVA: 0x00056F66 File Offset: 0x00055166
		protected ExecutionTarget()
		{
			this.Enabled = true;
		}

		/// <summary>
		/// Display name of the device
		/// </summary>
		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001547 RID: 5447
		public abstract string Name { get; }

		/// <summary>
		/// The display name of the item when it is selected
		/// </summary>
		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00056F75 File Offset: 0x00055175
		public virtual string FullName
		{
			get
			{
				return this.Name;
			}
		}

		/// <summary>
		/// Unique identifier of the target
		/// </summary>
		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001549 RID: 5449
		public abstract string Id { get; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:MonoDevelop.Core.Execution.ExecutionTarget" /> is enabled.
		/// </summary>
		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x00056F7D File Offset: 0x0005517D
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x00056F85 File Offset: 0x00055185
		public bool Enabled { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="T:MonoDevelop.Core.Execution.ExecutionTarget" /> is notable.
		/// </summary>
		/// <remarks>
		/// This is introduced to be able to highlight execution targets for whatever reason makes sense for the project. 
		/// For example, the android add-in uses this to indicate which emulators are currently running but other addins can use this
		/// for their own purposes
		/// </remarks>
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00056F8E File Offset: 0x0005518E
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x00056F96 File Offset: 0x00055196
		public bool Notable { get; set; }

		/// <summary>
		/// Target group on which this target is included
		/// </summary>
		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00056F9F File Offset: 0x0005519F
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x00056FA7 File Offset: 0x000551A7
		public ExecutionTargetGroup ParentGroup { get; internal set; }

		// Token: 0x06001550 RID: 5456 RVA: 0x00056FB0 File Offset: 0x000551B0
		public override bool Equals(object obj)
		{
			ExecutionTarget executionTarget = obj as ExecutionTarget;
			return executionTarget != null && executionTarget.Id == this.Id;
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00056FDA File Offset: 0x000551DA
		public override int GetHashCode()
		{
			return this.Id.GetHashCode();
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x00056FE7 File Offset: 0x000551E7
		public override string ToString()
		{
			return string.Format("[ExecutionTarget: Name={0}, FullName={1}, Id={2}]", this.Name, this.FullName, this.Id);
		}
	}
}
