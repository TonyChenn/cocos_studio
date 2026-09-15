using System;
using System.Collections.Generic;
using CocoStudio.Basic;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	public static class DisplayBuilderService
	{
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
