using System;
using System.Collections.Generic;
using Mono.Addins;

namespace CocoStudio.Projects.Formates
{
	[TypeExtensionPoint]
	public interface ICompositeResourceProcesser
	{
		bool IsHiddenCompositeFile { get; set; }

		bool CanProcess(string filePath);

		List<string> GetFiles(string filePath);

		List<string> GetPretreatmentTypes();

		List<string> GetAfterTypes();

		List<string> GetFilterTypes();
	}
}
