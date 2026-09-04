using System;
using System.Collections.Generic;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core;

namespace CocoStudio.Projects.Formates
{
	// Token: 0x02000035 RID: 53
	[Extension(typeof(IFileFormat))]
	[Extension(typeof(ICompositeResourceProcesser))]
	internal class CocosItemFormat : CompositeFormat
	{
		// Token: 0x06000138 RID: 312 RVA: 0x00005D24 File Offset: 0x00003F24
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".csd"
			}) && (expectedObjectType.Equals(typeof(CocosItem)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005D74 File Offset: 0x00003F74
		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().IsSubclassOf(typeof(CocosItem));
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005D90 File Offset: 0x00003F90
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new CocosItem(file);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005D98 File Offset: 0x00003F98
		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00005D9C File Offset: 0x00003F9C
		public override List<string> GetPretreatmentTypes()
		{
			return new List<string>
			{
				".csd"
			};
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00005DBC File Offset: 0x00003FBC
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

		// Token: 0x0600013E RID: 318 RVA: 0x00005DF0 File Offset: 0x00003FF0
		public override List<string> GetFilterTypes()
		{
			return new List<string>
			{
				".udf"
			};
		}
	}
}
