using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Enum that describes the accessibility of an entity.
	/// </summary>
	// Token: 0x02000054 RID: 84
	public enum Accessibility : byte
	{
		/// <summary>
		/// The entity is completely inaccessible. This is used for C# explicit interface implementations.
		/// </summary>
		// Token: 0x040000B5 RID: 181
		None,
		/// <summary>
		/// The entity is only accessible within the same class.
		/// </summary>
		// Token: 0x040000B6 RID: 182
		Private,
		/// <summary>
		/// The entity is accessible everywhere.
		/// </summary>
		// Token: 0x040000B7 RID: 183
		Public,
		/// <summary>
		/// The entity is only accessible within the same class and in derived classes.
		/// </summary>
		// Token: 0x040000B8 RID: 184
		Protected,
		/// <summary>
		/// The entity is accessible within the same project content.
		/// </summary>
		// Token: 0x040000B9 RID: 185
		Internal,
		/// <summary>
		/// The entity is accessible both everywhere in the project content, and in all derived classes.
		/// </summary>
		/// <remarks>This corresponds to C# 'protected internal'.</remarks>
		// Token: 0x040000BA RID: 186
		ProtectedOrInternal,
		/// <summary>
		/// The entity is accessible in derived classes within the same project content.
		/// </summary>
		/// <remarks>C# does not support this accessibility.</remarks>
		// Token: 0x040000BB RID: 187
		ProtectedAndInternal
	}
}
