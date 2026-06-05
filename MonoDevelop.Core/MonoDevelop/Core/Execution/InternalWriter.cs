using System;
using System.IO;
using System.Text;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000EA RID: 234
	internal class InternalWriter : TextWriter
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x0002146A File Offset: 0x0001F66A
		public InternalReader DataReader
		{
			get
			{
				return this.data;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00021472 File Offset: 0x0001F672
		public override Encoding Encoding
		{
			get
			{
				return Encoding.UTF8;
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00021479 File Offset: 0x0001F679
		public void SetDone()
		{
			this.data.SetDone();
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00021486 File Offset: 0x0001F686
		public override void Write(char value)
		{
			this.data.PushString(value.ToString());
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0002149A File Offset: 0x0001F69A
		public override void Write(string value)
		{
			this.data.PushString(value);
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x000214A8 File Offset: 0x0001F6A8
		public override void Write(char[] buffer, int index, int count)
		{
			this.data.PushString(new string(buffer, index, count));
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x000214BD File Offset: 0x0001F6BD
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.data.SetDone();
		}

		// Token: 0x040002A2 RID: 674
		private InternalReader data = new InternalReader();
	}
}
