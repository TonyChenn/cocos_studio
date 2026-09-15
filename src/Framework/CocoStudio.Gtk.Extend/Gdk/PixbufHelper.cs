using System;
using System.IO;

namespace Gdk
{
	public static class PixbufHelper
	{
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
