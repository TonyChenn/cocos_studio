using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using ICSharpCode.NRefactory.Documentation;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Projects.Extensions;
using Monodoc;

namespace MonoDevelop.Projects
{
	// Token: 0x02000188 RID: 392
	public static class HelpService
	{
		/// <summary>
		/// Starts loading the MonoDoc tree in the background.
		/// </summary>
		// Token: 0x06000F48 RID: 3912 RVA: 0x000392F8 File Offset: 0x000374F8
		public static void AsyncInitialize()
		{
			lock (HelpService.helpTreeLock)
			{
				if (HelpService.helpTreeInitialized)
				{
					return;
				}
			}
			ThreadPool.QueueUserWorkItem(delegate(object param0)
			{
				HelpService.InitializeHelpTree();
			});
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0003936C File Offset: 0x0003756C
		private static void InitializeHelpTree()
		{
			lock (HelpService.helpTreeLock)
			{
				if (!HelpService.helpTreeInitialized)
				{
					Counters.HelpServiceInitialization.BeginTiming();
					try
					{
						HelpService.helpTree = RootTree.LoadTree();
						foreach (object obj2 in AddinManager.GetExtensionNodes("/MonoDevelop/ProjectModel/MonoDocSources"))
						{
							HelpService.sources.Add(((MonoDocSourceNode)obj2).Directory);
						}
						if (Platform.IsWindows)
						{
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
							HelpService.sources.Add(Path.Combine(folderPath, "Monodoc"));
						}
						foreach (string item in from d in HelpService.sources.ToList<string>()
						where !Directory.Exists(d)
						select d)
						{
							HelpService.sources.Remove(item);
						}
						foreach (string text in HelpService.sources)
						{
							HelpService.helpTree.AddSource(text);
						}
					}
					catch (Exception ex)
					{
						if (!(ex is ThreadAbortException) && !(ex.InnerException is ThreadAbortException))
						{
							LoggingService.LogError("Monodoc documentation tree could not be loaded.", ex);
						}
					}
					finally
					{
						HelpService.helpTreeInitialized = true;
						Counters.HelpServiceInitialization.EndTiming();
					}
				}
			}
		}

		/// <summary>
		/// A MonoDoc docs tree.
		/// </summary>
		/// <remarks>
		/// The tree is background-loaded the help service, and accessing the property will block until it is finished 
		/// loading. If you don't wish to block, check the <see cref="P:MonoDevelop.Projects.HelpService.TreeInitialized" /> property first.
		///  </remarks>
		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0003959C File Offset: 0x0003779C
		public static RootTree HelpTree
		{
			get
			{
				RootTree result;
				lock (HelpService.helpTreeLock)
				{
					if (!HelpService.helpTreeInitialized)
					{
						HelpService.InitializeHelpTree();
					}
					result = HelpService.helpTree;
				}
				return result;
			}
		}

		/// <summary>
		/// Whether the MonoDoc docs tree has finished loading.
		/// </summary>
		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000F4B RID: 3915 RVA: 0x000395E8 File Offset: 0x000377E8
		public static bool TreeInitialized
		{
			get
			{
				return HelpService.helpTreeInitialized;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000F4C RID: 3916 RVA: 0x000395EF File Offset: 0x000377EF
		public static IEnumerable<string> Sources
		{
			get
			{
				return HelpService.sources;
			}
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x000395F8 File Offset: 0x000377F8
		public static string GetMonoDocHelpUrl(ResolveResult result)
		{
			if (result == null)
			{
				return null;
			}
			if (result is NamespaceResolveResult)
			{
				string namespaceName = ((NamespaceResolveResult)result).NamespaceName;
				Node node;
				if (!string.IsNullOrEmpty(namespaceName) && HelpService.HelpTree != null && HelpService.HelpTree.RenderUrl("N:" + namespaceName, ref node) != null)
				{
					return "N:" + namespaceName;
				}
				return null;
			}
			else
			{
				IMember member = null;
				if (result is MemberResolveResult)
				{
					member = ((MemberResolveResult)result).Member;
				}
				if (member != null && member.GetMonodocDocumentation() != null)
				{
					return member.GetIdString();
				}
				IType type = result.Type;
				if (type != null && !string.IsNullOrEmpty(type.FullName))
				{
					string text = "T:" + type.FullName;
					try
					{
						RootTree rootTree = HelpService.HelpTree;
						if (rootTree != null && rootTree.GetHelpXml(text) != null)
						{
							return text;
						}
					}
					catch (Exception)
					{
						return null;
					}
				}
				return null;
			}
		}

		// Token: 0x04000469 RID: 1129
		private static RootTree helpTree;

		// Token: 0x0400046A RID: 1130
		private static bool helpTreeInitialized;

		// Token: 0x0400046B RID: 1131
		private static object helpTreeLock = new object();

		// Token: 0x0400046C RID: 1132
		private static HashSet<string> sources = new HashSet<string>();
	}
}
