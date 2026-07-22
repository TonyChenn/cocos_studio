using System;
using CocoStudio.Projects;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Model.DataModel
{
	// Token: 0x02000016 RID: 22
	[Extension(Type = typeof(IUserData))]
	public class CameraData : IUserData
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00003920 File Offset: 0x00001B20
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00003937 File Offset: 0x00001B37
		[ItemProperty(DefaultValue = null)]
		public Point3F Position { get; set; }

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x00003940 File Offset: 0x00001B40
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x00003957 File Offset: 0x00001B57
		[ItemProperty(DefaultValue = null)]
		public Point3F Rotation { get; set; }

		// Token: 0x060000F7 RID: 247 RVA: 0x00003960 File Offset: 0x00001B60
		public CameraData()
		{
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000396B File Offset: 0x00001B6B
		public CameraData(Point3F position, Point3F rotation)
		{
			this.Position = position;
			this.Rotation = rotation;
		}

		// Token: 0x04000062 RID: 98
		public const string CameraDataKey = "CameraData";
	}
}
