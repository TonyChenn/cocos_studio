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
	[ControlGroup("Control_3DControl", 0)]
	[ModelExtension(true, 100, EnumModelType.ThreeDimensional)]
	[DisplayName("Display_Component_Sprite3D")]
	public class Sprite3DObject : Node3DObject
	{
		public Sprite3DObject()
		{
		}

		public Sprite3DObject(ResourceFile resourceFile) : this()
		{
			this.FileData = resourceFile;
			this.Name = this.FileData.FileName.FileNameWithoutExtension;
		}

		public Sprite3DObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSSprite3D();
		}

		private CSSprite3D GetInnerWidget()
		{
			return (CSSprite3D)this.innerNode;
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			this.LightFlag = LightFlag.LIGHT0;
			if (!useScript)
			{
				this.FileData = null;
			}
		}

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

		private ResourceFile file;

		private bool isHasAction;

		private bool runAction;
	}
}
