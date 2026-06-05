using System;
using System.Linq;
using Mono.Addins;

namespace MonoDevelop.Core
{
	// Token: 0x02000239 RID: 569
	public static class PasswordService
	{
		// Token: 0x0600151B RID: 5403 RVA: 0x000565C8 File Offset: 0x000547C8
		public static void AddWebPassword(Uri url, string password)
		{
			IPasswordProvider passwordProvider = AddinManager.GetExtensionObjects<IPasswordProvider>("/MonoDevelop/Core/PasswordProvider").FirstOrDefault<IPasswordProvider>();
			if (passwordProvider != null)
			{
				passwordProvider.AddWebPassword(url, password);
			}
		}

		// Token: 0x0600151C RID: 5404 RVA: 0x000565F0 File Offset: 0x000547F0
		public static void AddWebUserNameAndPassword(Uri url, string username, string password)
		{
			IPasswordProvider passwordProvider = AddinManager.GetExtensionObjects<IPasswordProvider>("/MonoDevelop/Core/PasswordProvider").FirstOrDefault<IPasswordProvider>();
			if (passwordProvider != null)
			{
				passwordProvider.AddWebUserNameAndPassword(url, username, password);
			}
		}

		// Token: 0x0600151D RID: 5405 RVA: 0x0005661C File Offset: 0x0005481C
		public static string GetWebPassword(Uri url)
		{
			IPasswordProvider passwordProvider = AddinManager.GetExtensionObjects<IPasswordProvider>("/MonoDevelop/Core/PasswordProvider").FirstOrDefault<IPasswordProvider>();
			if (passwordProvider == null)
			{
				return null;
			}
			return passwordProvider.GetWebPassword(url);
		}

		// Token: 0x0600151E RID: 5406 RVA: 0x00056648 File Offset: 0x00054848
		public static Tuple<string, string> GetWebUserNameAndPassword(Uri url)
		{
			IPasswordProvider passwordProvider = AddinManager.GetExtensionObjects<IPasswordProvider>("/MonoDevelop/Core/PasswordProvider").FirstOrDefault<IPasswordProvider>();
			if (passwordProvider == null)
			{
				return null;
			}
			return passwordProvider.GetWebUserNameAndPassword(url);
		}

		// Token: 0x04000660 RID: 1632
		private const string PasswordProvidersPath = "/MonoDevelop/Core/PasswordProvider";
	}
}
