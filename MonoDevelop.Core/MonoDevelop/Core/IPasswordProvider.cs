using System;

namespace MonoDevelop.Core
{
	// Token: 0x0200023A RID: 570
	public interface IPasswordProvider
	{
		// Token: 0x0600151F RID: 5407
		void AddWebPassword(Uri uri, string password);

		// Token: 0x06001520 RID: 5408
		string GetWebPassword(Uri uri);

		// Token: 0x06001521 RID: 5409
		void AddWebUserNameAndPassword(Uri url, string username, string password);

		// Token: 0x06001522 RID: 5410
		Tuple<string, string> GetWebUserNameAndPassword(Uri url);
	}
}
