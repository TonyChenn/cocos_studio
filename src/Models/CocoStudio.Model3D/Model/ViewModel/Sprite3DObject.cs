using System;
using System.Collections.Specialized;
using System.ComponentModel;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.Editor;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200002B RID: 43
	[ControlGroup("Control_3DControl", 0)]
	[ModelExtension(true, 100, EnumModelType.ThreeDimensional)]
	[DisplayName("Display_Component_Sprite3D")]
	public class Sprite3DObject : Node3DObject
	{
		// Token: 0x060001C1 RID: 449 RVA: 0x00006E17 File Offset: 0x00005017
		public Sprite3DObject()
		{
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00006E20 File Offset: 0x00005020
		public Sprite3DObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
			this.Name = this.FileData.FileName.FileNameWithoutExtension;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00006E53 File Offset: 0x00005053
		public Sprite3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00006E5C File Offset: 0x0000505C
		protected override void CreateCSObject()
		{
			this.innerNode = new CSSprite3D();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00006E69 File Offset: 0x00005069
		private CSSprite3D GetInnerWidget()
		{
			return (CSSprite3D)this.innerNode;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00006E76 File Offset: 0x00005076
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.LightFlag = LightFlag.LIGHT0;
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00006E90 File Offset: 0x00005090
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			Sprite3DObject sprite3DObject = cObject as Sprite3DObject;
			if (sprite3DObject == null)
			{
				return;
			}
			sprite3DObject.FileData = this.FileData;
			sprite3DObject.RunAction3D = this.RunAction3D;
			sprite3DObject.LightFlag = this.LightFlag;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00006ED4 File Offset: 0x000050D4
		internal override void AncestorObjectChanged(BaseObject sourceObj, NotifyCollectionChangedAction action)
		{
			base.AncestorObjectChanged(sourceObj, action);
			if (action != NotifyCollectionChangedAction.Remove)
			{
				CSSprite3D cssprite3D = this.innerNode as CSSprite3D;
				if (cssprite3D != null)
				{
					cssprite3D.RunAnimationIfPosible();
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00006F02 File Offset: 0x00005102
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00006F0F File Offset: 0x0000510F
		public virtual bool IsFlipped
		{
			get
			{
				return this.GetInnerWidget().IsFlipped();
			}
			set
			{
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00006F11 File Offset: 0x00005111
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00006F1C File Offset: 0x0000511C
		[Browsable(true)]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"c3b",
			"c3t",
			"obj"
		})]
		[Editor(typeof(ResourceFileBaseEditor), typeof(ResourceFileBaseEditor))]
		[DefaultValue(null)]
		[Description("Description_File")]
		[DisplayName("Display_Model_File")]
		[Category("Group_Feature")]
		[PropertyOrder(109)]
		public ResourceFile FileData
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
				if (this.file == null || this.file.IsDefault)
				{
					this.file = new MeshFile(Sprite3DObjectData.DefaultFile);
				}
				if (this.RunAction3D)
				{
					this.GetInnerWidget().StopAction();
				}
				this.GetInnerWidget().SetFileData(this.file.GetResourceData());
				this.isHasAction = this.GetInnerWidget().LoadAnimation();
				if (this.isHasAction)
				{
					if (this.RunAction3D)
					{
						this.GetInnerWidget().RunAction();
					}
					else
					{
						this.GetInnerWidget().StopAction();
					}
				}
				string taskName = base.GetType().Name + "FileData";
				using (CompositeTask.Run(taskName, null))
				{
					this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
				}
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000702C File Offset: 0x0000522C
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00007034 File Offset: 0x00005234
		[Browsable(true)]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"c3b",
			"c3t",
			"obj"
		})]
		[DisplayName("Display_IsRunAction")]
		[Category("Group_Routine")]
		[PropertyOrder(111)]
		public bool RunAction3D
		{
			get
			{
				return this.runAction;
			}
			set
			{
				this.runAction = value;
				if (this.isHasAction)
				{
					if (this.runAction)
					{
						this.GetInnerWidget().RunAction();
					}
					else
					{
						this.GetInnerWidget().StopAction();
					}
				}
				this.RaisePropertyChanged<bool>(() => this.RunAction3D);
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000070AA File Offset: 0x000052AA
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x000070B8 File Offset: 0x000052B8
		[Category("Group_Feature")]
		[UndoProperty]
		[DisplayName("Display_Component_LightFlag")]
		[Browsable(true)]
		[PropertyOrder(31)]
		public LightFlag LightFlag
		{
			get
			{
				return (LightFlag)this.GetInnerWidget().GetLightMask();
			}
			set
			{
				this.GetInnerWidget().SetLightMask((int)value);
				this.RaisePropertyChanged<LightFlag>(() => this.LightFlag);
			}
		}

		// Token: 0x0400008D RID: 141
		private ResourceFile file;

		// Token: 0x0400008E RID: 142
		private bool isHasAction;

		// Token: 0x0400008F RID: 143
		private bool runAction;
	}
}
