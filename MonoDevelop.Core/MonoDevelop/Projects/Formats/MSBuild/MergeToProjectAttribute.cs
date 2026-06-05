using System;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	/// <summary>
	/// Specifies that a property of a project configuration has to be stored as a project property
	/// </summary>
	/// <remarks>
	/// When applied to a property of a project configuration, MD will check
	/// if the value of that property is the same for all configurations.
	/// If they are the same, the value will be stored in the main property
	/// group, instead of individually in each configuration.
	/// </remarks>
	// Token: 0x020001D9 RID: 473
	public class MergeToProjectAttribute : Attribute
	{
	}
}
