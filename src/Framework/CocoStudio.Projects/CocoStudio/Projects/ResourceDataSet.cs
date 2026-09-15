using System;
using System.Collections.Generic;
using CocoStudio.Model;

namespace CocoStudio.Projects
{
	internal class ResourceDataSet : HashSet<ResourceData>
	{
		public ResourceDataSet(IEnumerable<ResourceData> source) : base(source, new ResourceDataSet.ResourceDataEquality())
		{
		}

		private class ResourceDataEquality : EqualityComparer<ResourceData>
		{
			public override bool Equals(ResourceData x, ResourceData y)
			{
				return (x == null && y == null) || ((!(x != null) || !(y == null)) && (!(x == null) || !(y != null)) && (object.ReferenceEquals(x, y) || (x.GetHashCode() == y.GetHashCode() && (x.Type == y.Type && string.Equals(x.Plist, y.Plist, StringComparison.OrdinalIgnoreCase)) && (x.Type == EnumResourceType.MarkedSubImage || x.Type == EnumResourceType.PlistSubImage || string.Equals(x.Path, y.Path, StringComparison.OrdinalIgnoreCase)))));
			}

			public override int GetHashCode(ResourceData obj)
			{
				if (obj.Type == EnumResourceType.MarkedSubImage || obj.Type == EnumResourceType.PlistSubImage)
				{
					return obj.Type.GetHashCode() | obj.Plist.GetHashCode();
				}
				return obj.GetHashCode();
			}
		}
	}
}
