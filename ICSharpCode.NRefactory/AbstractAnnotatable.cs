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
	// Token: 0x0200001F RID: 31
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
		// Token: 0x0600012D RID: 301 RVA: 0x000042AC File Offset: 0x000032AC
		protected void CloneAnnotations()
		{
			ICloneable cloneable = this.annotations as ICloneable;
			if (cloneable != null)
			{
				this.annotations = cloneable.Clone();
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000042D4 File Offset: 0x000032D4
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

		// Token: 0x0600012F RID: 303 RVA: 0x00004370 File Offset: 0x00003370
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

		// Token: 0x06000130 RID: 304 RVA: 0x000043EC File Offset: 0x000033EC
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

		// Token: 0x06000131 RID: 305 RVA: 0x00004474 File Offset: 0x00003474
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

		// Token: 0x06000132 RID: 306 RVA: 0x00004528 File Offset: 0x00003528
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
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000133 RID: 307 RVA: 0x000045D8 File Offset: 0x000035D8
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

		// Token: 0x0400003D RID: 61
		private object annotations;

		// Token: 0x02000020 RID: 32
		private sealed class AnnotationList : List<object>, ICloneable
		{
			// Token: 0x06000136 RID: 310 RVA: 0x0000464C File Offset: 0x0000364C
			public AnnotationList(int initialCapacity) : base(initialCapacity)
			{
			}

			// Token: 0x06000137 RID: 311 RVA: 0x00004658 File Offset: 0x00003658
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
