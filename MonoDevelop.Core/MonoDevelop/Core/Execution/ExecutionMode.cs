using System;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000AD RID: 173
	public class ExecutionMode : IExecutionMode
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x000166BD File Offset: 0x000148BD
		public ExecutionMode()
		{
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x000166C5 File Offset: 0x000148C5
		public ExecutionMode(string id, string name, IExecutionHandler handler)
		{
			this.Id = id;
			this.Name = name;
			this.ExecutionHandler = handler;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000166E2 File Offset: 0x000148E2
		// (set) Token: 0x060005F2 RID: 1522 RVA: 0x000166EA File Offset: 0x000148EA
		public string Name { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x000166F3 File Offset: 0x000148F3
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x000166FB File Offset: 0x000148FB
		public string Id { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00016704 File Offset: 0x00014904
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001670C File Offset: 0x0001490C
		public IExecutionHandler ExecutionHandler { get; set; }
	}
}
