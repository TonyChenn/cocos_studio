using System;
using System.Globalization;
using System.Linq;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListDate : PListElement<DateTime>
	{
		public override string Tag
		{
			get
			{
				return "date";
			}
		}

		public override byte TypeCode
		{
			get
			{
				return 3;
			}
		}

		public override DateTime Value { get; set; }

		public PListDate()
		{
		}

		public PListDate(DateTime value)
		{
			this.Value = value;
		}

		protected override void Parse(string value)
		{
			this.Value = DateTime.Parse(value, CultureInfo.InvariantCulture);
		}

		protected override string ToXmlString()
		{
			return this.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ffffffZ");
		}

		public override void ReadBinary(PListBinaryReader reader)
		{
			byte[] array = new byte[1 << reader.CurrentElementLength];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			double value;
			switch (reader.CurrentElementLength)
			{
			case 0:
				throw new PListFormatException("Date < 32Bit");
			case 1:
				throw new PListFormatException("Date < 32Bit");
			case 2:
				value = (double)BitConverter.ToSingle(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			case 3:
				value = BitConverter.ToDouble(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			default:
				throw new PListFormatException("Date > 64Bit");
			}
			this.Value = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value);
		}

		public override int GetPListElementLength()
		{
			return 3;
		}

		public override void WriteBinary(PListBinaryWriter writer)
		{
			DateTime d = new DateTime(2001, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			byte[] array = BitConverter.GetBytes((this.Value - d).TotalSeconds).Reverse<byte>().ToArray<byte>();
			writer.BaseStream.Write(array, 0, array.Length);
		}
	}
}
