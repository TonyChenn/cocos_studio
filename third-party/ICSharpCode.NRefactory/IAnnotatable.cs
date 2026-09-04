using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Provides an interface to handle annotations in an object.
	/// </summary>
	// Token: 0x0200001E RID: 30
	public interface IAnnotatable
	{
		/// <summary>
		/// Gets all annotations stored on this IAnnotatable.
		/// </summary>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000127 RID: 295
		IEnumerable<object> Annotations { get; }

		/// <summary>
		/// Gets the first annotation of the specified type.
		/// Returns null if no matching annotation exists.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the annotation.
		/// </typeparam>
		// Token: 0x06000128 RID: 296
		T Annotation<T>() where T : class;

		/// <summary>
		/// Gets the first annotation of the specified type.
		/// Returns null if no matching annotation exists.
		/// </summary>
		/// <param name="type">
		/// The type of the annotation.
		/// </param>
		// Token: 0x06000129 RID: 297
		object Annotation(Type type);

		/// <summary>
		/// Adds an annotation to this instance.
		/// </summary>
		/// <param name="annotation">
		/// The annotation to add.
		/// </param>
		// Token: 0x0600012A RID: 298
		void AddAnnotation(object annotation);

		/// <summary>
		/// Removes all annotations of the specified type.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the annotations to remove.
		/// </typeparam>
		// Token: 0x0600012B RID: 299
		void RemoveAnnotations<T>() where T : class;

		/// <summary>
		/// Removes all annotations of the specified type.
		/// </summary>
		/// <param name="type">
		/// The type of the annotations to remove.
		/// </param>
		// Token: 0x0600012C RID: 300
		void RemoveAnnotations(Type type);
	}
}
