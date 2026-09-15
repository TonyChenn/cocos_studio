using System;
using System.ComponentModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using MonoDevelop.Core.Serialization;
using Newtonsoft.Json;

namespace CocoStudio.Model.DataModel
{
	[DataModelExtension(typeof(Slice3DObject))]
	internal class Slice3DObjectData : Node3DObjectData
	{
		[ItemProperty(DefaultValue = BillBoardMode.SLICE_BILLBOARD_NONE)]
		[DefaultValue(BillBoardMode.SLICE_BILLBOARD_NONE)]
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
		public BillBoardMode SliceBillBoardMode { get; set; }

		[ItemProperty]
		[JsonProperty]
		public ResourceItemData FileData
		{
			get
			{
				return this.fileData;
			}
			set
			{
				this.fileData = value;
				if (this.fileData == null)
				{
					this.fileData = Slice3DObjectData.DefaultFile;
					base.Size = Slice3DObjectData.defaultSliceSize;
				}
			}
		}

		[ItemProperty]
		[JsonProperty]
		public SizeF SliceSize { get; set; }

		[JsonProperty]
		[ItemProperty(DefaultValue = false)]
		public bool AnimationSwitch { get; set; }

		[JsonProperty]
		[ItemProperty]
		public PointF AnimationSpeed { get; set; }

		[ItemProperty(DefaultValue = false)]
		[JsonProperty]
		public bool TextureSwitch { get; set; }

		[JsonProperty]
		[ItemProperty]
		public PointF TextureAnimation { get; set; }

		[JsonProperty]
		[ItemProperty(DefaultValue = 0)]
		public float framerate { get; set; }

		protected override void OnDataInitialize(VisualObject vObject)
		{
			Slice3DObject slice3DObject = vObject as Slice3DObject;
			if (slice3DObject != null && this.FileData != null && slice3DObject.FileData.GetResourceData().Type != this.FileData.Type)
			{
				slice3DObject.FileData = null;
			}
		}

		internal static readonly ResourceItemData DefaultFile = new ResourceItemData(EnumResourceType.Default, "Default/Sprite.png");

		private static readonly SizeF defaultSliceSize = new SizeF(46f, 46f);

		private ResourceItemData fileData;
	}
}
