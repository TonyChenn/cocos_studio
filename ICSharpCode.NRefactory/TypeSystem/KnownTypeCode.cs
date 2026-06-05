using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents some well-known types.
	/// </summary>
	// Token: 0x020000EF RID: 239
	public enum KnownTypeCode
	{
		/// <summary>
		/// Not one of the known types.
		/// </summary>
		// Token: 0x04000282 RID: 642
		None,
		/// <summary><c>object</c> (System.Object)</summary>
		// Token: 0x04000283 RID: 643
		Object,
		/// <summary><c>System.DBNull</c></summary>
		// Token: 0x04000284 RID: 644
		DBNull,
		/// <summary><c>bool</c> (System.Boolean)</summary>
		// Token: 0x04000285 RID: 645
		Boolean,
		/// <summary><c>char</c> (System.Char)</summary>
		// Token: 0x04000286 RID: 646
		Char,
		/// <summary><c>sbyte</c> (System.SByte)</summary>
		// Token: 0x04000287 RID: 647
		SByte,
		/// <summary><c>byte</c> (System.Byte)</summary>
		// Token: 0x04000288 RID: 648
		Byte,
		/// <summary><c>short</c> (System.Int16)</summary>
		// Token: 0x04000289 RID: 649
		Int16,
		/// <summary><c>ushort</c> (System.UInt16)</summary>
		// Token: 0x0400028A RID: 650
		UInt16,
		/// <summary><c>int</c> (System.Int32)</summary>
		// Token: 0x0400028B RID: 651
		Int32,
		/// <summary><c>uint</c> (System.UInt32)</summary>
		// Token: 0x0400028C RID: 652
		UInt32,
		/// <summary><c>long</c> (System.Int64)</summary>
		// Token: 0x0400028D RID: 653
		Int64,
		/// <summary><c>ulong</c> (System.UInt64)</summary>
		// Token: 0x0400028E RID: 654
		UInt64,
		/// <summary><c>float</c> (System.Single)</summary>
		// Token: 0x0400028F RID: 655
		Single,
		/// <summary><c>double</c> (System.Double)</summary>
		// Token: 0x04000290 RID: 656
		Double,
		/// <summary><c>decimal</c> (System.Decimal)</summary>
		// Token: 0x04000291 RID: 657
		Decimal,
		/// <summary><c>System.DateTime</c></summary>
		// Token: 0x04000292 RID: 658
		DateTime,
		/// <summary><c>string</c> (System.String)</summary>
		// Token: 0x04000293 RID: 659
		String = 18,
		/// <summary><c>void</c> (System.Void)</summary>
		// Token: 0x04000294 RID: 660
		Void,
		/// <summary><c>System.Type</c></summary>
		// Token: 0x04000295 RID: 661
		Type,
		/// <summary><c>System.Array</c></summary>
		// Token: 0x04000296 RID: 662
		Array,
		/// <summary><c>System.Attribute</c></summary>
		// Token: 0x04000297 RID: 663
		Attribute,
		/// <summary><c>System.ValueType</c></summary>
		// Token: 0x04000298 RID: 664
		ValueType,
		/// <summary><c>System.Enum</c></summary>
		// Token: 0x04000299 RID: 665
		Enum,
		/// <summary><c>System.Delegate</c></summary>
		// Token: 0x0400029A RID: 666
		Delegate,
		/// <summary><c>System.MulticastDelegate</c></summary>
		// Token: 0x0400029B RID: 667
		MulticastDelegate,
		/// <summary><c>System.Exception</c></summary>
		// Token: 0x0400029C RID: 668
		Exception,
		/// <summary><c>System.IntPtr</c></summary>
		// Token: 0x0400029D RID: 669
		IntPtr,
		/// <summary><c>System.UIntPtr</c></summary>
		// Token: 0x0400029E RID: 670
		UIntPtr,
		/// <summary><c>System.Collections.IEnumerable</c></summary>
		// Token: 0x0400029F RID: 671
		IEnumerable,
		/// <summary><c>System.Collections.IEnumerator</c></summary>
		// Token: 0x040002A0 RID: 672
		IEnumerator,
		/// <summary><c>System.Collections.Generic.IEnumerable{T}</c></summary>
		// Token: 0x040002A1 RID: 673
		IEnumerableOfT,
		/// <summary><c>System.Collections.Generic.IEnumerator{T}</c></summary>
		// Token: 0x040002A2 RID: 674
		IEnumeratorOfT,
		/// <summary><c>System.Collections.Generic.ICollection</c></summary>
		// Token: 0x040002A3 RID: 675
		ICollection,
		/// <summary><c>System.Collections.Generic.ICollection{T}</c></summary>
		// Token: 0x040002A4 RID: 676
		ICollectionOfT,
		/// <summary><c>System.Collections.Generic.IList</c></summary>
		// Token: 0x040002A5 RID: 677
		IList,
		/// <summary><c>System.Collections.Generic.IList{T}</c></summary>
		// Token: 0x040002A6 RID: 678
		IListOfT,
		/// <summary><c>System.Collections.Generic.IReadOnlyCollection{T}</c></summary>
		// Token: 0x040002A7 RID: 679
		IReadOnlyCollectionOfT,
		/// <summary><c>System.Collections.Generic.IReadOnlyList{T}</c></summary>
		// Token: 0x040002A8 RID: 680
		IReadOnlyListOfT,
		/// <summary><c>System.Threading.Tasks.Task</c></summary>
		// Token: 0x040002A9 RID: 681
		Task,
		/// <summary><c>System.Threading.Tasks.Task{T}</c></summary>
		// Token: 0x040002AA RID: 682
		TaskOfT,
		/// <summary><c>System.Nullable{T}</c></summary>
		// Token: 0x040002AB RID: 683
		NullableOfT,
		/// <summary><c>System.IDisposable</c></summary>
		// Token: 0x040002AC RID: 684
		IDisposable,
		/// <summary><c>System.Runtime.CompilerServices.INotifyCompletion</c></summary>
		// Token: 0x040002AD RID: 685
		INotifyCompletion,
		/// <summary><c>System.Runtime.CompilerServices.ICriticalNotifyCompletion</c></summary>
		// Token: 0x040002AE RID: 686
		ICriticalNotifyCompletion
	}
}
