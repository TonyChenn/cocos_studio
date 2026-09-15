using System;
using System.Threading;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Represents the role a node plays within its parent.
	/// </summary>
	public abstract class Role
	{
		[CLSCompliant(false)]
		public uint Index
		{
			get
			{
				return this.index;
			}
		}

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
		public abstract bool IsValid(object node);

		/// <summary>
		/// Gets the role with the specified index.
		/// </summary>
		[CLSCompliant(false)]
		public static Role GetByIndex(uint index)
		{
			return Role.roles[(int)((UIntPtr)index)];
		}

		public const int RoleIndexBits = 9;

		private static readonly Role[] roles = new Role[512];

		private static int nextRoleIndex = 0;

		private readonly uint index;
	}
}
