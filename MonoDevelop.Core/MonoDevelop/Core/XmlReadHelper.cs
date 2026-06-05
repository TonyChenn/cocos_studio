using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace MonoDevelop.Core
{
	// Token: 0x0200004D RID: 77
	public static class XmlReadHelper
	{
		// Token: 0x0600027A RID: 634 RVA: 0x00009FDC File Offset: 0x000081DC
		public static void ReadList(XmlReader reader, string endNode, XmlReadHelper.ReaderCallback callback)
		{
			XmlReadHelper.ReadList(reader, new string[]
			{
				endNode
			}, callback);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000A014 File Offset: 0x00008214
		public static void ReadList(XmlReader reader, ICollection<string> endNodes, XmlReadHelper.ReaderCallback callback)
		{
			XmlReadHelper.ReadList(reader, endNodes, (XmlReadHelper.ReadCallbackData data) => callback());
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000A044 File Offset: 0x00008244
		public static void ReadList(XmlReader reader, string endNode, XmlReadHelper.ReaderCallbackWithData callback)
		{
			XmlReadHelper.ReadList(reader, new string[]
			{
				endNode
			}, callback);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000A064 File Offset: 0x00008264
		private static string ConcatString(ICollection<string> strings)
		{
			string[] array = new string[strings.Count];
			strings.CopyTo(array, 0);
			return string.Join(",", array);
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000A090 File Offset: 0x00008290
		public static void ReadList(XmlReader reader, ICollection<string> endNodes, XmlReadHelper.ReaderCallbackWithData callback)
		{
			if (reader.IsEmptyElement)
			{
				return;
			}
			XmlReadHelper.ReadCallbackData readCallbackData = new XmlReadHelper.ReadCallbackData();
			bool flag = endNodes.Contains(reader.LocalName);
			while (readCallbackData.SkipNextRead || reader.Read())
			{
				readCallbackData.SkipNextRead = false;
				XmlNodeType nodeType = reader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType == XmlNodeType.EndElement)
					{
						if (endNodes.Contains(reader.LocalName))
						{
							return;
						}
						IXmlLineInfo xmlLineInfo = (IXmlLineInfo)reader;
						LoggingService.LogWarning("Encountered end node '{0}' when expecting one of '{1}'. Location ln:{2} col: {3}. Stack Trace:{4}", new object[]
						{
							reader.LocalName,
							XmlReadHelper.ConcatString(endNodes),
							xmlLineInfo.LineNumber,
							xmlLineInfo.LinePosition,
							new StackTrace()
						});
					}
				}
				else if (!flag && endNodes.Contains(reader.LocalName))
				{
					flag = true;
				}
				else if (!callback(readCallbackData))
				{
					LoggingService.LogWarning("Unknown node: " + reader.LocalName);
				}
			}
		}

		// Token: 0x0200004E RID: 78
		// (Invoke) Token: 0x06000280 RID: 640
		public delegate bool ReaderCallback();

		// Token: 0x0200004F RID: 79
		// (Invoke) Token: 0x06000284 RID: 644
		public delegate bool ReaderCallbackWithData(XmlReadHelper.ReadCallbackData data);

		// Token: 0x02000050 RID: 80
		public class ReadCallbackData
		{
			// Token: 0x1700007B RID: 123
			// (get) Token: 0x06000287 RID: 647 RVA: 0x0000A18D File Offset: 0x0000838D
			// (set) Token: 0x06000288 RID: 648 RVA: 0x0000A195 File Offset: 0x00008395
			public bool SkipNextRead
			{
				get
				{
					return this.skipNextRead;
				}
				set
				{
					this.skipNextRead = value;
				}
			}

			// Token: 0x040000E5 RID: 229
			private bool skipNextRead;
		}
	}
}
