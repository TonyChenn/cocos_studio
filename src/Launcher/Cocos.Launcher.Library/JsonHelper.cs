using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace Cocos.Launcher.Library
{
	// Token: 0x02000006 RID: 6
	public static class JsonHelper
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002B18 File Offset: 0x00000D18
		public static T Parse<T>(string jsonString)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
			{
				result = (T)((object)new DataContractJsonSerializer(typeof(T)).ReadObject(memoryStream));
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002B70 File Offset: 0x00000D70
		public static string Stringify(object jsonObject)
		{
			string @string;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(memoryStream, jsonObject);
				@string = Encoding.UTF8.GetString(memoryStream.ToArray());
			}
			return @string;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002BC4 File Offset: 0x00000DC4
		public static string EncryptMD5(this string str)
		{
			MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			byte[] array = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(str));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString().ToLower();
		}
	}
}
