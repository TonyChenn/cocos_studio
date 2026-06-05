using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000210 RID: 528
	public static class ProcessExtensions
	{
		// Token: 0x060013E4 RID: 5092 RVA: 0x000529A8 File Offset: 0x00050BA8
		public static void KillProcessTree(this Process p)
		{
			if (Platform.IsWindows)
			{
				Dictionary<int, List<int>> procRelations = ProcessExtensions.GetProcRelations();
				foreach (int processId in ProcessExtensions.GetAllChildren(procRelations, p.Id))
				{
					Process processById = Process.GetProcessById(processId);
					try
					{
						processById.Kill();
					}
					catch
					{
					}
				}
			}
			p.Kill();
		}

		// Token: 0x060013E5 RID: 5093 RVA: 0x00052CAC File Offset: 0x00050EAC
		private static IEnumerable<int> GetAllChildren(Dictionary<int, List<int>> procRelations, int pid)
		{
			List<int> children;
			if (procRelations.TryGetValue(pid, out children))
			{
				foreach (int cpid in children)
				{
					foreach (int c in ProcessExtensions.GetAllChildren(procRelations, cpid))
					{
						yield return c;
					}
					yield return cpid;
				}
			}
			yield break;
		}

		// Token: 0x060013E6 RID: 5094 RVA: 0x00052CD0 File Offset: 0x00050ED0
		private static Dictionary<int, List<int>> GetProcRelations()
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			IntPtr intPtr = ProcessExtensions.CreateToolhelp32Snapshot(2U, 0U);
			if (intPtr == IntPtr.Zero)
			{
				return dictionary;
			}
			ProcessExtensions.PROCESSENTRY32 processentry = default(ProcessExtensions.PROCESSENTRY32);
			processentry.dwSize = (uint)Marshal.SizeOf(typeof(ProcessExtensions.PROCESSENTRY32));
			if (!ProcessExtensions.Process32First(intPtr, ref processentry))
			{
				return dictionary;
			}
			do
			{
				List<int> list;
				if (!dictionary.TryGetValue((int)processentry.th32ParentProcessID, out list))
				{
					list = new List<int>();
					dictionary[(int)processentry.th32ParentProcessID] = list;
				}
				list.Add((int)processentry.th32ProcessID);
			}
			while (ProcessExtensions.Process32Next(intPtr, ref processentry));
			return dictionary;
		}

		// Token: 0x060013E7 RID: 5095
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

		// Token: 0x060013E8 RID: 5096
		[DllImport("kernel32.dll")]
		private static extern bool Process32First(IntPtr hSnapshot, ref ProcessExtensions.PROCESSENTRY32 lppe);

		// Token: 0x060013E9 RID: 5097
		[DllImport("kernel32.dll")]
		private static extern bool Process32Next(IntPtr hSnapshot, ref ProcessExtensions.PROCESSENTRY32 lppe);

		// Token: 0x040005F0 RID: 1520
		private const uint TH32CS_SNAPPROCESS = 2U;

		// Token: 0x040005F1 RID: 1521
		private const string kernel = "kernel32.dll";

		// Token: 0x02000211 RID: 529
		public struct PROCESSENTRY32
		{
			// Token: 0x040005F2 RID: 1522
			public uint dwSize;

			// Token: 0x040005F3 RID: 1523
			public uint cntUsage;

			// Token: 0x040005F4 RID: 1524
			public uint th32ProcessID;

			// Token: 0x040005F5 RID: 1525
			public IntPtr th32DefaultHeapID;

			// Token: 0x040005F6 RID: 1526
			public uint th32ModuleID;

			// Token: 0x040005F7 RID: 1527
			public uint cntThreads;

			// Token: 0x040005F8 RID: 1528
			public uint th32ParentProcessID;

			// Token: 0x040005F9 RID: 1529
			public int pcPriClassBase;

			// Token: 0x040005FA RID: 1530
			public uint dwFlags;

			// Token: 0x040005FB RID: 1531
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szExeFile;
		}
	}
}
