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
	[ControlGroup("Control_BaseObject", 0)]
	[DisplayName("Display_Component_Particle")]
	[ModelExtension(true, 10)]
	[EngineClassName("ParticleSystemQuad")]
	public class ParticleObject : NodeObject, IPlayControl, IBlendFunc
	{
		private CSParticleSystem GetInnerWidget()
		{
			return (CSParticleSystem)this.innerNode;
		}

		public ParticleObject()
		{
		}

		public ParticleObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSParticleSystem();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon(new SizeF(50f, 50f));
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65511;
		}

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

		public void Start()
		{
			this.IsPlaying = true;
		}

		public void Stop()
		{
			this.IsPlaying = false;
		}

		public bool HasData()
		{
			return this.FileData != null;
		}

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

		private bool isPlaying = true;

		private ResourceFile file = null;
	}
}
