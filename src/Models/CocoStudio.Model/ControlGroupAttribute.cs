using System;

namespace CocoStudio.Model
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class ControlGroupAttribute : Attribute
	{
		public string GroupName { get; set; }

		public int Order { get; set; }

		public ControlGroupAttribute(string groupName, int order)
		{
			this.GroupName = groupName;
			this.Order = order;
		}

		public override bool Equals(object obj)
		{
			ControlGroupAttribute controlGroupAttribute = obj as ControlGroupAttribute;
			return this.GroupName.Equals(controlGroupAttribute.GroupName);
		}

		public override int GetHashCode()
		{
			return this.GroupName.GetHashCode();
		}
	}
}
