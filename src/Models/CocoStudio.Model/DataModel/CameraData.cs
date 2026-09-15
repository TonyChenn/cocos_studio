using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.DataModel
{
	[Extension(Type = typeof(IUserData))]
	public class CameraData : IUserData
	{
		[ItemProperty(DefaultValue = null)]
		public Point3F Position { get; set; }

		[ItemProperty(DefaultValue = null)]
		public Point3F Rotation { get; set; }

		public CameraData()
		{
		}

		public CameraData(Point3F position, Point3F rotation)
		{
			this.Position = position;
			this.Rotation = rotation;
		}

		public const string CameraDataKey = "CameraData";
	}
}
