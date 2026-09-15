using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CocoStudio.EngineAdapterWrap
{
	public class UTF8Marshaler : ICustomMarshaler
	{
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

		public void CleanUpNativeData(IntPtr pNativeData)
		{
			Marshal.FreeHGlobal(pNativeData);
		}

		public void CleanUpManagedData(object managedObj)
		{
		}

		public int GetNativeDataSize()
		{
			return -1;
		}

		public static ICustomMarshaler GetInstance(string cookie)
		{
			return UTF8Marshaler.static_instance;
		}

		private static UTF8Marshaler static_instance = new UTF8Marshaler();
	}
}
