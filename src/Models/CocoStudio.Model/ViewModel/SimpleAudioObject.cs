using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F5 RID: 245
	[EngineClassName("ComAudio")]
	[ControlGroup("Control_BaseObject", 0)]
	[DisplayName("Display_Component_Audio")]
	[ModelExtension(true, 12)]
	public class SimpleAudioObject : NodeObject, IPlayControl
	{
		// Token: 0x0600089A RID: 2202 RVA: 0x00022B9C File Offset: 0x00020D9C
		private CSSimpleAudio GetInnerWidget()
		{
			return (CSSimpleAudio)this.innerNode;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00022BB9 File Offset: 0x00020DB9
		public SimpleAudioObject()
		{
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00022BCB File Offset: 0x00020DCB
		public SimpleAudioObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00022BDE File Offset: 0x00020DDE
		public SimpleAudioObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00022BF1 File Offset: 0x00020DF1
		protected override void CreateCSObject()
		{
			this.innerNode = new CSSimpleAudio();
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00022BFF File Offset: 0x00020DFF
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon("SimpleAudio.png");
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00022C18 File Offset: 0x00020E18
		protected internal override string GetNamePrefix()
		{
			return "Audio_";
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x00022C2F File Offset: 0x00020E2F
		public override void InitOperation()
		{
			this.OperationFlag = OperationMask.MoveFlag;
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00022C3C File Offset: 0x00020E3C
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00022C54 File Offset: 0x00020E54
		[Browsable(false)]
		public override ScaleValue Scale
		{
			get
			{
				return base.Scale;
			}
			set
			{
				base.Scale = value;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x00022C60 File Offset: 0x00020E60
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x00022C78 File Offset: 0x00020E78
		[Browsable(false)]
		public override float Rotation
		{
			get
			{
				return base.Rotation;
			}
			set
			{
				base.Rotation = value;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00022C84 File Offset: 0x00020E84
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x00022C9C File Offset: 0x00020E9C
		[Browsable(false)]
		public override ScaleValue RotationSkew
		{
			get
			{
				return base.RotationSkew;
			}
			set
			{
				base.RotationSkew = value;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00022CA8 File Offset: 0x00020EA8
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x00022CC0 File Offset: 0x00020EC0
		[Browsable(false)]
		public override int Alpha
		{
			get
			{
				return base.Alpha;
			}
			set
			{
				base.Alpha = value;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00022CCC File Offset: 0x00020ECC
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x00022CE4 File Offset: 0x00020EE4
		[Browsable(false)]
		public override Color CColor
		{
			get
			{
				return base.CColor;
			}
			set
			{
				base.CColor = value;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x00022CF0 File Offset: 0x00020EF0
		// (set) Token: 0x060008AD RID: 2221 RVA: 0x00022D08 File Offset: 0x00020F08
		[Browsable(false)]
		public override SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				base.Size = value;
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x00022D14 File Offset: 0x00020F14
		// (set) Token: 0x060008AF RID: 2223 RVA: 0x00022D2C File Offset: 0x00020F2C
		[Browsable(false)]
		public override bool VisibleForFrame
		{
			get
			{
				return base.VisibleForFrame;
			}
			set
			{
				base.VisibleForFrame = value;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x00022D38 File Offset: 0x00020F38
		// (set) Token: 0x060008B1 RID: 2225 RVA: 0x00022D50 File Offset: 0x00020F50
		[Browsable(false)]
		public override ScaleValue AnchorPoint
		{
			get
			{
				return base.AnchorPoint;
			}
			set
			{
				base.AnchorPoint = value;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060008B2 RID: 2226 RVA: 0x00022D5C File Offset: 0x00020F5C
		// (set) Token: 0x060008B3 RID: 2227 RVA: 0x00022D74 File Offset: 0x00020F74
		[Browsable(false)]
		public override HorizontalBerthEdge HorizontalEdge
		{
			get
			{
				return base.HorizontalEdge;
			}
			set
			{
				base.HorizontalEdge = value;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00022D80 File Offset: 0x00020F80
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x00022DA0 File Offset: 0x00020FA0
		[DefaultValue(1)]
		public virtual float Volume
		{
			get
			{
				return this.GetInnerWidget().GetVolume();
			}
			set
			{
				if (value >= 0f && value <= 1f)
				{
					this.GetInnerWidget().SetVolume(value);
					this.RaisePropertyChanged<float>(() => this.Volume);
				}
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00022E14 File Offset: 0x00021014
		// (set) Token: 0x060008B7 RID: 2231 RVA: 0x00022E34 File Offset: 0x00021034
		[DisplayName("Display_Loop")]
		[PropertyOrder(108)]
		[Browsable(true)]
		[UndoProperty]
		[DefaultValue(false)]
		[Category("Group_Feature")]
		public virtual bool Loop
		{
			get
			{
				return this.GetInnerWidget().GetIsLoop();
			}
			set
			{
				this.GetInnerWidget().SetIsLoop(value);
				this.IsPlaying = false;
				this.RaisePropertyChanged<bool>(() => this.Loop);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00022E94 File Offset: 0x00021094
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x00022EDC File Offset: 0x000210DC
		[PropertyOrder(107)]
		[Browsable(true)]
		[UndoProperty]
		[IgnoreResize]
		[ResourceFilter(new string[]
		{
			"wav",
			"mp3"
		})]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
		[DefaultValue(null)]
		[Description("Description_File")]
		[DisplayName("Display_File")]
		[Category("Group_Feature")]
		public ResourceFile FileData
		{
			get
			{
				if (this.file == null)
				{
					this.file = (Services.ProjectOperations.FindResourceItem(this.GetInnerWidget().GetFileData()) as ResourceFile);
				}
				return this.file;
			}
			set
			{
				if (value == null || value.DataError != null)
				{
					this.IsPlaying = false;
				}
				if (this.file != value)
				{
					this.file = value;
					ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, ResourceFile.Empty, true);
					this.GetInnerWidget().SetFileData(resourceFile.GetResourceData());
					this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
				}
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00022F80 File Offset: 0x00021180
		[DisplayName("Display_AudioPlay")]
		[Editor(typeof(PlayControlEditor), typeof(PlayControlEditor))]
		[Category("Group_Feature")]
		[Browsable(true)]
		public SimpleAudioObject Instance
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00022F93 File Offset: 0x00021193
		internal override void BeforeRemoved()
		{
			base.BeforeRemoved();
			this.IsPlaying = false;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00022FA8 File Offset: 0x000211A8
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			SimpleAudioObject simpleAudioObject = cObject as SimpleAudioObject;
			if (simpleAudioObject != null)
			{
				simpleAudioObject.FileData = this.FileData;
				simpleAudioObject.Loop = this.Loop;
			}
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00022FEC File Offset: 0x000211EC
		public bool HasData()
		{
			return this.FileData != null;
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x0002306C File Offset: 0x0002126C
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x0002300C File Offset: 0x0002120C
		public bool IsPlaying
		{
			get
			{
				return false;
			}
			set
			{
				if (this.file != null)
				{
					if (value)
					{
						this.GetInnerWidget().SetFileData(this.file.GetResourceData());
						this.GetInnerWidget().Start();
					}
					else
					{
						this.GetInnerWidget().Stop();
					}
				}
			}
		}

		// Token: 0x0400033B RID: 827
		private ResourceFile file = null;
	}
}
