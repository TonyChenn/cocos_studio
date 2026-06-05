using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Modules.Communal.MutualEditor
{
	// Token: 0x02000009 RID: 9
	internal static class SerializersHelper
	{
		// Token: 0x0600001D RID: 29 RVA: 0x000026D0 File Offset: 0x000008D0
		public static MemoryStream SerializeBinary(this object request)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, request);
			return memoryStream;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000026F8 File Offset: 0x000008F8
		public static T DeSerializeBinary<T>(this MemoryStream memStream) where T : class
		{
			return memStream.DeSerializeBinary() as T;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000271C File Offset: 0x0000091C
		public static object DeSerializeBinary(this MemoryStream memStream)
		{
			try
			{
				memStream.Position = 0L;
				object result = new BinaryFormatter().Deserialize(memStream);
				memStream.Close();
				return result;
			}
			catch
			{
			}
			return null;
		}
	}
}
