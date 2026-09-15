using System;
using System.Xml.Serialization;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public interface IPListElement : IXmlSerializable
	{
		string Tag { get; }

		byte TypeCode { get; }

		int GetPListElementLength();

		int GetPListElementCount();

		bool IsBinaryUnique { get; }

		void WriteBinary(PListBinaryWriter writer);

		void ReadBinary(PListBinaryReader reader);
	}
}
