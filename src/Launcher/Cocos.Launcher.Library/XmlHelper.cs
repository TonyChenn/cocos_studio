using System;
using System.IO;
using System.Xml.Serialization;
using CocoStudio.Basic;

namespace Cocos.Launcher.Library
{
	// Token: 0x02000008 RID: 8
	public static class XmlHelper
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00003058 File Offset: 0x00001258
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

		// Token: 0x06000037 RID: 55 RVA: 0x0000309C File Offset: 0x0000129C
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

		// Token: 0x06000038 RID: 56 RVA: 0x00003108 File Offset: 0x00001308
		public static object LoadXml(Stream stream, Type type)
		{
			if (stream == null || stream.Length == 0L)
			{
				return null;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(type);
			return xmlSerializer.Deserialize(stream);
		}

		// Token: 0x0400001C RID: 28
		public static string FileDirectory = string.Empty;

		// Token: 0x02000009 RID: 9
		public static class XMLAction<T> where T : class
		{
			// Token: 0x0600003A RID: 58 RVA: 0x00003140 File Offset: 0x00001340
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

			// Token: 0x0600003B RID: 59 RVA: 0x000031A8 File Offset: 0x000013A8
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

			// Token: 0x0600003C RID: 60 RVA: 0x00003208 File Offset: 0x00001408
			public static void WriteData(T obj, string fullpath)
			{
				XmlHelper.SaveXml(fullpath, obj);
			}
		}
	}
}
