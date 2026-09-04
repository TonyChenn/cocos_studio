using System;
using System.IO;
using CocoStudio.DefaultResource;
using CocoStudio.Model.DataModel;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Mono.Addins;
using Xwt.Drawing;

namespace CocoStudio.Model
{
	// Token: 0x020000C3 RID: 195
	[TypeExtensionPoint]
	public abstract class BaseProjectFileTemplate : IProjectFileCreator, IProjectFileRenderView, IComparable
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600061E RID: 1566 RVA: 0x00019610 File Offset: 0x00017810
		public string Name
		{
			get
			{
				return this.FileType.ToString();
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600061F RID: 1567
		public abstract NodeType FileType { get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x00019634 File Offset: 0x00017834
		public virtual string FileExtension
		{
			get
			{
				return ".csd";
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000621 RID: 1569
		public abstract int Order { get; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000622 RID: 1570 RVA: 0x0001964C File Offset: 0x0001784C
		public virtual string LabelName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00019664 File Offset: 0x00017864
		public virtual string Description
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000624 RID: 1572 RVA: 0x0001967C File Offset: 0x0001787C
		public Image Icon
		{
			get
			{
				if (this.icon == null)
				{
					Stream resourceStream = Resources.GetResourceStream(this.OnGetIconResource());
					this.icon = Image.FromStream(resourceStream);
				}
				return this.icon;
			}
		}

		// Token: 0x06000625 RID: 1573
		protected abstract string OnGetIconResource();

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x000196C0 File Offset: 0x000178C0
		public virtual int MaxSize
		{
			get
			{
				return int.MaxValue;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x000196D8 File Offset: 0x000178D8
		public virtual bool CanEditSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000196EC File Offset: 0x000178EC
		public virtual bool IsShowTrackPoint
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00019700 File Offset: 0x00017900
		public GameFileData CreateGameProjectData()
		{
			GameFileData gameFileData = new GameFileData();
			this.OnInitGameProejctData(gameFileData);
			gameFileData.ObjectData.Tag = VisualObject.tag;
			gameFileData.ObjectData.Name = this.FileType.ToString();
			return gameFileData;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0001974E File Offset: 0x0001794E
		protected virtual void OnInitGameProejctData(GameFileData gameProjectData)
		{
			gameProjectData.ObjectData = new GameNodeObjectData();
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x0001975D File Offset: 0x0001795D
		public void ChangeView(CanvasObject canvas, CocosItem project)
		{
			this.OnChangeView(canvas, project);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00019769 File Offset: 0x00017969
		protected virtual void OnChangeView(CanvasObject canvas, CocosItem project)
		{
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x0001976C File Offset: 0x0001796C
		public int CompareTo(object obj)
		{
			IProjectFileCreator projectFileCreator = obj as IProjectFileCreator;
			return this.Order.CompareTo(projectFileCreator.Order);
		}

		// Token: 0x040002BD RID: 701
		private Image icon;
	}
}
