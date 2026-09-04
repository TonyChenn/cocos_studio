using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x0200005F RID: 95
	public class CSVectorDouble : IDisposable, IList<double>, ICollection<double>, IEnumerable<double>, IEnumerable
	{
		// Token: 0x06000AEB RID: 2795 RVA: 0x00011ED6 File Offset: 0x000100D6
		public CSVectorDouble(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00011EF8 File Offset: 0x000100F8
		public static HandleRef getCPtr(CSVectorDouble obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00011F24 File Offset: 0x00010124
		~CSVectorDouble()
		{
			this.Dispose();
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00011F58 File Offset: 0x00010158
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						CocoStudioEngineAdapterPINVOKE.delete_CSVectorDouble(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x00011FF0 File Offset: 0x000101F0
		public CSVectorDouble(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			foreach (object obj in c)
			{
				double x = (double)obj;
				this.Add(x);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00012070 File Offset: 0x00010270
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00012084 File Offset: 0x00010284
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000041 RID: 65
		public double this[int index]
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

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000120C0 File Offset: 0x000102C0
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x000120D8 File Offset: 0x000102D8
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x0001210C File Offset: 0x0001030C
		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00012124 File Offset: 0x00010324
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x00012137 File Offset: 0x00010337
		public void CopyTo(double[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0001214A File Offset: 0x0001034A
		public void CopyTo(double[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00012160 File Offset: 0x00010360
		public void CopyTo(int index, double[] array, int arrayIndex, int count)
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

		// Token: 0x06000AFB RID: 2811 RVA: 0x0001224C File Offset: 0x0001044C
		IEnumerator<double> IEnumerable<double>.GetEnumerator()
		{
			return new CSVectorDouble.CSVectorDoubleEnumerator(this);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00012264 File Offset: 0x00010464
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CSVectorDouble.CSVectorDoubleEnumerator(this);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x0001227C File Offset: 0x0001047C
		public CSVectorDouble.CSVectorDoubleEnumerator GetEnumerator()
		{
			return new CSVectorDouble.CSVectorDoubleEnumerator(this);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00012294 File Offset: 0x00010494
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Clear(this.swigCPtr);
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x000122A3 File Offset: 0x000104A3
		public void Add(double x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Add(this.swigCPtr, x);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000122B4 File Offset: 0x000104B4
		private uint size()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorDouble_size(this.swigCPtr);
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x000122D4 File Offset: 0x000104D4
		private uint capacity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorDouble_capacity(this.swigCPtr);
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x000122F3 File Offset: 0x000104F3
		private void reserve(uint n)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_reserve(this.swigCPtr, n);
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00012303 File Offset: 0x00010503
		public CSVectorDouble() : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorDouble__SWIG_0(), true)
		{
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00012314 File Offset: 0x00010514
		public CSVectorDouble(CSVectorDouble other) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorDouble__SWIG_1(CSVectorDouble.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00012348 File Offset: 0x00010548
		public CSVectorDouble(int capacity) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorDouble__SWIG_2(capacity), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00012378 File Offset: 0x00010578
		private double getitemcopy(int index)
		{
			double result = CocoStudioEngineAdapterPINVOKE.CSVectorDouble_getitemcopy(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x000123AC File Offset: 0x000105AC
		private double getitem(int index)
		{
			double result = CocoStudioEngineAdapterPINVOKE.CSVectorDouble_getitem(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x000123E0 File Offset: 0x000105E0
		private void setitem(int index, double val)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_setitem(this.swigCPtr, index, val);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00012410 File Offset: 0x00010610
		public void AddRange(CSVectorDouble values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_AddRange(this.swigCPtr, CSVectorDouble.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00012444 File Offset: 0x00010644
		public CSVectorDouble GetRange(int index, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorDouble_GetRange(this.swigCPtr, index, count);
			CSVectorDouble result = (intPtr == IntPtr.Zero) ? null : new CSVectorDouble(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00012490 File Offset: 0x00010690
		public void Insert(int index, double x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Insert(this.swigCPtr, index, x);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B0C RID: 2828 RVA: 0x000124C0 File Offset: 0x000106C0
		public void InsertRange(int index, CSVectorDouble values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_InsertRange(this.swigCPtr, index, CSVectorDouble.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x000124F4 File Offset: 0x000106F4
		public void RemoveAt(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_RemoveAt(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x00012524 File Offset: 0x00010724
		public void RemoveRange(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_RemoveRange(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00012554 File Offset: 0x00010754
		public static CSVectorDouble Repeat(double value, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Repeat(value, count);
			CSVectorDouble result = (intPtr == IntPtr.Zero) ? null : new CSVectorDouble(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x0001259A File Offset: 0x0001079A
		public void Reverse()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Reverse__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000125AC File Offset: 0x000107AC
		public void Reverse(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000125DC File Offset: 0x000107DC
		public void SetRange(int index, CSVectorDouble values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorDouble_SetRange(this.swigCPtr, index, CSVectorDouble.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00012610 File Offset: 0x00010810
		public bool Contains(double value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Contains(this.swigCPtr, value);
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00012630 File Offset: 0x00010830
		public int IndexOf(double value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorDouble_IndexOf(this.swigCPtr, value);
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00012650 File Offset: 0x00010850
		public int LastIndexOf(double value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorDouble_LastIndexOf(this.swigCPtr, value);
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00012670 File Offset: 0x00010870
		public bool Remove(double value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorDouble_Remove(this.swigCPtr, value);
		}

		// Token: 0x040000A5 RID: 165
		private HandleRef swigCPtr;

		// Token: 0x040000A6 RID: 166
		protected bool swigCMemOwn;

		// Token: 0x02000060 RID: 96
		public sealed class CSVectorDoubleEnumerator : IEnumerator<double>, IDisposable, IEnumerator
		{
			// Token: 0x06000B17 RID: 2839 RVA: 0x00012690 File Offset: 0x00010890
			public CSVectorDoubleEnumerator(CSVectorDouble collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			// Token: 0x17000045 RID: 69
			// (get) Token: 0x06000B18 RID: 2840 RVA: 0x000126C4 File Offset: 0x000108C4
			public double Current
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
					return (double)this.currentObject;
				}
			}

			// Token: 0x17000046 RID: 70
			// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0001273C File Offset: 0x0001093C
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000B1A RID: 2842 RVA: 0x0001275C File Offset: 0x0001095C
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

			// Token: 0x06000B1B RID: 2843 RVA: 0x000127D4 File Offset: 0x000109D4
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			// Token: 0x06000B1C RID: 2844 RVA: 0x00012813 File Offset: 0x00010A13
			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}

			// Token: 0x040000A7 RID: 167
			private CSVectorDouble collectionRef;

			// Token: 0x040000A8 RID: 168
			private int currentIndex;

			// Token: 0x040000A9 RID: 169
			private object currentObject;

			// Token: 0x040000AA RID: 170
			private int currentSize;
		}
	}
}
