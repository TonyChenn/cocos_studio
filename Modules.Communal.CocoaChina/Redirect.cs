using System;
using System.Diagnostics;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x0200000A RID: 10
	public class Redirect
	{
		// Token: 0x0600004A RID: 74 RVA: 0x00003010 File Offset: 0x00001210
		public static void RedirectCocos(string access_token)
		{
			Process.Start(Redirect.RedirectCocosUrl(access_token));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003020 File Offset: 0x00001220
		private static string RedirectCocosUrl(string access_token)
		{
			User user = new User
			{
				Access_token = access_token
			};
			return CocosConsts.CocosRedirectUrl + user.CocosRedirectData;
		}
	}
}
