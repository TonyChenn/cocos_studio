using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Policies
{
	/// <summary>
	/// A named set of policies.
	/// </summary>
	// Token: 0x020001EE RID: 494
	public sealed class PolicySet : PolicyContainer
	{
		// Token: 0x060012E4 RID: 4836 RVA: 0x0004E73A File Offset: 0x0004C93A
		internal PolicySet(string id, string name)
		{
			this.Id = id;
			this.Name = name;
			this.Visible = true;
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0004E762 File Offset: 0x0004C962
		public PolicySet()
		{
			this.Visible = true;
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x060012E6 RID: 4838 RVA: 0x0004E77C File Offset: 0x0004C97C
		public override bool IsRoot
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// When set to false, this policy set is not visible to the user. This flag can be used
		/// to deprecate existing policy sets (since registered policy sets can't be modified/removed).
		/// </summary>
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x060012E7 RID: 4839 RVA: 0x0004E77F File Offset: 0x0004C97F
		// (set) Token: 0x060012E8 RID: 4840 RVA: 0x0004E787 File Offset: 0x0004C987
		public bool Visible { get; set; }

		/// <summary>
		/// When set to true, this policy can be used as a base for a differential serialization. It's false by default
		/// </summary>
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x060012E9 RID: 4841 RVA: 0x0004E790 File Offset: 0x0004C990
		// (set) Token: 0x060012EA RID: 4842 RVA: 0x0004E798 File Offset: 0x0004C998
		public bool AllowDiffSerialize { get; internal set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x060012EB RID: 4843 RVA: 0x0004E7A1 File Offset: 0x0004C9A1
		public override PolicyContainer ParentPolicies
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0004E7A4 File Offset: 0x0004C9A4
		protected override T GetDefaultPolicy<T>()
		{
			return default(T);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0004E7BC File Offset: 0x0004C9BC
		protected override T GetDefaultPolicy<T>(IEnumerable<string> scopes)
		{
			return default(T);
		}

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x0004E7D2 File Offset: 0x0004C9D2
		// (set) Token: 0x060012EF RID: 4847 RVA: 0x0004E7DA File Offset: 0x0004C9DA
		public string Name { get; set; }

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0004E7E3 File Offset: 0x0004C9E3
		// (set) Token: 0x060012F1 RID: 4849 RVA: 0x0004E7EB File Offset: 0x0004C9EB
		public string Id { get; private set; }

		// Token: 0x060012F2 RID: 4850 RVA: 0x0004E7F4 File Offset: 0x0004C9F4
		internal PolicyKey[] AddSerializedPolicies(StreamReader reader)
		{
			if (this.policies == null)
			{
				this.policies = new PolicyDictionary();
			}
			List<PolicyKey> list = new List<PolicyKey>();
			foreach (ScopedPolicy scopedPolicy in PolicyService.RawDeserializeXml(reader))
			{
				PolicyKey policyKey = new PolicyKey(scopedPolicy.PolicyType, scopedPolicy.Scope);
				if (this.policies.ContainsKey(policyKey))
				{
					throw new InvalidOperationException(string.Concat(new string[]
					{
						"Cannot add second policy of type '",
						policyKey.ToString(),
						"' to policy set '",
						this.Id,
						"'"
					}));
				}
				list.Add(policyKey);
				this.policies[policyKey] = scopedPolicy.Policy;
				if (!scopedPolicy.SupportsDiffSerialize)
				{
					this.externalPolicies.Add(policyKey);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0004E8FC File Offset: 0x0004CAFC
		internal bool SupportsDiffSerialize(ScopedPolicy pol)
		{
			if (!this.AllowDiffSerialize)
			{
				return false;
			}
			PolicyKey item = new PolicyKey(pol.PolicyType, pol.Scope);
			return !this.externalPolicies.Contains(item);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0004E938 File Offset: 0x0004CB38
		public void SaveToFile(FilePath file)
		{
			using (StreamWriter streamWriter = new StreamWriter(file))
			{
				this.SaveToFile(streamWriter);
			}
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0004E974 File Offset: 0x0004CB74
		internal void SaveToFile(StreamWriter writer)
		{
			XmlWriter xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings
			{
				Indent = true
			});
			using (xmlWriter)
			{
				this.SaveToXml(xmlWriter);
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0004E9BC File Offset: 0x0004CBBC
		internal void SaveToXml(XmlWriter xw)
		{
			XmlConfigurationWriter xmlConfigurationWriter = new XmlConfigurationWriter();
			xmlConfigurationWriter.StoreAllInElements = true;
			xmlConfigurationWriter.StoreInElementExceptions = new string[]
			{
				"scope",
				"inheritsSet",
				"inheritsScope"
			};
			xw.WriteStartElement("PolicySet");
			if (!string.IsNullOrEmpty(this.Name))
			{
				xw.WriteAttributeString("name", this.Name);
			}
			if (!string.IsNullOrEmpty(this.Id))
			{
				xw.WriteAttributeString("id", this.Id);
			}
			if (this.policies != null)
			{
				foreach (KeyValuePair<PolicyKey, object> keyValuePair in this.policies)
				{
					xmlConfigurationWriter.Write(xw, PolicyService.DiffSerialize(keyValuePair.Key.PolicyType, keyValuePair.Value, keyValuePair.Key.Scope));
				}
			}
			xw.WriteEndElement();
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0004EAC4 File Offset: 0x0004CCC4
		public void LoadFromFile(FilePath file)
		{
			using (StreamReader streamReader = new StreamReader(file))
			{
				this.LoadFromFile(streamReader);
			}
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0004EB00 File Offset: 0x0004CD00
		internal void LoadFromFile(StreamReader reader)
		{
			XmlReader reader2 = XmlReader.Create(reader);
			this.LoadFromXml(reader2);
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0004EB1C File Offset: 0x0004CD1C
		internal void LoadFromXml(XmlReader reader)
		{
			if (this.policies == null)
			{
				this.policies = new PolicyDictionary();
			}
			else
			{
				this.policies.Clear();
			}
			reader.MoveToContent();
			string attribute = reader.GetAttribute("name");
			if (!string.IsNullOrEmpty(attribute))
			{
				this.Name = attribute;
			}
			attribute = reader.GetAttribute("id");
			if (!string.IsNullOrEmpty(attribute))
			{
				this.Id = attribute;
			}
			reader.MoveToElement();
			foreach (ScopedPolicy scopedPolicy in PolicyService.DiffDeserializeXml(reader))
			{
				PolicyKey policyKey = new PolicyKey(scopedPolicy.PolicyType, scopedPolicy.Scope);
				if (this.policies.ContainsKey(policyKey))
				{
					throw new InvalidOperationException(string.Concat(new object[]
					{
						"Cannot add second policy of type '",
						policyKey,
						"' to policy set '",
						this.Id,
						"'"
					}));
				}
				this.policies[policyKey] = scopedPolicy.Policy;
			}
		}

		// Token: 0x060012FA RID: 4858 RVA: 0x0004EC40 File Offset: 0x0004CE40
		public PolicySet Clone()
		{
			PolicySet policySet = new PolicySet();
			policySet.CopyFrom(this);
			policySet.Name = this.Name;
			return policySet;
		}

		// Token: 0x04000583 RID: 1411
		private HashSet<PolicyKey> externalPolicies = new HashSet<PolicyKey>();
	}
}
