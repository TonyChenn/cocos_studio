using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Modules.Communal.PList.Internal
{
	// Token: 0x02000007 RID: 7
	public abstract class PListElement<T> : IPListElement, IXmlSerializable, IEquatable<IPListElement>
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000035 RID: 53
		public abstract string Tag { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000036 RID: 54
		public abstract byte TypeCode { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002ED4 File Offset: 0x000010D4
		public virtual bool IsBinaryUnique
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000038 RID: 56
		// (set) Token: 0x06000039 RID: 57
		public abstract T Value { get; set; }

		// Token: 0x0600003A RID: 58 RVA: 0x00002EE8 File Offset: 0x000010E8
		public virtual XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002EFB File Offset: 0x000010FB
		public virtual void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			this.Parse(reader.ReadString());
			reader.ReadEndElement();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002F19 File Offset: 0x00001119
		public virtual void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			writer.WriteValue(this.ToXmlString());
			writer.WriteEndElement();
		}

		// Token: 0x0600003D RID: 61
		protected abstract void Parse(string value);

		// Token: 0x0600003E RID: 62
		protected abstract string ToXmlString();

		// Token: 0x0600003F RID: 63 RVA: 0x00002F40 File Offset: 0x00001140
		public static implicit operator T(PListElement<T> element)
		{
			return element.Value;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002F58 File Offset: 0x00001158
		public override string ToString()
		{
			return string.Format("{0}: {1}", this.Tag, this.Value);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002F88 File Offset: 0x00001188
		public virtual int GetPListElementCount()
		{
			return 1;
		}

		// Token: 0x06000042 RID: 66
		public abstract int GetPListElementLength();

		// Token: 0x06000043 RID: 67
		public abstract void ReadBinary(PListBinaryReader reader);

		// Token: 0x06000044 RID: 68
		public abstract void WriteBinary(PListBinaryWriter writer);

		// Token: 0x06000045 RID: 69 RVA: 0x00002F9C File Offset: 0x0000119C
		public bool Equals(IPListElement other)
		{
			bool result;
			if (other is PListElement<T>)
			{
				T value = this.Value;
				result = value.Equals(((PListElement<T>)other).Value);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002FE0 File Offset: 0x000011E0
		public override bool Equals(object obj)
		{
			return obj is IPListElement && this.Equals((IPListElement)obj);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003010 File Offset: 0x00001210
		public override int GetHashCode()
		{
			T value = this.Value;
			return value.GetHashCode();
		}
	}
}
