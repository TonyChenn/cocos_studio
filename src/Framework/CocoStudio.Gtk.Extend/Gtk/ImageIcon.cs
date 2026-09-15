using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cocos.Launcher;
using CocoStudio.Basic;
using CocoStudio.DefaultResource;
using Gdk;
using Modules.Communal.MultiLanguage;
using Xwt.Drawing;
using Xwt.GtkBackend;

namespace Gtk
{
	public class ImageIcon
	{
		public static double ScaleFactor
		{
			get
			{
				return GtkWorkarounds.GetScaleFactor(ApplicationCurrent.MainWindow);
			}
		}

		static ImageIcon()
		{
			ImageIcon.iconFactory.AddDefault();
		}

		public static Xwt.Drawing.Image GetIcon(string resourceID)
		{
			Xwt.Drawing.Image image = null;
			try
			{
				if (ImageIcon.icons.TryGetValue(resourceID, out image))
				{
					return image;
				}
				Assembly callingAssembly = Assembly.GetCallingAssembly();
				Stream stream;
				Assembly assembly;
				if (!ImageIcon.TrySearchResourceStream(resourceID, callingAssembly, out stream, out assembly))
				{
					return null;
				}
				image = Xwt.Drawing.Image.FromResource(assembly, resourceID);
				if (image != null)
				{
					ImageIcon.icons[resourceID] = image;
				}
			}
			catch (Exception arg)
			{
				LogConfig.Logger.Error("加载图标错误" + arg);
			}
			return image;
		}

		public static Xwt.Drawing.Image GetCustomControlIcon(string resourceID)
		{
			Xwt.Drawing.Image icon = ImageIcon.GetIcon(resourceID);
			if (icon == null)
			{
				icon = ImageIcon.GetIcon("CocoStudio.DefaultResource.ComponentResource.Custom.png");
			}
			return icon;
		}

		public static Xwt.Drawing.Image GetIconFromFile(string filePath)
		{
			string extension = Path.GetExtension(filePath);
			Xwt.Drawing.Image result;
			if (!File.Exists(filePath))
			{
				result = null;
			}
			else
			{
				try
				{
					Xwt.Drawing.Image image = null;
					using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read))
					{
						image = Xwt.Drawing.Image.FromStream(fileStream);
					}
					List<Xwt.Drawing.Image> list = new List<Xwt.Drawing.Image>();
					string resource2xID = ImageIcon.GetResource2xID(filePath);
					if (File.Exists(resource2xID))
					{
						using (FileStream fileStream = File.Open(resource2xID, FileMode.Open, FileAccess.Read))
						{
							Xwt.Drawing.Image item = Xwt.Drawing.Image.FromStream(fileStream);
							list.Add(item);
						}
					}
					if (list.Count > 0)
					{
						list.Insert(0, image);
						image = Xwt.Drawing.Image.CreateMultiResolutionImage(list);
					}
					result = image;
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Debug(LanguageInfo.MessageBox_Content170, exception);
					result = null;
				}
			}
			return result;
		}

		public static Pixbuf GetPixbuf(string resourceID)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			IconSet iconSet = IconFactory.LookupDefault(resourceID);
			if (iconSet == null)
			{
				iconSet = new IconSet();
				ImageIcon.LoadIcon(iconSet, resourceID, callingAssembly);
				ImageIcon.iconFactory.Add(resourceID, iconSet);
			}
			return iconSet.RenderIcon(Widget.DefaultStyle, TextDirection.Ltr, StateType.Normal, IconSize.Button, null, null, ImageIcon.ScaleFactor);
		}

		private static void LoadIcon(IconSet iconSet, string resourceID, Assembly callingAssembly)
		{
			Pixbuf pixbuf = ImageIcon.LoadResource(resourceID, callingAssembly);
			Pixbuf pixbuf2 = ImageIcon.LoadResource2x(resourceID, callingAssembly);
			IconSource iconSource = new IconSource();
			ImageIcon.ConfigIconSource(pixbuf, iconSource);
			if (pixbuf2 != null)
			{
				GtkWorkarounds.SetSourceScale(iconSource, 1.0);
				GtkWorkarounds.SetSourceScaleWildcarded(iconSource, false);
				IconSource iconSource2 = new IconSource();
				ImageIcon.ConfigIconSource(pixbuf2, iconSource2);
				GtkWorkarounds.SetSourceScale(iconSource2, ImageIcon.ScaleFactor);
				GtkWorkarounds.SetSourceScaleWildcarded(iconSource2, false);
				iconSet.AddSource(iconSource2);
			}
			else
			{
				iconSet.AddSource(iconSource);
			}
		}

		private static void ConfigIconSource(Pixbuf pixbuf, IconSource iconSource)
		{
			iconSource.Pixbuf = pixbuf;
			iconSource.Size = IconSize.Button;
			iconSource.SizeWildcarded = false;
		}

		private static Pixbuf LoadResource2x(string resourceID, Assembly callingAssembly)
		{
			string resource2xID = ImageIcon.GetResource2xID(resourceID);
			return ImageIcon.LoadResource(resource2xID, callingAssembly);
		}

		private static Pixbuf LoadResource(string resourceID, Assembly callingAssembly)
		{
			Stream stream;
			Assembly assembly;
			Pixbuf result;
			if (!ImageIcon.TrySearchResourceStream(resourceID, callingAssembly, out stream, out assembly))
			{
				result = null;
			}
			else
			{
				byte[] buffer;
				using (stream)
				{
					if (stream == null || stream.Length < 0L)
					{
						return null;
					}
					buffer = new byte[stream.Length];
					stream.Read(buffer, 0, (int)stream.Length);
				}
				Pixbuf pixbuf = new Pixbuf(buffer);
				result = pixbuf;
			}
			return result;
		}

		private static string GetResource2xID(string resourceID)
		{
			string text = Path.GetFileNameWithoutExtension(resourceID) + "@2x" + Path.GetExtension(resourceID);
			if (Path.IsPathRooted(resourceID))
			{
				text = Path.Combine(Path.GetDirectoryName(resourceID), text);
			}
			return text;
		}

		private static bool TrySearchResourceStream(string resourceID, Assembly callingAssembly, out Stream stream, out Assembly resultAssmbly)
		{
			stream = callingAssembly.GetManifestResourceStream(resourceID);
			bool result;
			if (stream != null)
			{
				resultAssmbly = callingAssembly;
				result = true;
			}
			else
			{
				stream = CocoStudio.DefaultResource.Resources.GetResourceStream(resourceID);
				if (stream != null)
				{
					resultAssmbly = typeof(CocoStudio.DefaultResource.Resources).Assembly;
					result = true;
				}
				else
				{
					stream = Cocos.Launcher.Resources.GetResourceStream(resourceID);
					if (stream != null)
					{
						resultAssmbly = typeof(Cocos.Launcher.Resources).Assembly;
						result = true;
					}
					else
					{
						resultAssmbly = null;
						result = false;
					}
				}
			}
			return result;
		}

		private static Stream GetResourceStreamInAssembly(string resourceID, Assembly assembly)
		{
			return assembly.GetManifestResourceStream(resourceID);
		}

		private const string hightDPIResourceName = "@2x";

		private const IconSize defaultIconSize = IconSize.Button;

		private static IconFactory iconFactory = new IconFactory();

		private static Dictionary<string, Xwt.Drawing.Image> icons = new Dictionary<string, Xwt.Drawing.Image>();
	}
}
