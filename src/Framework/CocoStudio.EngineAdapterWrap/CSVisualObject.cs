using System;
using System.Drawing;
using System.Runtime.InteropServices;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	public class CSVisualObject : CSObject
	{
		protected internal CSVisualObject()
		{
		}

		public CSVisualObject(IntPtr cPtr, bool cMemoryOwn) : base(CocoStudioEngineAdapterPINVOKE.CSVisualObject_SWIGUpcast(cPtr), cMemoryOwn)
		{
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		public static HandleRef getCPtr(CSVisualObject obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

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

		public virtual int GetTag()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetTag(this.swigCPtr);
		}

		public virtual void SetTag(int tag)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetTag(this.swigCPtr, tag);
		}

		public virtual string GetName()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetName(this.swigCPtr);
		}

		public virtual void SetName(string nameStr)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetName(this.swigCPtr, nameStr);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual bool GetVisible()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetVisible(this.swigCPtr);
		}

		public virtual void SetVisible(bool visible)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetVisible(this.swigCPtr, visible);
		}

		public virtual CocoStudio.Model.PointF GetPosition()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetPosition(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		public virtual void SetPosition(CocoStudio.Model.PointF position)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetPosition(this.swigCPtr, Vec2.getCPtr(new Vec2(position.X, position.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ScaleValue GetAnchorPoint()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAnchorPoint(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		public virtual CocoStudio.Model.PointF GetAnchorPointInPoints()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAnchorPointInPoints(this.swigCPtr);
			Vec2 vec = new Vec2(cPtr, true);
			return new CocoStudio.Model.PointF(vec.x, vec.y);
		}

		public virtual void SetAnchorPoint(ScaleValue anchorPoint)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetAnchorPoint(this.swigCPtr, CSScale.getCPtr(new CSScale(anchorPoint.ScaleX, anchorPoint.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual ScaleValue GetScale()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetScale(this.swigCPtr);
			CSScale csscale = new CSScale(cPtr, false);
			return new ScaleValue(csscale.GetScaleX(), csscale.GetScaleY(), 0.1, -99999999.0, 99999999.0);
		}

		public virtual void SetScale(ScaleValue scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetScale(this.swigCPtr, CSScale.getCPtr(new CSScale(scale.ScaleX, scale.ScaleY)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual float GetRotation()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotation(this.swigCPtr);
		}

		public virtual void SetRotation(float rotation)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotation(this.swigCPtr, rotation);
		}

		public virtual float GetRotationSkewX()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotationSkewX(this.swigCPtr);
		}

		public virtual void SetRotationSkewX(float rotationSkewX)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotationSkewX(this.swigCPtr, rotationSkewX);
		}

		public virtual float GetRotationSkewY()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotationSkewY(this.swigCPtr);
		}

		public virtual void SetRotationSkewY(float rotationSkewY)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotationSkewY(this.swigCPtr, rotationSkewY);
		}

		public virtual int GetZOrder()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetZOrder(this.swigCPtr);
		}

		public virtual void SetZOrder(int zOrder)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetZOrder(this.swigCPtr, zOrder);
		}

		public virtual int GetOrderOfArrival()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetOrderOfArrival(this.swigCPtr);
		}

		public virtual Color GetDisplayColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetDisplayColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual Color GetColor()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetColor(this.swigCPtr);
			Color3B color3B = new Color3B(cPtr, true);
			return Color.FromArgb((int)color3B.r, (int)color3B.g, (int)color3B.b);
		}

		public virtual void SetColor(Color color)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetColor(this.swigCPtr, Color3B.getCPtr(new Color3B(color.R, color.G, color.B)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual int GetAlpha()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAlpha(this.swigCPtr);
		}

		public virtual void SetAlpha(int alpha)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetAlpha(this.swigCPtr, alpha);
		}

		public virtual bool GetCascadeColorEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetCascadeColorEnabled(this.swigCPtr);
		}

		public virtual void SetCascadeColorEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetCascadeColorEnabled(this.swigCPtr, enable);
		}

		public virtual bool GetCascadeOpacityEnabled()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetCascadeOpacityEnabled(this.swigCPtr);
		}

		public virtual void SetCascadeOpacityEnabled(bool enable)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetCascadeOpacityEnabled(this.swigCPtr, enable);
		}

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

		public virtual void SetSize(CocoStudio.Model.SizeF cSize)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetSize(this.swigCPtr, Size.getCPtr(new Size(cSize.Width, cSize.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual void SetPosition3D(Point3F pos)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetPosition3D(this.swigCPtr, Vec3.getCPtr(new Vec3(pos.X, pos.Y, pos.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Point3F GetPosition3D()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetPosition3D(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public virtual Point3F GetWorldPosition()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetWorldPosition(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public virtual void SetRotation3D(Point3F rot)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetRotation3D(this.swigCPtr, Vec3.getCPtr(new Vec3(rot.X, rot.Y, rot.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Point3F GetRotation3D()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetRotation3D(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public virtual void SetScale3D(Point3F scale)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetScale3D(this.swigCPtr, Vec3.getCPtr(new Vec3(scale.X, scale.Y, scale.Z)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Point3F GetScale3D()
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetScale3D(this.swigCPtr);
			Vec3 vec = new Vec3(cPtr, true);
			return new Point3F(vec.x, vec.y, vec.z);
		}

		public virtual void SetOrientation(Quaternion q)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetOrientation(this.swigCPtr, Quaternion.getCPtr(q));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		public virtual Quaternion GetOrientation()
		{
			return new Quaternion(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetOrientation(this.swigCPtr), true);
		}

		public virtual CSVisualObject.ObjectState GetObjectState()
		{
			return (CSVisualObject.ObjectState)CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetObjectState(this.swigCPtr);
		}

		public virtual void SetObjectState(CSVisualObject.ObjectState boxState)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_SetObjectState(this.swigCPtr, (int)boxState);
		}

		public virtual void AddChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_AddChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		public virtual void InsertChild(int index, CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_InsertChild(this.swigCPtr, index, CSVisualObject.getCPtr(child));
		}

		public virtual void RemoveChild(CSVisualObject child)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_RemoveChild(this.swigCPtr, CSVisualObject.getCPtr(child));
		}

		public virtual int HitTest(CocoStudio.Model.PointF point)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_HitTest(this.swigCPtr, Vec2.getCPtr(new Vec2(point.X, point.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual bool RectTest(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_RectTest(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual float HitTest3D(CocoStudio.Model.PointF screenPoint)
		{
			float result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_HitTest3D(this.swigCPtr, Vec2.getCPtr(new Vec2(screenPoint.X, screenPoint.Y)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual bool RectTest3D(RectF rect)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSVisualObject_RectTest3D(this.swigCPtr, Rect.getCPtr(new Rect(rect.X, rect.Y, rect.Width, rect.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

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

		public virtual CSMatrix GetWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetWorldMatrix(this.swigCPtr), true);
		}

		public virtual CSMatrix GetAnchorWorldMatrix()
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetAnchorWorldMatrix(this.swigCPtr), true);
		}

		public virtual CSMatrix GetParentWorldMatrix()
		{
			CSMatrix result = new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_GetParentWorldMatrix(this.swigCPtr), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		public virtual CSMatrix ConvertToNodeMatrix(CSVisualObject dst)
		{
			return new CSMatrix(CocoStudioEngineAdapterPINVOKE.CSVisualObject_ConvertToNodeMatrix(this.swigCPtr, CSVisualObject.getCPtr(dst)), true);
		}

		public virtual void ApplySelfWorldMatirx(CSMatrix selfWorldMatrix)
		{
			CocoStudioEngineAdapterPINVOKE.CSVisualObject_ApplySelfWorldMatirx(this.swigCPtr, CSMatrix.getCPtr(selfWorldMatrix));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		private HandleRef swigCPtr;

		public enum TransformSpace
		{
			TS_LOCAL,
			TS_WORLD,
			TS_PARENT
		}

		public enum ObjectState
		{
			Default,
			DragOver,
			Seleted
		}
	}
}
