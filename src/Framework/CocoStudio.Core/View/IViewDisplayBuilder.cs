using System;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocoStudio.Core.View
{
	public interface IViewDisplayBuilder : IDisplayBuilder
	{
		IViewContentExtend CreateContent(FilePath fileName, string mimeType, CocosItem ownerProject);

		string Name { get; }
	}
}
