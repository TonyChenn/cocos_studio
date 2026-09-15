using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace Modules.Communal.CocosAdapter
{
	[Extension(Type = typeof(IUserData))]
	internal class CocosRecentOperation : IUserData
	{
		[ItemProperty("LastPublishType/Value")]
		public EnumPublishType LastPublishType { get; set; }

		[ItemProperty("IsLastPublish/Value")]
		public bool IsLastPublish { get; set; }

		[ItemProperty("LastRunType/Value")]
		public EnumPlatform LastRunType { get; set; }

		public CocosRecentOperation()
		{
			this.IsLastPublish = true;
			this.LastPublishType = EnumPublishType.Resource;
			if (MonoDevelop.Core.Platform.IsWindows)
			{
				this.LastRunType = EnumPlatform.Windows;
				return;
			}
			this.LastRunType = EnumPlatform.Mac;
		}

		public const string userDataKey = "CocosRecentOperation";
	}
}
