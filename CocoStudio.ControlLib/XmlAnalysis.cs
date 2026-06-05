using System;
using System.IO;
using System.Xml;
using Gtk;

namespace CocoStudio.ControlLib
{
	// Token: 0x02000009 RID: 9
	public class XmlAnalysis
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00003578 File Offset: 0x00001778
		public static XmlNode GetNode(XmlNode node, string name)
		{
			if (node != null && node.HasChildNodes)
			{
				foreach (object obj in node.ChildNodes)
				{
					XmlNode xmlNode = (XmlNode)obj;
					if (xmlNode.Name == name)
					{
						return xmlNode;
					}
				}
			}
			return null;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003610 File Offset: 0x00001810
		public static string GetAttribute(XmlNode node, string name)
		{
			if (node != null && node.Attributes != null)
			{
				foreach (object obj in node.Attributes)
				{
					XmlAttribute xmlAttribute = (XmlAttribute)obj;
					if (xmlAttribute.Name == name)
					{
						return xmlAttribute.Value;
					}
				}
			}
			return "";
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000036B0 File Offset: 0x000018B0
		public static XmlDocument ReaderXmlFile(string path)
		{
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				if (File.Exists(path))
				{
					XmlAnalysis.file = new FileStream(path, FileMode.Open);
					XmlAnalysis.reader = new StreamReader(XmlAnalysis.file);
					xmlDocument.Load(XmlAnalysis.reader);
					XmlAnalysis.reader.Close();
					XmlAnalysis.file.Close();
				}
				else
				{
					MessageBox.Show("文件不存在", MessageBoxImage.Other, null, null);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("解析xml文件发生异常:" + ex.Message, MessageBoxImage.Other, null, null);
			}
			return xmlDocument;
		}

		// Token: 0x04000025 RID: 37
		private static FileStream file;

		// Token: 0x04000026 RID: 38
		private static StreamReader reader;
	}
}
