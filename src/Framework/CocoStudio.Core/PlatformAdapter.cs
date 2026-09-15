using System;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Desktop;
using MonoDevelop.Ide.Fonts;
using Xwt;

namespace CocoStudio.Core
{
	public static class PlatformAdapter
	{
		public static PlatformService PlatformService
		{
			get
			{
				if (PlatformAdapter.platformService == null)
				{
					throw new InvalidOperationException("Not initialized");
				}
				return PlatformAdapter.platformService;
			}
		}

		public static void Initialize()
		{
			try
			{
				if (PlatformAdapter.platformService == null)
				{
					Application.Initialize(ToolkitType.Gtk);
					Toolkit currentEngine = Toolkit.CurrentEngine;
					ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes("/CocoStudio/Core/PlatformService");
					foreach (object obj in extensionNodes)
					{
						TypeExtensionNode typeExtensionNode = obj as TypeExtensionNode;
						if (typeExtensionNode != null)
						{
							PlatformAdapter.platformService = (typeExtensionNode.CreateInstance() as PlatformService);
						}
					}
					DesktopService.PlatformService = PlatformAdapter.platformService;
					GC.KeepAlive(PlatformAdapter.NativeToolkit);
					FontService.Initialize();
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Initialize PlatformService failed.", exception);
			}
		}

		public static Toolkit NativeToolkit
		{
			get
			{
				if (PlatformAdapter.nativeToolkit == null)
				{
					PlatformAdapter.nativeToolkit = PlatformAdapter.platformService.LoadNativeToolkit();
				}
				return PlatformAdapter.nativeToolkit;
			}
		}

		private static PlatformService platformService;

		private static Toolkit nativeToolkit;
	}
}
