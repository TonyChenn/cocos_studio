using System;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000133 RID: 307
	[Extension(Type = typeof(IUserData))]
	[DataItem(Name = "Guides")]
	public class GuidesObject : BaseObject, IComparable<GuidesObject>, ICloneable, IUserData
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x0002CD44 File Offset: 0x0002AF44
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x0002CD5C File Offset: 0x0002AF5C
		[ItemProperty(DefaultValue = Orientation.Vertical)]
		[UndoProperty]
		public virtual Orientation Direction
		{
			get
			{
				return this.direction;
			}
			set
			{
				this.direction = value;
				if (this.innerNode == null)
				{
					this.innerNode = new CSGuides((LineDirection)value);
				}
				else
				{
					this.innerNode.SetDirection((LineDirection)value);
				}
				this.RaisePropertyChanged<Orientation>(() => this.Direction);
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x0002CDD8 File Offset: 0x0002AFD8
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x0002CDF0 File Offset: 0x0002AFF0
		[ItemProperty]
		[UndoProperty]
		public virtual float Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				if (this.innerNode == null)
				{
					this.innerNode = new CSGuides((LineDirection)this.Direction);
				}
				this.innerNode.SetPosition(value);
				this.RaisePropertyChanged<float>(() => this.Position);
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x0002CE6E File Offset: 0x0002B06E
		protected GuidesObject()
		{
			this.Direction = Orientation.Vertical;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x0002CE8C File Offset: 0x0002B08C
		public GuidesObject(Orientation direction)
		{
			this.innerNode = new CSGuides((LineDirection)direction);
			this.Direction = direction;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0002CEB8 File Offset: 0x0002B0B8
		internal CSGuides GetNode()
		{
			return this.innerNode;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0002CED0 File Offset: 0x0002B0D0
		public PointF GetPoint()
		{
			PointF pointF = new PointF();
			switch (this.direction)
			{
			case Orientation.Horizontal:
				pointF.Y = this.position;
				break;
			case Orientation.Vertical:
				pointF.X = this.position;
				break;
			}
			return pointF;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x0002CF20 File Offset: 0x0002B120
		public int CompareTo(GuidesObject other)
		{
			int result;
			if (this.Direction != other.Direction)
			{
				result = this.Direction.CompareTo(other.Direction);
			}
			else
			{
				result = this.Position.CompareTo(other.Position);
			}
			return result;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0002CF78 File Offset: 0x0002B178
		public override string ToString()
		{
			string arg = "x";
			if (this.Direction == Orientation.Horizontal)
			{
				arg = "y";
			}
			return string.Format("{0} : {1}", arg, this.Position.ToString("0.0"));
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0002CFC4 File Offset: 0x0002B1C4
		public object Clone()
		{
			return new GuidesObject(this.Direction)
			{
				Position = this.Position
			};
		}

		// Token: 0x040004D5 RID: 1237
		private Orientation direction;

		// Token: 0x040004D6 RID: 1238
		private float position = 0f;

		// Token: 0x040004D7 RID: 1239
		private CSGuides innerNode;
	}
}
