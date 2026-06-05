using System;
using System.Collections.Generic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001EA RID: 490
	[DataItem("Policies")]
	public class PolicyBag : PolicyContainer, ICustomDataItem
	{
		// Token: 0x06001293 RID: 4755 RVA: 0x0004BBB9 File Offset: 0x00049DB9
		public PolicyBag(SolutionItem owner)
		{
			this.Owner = owner;
		}

		// Token: 0x06001294 RID: 4756 RVA: 0x0004BBC8 File Offset: 0x00049DC8
		internal PolicyBag()
		{
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x0004BBD0 File Offset: 0x00049DD0
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x0004BBD8 File Offset: 0x00049DD8
		public SolutionItem Owner { get; internal set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x0004BBE1 File Offset: 0x00049DE1
		public override bool IsRoot
		{
			get
			{
				return this.Owner == null || this.Owner.ParentFolder == null;
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06001298 RID: 4760 RVA: 0x0004BBFB File Offset: 0x00049DFB
		public override PolicyContainer ParentPolicies
		{
			get
			{
				if (this.Owner != null && this.Owner.ParentFolder != null)
				{
					return this.Owner.ParentFolder.Policies;
				}
				return null;
			}
		}

		// Token: 0x06001299 RID: 4761 RVA: 0x0004BC24 File Offset: 0x00049E24
		private bool DirectHas(Type type, string scope)
		{
			return this.policies != null && this.policies.ContainsKey(new PolicyKey(type, scope));
		}

		// Token: 0x0600129A RID: 4762 RVA: 0x0004BC44 File Offset: 0x00049E44
		DataCollection ICustomDataItem.Serialize(ITypeSerializer handler)
		{
			if (this.policies == null)
			{
				return null;
			}
			DataCollection dataCollection = new DataCollection();
			foreach (KeyValuePair<PolicyKey, object> keyValuePair in this.policies)
			{
				dataCollection.Add(PolicyService.DiffSerialize(keyValuePair.Key.PolicyType, keyValuePair.Value, keyValuePair.Key.Scope));
			}
			return dataCollection;
		}

		// Token: 0x0600129B RID: 4763 RVA: 0x0004BCD4 File Offset: 0x00049ED4
		void ICustomDataItem.Deserialize(ITypeSerializer handler, DataCollection data)
		{
			if (data.Count == 0)
			{
				return;
			}
			this.policies = new PolicyDictionary();
			foreach (object obj in data)
			{
				DataNode dataNode = (DataNode)obj;
				try
				{
					if (dataNode is DataItem)
					{
						ScopedPolicy scopedPolicy = PolicyService.DiffDeserialize((DataItem)dataNode);
						this.policies.Add(scopedPolicy);
					}
				}
				catch (Exception ex)
				{
					if (handler.SerializationContext.ProgressMonitor != null)
					{
						handler.SerializationContext.ProgressMonitor.ReportError(ex.Message, ex);
					}
					else
					{
						LoggingService.LogError(ex.Message, ex);
					}
				}
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x0004BDA0 File Offset: 0x00049FA0
		internal void PropagatePolicyChangeEvent(PolicyChangedEventArgs args)
		{
			SolutionFolder solutionFolder = this.Owner as SolutionFolder;
			if (solutionFolder != null)
			{
				foreach (SolutionItem solutionItem in solutionFolder.Items)
				{
					if (!solutionItem.Policies.DirectHas(args.PolicyType, args.Scope))
					{
						solutionItem.Policies.OnPolicyChanged(args.PolicyType, args.Scope);
					}
				}
			}
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x0004BE28 File Offset: 0x0004A028
		protected override void OnPolicyChanged(Type policyType, string scope)
		{
			base.OnPolicyChanged(policyType, scope);
			this.PropagatePolicyChangeEvent(new PolicyChangedEventArgs(policyType, scope));
		}
	}
}
