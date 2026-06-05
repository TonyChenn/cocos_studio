using System;
using Mono.Addins;
using Mono.Addins.Setup;
using MonoDevelop.Core.AddIns;

namespace MonoDevelop.Core
{
	// Token: 0x02000221 RID: 545
	public class ApplicationService
	{
		// Token: 0x0600146F RID: 5231 RVA: 0x000545E8 File Offset: 0x000527E8
		public int StartApplication(string appId, string[] parameters)
		{
			IApplication application = this.GetApplication(appId);
			if (application == null)
			{
				throw new InstallException("Application not found: " + appId);
			}
			return application.Run(parameters);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00054618 File Offset: 0x00052818
		public IApplication GetApplication(string appId)
		{
			ExtensionNode extensionNode = AddinManager.GetExtensionNode("/MonoDevelop/Core/Applications/" + appId);
			if (extensionNode == null)
			{
				return null;
			}
			ApplicationExtensionNode applicationExtensionNode = extensionNode as ApplicationExtensionNode;
			if (applicationExtensionNode == null)
			{
				throw new Exception("Invalid node type");
			}
			return (IApplication)applicationExtensionNode.CreateInstance();
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0005465C File Offset: 0x0005285C
		public IApplicationInfo[] GetApplications()
		{
			ExtensionNodeList extensionNodes = AddinManager.GetExtensionNodes("/MonoDevelop/Core/Applications");
			IApplicationInfo[] array = new IApplicationInfo[extensionNodes.Count];
			for (int i = 0; i < extensionNodes.Count; i++)
			{
				array[i] = (ApplicationExtensionNode)extensionNodes[i];
			}
			return array;
		}
	}
}
