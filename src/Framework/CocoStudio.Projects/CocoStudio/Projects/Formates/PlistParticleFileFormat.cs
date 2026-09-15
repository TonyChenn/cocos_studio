using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Model;
using Modules.Communal.Packer;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(IPublishProcesser))]
	internal class PlistParticleFileFormat : CompositeFormat, IPublishProcesser
	{
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PlistParticleFile;
		}

		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			try
			{
				if (!FileFormat.CheckFileSuffix(file, new string[]
				{
					".plist"
				}))
				{
					return false;
				}
				if (!expectedObjectType.Equals(typeof(ResourceItem)))
				{
					return false;
				}
				return PlistParticleReader.CheckIsParticle(file);
			}
			catch
			{
			}
			return false;
		}

		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PlistParticleFile(file);
		}

		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.PlistSubImage || resourceData.Type == EnumResourceType.MarkedSubImage)
			{
				return false;
			}
			FilePath fullPath = ProjectsService.Instance.GetFullPath(resourceData);
			return base.CanReadFile(fullPath, typeof(ResourceItem)) && ((ICompositeResourceProcesser)this).GetFiles(fullPath) != null;
		}

		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			string filePath = ProjectsService.Instance.GetFullPath(resourceData);
			return CompositeResourceHelp.GetResourcesIncludeImage(resourceData, ((ICompositeResourceProcesser)this).GetFiles(filePath));
		}

		public override bool CanProcess(string filePath)
		{
			return PlistParticleReader.CheckIsParticle(filePath);
		}

		public override List<string> GetFiles(string filePath)
		{
			FilePath filePath2 = PlistParticleReader.GetMatchImage(filePath);
			if (filePath2 == null)
			{
				return null;
			}
			string directoryName = Path.GetDirectoryName(filePath);
			filePath2 = filePath2.ToAbsolute((FilePath)directoryName);
			return new List<string>
			{
				filePath2
			};
		}

		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".plist"
			};
		}
	}
}
