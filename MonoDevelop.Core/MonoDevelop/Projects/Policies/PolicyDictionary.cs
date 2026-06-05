using System;
using System.Collections.Generic;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001F6 RID: 502
	internal class PolicyDictionary : Dictionary<PolicyKey, object>
	{
		// Token: 0x17000401 RID: 1025
		public object this[Type policyType]
		{
			get
			{
				return base[new PolicyKey(policyType, null)];
			}
			set
			{
				base[new PolicyKey(policyType, null)] = value;
			}
		}

		// Token: 0x17000402 RID: 1026
		public object this[Type policyType, string scope]
		{
			get
			{
				return base[new PolicyKey(policyType, scope)];
			}
			set
			{
				base[new PolicyKey(policyType, scope)] = value;
			}
		}

		// Token: 0x06001321 RID: 4897 RVA: 0x0004EF30 File Offset: 0x0004D130
		public bool TryGetValue(Type policyTypeKey, string scopeKey, out object value)
		{
			return base.TryGetValue(new PolicyKey(policyTypeKey, scopeKey), out value);
		}

		// Token: 0x06001322 RID: 4898 RVA: 0x0004EF40 File Offset: 0x0004D140
		public bool TryGetValue(Type policyTypeKey, out object value)
		{
			return base.TryGetValue(new PolicyKey(policyTypeKey, null), out value);
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0004EF50 File Offset: 0x0004D150
		public void Add(ScopedPolicy scopedPolicy)
		{
			base.Add(new PolicyKey(scopedPolicy.PolicyType, scopedPolicy.Scope), scopedPolicy.Policy);
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0004EF6F File Offset: 0x0004D16F
		public bool ContainsKey(Type policyType, string scope)
		{
			return base.ContainsKey(new PolicyKey(policyType, scope));
		}
	}
}
