using System;
using System.Collections;
using System.Reflection;

namespace MonoDevelop.Core.Serialization
{
	// Token: 0x02000074 RID: 116
	internal abstract class GenericCollectionHandler : ICollectionHandler
	{
		// Token: 0x060003BF RID: 959 RVA: 0x0000E4A2 File Offset: 0x0000C6A2
		protected GenericCollectionHandler(Type type, Type elemType, MethodInfo addMethod)
		{
			this.type = type;
			this.elementType = elemType;
			this.addMethod = addMethod;
			this.hasPublicConstructor = (type.GetConstructor(Type.EmptyTypes) != null);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000E4E4 File Offset: 0x0000C6E4
		public static ICollectionHandler CreateHandler(Type t)
		{
			if (!typeof(IList).IsAssignableFrom(t))
			{
				return null;
			}
			MethodInfo methodInfo = null;
			try
			{
				methodInfo = t.GetMethod("Add");
			}
			catch (AmbiguousMatchException)
			{
			}
			if (methodInfo == null)
			{
				return null;
			}
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length != 1)
			{
				return null;
			}
			Type parameterType = parameters[0].ParameterType;
			PropertyInfo propertyInfo = null;
			PropertyInfo propertyInfo2 = null;
			PropertyInfo[] properties = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo3 in properties)
			{
				if (propertyInfo3.CanRead)
				{
					ParameterInfo[] indexParameters = propertyInfo3.GetIndexParameters();
					if (propertyInfo3.CanWrite && indexParameters != null && indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(int))
					{
						propertyInfo = propertyInfo3;
					}
					else if (propertyInfo3.Name == "Count" && propertyInfo3.PropertyType == typeof(int))
					{
						propertyInfo2 = propertyInfo3;
					}
				}
			}
			if (propertyInfo != null && propertyInfo2 != null && propertyInfo.PropertyType == parameterType)
			{
				return new IndexedCollectionHandler(t, parameterType, methodInfo, propertyInfo, propertyInfo2);
			}
			if (!typeof(IEnumerable).IsAssignableFrom(t))
			{
				return null;
			}
			return new EnumerableCollectionHandler(t, parameterType, methodInfo, propertyInfo2);
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x0000E640 File Offset: 0x0000C840
		public bool CanCreateInstance
		{
			get
			{
				return this.hasPublicConstructor;
			}
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000E648 File Offset: 0x0000C848
		public Type GetItemType()
		{
			return this.elementType;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000E650 File Offset: 0x0000C850
		public object CreateCollection(out object position, int size)
		{
			position = 0;
			return Activator.CreateInstance(this.type);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000E665 File Offset: 0x0000C865
		public void ResetCollection(object collection, out object position, int size)
		{
			position = 0;
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000E66F File Offset: 0x0000C86F
		public void AddItem(ref object collection, ref object position, object item)
		{
			this.itemParam[0] = item;
			this.addMethod.Invoke(collection, this.itemParam);
			this.itemParam[0] = null;
			position = (int)position + 1;
		}

		// Token: 0x060003C6 RID: 966
		public abstract void SetItem(object collection, object position, object item);

		// Token: 0x060003C7 RID: 967 RVA: 0x0000E6A7 File Offset: 0x0000C8A7
		public void FinishCreation(ref object collection, object position)
		{
		}

		// Token: 0x060003C8 RID: 968
		public abstract object GetInitialPosition(object collection);

		// Token: 0x060003C9 RID: 969
		public abstract bool MoveNextItem(object collection, ref object position);

		// Token: 0x060003CA RID: 970
		public abstract object GetCurrentItem(object collection, object position);

		// Token: 0x060003CB RID: 971
		public abstract bool IsEmpty(object collection);

		// Token: 0x04000149 RID: 329
		protected Type type;

		// Token: 0x0400014A RID: 330
		protected Type elementType;

		// Token: 0x0400014B RID: 331
		protected MethodInfo addMethod;

		// Token: 0x0400014C RID: 332
		protected object[] itemParam = new object[1];

		// Token: 0x0400014D RID: 333
		private bool hasPublicConstructor;
	}
}
