using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ICSharpCode.NRefactory.Utils
{
	/// <summary>
	/// GraphViz graph.
	/// </summary>
	// Token: 0x02000119 RID: 281
	public sealed class GraphVizGraph
	{
		// Token: 0x06000A00 RID: 2560 RVA: 0x0001DBD7 File Offset: 0x0001CBD7
		public void AddEdge(GraphVizEdge edge)
		{
			this.edges.Add(edge);
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0001DBE5 File Offset: 0x0001CBE5
		public void AddNode(GraphVizNode node)
		{
			this.nodes.Add(node);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0001DBF4 File Offset: 0x0001CBF4
		public void Save(string fileName)
		{
			using (StreamWriter streamWriter = new StreamWriter(fileName))
			{
				this.Save(streamWriter);
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0001DC2C File Offset: 0x0001CC2C
		public void Show()
		{
			this.Show(null);
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x0001DC38 File Offset: 0x0001CC38
		public void Show(string name)
		{
			if (name == null)
			{
				name = this.Title;
			}
			if (name != null)
			{
				foreach (char oldChar in Path.GetInvalidFileNameChars())
				{
					name = name.Replace(oldChar, '-');
				}
			}
			string text = (name != null) ? Path.Combine(Path.GetTempPath(), name) : Path.GetTempFileName();
			this.Save(text + ".gv");
			Process.Start("dot", string.Concat(new string[]
			{
				"\"",
				text,
				".gv\" -Tpng -o \"",
				text,
				".png\""
			})).WaitForExit();
			Process.Start(text + ".png");
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x0001DCF4 File Offset: 0x0001CCF4
		private static string Escape(string text)
		{
			if (Regex.IsMatch(text, "^[\\w\\d]+$"))
			{
				return text;
			}
			return "\"" + text.Replace("\\", "\\\\").Replace("\r", "").Replace("\n", "\\n").Replace("\"", "\\\"") + "\"";
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0001DD5C File Offset: 0x0001CD5C
		private static void WriteGraphAttribute(TextWriter writer, string name, string value)
		{
			if (value != null)
			{
				writer.WriteLine("{0}={1};", name, GraphVizGraph.Escape(value));
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0001DD74 File Offset: 0x0001CD74
		internal static void WriteAttribute(TextWriter writer, string name, double? value, ref bool isFirst)
		{
			if (value != null)
			{
				GraphVizGraph.WriteAttribute(writer, name, value.Value.ToString(CultureInfo.InvariantCulture), ref isFirst);
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0001DDA6 File Offset: 0x0001CDA6
		internal static void WriteAttribute(TextWriter writer, string name, bool? value, ref bool isFirst)
		{
			if (value != null)
			{
				GraphVizGraph.WriteAttribute(writer, name, value.Value ? "true" : "false", ref isFirst);
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0001DDCE File Offset: 0x0001CDCE
		internal static void WriteAttribute(TextWriter writer, string name, string value, ref bool isFirst)
		{
			if (value != null)
			{
				if (isFirst)
				{
					isFirst = false;
				}
				else
				{
					writer.Write(',');
				}
				writer.Write("{0}={1}", name, GraphVizGraph.Escape(value));
			}
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0001DDF8 File Offset: 0x0001CDF8
		public void Save(TextWriter writer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException("writer");
			}
			writer.WriteLine("digraph G {");
			writer.WriteLine("node [fontsize = 16];");
			GraphVizGraph.WriteGraphAttribute(writer, "rankdir", this.rankdir);
			foreach (GraphVizNode graphVizNode in this.nodes)
			{
				graphVizNode.Save(writer);
			}
			foreach (GraphVizEdge graphVizEdge in this.edges)
			{
				graphVizEdge.Save(writer);
			}
			writer.WriteLine("}");
		}

		// Token: 0x0400035F RID: 863
		private List<GraphVizNode> nodes = new List<GraphVizNode>();

		// Token: 0x04000360 RID: 864
		private List<GraphVizEdge> edges = new List<GraphVizEdge>();

		// Token: 0x04000361 RID: 865
		public string rankdir;

		// Token: 0x04000362 RID: 866
		public string Title;
	}
}
