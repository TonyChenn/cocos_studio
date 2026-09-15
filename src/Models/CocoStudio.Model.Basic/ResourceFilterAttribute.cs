using System;

namespace CocoStudio.Model
{
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class ResourceFilterAttribute : Attribute
	{
		public string[] FileFilter { get; private set; }

		public EnumResourceType[] ResourceTypeFilter { get; private set; }

		public bool DefaultFileMarker { get; set; }

		public bool CanReset { get; set; }

		public ResourceFilterAttribute(params string[] fileFilter)
		{
			this.FileFilter = fileFilter;
		}

		public ResourceFilterAttribute(bool defaultFileMarker = false, bool canReset = false, params string[] fileFilter)
		{
			this.FileFilter = fileFilter;
			this.DefaultFileMarker = defaultFileMarker;
			this.CanReset = canReset;
		}

		public ResourceFilterAttribute(EnumResourceType resouceType, params string[] fileFilter) : this(fileFilter)
		{
			this.ResourceTypeFilter = new EnumResourceType[]
			{
				resouceType
			};
		}

		public ResourceFilterAttribute(EnumResourceType[] resoureTypeFilter, params string[] fileFilter) : this(fileFilter)
		{
			this.ResourceTypeFilter = resoureTypeFilter;
		}
	}
}
