using System;
using System.IO;
using System.Xml;
using Gtk;

namespace CocoStudio.ControlLib
{
	public class XmlAnalysis
	{
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

		private static FileStream file;

		private static StreamReader reader;
	}
}
