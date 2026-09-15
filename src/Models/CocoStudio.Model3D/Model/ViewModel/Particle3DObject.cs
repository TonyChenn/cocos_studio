using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	[ModelExtension(true, 101, EnumModelType.ThreeDimensional)]
	[DisplayName("Display_Component_Particle3D")]
	[ControlGroup("Control_3DControl", 0)]
	public class Particle3DObject : Node3DObject
	{
		public Particle3DObject()
		{
		}

		public Particle3DObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
			this.Name = this.FileData.FileName.FileNameWithoutExtension;
			this.GetInnerWidget().InitParticle3DSystem(resourceFile.FullPath);
		}

		public Particle3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSParticle3D();
		}

		private CSParticle3D GetInnerObject()
		{
			return this.innerNode as CSParticle3D;
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			Particle3DObject particle3DObject = cObject as Particle3DObject;
			if (particle3DObject == null)
			{
				return;
			}
			particle3DObject.FileData = this.FileData;
		}

		internal override void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			base.AncestorObjectChanged(sourceObj, action);
			if (action != NotifyCollectionChangedAction.Remove)
			{
				CSParticle3D csparticle3D = this.innerNode as CSParticle3D;
				if (csparticle3D != null)
				{
					csparticle3D.StartParticleIfPossible();
				}
			}
		}

		[Browsable(false)]
		[Category("Group_Routine")]
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
		[Category("Group_Routine")]
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

		private CSParticle3D GetInnerWidget()
		{
			return (CSParticle3D)this.innerNode;
		}

		[PropertyOrder(109)]
		[DefaultValue(null)]
		[Description("Description_File")]
		[DisplayName("Display_Particle_File")]
		[Browsable(true)]
		[Category("Group_Feature")]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"pu"
		})]
		[Editor(typeof(ResourceFileBaseEditor), typeof(ResourceFileBaseEditor))]
		public ResourceFile FileData
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
				PuFile defaultFile = new PuFile(Particle3DObjectData.DefaultFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
				this.GetInnerWidget().InitParticle3DSystem(resourceFile.FullPath);
				string taskName = base.GetType().Name + "FileData";
				using (CompositeTask.Run(taskName, null))
				{
					this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
				}
			}
		}

		protected override void RefreshBaseProperties()
		{
			CSParticle3D innerObject = this.GetInnerObject();
			if (innerObject.isWorldPosition())
			{
				innerObject.stopParticle();
				innerObject.startParticle();
			}
		}

		private ResourceFile file;
	}
}
