using System;
using System.Collections.Generic;
using System.IO;
using CocoStudio.Model;
using Modules.Communal.Packer;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x0200002E RID: 46
	[Extension(typeof(ICompositeResourceProcesser))]
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(IPublishProcesser))]
	internal class PlistParticleFileFormat : CompositeFormat, IPublishProcesser
	{
		// Token: 0x06000102 RID: 258 RVA: 0x00004F4B File Offset: 0x0000314B
		protected override bool OnCanWriteFile(object obj)
		{
			return obj is PlistParticleFile;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00004F58 File Offset: 0x00003158
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

		// Token: 0x06000104 RID: 260 RVA: 0x00004FBC File Offset: 0x000031BC
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PlistParticleFile(file);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004FC4 File Offset: 0x000031C4
		bool IPublishProcesser.CanProcess(ResourceData resourceData)
		{
			if (resourceData.Type == EnumResourceType.PlistSubImage || resourceData.Type == EnumResourceType.MarkedSubImage)
			{
				return false;
			}
			FilePath fullPath = ProjectsService.Instance.GetFullPath(resourceData);
			return base.CanReadFile(fullPath, typeof(ResourceItem)) && ((ICompositeResourceProcesser)this).GetFiles(fullPath) != null;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000501C File Offset: 0x0000321C
		HashSet<ResourceData> IPublishProcesser.Process(ResourceData resourceData)
		{
			string filePath = ProjectsService.Instance.GetFullPath(resourceData);
			return CompositeResourceHelp.GetResourcesIncludeImage(resourceData, ((ICompositeResourceProcesser)this).GetFiles(filePath));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005047 File Offset: 0x00003247
		public override bool CanProcess(string filePath)
		{
			return PlistParticleReader.CheckIsParticle(filePath);
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00005050 File Offset: 0x00003250
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

		// Token: 0x06000109 RID: 265 RVA: 0x000050A4 File Offset: 0x000032A4
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".plist"
			};
		}
	}
}
