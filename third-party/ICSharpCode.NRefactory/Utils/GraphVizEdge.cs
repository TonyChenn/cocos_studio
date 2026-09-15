using System;
using System.Globalization;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	public sealed class GraphVizEdge
	{
		public GraphVizEdge(string source, string target)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (target == null)
			{
				throw new ArgumentNullException("target");
			}
			this.Source = source;
			this.Target = target;
		}

		public GraphVizEdge(int source, int target)
		{
			this.Source = source.ToString(CultureInfo.InvariantCulture);
			this.Target = target.ToString(CultureInfo.InvariantCulture);
		}

		public void Save(TextWriter writer)
		{
			writer.Write("{0} -> {1} [", this.Source, this.Target);
			bool flag = true;
			GraphVizGraph.WriteAttribute(writer, "label", this.label, ref flag);
			GraphVizGraph.WriteAttribute(writer, "style", this.style, ref flag);
			string name = "fontsize";
			int? num = this.fontsize;
			GraphVizGraph.WriteAttribute(writer, name, (num != null) ? new double?((double)num.GetValueOrDefault()) : null, ref flag);
			GraphVizGraph.WriteAttribute(writer, "color", this.color, ref flag);
			GraphVizGraph.WriteAttribute(writer, "constraint", this.constraint, ref flag);
			writer.WriteLine("];");
		}

		public readonly string Source;

		public readonly string Target;

		/// <summary>edge stroke color</summary>
		public string color;

		/// <summary>use edge to affect node ranking</summary>
		public bool? constraint;

		public string label;

		public string style;

		/// <summary>point size of label</summary>
		public int? fontsize;
	}
}
