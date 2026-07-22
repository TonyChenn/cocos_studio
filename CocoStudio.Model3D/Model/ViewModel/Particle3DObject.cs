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
	// Token: 0x02000026 RID: 38
	[ModelExtension(true, 101, EnumModelType.ThreeDimensional)]
	[DisplayName("Display_Component_Particle3D")]
	[ControlGroup("Control_3DControl", 0)]
	public class Particle3DObject : Node3DObject
	{
		// Token: 0x06000184 RID: 388 RVA: 0x000061EB File Offset: 0x000043EB
		public Particle3DObject()
		{
		}

		// Token: 0x06000185 RID: 389 RVA: 0x000061F4 File Offset: 0x000043F4
		public Particle3DObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
			this.Name = this.FileData.FileName.FileNameWithoutExtension;
			this.GetInnerWidget().InitParticle3DSystem(resourceFile.FullPath);
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00006238 File Offset: 0x00004438
		public Particle3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00006241 File Offset: 0x00004441
		protected override void CreateCSObject()
		{
			this.innerNode = new CSParticle3D();
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000624E File Offset: 0x0000444E
		private CSParticle3D GetInnerObject()
		{
			return this.innerNode as CSParticle3D;
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0000625B File Offset: 0x0000445B
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00006270 File Offset: 0x00004470
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

		// Token: 0x0600018B RID: 395 RVA: 0x0000629C File Offset: 0x0000449C
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

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000062CA File Offset: 0x000044CA
		// (set) Token: 0x0600018D RID: 397 RVA: 0x000062D2 File Offset: 0x000044D2
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

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000062DB File Offset: 0x000044DB
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000062E3 File Offset: 0x000044E3
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

		// Token: 0x06000190 RID: 400 RVA: 0x000062EC File Offset: 0x000044EC
		private CSParticle3D GetInnerWidget()
		{
			return (CSParticle3D)this.innerNode;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000191 RID: 401 RVA: 0x000062F9 File Offset: 0x000044F9
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00006304 File Offset: 0x00004504
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

		// Token: 0x06000193 RID: 403 RVA: 0x000063B8 File Offset: 0x000045B8
		protected override void RefreshBaseProperties()
		{
			CSParticle3D innerObject = this.GetInnerObject();
			if (innerObject.isWorldPosition())
			{
				innerObject.stopParticle();
				innerObject.startParticle();
			}
		}

		// Token: 0x0400008A RID: 138
		private ResourceFile file;
	}
}
