using System;
using System.Globalization;
using System.IO;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x0200011B RID: 283
	public sealed class GraphVizNode
	{
		// Token: 0x06000A0F RID: 2575 RVA: 0x0001DFFE File Offset: 0x0001CFFE
		public GraphVizNode(string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}
			this.ID = id;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0001E01B File Offset: 0x0001D01B
		public GraphVizNode(int id)
		{
			this.ID = id.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0001E038 File Offset: 0x0001D038
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

		// Token: 0x0400036A RID: 874
		public readonly string ID;

		// Token: 0x0400036B RID: 875
		public string label;

		// Token: 0x0400036C RID: 876
		public string labelloc;

		/// <summary>point size of label</summary>
		// Token: 0x0400036D RID: 877
		public int? fontsize;

		/// <summary>minimum height in inches</summary>
		// Token: 0x0400036E RID: 878
		public double? height;

		/// <summary>space around label</summary>
		// Token: 0x0400036F RID: 879
		public string margin;

		/// <summary>node shape</summary>
		// Token: 0x04000370 RID: 880
		public string shape;
	}
}
