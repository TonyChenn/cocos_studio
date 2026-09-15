using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public interface IFoldeItem
	{
		FilePath BaseDirectory { get; }
	}
}
