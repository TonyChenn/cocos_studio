using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x020000E9 RID: 233
	internal class InternalReader : TextReader
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x000210A0 File Offset: 0x0001F2A0
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			lock (this.queue)
			{
				this.queue.Clear();
				this.current = null;
				this.disposed = true;
				this.done = true;
				Monitor.PulseAll(this.queue);
			}
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002110C File Offset: 0x0001F30C
		internal void PushString(string s)
		{
			lock (this.queue)
			{
				if (!this.disposed && !string.IsNullOrEmpty(s))
				{
					this.queue.Enqueue(s);
					Monitor.PulseAll(this.queue);
				}
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00021170 File Offset: 0x0001F370
		internal void SetDone()
		{
			lock (this.queue)
			{
				this.done = true;
				Monitor.PulseAll(this.queue);
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000211BC File Offset: 0x0001F3BC
		private bool LoadCurrent(bool block)
		{
			if (this.current != null && this.idx < this.current.Length)
			{
				return true;
			}
			lock (this.queue)
			{
				while (this.queue.Count == 0 && !this.done)
				{
					if (!block)
					{
						return false;
					}
					Monitor.Wait(this.queue);
				}
				if (this.queue.Count == 0)
				{
					return false;
				}
				this.current = this.queue.Dequeue();
				this.idx = 0;
			}
			return true;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002126C File Offset: 0x0001F46C
		public override int Peek()
		{
			if (this.LoadCurrent(true))
			{
				return (int)this.current[this.idx];
			}
			return -1;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0002128A File Offset: 0x0001F48A
		public override int Read()
		{
			if (this.LoadCurrent(true))
			{
				return (int)this.current[this.idx];
			}
			return -1;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000212A8 File Offset: 0x0001F4A8
		public override int Read(char[] buffer, int index, int count)
		{
			int num = 0;
			while (count > 0 && this.LoadCurrent(num == 0))
			{
				int num2 = Math.Min(this.current.Length - this.idx, count);
				this.current.CopyTo(this.idx, buffer, index, num2);
				index += num2;
				this.idx += num2;
				count -= num2;
				num += num2;
			}
			return num;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00021314 File Offset: 0x0001F514
		public override string ReadLine()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (this.LoadCurrent(true))
			{
				for (int i = this.idx; i < this.current.Length; i++)
				{
					if (this.current[i] == '\n')
					{
						this.idx = i + 1;
						stringBuilder.Append(this.current.Substring(0, i));
						return stringBuilder.ToString();
					}
					if (this.current[i] == '\r')
					{
						this.idx = i + 1;
						stringBuilder.Append(this.current.Substring(0, i));
						if (this.LoadCurrent(true) && this.current[this.idx] == '\n')
						{
							this.idx++;
						}
						return stringBuilder.ToString();
					}
				}
				stringBuilder.Append(this.current.Substring(this.idx));
				this.current = null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x00021414 File Offset: 0x0001F614
		public override string ReadToEnd()
		{
			StringBuilder stringBuilder = new StringBuilder();
			while (this.LoadCurrent(true))
			{
				stringBuilder.Append(this.current.Substring(this.idx));
				this.current = null;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400029D RID: 669
		private Queue<string> queue = new Queue<string>();

		// Token: 0x0400029E RID: 670
		private string current;

		// Token: 0x0400029F RID: 671
		private int idx;

		// Token: 0x040002A0 RID: 672
		private bool disposed;

		// Token: 0x040002A1 RID: 673
		private bool done;
	}
}
