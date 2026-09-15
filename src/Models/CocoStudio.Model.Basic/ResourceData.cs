using System;
using System.ComponentModel;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model
{
	[DataInclude(typeof(EnumResourceType))]
	[JsonObject(MemberSerialization.OptIn)]
	public class ResourceData
	{
		[JsonProperty]
		[ItemProperty]
		public EnumResourceType Type { get; protected internal set; }

		[JsonProperty]
		[ItemProperty]
		public string Path { get; protected internal set; }

		[DefaultValue(null)]
		[ItemProperty(DefaultValue = null)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public string Plist { get; protected internal set; }

		protected ResourceData() : this(EnumResourceType.Normal, null, null)
		{
		}

		public ResourceData(string path) : this(EnumResourceType.Normal, path)
		{
		}

		public ResourceData(EnumResourceType type, string path) : this(type, path, string.Empty)
		{
		}

		public ResourceData(EnumResourceType type, string path, string plist)
		{
			this.Type = type;
			this.Path = ((path == null) ? string.Empty : path);
			this.Plist = ((plist == null) ? string.Empty : plist);
		}

		public override string ToString()
		{
			return this.Path;
		}

		public bool Equals(ResourceData others)
		{
			return !(others == null) && (object.ReferenceEquals(this, others) || (this.GetHashCode() == others.GetHashCode() && (this.Type == others.Type && string.Equals(this.Path, others.Path, StringComparison.OrdinalIgnoreCase) && string.Equals(this.Plist, others.Plist, StringComparison.OrdinalIgnoreCase))));
		}

		public override bool Equals(object obj)
		{
			ResourceData resourceData = obj as ResourceData;
			return resourceData != null && this.Equals(resourceData);
		}

		public override int GetHashCode()
		{
			int result;
			if (this.hashCode != 0)
			{
				result = this.hashCode;
			}
			else
			{
				this.hashCode = (this.Type.GetHashCode() | (this.Path.GetHashCode() ^ this.Plist.GetHashCode()));
				result = this.hashCode;
			}
			return result;
		}

		public static bool operator ==(ResourceData leftValue, ResourceData rightValue)
		{
			bool result;
			if (object.ReferenceEquals(leftValue, null))
			{
				result = object.ReferenceEquals(rightValue, null);
			}
			else
			{
				result = leftValue.Equals(rightValue);
			}
			return result;
		}

		public static bool operator !=(ResourceData leftValue, ResourceData rightValue)
		{
			return !(leftValue == rightValue);
		}

		protected internal int hashCode;

		public static readonly ResourceData Empty = new ResourceData(string.Empty);
	}
}
