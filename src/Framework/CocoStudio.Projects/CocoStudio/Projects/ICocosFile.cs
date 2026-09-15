using System;
using System.Collections.Generic;
using CocoStudio.Model;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public interface ICocosFile : IInitialize
	{
		bool IsLoaded { get; }

		void Load(IProgressMonitor monitor);

		void Save(IProgressMonitor monitor);

		void UnLoad(IProgressMonitor monitor);

		HashSet<ResourceData> GetUsedResources(IProgressMonitor monitor);

		bool UpdateUsedResources(IProgressMonitor monitor, ChangedResourceCollection changedResourceCollection);
	}
}
