using System;

namespace ICSharpCode.NRefactory.Completion
{
	/// <summary>
	/// Provides intellisense information for a collection of parametrized members.
	/// </summary>
	// Token: 0x0200012D RID: 301
	public interface IParameterDataProvider
	{
		/// <summary>
		/// Gets the overload count.
		/// </summary>
		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x06000A7F RID: 2687
		int Count { get; }

		/// <summary>
		/// Gets the start offset of the parameter expression node.
		/// </summary>
		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x06000A80 RID: 2688
		int StartOffset { get; }

		/// <summary>
		/// Returns the markup to use to represent the specified method overload
		/// in the parameter information window.
		/// </summary>
		// Token: 0x06000A81 RID: 2689
		string GetHeading(int overload, string[] parameterDescription, int currentParameter);

		/// <summary>
		/// Returns the markup for the description to use to represent the specified method overload
		/// in the parameter information window.
		/// </summary>
		// Token: 0x06000A82 RID: 2690
		string GetDescription(int overload, int currentParameter);

		/// <summary>
		/// Returns the text to use to represent the specified parameter
		/// </summary>
		// Token: 0x06000A83 RID: 2691
		string GetParameterDescription(int overload, int paramIndex);

		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		// Token: 0x06000A84 RID: 2692
		string GetParameterName(int overload, int currentParameter);

		/// <summary>
		/// Returns the number of parameters of the specified method
		/// </summary>
		// Token: 0x06000A85 RID: 2693
		int GetParameterCount(int overload);

		/// <summary>
		/// Used for the params lists. (for example "params" in c#).
		/// </summary>
		// Token: 0x06000A86 RID: 2694
		bool AllowParameterList(int overload);
	}
}
