using System;
using System.IO;

namespace MonoDevelop.Projects.Text
{
	// Token: 0x020001FF RID: 511
	public class TextFileReader : TextReader
	{
		// Token: 0x06001366 RID: 4966 RVA: 0x000503D8 File Offset: 0x0004E5D8
		public TextFileReader(string fileName)
		{
			TextFile textFile = TextFile.ReadFile(fileName);
			this.reader = new StringReader(textFile.Text);
			this.sourceEncoding = textFile.SourceEncoding;
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00050414 File Offset: 0x0004E614
		public override void Close()
		{
			this.reader.Close();
		}

		// Token: 0x06001368 RID: 4968 RVA: 0x00050421 File Offset: 0x0004E621
		public override int Peek()
		{
			return this.reader.Peek();
		}

		// Token: 0x06001369 RID: 4969 RVA: 0x0005042E File Offset: 0x0004E62E
		public override int Read()
		{
			return this.reader.Read();
		}

		// Token: 0x0600136A RID: 4970 RVA: 0x0005043B File Offset: 0x0004E63B
		public override int Read(char[] buffer, int index, int len)
		{
			return this.reader.Read(buffer, index, len);
		}

		// Token: 0x0600136B RID: 4971 RVA: 0x0005044B File Offset: 0x0004E64B
		public override string ReadLine()
		{
			return this.reader.ReadLine();
		}

		// Token: 0x0600136C RID: 4972 RVA: 0x00050458 File Offset: 0x0004E658
		public override string ReadToEnd()
		{
			return this.reader.ReadToEnd();
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600136D RID: 4973 RVA: 0x00050465 File Offset: 0x0004E665
		public string SourceEncoding
		{
			get
			{
				return this.sourceEncoding;
			}
		}

		// Token: 0x0600136E RID: 4974 RVA: 0x0005046D File Offset: 0x0004E66D
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.reader != null)
			{
				this.reader.Close();
			}
			this.reader = null;
			base.Dispose(disposing);
		}

		// Token: 0x040005AC RID: 1452
		private StringReader reader;

		// Token: 0x040005AD RID: 1453
		private string sourceEncoding;
	}
}
