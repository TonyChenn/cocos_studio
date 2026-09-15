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
	[EngineClassName("ComAudio")]
	[ControlGroup("Control_BaseObject", 0)]
	[DisplayName("Display_Component_Audio")]
	[ModelExtension(true, 12)]
	public class SimpleAudioObject : NodeObject, IPlayControl
	{
		private CSSimpleAudio GetInnerWidget()
		{
			return (CSSimpleAudio)this.innerNode;
		}

		public SimpleAudioObject()
		{
		}

		public SimpleAudioObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
		}

		public SimpleAudioObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSSimpleAudio();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			base.InitIcon("SimpleAudio.png");
		}

		protected internal override string GetNamePrefix()
		{
			return "Audio_";
		}

		public override void InitOperation()
		{
			this.OperationFlag = OperationMask.MoveFlag;
		}

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

		internal override void BeforeRemoved()
		{
			base.BeforeRemoved();
			this.IsPlaying = false;
		}

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

		public bool HasData()
		{
			return this.FileData != null;
		}

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

		private ResourceFile file = null;
	}
}
