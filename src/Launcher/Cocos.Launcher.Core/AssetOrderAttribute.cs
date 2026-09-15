using System;

namespace Cocos.Launcher.Core
{
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
	public sealed class AssetOrderAttribute : Attribute
	{
		public int Order { get; set; }

		public AssetOrderAttribute(int order)
		{
			this.Order = order;
		}
	}
}
