using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoDevelop.Projects.Policies
{
	/// <summary>
	/// A set of policies. Policies are identified by type.
	/// </summary>
	// Token: 0x020001E9 RID: 489
	public abstract class PolicyContainer : IPolicyProvider
	{
		/// <summary>
		/// Returns true if there isn't any policy defined.
		/// </summary>
		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x0600126B RID: 4715 RVA: 0x0004AC75 File Offset: 0x00048E75
		public bool IsEmpty
		{
			get
			{
				return this.policies == null || this.policies.Count == 0;
			}
		}

		/// <summary>
		/// The Get methods return policies taking into account inheritance. If a policy
		/// can't be found it may return null, but never an 'undefined' policy.
		/// </summary>
		/// <returns>
		/// The policy of the given type, or null if not found.
		/// </returns>
		// Token: 0x0600126C RID: 4716 RVA: 0x0004AC90 File Offset: 0x00048E90
		public T Get<T>() where T : class, IEquatable<T>, new()
		{
			object obj;
			if (this.policies != null && this.policies.TryGetValue(typeof(T), null, out obj))
			{
				if (!PolicyService.IsUndefinedPolicy<object>(obj))
				{
					return (T)((object)obj);
				}
				return this.GetDefaultPolicy<T>();
			}
			else
			{
				if (this.IsRoot)
				{
					return this.GetDefaultPolicy<T>();
				}
				return this.ParentPolicies.Get<T>();
			}
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x0004ACF0 File Offset: 0x00048EF0
		public T Get<T>(string scope) where T : class, IEquatable<T>, new()
		{
			return this.Get<T>(new string[]
			{
				scope
			});
		}

		// Token: 0x0600126E RID: 4718 RVA: 0x0004AD10 File Offset: 0x00048F10
		public T Get<T>(IEnumerable<string> scopes) where T : class, IEquatable<T>, new()
		{
			foreach (string scope in scopes)
			{
				PolicyContainer policyContainer = this;
				while (policyContainer != null)
				{
					if (policyContainer.DirectHas<T>(scope))
					{
						T t = policyContainer.DirectGet<T>(scope);
						if (!PolicyService.IsUndefinedPolicy<T>(t))
						{
							return t;
						}
						break;
					}
					else
					{
						policyContainer = policyContainer.ParentPolicies;
					}
				}
			}
			return this.GetDefaultPolicy<T>(scopes);
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x0004AD8C File Offset: 0x00048F8C
		public void Set<T>(T value) where T : class, IEquatable<T>, new()
		{
			this.Set<T>(value, null);
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x0004AD98 File Offset: 0x00048F98
		public void Set<T>(T value, string scope) where T : class, IEquatable<T>, new()
		{
			this.CheckReadOnly();
			PolicyKey key = new PolicyKey(typeof(T), scope);
			if (this.policies == null)
			{
				this.policies = new PolicyDictionary();
			}
			else if (value != null)
			{
				object obj = null;
				this.policies.TryGetValue(key, out obj);
				if (obj != null && ((IEquatable<T>)obj).Equals(value))
				{
					return;
				}
			}
			this.policies[key] = value;
			this.OnPolicyChanged(key.PolicyType, key.Scope);
		}

		// Token: 0x06001271 RID: 4721 RVA: 0x0004AE24 File Offset: 0x00049024
		internal void InternalSet(Type t, string scope, object ob)
		{
			PolicyKey key = new PolicyKey(t, scope);
			if (this.policies == null)
			{
				this.policies = new PolicyDictionary();
			}
			this.policies[key] = ob;
			this.OnPolicyChanged(key.PolicyType, key.Scope);
		}

		/// <summary>
		/// Removes all policies defined in this container
		/// </summary>
		// Token: 0x06001272 RID: 4722 RVA: 0x0004AE70 File Offset: 0x00049070
		public void Clear()
		{
			PolicyDictionary policyDictionary = this.policies;
			this.policies = null;
			foreach (PolicyKey policyKey in policyDictionary.Keys)
			{
				this.OnPolicyChanged(policyKey.PolicyType, policyKey.Scope);
			}
		}

		// Token: 0x06001273 RID: 4723 RVA: 0x0004AEE0 File Offset: 0x000490E0
		public bool Remove<T>() where T : class, IEquatable<T>, new()
		{
			this.CheckReadOnly();
			return this.Remove<T>(null);
		}

		// Token: 0x06001274 RID: 4724 RVA: 0x0004AEEF File Offset: 0x000490EF
		public bool Remove<T>(string scope) where T : class, IEquatable<T>, new()
		{
			this.CheckReadOnly();
			return this.InternalRemove(typeof(T), scope);
		}

		// Token: 0x06001275 RID: 4725 RVA: 0x0004AF08 File Offset: 0x00049108
		internal bool InternalRemove(Type type, string scope)
		{
			if (this.policies != null && this.policies.Remove(new PolicyKey(type, scope)))
			{
				this.OnPolicyChanged(type, scope);
				if (this.policies.Count == 0)
				{
					this.policies = null;
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001276 RID: 4726 RVA: 0x0004AF48 File Offset: 0x00049148
		internal void RemoveAll(Type type)
		{
			if (this.policies != null)
			{
				foreach (KeyValuePair<PolicyKey, object> keyValuePair in this.policies.ToArray<KeyValuePair<PolicyKey, object>>())
				{
					if (keyValuePair.Key.PolicyType == type)
					{
						this.policies.Remove(keyValuePair.Key);
						this.OnPolicyChanged(type, keyValuePair.Key.Scope);
					}
				}
				if (this.policies.Count == 0)
				{
					this.policies = null;
				}
			}
		}

		// Token: 0x06001277 RID: 4727 RVA: 0x0004AFDC File Offset: 0x000491DC
		internal void RemoveAll(PolicyKey[] keys)
		{
			if (this.policies != null)
			{
				foreach (PolicyKey key in keys)
				{
					this.policies.Remove(key);
					this.OnPolicyChanged(key.PolicyType, key.Scope);
				}
				if (this.policies.Count == 0)
				{
					this.policies = null;
				}
			}
		}

		/// <summary>
		/// Copies the policies defined in another container
		/// </summary>
		/// <param name="other">
		/// A policy container from which to copy the policies
		/// </param>
		/// <remarks>
		/// Policies of this container are removed or replaced by policies defined in the
		/// provided container.
		/// </remarks>
		// Token: 0x06001278 RID: 4728 RVA: 0x0004B044 File Offset: 0x00049244
		public void CopyFrom(PolicyContainer other)
		{
			if (other.policies == null && this.policies == null)
			{
				return;
			}
			if (other.policies != null)
			{
				foreach (KeyValuePair<PolicyKey, object> keyValuePair in other.policies)
				{
					object obj;
					if (this.policies == null || !this.policies.TryGetValue(keyValuePair.Key, out obj) || obj == null || !obj.Equals(keyValuePair.Value))
					{
						if (this.policies == null)
						{
							this.policies = new PolicyDictionary();
						}
						this.policies[keyValuePair.Key] = keyValuePair.Value;
						this.OnPolicyChanged(keyValuePair.Key.PolicyType, keyValuePair.Key.Scope);
					}
				}
			}
			if (this.policies != null)
			{
				foreach (PolicyKey key in this.policies.Keys.ToArray<PolicyKey>())
				{
					if (other.policies == null || !other.policies.ContainsKey(key))
					{
						this.policies.Remove(key);
						this.OnPolicyChanged(key.PolicyType, key.Scope);
					}
				}
			}
		}

		/// <summary>
		/// Import the policies defined by another policy container
		/// </summary>
		/// <param name="source">
		/// The policy container to be imported
		/// </param>
		/// <param name="includeParentPolicies">
		/// If <c>true</c>, policies defined by all ancestors of polContainer will also
		/// be imported
		/// </param>
		/// <remarks>
		/// This method adds or replaces policies defined in the source container into
		/// this container. Policies in this container which are not defined in the source container
		/// are not modified or removed.
		/// </remarks>
		// Token: 0x06001279 RID: 4729 RVA: 0x0004B1A8 File Offset: 0x000493A8
		public void Import(PolicyContainer source, bool includeParentPolicies)
		{
			if (includeParentPolicies && source.ParentPolicies != null)
			{
				this.Import(source.ParentPolicies, true);
			}
			if (source.policies == null && this.policies == null)
			{
				return;
			}
			if (source.policies != null)
			{
				foreach (KeyValuePair<PolicyKey, object> keyValuePair in source.policies)
				{
					object obj;
					if (this.policies == null || !this.policies.TryGetValue(keyValuePair.Key, out obj) || obj == null || !obj.Equals(keyValuePair.Value))
					{
						if (this.policies == null)
						{
							this.policies = new PolicyDictionary();
						}
						this.policies[keyValuePair.Key] = keyValuePair.Value;
						this.OnPolicyChanged(keyValuePair.Key.PolicyType, keyValuePair.Key.Scope);
					}
				}
			}
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x0004B464 File Offset: 0x00049664
		public IEnumerable<string> GetScopes<T>()
		{
			foreach (PolicyKey pk in this.policies.Keys)
			{
				PolicyKey policyKey = pk;
				if (policyKey.PolicyType == typeof(T))
				{
					PolicyKey policyKey2 = pk;
					yield return policyKey2.Scope;
				}
			}
			yield break;
		}

		// Token: 0x0600127B RID: 4731 RVA: 0x0004B6D4 File Offset: 0x000498D4
		public IEnumerable<ScopedPolicy<T>> GetScoped<T>() where T : class, IEquatable<T>, new()
		{
			if (this.policies != null)
			{
				foreach (KeyValuePair<PolicyKey, object> pinfo in this.policies)
				{
					KeyValuePair<PolicyKey, object> keyValuePair = pinfo;
					if (keyValuePair.Key.PolicyType == typeof(T))
					{
						KeyValuePair<PolicyKey, object> keyValuePair2 = pinfo;
						T policy = (T)((object)keyValuePair2.Value);
						KeyValuePair<PolicyKey, object> keyValuePair3 = pinfo;
						yield return new ScopedPolicy<T>(policy, keyValuePair3.Key.Scope);
					}
				}
				T pol = this.Get<T>();
				if (pol != null && !PolicyService.IsUndefinedPolicy<T>(pol))
				{
					yield return new ScopedPolicy<T>(pol, null);
				}
			}
			yield break;
		}

		// Token: 0x0600127C RID: 4732 RVA: 0x0004B944 File Offset: 0x00049B44
		internal IEnumerable<ScopedPolicy> GetScoped(Type t)
		{
			if (this.policies != null)
			{
				foreach (KeyValuePair<PolicyKey, object> pinfo in this.policies)
				{
					KeyValuePair<PolicyKey, object> keyValuePair = pinfo;
					if (keyValuePair.Key.PolicyType == t)
					{
						KeyValuePair<PolicyKey, object> keyValuePair2 = pinfo;
						object value = keyValuePair2.Value;
						KeyValuePair<PolicyKey, object> keyValuePair3 = pinfo;
						yield return new ScopedPolicy(t, value, keyValuePair3.Key.Scope);
					}
				}
			}
			object pol = this.Get(t);
			if (pol != null)
			{
				yield return new ScopedPolicy(t, pol, null);
			}
			yield break;
		}

		// Token: 0x0600127D RID: 4733 RVA: 0x0004B968 File Offset: 0x00049B68
		internal object Get(Type type)
		{
			return this.Get(type, null);
		}

		// Token: 0x0600127E RID: 4734 RVA: 0x0004B974 File Offset: 0x00049B74
		internal object Get(Type type, string scope)
		{
			if (this.policies == null)
			{
				return null;
			}
			object obj;
			if (this.policies.TryGetValue(type, scope, out obj) && PolicyService.IsUndefinedPolicy<object>(obj))
			{
				return null;
			}
			return obj;
		}

		/// <summary>
		/// Gets a list of all policies defined in this container (not inherited)
		/// </summary>
		// Token: 0x0600127F RID: 4735 RVA: 0x0004B9DF File Offset: 0x00049BDF
		public IEnumerable<ScopedPolicy> DirectGetAll()
		{
			if (this.policies == null)
			{
				return new ScopedPolicy[0];
			}
			return from pk in this.policies
			select new ScopedPolicy(pk.Key.PolicyType, pk.Value, pk.Key.Scope);
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001280 RID: 4736
		public abstract bool IsRoot { get; }

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06001281 RID: 4737 RVA: 0x0004BA18 File Offset: 0x00049C18
		// (remove) Token: 0x06001282 RID: 4738 RVA: 0x0004BA50 File Offset: 0x00049C50
		public event EventHandler<PolicyChangedEventArgs> PolicyChanged;

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x0004BA85 File Offset: 0x00049C85
		internal PolicyDictionary Policies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x06001284 RID: 4740 RVA: 0x0004BA8D File Offset: 0x00049C8D
		public T DirectGet<T>() where T : class, IEquatable<T>, new()
		{
			return this.DirectGet<T>(null);
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x0004BA98 File Offset: 0x00049C98
		public T DirectGet<T>(string scope) where T : class, IEquatable<T>, new()
		{
			object obj;
			if (this.policies != null && this.policies.TryGetValue(typeof(T), scope, out obj))
			{
				return (T)((object)obj);
			}
			return default(T);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x0004BAD7 File Offset: 0x00049CD7
		public bool DirectHas<T>()
		{
			return this.DirectHas<T>(null);
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x0004BAE0 File Offset: 0x00049CE0
		public bool DirectHas<T>(string scope)
		{
			return this.policies != null && this.policies.ContainsKey(new PolicyKey(typeof(T), scope));
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x0004BB08 File Offset: 0x00049D08
		public bool DirectHas<T>(IEnumerable<string> scopes)
		{
			foreach (string scope in scopes)
			{
				if (this.DirectHas<T>(scope))
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// The set of policies from which inherit policies when not found in this container
		/// </summary>
		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06001289 RID: 4745
		public abstract PolicyContainer ParentPolicies { get; }

		// Token: 0x0600128A RID: 4746 RVA: 0x0004BB5C File Offset: 0x00049D5C
		protected virtual void OnPolicyChanged(Type policyType, string scope)
		{
			if (this.PolicyChanged != null)
			{
				this.PolicyChanged(this, new PolicyChangedEventArgs(policyType, scope));
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x0004BB79 File Offset: 0x00049D79
		protected virtual T GetDefaultPolicy<T>() where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetDefaultPolicy<T>();
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x0004BB80 File Offset: 0x00049D80
		protected virtual T GetDefaultPolicy<T>(IEnumerable<string> scopes) where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetDefaultPolicy<T>(scopes);
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x0004BB88 File Offset: 0x00049D88
		// (set) Token: 0x0600128E RID: 4750 RVA: 0x0004BB90 File Offset: 0x00049D90
		public virtual bool ReadOnly { get; internal set; }

		// Token: 0x0600128F RID: 4751 RVA: 0x0004BB99 File Offset: 0x00049D99
		private void CheckReadOnly()
		{
			if (this.ReadOnly)
			{
				throw new InvalidOperationException("This PolicyContainer can't be modified");
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x0004BBAE File Offset: 0x00049DAE
		PolicyContainer IPolicyProvider.Policies
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0400056F RID: 1391
		internal PolicyDictionary policies;
	}
}
