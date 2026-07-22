using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000008 RID: 8
	[DataModelExtension(typeof(UserCameraObject))]
	public class UserCameraObjectData : Node3DObjectData
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00002607 File Offset: 0x00000807
		// (set) Token: 0x06000055 RID: 85 RVA: 0x0000260F File Offset: 0x0000080F
		[JsonProperty]
		[ItemProperty(DefaultValue = 60)]
		public float Fov { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00002618 File Offset: 0x00000818
		// (set) Token: 0x06000057 RID: 87 RVA: 0x00002620 File Offset: 0x00000820
		[JsonProperty]
		[ItemProperty]
		public SizeF ViewSize { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00002629 File Offset: 0x00000829
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00002631 File Offset: 0x00000831
		[JsonProperty]
		[ItemProperty]
		public PointF ClipPlane { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0000263A File Offset: 0x0000083A
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00002642 File Offset: 0x00000842
		[ItemProperty]
		[JsonProperty]
		public CameraFlag UserCameraFlagMode { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600005C RID: 92 RVA: 0x0000264B File Offset: 0x0000084B
		// (set) Token: 0x0600005D RID: 93 RVA: 0x00002653 File Offset: 0x00000853
		[ItemProperty]
		[JsonProperty]
		public uint CameraFlagData { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600005E RID: 94 RVA: 0x0000265C File Offset: 0x0000085C
		// (set) Token: 0x0600005F RID: 95 RVA: 0x00002664 File Offset: 0x00000864
		[JsonProperty]
		[ItemProperty]
		public bool SkyBoxEnabled { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000060 RID: 96 RVA: 0x0000266D File Offset: 0x0000086D
		// (set) Token: 0x06000061 RID: 97 RVA: 0x00002675 File Offset: 0x00000875
		[JsonProperty]
		[ItemProperty]
		public bool SkyBoxValid { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000062 RID: 98 RVA: 0x0000267E File Offset: 0x0000087E
		// (set) Token: 0x06000063 RID: 99 RVA: 0x00002686 File Offset: 0x00000886
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

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00002695 File Offset: 0x00000895
		// (set) Token: 0x06000065 RID: 101 RVA: 0x0000269D File Offset: 0x0000089D
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

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000066 RID: 102 RVA: 0x000026AC File Offset: 0x000008AC
		// (set) Token: 0x06000067 RID: 103 RVA: 0x000026B4 File Offset: 0x000008B4
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

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000026C3 File Offset: 0x000008C3
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000026CB File Offset: 0x000008CB
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

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000026DA File Offset: 0x000008DA
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000026E2 File Offset: 0x000008E2
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

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000026F1 File Offset: 0x000008F1
		// (set) Token: 0x0600006D RID: 109 RVA: 0x000026F9 File Offset: 0x000008F9
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

		// Token: 0x0600006E RID: 110 RVA: 0x00002708 File Offset: 0x00000908
		public UserCameraObjectData()
		{
			this.UserCameraFlagMode = CameraFlag.USER1;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002718 File Offset: 0x00000918
		protected override void OnDataInitialize(VisualObject vObject)
		{
			UserCameraObject userCameraObject = vObject as UserCameraObject;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000272E File Offset: 0x0000092E
		private ResourceItemData CheckSkyBoxImageData(ResourceItemData value)
		{
			if (value == null || value == ResourceItemData.DefaultMarker)
			{
				value = UserCameraObjectData.DefaultFile;
			}
			return value;
		}

		// Token: 0x04000027 RID: 39
		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/skybox.png");

		// Token: 0x04000028 RID: 40
		private ResourceItemData _left;

		// Token: 0x04000029 RID: 41
		private ResourceItemData _right;

		// Token: 0x0400002A RID: 42
		private ResourceItemData _up;

		// Token: 0x0400002B RID: 43
		private ResourceItemData _down;

		// Token: 0x0400002C RID: 44
		private ResourceItemData _forward;

		// Token: 0x0400002D RID: 45
		private ResourceItemData _back;
	}
}
