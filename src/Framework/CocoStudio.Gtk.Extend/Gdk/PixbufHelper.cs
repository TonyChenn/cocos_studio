using System;
using System.IO;

namespace Gdk
{
	// Token: 0x02000067 RID: 103
	public static class PixbufHelper
	{
		// Token: 0x0600023B RID: 571 RVA: 0x00009D30 File Offset: 0x00007F30
		public static Pixbuf Load(string filePath)
		{
			Pixbuf result;
			try
			{
				if (!File.Exists(filePath))
				{
					result = null;
				}
				else
				{
					using (FileStream fileStream = File.OpenRead(filePath))
					{
						result = new Pixbuf(fileStream);
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009D94 File Offset: 0x00007F94
		public static Pixbuf Load(string filePath, out string format)
		{
			format = string.Empty;
			Pixbuf result;
			try
			{
				if (!File.Exists(filePath))
				{
					result = null;
				}
				else
				{
					using (FileStream fileStream = File.OpenRead(filePath))
					{
						PixbufLoader pixbufLoader = new PixbufLoader(fileStream);
						Pixbuf pixbuf = pixbufLoader.Pixbuf;
						format = pixbufLoader.Format.Name;
						pixbufLoader.Dispose();
						result = pixbuf;
					}
				}
			}
			catch (Exception ex)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00009E24 File Offset: 0x00008024
		public static bool Save(Pixbuf buf, string filePath, string type)
		{
			bool result = false;
			try
			{
				byte[] array = buf.SaveToBuffer(type);
				using (FileStream fileStream = File.OpenWrite(filePath))
				{
					fileStream.Write(array, 0, array.Length);
					fileStream.Close();
					result = true;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
	}
}
