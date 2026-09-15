using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ICSharpCode.NRefactory
{
	/// <summary>
	/// Base class used to implement the IAnnotatable interface.
	/// This implementation is thread-safe.
	/// </summary>
	[Serializable]
	public abstract class AbstractAnnotatable : IAnnotatable
	{
		/// <summary>
		/// Clones all annotations.
		/// This method is intended to be called by Clone() implementations in derived classes.
		/// <code>
		/// AstNode copy = (AstNode)MemberwiseClone();
		/// copy.CloneAnnotations();
		/// </code>
		/// </summary>
		protected void CloneAnnotations()
		{
			ICloneable cloneable = this.annotations as ICloneable;
			if (cloneable != null)
			{
				this.annotations = cloneable.Clone();
			}
		}

		public virtual void AddAnnotation(object annotation)
		{
			if (annotation == null)
			{
				throw new ArgumentNullException("annotation");
			}
			AbstractAnnotatable.AnnotationList annotationList;
			for (;;)
			{
				object obj = Interlocked.CompareExchange(ref this.annotations, annotation, null);
				if (obj == null)
				{
					break;
				}
				annotationList = (obj as AbstractAnnotatable.AnnotationList);
				if (annotationList != null)
				{
					goto IL_51;
				}
				if (Interlocked.CompareExchange(ref this.annotations, new AbstractAnnotatable.AnnotationList(4)
				{
					obj,
					annotation
				}, obj) == obj)
				{
					return;
				}
			}
			return;
			IL_51:
			lock (annotationList)
			{
				annotationList.Add(annotation);
			}
		}

		public virtual void RemoveAnnotations<T>() where T : class
		{
			Predicate<object> predicate = null;
			object obj3;
			do
			{
				obj3 = this.annotations;
				AbstractAnnotatable.AnnotationList annotationList = obj3 as AbstractAnnotatable.AnnotationList;
				if (annotationList != null)
				{
					lock (annotationList)
					{
						List<object> list = annotationList;
						if (predicate == null)
						{
							predicate = ((object obj) => obj is T);
						}
						list.RemoveAll(predicate);
						break;
					}
				}
			}
			while (obj3 is T && Interlocked.CompareExchange(ref this.annotations, null, obj3) != obj3);
		}

		public virtual void RemoveAnnotations(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object obj;
			do
			{
				obj = this.annotations;
				AbstractAnnotatable.AnnotationList annotationList = obj as AbstractAnnotatable.AnnotationList;
				if (annotationList != null)
				{
					lock (annotationList)
					{
						annotationList.RemoveAll(new Predicate<object>(type.IsInstanceOfType));
						break;
					}
				}
			}
			while (type.IsInstanceOfType(obj) && Interlocked.CompareExchange(ref this.annotations, null, obj) != obj);
		}

		public T Annotation<T>() where T : class
		{
			object obj = this.annotations;
			AbstractAnnotatable.AnnotationList annotationList = obj as AbstractAnnotatable.AnnotationList;
			if (annotationList != null)
			{
				lock (annotationList)
				{
					foreach (object obj3 in annotationList)
					{
						T t = obj3 as T;
						if (t != null)
						{
							return t;
						}
					}
					return default(T);
				}
			}
			return obj as T;
		}

		public object Annotation(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			object obj = this.annotations;
			AbstractAnnotatable.AnnotationList annotationList = obj as AbstractAnnotatable.AnnotationList;
			if (annotationList != null)
			{
				lock (annotationList)
				{
					foreach (object obj3 in annotationList)
					{
						if (type.IsInstanceOfType(obj3))
						{
							return obj3;
						}
					}
					goto IL_83;
				}
			}
			if (type.IsInstanceOfType(obj))
			{
				return obj;
			}
			IL_83:
			return null;
		}

		/// <summary>
		/// Gets all annotations stored on this AstNode.
		/// </summary>
		public IEnumerable<object> Annotations
		{
			get
			{
				object obj = this.annotations;
				AbstractAnnotatable.AnnotationList annotationList = obj as AbstractAnnotatable.AnnotationList;
				if (annotationList != null)
				{
					lock (annotationList)
					{
						return annotationList.ToArray();
					}
				}
				if (obj != null)
				{
					return new object[]
					{
						obj
					};
				}
				return Enumerable.Empty<object>();
			}
		}

		private object annotations;

		private sealed class AnnotationList : List<object>, ICloneable
		{
			public AnnotationList(int initialCapacity) : base(initialCapacity)
			{
			}

			public object Clone()
			{
				object result;
				lock (this)
				{
					AbstractAnnotatable.AnnotationList annotationList = new AbstractAnnotatable.AnnotationList(base.Count);
					for (int i = 0; i < base.Count; i++)
					{
						object obj = base[i];
						ICloneable cloneable = obj as ICloneable;
						annotationList.Add((cloneable != null) ? cloneable.Clone() : obj);
					}
					result = annotationList;
				}
				return result;
			}
		}
	}
}
