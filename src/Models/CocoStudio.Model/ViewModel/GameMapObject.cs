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
	// Token: 0x020000F1 RID: 241
	[EngineClassName("TMXTiledMap")]
	[ControlGroup("Control_BaseObject", 0)]
	[ModelExtension(true, 11)]
	[DisplayName("Display_Component_Map")]
	public class GameMapObject : NodeObject
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x00020CD4 File Offset: 0x0001EED4
		private CSGameMap GetInnerWidget()
		{
			return (CSGameMap)this.innerNode;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00020CF1 File Offset: 0x0001EEF1
		public GameMapObject()
		{
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00020D03 File Offset: 0x0001EF03
		public GameMapObject(ScriptFileData fileData) : base(fileData)
		{
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00020D16 File Offset: 0x0001EF16
		protected override void CreateCSObject()
		{
			this.innerNode = new CSGameMap();
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x00020D24 File Offset: 0x0001EF24
		protected override void InitData(bool useScript)
		{
			base.InitData(useScript);
			if (!useScript)
			{
				this.FileData = null;
			}
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00020D4C File Offset: 0x0001EF4C
		protected internal override string GetNamePrefix()
		{
			return "Map_";
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00020D63 File Offset: 0x0001EF63
		public override void InitOperation()
		{
			this.OperationFlag = (OperationMask)65511;
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00020D74 File Offset: 0x0001EF74
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x00020DBC File Offset: 0x0001EFBC
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

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600083B RID: 2107 RVA: 0x00020E38 File Offset: 0x0001F038
		// (set) Token: 0x0600083C RID: 2108 RVA: 0x00020E58 File Offset: 0x0001F058
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

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x00020EB0 File Offset: 0x0001F0B0
		// (set) Token: 0x0600083E RID: 2110 RVA: 0x00020ED0 File Offset: 0x0001F0D0
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

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600083F RID: 2111 RVA: 0x00020F28 File Offset: 0x0001F128
		// (set) Token: 0x06000840 RID: 2112 RVA: 0x00020F48 File Offset: 0x0001F148
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

		// Token: 0x06000841 RID: 2113 RVA: 0x00020FA0 File Offset: 0x0001F1A0
		protected override void SetValue(object cObject)
		{
			base.SetValue(cObject);
			GameMapObject gameMapObject = cObject as GameMapObject;
			if (gameMapObject != null)
			{
				gameMapObject.FileData = this.FileData;
			}
		}

		// Token: 0x0400032A RID: 810
		private ResourceFile file = null;
	}
}
