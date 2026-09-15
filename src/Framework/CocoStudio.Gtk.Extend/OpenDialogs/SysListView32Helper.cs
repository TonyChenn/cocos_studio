using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDialogs
{
	public static class SysListView32Helper
	{
		public static bool IsRowSelected(this IntPtr handle, int rowindex)
		{
			return NativeMethods.SendMessage(handle, 4140, rowindex, 2) == 2;
		}

		public static List<int> GetSelectedRowsIndex(this IntPtr handle)
		{
			List<int> list = new List<int>();
			int num = NativeMethods.SendMessage(handle, 4100, 0, 0);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					if (handle.IsRowSelected(i))
					{
						list.Add(i);
					}
				}
			}
			return list;
		}

		public static int GetItemCount(this IntPtr handle)
		{
			return NativeMethods.SendMessage(handle, 4100, 0, 0);
		}

		public static List<string> GetItemsText(this IntPtr handle, List<int> rowIndex, int column)
		{
			List<string> list = new List<string>();
			if (rowIndex != null)
			{
				int num = 0;
				NativeMethods.GetWindowThreadProcessId(handle, ref num);
				if (num != 0)
				{
					int hProcess = NativeMethods.OpenProcess(56, false, num);
					int num2 = NativeMethods.VirtualAllocEx(hProcess, IntPtr.Zero, 4096U, 12288U, 4U);
					int itemCount = handle.GetItemCount();
					if (itemCount > 0 && rowIndex.Count <= itemCount)
					{
						foreach (int num3 in rowIndex)
						{
							byte[] array = new byte[256];
							SysListView32Helper.LVITEM[] array2 = new SysListView32Helper.LVITEM[1];
							array2[0].mask = 1;
							array2[0].iItem = num3;
							array2[0].iSubItem = column;
							array2[0].cchTextMax = array.Length;
							array2[0].pszText = (IntPtr)(num2 + Marshal.SizeOf(typeof(SysListView32Helper.LVITEM)));
							uint count = 0U;
							NativeMethods.WriteProcessMemory(hProcess, num2, Marshal.UnsafeAddrOfPinnedArrayElement(array2, 0), Marshal.SizeOf(typeof(SysListView32Helper.LVITEM)), ref count);
							NativeMethods.SendMessage(handle, 4171, num3, num2);
							NativeMethods.ReadProcessMemory(hProcess, num2 + Marshal.SizeOf(typeof(SysListView32Helper.LVITEM)), Marshal.UnsafeAddrOfPinnedArrayElement(array, 0), array.Length, ref count);
							list.Add(Encoding.Unicode.GetString(array, 0, (int)count));
						}
					}
				}
			}
			return list;
		}

		public static List<string> GetSlectedItemsText(this IntPtr handle, int column = 0)
		{
			List<string> list = new List<string>();
			if (handle != IntPtr.Zero)
			{
				List<int> selectedRowsIndex = handle.GetSelectedRowsIndex();
				if (selectedRowsIndex.Count > 0)
				{
					list.AddRange(handle.GetItemsText(selectedRowsIndex, column));
				}
			}
			return list;
		}

		public static IntPtr GetSysListView32Handle(this IntPtr mainWindowHandle)
		{
			IntPtr hwndParent = NativeMethods.FindWindowEx(mainWindowHandle, IntPtr.Zero, "SHELLDLL_DefView", "");
			return NativeMethods.FindWindowEx(hwndParent, (IntPtr)null, "SysListView32", null);
		}

		private const int LVIF_TEXT = 1;

		private enum PROCESS
		{
			PROCESS_VM_OPERATION = 8,
			PROCESS_VM_READ = 16,
			PROCESS_VM_WRITE = 32
		}

		private enum SysListView32
		{
			LVM_FIRST = 4096,
			HDM_GETITEMCOUNT = 4608,
			LVM_GETHEADER = 4127,
			LVM_GETITEMSTATE = 4140,
			LVIS_SELECTED = 2,
			LVM_GETITEMCOUNT = 4100,
			LVM_GETITEMW = 4171
		}

		private enum MEM
		{
			MEM_COMMIT = 4096,
			MEM_RELEASE = 32768,
			MEM_RESERVE = 8192
		}

		private enum PAGE
		{
			PAGE_READWRITE = 4
		}

		private struct LVITEM
		{
			public int mask;

			public int iItem;

			public int iSubItem;

			public int state;

			public int stateMask;

			public IntPtr pszText;

			public int cchTextMax;
		}
	}
}
