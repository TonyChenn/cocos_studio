using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	public static class CodeBehind
	{
		public static IEnumerable<string> GuessDependencies(DotNetProject proj, ProjectFile file, IEnumerable<string> groupedExtensions)
		{
			if (!string.IsNullOrEmpty(file.DependsOn) || proj.LanguageBinding == null)
			{
				return null;
			}
			string text = proj.LanguageBinding.GetFileName("a");
			text = text.Substring(1, text.Length - 1);
			if (file.Name.EndsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				string fileName = Path.GetFileName(file.Name);
				fileName = fileName.Substring(0, fileName.Length - text.Length);
				if (fileName.EndsWith(".designer", StringComparison.OrdinalIgnoreCase))
				{
					fileName = fileName.Substring(0, fileName.Length - 9);
				}
				foreach (string groupedExtension in groupedExtensions)
				{
					if (fileName.EndsWith(groupedExtension, StringComparison.OrdinalIgnoreCase))
					{
						string text2 = Path.Combine(Path.GetDirectoryName(file.FilePath), fileName);
						if (File.Exists(text2))
						{
							file.DependsOn = fileName;
							return new string[1] { text2 };
						}
					}
				}
			}
			else
			{
				foreach (string groupedExtension2 in groupedExtensions)
				{
					if (!file.FilePath.ToString().EndsWith(groupedExtension2, StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					string text3 = string.Concat(file.FilePath, text);
					if (!File.Exists(text3))
					{
						text3 = null;
					}
					string text4 = string.Concat(file.FilePath, ".designer", text);
					if (!File.Exists(text4))
					{
						text4 = string.Concat(file.FilePath, ".Designer", text);
						if (!File.Exists(text4))
						{
							text4 = null;
						}
					}
					if (text4 != null)
					{
						return (text3 != null) ? new string[2] { text4, text3 } : new string[1] { text4 };
					}
					return (text3 != null) ? new string[1] { text3 } : null;
				}
			}
			return null;
		}

		public static IUnresolvedTypeDefinition GetDesignerClass(IType cls)
		{
			if (cls.GetDefinition().Parts.Count == 1)
			{
				return null;
			}
			string value = ".designer" + Path.GetExtension(cls.GetDefinition().Region.FileName);
			foreach (IUnresolvedTypeDefinition part in cls.GetDefinition().Parts)
			{
				if (part.Region.FileName.EndsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return part;
				}
			}
			return null;
		}

		public static IUnresolvedTypeDefinition GetNonDesignerClass(IType cls)
		{
			if (cls.GetDefinition().Parts.Count == 1)
			{
				return null;
			}
			string value = ".designer" + Path.GetExtension(cls.GetDefinition().Region.FileName);
			foreach (IUnresolvedTypeDefinition part in cls.GetDefinition().Parts)
			{
				if (!part.Region.FileName.EndsWith(value, StringComparison.OrdinalIgnoreCase))
				{
					return part;
				}
			}
			return null;
		}

		public static bool IsDesignerFile(string name)
		{
			string extension = Path.GetExtension(name);
			if (!string.IsNullOrEmpty(extension))
			{
				return name.EndsWith(".designer" + extension, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}
	}
}
