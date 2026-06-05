using System;
using System.Linq;
using Mono.Addins;

namespace MonoDevelop.Core
{
	// Token: 0x02000241 RID: 577
	internal static class WebCertificateService
	{
		// Token: 0x06001544 RID: 5444 RVA: 0x00056F3C File Offset: 0x0005513C
		public static bool GetIsCertificateTrusted(string uri, string certificateFingerprint)
		{
			IWebCertificateProvider webCertificateProvider = AddinManager.GetExtensionObjects<IWebCertificateProvider>("/MonoDevelop/Core/WebCertificateProvider").FirstOrDefault<IWebCertificateProvider>();
			return webCertificateProvider != null && webCertificateProvider.GetIsCertificateTrusted(uri, certificateFingerprint);
		}

		// Token: 0x04000667 RID: 1639
		private const string WebCertificateProvidersPath = "/MonoDevelop/Core/WebCertificateProvider";
	}
}
