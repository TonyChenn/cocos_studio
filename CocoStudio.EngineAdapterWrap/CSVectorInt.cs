using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000063 RID: 99
	public class CSVectorInt : IDisposable, IList<int>, ICollection<int>, IEnumerable<int>, IEnumerable
	{
		// Token: 0x06000B4F RID: 2895 RVA: 0x00013170 File Offset: 0x00011370
		public CSVectorInt(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00013190 File Offset: 0x00011390
		public static HandleRef getCPtr(CSVectorInt obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x000131BC File Offset: 0x000113BC
		~CSVectorInt()
		{
			this.Dispose();
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x000131F0 File Offset: 0x000113F0
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						CocoStudioEngineAdapterPINVOKE.delete_CSVectorInt(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x00013288 File Offset: 0x00011488
		public CSVectorInt(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			foreach (object obj in c)
			{
				int x = (int)obj;
				this.Add(x);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00013308 File Offset: 0x00011508
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000B55 RID: 2901 RVA: 0x0001331C File Offset: 0x0001151C
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000051 RID: 81
		public int this[int index]
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00013358 File Offset: 0x00011558
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x00013370 File Offset: 0x00011570
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

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x000133A4 File Offset: 0x000115A4
		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000B5B RID: 2907 RVA: 0x000133BC File Offset: 0x000115BC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x000133CF File Offset: 0x000115CF
		public void CopyTo(int[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000133E2 File Offset: 0x000115E2
		public void CopyTo(int[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000133F8 File Offset: 0x000115F8
		public void CopyTo(int index, int[] array, int arrayIndex, int count)
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

		// Token: 0x06000B5F RID: 2911 RVA: 0x000134E4 File Offset: 0x000116E4
		IEnumerator<int> IEnumerable<int>.GetEnumerator()
		{
			return new CSVectorInt.CSVectorIntEnumerator(this);
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x000134FC File Offset: 0x000116FC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CSVectorInt.CSVectorIntEnumerator(this);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00013514 File Offset: 0x00011714
		public CSVectorInt.CSVectorIntEnumerator GetEnumerator()
		{
			return new CSVectorInt.CSVectorIntEnumerator(this);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0001352C File Offset: 0x0001172C
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_Clear(this.swigCPtr);
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x0001353B File Offset: 0x0001173B
		public void Add(int x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_Add(this.swigCPtr, x);
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0001354C File Offset: 0x0001174C
		private uint size()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorInt_size(this.swigCPtr);
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x0001356C File Offset: 0x0001176C
		private uint capacity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorInt_capacity(this.swigCPtr);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x0001358B File Offset: 0x0001178B
		private void reserve(uint n)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_reserve(this.swigCPtr, n);
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x0001359B File Offset: 0x0001179B
		public CSVectorInt() : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorInt__SWIG_0(), true)
		{
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000135AC File Offset: 0x000117AC
		public CSVectorInt(CSVectorInt other) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorInt__SWIG_1(CSVectorInt.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x000135E0 File Offset: 0x000117E0
		public CSVectorInt(int capacity) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorInt__SWIG_2(capacity), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00013610 File Offset: 0x00011810
		private int getitemcopy(int index)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSVectorInt_getitemcopy(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00013644 File Offset: 0x00011844
		private int getitem(int index)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSVectorInt_getitem(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00013678 File Offset: 0x00011878
		private void setitem(int index, int val)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_setitem(this.swigCPtr, index, val);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x000136A8 File Offset: 0x000118A8
		public void AddRange(CSVectorInt values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_AddRange(this.swigCPtr, CSVectorInt.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x000136DC File Offset: 0x000118DC
		public CSVectorInt GetRange(int index, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorInt_GetRange(this.swigCPtr, index, count);
			CSVectorInt result = (intPtr == IntPtr.Zero) ? null : new CSVectorInt(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00013728 File Offset: 0x00011928
		public void Insert(int index, int x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_Insert(this.swigCPtr, index, x);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00013758 File Offset: 0x00011958
		public void InsertRange(int index, CSVectorInt values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_InsertRange(this.swigCPtr, index, CSVectorInt.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x0001378C File Offset: 0x0001198C
		public void RemoveAt(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_RemoveAt(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x000137BC File Offset: 0x000119BC
		public void RemoveRange(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_RemoveRange(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x000137EC File Offset: 0x000119EC
		public static CSVectorInt Repeat(int value, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorInt_Repeat(value, count);
			CSVectorInt result = (intPtr == IntPtr.Zero) ? null : new CSVectorInt(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00013832 File Offset: 0x00011A32
		public void Reverse()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_Reverse__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x00013844 File Offset: 0x00011A44
		public void Reverse(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x00013874 File Offset: 0x00011A74
		public void SetRange(int index, CSVectorInt values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorInt_SetRange(this.swigCPtr, index, CSVectorInt.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000138A8 File Offset: 0x00011AA8
		public bool Contains(int value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorInt_Contains(this.swigCPtr, value);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000138C8 File Offset: 0x00011AC8
		public int IndexOf(int value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorInt_IndexOf(this.swigCPtr, value);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x000138E8 File Offset: 0x00011AE8
		public int LastIndexOf(int value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorInt_LastIndexOf(this.swigCPtr, value);
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00013908 File Offset: 0x00011B08
		public bool Remove(int value)
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorInt_Remove(this.swigCPtr, value);
		}

		// Token: 0x040000B1 RID: 177
		private HandleRef swigCPtr;

		// Token: 0x040000B2 RID: 178
		protected bool swigCMemOwn;

		// Token: 0x02000064 RID: 100
		public sealed class CSVectorIntEnumerator : IEnumerator<int>, IDisposable, IEnumerator
		{
			// Token: 0x06000B7B RID: 2939 RVA: 0x00013928 File Offset: 0x00011B28
			public CSVectorIntEnumerator(CSVectorInt collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			// Token: 0x17000055 RID: 85
			// (get) Token: 0x06000B7C RID: 2940 RVA: 0x0001395C File Offset: 0x00011B5C
			public int Current
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
					return (int)this.currentObject;
				}
			}

			// Token: 0x17000056 RID: 86
			// (get) Token: 0x06000B7D RID: 2941 RVA: 0x000139D4 File Offset: 0x00011BD4
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000B7E RID: 2942 RVA: 0x000139F4 File Offset: 0x00011BF4
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

			// Token: 0x06000B7F RID: 2943 RVA: 0x00013A6C File Offset: 0x00011C6C
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			// Token: 0x06000B80 RID: 2944 RVA: 0x00013AAB File Offset: 0x00011CAB
			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}

			// Token: 0x040000B3 RID: 179
			private CSVectorInt collectionRef;

			// Token: 0x040000B4 RID: 180
			private int currentIndex;

			// Token: 0x040000B5 RID: 181
			private object currentObject;

			// Token: 0x040000B6 RID: 182
			private int currentSize;
		}
	}
}
