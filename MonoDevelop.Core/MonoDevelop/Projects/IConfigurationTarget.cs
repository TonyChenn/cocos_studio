using System;
using System.Collections.ObjectModel;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x0200010D RID: 269
	public interface IConfigurationTarget : IBuildTarget, IWorkspaceObject, IExtendedDataItem, IFolderItem, IDisposable
	{
		// Token: 0x060009FA RID: 2554
		ReadOnlyCollection<string> GetConfigurations();

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060009FB RID: 2555
		IItemConfigurationCollection Configurations { get; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060009FC RID: 2556
		// (set) Token: 0x060009FD RID: 2557
		ItemConfiguration DefaultConfiguration { get; set; }

		// Token: 0x060009FE RID: 2558
		ItemConfiguration CreateConfiguration(string name);

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060009FF RID: 2559
		// (set) Token: 0x06000A00 RID: 2560
		string DefaultConfigurationId { get; set; }
	}
}
