using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Modules.Communal.MutualEditor
{
	internal static class SerializersHelper
	{
		public static MemoryStream SerializeBinary(this object request)
		{
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize(memoryStream, request);
			return memoryStream;
		}

		public static T DeSerializeBinary<T>(this MemoryStream memStream) where T : class
		{
			return memStream.DeSerializeBinary() as T;
		}

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
