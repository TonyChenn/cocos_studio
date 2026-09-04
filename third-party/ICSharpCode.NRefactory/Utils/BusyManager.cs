using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// This class is used to prevent stack overflows by representing a 'busy' flag
	/// that prevents reentrance when another call is running.
	/// However, using a simple 'bool busy' is not thread-safe, so we use a
	/// thread-static BusyManager.
	/// </summary>
	// Token: 0x02000107 RID: 263
	public static class BusyManager
	{
		// Token: 0x06000988 RID: 2440 RVA: 0x000198A4 File Offset: 0x000188A4
		public static BusyManager.BusyLock Enter(object obj)
		{
			List<object> list = BusyManager._activeObjects;
			if (list == null)
			{
				list = (BusyManager._activeObjects = new List<object>());
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] == obj)
				{
					return BusyManager.BusyLock.Failed;
				}
			}
			list.Add(obj);
			return new BusyManager.BusyLock(list);
		}

		// Token: 0x04000313 RID: 787
		[ThreadStatic]
		private static List<object> _activeObjects;

		// Token: 0x02000108 RID: 264
		public struct BusyLock : IDisposable
		{
			// Token: 0x06000989 RID: 2441 RVA: 0x000198F4 File Offset: 0x000188F4
			internal BusyLock(List<object> objectList)
			{
				this.objectList = objectList;
			}

			// Token: 0x170003D9 RID: 985
			// (get) Token: 0x0600098A RID: 2442 RVA: 0x000198FD File Offset: 0x000188FD
			public bool Success
			{
				get
				{
					return this.objectList != null;
				}
			}

			// Token: 0x0600098B RID: 2443 RVA: 0x0001990B File Offset: 0x0001890B
			public void Dispose()
			{
				if (this.objectList != null)
				{
					this.objectList.RemoveAt(this.objectList.Count - 1);
				}
			}

			// Token: 0x04000314 RID: 788
			public static readonly BusyManager.BusyLock Failed = new BusyManager.BusyLock(null);

			// Token: 0x04000315 RID: 789
			private readonly List<object> objectList;
		}
	}
}
