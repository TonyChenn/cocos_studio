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
	[EngineClassName("TMXTiledMap")]
	[ControlGroup("Control_BaseObject", 0)]
	[ModelExtension(true, 11)]
	[DisplayName("Display_Component_Map")]
	public class GameMapObject : NodeObject
	{
		private CSGameMap GetInnerWidget()
		{
			return (CSGameMap)this.innerNode;
		}

		public GameMapObject()
		{
		}

		public GameMapObject(ScriptFileData fileData) : base(fileData)
		{
		}

		protected override void CreateCSObject()
		{
			this.innerNode = new CSGameMap();
		}

		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		protected internal override string GetNamePrefix()
		{
			return "Map_";
		}

		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65511;
		}

		[Description("Description_File")]
		[DisplayName("Display_File")]
		[DefaultValue(null)]
		[PropertyOrder(112)]
		[Category("Group_Feature")]
		[Browsable(true)]
		[UndoProperty]
		[ResourceFilter(new string[]
		{
			"tmx"
		})]
		[Editor(typeof(ResourceFileEditor), typeof(ResourceFileEditor))]
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
				TmxFile defaultFile = new TmxFile(GameMapObjectData.DefaultFile);
				ResourceFile resourceFile = ResourceFile.PreprocessToEngine(ref this.file, defaultFile, false);
				this.GetInnerWidget().SetFileData(resourceFile.GetResourceData());
				this.RaisePropertyChanged<ResourceFile>(() => this.FileData);
			}
		}

		[Browsable(true)]
		[UndoProperty]
		public override ScaleValue AnchorPoint
		{
			get
			{
				return this.GetCSVisual().GetAnchorPoint();
			}
			set
			{
				this.GetCSVisual().SetAnchorPoint(value);
				this.RaisePropertyChanged<ScaleValue>(() => this.AnchorPoint);
			}
		}

		[Browsable(false)]
		[UndoProperty]
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
		[UndoProperty]
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

		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			GameMapObject gameMapObject = cObject as GameMapObject;
			if (gameMapObject != null)
			{
				gameMapObject.FileData = this.FileData;
			}
		}

		private ResourceFile file = null;
	}
}
