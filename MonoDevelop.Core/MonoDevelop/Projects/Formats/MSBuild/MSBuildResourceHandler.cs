using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C4 RID: 452
	public class MSBuildResourceHandler : IResourceHandler
	{
		// Token: 0x06001148 RID: 4424 RVA: 0x00046344 File Offset: 0x00044544
		public virtual string GetDefaultResourceId(ProjectFile file)
		{
			string text = file.ProjectVirtualPath;
			text = FileService.NormalizeRelativePath(text);
			text = Path.Combine(Path.GetDirectoryName(text).Replace(' ', '_'), Path.GetFileName(text));
			string str;
			string text2;
			string str2;
			if (string.Compare(Path.GetExtension(text), ".resx", true) == 0)
			{
				text = Path.ChangeExtension(text, ".resources");
			}
			else if (MSBuildProjectService.TrySplitResourceName(text, out str, out text2, out str2))
			{
				text = str + "." + str2;
			}
			string text3 = text.Replace(Path.DirectorySeparatorChar, '.');
			DotNetProject dotNetProject = file.Project as DotNetProject;
			if (dotNetProject == null || string.IsNullOrEmpty(dotNetProject.DefaultNamespace))
			{
				return text3;
			}
			return dotNetProject.DefaultNamespace + "." + text3;
		}

		// Token: 0x040004FF RID: 1279
		public static MSBuildResourceHandler Instance = new MSBuildResourceHandler();
	}
}
