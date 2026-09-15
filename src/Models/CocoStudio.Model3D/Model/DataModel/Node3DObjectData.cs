using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(Node3DObject))]
	public class Node3DObjectData : AbstractNodeObjectData
	{
		[JsonProperty]
		[ItemProperty]
		public ColorData CColor { get; set; }

		[ItemProperty]
		[JsonProperty]
		public Point3F Position3D { get; set; }

		[ItemProperty]
		[JsonProperty]
		public Point3F Rotation3D { get; set; }

		[ItemProperty]
		[JsonProperty]
		public Point3F Scale3D { get; set; }

		[JsonProperty]
		[ItemProperty]
		public int CameraFlagMode { get; set; }

		public Node3DObjectData()
		{
			this.CameraFlagMode = 31;
			base.Alpha = 255;
			base.VisibleForFrame = true;
			this.CColor = new ColorData(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		}
	}
}
