using System;
using System.Diagnostics;

namespace Modules.Communal.CocoaChina
{
	public class Redirect
	{
		public static void RedirectCocos(string access_token)
		{
			Process.Start(Redirect.RedirectCocosUrl(access_token));
		}

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
