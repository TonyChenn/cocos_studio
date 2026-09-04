using System;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.Core;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000F3 RID: 243
	[ControlGroup("Control_BaseObject", 0)]
	[DisplayName("Display_Component_Particle")]
	[ModelExtension(true, 10)]
	[EngineClassName("ParticleSystemQuad")]
	public class ParticleObject : NodeObject, IPlayControl, IBlendFunc
	{
		// Token: 0x06000854 RID: 2132 RVA: 0x000212C0 File Offset: 0x0001F4C0
		private CSParticleSystem GetInnerWidget()
		{
			return (CSParticleSystem)this.innerNode;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x000212DD File Offset: 0x0001F4DD
		public ParticleObject()
		{
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x000212F6 File Offset: 0x0001F4F6
		public ParticleObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00021310 File Offset: 0x0001F510
		protected override void CreateCSObject()
		{
			this.innerNode = new CSParticleSystem();
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00021320 File Offset: 0x0001F520
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon(new SizeF(50f, 50f));
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002135B File Offset: 0x0001F55B
		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65511;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0002136C File Offset: 0x0001F56C
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x00021384 File Offset: 0x0001F584
		public virtual bool IsPlaying
		{
			get
			{
				return this.isPlaying;
			}
			set
			{
				this.isPlaying = value;
				if (this.isPlaying)
				{
					this.GetInnerWidget().Start();
				}
				else
				{
					this.GetInnerWidget().Stop();
				}
				this.RaisePropertyChanged<bool>(() => this.IsPlaying);
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x00021400 File Offset: 0x0001F600
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x00021418 File Offset: 0x0001F618
		[UndoProperty]
		public override ScaleValue AnchorPoint
		{
			get
			{
				return ScaleValue.Empty;
			}
			set
			{
				this.GetCSVisual().SetAnchorPoint(value);
				this.RaisePropertyChanged<ScaleValue>(() => this.AnchorPoint);
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x00021470 File Offset: 0x0001F670
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x000214B8 File Offset: 0x0001F6B8
		[DisplayName("Display_File")]
		[UndoProperty]
		[IgnoreResize]
		[ResourceFilter(new string[]
		{
			"plist"
		})]
		[PropertyOrder(109)]
		[DefaultValue(null)]
		[Description("Description_File")]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
		[Category("Group_Feature")]
		[Browsable(true)]
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
				this.file = value;
				PlistParticleFile defaultFile = new PlistParticleFile(ParticleObjectData.DefaultFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
				this.GetInnerWidget().SetFileData(resourceFile.GetResourceData());
				using (CompositeTask.Run("Particle FileData", null))
				{
					this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
					this.RaisePropertyChanged<BlendFuncValue>(() => this.BlendFunc, false);
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x000215A0 File Offset: 0x0001F7A0
		[Browsable(true)]
		[Category("Group_Feature")]
		[Editor(typeof(PlayControlEditor), typeof(PlayControlEditor))]
		[DisplayName("Display_AudioPlay")]
		public ParticleObject Instance
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x000215B4 File Offset: 0x0001F7B4
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x000215D4 File Offset: 0x0001F7D4
		[Browsable(false)]
		public override int Alpha
		{
			get
			{
				return this.GetCSVisual().GetAlpha();
			}
			set
			{
				this.GetCSVisual().SetAlpha(value);
				this.RaisePropertyChanged<int>(() => this.Alpha);
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0002162C File Offset: 0x0001F82C
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x0002164C File Offset: 0x0001F84C
		[Browsable(false)]
		public override Color CColor
		{
			get
			{
				return this.GetCSVisual().GetColor();
			}
			set
			{
				this.GetCSVisual().SetColor(value);
				this.RaisePropertyChanged<Color>(() => this.CColor);
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x000216A4 File Offset: 0x0001F8A4
		// (set) Token: 0x06000866 RID: 2150 RVA: 0x000216C4 File Offset: 0x0001F8C4
		[PropertyOrder(110)]
		[Browsable(true)]
		[FrameProperty]
		[UndoProperty]
		[DisplayName("Animation_Blend_Blend")]
		[Editor(typeof(BlendFuncEditor), typeof(BlendFuncEditor))]
		[Category("Group_Feature")]
		public BlendFuncValue BlendFunc
		{
			get
			{
				return this.GetInnerWidget().GetBlendFunc();
			}
			set
			{
				if (value != null)
				{
					this.GetInnerWidget().SetBlendFunc(value);
				}
				this.RaisePropertyChanged<BlendFuncValue>(() => this.BlendFunc);
			}
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00021722 File Offset: 0x0001F922
		public void Start()
		{
			this.IsPlaying = true;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x0002172D File Offset: 0x0001F92D
		public void Stop()
		{
			this.IsPlaying = false;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00021738 File Offset: 0x0001F938
		public bool HasData()
		{
			return this.FileData != null;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00021758 File Offset: 0x0001F958
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			ParticleObject particleObject = cObject as ParticleObject;
			if (particleObject != null)
			{
				particleObject.FileData = this.FileData;
				particleObject.IsPlaying = this.IsPlaying;
				particleObject.BlendFunc = (this.BlendFunc.Clone() as BlendFuncValue);
			}
		}

		// Token: 0x0400032C RID: 812
		private bool isPlaying = true;

		// Token: 0x0400032D RID: 813
		private ResourceFile file = null;
	}
}
