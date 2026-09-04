using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CocoStudio.EngineAdapterWrap.Extend;
using CocoStudio.Model;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000067 RID: 103
	public class CSVectorRect : IDisposable, IEnumerable<RectF>, IEnumerable
	{
		// Token: 0x06000BAF RID: 2991 RVA: 0x0001443C File Offset: 0x0001263C
		public CSVectorRect(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0001445C File Offset: 0x0001265C
		public static HandleRef getCPtr(CSVectorRect obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x00014488 File Offset: 0x00012688
		~CSVectorRect()
		{
			this.Dispose();
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x000144EC File Offset: 0x000126EC
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
								CocoStudioEngineAdapterPINVOKE.delete_CSVectorRect(this.swigCPtr);
							});
						}
						else
						{
							CocoStudioEngineAdapterPINVOKE.delete_CSVectorRect(this.swigCPtr);
						}
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x000145E4 File Offset: 0x000127E4
		public CSVectorRect(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			foreach (object obj in c)
			{
				RectF x = (RectF)obj;
				this.Add(x);
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x00014664 File Offset: 0x00012864
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00014678 File Offset: 0x00012878
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000061 RID: 97
		public RectF this[int index]
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

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x000146B4 File Offset: 0x000128B4
		// (set) Token: 0x06000BB9 RID: 3001 RVA: 0x000146CC File Offset: 0x000128CC
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

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x00014700 File Offset: 0x00012900
		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x00014718 File Offset: 0x00012918
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0001472B File Offset: 0x0001292B
		public void CopyTo(RectF[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0001473E File Offset: 0x0001293E
		public void CopyTo(RectF[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00014754 File Offset: 0x00012954
		public void CopyTo(int index, RectF[] array, int arrayIndex, int count)
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

		// Token: 0x06000BBF RID: 3007 RVA: 0x0001483C File Offset: 0x00012A3C
		IEnumerator<RectF> IEnumerable<RectF>.GetEnumerator()
		{
			return new CSVectorRect.CSVectorRectEnumerator(this);
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00014854 File Offset: 0x00012A54
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CSVectorRect.CSVectorRectEnumerator(this);
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0001486C File Offset: 0x00012A6C
		public CSVectorRect.CSVectorRectEnumerator GetEnumerator()
		{
			return new CSVectorRect.CSVectorRectEnumerator(this);
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00014884 File Offset: 0x00012A84
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_Clear(this.swigCPtr);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00014894 File Offset: 0x00012A94
		public void Add(RectF x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_Add(this.swigCPtr, Rect.getCPtr(new Rect(x.X, x.Y, x.Width, x.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x000148E4 File Offset: 0x00012AE4
		private uint size()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorRect_size(this.swigCPtr);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00014904 File Offset: 0x00012B04
		private uint capacity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorRect_capacity(this.swigCPtr);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00014923 File Offset: 0x00012B23
		private void reserve(uint n)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_reserve(this.swigCPtr, n);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00014933 File Offset: 0x00012B33
		public CSVectorRect() : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorRect__SWIG_0(), true)
		{
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x00014944 File Offset: 0x00012B44
		public CSVectorRect(CSVectorRect other) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorRect__SWIG_1(CSVectorRect.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x00014978 File Offset: 0x00012B78
		public CSVectorRect(int capacity) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorRect__SWIG_2(capacity), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x000149A8 File Offset: 0x00012BA8
		private RectF getitemcopy(int index)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVectorRect_getitemcopy(this.swigCPtr, index);
			Rect rect = new Rect(cPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			if (rect.size.width < 0f || rect.size.height < 0f)
			{
				rect.origin.x = 0f;
				rect.origin.y = 0f;
				rect.size.width = 0f;
				rect.size.height = 0f;
			}
			return new RectF(rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x00014A90 File Offset: 0x00012C90
		private RectF getitem(int index)
		{
			IntPtr cPtr = CocoStudioEngineAdapterPINVOKE.CSVectorRect_getitem(this.swigCPtr, index);
			Rect rect = new Rect(cPtr, false);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			if (rect.size.width < 0f || rect.size.height < 0f)
			{
				rect.origin.x = 0f;
				rect.origin.y = 0f;
				rect.size.width = 0f;
				rect.size.height = 0f;
			}
			return new RectF(rect.origin.x, rect.origin.y, rect.size.width, rect.size.height);
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x00014B78 File Offset: 0x00012D78
		private void setitem(int index, RectF val)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_setitem(this.swigCPtr, index, Rect.getCPtr(new Rect(val.X, val.Y, val.Width, val.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x00014BC8 File Offset: 0x00012DC8
		public void AddRange(CSVectorRect values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_AddRange(this.swigCPtr, CSVectorRect.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x00014BFC File Offset: 0x00012DFC
		public CSVectorRect GetRange(int index, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorRect_GetRange(this.swigCPtr, index, count);
			CSVectorRect result = (intPtr == IntPtr.Zero) ? null : new CSVectorRect(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x00014C48 File Offset: 0x00012E48
		public void Insert(int index, RectF x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_Insert(this.swigCPtr, index, Rect.getCPtr(new Rect(x.X, x.Y, x.Width, x.Height)));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x00014C98 File Offset: 0x00012E98
		public void InsertRange(int index, CSVectorRect values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_InsertRange(this.swigCPtr, index, CSVectorRect.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x00014CCC File Offset: 0x00012ECC
		public void RemoveAt(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_RemoveAt(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x00014CFC File Offset: 0x00012EFC
		public void RemoveRange(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_RemoveRange(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00014D2C File Offset: 0x00012F2C
		public static CSVectorRect Repeat(RectF value, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorRect_Repeat(Rect.getCPtr(new Rect(value.X, value.Y, value.Width, value.Height)), count);
			CSVectorRect result = (intPtr == IntPtr.Zero) ? null : new CSVectorRect(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x00014D93 File Offset: 0x00012F93
		public void Reverse()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_Reverse__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x00014DA4 File Offset: 0x00012FA4
		public void Reverse(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x00014DD4 File Offset: 0x00012FD4
		public void SetRange(int index, CSVectorRect values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorRect_SetRange(this.swigCPtr, index, CSVectorRect.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x040000BD RID: 189
		private HandleRef swigCPtr;

		// Token: 0x040000BE RID: 190
		protected bool swigCMemOwn;

		// Token: 0x02000068 RID: 104
		public sealed class CSVectorRectEnumerator : IEnumerator<RectF>, IDisposable, IEnumerator
		{
			// Token: 0x06000BD7 RID: 3031 RVA: 0x00014E07 File Offset: 0x00013007
			public CSVectorRectEnumerator(CSVectorRect collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			// Token: 0x17000065 RID: 101
			// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00014E38 File Offset: 0x00013038
			public RectF Current
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
					return (RectF)this.currentObject;
				}
			}

			// Token: 0x17000066 RID: 102
			// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x00014EB0 File Offset: 0x000130B0
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000BDA RID: 3034 RVA: 0x00014EC8 File Offset: 0x000130C8
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

			// Token: 0x06000BDB RID: 3035 RVA: 0x00014F3C File Offset: 0x0001313C
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			// Token: 0x06000BDC RID: 3036 RVA: 0x00014F7B File Offset: 0x0001317B
			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}

			// Token: 0x040000BF RID: 191
			private CSVectorRect collectionRef;

			// Token: 0x040000C0 RID: 192
			private int currentIndex;

			// Token: 0x040000C1 RID: 193
			private object currentObject;

			// Token: 0x040000C2 RID: 194
			private int currentSize;
		}
	}
}
