using System;
using System.Runtime.Serialization;
using Gdk;
using Mono.Addins;

namespace EditorCommon.JsonModel
{
	[DataContract]
	[Extension(typeof(IJsonModel))]
	internal class RectSurrogate : BaseEntitySurrogate
	{
		[DataMember]
		public double x { get; set; }

		[DataMember]
		public double y { get; set; }

		[DataMember]
		public double w { get; set; }

		[DataMember]
		public double h { get; set; }

		protected RectSurrogate()
		{
		}

		public RectSurrogate(Rectangle rect)
		{
			this.x = (double)rect.X;
			this.y = (double)rect.Y;
			this.w = (double)rect.Width;
			this.h = (double)rect.Height;
		}
	}
}
