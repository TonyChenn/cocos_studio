using System;
using System.Collections.Generic;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	[Extension(Type = typeof(IUserData))]
	public class TabsInfo : IUserData
	{
		[ItemProperty("OpenedDocuments")]
		public List<FilePathData> OpenedDocuments { get; set; }

		[ItemProperty("ActiveDocument")]
		public FilePathData ActiveDocument { get; set; }

		public const string TabsParamsKey = "TabsParamsKey";
	}
}
