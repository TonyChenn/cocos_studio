using System;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Represents the role a node plays within its parent.
	/// All nodes with this role have type T.
	/// </summary>
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
		public T NullObject
		{
			get
			{
				return this.nullObject;
			}
		}

		public override bool IsValid(object node)
		{
			return node is T;
		}

		public Role(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
		}

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

		public override string ToString()
		{
			return this.name;
		}

		private readonly string name;

		private readonly T nullObject;
	}
}
