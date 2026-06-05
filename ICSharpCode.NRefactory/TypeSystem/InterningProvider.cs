using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Provider used for interning.
	/// </summary>
	/// <remarks>
	/// A simple IInterningProvider implementation could use 3 dictionaries:
	///  1. using value equality comparer (for certain types known to implement value equality, e.g. string and IType)
	///  2. using comparer that calls into ISupportsInterning (for types implementing ISupportsInterning)
	///  3. list comparer (for InternList method)
	///
	/// On the first Intern()-call, the provider tells the object to prepare for interning (ISupportsInterning.PrepareForInterning)
	/// and stores it into a dictionary. On further Intern() calls, the original object is returned for all equal objects.
	/// This allows reducing the memory usage by using a single object instance where possible.
	///
	/// Interning provider implementations could also use the interning logic for different purposes:
	/// for example, it could be used to determine which objects are used jointly between multiple type definitions
	/// and which are used only within a single type definition. Then a persistent file format could be organized so
	/// that shared objects are loaded only once, yet non-shared objects get loaded lazily together with the class.
	/// </remarks>
	// Token: 0x0200009A RID: 154
	public abstract class InterningProvider
	{
		/// <summary>
		/// Interns the specified object.
		///
		/// If the object is freezable, it will be frozen.
		/// </summary>
		// Token: 0x060004D5 RID: 1237
		public abstract ISupportsInterning Intern(ISupportsInterning obj);

		/// <summary>
		/// Interns the specified object.
		///
		/// If the object is freezable, it will be frozen.
		/// </summary>
		// Token: 0x060004D6 RID: 1238 RVA: 0x0000BE8C File Offset: 0x0000AE8C
		public T Intern<T>(T obj) where T : class, ISupportsInterning
		{
			ISupportsInterning obj2 = obj;
			return (T)((object)this.Intern(obj2));
		}

		/// <summary>
		/// Interns the specified string.
		/// </summary>
		// Token: 0x060004D7 RID: 1239
		public abstract string Intern(string text);

		/// <summary>
		/// Inters a boxed value type.
		/// </summary>
		// Token: 0x060004D8 RID: 1240
		public abstract object InternValue(object obj);

		/// <summary>
		/// Interns the given list. Uses reference equality to compare the list elements.
		/// </summary>
		// Token: 0x060004D9 RID: 1241
		public abstract IList<T> InternList<T>(IList<T> list) where T : class;

		// Token: 0x0400014F RID: 335
		public static readonly InterningProvider Dummy = new InterningProvider.DummyInterningProvider();

		// Token: 0x0200009B RID: 155
		private sealed class DummyInterningProvider : InterningProvider
		{
			// Token: 0x060004DC RID: 1244 RVA: 0x0000BEC0 File Offset: 0x0000AEC0
			public override ISupportsInterning Intern(ISupportsInterning obj)
			{
				return obj;
			}

			// Token: 0x060004DD RID: 1245 RVA: 0x0000BEC3 File Offset: 0x0000AEC3
			public override string Intern(string text)
			{
				return text;
			}

			// Token: 0x060004DE RID: 1246 RVA: 0x0000BEC6 File Offset: 0x0000AEC6
			public override object InternValue(object obj)
			{
				return obj;
			}

			// Token: 0x060004DF RID: 1247 RVA: 0x0000BEC9 File Offset: 0x0000AEC9
			public override IList<T> InternList<T>(IList<T> list)
			{
				return list;
			}
		}
	}
}
