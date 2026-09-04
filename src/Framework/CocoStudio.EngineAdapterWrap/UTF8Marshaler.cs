using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200001E RID: 30
	public class UTF8Marshaler : ICustomMarshaler
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00006D8C File Offset: 0x00004F8C
		public IntPtr MarshalManagedToNative(object managedObj)
		{
			IntPtr result;
			if (managedObj == null)
			{
				result = IntPtr.Zero;
			}
			else
			{
				if (!(managedObj is string))
				{
					throw new MarshalDirectiveException("UTF8Marshaler must be used on a string.");
				}
				byte[] bytes = Encoding.UTF8.GetBytes((string)managedObj);
				IntPtr intPtr = Marshal.AllocHGlobal(bytes.Length + 1);
				Marshal.Copy(bytes, 0, intPtr, bytes.Length);
				Marshal.WriteByte(intPtr + bytes.Length, 0);
				result = intPtr;
			}
			return result;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00006E04 File Offset: 0x00005004
		public unsafe object MarshalNativeToManaged(IntPtr pNativeData)
		{
			byte* ptr = (byte*)((void*)pNativeData);
			while (*ptr != 0)
			{
				ptr++;
			}
			int num = (int)((long)((byte*)ptr - (byte*)((void*)pNativeData)));
			byte[] array = new byte[num];
			Marshal.Copy(pNativeData, array, 0, num);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00006E61 File Offset: 0x00005061
		public void CleanUpNativeData(IntPtr pNativeData)
		{
			Marshal.FreeHGlobal(pNativeData);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00006E6B File Offset: 0x0000506B
		public void CleanUpManagedData(object managedObj)
		{
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00006E70 File Offset: 0x00005070
		public int GetNativeDataSize()
		{
			return -1;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00006E84 File Offset: 0x00005084
		public static ICustomMarshaler GetInstance(string cookie)
		{
			return UTF8Marshaler.static_instance;
		}

		// Token: 0x04000021 RID: 33
		private static UTF8Marshaler static_instance = new UTF8Marshaler();
	}
}
