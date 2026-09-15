using System;
using System.Collections.Generic;
using System.Threading;
using CocoStudio.ControlLib;
using CocoStudio.Projects;

namespace CocoStudio.Core
{
	public class ImportResourceResult
	{
		public List<ResourceItem> ImportResources { get; set; }

		public List<ResourceItem> AddResourcePanelItems { get; set; }

		internal DialogResult DialogResult { get; set; }

		public IEnumerable<string> FileTypeSuffix { get; set; }

		public ImportResourceResult()
		{
			this.ImportResources = new List<ResourceItem>();
			this.AddResourcePanelItems = new List<ResourceItem>();
			this.DialogResult = new DialogResult();
			this.Token = CancellationToken.None;
		}

		internal CancellationToken Token { get; set; }
	}
}
