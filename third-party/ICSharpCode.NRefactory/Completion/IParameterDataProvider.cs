using System;

namespace ICSharpCode.NRefactory.Completion
{
	/// <summary>
	/// Provides intellisense information for a collection of parametrized members.
	/// </summary>
	public interface IParameterDataProvider
	{
		/// <summary>
		/// Gets the overload count.
		/// </summary>
		int Count { get; }

		/// <summary>
		/// Gets the start offset of the parameter expression node.
		/// </summary>
		int StartOffset { get; }

		/// <summary>
		/// Returns the markup to use to represent the specified method overload
		/// in the parameter information window.
		/// </summary>
		string GetHeading(int overload, string[] parameterDescription, int currentParameter);

		/// <summary>
		/// Returns the markup for the description to use to represent the specified method overload
		/// in the parameter information window.
		/// </summary>
		string GetDescription(int overload, int currentParameter);

		/// <summary>
		/// Returns the text to use to represent the specified parameter
		/// </summary>
		string GetParameterDescription(int overload, int paramIndex);

		/// <summary>
		/// Gets the name of the parameter.
		/// </summary>
		string GetParameterName(int overload, int currentParameter);

		/// <summary>
		/// Returns the number of parameters of the specified method
		/// </summary>
		int GetParameterCount(int overload);

		/// <summary>
		/// Used for the params lists. (for example "params" in c#).
		/// </summary>
		bool AllowParameterList(int overload);
	}
}
