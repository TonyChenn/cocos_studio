using System;
using System.Net;

namespace MonoDevelop.Core.Web
{
	/// <summary>
	/// This interface represents the basic interface that one needs to implement in order to
	/// support repository authentication. 
	/// </summary>
	// Token: 0x02000255 RID: 597
	public interface ICredentialProvider
	{
		/// <summary>
		/// Returns CredentialState state that let's the consumer know if ICredentials
		/// were discovered by the ICredentialProvider. The credentials argument is then
		/// populated with the discovered valid credentials that can be used for the given Uri.
		/// The proxy instance if passed will be used to ensure that the request goes through the proxy
		/// to ensure successful connection to the destination Uri.
		/// </summary>
		// Token: 0x060015FA RID: 5626
		ICredentials GetCredentials(Uri uri, IWebProxy proxy, CredentialType credentialType, bool retrying);
	}
}
