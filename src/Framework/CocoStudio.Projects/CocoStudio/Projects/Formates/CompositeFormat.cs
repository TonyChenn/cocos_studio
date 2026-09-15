using System;
using System.Collections.Generic;

namespace CocoStudio.Projects.Formates
{
	public abstract class CompositeFormat : FileFormat, ICompositeResourceProcesser
	{
		public bool IsHiddenCompositeFile { get; set; }

		public CompositeFormat()
		{
			this.IsHiddenCompositeFile = true;
		}

		public virtual bool CanProcess(string filePath)
		{
			return base.CanReadFile(filePath, typeof(ResourceItem));
		}

		public virtual List<string> GetFiles(string filePath)
		{
			return null;
		}

		public virtual List<string> GetPretreatmentTypes()
		{
			return null;
		}

		public virtual List<string> GetFilterTypes()
		{
			return null;
		}

		public virtual List<string> GetAfterTypes()
		{
			return null;
		}
	}
}
