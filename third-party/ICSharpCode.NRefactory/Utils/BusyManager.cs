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
	public static class BusyManager
	{
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

		[ThreadStatic]
		private static List<object> _activeObjects;

		public struct BusyLock : IDisposable
		{
			internal BusyLock(List<object> objectList)
			{
				this.objectList = objectList;
			}

			public bool Success
			{
				get
				{
					return this.objectList != null;
				}
			}

			public void Dispose()
			{
				if (this.objectList != null)
				{
					this.objectList.RemoveAt(this.objectList.Count - 1);
				}
			}

			public static readonly BusyManager.BusyLock Failed = new BusyManager.BusyLock(null);

			private readonly List<object> objectList;
		}
	}
}
