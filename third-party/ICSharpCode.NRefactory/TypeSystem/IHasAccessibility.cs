using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000055 RID: 85
	public interface IHasAccessibility
	{
		/// <summary>
		/// Gets the accessibility of this entity.
		/// </summary>
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000264 RID: 612
		Accessibility Accessibility { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is private.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is private; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000265 RID: 613
		bool IsPrivate { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is public.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is public; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000266 RID: 614
		bool IsPublic { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is protected.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is protected; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000267 RID: 615
		bool IsProtected { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is internal.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is internal; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000268 RID: 616
		bool IsInternal { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is protected or internal.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is protected or internal; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000269 RID: 617
		bool IsProtectedOrInternal { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is protected and internal.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is protected and internal; otherwise, <c>false</c>.
		/// </value>
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600026A RID: 618
		bool IsProtectedAndInternal { get; }
	}
}
