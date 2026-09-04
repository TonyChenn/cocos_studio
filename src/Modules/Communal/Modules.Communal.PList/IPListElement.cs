using System;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	// Token: 0x02000002 RID: 2
	public interface IPListElement : IXmlSerializable
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		string Tag { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		byte TypeCode { get; }

		// Token: 0x06000003 RID: 3
		int GetPListElementLength();

		// Token: 0x06000004 RID: 4
		int GetPListElementCount();

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5
		bool IsBinaryUnique { get; }

		// Token: 0x06000006 RID: 6
		void WriteBinary(PListBinaryWriter writer);

		// Token: 0x06000007 RID: 7
		void ReadBinary(PListBinaryReader reader);
	}
}
