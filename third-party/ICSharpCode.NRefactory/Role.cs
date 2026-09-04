using System;
using System.Threading;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Represents the role a node plays within its parent.
	/// </summary>
	// Token: 0x02000031 RID: 49
	public abstract class Role
	{
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000187 RID: 391 RVA: 0x000058F5 File Offset: 0x000048F5
		[CLSCompliant(false)]
		public uint Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00005900 File Offset: 0x00004900
		internal Role()
		{
			this.index = (uint)Interlocked.Increment(ref Role.nextRoleIndex);
			if ((ulong)this.index >= (ulong)((long)Role.roles.Length))
			{
				throw new InvalidOperationException("Too many roles");
			}
			Role.roles[(int)((UIntPtr)this.index)] = this;
		}

		/// <summary>
		/// Gets whether the specified node is valid in this role.
		/// </summary>
		// Token: 0x06000189 RID: 393
		public abstract bool IsValid(object node);

		/// <summary>
		/// Gets the role with the specified index.
		/// </summary>
		// Token: 0x0600018A RID: 394 RVA: 0x0000594D File Offset: 0x0000494D
		[CLSCompliant(false)]
		public static Role GetByIndex(uint index)
		{
			return Role.roles[(int)((UIntPtr)index)];
		}

		// Token: 0x04000055 RID: 85
		public const int RoleIndexBits = 9;

		// Token: 0x04000056 RID: 86
		private static readonly Role[] roles = new Role[512];

		// Token: 0x04000057 RID: 87
		private static int nextRoleIndex = 0;

		// Token: 0x04000058 RID: 88
		private readonly uint index;
	}
}
