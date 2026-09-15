using System;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.Projects;
using CocoStudio.UndoManager;
using Gtk;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.ViewModel
{
	[Extension(Type = typeof(IUserData))]
	[DataItem(Name = "Guides")]
	public class GuidesObject : BaseObject, IComparable<GuidesObject>, ICloneable, IUserData
	{
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

		protected GuidesObject()
		{
			this.Direction = Orientation.Vertical;
		}

		public GuidesObject(Orientation direction)
		{
			this.innerNode = new CSGuides((LineDirection)direction);
			this.Direction = direction;
		}

		internal CSGuides GetNode()
		{
			return this.innerNode;
		}

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

		public override string ToString()
		{
			string arg = "x";
			if (this.Direction == Orientation.Horizontal)
			{
				arg = "y";
			}
			return string.Format("{0} : {1}", arg, this.Position.ToString("0.0"));
		}

		public object Clone()
		{
			return new GuidesObject(this.Direction)
			{
				Position = this.Position
			};
		}

		private Orientation direction;

		private float position = 0f;

		private CSGuides innerNode;
	}
}
