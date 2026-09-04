using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000065 RID: 101
	public class CSVectorPoint : IDisposable, IEnumerable<Vec2>, IEnumerable
	{
		// Token: 0x06000B81 RID: 2945 RVA: 0x00013ABC File Offset: 0x00011CBC
		public CSVectorPoint(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x00013ADC File Offset: 0x00011CDC
		public static HandleRef getCPtr(CSVectorPoint obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00013B08 File Offset: 0x00011D08
		~CSVectorPoint()
		{
			this.Dispose();
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x00013B6C File Offset: 0x00011D6C
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						HandleRef handle = new HandleRef(null, this.swigCPtr.Handle);
						if (this.IsContainOpenGLResource())
						{
							GtkInvokeHelp.BeginInvoke(delegate
							{
								this.swigCPtr = handle;
								CocoStudioEngineAdapterPINVOKE.delete_CSVectorPoint(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSVectorPoint(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x00013C64 File Offset: 0x00011E64
		public CSVectorPoint(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			foreach (object obj in c)
			{
				Vec2 x = (Vec2)obj;
				this.Add(x);
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000B86 RID: 2950 RVA: 0x00013CE4 File Offset: 0x00011EE4
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x00013CF8 File Offset: 0x00011EF8
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000059 RID: 89
		public Vec2 this[int index]
		{
			get
			{
				return this.getitem(index);
			}
			set
			{
				this.setitem(index, value);
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000B8A RID: 2954 RVA: 0x00013D34 File Offset: 0x00011F34
		// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00013D4C File Offset: 0x00011F4C
		public int Capacity
		{
			get
			{
				return (int)this.capacity();
			}
			set
			{
				if ((long)value < (long)((ulong)this.size()))
				{
					throw new ArgumentOutOfRangeException("Capacity");
				}
				this.reserve((uint)value);
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00013D80 File Offset: 0x00011F80
		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x00013D98 File Offset: 0x00011F98
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00013DAB File Offset: 0x00011FAB
		public void CopyTo(Vec2[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00013DBE File Offset: 0x00011FBE
		public void CopyTo(Vec2[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00013DD4 File Offset: 0x00011FD4
		public void CopyTo(int index, Vec2[] array, int arrayIndex, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Value is less than zero");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Value is less than zero");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", "Value is less than zero");
			}
			if (array.Rank > 1)
			{
				throw new ArgumentException("Multi dimensional array.", "array");
			}
			if (index + count > this.Count || arrayIndex + count > array.Length)
			{
				throw new ArgumentException("Number of elements to copy is too large.");
			}
			for (int i = 0; i < count; i++)
			{
				array.SetValue(this.getitemcopy(index + i), arrayIndex + i);
			}
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00013EBC File Offset: 0x000120BC
		IEnumerator<Vec2> IEnumerable<Vec2>.GetEnumerator()
		{
			return new CSVectorPoint.CSVectorPointEnumerator(this);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00013ED4 File Offset: 0x000120D4
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CSVectorPoint.CSVectorPointEnumerator(this);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00013EEC File Offset: 0x000120EC
		public CSVectorPoint.CSVectorPointEnumerator GetEnumerator()
		{
			return new CSVectorPoint.CSVectorPointEnumerator(this);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00013F04 File Offset: 0x00012104
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_Clear(this.swigCPtr);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00013F14 File Offset: 0x00012114
		public void Add(Vec2 x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_Add(this.swigCPtr, Vec2.getCPtr(x));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00013F48 File Offset: 0x00012148
		private uint size()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorPoint_size(this.swigCPtr);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00013F68 File Offset: 0x00012168
		private uint capacity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorPoint_capacity(this.swigCPtr);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00013F87 File Offset: 0x00012187
		private void reserve(uint n)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_reserve(this.swigCPtr, n);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00013F97 File Offset: 0x00012197
		public CSVectorPoint() : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorPoint__SWIG_0(), true)
		{
		}

		// Token: 0x06000B9A RID: 2970 RVA: 0x00013FA8 File Offset: 0x000121A8
		public CSVectorPoint(CSVectorPoint other) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorPoint__SWIG_1(CSVectorPoint.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B9B RID: 2971 RVA: 0x00013FDC File Offset: 0x000121DC
		public CSVectorPoint(int capacity) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorPoint__SWIG_2(capacity), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0001400C File Offset: 0x0001220C
		private Vec2 getitemcopy(int index)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.CSVectorPoint_getitemcopy(this.swigCPtr, index), true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B9D RID: 2973 RVA: 0x00014044 File Offset: 0x00012244
		private Vec2 getitem(int index)
		{
			Vec2 result = new Vec2(CocoStudioEngineAdapterPINVOKE.CSVectorPoint_getitem(this.swigCPtr, index), false);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B9E RID: 2974 RVA: 0x0001407C File Offset: 0x0001227C
		private void setitem(int index, Vec2 val)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_setitem(this.swigCPtr, index, Vec2.getCPtr(val));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B9F RID: 2975 RVA: 0x000140B0 File Offset: 0x000122B0
		public void AddRange(CSVectorPoint values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_AddRange(this.swigCPtr, CSVectorPoint.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x000140E4 File Offset: 0x000122E4
		public CSVectorPoint GetRange(int index, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorPoint_GetRange(this.swigCPtr, index, count);
			CSVectorPoint result = (intPtr == IntPtr.Zero) ? null : new CSVectorPoint(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x00014130 File Offset: 0x00012330
		public void Insert(int index, Vec2 x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_Insert(this.swigCPtr, index, Vec2.getCPtr(x));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x00014164 File Offset: 0x00012364
		public void InsertRange(int index, CSVectorPoint values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_InsertRange(this.swigCPtr, index, CSVectorPoint.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x00014198 File Offset: 0x00012398
		public void RemoveAt(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_RemoveAt(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x000141C8 File Offset: 0x000123C8
		public void RemoveRange(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_RemoveRange(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x000141F8 File Offset: 0x000123F8
		public static CSVectorPoint Repeat(Vec2 value, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorPoint_Repeat(Vec2.getCPtr(value), count);
			CSVectorPoint result = (intPtr == IntPtr.Zero) ? null : new CSVectorPoint(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x00014243 File Offset: 0x00012443
		public void Reverse()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_Reverse__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x00014254 File Offset: 0x00012454
		public void Reverse(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00014284 File Offset: 0x00012484
		public void SetRange(int index, CSVectorPoint values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorPoint_SetRange(this.swigCPtr, index, CSVectorPoint.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x040000B7 RID: 183
		private HandleRef swigCPtr;

		// Token: 0x040000B8 RID: 184
		protected bool swigCMemOwn;

		// Token: 0x02000066 RID: 102
		public sealed class CSVectorPointEnumerator : IEnumerator<Vec2>, IDisposable, IEnumerator
		{
			// Token: 0x06000BA9 RID: 2985 RVA: 0x000142B7 File Offset: 0x000124B7
			public CSVectorPointEnumerator(CSVectorPoint collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			// Token: 0x1700005D RID: 93
			// (get) Token: 0x06000BAA RID: 2986 RVA: 0x000142E8 File Offset: 0x000124E8
			public Vec2 Current
			{
				get
				{
					if (this.currentIndex == -1)
					{
						throw new InvalidOperationException("Enumeration not started.");
					}
					if (this.currentIndex > this.currentSize - 1)
					{
						throw new InvalidOperationException("Enumeration finished.");
					}
					if (this.currentObject == null)
					{
						throw new InvalidOperationException("Collection modified.");
					}
					return (Vec2)this.currentObject;
				}
			}

			// Token: 0x1700005E RID: 94
			// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00014360 File Offset: 0x00012560
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000BAC RID: 2988 RVA: 0x00014378 File Offset: 0x00012578
			public bool MoveNext()
			{
				int count = this.collectionRef.Count;
				bool flag = this.currentIndex + 1 < count && count == this.currentSize;
				if (flag)
				{
					this.currentIndex++;
					this.currentObject = this.collectionRef[this.currentIndex];
				}
				else
				{
					this.currentObject = null;
				}
				return flag;
			}

			// Token: 0x06000BAD RID: 2989 RVA: 0x000143EC File Offset: 0x000125EC
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			// Token: 0x06000BAE RID: 2990 RVA: 0x0001442B File Offset: 0x0001262B
			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}

			// Token: 0x040000B9 RID: 185
			private CSVectorPoint collectionRef;

			// Token: 0x040000BA RID: 186
			private int currentIndex;

			// Token: 0x040000BB RID: 187
			private object currentObject;

			// Token: 0x040000BC RID: 188
			private int currentSize;
		}
	}
}
