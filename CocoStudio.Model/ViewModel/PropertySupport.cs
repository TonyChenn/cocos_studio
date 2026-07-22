using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x02000130 RID: 304
	public static class PropertySupport
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x0002CAA8 File Offset: 0x0002ACA8
		public static string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			return PropertySupport.ExtractPropertyInfo<T>(propertyExpression).Name;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0002CAC8 File Offset: 0x0002ACC8
		public static PropertyInfo ExtractPropertyInfo<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}
			MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
			if (memberExpression == null)
			{
				throw new ArgumentException("PropertySupport_NotMemberAccessExpression_Exception", "propertyExpression");
			}
			PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("PropertySupport_ExpressionNotProperty_Exception", "propertyExpression");
			}
			MethodInfo getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod.IsStatic)
			{
				throw new ArgumentException("PropertySupport_StaticExpression_Exception", "propertyExpression");
			}
			return propertyInfo;
		}
	}
}
