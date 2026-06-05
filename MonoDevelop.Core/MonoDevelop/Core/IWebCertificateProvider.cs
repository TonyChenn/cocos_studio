using System;

namespace MonoDevelop.Core
{
	// Token: 0x02000242 RID: 578
	public interface IWebCertificateProvider
	{
		// Token: 0x06001545 RID: 5445
		bool GetIsCertificateTrusted(string uri, string certificateFingerprint);
	}
}
