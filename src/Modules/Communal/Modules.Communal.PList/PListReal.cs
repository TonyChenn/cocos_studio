using System;
using System.Globalization;
using System.Linq;
using Modules.Communal.PList.Internal;

namespace Modules.Communal.PList
{
	public class PListReal : PListElement<double>
	{
		public override string Tag
		{
			get
			{
				return "real";
			}
		}

		public override byte TypeCode
		{
			get
			{
				return 2;
			}
		}

		public override double Value { get; set; }

		public PListReal()
		{
		}

		public PListReal(double value)
		{
			this.Value = value;
		}

		protected override void Parse(string value)
		{
			this.Value = double.Parse(value, CultureInfo.InvariantCulture);
		}

		protected override string ToXmlString()
		{
			return this.Value.ToString(CultureInfo.InvariantCulture);
		}

		public override void ReadBinary(PListBinaryReader reader)
		{
			byte[] array = new byte[1 << reader.CurrentElementLength];
			if (reader.BaseStream.Read(array, 0, array.Length) != array.Length)
			{
				throw new PListFormatException();
			}
			switch (reader.CurrentElementLength)
			{
			case 0:
				throw new PListFormatException("Real < 32Bit");
			case 1:
				throw new PListFormatException("Real < 32Bit");
			case 2:
				this.Value = (double)BitConverter.ToSingle(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			case 3:
				this.Value = BitConverter.ToDouble(array.Reverse<byte>().ToArray<byte>(), 0);
				break;
			default:
				throw new PListFormatException("Real > 64Bit");
			}
		}

		public override int GetPListElementLength()
		{
			return 3;
		}

		public override void WriteBinary(PListBinaryWriter writer)
		{
			byte[] array = BitConverter.GetBytes(this.Value).Reverse<byte>().ToArray<byte>();
			writer.BaseStream.Write(array, 0, array.Length);
		}
	}
}
