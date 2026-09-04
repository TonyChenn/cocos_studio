using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000061 RID: 97
	public class CSVectorFloat : IDisposable, IList<float>, ICollection<float>, IEnumerable<float>, IEnumerable
	{
		// Token: 0x06000B1D RID: 2845 RVA: 0x00012824 File Offset: 0x00010A24
		public CSVectorFloat(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x00012844 File Offset: 0x00010A44
		public static HandleRef getCPtr(CSVectorFloat obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x00012870 File Offset: 0x00010A70
		~CSVectorFloat()
		{
			this.Dispose();
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x000128A4 File Offset: 0x00010AA4
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						CocoStudioEngineAdapterPINVOKE.delete_CSVectorFloat(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0001293C File Offset: 0x00010B3C
		public CSVectorFloat(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			foreach (object obj in c)
			{
				float x = (float)obj;
				this.Add(x);
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x000129BC File Offset: 0x00010BBC
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x000129D0 File Offset: 0x00010BD0
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000049 RID: 73
		public float this[int index]
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

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x00012A0C File Offset: 0x00010C0C
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x00012A24 File Offset: 0x00010C24
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

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x00012A58 File Offset: 0x00010C58
		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00012A70 File Offset: 0x00010C70
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x00012A83 File Offset: 0x00010C83
		public void CopyTo(float[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x00012A96 File Offset: 0x00010C96
		public void CopyTo(float[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00012AAC File Offset: 0x00010CAC
		public void CopyTo(int index, float[] array, int arrayIndex, int count)
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

		// Token: 0x06000B2D RID: 2861 RVA: 0x00012B98 File Offset: 0x00010D98
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			return new CSVectorFloat.CSVectorFloatEnumerator(this);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00012BB0 File Offset: 0x00010DB0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CSVectorFloat.CSVectorFloatEnumerator(this);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00012BC8 File Offset: 0x00010DC8
		public CSVectorFloat.CSVectorFloatEnumerator GetEnumerator()
		{
			return new CSVectorFloat.CSVectorFloatEnumerator(this);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x00012BE0 File Offset: 0x00010DE0
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Clear(this.swigCPtr);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00012BEF File Offset: 0x00010DEF
		public void Add(float x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Add(this.swigCPtr, x);
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00012C00 File Offset: 0x00010E00
		private uint size()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorFloat_size(this.swigCPtr);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00012C20 File Offset: 0x00010E20
		private uint capacity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorFloat_capacity(this.swigCPtr);
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00012C3F File Offset: 0x00010E3F
		private void reserve(uint n)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_reserve(this.swigCPtr, n);
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00012C4F File Offset: 0x00010E4F
		public CSVectorFloat() : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorFloat__SWIG_0(), true)
		{
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00012C60 File Offset: 0x00010E60
		public CSVectorFloat(CSVectorFloat other) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorFloat__SWIG_1(CSVectorFloat.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x00012C94 File Offset: 0x00010E94
		public CSVectorFloat(int capacity) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorFloat__SWIG_2(capacity), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x00012CC4 File Offset: 0x00010EC4
		private float getitemcopy(int index)
		{
			float result = CocoStudioEngineAdapterPINVOKE.CSVectorFloat_getitemcopy(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x00012CF8 File Offset: 0x00010EF8
		private float getitem(int index)
		{
			float result = CocoStudioEngineAdapterPINVOKE.CSVectorFloat_getitem(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00012D2C File Offset: 0x00010F2C
		private void setitem(int index, float val)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_setitem(this.swigCPtr, index, val);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00012D5C File Offset: 0x00010F5C
		public void AddRange(CSVectorFloat values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_AddRange(this.swigCPtr, CSVectorFloat.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00012D90 File Offset: 0x00010F90
		public CSVectorFloat GetRange(int index, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorFloat_GetRange(this.swigCPtr, index, count);
			CSVectorFloat result = (intPtr == IntPtr.Zero) ? null : new CSVectorFloat(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00012DDC File Offset: 0x00010FDC
		public void Insert(int index, float x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Insert(this.swigCPtr, index, x);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x00012E0C File Offset: 0x0001100C
		public void InsertRange(int index, CSVectorFloat values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_InsertRange(this.swigCPtr, index, CSVectorFloat.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00012E40 File Offset: 0x00011040
		public void RemoveAt(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_RemoveAt(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x00012E70 File Offset: 0x00011070
		public void RemoveRange(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_RemoveRange(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00012EA0 File Offset: 0x000110A0
		public static CSVectorFloat Repeat(float value, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Repeat(value, count);
			CSVectorFloat result = (intPtr == IntPtr.Zero) ? null : new CSVectorFloat(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00012EE6 File Offset: 0x000110E6
		public void Reverse()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Reverse__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00012EF8 File Offset: 0x000110F8
		public void Reverse(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00012F28 File Offset: 0x00011128
		public void SetRange(int index, CSVectorFloat values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorFloat_SetRange(this.swigCPtr, index, CSVectorFloat.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00012F5C File Offset: 0x0001115C
		public bool Contains(float value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Contains(this.swigCPtr, value);
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00012F7C File Offset: 0x0001117C
		public int IndexOf(float value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorFloat_IndexOf(this.swigCPtr, value);
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00012F9C File Offset: 0x0001119C
		public int LastIndexOf(float value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorFloat_LastIndexOf(this.swigCPtr, value);
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00012FBC File Offset: 0x000111BC
		public bool Remove(float value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorFloat_Remove(this.swigCPtr, value);
		}

		// Token: 0x040000AB RID: 171
		private HandleRef swigCPtr;

		// Token: 0x040000AC RID: 172
		protected bool swigCMemOwn;

		// Token: 0x02000062 RID: 98
		public sealed class CSVectorFloatEnumerator : IEnumerator<float>, IDisposable, IEnumerator
		{
			// Token: 0x06000B49 RID: 2889 RVA: 0x00012FDC File Offset: 0x000111DC
			public CSVectorFloatEnumerator(CSVectorFloat collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			// Token: 0x1700004D RID: 77
			// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00013010 File Offset: 0x00011210
			public float Current
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
					return (float)this.currentObject;
				}
			}

			// Token: 0x1700004E RID: 78
			// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00013088 File Offset: 0x00011288
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000B4C RID: 2892 RVA: 0x000130A8 File Offset: 0x000112A8
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

			// Token: 0x06000B4D RID: 2893 RVA: 0x00013120 File Offset: 0x00011320
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			// Token: 0x06000B4E RID: 2894 RVA: 0x0001315F File Offset: 0x0001135F
			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}

			// Token: 0x040000AD RID: 173
			private CSVectorFloat collectionRef;

			// Token: 0x040000AE RID: 174
			private int currentIndex;

			// Token: 0x040000AF RID: 175
			private object currentObject;

			// Token: 0x040000B0 RID: 176
			private int currentSize;
		}
	}
}
