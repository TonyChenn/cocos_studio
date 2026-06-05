using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace CocoStudio.EngineAdapterWrap
{
	// Token: 0x02000069 RID: 105
	public class CSVectorString : IDisposable, IList<string>, ICollection<string>, IEnumerable<string>, IEnumerable
	{
		// Token: 0x06000BDD RID: 3037 RVA: 0x00014F8C File Offset: 0x0001318C
		public CSVectorString(IntPtr cPtr, bool cMemoryOwn)
		{
			this.swigCMemOwn = cMemoryOwn;
			this.swigCPtr = new HandleRef(this, cPtr);
		}

		// Token: 0x06000BDE RID: 3038 RVA: 0x00014FAC File Offset: 0x000131AC
		public static HandleRef getCPtr(CSVectorString obj)
		{
			return (obj == null) ? new HandleRef(null, IntPtr.Zero) : obj.swigCPtr;
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00014FD8 File Offset: 0x000131D8
		~CSVectorString()
		{
			this.Dispose();
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0001500C File Offset: 0x0001320C
		public virtual void Dispose()
		{
			lock (this)
			{
				if (this.swigCPtr.Handle != IntPtr.Zero)
				{
					if (this.swigCMemOwn)
					{
						this.swigCMemOwn = false;
						CocoStudioEngineAdapterPINVOKE.delete_CSVectorString(this.swigCPtr);
					}
					this.swigCPtr = new HandleRef(null, IntPtr.Zero);
				}
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x000150A4 File Offset: 0x000132A4
		public CSVectorString(ICollection c) : this()
		{
			if (c == null)
			{
				throw new ArgumentNullException("c");
			}
			foreach (object obj in c)
			{
				string x = (string)obj;
				this.Add(x);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000BE2 RID: 3042 RVA: 0x00015124 File Offset: 0x00013324
		public bool IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x00015138 File Offset: 0x00013338
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000069 RID: 105
		public string this[int index]
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

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00015174 File Offset: 0x00013374
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0001518C File Offset: 0x0001338C
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

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x000151C0 File Offset: 0x000133C0
		public int Count
		{
			get
			{
				return (int)this.size();
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x000151D8 File Offset: 0x000133D8
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000BEA RID: 3050 RVA: 0x000151EB File Offset: 0x000133EB
		public void CopyTo(string[] array)
		{
			this.CopyTo(0, array, 0, this.Count);
		}

		// Token: 0x06000BEB RID: 3051 RVA: 0x000151FE File Offset: 0x000133FE
		public void CopyTo(string[] array, int arrayIndex)
		{
			this.CopyTo(0, array, arrayIndex, this.Count);
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x00015214 File Offset: 0x00013414
		public void CopyTo(int index, string[] array, int arrayIndex, int count)
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

		// Token: 0x06000BED RID: 3053 RVA: 0x000152FC File Offset: 0x000134FC
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			return new CSVectorString.CSVectorStringEnumerator(this);
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x00015314 File Offset: 0x00013514
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CSVectorString.CSVectorStringEnumerator(this);
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0001532C File Offset: 0x0001352C
		public CSVectorString.CSVectorStringEnumerator GetEnumerator()
		{
			return new CSVectorString.CSVectorStringEnumerator(this);
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00015344 File Offset: 0x00013544
		public void Clear()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_Clear(this.swigCPtr);
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00015354 File Offset: 0x00013554
		public void Add(string x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_Add(this.swigCPtr, x);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00015384 File Offset: 0x00013584
		private uint size()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorString_size(this.swigCPtr);
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x000153A4 File Offset: 0x000135A4
		private uint capacity()
		{
			return CocoStudioEngineAdapterPINVOKE.CSVectorString_capacity(this.swigCPtr);
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x000153C3 File Offset: 0x000135C3
		private void reserve(uint n)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_reserve(this.swigCPtr, n);
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x000153D3 File Offset: 0x000135D3
		public CSVectorString() : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorString__SWIG_0(), true)
		{
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x000153E4 File Offset: 0x000135E4
		public CSVectorString(CSVectorString other) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorString__SWIG_1(CSVectorString.getCPtr(other)), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x00015418 File Offset: 0x00013618
		public CSVectorString(int capacity) : this(CocoStudioEngineAdapterPINVOKE.new_CSVectorString__SWIG_2(capacity), true)
		{
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BF8 RID: 3064 RVA: 0x00015448 File Offset: 0x00013648
		private string getitemcopy(int index)
		{
			string result = CocoStudioEngineAdapterPINVOKE.CSVectorString_getitemcopy(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0001547C File Offset: 0x0001367C
		private string getitem(int index)
		{
			string result = CocoStudioEngineAdapterPINVOKE.CSVectorString_getitem(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x000154B0 File Offset: 0x000136B0
		private void setitem(int index, string val)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_setitem(this.swigCPtr, index, val);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x000154E0 File Offset: 0x000136E0
		public void AddRange(CSVectorString values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_AddRange(this.swigCPtr, CSVectorString.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x00015514 File Offset: 0x00013714
		public CSVectorString GetRange(int index, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorString_GetRange(this.swigCPtr, index, count);
			CSVectorString result = (intPtr == IntPtr.Zero) ? null : new CSVectorString(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x00015560 File Offset: 0x00013760
		public void Insert(int index, string x)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_Insert(this.swigCPtr, index, x);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x00015590 File Offset: 0x00013790
		public void InsertRange(int index, CSVectorString values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_InsertRange(this.swigCPtr, index, CSVectorString.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000BFF RID: 3071 RVA: 0x000155C4 File Offset: 0x000137C4
		public void RemoveAt(int index)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_RemoveAt(this.swigCPtr, index);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C00 RID: 3072 RVA: 0x000155F4 File Offset: 0x000137F4
		public void RemoveRange(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_RemoveRange(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x00015624 File Offset: 0x00013824
		public static CSVectorString Repeat(string value, int count)
		{
			IntPtr intPtr = CocoStudioEngineAdapterPINVOKE.CSVectorString_Repeat(value, count);
			CSVectorString result = (intPtr == IntPtr.Zero) ? null : new CSVectorString(intPtr, true);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0001566A File Offset: 0x0001386A
		public void Reverse()
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_Reverse__SWIG_0(this.swigCPtr);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0001567C File Offset: 0x0001387C
		public void Reverse(int index, int count)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_Reverse__SWIG_1(this.swigCPtr, index, count);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x000156AC File Offset: 0x000138AC
		public void SetRange(int index, CSVectorString values)
		{
			CocoStudioEngineAdapterPINVOKE.CSVectorString_SetRange(this.swigCPtr, index, CSVectorString.getCPtr(values));
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x000156E0 File Offset: 0x000138E0
		public bool Contains(string value)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSVectorString_Contains(this.swigCPtr, value);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00015714 File Offset: 0x00013914
		public int IndexOf(string value)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSVectorString_IndexOf(this.swigCPtr, value);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00015748 File Offset: 0x00013948
		public int LastIndexOf(string value)
		{
			int result = CocoStudioEngineAdapterPINVOKE.CSVectorString_LastIndexOf(this.swigCPtr, value);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0001577C File Offset: 0x0001397C
		public bool Remove(string value)
		{
			bool result = CocoStudioEngineAdapterPINVOKE.CSVectorString_Remove(this.swigCPtr, value);
			if (CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Pending)
			{
				throw CocoStudioEngineAdapterPINVOKE.SWIGPendingException.Retrieve();
			}
			return result;
		}

		// Token: 0x040000C3 RID: 195
		private HandleRef swigCPtr;

		// Token: 0x040000C4 RID: 196
		protected bool swigCMemOwn;

		// Token: 0x0200006A RID: 106
		public sealed class CSVectorStringEnumerator : IEnumerator<string>, IDisposable, IEnumerator
		{
			// Token: 0x06000C09 RID: 3081 RVA: 0x000157AE File Offset: 0x000139AE
			public CSVectorStringEnumerator(CSVectorString collection)
			{
				this.collectionRef = collection;
				this.currentIndex = -1;
				this.currentObject = null;
				this.currentSize = this.collectionRef.Count;
			}

			// Token: 0x1700006D RID: 109
			// (get) Token: 0x06000C0A RID: 3082 RVA: 0x000157E0 File Offset: 0x000139E0
			public string Current
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
					return (string)this.currentObject;
				}
			}

			// Token: 0x1700006E RID: 110
			// (get) Token: 0x06000C0B RID: 3083 RVA: 0x00015858 File Offset: 0x00013A58
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06000C0C RID: 3084 RVA: 0x00015870 File Offset: 0x00013A70
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

			// Token: 0x06000C0D RID: 3085 RVA: 0x000158E4 File Offset: 0x00013AE4
			public void Reset()
			{
				this.currentIndex = -1;
				this.currentObject = null;
				if (this.collectionRef.Count != this.currentSize)
				{
					throw new InvalidOperationException("Collection modified.");
				}
			}

			// Token: 0x06000C0E RID: 3086 RVA: 0x00015923 File Offset: 0x00013B23
			public void Dispose()
			{
				this.currentIndex = -1;
				this.currentObject = null;
			}

			// Token: 0x040000C5 RID: 197
			private CSVectorString collectionRef;

			// Token: 0x040000C6 RID: 198
			private int currentIndex;

			// Token: 0x040000C7 RID: 199
			private object currentObject;

			// Token: 0x040000C8 RID: 200
			private int currentSize;
		}
	}
}
