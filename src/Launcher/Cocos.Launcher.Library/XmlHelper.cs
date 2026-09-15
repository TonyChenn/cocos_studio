using System;
using System.IO;
using System.Xml.Serialization;
using CocoStudio.Basic;

namespace Cocos.Launcher.Library
{
	public static class XmlHelper
	{
		public static void SaveXml(string filePath, object obj)
		{
			try
			{
				XmlHelper.SaveXml(filePath, obj, obj.GetType());
			}
			catch (Exception arg)
			{
				LogConfig.Output.Error("序列化失败：" + arg);
			}
		}

		public static void SaveXml(string filePath, object obj, Type type)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Directory.Exists)
			{
				Directory.CreateDirectory(fileInfo.Directory.FullName);
			}
			using (StreamWriter streamWriter = new StreamWriter(filePath))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(type);
				xmlSerializer.Serialize(streamWriter, obj);
				streamWriter.Close();
			}
		}

		public static object LoadXml(Stream stream, Type type)
		{
			if (stream == null || stream.Length == 0L)
			{
				return null;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(stream);
		}

		public static string FileDirectory = string.Empty;

		public static class XMLAction<T> where T : class
		{
			public static T ReadData(string FullPath)
			{
				T result;
				try
				{
					using (FileStream fileStream = new FileStream(FullPath, FileMode.Open, FileAccess.Read))
					{
						result = XmlHelper.XMLAction<T>.ReadData(fileStream);
					}
				}
				catch (Exception arg)
				{
					LogConfig.Output.Error("读取文件失败：" + arg);
					result = default(T);
				}
				return result;
			}

			public static T ReadData(Stream stream)
			{
				T result;
				try
				{
					result = (XmlHelper.LoadXml(stream, typeof(T)) as T);
				}
				catch (Exception arg)
				{
					LogConfig.Output.Error("反序列化失败:" + arg);
					result = default(T);
				}
				return result;
			}

			public static void WriteData(T obj, string fullpath)
			{
				XmlHelper.SaveXml(fullpath, obj);
			}
		}
	}
}
