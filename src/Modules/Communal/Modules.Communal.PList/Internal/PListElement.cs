using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Modules.Communal.PList.Internal
{
	public abstract class PListElement<T> : IPListElement, IXmlSerializable, IEquatable<IPListElement>
	{
		public abstract string Tag { get; }

		public abstract byte TypeCode { get; }

		public virtual bool IsBinaryUnique
		{
			get
			{
				return true;
			}
		}

		public abstract T Value { get; set; }

		public virtual XmlSchema GetSchema()
		{
			return null;
		}

		public virtual void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement();
			this.Parse(reader.ReadString());
			reader.ReadEndElement();
		}

		public virtual void WriteXml(XmlWriter writer)
		{
			writer.WriteStartElement(this.Tag);
			writer.WriteValue(this.ToXmlString());
			writer.WriteEndElement();
		}

		protected abstract void Parse(string value);

		protected abstract string ToXmlString();

		public static implicit operator T(PListElement<T> element)
		{
			return element.Value;
		}

		public override string ToString()
		{
			return string.Format("{0}: {1}", this.Tag, this.Value);
		}

		public virtual int GetPListElementCount()
		{
			return 1;
		}

		public abstract int GetPListElementLength();

		public abstract void ReadBinary(PListBinaryReader reader);

		public abstract void WriteBinary(PListBinaryWriter writer);

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

		public override bool Equals(object obj)
		{
			return obj is IPListElement && this.Equals((IPListElement)obj);
		}

		public override int GetHashCode()
		{
			T value = this.Value;
			return value.GetHashCode();
		}
	}
}
