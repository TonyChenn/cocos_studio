using System;
using System.Globalization;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	public sealed class GraphVizNode
	{
		public GraphVizNode(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.ID = id;
		}

		public GraphVizNode(int id)
		{
			this.ID = id.ToString(CultureInfo.InvariantCulture);
		}

		public void Save(TextWriter writer)
		{
			writer.Write(this.ID);
			writer.Write(" [");
			bool flag = true;
			GraphVizGraph.WriteAttribute(writer, "label", this.label, ref flag);
			GraphVizGraph.WriteAttribute(writer, "labelloc", this.labelloc, ref flag);
			string name = "fontsize";
			int? num = this.fontsize;
			GraphVizGraph.WriteAttribute(writer, name, (num != null) ? new double?((double)num.GetValueOrDefault()) : null, ref flag);
			GraphVizGraph.WriteAttribute(writer, "margin", this.margin, ref flag);
			GraphVizGraph.WriteAttribute(writer, "shape", this.shape, ref flag);
			writer.WriteLine("];");
		}

		public readonly string ID;

		public string label;

		public string labelloc;

		/// <summary>point size of label</summary>
		public int? fontsize;

		/// <summary>minimum height in inches</summary>
		public double? height;

		/// <summary>space around label</summary>
		public string margin;

		/// <summary>node shape</summary>
		public string shape;
	}
}
