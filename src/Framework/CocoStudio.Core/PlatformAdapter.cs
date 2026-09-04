using System;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Desktop;
using MonoDevelop.Ide.Fonts;
using Xwt;

namespace CocoStudio.Core
{
	// Token: 0x02000053 RID: 83
	public static class PlatformAdapter
	{
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000EA90 File Offset: 0x0000CC90
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

		// Token: 0x06000349 RID: 841 RVA: 0x0000EAC4 File Offset: 0x0000CCC4
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

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000EBB8 File Offset: 0x0000CDB8
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

		// Token: 0x04000170 RID: 368
		private static PlatformService platformService;

		// Token: 0x04000171 RID: 369
		private static Toolkit nativeToolkit;
	}
}
