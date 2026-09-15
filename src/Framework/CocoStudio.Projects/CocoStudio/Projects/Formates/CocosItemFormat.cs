using System;
using System.Collections.Generic;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	internal class CocosItemFormat : CompositeFormat
	{
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".csd"
			}) && (expectedObjectType.Equals(typeof(CocosItem)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().IsSubclassOf(typeof(CocosItem));
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new CocosItem(file);
		}

		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".csd"
			};
		}

		public override List<string> GetFiles(string filePath)
		{
			string text = Path.ChangeExtension(filePath, ".csd.udf");
			if (File.Exists(text))
			{
				return new List<string>
				{
					text
				};
			}
			return null;
		}

		public override List<string> GetFilterTypes()
		{
			return new List<string>
			{
				".udf"
			};
		}
	}
}
