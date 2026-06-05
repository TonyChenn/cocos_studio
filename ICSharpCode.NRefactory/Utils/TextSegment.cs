using System;

namespace ICSharpCode.NRefactory.Utils
{
	// Token: 0x02000135 RID: 309
	public class TextSegment : FormatStringSegmentBase
	{
		// Token: 0x06000ABB RID: 2747 RVA: 0x0002031C File Offset: 0x0001F31C
		public TextSegment(string text, int startLocation = 0, int? endLocation = null)
		{
			this.Text = text;
			base.StartLocation = startLocation;
			base.EndLocation = (endLocation ?? (startLocation + text.Length));
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000ABC RID: 2748 RVA: 0x0002035F File Offset: 0x0001F35F
		// (set) Token: 0x06000ABD RID: 2749 RVA: 0x00020367 File Offset: 0x0001F367
		public string Text { get; set; }

		// Token: 0x06000ABE RID: 2750 RVA: 0x00020370 File Offset: 0x0001F370
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj.GetType() != typeof(TextSegment))
			{
				return false;
			}
			TextSegment textSegment = (TextSegment)obj;
			return object.Equals(this.Text, textSegment.Text);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x000203B3 File Offset: 0x0001F3B3
		public bool Equals(TextSegment other)
		{
			return other != null && (object.Equals(this.Text, other.Text) && base.StartLocation == other.StartLocation) && base.EndLocation == other.EndLocation;
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x000203EC File Offset: 0x0001F3EC
		public override int GetHashCode()
		{
			int num = 23;
			num = num * 37 + this.Text.GetHashCode();
			num = num * 37 + base.StartLocation.GetHashCode();
			return num * 37 + base.EndLocation.GetHashCode();
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00020436 File Offset: 0x0001F436
		public override string ToString()
		{
			return string.Format("[TextSegment: Text={0}, StartLocation={1}, EndLocation={2}]", this.Text, base.StartLocation, base.EndLocation);
		}
	}
}
