using System;
using System.Collections.Generic;
using CocoStudio.Model;

namespace CocoStudio.Projects
{
	// Token: 0x0200006D RID: 109
	internal class ResourceDataSet : HashSet<ResourceData>
	{
		// Token: 0x06000377 RID: 887 RVA: 0x0000C775 File Offset: 0x0000A975
		public ResourceDataSet(IEnumerable<ResourceData> source) : base(source, new ResourceDataSet.ResourceDataEquality())
		{
		}

		// Token: 0x0200006E RID: 110
		private class ResourceDataEquality : EqualityComparer<ResourceData>
		{
			// Token: 0x06000378 RID: 888 RVA: 0x0000C784 File Offset: 0x0000A984
			public override bool Equals(ResourceData x, ResourceData y)
			{
				return (x == null && y == null) || ((!(x != null) || !(y == null)) && (!(x == null) || !(y != null)) && (object.ReferenceEquals(x, y) || (x.GetHashCode() == y.GetHashCode() && (x.Type == y.Type && string.Equals(x.Plist, y.Plist, StringComparison.OrdinalIgnoreCase)) && (x.Type == EnumResourceType.MarkedSubImage || x.Type == EnumResourceType.PlistSubImage || string.Equals(x.Path, y.Path, StringComparison.OrdinalIgnoreCase)))));
			}

			// Token: 0x06000379 RID: 889 RVA: 0x0000C832 File Offset: 0x0000AA32
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
