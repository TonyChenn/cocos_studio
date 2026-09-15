using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace Cocos.Launcher.Library
{
	public static class JsonHelper
	{
		public static T Parse<T>(string jsonString)
		{
			T result;
			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
			{
				result = (T)((object)new DataContractJsonSerializer(typeof(T)).ReadObject(memoryStream));
			}
			return result;
		}

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
