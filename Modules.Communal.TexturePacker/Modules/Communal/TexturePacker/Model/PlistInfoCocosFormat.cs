using System;
using CocoStudio.Projects;
using CocoStudio.Projects.Formates;
using Mono.Addins;
using MonoDevelop.Core;

namespace Modules.Communal.TexturePacker.Model
{
	// Token: 0x02000010 RID: 16
	[Extension(typeof(IFileFormat))]
	internal class PlistInfoCocosFormat : FileFormat
	{
		// Token: 0x060000AE RID: 174 RVA: 0x00004E50 File Offset: 0x00003050
		protected override bool OnCanReadFile(FilePath file, Type expectedObjectType)
		{
			return FileFormat.CheckFileSuffix(file, new string[]
			{
				".csi"
			}) && (expectedObjectType.Equals(typeof(CocosItem)) || expectedObjectType.Equals(typeof(ResourceItem)));
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004EA0 File Offset: 0x000030A0
		protected override bool OnCanWriteFile(object obj)
		{
			return obj.GetType().IsSubclassOf(typeof(PlistInfoCocosItem));
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004EBC File Offset: 0x000030BC
		protected override object OnReadFile(FilePath file, Type expectedType, IProgressMonitor monitor)
		{
			return new PlistInfoCocosItem(file);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004EC4 File Offset: 0x000030C4
		protected override void OnWriteFile(FilePath file, object obj, IProgressMonitor monitor)
		{
		}
	}
}
