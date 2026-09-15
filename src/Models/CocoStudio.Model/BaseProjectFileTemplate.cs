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
	[TypeExtensionPoint]
	public abstract class BaseProjectFileTemplate : IProjectFileCreator, IProjectFileRenderView, IComparable
	{
		public string Name
		{
			get
			{
				return this.FileType.ToString();
			}
		}

		public abstract NodeType FileType { get; }

		public virtual string FileExtension
		{
			get
			{
				return ".csd";
			}
		}

		public abstract int Order { get; }

		public virtual string LabelName
		{
			get
			{
				return this.Name;
			}
		}

		public virtual string Description
		{
			get
			{
				return "";
			}
		}

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

		protected abstract string OnGetIconResource();

		public virtual int MaxSize
		{
			get
			{
				return int.MaxValue;
			}
		}

		public virtual bool CanEditSize
		{
			get
			{
				return false;
			}
		}

		public virtual bool IsShowTrackPoint
		{
			get
			{
				return false;
			}
		}

		public GameFileData CreateGameProjectData()
		{
			GameFileData gameFileData = new GameFileData();
			this.OnInitGameProejctData(gameFileData);
			gameFileData.ObjectData.Tag = VisualObject.tag;
			gameFileData.ObjectData.Name = this.FileType.ToString();
			return gameFileData;
		}

		protected virtual void OnInitGameProejctData(GameFileData gameProjectData)
		{
			gameProjectData.ObjectData = new GameNodeObjectData();
		}

		public void ChangeView(CanvasObject canvas, CocosItem project)
		{
			this.OnChangeView(canvas, project);
		}

		protected virtual void OnChangeView(CanvasObject canvas, CocosItem project)
		{
		}

		public int CompareTo(object obj)
		{
			IProjectFileCreator projectFileCreator = obj as IProjectFileCreator;
			return this.Order.CompareTo(projectFileCreator.Order);
		}

		private Image icon;
	}
}
