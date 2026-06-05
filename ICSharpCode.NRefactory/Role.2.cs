using System;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Represents the role a node plays within its parent.
	/// All nodes with this role have type T.
	/// </summary>
	// Token: 0x02000032 RID: 50
	public class Role<T> : Role where T : class
	{
		/// <summary>
		/// Gets the null object used when there's no node with this role.
		/// Not every role has a null object; this property returns null for roles without a null object.
		/// </summary>
		/// <remarks>
		/// Roles used for non-collections should always have a null object, so that no AST property returns null.
		/// However, if a role used for collections only, it may leave out the null object.
		/// </remarks>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600018C RID: 396 RVA: 0x0000596E File Offset: 0x0000496E
		public T NullObject
		{
			get
			{
				return this.nullObject;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00005976 File Offset: 0x00004976
		public override bool IsValid(object node)
		{
			return node is T;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00005981 File Offset: 0x00004981
		public Role(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000599E File Offset: 0x0000499E
		public Role(string name, T nullObject)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (nullObject == null)
			{
				throw new ArgumentNullException("nullObject");
			}
			this.nullObject = nullObject;
			this.name = name;
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000059D5 File Offset: 0x000049D5
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000059 RID: 89
		private readonly string name;

		// Token: 0x0400005A RID: 90
		private readonly T nullObject;
	}
}
