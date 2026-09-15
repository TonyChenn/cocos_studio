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
	public sealed class GraphVizGraph
	{
		public void AddEdge(GraphVizEdge edge)
		{
			this.edges.Add(edge);
		}

		public void AddNode(GraphVizNode node)
		{
			this.nodes.Add(node);
		}

		public void Save(string fileName)
		{
			using (StreamWriter streamWriter = new StreamWriter(fileName))
			{
				this.Save(streamWriter);
			}
		}

		public void Show()
		{
			this.Show(null);
		}

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

		private static string Escape(string text)
		{
			if (Regex.IsMatch(text, "^[\\w\\d]+$"))
			{
				return text;
			}
			return "\"" + text.Replace("\\", "\\\\").Replace("\r", "").Replace("\n", "\\n").Replace("\"", "\\\"") + "\"";
		}

		private static void WriteGraphAttribute(TextWriter writer, string name, string value)
		{
			if (value != null)
			{
				writer.WriteLine("{0}={1};", name, GraphVizGraph.Escape(value));
			}
		}

		internal static void WriteAttribute(TextWriter writer, string name, double? value, ref bool isFirst)
		{
			if (value != null)
			{
				GraphVizGraph.WriteAttribute(writer, name, value.Value.ToString(CultureInfo.InvariantCulture), ref isFirst);
			}
		}

		internal static void WriteAttribute(TextWriter writer, string name, bool? value, ref bool isFirst)
		{
			if (value != null)
			{
				GraphVizGraph.WriteAttribute(writer, name, value.Value ? "true" : "false", ref isFirst);
			}
		}

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

		private List<GraphVizNode> nodes = new List<GraphVizNode>();

		private List<GraphVizEdge> edges = new List<GraphVizEdge>();

		public string rankdir;

		public string Title;
	}
}
