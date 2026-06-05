using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	// Token: 0x02000056 RID: 86
	public interface INamedElement
	{
		/// <summary>
		/// Gets the fully qualified name of the class the return type is pointing to.
		/// </summary>
		/// <returns>
		/// "System.Int32[]" for int[]<br />
		/// "System.Collections.Generic.List" for List&lt;string&gt;
		/// "System.Environment.SpecialFolder" for Environment.SpecialFolder
		/// </returns>
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600026B RID: 619
		string FullName { get; }

		/// <summary>
		/// Gets the short name of the class the return type is pointing to.
		/// </summary>
		/// <returns>
		/// "Int32[]" for int[]<br />
		/// "List" for List&lt;string&gt;
		/// "SpecialFolder" for Environment.SpecialFolder
		/// </returns>
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600026C RID: 620
		string Name { get; }

		/// <summary>
		/// Gets the full reflection name of the element.
		/// </summary>
		/// <remarks>
		/// For types, the reflection name can be parsed back into a ITypeReference by using
		/// <see cref="M:ICSharpCode.NRefactory.TypeSystem.ReflectionHelper.ParseReflectionName(System.String)" />.
		/// </remarks>
		/// <returns>
		/// "System.Int32[]" for int[]<br />
		/// "System.Int32[][,]" for C# int[,][]<br />
		/// "System.Collections.Generic.List`1[[System.String]]" for List&lt;string&gt;
		/// "System.Environment+SpecialFolder" for Environment.SpecialFolder
		/// </returns>
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600026D RID: 621
		string ReflectionName { get; }

		/// <summary>
		/// Gets the full name of the namespace containing this entity.
		/// </summary>
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600026E RID: 622
		string Namespace { get; }
	}
}
