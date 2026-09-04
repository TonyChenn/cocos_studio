using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace OpenDialogs
{
	// Token: 0x0200004C RID: 76
	public static class SysListView32Helper
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x000081B0 File Offset: 0x000063B0
		public static bool IsRowSelected(this IntPtr handle, int rowindex)
		{
			return NativeMethods.SendMessage(handle, 4140, rowindex, 2) == 2;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x000081D4 File Offset: 0x000063D4
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

		// Token: 0x060001B8 RID: 440 RVA: 0x00008238 File Offset: 0x00006438
		public static int GetItemCount(this IntPtr handle)
		{
			return NativeMethods.SendMessage(handle, 4100, 0, 0);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008258 File Offset: 0x00006458
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

		// Token: 0x060001BA RID: 442 RVA: 0x0000841C File Offset: 0x0000661C
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

		// Token: 0x060001BB RID: 443 RVA: 0x00008474 File Offset: 0x00006674
		public static IntPtr GetSysListView32Handle(this IntPtr mainWindowHandle)
		{
			IntPtr hwndParent = NativeMethods.FindWindowEx(mainWindowHandle, IntPtr.Zero, "SHELLDLL_DefView", "");
			return NativeMethods.FindWindowEx(hwndParent, (IntPtr)null, "SysListView32", null);
		}

		// Token: 0x040002D3 RID: 723
		private const int LVIF_TEXT = 1;

		// Token: 0x0200004D RID: 77
		private enum PROCESS
		{
			// Token: 0x040002D5 RID: 725
			PROCESS_VM_OPERATION = 8,
			// Token: 0x040002D6 RID: 726
			PROCESS_VM_READ = 16,
			// Token: 0x040002D7 RID: 727
			PROCESS_VM_WRITE = 32
		}

		// Token: 0x0200004E RID: 78
		private enum SysListView32
		{
			// Token: 0x040002D9 RID: 729
			LVM_FIRST = 4096,
			// Token: 0x040002DA RID: 730
			HDM_GETITEMCOUNT = 4608,
			// Token: 0x040002DB RID: 731
			LVM_GETHEADER = 4127,
			// Token: 0x040002DC RID: 732
			LVM_GETITEMSTATE = 4140,
			// Token: 0x040002DD RID: 733
			LVIS_SELECTED = 2,
			// Token: 0x040002DE RID: 734
			LVM_GETITEMCOUNT = 4100,
			// Token: 0x040002DF RID: 735
			LVM_GETITEMW = 4171
		}

		// Token: 0x0200004F RID: 79
		private enum MEM
		{
			// Token: 0x040002E1 RID: 737
			MEM_COMMIT = 4096,
			// Token: 0x040002E2 RID: 738
			MEM_RELEASE = 32768,
			// Token: 0x040002E3 RID: 739
			MEM_RESERVE = 8192
		}

		// Token: 0x02000050 RID: 80
		private enum PAGE
		{
			// Token: 0x040002E5 RID: 741
			PAGE_READWRITE = 4
		}

		// Token: 0x02000051 RID: 81
		private struct LVITEM
		{
			// Token: 0x040002E6 RID: 742
			public int mask;

			// Token: 0x040002E7 RID: 743
			public int iItem;

			// Token: 0x040002E8 RID: 744
			public int iSubItem;

			// Token: 0x040002E9 RID: 745
			public int state;

			// Token: 0x040002EA RID: 746
			public int stateMask;

			// Token: 0x040002EB RID: 747
			public IntPtr pszText;

			// Token: 0x040002EC RID: 748
			public int cchTextMax;
		}
	}
}
