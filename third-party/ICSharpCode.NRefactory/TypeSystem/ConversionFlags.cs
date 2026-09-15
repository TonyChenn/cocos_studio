using System;

namespace ICSharpCode.NRefactory.TypeSystem
{
	[Flags]
	public enum ConversionFlags
	{
		/// <summary>
		/// Convert only the name.
		/// </summary>
		None = 0,
		/// <summary>
		/// Show the parameter list
		/// </summary>
		ShowParameterList = 1,
		/// <summary>
		/// Show names for parameters
		/// </summary>
		ShowParameterNames = 2,
		/// <summary>
		/// Show the accessibility (private, public, etc.)
		/// </summary>
		ShowAccessibility = 4,
		/// <summary>
		/// Show the definition key word (class, struct, Sub, Function, etc.)
		/// </summary>
		ShowDefinitionKeyword = 8,
		/// <summary>
		/// Show the declaring type for the member
		/// </summary>
		ShowDeclaringType = 16,
		/// <summary>
		/// Show modifiers (virtual, override, etc.)
		/// </summary>
		ShowModifiers = 32,
		/// <summary>
		/// Show the return type
		/// </summary>
		ShowReturnType = 64,
		/// <summary>
		/// Use fully qualified names for types.
		/// </summary>
		UseFullyQualifiedTypeNames = 128,
		/// <summary>
		/// Show the list of type parameters on method and class declarations.
		/// Type arguments for parameter/return types are always shown.
		/// </summary>
		ShowTypeParameterList = 256,
		/// <summary>
		/// For fields, events and methods: adds a semicolon at the end.
		/// For properties: shows "{ get; }" or similar.
		/// </summary>
		ShowBody = 512,
		/// <summary>
		/// Use fully qualified names for members.
		/// </summary>
		UseFullyQualifiedEntityNames = 1024,
		StandardConversionFlags = 879,
		All = 2047
	}
}
