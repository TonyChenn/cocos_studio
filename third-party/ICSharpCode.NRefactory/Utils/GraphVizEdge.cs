using System;
using System.Globalization;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x0200011A RID: 282
	public sealed class GraphVizEdge
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x0001DEEE File Offset: 0x0001CEEE
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

		// Token: 0x06000A0D RID: 2573 RVA: 0x0001DF20 File Offset: 0x0001CF20
		public GraphVizEdge(int source, int target)
		{
			this.Source = source.ToString(CultureInfo.InvariantCulture);
			this.Target = target.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0001DF4C File Offset: 0x0001CF4C
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

		// Token: 0x04000363 RID: 867
		public readonly string Source;

		// Token: 0x04000364 RID: 868
		public readonly string Target;

		/// <summary>edge stroke color</summary>
		// Token: 0x04000365 RID: 869
		public string color;

		/// <summary>use edge to affect node ranking</summary>
		// Token: 0x04000366 RID: 870
		public bool? constraint;

		// Token: 0x04000367 RID: 871
		public string label;

		// Token: 0x04000368 RID: 872
		public string style;

		/// <summary>point size of label</summary>
		// Token: 0x04000369 RID: 873
		public int? fontsize;
	}
}
