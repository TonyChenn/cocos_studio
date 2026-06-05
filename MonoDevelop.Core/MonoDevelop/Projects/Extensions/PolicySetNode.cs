using System;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Projects.Policies;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200019F RID: 415
	[ExtensionNode(Description = "A named set of defined policies")]
	[ExtensionNodeChild(typeof(PolicyResourceNode), "Policies")]
	internal class PolicySetNode : ExtensionNode
	{
		// Token: 0x06000FDB RID: 4059 RVA: 0x0003AC0C File Offset: 0x00038E0C
		protected override void OnChildNodeAdded(ExtensionNode node)
		{
			PolicyResourceNode policyResourceNode = (PolicyResourceNode)node;
			using (StreamReader stream = policyResourceNode.GetStream())
			{
				policyResourceNode.AddedKeys = this.polSet.AddSerializedPolicies(stream);
			}
			base.OnChildNodeAdded(node);
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x0003AC5C File Offset: 0x00038E5C
		protected override void OnChildNodeRemoved(ExtensionNode node)
		{
			PolicyResourceNode policyResourceNode = (PolicyResourceNode)node;
			this.polSet.RemoveAll(policyResourceNode.AddedKeys);
			base.OnChildNodeRemoved(node);
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0003AC88 File Offset: 0x00038E88
		public PolicySet Set
		{
			get
			{
				if (this.polSet == null)
				{
					this.polSet = new PolicySet(base.Id, GettextCatalog.GetString(this.name));
					this.polSet.Visible = this.visible;
					this.polSet.AllowDiffSerialize = this.allowDiffSerialize;
					foreach (object obj in base.ChildNodes)
					{
						PolicyResourceNode policyResourceNode = (PolicyResourceNode)obj;
						try
						{
							using (StreamReader stream = policyResourceNode.GetStream())
							{
								policyResourceNode.AddedKeys = this.polSet.AddSerializedPolicies(stream);
							}
						}
						catch (Exception ex)
						{
							LoggingService.LogError("Error deserialising policies for {0}@{1}:\n{2}", new object[]
							{
								policyResourceNode.Addin,
								policyResourceNode.Path,
								ex
							});
						}
					}
				}
				return this.polSet;
			}
		}

		// Token: 0x0400049A RID: 1178
		private PolicySet polSet;

		// Token: 0x0400049B RID: 1179
		[NodeAttribute("_name", Required = true)]
		private string name;

		// Token: 0x0400049C RID: 1180
		[NodeAttribute("visible")]
		private bool visible = true;

		// Token: 0x0400049D RID: 1181
		[NodeAttribute("allowDiffSerialize")]
		private bool allowDiffSerialize;
	}
}
