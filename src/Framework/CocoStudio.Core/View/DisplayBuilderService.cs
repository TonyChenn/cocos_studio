using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	// Token: 0x02000038 RID: 56
	public static class DisplayBuilderService
	{
		// Token: 0x06000210 RID: 528 RVA: 0x00009CE4 File Offset: 0x00007EE4
		internal static IEnumerable<T> GetBuilder<T>()
		{
			IEnumerable<T> result;
			try
			{
				T[] extensionObjects = AddinManager.GetExtensionObjects<T>("CocoStudio/Ide/DisplayBuilder", true);
				result = extensionObjects;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Info("Loaded displayBuilder failed.", exception);
				result = null;
			}
			return result;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00009FBC File Offset: 0x000081BC
		internal static IEnumerable<IDisplayBuilder> GetDisplayBuilders(FilePath filePath, string mimeType, CocosItem ownerProject)
		{
			IEnumerable<IDisplayBuilder> builders = DisplayBuilderService.GetBuilder<IDisplayBuilder>();
			if (builders == null)
			{
				yield return null;
			}
			else
			{
				foreach (IDisplayBuilder b in builders)
				{
					if (b != null && b.CanHandle(filePath, mimeType, ownerProject))
					{
						yield return b;
					}
				}
			}
			yield break;
		}
	}
}
