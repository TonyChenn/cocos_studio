using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Provides an interface to handle annotations in an object.
	/// </summary>
	public interface IAnnotatable
	{
		/// <summary>
		/// Gets all annotations stored on this IAnnotatable.
		/// </summary>
		IEnumerable<object> Annotations { get; }

		/// <summary>
		/// Gets the first annotation of the specified type.
		/// Returns null if no matching annotation exists.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the annotation.
		/// </typeparam>
		T Annotation<T>() where T : class;

		/// <summary>
		/// Gets the first annotation of the specified type.
		/// Returns null if no matching annotation exists.
		/// </summary>
		/// <param name="type">
		/// The type of the annotation.
		/// </param>
		object Annotation(Type type);

		/// <summary>
		/// Adds an annotation to this instance.
		/// </summary>
		/// <param name="annotation">
		/// The annotation to add.
		/// </param>
		void AddAnnotation(object annotation);

		/// <summary>
		/// Removes all annotations of the specified type.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the annotations to remove.
		/// </typeparam>
		void RemoveAnnotations<T>() where T : class;

		/// <summary>
		/// Removes all annotations of the specified type.
		/// </summary>
		/// <param name="type">
		/// The type of the annotations to remove.
		/// </param>
		void RemoveAnnotations(Type type);
	}
}
