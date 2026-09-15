using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(GameNode3DObject))]
	public class GameNode3DObjectData : GameNodeObjectData
	{
		[JsonProperty]
		[ItemProperty]
		public bool UseDefaultLight
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		[JsonProperty]
		[ItemProperty]
		public bool SkyBoxEnabled { get; set; }

		[ItemProperty]
		[JsonProperty]
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

		[JsonProperty]
		[ItemProperty]
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

		[JsonProperty]
		[ItemProperty]
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

		[JsonProperty]
		[ItemProperty]
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

		[ItemProperty]
		[JsonProperty]
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

		[JsonProperty]
		[ItemProperty]
		public int SkyBoxMask
		{
			get
			{
				return 1024;
			}
			set
			{
			}
		}

		public GameNode3DObjectData()
		{
		}

		public GameNode3DObjectData(AbstractNodeObjectData sNode)
		{
			base.CanEdit = sNode.CanEdit;
			base.ActionTag = sNode.ActionTag;
			base.CallBackName = sNode.CallBackName;
			base.CallBackType = sNode.CallBackType;
			base.CustomClassName = sNode.CustomClassName;
			base.FrameEvent = sNode.FrameEvent;
			base.InnerClassName = sNode.InnerClassName;
			base.IsAutoSize = sNode.IsAutoSize;
			base.Name = sNode.Name;
			base.ScriptData = sNode.ScriptData;
			base.Size = sNode.Size;
			base.Tag = sNode.Tag;
			base.UserData = sNode.UserData;
			base.Visible = sNode.Visible;
			base.ZOrder = sNode.ZOrder;
			base.Children = sNode.Children;
		}

		private ResourceItemData CheckSkyBoxImageData(ResourceItemData value)
		{
			if (value == null || value == ResourceItemData.DefaultMarker)
			{
				value = UserCameraObjectData.DefaultFile;
			}
			return value;
		}

		private ResourceItemData _left;

		private ResourceItemData _right;

		private ResourceItemData _up;

		private ResourceItemData _down;

		private ResourceItemData _forward;

		private ResourceItemData _back;
	}
}
