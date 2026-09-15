using System;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public interface IInitialize
	{
		bool IsAutoInitialize { get; }

		void Initialize(IProgressMonitor monitor);
	}
}
