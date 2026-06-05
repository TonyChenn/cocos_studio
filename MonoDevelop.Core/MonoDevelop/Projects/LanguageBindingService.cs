using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects
{
	// Token: 0x02000223 RID: 547
	public static class LanguageBindingService
	{
		// Token: 0x0600147F RID: 5247 RVA: 0x00054835 File Offset: 0x00052A35
		static LanguageBindingService()
		{
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/LanguageBindings", delegate(object sender, ExtensionNodeEventArgs args)
			{
				LanguageBindingCodon languageBindingCodon = (LanguageBindingCodon)args.ExtensionNode;
				switch (args.Change)
				{
				case ExtensionChange.Add:
				{
					LanguageBindingService.languageBindingCodons.Add(languageBindingCodon);
					IDotNetLanguageBinding dotNetLanguageBinding = languageBindingCodon as IDotNetLanguageBinding;
					if (dotNetLanguageBinding != null)
					{
						object obj = dotNetLanguageBinding.CreateCompilationParameters(null);
						if (obj != null)
						{
							Services.ProjectService.DataContext.IncludeType(obj.GetType());
						}
						obj = dotNetLanguageBinding.CreateProjectParameters(null);
						if (obj != null)
						{
							Services.ProjectService.DataContext.IncludeType(obj.GetType());
						}
					}
					break;
				}
				case ExtensionChange.Remove:
					LanguageBindingService.languageBindingCodons.Remove(languageBindingCodon);
					break;
				}
				LanguageBindingService.languageBindings = null;
			});
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001480 RID: 5248 RVA: 0x0005486E File Offset: 0x00052A6E
		public static IEnumerable<ILanguageBinding> LanguageBindings
		{
			get
			{
				LanguageBindingService.CheckBindings();
				return LanguageBindingService.languageBindings;
			}
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x00054882 File Offset: 0x00052A82
		private static void CheckBindings()
		{
			if (LanguageBindingService.languageBindings == null)
			{
				LanguageBindingService.languageBindings = new List<ILanguageBinding>(from codon in LanguageBindingService.languageBindingCodons
				select codon.LanguageBinding);
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x000548D8 File Offset: 0x00052AD8
		public static ILanguageBinding GetBindingPerFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				LoggingService.LogWarning("Cannot get binding for null filename at {0}", new object[]
				{
					Environment.StackTrace
				});
				return null;
			}
			LanguageBindingService.CheckBindings();
			return LanguageBindingService.languageBindings.FirstOrDefault((ILanguageBinding binding) => binding.IsSourceCodeFile(fileName));
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x00054954 File Offset: 0x00052B54
		public static ILanguageBinding GetBindingPerLanguageName(string language)
		{
			if (string.IsNullOrEmpty(language))
			{
				LoggingService.LogWarning("Cannot get binding for null language at {0}", new object[]
				{
					Environment.StackTrace
				});
				return null;
			}
			LanguageBindingService.CheckBindings();
			return LanguageBindingService.languageBindings.FirstOrDefault((ILanguageBinding binding) => binding.Language == language);
		}

		// Token: 0x04000624 RID: 1572
		private static List<LanguageBindingCodon> languageBindingCodons = new List<LanguageBindingCodon>();

		// Token: 0x04000625 RID: 1573
		private static List<ILanguageBinding> languageBindings = null;
	}
}
