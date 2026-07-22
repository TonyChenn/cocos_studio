using System;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000002 RID: 2
	[DataModelExtension(typeof(GameNode3DObject))]
	public class GameNode3DObjectData : GameNodeObjectData
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002053 File Offset: 0x00000253
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

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002055 File Offset: 0x00000255
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000205D File Offset: 0x0000025D
		[JsonProperty]
		[ItemProperty]
		public bool SkyBoxEnabled { get; set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002066 File Offset: 0x00000266
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000206E File Offset: 0x0000026E
		[ItemProperty]
		[JsonProperty]
		public bool SkyBoxValid { get; set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002077 File Offset: 0x00000277
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000207F File Offset: 0x0000027F
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

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x0000208E File Offset: 0x0000028E
		// (set) Token: 0x0600000A RID: 10 RVA: 0x00002096 File Offset: 0x00000296
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020A5 File Offset: 0x000002A5
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000020AD File Offset: 0x000002AD
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

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020BC File Offset: 0x000002BC
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000020C4 File Offset: 0x000002C4
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000020D3 File Offset: 0x000002D3
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000020DB File Offset: 0x000002DB
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

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000020EA File Offset: 0x000002EA
		// (set) Token: 0x06000012 RID: 18 RVA: 0x000020F2 File Offset: 0x000002F2
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002101 File Offset: 0x00000301
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002108 File Offset: 0x00000308
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

		// Token: 0x06000015 RID: 21 RVA: 0x0000210A File Offset: 0x0000030A
		public GameNode3DObjectData()
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002114 File Offset: 0x00000314
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

		// Token: 0x06000017 RID: 23 RVA: 0x000021E7 File Offset: 0x000003E7
		private ResourceItemData CheckSkyBoxImageData(ResourceItemData value)
		{
			if (value == null || value == ResourceItemData.DefaultMarker)
			{
				value = UserCameraObjectData.DefaultFile;
			}
			return value;
		}

		// Token: 0x04000001 RID: 1
		private ResourceItemData _left;

		// Token: 0x04000002 RID: 2
		private ResourceItemData _right;

		// Token: 0x04000003 RID: 3
		private ResourceItemData _up;

		// Token: 0x04000004 RID: 4
		private ResourceItemData _down;

		// Token: 0x04000005 RID: 5
		private ResourceItemData _forward;

		// Token: 0x04000006 RID: 6
		private ResourceItemData _back;
	}
}
