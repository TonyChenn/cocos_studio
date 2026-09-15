using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public interface IFileItem
	{
		FilePath FileName { get; }
	}
}
