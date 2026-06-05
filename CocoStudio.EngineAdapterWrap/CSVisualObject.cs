using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000009 RID: 9
	public class CSVisualObject : CSObject
	{
		// Token: 0x06000060 RID: 96 RVA: 0x0000322E File Offset: 0x0000142E
		protected internal CSVisualObject()
		{
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00003239 File Offset: 0x00001439
		public CSVisualObject(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSVisualObject_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003258 File Offset: 0x00001458
		public static HandleRef getCPtr(CSVisualObject obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000032A8 File Offset: 0x000014A8
		public override void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						HandleRef handle = new HandleRef(null, this.swigCPtr.Handle);
						if (!this.IsContainOpenGLResource())
						{
							throw new MethodAccessException("C++ destructor does not have public access");
						}
						GtkInvokeHelp.BeginInvoke(delegate
						{
							this.swigCPtr = handle;
							throw new MethodAccessException("C++ destructor does not have public access");
						});
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
				base.Dispose();
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000033A8 File Offset: 0x000015A8
		public virtual int GetTag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetTag(this.swigCPtr);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000033C7 File Offset: 0x000015C7
		public virtual void SetTag(int tag)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetTag(this.swigCPtr, tag);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000033D8 File Offset: 0x000015D8
		public virtual string GetName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetName(this.swigCPtr);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000033F8 File Offset: 0x000015F8
		public virtual void SetName(string nameStr)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetName(this.swigCPtr, nameStr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003428 File Offset: 0x00001628
		public virtual bool GetVisible()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetVisible(this.swigCPtr);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003447 File Offset: 0x00001647
		public virtual void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetVisible(this.swigCPtr, visible);
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00003458 File Offset: 0x00001658
		public virtual CocoStudio.Model.PointF GetPosition()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetPosition(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00003494 File Offset: 0x00001694
		public virtual void SetPosition(CocoStudio.Model.PointF position)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetPosition(this.swigCPtr, Vec2.getCPtr(new Vec2(position.X, position.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x000034D8 File Offset: 0x000016D8
		public virtual ScaleValue GetAnchorPoint()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAnchorPoint(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000352C File Offset: 0x0000172C
		public virtual CocoStudio.Model.PointF GetAnchorPointInPoints()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAnchorPointInPoints(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003568 File Offset: 0x00001768
		public virtual void SetAnchorPoint(ScaleValue anchorPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetAnchorPoint(this.swigCPtr, CSScale.getCPtr(new CSScale(anchorPoint.ScaleX, anchorPoint.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000035AC File Offset: 0x000017AC
		public virtual ScaleValue GetScale()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetScale(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00003600 File Offset: 0x00001800
		public virtual void SetScale(ScaleValue scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetScale(this.swigCPtr, CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003644 File Offset: 0x00001844
		public virtual float GetRotation()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotation(this.swigCPtr);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00003663 File Offset: 0x00001863
		public virtual void SetRotation(float rotation)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotation(this.swigCPtr, rotation);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003674 File Offset: 0x00001874
		public virtual float GetRotationSkewX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotationSkewX(this.swigCPtr);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003693 File Offset: 0x00001893
		public virtual void SetRotationSkewX(float rotationSkewX)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotationSkewX(this.swigCPtr, rotationSkewX);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000036A4 File Offset: 0x000018A4
		public virtual float GetRotationSkewY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotationSkewY(this.swigCPtr);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000036C3 File Offset: 0x000018C3
		public virtual void SetRotationSkewY(float rotationSkewY)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotationSkewY(this.swigCPtr, rotationSkewY);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000036D4 File Offset: 0x000018D4
		public virtual int GetZOrder()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetZOrder(this.swigCPtr);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000036F3 File Offset: 0x000018F3
		public virtual void SetZOrder(int zOrder)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetZOrder(this.swigCPtr, zOrder);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00003704 File Offset: 0x00001904
		public virtual int GetOrderOfArrival()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetOrderOfArrival(this.swigCPtr);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003724 File Offset: 0x00001924
		public virtual Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003764 File Offset: 0x00001964
		public virtual Color GetColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000037A4 File Offset: 0x000019A4
		public virtual void SetColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000037F0 File Offset: 0x000019F0
		public virtual int GetAlpha()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAlpha(this.swigCPtr);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000380F File Offset: 0x00001A0F
		public virtual void SetAlpha(int alpha)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetAlpha(this.swigCPtr, alpha);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003820 File Offset: 0x00001A20
		public virtual bool GetCascadeColorEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetCascadeColorEnabled(this.swigCPtr);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000383F File Offset: 0x00001A3F
		public virtual void SetCascadeColorEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetCascadeColorEnabled(this.swigCPtr, enable);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00003850 File Offset: 0x00001A50
		public virtual bool GetCascadeOpacityEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetCascadeOpacityEnabled(this.swigCPtr);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000386F File Offset: 0x00001A6F
		public virtual void SetCascadeOpacityEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetCascadeOpacityEnabled(this.swigCPtr, enable);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003880 File Offset: 0x00001A80
		public virtual RectF GetBoundingRect()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetBoundingRect(this.swigCPtr);
			Rect rect = new Rect(cPtr, true);
			if (rect.size.width < 0f || rect.size.height < 0f)
			{
				rect.origin.x = 0f;
				rect.origin.y = 0f;
				rect.size.width = 0f;
				rect.size.height = 0f;
			}
			return new RectF(rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003950 File Offset: 0x00001B50
		public virtual CocoStudio.Model.SizeF GetSize()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetSize(this.swigCPtr);
			Size size = new Size(cPtr, true);
			if (size.width < 0f || size.height < 0f)
			{
				size.width = 0f;
				size.height = 0f;
			}
			return new CocoStudio.Model.SizeF(size.width, size.height);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000039CC File Offset: 0x00001BCC
		public virtual void SetSize(CocoStudio.Model.SizeF cSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetSize(this.swigCPtr, Size.getCPtr(new Size(cSize.Width, cSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00003A10 File Offset: 0x00001C10
		public virtual void SetPosition3D(Point3F pos)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetPosition3D(this.swigCPtr, Vec3.getCPtr(new Vec3(pos.X, pos.Y, pos.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003A58 File Offset: 0x00001C58
		public virtual Point3F GetPosition3D()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetPosition3D(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00003A98 File Offset: 0x00001C98
		public virtual Point3F GetWorldPosition()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetWorldPosition(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003AD8 File Offset: 0x00001CD8
		public virtual void SetRotation3D(Point3F rot)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotation3D(this.swigCPtr, Vec3.getCPtr(new Vec3(rot.X, rot.Y, rot.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00003B20 File Offset: 0x00001D20
		public virtual Point3F GetRotation3D()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotation3D(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x00003B60 File Offset: 0x00001D60
		public virtual void SetScale3D(Point3F scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetScale3D(this.swigCPtr, Vec3.getCPtr(new Vec3(scale.X, scale.Y, scale.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00003BA8 File Offset: 0x00001DA8
		public virtual Point3F GetScale3D()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetScale3D(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00003BE8 File Offset: 0x00001DE8
		public virtual void SetOrientation(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetOrientation(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00003C1C File Offset: 0x00001E1C
		public virtual Quaternion GetOrientation()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetOrientation(this.swigCPtr), true);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00003C44 File Offset: 0x00001E44
		public virtual CSVisualObject.ObjectState GetObjectState()
		{
			return (CSVisualObject.ObjectState)CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetObjectState(this.swigCPtr);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00003C63 File Offset: 0x00001E63
		public virtual void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetObjectState(this.swigCPtr, (int)boxState);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003C73 File Offset: 0x00001E73
		public virtual void AddChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_AddChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003C88 File Offset: 0x00001E88
		public virtual void InsertChild(int index, CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_InsertChild(this.swigCPtr, index, CSVisualObject.getCPtr(child));
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00003C9E File Offset: 0x00001E9E
		public virtual void RemoveChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_RemoveChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00003CB4 File Offset: 0x00001EB4
		public virtual int HitTest(CocoStudio.Model.PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00003D00 File Offset: 0x00001F00
		public virtual bool RectTest(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_RectTest(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003D54 File Offset: 0x00001F54
		public virtual float HitTest3D(CocoStudio.Model.PointF screenPoint)
		{
			float result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_HitTest3D(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003DA0 File Offset: 0x00001FA0
		public virtual bool RectTest3D(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_RectTest3D(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003DF4 File Offset: 0x00001FF4
		public virtual CocoStudio.Model.PointF TransformToSelf(CocoStudio.Model.PointF scenePoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_TransformToSelf(this.swigCPtr, Vec2.getCPtr(new Vec2(scenePoint.X, scenePoint.Y)));
			Vec2 vec = new Vec2(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003E5C File Offset: 0x0000205C
		public virtual CocoStudio.Model.PointF TransformToScene(CocoStudio.Model.PointF selfPoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_TransformToScene(this.swigCPtr, Vec2.getCPtr(new Vec2(selfPoint.X, selfPoint.Y)));
			Vec2 vec = new Vec2(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003EC4 File Offset: 0x000020C4
		public virtual CocoStudio.Model.PointF TransformToParent(CocoStudio.Model.PointF selfPoint)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_TransformToParent(this.swigCPtr, Vec2.getCPtr(new Vec2(selfPoint.X, selfPoint.Y)));
			Vec2 vec = new Vec2(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003F2C File Offset: 0x0000212C
		public virtual CSMatrix GetWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetWorldMatrix(this.swigCPtr), true);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003F54 File Offset: 0x00002154
		public virtual CSMatrix GetAnchorWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAnchorWorldMatrix(this.swigCPtr), true);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003F7C File Offset: 0x0000217C
		public virtual CSMatrix GetParentWorldMatrix()
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetParentWorldMatrix(this.swigCPtr), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003FB4 File Offset: 0x000021B4
		public virtual CSMatrix ConvertToNodeMatrix(CSVisualObject dst)
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_ConvertToNodeMatrix(this.swigCPtr, CSVisualObject.getCPtr(dst)), true);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003FE0 File Offset: 0x000021E0
		public virtual void ApplySelfWorldMatirx(CSMatrix selfWorldMatrix)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_ApplySelfWorldMatirx(this.swigCPtr, CSMatrix.getCPtr(selfWorldMatrix));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x04000009 RID: 9
		private HandleRef swigCPtr;

		// Token: 0x0200000A RID: 10
		public enum TransformSpace
		{
			// Token: 0x0400000B RID: 11
			TS_LOCAL,
			// Token: 0x0400000C RID: 12
			TS_WORLD,
			// Token: 0x0400000D RID: 13
			TS_PARENT
		}

		// Token: 0x0200000B RID: 11
		public enum ObjectState
		{
			// Token: 0x0400000F RID: 15
			Default,
			// Token: 0x04000010 RID: 16
			DragOver,
			// Token: 0x04000011 RID: 17
			Seleted
		}
	}
}
