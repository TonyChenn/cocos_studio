using System;
using CocoStudio.Model.ViewModel;
using Gtk;
using Modules.Communal.PropertyGrid;

namespace CocoStudio.Model.Editor
{
	// Token: 0x02000012 RID: 18
	internal class Position3DEditor : ThreeNumberEditor
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x00003618 File Offset: 0x00001818
		protected override void OnSetControl()
		{
			Func<Node3DObject, Node3DObject, bool> funcX = (Node3DObject a, Node3DObject b) => a.Position3D.X == b.Position3D.X;
			Func<Node3DObject, Node3DObject, bool> funcY = (Node3DObject a, Node3DObject b) => a.Position3D.Y == b.Position3D.Y;
			Func<Node3DObject, Node3DObject, bool> funcZ = (Node3DObject a, Node3DObject b) => a.Position3D.Z == b.Position3D.Z;
			base.CompareNumber(funcX, funcY, funcZ);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00003688 File Offset: 0x00001888
		protected override void OnXValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.X = e.Value;
					if (point3F.X > 20000f)
					{
						point3F.X = 20000f;
					}
					if (point3F.X < -20000f)
					{
						point3F.X = -20000f;
					}
					this.xInnerEntry.Value = point3F.X;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00003748 File Offset: 0x00001948
		protected override void OnYValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.Y = e.Value;
					if (point3F.Y > 20000f)
					{
						point3F.Y = 20000f;
					}
					if (point3F.Y < -20000f)
					{
						point3F.Y = -20000f;
					}
					this.yInnerEntry.Value = point3F.Y;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00003808 File Offset: 0x00001A08
		protected override void OnZValueChanged(EntryIntEventArgs e)
		{
			using (base.GetLock(true))
			{
				for (int i = 0; i < PropertyItem.Objects.Count; i++)
				{
					Point3F point3F = (Point3F)base.PropertyItem.Values[i];
					point3F.Z = e.Value;
					if (point3F.Z > 20000f)
					{
						point3F.Z = 20000f;
					}
					if (point3F.Z < -20000f)
					{
						point3F.Z = -20000f;
					}
					this.zInnerEntry.Value = point3F.Z;
					base.PropertyItem.Values[i] = point3F;
				}
			}
		}

		// Token: 0x04000048 RID: 72
		private const float MAXVALUE = 20000f;

		// Token: 0x04000049 RID: 73
		private const float MINVALUE = -20000f;
	}
}
