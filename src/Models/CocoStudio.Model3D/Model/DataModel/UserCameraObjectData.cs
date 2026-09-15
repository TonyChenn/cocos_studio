using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(UserCameraObject))]
	public class UserCameraObjectData : Node3DObjectData
	{
		[JsonProperty]
		[ItemProperty(DefaultValue = 60)]
		public float Fov { get; set; }

		[JsonProperty]
		[ItemProperty]
		public SizeF ViewSize { get; set; }

		[JsonProperty]
		[ItemProperty]
		public PointF ClipPlane { get; set; }

		[ItemProperty]
		[JsonProperty]
		public CameraFlag UserCameraFlagMode { get; set; }

		[ItemProperty]
		[JsonProperty]
		public uint CameraFlagData { get; set; }

		[JsonProperty]
		[ItemProperty]
		public bool SkyBoxEnabled { get; set; }

		[JsonProperty]
		[ItemProperty]
		public bool SkyBoxValid { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData LeftImage
		{
			get
			{
				return this._left;
			}
			set
			{
				this._left = this.CheckSkyBoxImageData(value);
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData RightImage
		{
			get
			{
				return this._right;
			}
			set
			{
				this._right = this.CheckSkyBoxImageData(value);
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData UpImage
		{
			get
			{
				return this._up;
			}
			set
			{
				this._up = this.CheckSkyBoxImageData(value);
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData DownImage
		{
			get
			{
				return this._down;
			}
			set
			{
				this._down = this.CheckSkyBoxImageData(value);
			}
		}

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData ForwardImage
		{
			get
			{
				return this._forward;
			}
			set
			{
				this._forward = this.CheckSkyBoxImageData(value);
			}
		}

		[JsonProperty]
		[ItemProperty]
		public ResourceItemData BackImage
		{
			get
			{
				return this._back;
			}
			set
			{
				this._back = this.CheckSkyBoxImageData(value);
			}
		}

		public UserCameraObjectData()
		{
			this.UserCameraFlagMode = CameraFlag.USER1;
		}

		protected override void OnDataInitialize(VisualObject vObject)
		{
			UserCameraObject userCameraObject = vObject as UserCameraObject;
		}

		private ResourceItemData CheckSkyBoxImageData(ResourceItemData value)
		{
			if (value == null || value == ResourceItemData.DefaultMarker)
			{
				value = UserCameraObjectData.DefaultFile;
			}
			return value;
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/skybox.png");

		private ResourceItemData _left;

		private ResourceItemData _right;

		private ResourceItemData _up;

		private ResourceItemData _down;

		private ResourceItemData _forward;

		private ResourceItemData _back;
	}
}
