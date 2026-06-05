using System;
using System.Xml;

namespace MonoDevelop.Core
{
	// Token: 0x02000047 RID: 71
	public interface ICustomXmlSerializer
	{
		// Token: 0x0600023F RID: 575
		void WriteTo(XmlWriter writer);

		// Token: 0x06000240 RID: 576
		ICustomXmlSerializer ReadFrom(XmlReader reader);
	}
}
