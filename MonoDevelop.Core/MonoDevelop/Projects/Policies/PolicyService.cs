using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;
using MonoDevelop.Projects.Extensions;

namespace MonoDevelop.Projects.Policies
{
	// Token: 0x020001EB RID: 491
	public static class PolicyService
	{
		// Token: 0x0600129E RID: 4766 RVA: 0x0004BE40 File Offset: 0x0004A040
		static PolicyService()
		{
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/PolicyTypes", new ExtensionNodeEventHandler(PolicyService.HandlePolicyTypeUpdated));
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/ProjectModel/PolicySets", new ExtensionNodeEventHandler(PolicyService.HandlePolicySetUpdated));
			PolicyService.LoadPolicies();
			PolicyService.defaultPolicyBag.ReadOnly = true;
			PolicySet policySetById = PolicyService.GetPolicySetById("Invariant");
			policySetById.PolicyChanged += PolicyService.HandleInvariantPolicySetChanged;
			foreach (KeyValuePair<PolicyKey, object> keyValuePair in policySetById.Policies)
			{
				PolicyService.invariantPolicies.InternalSet(keyValuePair.Key.PolicyType, keyValuePair.Key.Scope, keyValuePair.Value);
			}
			PolicyService.invariantPolicies.ReadOnly = true;
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x0004BF68 File Offset: 0x0004A168
		private static void HandleInvariantPolicySetChanged(object sender, PolicyChangedEventArgs e)
		{
			PolicySet policySetById = PolicyService.GetPolicySetById("Invariant");
			object obj = policySetById.Get(e.PolicyType, e.Scope);
			if (obj != null)
			{
				PolicyService.invariantPolicies.InternalSet(e.PolicyType, e.Scope, obj);
				return;
			}
			PolicyService.invariantPolicies.InternalSet(e.PolicyType, e.Scope, Activator.CreateInstance(e.PolicyType));
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x0004BFD0 File Offset: 0x0004A1D0
		private static void HandlePolicySetUpdated(object sender, ExtensionNodeEventArgs args)
		{
			switch (args.Change)
			{
			case ExtensionChange.Add:
			{
				PolicySet set = ((PolicySetNode)args.ExtensionNode).Set;
				set.ReadOnly = true;
				PolicyService.sets.Add(set);
				return;
			}
			case ExtensionChange.Remove:
				PolicyService.sets.Remove(((PolicySetNode)args.ExtensionNode).Set);
				return;
			default:
				return;
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x0004C034 File Offset: 0x0004A234
		private static void HandlePolicyTypeUpdated(object sender, ExtensionNodeEventArgs args)
		{
			Type type = ((TypeExtensionNode)args.ExtensionNode).Type;
			string text = null;
			object[] customAttributes = type.GetCustomAttributes(typeof(DataItemAttribute), true);
			if (customAttributes != null && customAttributes.Length > 0)
			{
				text = ((DataItemAttribute)customAttributes[0]).Name;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = type.Name;
			}
			switch (args.Change)
			{
			case ExtensionChange.Add:
				if (PolicyService.policyTypes.ContainsKey(type))
				{
					throw new UserException("The Policy type '" + type.FullName + "' may only be registered once.");
				}
				if (PolicyService.policyNames.ContainsKey(text))
				{
					throw new UserException("Only one Policy type may have the ID '" + text + "'");
				}
				PolicyService.policyTypes.Add(type, text);
				PolicyService.policyNames.Add(text, type);
				if (PolicyService.invariantPolicies.Get(type) == null)
				{
					PolicyService.invariantPolicies.InternalSet(type, null, Activator.CreateInstance(type));
					return;
				}
				break;
			case ExtensionChange.Remove:
				foreach (PolicySet policySet in PolicyService.sets)
				{
					policySet.RemoveAll(type);
				}
				PolicyService.policyTypes.Remove(type);
				PolicyService.policyNames.Remove(text);
				PolicyService.invariantPolicies.InternalRemove(type, null);
				break;
			default:
				return;
			}
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0004C194 File Offset: 0x0004A394
		public static string GetPolicyTypeDescription(Type t)
		{
			foreach (object obj in AddinManager.GetExtensionNodes("/MonoDevelop/ProjectModel/PolicyTypes"))
			{
				TypeExtensionNode<PolicyTypeAttribute> typeExtensionNode = (TypeExtensionNode<PolicyTypeAttribute>)obj;
				if (typeExtensionNode.Type == t)
				{
					return typeExtensionNode.Data.Description;
				}
			}
			return t.Name;
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060012A3 RID: 4771 RVA: 0x0004C210 File Offset: 0x0004A410
		private static DataSerializer Serializer
		{
			get
			{
				if (PolicyService.serializer == null)
				{
					PolicyService.serializer = new DataSerializer(new DataContext());
					PolicyService.serializer.SerializationContext.IncludeDefaultValues = true;
				}
				return PolicyService.serializer;
			}
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x0004C498 File Offset: 0x0004A698
		internal static IEnumerable<ScopedPolicy> RawDeserializeXml(StreamReader reader)
		{
			XmlTextReader xr = new XmlTextReader(reader);
			XmlConfigurationReader configReader = XmlConfigurationReader.DefaultReader;
			while (!xr.EOF && xr.MoveToContent() != XmlNodeType.None)
			{
				DataNode node = configReader.Read(xr);
				if (node.Name == "PolicySet" && node is DataItem)
				{
					foreach (object obj in ((DataItem)node).ItemData)
					{
						DataNode child = (DataNode)obj;
						yield return PolicyService.RawDeserialize(child);
					}
				}
				else
				{
					yield return PolicyService.RawDeserialize(node);
				}
			}
			yield break;
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x0004C4B8 File Offset: 0x0004A6B8
		internal static IEnumerable<ScopedPolicy> DiffDeserializeXml(StreamReader reader)
		{
			XmlReader xr = XmlReader.Create(reader);
			return PolicyService.DiffDeserializeXml(xr);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x0004C73C File Offset: 0x0004A93C
		internal static IEnumerable<ScopedPolicy> DiffDeserializeXml(XmlReader xr)
		{
			XmlConfigurationReader configReader = XmlConfigurationReader.DefaultReader;
			while (!xr.EOF && xr.MoveToContent() == XmlNodeType.Element)
			{
				DataNode node = configReader.Read(xr);
				if (node.Name == "PolicySet" && node is DataItem)
				{
					foreach (object obj in ((DataItem)node).ItemData)
					{
						DataNode child = (DataNode)obj;
						if (child is DataItem)
						{
							yield return PolicyService.DiffDeserialize((DataItem)child);
						}
					}
					break;
				}
				yield return PolicyService.DiffDeserialize((DataItem)node);
			}
			yield break;
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x0004C75C File Offset: 0x0004A95C
		internal static ScopedPolicy RawDeserialize(DataNode data)
		{
			string scope = null;
			Type registeredType = PolicyService.GetRegisteredType(data.Name);
			if (registeredType == null)
			{
				UnknownPolicy unknownPolicy = new UnknownPolicy(data);
				return new ScopedPolicy(typeof(UnknownPolicy), unknownPolicy, unknownPolicy.Scope);
			}
			bool supportsDiffSerialize = false;
			DataItem dataItem = data as DataItem;
			if (dataItem != null)
			{
				DataValue dataValue = dataItem["allowDiffSerialize"] as DataValue;
				supportsDiffSerialize = (dataValue != null && dataValue.Value == "True");
				DataValue dataValue2 = dataItem["scope"] as DataValue;
				if (dataValue2 != null)
				{
					scope = dataValue2.Value;
				}
				DataValue dataValue3 = dataItem["inheritsSet"] as DataValue;
				if (dataValue3 != null && dataValue3.Value == "null")
				{
					return new ScopedPolicy(registeredType, null, scope, supportsDiffSerialize);
				}
			}
			object ob = PolicyService.Serializer.Deserialize(registeredType, data);
			return new ScopedPolicy(registeredType, ob, scope, supportsDiffSerialize);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x0004C844 File Offset: 0x0004AA44
		private static Type GetRegisteredType(string name)
		{
			Type result;
			if (!PolicyService.policyNames.TryGetValue(name, out result))
			{
				LoggingService.LogWarning("Cannot deserialise unregistered policy name '" + name + "'");
				return null;
			}
			return result;
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0004C878 File Offset: 0x0004AA78
		internal static DataNode RawSerialize(Type policyType, object policy)
		{
			if (policy is UnknownPolicy)
			{
				return ((UnknownPolicy)policy).Data;
			}
			string name;
			if (!PolicyService.policyTypes.TryGetValue(policyType, out name))
			{
				throw new InvalidOperationException("Cannot serialise unregistered policy type '" + policyType + "'");
			}
			DataNode dataNode;
			if (policy != null)
			{
				dataNode = PolicyService.Serializer.Serialize(policy);
			}
			else
			{
				dataNode = new DataItem();
			}
			dataNode.Name = name;
			return dataNode;
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x0004C8E0 File Offset: 0x0004AAE0
		internal static ScopedPolicy DiffDeserialize(DataItem item)
		{
			DataValue dataValue = item.ItemData["inheritsSet"] as DataValue;
			if (dataValue == null || dataValue.Value == "null")
			{
				return PolicyService.RawDeserialize(item);
			}
			Type registeredType = PolicyService.GetRegisteredType(item.Name);
			if (registeredType == null)
			{
				UnknownPolicy unknownPolicy = new UnknownPolicy(item);
				return new ScopedPolicy(typeof(UnknownPolicy), unknownPolicy, unknownPolicy.Scope);
			}
			item.ItemData.Remove(dataValue);
			PolicySet policySetById = PolicyService.GetPolicySetById(dataValue.Value);
			if (policySetById == null)
			{
				throw new InvalidOperationException("No policy set found for id '" + dataValue.Value + "'");
			}
			DataValue dataValue2 = item.ItemData.Extract("inheritsScope") as DataValue;
			object obj = policySetById.Get(registeredType, (dataValue2 != null) ? dataValue2.Value : null);
			if (obj == null)
			{
				string text = string.Concat(new string[]
				{
					"Policy set '",
					policySetById.Id,
					"' does not contain a policy for '",
					item.Name,
					"'"
				});
				if (dataValue2 != null)
				{
					text = text + ", scope '" + dataValue2.Value + "'";
				}
				text += ". This policy is likely provided by an addin that is not currently installed.";
				throw new InvalidOperationException(text);
			}
			DataValue dataValue3 = item.ItemData.Extract("scope") as DataValue;
			DataNode baseline = PolicyService.RawSerialize(registeredType, obj);
			ScopedPolicy scopedPolicy = PolicyService.RawDeserialize(PolicyService.ApplyOverlay(baseline, item));
			return new ScopedPolicy(registeredType, scopedPolicy.Policy, (dataValue3 != null) ? dataValue3.Value : null);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0004CA7C File Offset: 0x0004AC7C
		private static PolicySet GetPolicySetById(string id)
		{
			foreach (PolicySet policySet in PolicyService.sets)
			{
				if (policySet.Id == id)
				{
					return policySet;
				}
			}
			return null;
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x0004CADC File Offset: 0x0004ACDC
		internal static DataNode DiffSerialize(Type policyType, object policy, string scope)
		{
			string value = null;
			int num = int.MaxValue;
			DataNode dataNode = null;
			string text = null;
			if (policy is UnknownPolicy)
			{
				return ((UnknownPolicy)policy).Data;
			}
			DataNode dataNode2 = PolicyService.RawSerialize(policyType, policy);
			if (policy != null)
			{
				using (List<PolicySet>.Enumerator enumerator = PolicyService.sets.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PolicySet policySet = enumerator.Current;
						foreach (ScopedPolicy scopedPolicy in policySet.GetScoped(policyType))
						{
							if (policySet.SupportsDiffSerialize(scopedPolicy))
							{
								DataNode baseline = PolicyService.RawSerialize(policyType, scopedPolicy.Policy);
								int num2 = 0;
								DataNode dataNode3 = PolicyService.ExtractOverlay(baseline, dataNode2, ref num2);
								if (num2 < num)
								{
									value = policySet.Id;
									num = num2;
									dataNode = dataNode3;
									text = scopedPolicy.Scope;
								}
							}
						}
					}
					goto IL_E1;
				}
			}
			value = "null";
			dataNode = dataNode2;
			IL_E1:
			if (dataNode != null)
			{
				((DataItem)dataNode).ItemData.Add(new DataValue("inheritsSet", value));
				if (text != null)
				{
					((DataItem)dataNode).ItemData.Add(new DataValue("inheritsScope", text));
				}
				dataNode2 = dataNode;
			}
			if (scope != null)
			{
				((DataItem)dataNode2).ItemData.Add(new DataValue("scope", scope));
			}
			return dataNode2;
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0004CC48 File Offset: 0x0004AE48
		private static DataNode ApplyOverlay(DataNode baseline, DataNode diffNode)
		{
			if (baseline.Name != diffNode.Name)
			{
				throw new InvalidOperationException("Node names do not match");
			}
			if (diffNode is DataValue)
			{
				return diffNode;
			}
			DataItem dataItem = (DataItem)baseline;
			DataItem dataItem2 = (DataItem)diffNode;
			if (dataItem2.HasItemData)
			{
				PolicyService.ApplyOverlay(dataItem, dataItem2);
			}
			return dataItem;
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0004CC9C File Offset: 0x0004AE9C
		private static void ApplyOverlay(DataItem baseline, DataItem diffNode)
		{
			DataValue dataValue = diffNode.Extract("__removed") as DataValue;
			if (dataValue != null)
			{
				List<DataNode> list = new List<DataNode>();
				foreach (string text in dataValue.Value.Split(new char[]
				{
					' '
				}))
				{
					if (text[0] == '@')
					{
						string name = text.Substring(1);
						DataNode dataNode = baseline.ItemData[name];
						if (dataNode != null)
						{
							list.Add(dataNode);
						}
					}
					else
					{
						int num = int.Parse(text, CultureInfo.InvariantCulture);
						if (num < baseline.ItemData.Count)
						{
							list.Add(baseline.ItemData[num]);
						}
					}
				}
				foreach (DataNode entry in list)
				{
					baseline.ItemData.Remove(entry);
				}
			}
			List<DataNode> list2 = new List<DataNode>();
			HashSet<DataNode> hashSet = new HashSet<DataNode>();
			foreach (object obj in diffNode.ItemData)
			{
				DataNode dataNode2 = (DataNode)obj;
				DataNode dataNode3 = null;
				if (dataNode2 is DataItem)
				{
					DataItem dataItem = (DataItem)dataNode2;
					DataValue dataValue2 = dataItem.ItemData.Extract("__added") as DataValue;
					if (dataValue2 != null)
					{
						DataNode entry2 = dataNode2;
						DataValue dataValue3 = dataItem.ItemData.Extract("__value") as DataValue;
						if (dataValue3 != null)
						{
							entry2 = new DataValue(dataNode2.Name, dataValue3.Value);
						}
						int num2 = int.Parse(dataValue2.Value, CultureInfo.InvariantCulture);
						if (num2 > baseline.ItemData.Count)
						{
							num2 = baseline.ItemData.Count;
						}
						baseline.ItemData.Insert(num2, entry2);
						continue;
					}
					DataValue dataValue4 = dataItem.ItemData.Extract("__index") as DataValue;
					if (dataValue4 != null)
					{
						int n = int.Parse(dataValue4.Value, CultureInfo.InvariantCulture);
						dataNode3 = baseline.ItemData[n];
						DataValue dataValue5 = dataItem.ItemData.Extract("__value") as DataValue;
						if (dataValue5 != null)
						{
							baseline.ItemData[n] = new DataValue(dataNode2.Name, dataValue5.Value);
							continue;
						}
					}
				}
				if (dataNode3 == null)
				{
					dataNode3 = baseline[dataNode2.Name];
				}
				if (dataNode3 != null && !hashSet.Add(dataNode3))
				{
					dataNode3 = null;
				}
				if (dataNode3 == null)
				{
					list2.Add(dataNode2);
				}
				else if (dataNode3 is DataValue)
				{
					int n2 = baseline.ItemData.IndexOf(dataNode3);
					baseline.ItemData[n2] = dataNode2;
				}
				else
				{
					PolicyService.ApplyOverlay((DataItem)dataNode3, (DataItem)dataNode2);
				}
			}
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0004CFB0 File Offset: 0x0004B1B0
		private static DataNode ExtractOverlay(DataNode baseline, DataNode diffNode, ref int size)
		{
			if (baseline.Name != diffNode.Name)
			{
				throw new InvalidOperationException("Node names do not match");
			}
			DataValue dataValue = diffNode as DataValue;
			if (dataValue == null)
			{
				return PolicyService.ExtractOverlay((DataItem)baseline, (DataItem)diffNode, ref size);
			}
			size += dataValue.Name.Length;
			if (dataValue.Value == null)
			{
				throw new InvalidOperationException("Data node '" + dataValue.Name + "' has null value, which cannot safely be diff-serialised.");
			}
			size += dataValue.Value.Length;
			return diffNode;
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x0004D03C File Offset: 0x0004B23C
		private static DataItem ExtractOverlay(DataItem baseline, DataItem diffNode, ref int size)
		{
			DataItem dataItem = new DataItem();
			dataItem.Name = baseline.Name;
			HashSet<DataNode> hashSet = new HashSet<DataNode>();
			for (int i = 0; i < diffNode.ItemData.Count; i++)
			{
				DataNode dataNode = diffNode.ItemData[i];
				int num;
				DataNode dataNode2 = PolicyService.GetBestOverlayNode(baseline, dataNode, out num);
				if (dataNode2 != null && !hashSet.Add(dataNode2))
				{
					dataNode2 = null;
				}
				if (dataNode2 == null)
				{
					if (dataNode is DataItem)
					{
						((DataItem)dataNode).ItemData.Add(new DataValue("__added", i.ToString(CultureInfo.InvariantCulture))
						{
							StoreAsAttribute = true
						});
						dataItem.ItemData.Add(dataNode);
					}
					else
					{
						DataItem dataItem2 = new DataItem();
						dataItem2.Name = dataNode.Name;
						dataItem2.ItemData.Add(new DataValue("__added", i.ToString(CultureInfo.InvariantCulture))
						{
							StoreAsAttribute = true
						});
						dataItem2.ItemData.Add(new DataValue("__value", ((DataValue)dataNode).Value)
						{
							StoreAsAttribute = true
						});
						dataItem.ItemData.Add(dataItem2);
					}
				}
				else
				{
					DataValue dataValue = dataNode2 as DataValue;
					if (dataValue != null)
					{
						if (dataValue.Value != ((DataValue)dataNode).Value)
						{
							size += dataValue.Name.Length;
							if (dataValue.Value == null)
							{
								throw new InvalidOperationException("Data node '" + dataValue.Name + "' has null value, which cannot safely be diff-serialised.");
							}
							size += dataValue.Value.Length;
							if (num == -1)
							{
								dataItem.ItemData.Add(dataNode);
							}
							else
							{
								DataItem dataItem3 = new DataItem();
								dataItem3.Name = dataNode.Name;
								dataItem3.ItemData.Add(new DataValue("__index", num.ToString(CultureInfo.InvariantCulture))
								{
									StoreAsAttribute = true
								});
								dataItem3.ItemData.Add(new DataValue("__value", ((DataValue)dataNode).Value)
								{
									StoreAsAttribute = true
								});
								dataItem.ItemData.Add(dataItem3);
							}
						}
					}
					else
					{
						DataItem dataItem4 = PolicyService.ExtractOverlay((DataItem)dataNode2, (DataItem)dataNode, ref size);
						if (dataItem4 != null && dataItem4.HasItemData)
						{
							size += dataItem4.Name.Length + dataItem4.Name.Length;
							dataItem.ItemData.Add(dataItem4);
							if (num != -1)
							{
								dataItem4.ItemData.Add(new DataValue("__index", num.ToString(CultureInfo.InvariantCulture))
								{
									StoreAsAttribute = true
								});
							}
						}
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < baseline.ItemData.Count; j++)
			{
				DataNode dataNode3 = baseline.ItemData[j];
				if (!hashSet.Contains(dataNode3))
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(' ');
					}
					if (baseline.UniqueNames && dataNode3 is DataValue)
					{
						stringBuilder.Append("@" + dataNode3.Name);
					}
					else
					{
						stringBuilder.Append(j.ToString(CultureInfo.InvariantCulture));
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				dataItem.ItemData.Add(new DataValue("__removed", stringBuilder.ToString())
				{
					StoreAsAttribute = true
				});
			}
			return dataItem;
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x0004D3C8 File Offset: 0x0004B5C8
		private static DataNode GetBestOverlayNode(DataItem baseline, DataNode node, out int index)
		{
			int num = -1;
			int num2 = -1;
			for (int i = 0; i < baseline.ItemData.Count; i++)
			{
				DataNode dataNode = baseline.ItemData[i];
				if (!(dataNode.Name != node.Name))
				{
					if (num == -1)
					{
						num = i;
					}
					else
					{
						if (num2 == -1)
						{
							num2 = PolicyService.CalcDiffSize(baseline.ItemData[num], node);
						}
						int num3 = PolicyService.CalcDiffSize(dataNode, node);
						if (num3 < num2)
						{
							num2 = num3;
							num = i;
						}
					}
				}
			}
			if (num == -1)
			{
				index = -1;
				return null;
			}
			if (num2 != -1)
			{
				index = num;
			}
			else
			{
				index = -1;
			}
			return baseline.ItemData[num];
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x0004D464 File Offset: 0x0004B664
		private static int CalcDiffSize(DataNode node1, DataNode node2)
		{
			if (node1 is DataValue)
			{
				DataValue dataValue = (DataValue)node1;
				DataValue dataValue2 = node2 as DataValue;
				if (dataValue2 == null)
				{
					return int.MaxValue;
				}
				if (dataValue.Value == dataValue2.Value)
				{
					return 0;
				}
				return dataValue2.Value.Length;
			}
			else
			{
				DataItem dataItem = (DataItem)node1;
				DataItem dataItem2 = node2 as DataItem;
				if (dataItem2 == null)
				{
					return int.MaxValue;
				}
				int num = 0;
				foreach (object obj in dataItem2.ItemData)
				{
					DataNode dataNode = (DataNode)obj;
					DataNode dataNode2 = dataItem.ItemData[dataNode.Name];
					if (dataNode2 != null)
					{
						num += PolicyService.CalcDiffSize(dataNode2, dataNode);
					}
					else
					{
						num += PolicyService.CalcSize(dataNode);
					}
				}
				return num;
			}
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x0004D550 File Offset: 0x0004B750
		private static int CalcSize(DataNode node)
		{
			DataValue dataValue = node as DataValue;
			if (dataValue != null)
			{
				return node.Name.Length + ((dataValue.Value != null) ? dataValue.Value.Length : 0);
			}
			int num = 0;
			foreach (object obj in ((DataItem)node).ItemData)
			{
				DataNode node2 = (DataNode)obj;
				num += PolicyService.CalcSize(node2);
			}
			return num;
		}

		/// <summary>
		/// Gets a policy set.
		/// </summary>
		/// <returns>
		/// The policy set.
		/// </returns>
		/// <param name="name">
		/// Name of the policy set
		/// </param>
		// Token: 0x060012B4 RID: 4788 RVA: 0x0004D5E4 File Offset: 0x0004B7E4
		public static PolicySet GetPolicySet(string name)
		{
			foreach (PolicySet policySet in PolicyService.sets)
			{
				if (policySet.Name == name)
				{
					return policySet;
				}
			}
			return null;
		}

		/// <summary>
		/// Get all policy sets which define a specific policy
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		/// <typeparam name="T">
		/// Type of the policy to look for
		/// </typeparam>
		// Token: 0x060012B5 RID: 4789 RVA: 0x0004D644 File Offset: 0x0004B844
		public static IEnumerable<PolicySet> GetPolicySets<T>()
		{
			return PolicyService.GetPolicySets<T>(false);
		}

		/// <summary>
		/// Get all policy sets which define a specific policy
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		// Token: 0x060012B6 RID: 4790 RVA: 0x0004D7F0 File Offset: 0x0004B9F0
		public static IEnumerable<PolicySet> GetPolicySets<T>(bool includeHidden)
		{
			foreach (PolicySet s in PolicyService.sets)
			{
				if (s.DirectHas<T>() && (s.Visible || includeHidden))
				{
					yield return s;
				}
			}
			yield break;
		}

		/// <summary>
		/// Get all policy sets which define a policy under a specific scope
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		/// <param name="scope">
		/// Scope under which the policy has to be defined (it can be for example a mime type)
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		// Token: 0x060012B7 RID: 4791 RVA: 0x0004D80D File Offset: 0x0004BA0D
		public static IEnumerable<PolicySet> GetPolicySets<T>(string scope)
		{
			return PolicyService.GetPolicySets<T>(scope, false);
		}

		/// <summary>
		/// Get all policy sets which define a policy under a specific scope
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		/// <param name="scope">
		/// Scope under which the policy has to be defined (it can be for example a mime type)
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		// Token: 0x060012B8 RID: 4792 RVA: 0x0004D9D0 File Offset: 0x0004BBD0
		public static IEnumerable<PolicySet> GetPolicySets<T>(string scope, bool includeHidden)
		{
			foreach (PolicySet s in PolicyService.sets)
			{
				if (s.DirectHas<T>(scope) && (s.Visible || includeHidden))
				{
					yield return s;
				}
			}
			yield break;
		}

		/// <summary>
		/// Get all policy sets which define a policy under a specific set of scopes
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		/// <param name="scopes">
		/// Scopes under which the policy has to be defined (it can be for example a hirearchy of mime types)
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		// Token: 0x060012B9 RID: 4793 RVA: 0x0004D9F4 File Offset: 0x0004BBF4
		public static IEnumerable<PolicySet> GetPolicySets<T>(IEnumerable<string> scopes)
		{
			return PolicyService.GetPolicySets<T>(scopes, false);
		}

		/// <summary>
		/// Get all policy sets which define a policy under a specific set of scopes
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		/// <param name="scopes">
		/// Scopes under which the policy has to be defined (it can be for example a hirearchy of mime types)
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		// Token: 0x060012BA RID: 4794 RVA: 0x0004DBB8 File Offset: 0x0004BDB8
		public static IEnumerable<PolicySet> GetPolicySets<T>(IEnumerable<string> scopes, bool includeHidden)
		{
			foreach (PolicySet s in PolicyService.sets)
			{
				if (s.DirectHas<T>(scopes) && (s.Visible || includeHidden))
				{
					yield return s;
				}
			}
			yield break;
		}

		/// <summary>
		/// Gets a list of sets which contain a specific policy value
		/// </summary>
		/// <returns>
		/// The matching sets.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		/// <remarks>
		/// This method returns a list of policy sets which define a policy of type T which is identical
		/// to the policy provided as argument.
		/// </remarks>
		// Token: 0x060012BB RID: 4795 RVA: 0x0004DBDC File Offset: 0x0004BDDC
		public static IEnumerable<PolicySet> GetMatchingSets<T>(T policy) where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetMatchingSets<T>(policy, false);
		}

		/// <summary>
		/// Gets a list of sets which contain a specific policy value
		/// </summary>
		/// <returns>
		/// The matching sets.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for. Only sets containing this policy will be returned
		/// </typeparam>
		/// <remarks>
		/// This method returns a list of policy sets which define a policy of type T which is identical
		/// to the policy provided as argument.
		/// </remarks>
		// Token: 0x060012BC RID: 4796 RVA: 0x0004DDC0 File Offset: 0x0004BFC0
		public static IEnumerable<PolicySet> GetMatchingSets<T>(T policy, bool includeHidden) where T : class, IEquatable<T>, new()
		{
			foreach (PolicySet ps in PolicyService.sets)
			{
				IEquatable<T> match = ps.Get<T>();
				if (match != null && (ps.Visible || includeHidden) && match.Equals(policy))
				{
					yield return ps;
				}
			}
			yield break;
		}

		/// <summary>
		/// Gets a policy set which contains a specific policy value
		/// </summary>
		/// <returns>
		/// The matching policy set.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for.
		/// </typeparam>
		/// <remarks>
		/// This method returns a policy set which defines a policy of type T which is identical
		/// to the policy provided as argument. If there are several matching policy sets, it
		/// returns the first it finds
		/// </remarks>
		// Token: 0x060012BD RID: 4797 RVA: 0x0004DDE4 File Offset: 0x0004BFE4
		public static PolicySet GetMatchingSet<T>(T policy) where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetMatchingSet<T>(policy, false);
		}

		/// <summary>
		/// Gets a policy set which contains a specific policy value
		/// </summary>
		/// <returns>
		/// The matching policy set.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for.
		/// </typeparam>
		/// <remarks>
		/// This method returns a policy set which defines a policy of type T which is identical
		/// to the policy provided as argument. If there are several matching policy sets, it
		/// returns the first it finds
		/// </remarks>
		// Token: 0x060012BE RID: 4798 RVA: 0x0004DDED File Offset: 0x0004BFED
		public static PolicySet GetMatchingSet<T>(T policy, bool includeHidden) where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetMatchingSet<T>(policy, PolicyService.sets, includeHidden);
		}

		/// <summary>
		/// Gets a policy set which contains a specific policy value
		/// </summary>
		/// <returns>
		/// The matching policy set.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <param name="candidateSets">
		/// List of policy sets where to look for the specified policy
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for.
		/// </typeparam>
		/// <remarks>
		/// This method returns a policy set which defines a policy of type T which is identical
		/// to the policy provided as argument. If there are several matching policy sets, it
		/// returns the first it finds
		/// </remarks>
		// Token: 0x060012BF RID: 4799 RVA: 0x0004DDFC File Offset: 0x0004BFFC
		public static PolicySet GetMatchingSet<T>(T policy, IEnumerable<PolicySet> candidateSets, bool includeHidden) where T : class, IEquatable<T>, new()
		{
			foreach (PolicySet policySet in candidateSets)
			{
				T t = policySet.Get<T>();
				if (t != null && (policySet.Visible || includeHidden) && t.Equals(policy))
				{
					return policySet;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets a policy sets which contains a specific policy value
		/// </summary>
		/// <returns>
		/// The policy set.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <param name="scopes">
		/// Scopes under which the policy has to be defined (it can be for example a hirearchy of mime types)
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for.
		/// </typeparam>
		/// <remarks>
		/// This method returns a policy set which defines a policy of type T which is identical
		/// to the policy provided as argument. This policy has to be defined under one of the
		/// provided scopes. If there are several matching policy sets, it returns the first it finds.
		/// </remarks>
		// Token: 0x060012C0 RID: 4800 RVA: 0x0004DE70 File Offset: 0x0004C070
		public static PolicySet GetMatchingSet<T>(T policy, IEnumerable<string> scopes) where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetMatchingSet<T>(policy, scopes, false);
		}

		/// <summary>
		/// Gets a policy sets which contains a specific policy value
		/// </summary>
		/// <returns>
		/// The policy set.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <param name="scopes">
		/// Scopes under which the policy has to be defined (it can be for example a hirearchy of mime types)
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for.
		/// </typeparam>
		/// <remarks>
		/// This method returns a policy set which defines a policy of type T which is identical
		/// to the policy provided as argument. This policy has to be defined under one of the
		/// provided scopes. If there are several matching policy sets, it returns the first it finds.
		/// </remarks>		
		// Token: 0x060012C1 RID: 4801 RVA: 0x0004DE7A File Offset: 0x0004C07A
		public static PolicySet GetMatchingSet<T>(T policy, IEnumerable<string> scopes, bool includeHidden) where T : class, IEquatable<T>, new()
		{
			return PolicyService.GetMatchingSet<T>(policy, PolicyService.sets, scopes, includeHidden);
		}

		/// <summary>
		/// Gets a policy set which contains a specific policy value
		/// </summary>
		/// <returns>
		/// The policy set.
		/// </returns>
		/// <param name="policy">
		/// Policy to be compared
		/// </param>
		/// <param name="candidateSets">
		/// List of policy sets where to look for the specified policy
		/// </param>
		/// <param name="scopes">
		/// Scopes under which the policy has to be defined (it can be for example a hirearchy of mime types)
		/// </param>
		/// <param name="includeHidden">
		/// True if hidden (system) policy sets have to be returned, False otherwise.
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to look for.
		/// </typeparam>
		/// <remarks>
		/// This method returns a policy set which defines a policy of type T which is identical
		/// to the policy provided as argument. This policy has to be defined under one of the
		/// provided scopes. If there are several matching policy sets, it returns the first it finds.
		/// </remarks>		
		// Token: 0x060012C2 RID: 4802 RVA: 0x0004DE8C File Offset: 0x0004C08C
		public static PolicySet GetMatchingSet<T>(T policy, IEnumerable<PolicySet> candidateSets, IEnumerable<string> scopes, bool includeHidden) where T : class, IEquatable<T>, new()
		{
			foreach (PolicySet policySet in candidateSets)
			{
				T t = policySet.Get<T>(scopes);
				if (t != null && (policySet.Visible || includeHidden) && t.Equals(policy))
				{
					return policySet;
				}
			}
			return null;
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x060012C3 RID: 4803 RVA: 0x0004DF00 File Offset: 0x0004C100
		private static FilePath PoliciesFolder
		{
			get
			{
				return UserProfile.Current.UserDataRoot.Combine(new string[]
				{
					"Policies"
				});
			}
		}

		/// <summary>
		/// Gets a default policy.
		/// </summary>
		/// <returns>
		/// The default policy.
		/// </returns>
		/// <typeparam name="T">
		/// Type of the policy to be returned
		/// </typeparam>
		/// <remarks>
		/// This method returns the default value for the specified policy type. It can be a value defined by
		/// the user using the default policy options panel, or a system default if the user didn't change it.
		/// </remarks>
		// Token: 0x060012C4 RID: 4804 RVA: 0x0004DF2F File Offset: 0x0004C12F
		public static T GetDefaultPolicy<T>() where T : class, IEquatable<T>, new()
		{
			T result;
			if ((result = PolicyService.defaultPolicies.Get<T>()) == null)
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		/// <summary>
		/// Gets a default policy for a specific scope
		/// </summary>
		/// <returns>
		/// The default policy.
		/// </returns>
		/// <param name="scope">
		/// Scope under which the policy has to be defined
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to be returned
		/// </typeparam>
		/// <remarks>
		/// This method returns the default value for the specified policy type and scope. It can be a value defined by
		/// the user using the default policy options panel, or a system default if the user didn't change it.
		/// </remarks>
		// Token: 0x060012C5 RID: 4805 RVA: 0x0004DF49 File Offset: 0x0004C149
		public static T GetDefaultPolicy<T>(string scope) where T : class, IEquatable<T>, new()
		{
			T result;
			if ((result = PolicyService.defaultPolicies.Get<T>(scope)) == null)
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		/// <summary>
		/// Gets a default policy for a specific scope
		/// </summary>
		/// <returns>
		/// The default policy, or NULL if the policy is not defined and createDefault is False
		/// </returns>
		/// <param name="scope">
		/// Scope under which the policy has to be defined
		/// </param>
		/// <param name="createDefault">
		/// When set to False and there is no default policy defined of this type, the method returns null.
		/// When set to True, a policy value is always returned (it can be the system default).
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to be returned
		/// </typeparam>
		/// <remarks>
		/// This method returns the default value for the specified policy type and scope.
		/// </remarks>
		// Token: 0x060012C6 RID: 4806 RVA: 0x0004DF64 File Offset: 0x0004C164
		public static T GetDefaultPolicy<T>(string scope, bool createDefault) where T : class, IEquatable<T>, new()
		{
			T result;
			if ((result = PolicyService.defaultPolicies.Get<T>(scope)) == null)
			{
				if (!createDefault)
				{
					return default(T);
				}
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		/// <summary>
		/// Gets a default policy for a specific set of scopes
		/// </summary>
		/// <returns>
		/// The default policy.
		/// </returns>
		/// <param name="scopes">
		/// Scopes under which the policy has to be defined (it can be for example a hirearchy of mime types)
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to be returned
		/// </typeparam>
		/// <remarks>
		/// This method returns the default value of a policy type for a set of scopes. The policy is looked up under
		/// the provided scopes in sequence, and the first value found is the one returned. If no value is found,
		/// a system default is returned.
		/// </remarks>
		// Token: 0x060012C7 RID: 4807 RVA: 0x0004DF97 File Offset: 0x0004C197
		public static T GetDefaultPolicy<T>(IEnumerable<string> scopes) where T : class, IEquatable<T>, new()
		{
			T result;
			if ((result = PolicyService.defaultPolicies.Get<T>(scopes)) == null)
			{
				result = Activator.CreateInstance<T>();
			}
			return result;
		}

		/// <summary>
		/// Sets a default policy value.
		/// </summary>
		/// <param name="value">
		/// Policy to be set
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to be set
		/// </typeparam>
		// Token: 0x060012C8 RID: 4808 RVA: 0x0004DFB2 File Offset: 0x0004C1B2
		public static void SetDefaultPolicy<T>(T value) where T : class, IEquatable<T>, new()
		{
			PolicyService.defaultPolicies.Set<T>(value);
		}

		/// <summary>
		/// Gets default user-defined policy set
		/// </summary>
		// Token: 0x060012C9 RID: 4809 RVA: 0x0004DFBF File Offset: 0x0004C1BF
		public static PolicySet GetUserDefaultPolicySet()
		{
			return PolicyService.defaultPolicies;
		}

		/// <summary>
		/// Gets the invariant policy set
		/// </summary>
		/// <remarks>
		/// The invariant policy set is a policy set whose values will not change in future MonoDevelop versions.
		/// </remarks>
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x0004DFC6 File Offset: 0x0004C1C6
		public static PolicyContainer InvariantPolicies
		{
			get
			{
				return PolicyService.invariantPolicies;
			}
		}

		/// <summary>
		/// Gets the system default policies
		/// </summary>
		/// <value>
		/// The default policies.
		/// </value>
		/// <remarks>
		/// The returned PolicyContainer can be used to query the system default value of policies
		/// </remarks>
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0004DFCD File Offset: 0x0004C1CD
		public static PolicyContainer DefaultPolicies
		{
			get
			{
				return PolicyService.defaultPolicyBag;
			}
		}

		/// <summary>
		/// Determines whether a policy instance is an undefined policy
		/// </summary>
		/// <returns>
		/// <c>true</c> if the policy is undefined; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="policy">
		/// Policy to check
		/// </param>
		/// <typeparam name="T">
		/// Type of the policy to check
		/// </typeparam>
		// Token: 0x060012CC RID: 4812 RVA: 0x0004DFD4 File Offset: 0x0004C1D4
		public static bool IsUndefinedPolicy<T>(T policy)
		{
			return policy == null;
		}

		/// <summary>
		/// Gets a undefined policy value
		/// </summary>
		/// <returns>
		/// The undefined policy.
		/// </returns>
		/// <typeparam name="T">
		/// Type of the policy
		/// </typeparam>
		// Token: 0x060012CD RID: 4813 RVA: 0x0004DFE0 File Offset: 0x0004C1E0
		public static T GetUndefinedPolicy<T>() where T : class, IEquatable<T>, new()
		{
			return default(T);
		}

		/// <summary>
		/// Gets the policy sets defined by the user
		/// </summary>
		/// <returns>
		/// The user policy sets.
		/// </returns>
		// Token: 0x060012CE RID: 4814 RVA: 0x0004DFF6 File Offset: 0x0004C1F6
		public static IEnumerable<PolicySet> GetUserPolicySets()
		{
			return PolicyService.userSets;
		}

		/// <summary>
		/// Adds a new user defined policy set
		/// </summary>
		/// <param name="pset">
		/// The policy set
		/// </param>
		// Token: 0x060012CF RID: 4815 RVA: 0x0004E020 File Offset: 0x0004C220
		public static void AddUserPolicySet(PolicySet pset)
		{
			if (pset.Id != null)
			{
				throw new ArgumentException("User policy cannot have ID");
			}
			if (string.IsNullOrEmpty(pset.Name))
			{
				throw new ArgumentException("User policy cannot have null or empty name");
			}
			if (PolicyService.sets.Any((PolicySet ps) => ps.Name == pset.Name))
			{
				throw new ArgumentException("There is already a policy with the name ' " + pset.Name + "'");
			}
			PolicyService.userSets.Add(pset);
			PolicyService.sets.Add(pset);
		}

		/// <summary>
		/// Removes a user defined policy set
		/// </summary>
		/// <param name="pset">
		/// The policy set
		/// </param>
		// Token: 0x060012D0 RID: 4816 RVA: 0x0004E0C7 File Offset: 0x0004C2C7
		public static void RemoveUserPolicySet(PolicySet pset)
		{
			if (PolicyService.userSets.Remove(pset))
			{
				PolicyService.deletedUserSets.Add(PolicyService.GetPolicyFile(pset));
				PolicyService.sets.Remove(pset);
				return;
			}
			throw new InvalidOperationException("The provided property set is not a user defined property set");
		}

		/// <summary>
		/// Get all defined policy sets
		/// </summary>
		/// <returns>
		/// The policy sets.
		/// </returns>
		// Token: 0x060012D1 RID: 4817 RVA: 0x0004E0FD File Offset: 0x0004C2FD
		public static IEnumerable<PolicySet> GetPolicySets()
		{
			return PolicyService.sets;
		}

		/// <summary>
		/// Saves the policies.
		/// </summary>
		// Token: 0x060012D2 RID: 4818 RVA: 0x0004E104 File Offset: 0x0004C304
		public static void SavePolicies()
		{
			foreach (string path in PolicyService.deletedUserSets)
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			PolicyService.deletedUserSets.Clear();
			PolicyService.SavePolicy(PolicyService.defaultPolicies);
			foreach (PolicySet set in PolicyService.userSets)
			{
				PolicyService.SavePolicy(set);
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0004E21C File Offset: 0x0004C41C
		private static void SavePolicy(PolicySet set)
		{
			string policyFile = PolicyService.GetPolicyFile(set);
			string friendlyName = string.Format("policy '{0}'", set.Name);
			PolicyService.ParanoidSave(policyFile, friendlyName, delegate(StreamWriter writer)
			{
				XmlWriterSettings settings = new XmlWriterSettings
				{
					Indent = true
				};
				using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
				{
					xmlWriter.WriteStartElement("Policies");
					set.SaveToXml(xmlWriter);
					xmlWriter.WriteEndElement();
				}
			});
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0004E26C File Offset: 0x0004C46C
		private static string GetPolicyFile(PolicySet set)
		{
			return PolicyService.PoliciesFolder.Combine(new string[]
			{
				(set.Name ?? set.Id) + ".mdpolicy.xml"
			});
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0004E2B0 File Offset: 0x0004C4B0
		private static void LoadPolicies()
		{
			if (PolicyService.defaultPolicies != null)
			{
				PolicyService.defaultPolicies.PolicyChanged -= PolicyService.DefaultPoliciesPolicyChanged;
			}
			PolicyService.userSets.Clear();
			PolicyService.defaultPolicies = null;
			if (Directory.Exists(PolicyService.PoliciesFolder))
			{
				foreach (string text in Directory.GetFiles(PolicyService.PoliciesFolder, "*.mdpolicy.xml"))
				{
					try
					{
						PolicyService.LoadPolicy(text);
					}
					catch (Exception ex)
					{
						LoggingService.LogError(string.Format("Failed to load policy file '{0}'", Path.GetFileName(text)), ex);
					}
				}
			}
			if (PolicyService.defaultPolicies == null)
			{
				PolicyService.defaultPolicies = new PolicySet("Default", "Default");
			}
			PolicyService.defaultPolicies.PolicyChanged += PolicyService.DefaultPoliciesPolicyChanged;
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0004E43C File Offset: 0x0004C63C
		private static void LoadPolicy(FilePath file)
		{
			string friendlyName = string.Format("policy file '{0}'", file.FileName);
			PolicyService.ParanoidLoad(file, friendlyName, delegate(StreamReader reader)
			{
				XmlReader xmlReader = XmlReader.Create(reader);
				xmlReader.MoveToContent();
				if (xmlReader.LocalName == "PolicySet")
				{
					PolicyService.defaultPolicies = new PolicySet("Default", null);
					PolicyService.defaultPolicies.LoadFromXml(xmlReader);
					return;
				}
				if (xmlReader.LocalName == "Policies" && !xmlReader.IsEmptyElement)
				{
					xmlReader.ReadStartElement();
					xmlReader.MoveToContent();
					while (xmlReader.NodeType != XmlNodeType.EndElement)
					{
						PolicySet policySet = new PolicySet();
						policySet.LoadFromXml(xmlReader);
						if (policySet.Id == "Default")
						{
							PolicyService.defaultPolicies = policySet;
						}
						else
						{
							PolicyService.AddUserPolicySet(policySet);
						}
						xmlReader.MoveToContent();
					}
				}
			});
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0004E485 File Offset: 0x0004C685
		private static void DefaultPoliciesPolicyChanged(object sender, PolicyChangedEventArgs e)
		{
			PolicyService.defaultPolicyBag.PropagatePolicyChangeEvent(e);
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0004E494 File Offset: 0x0004C694
		private static bool ParanoidLoad(string fileName, string friendlyName, Action<StreamReader> read)
		{
			StreamReader streamReader = null;
			try
			{
				if (File.Exists(fileName))
				{
					streamReader = new StreamReader(fileName, Encoding.UTF8);
					read(streamReader);
					return true;
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Error loading {0} file '{1}':\n{2}", new object[]
				{
					friendlyName,
					fileName,
					ex
				});
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
					streamReader = null;
				}
			}
			string text = fileName + ".previous";
			try
			{
				if (File.Exists(text))
				{
					streamReader = new StreamReader(text, Encoding.UTF8);
					read(streamReader);
					return true;
				}
			}
			catch (Exception ex2)
			{
				LoggingService.LogError("Error loading {0} backup file '{1}':\n{2}", new object[]
				{
					friendlyName,
					text,
					ex2
				});
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
			}
			return false;
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0004E594 File Offset: 0x0004C794
		private static bool ParanoidSave(string fileName, string friendlyName, Action<StreamWriter> write)
		{
			string destFileName = fileName + ".previous";
			string directoryName = Path.GetDirectoryName(fileName);
			string text = Path.Combine(directoryName, ".#" + Path.GetFileName(fileName));
			try
			{
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
			}
			catch (IOException ex)
			{
				LoggingService.LogError("Error creating directory '{0}' for {1} file\n{2}", new object[]
				{
					directoryName,
					friendlyName,
					ex
				});
				return false;
			}
			try
			{
				if (File.Exists(fileName))
				{
					File.Copy(fileName, destFileName, true);
				}
			}
			catch (IOException ex2)
			{
				LoggingService.LogError("Error copying {0} file '{1}' to backup\n{2}", new object[]
				{
					friendlyName,
					fileName,
					ex2
				});
			}
			StreamWriter streamWriter = null;
			try
			{
				streamWriter = new StreamWriter(text, false, Encoding.UTF8);
				write(streamWriter);
				streamWriter.Close();
				streamWriter = null;
				FileService.SystemRename(text, fileName);
				return true;
			}
			catch (Exception ex3)
			{
				LoggingService.LogError("Error writing {0} file '{1}'\n{2}", new object[]
				{
					friendlyName,
					text,
					ex3
				});
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
				}
			}
			return false;
		}

		// Token: 0x04000574 RID: 1396
		private const string TYPE_EXT_POINT = "/MonoDevelop/ProjectModel/PolicyTypes";

		// Token: 0x04000575 RID: 1397
		private const string SET_EXT_POINT = "/MonoDevelop/ProjectModel/PolicySets";

		// Token: 0x04000576 RID: 1398
		private static List<PolicySet> sets = new List<PolicySet>();

		// Token: 0x04000577 RID: 1399
		private static List<PolicySet> userSets = new List<PolicySet>();

		// Token: 0x04000578 RID: 1400
		private static DataSerializer serializer;

		// Token: 0x04000579 RID: 1401
		private static Dictionary<string, Type> policyNames = new Dictionary<string, Type>();

		// Token: 0x0400057A RID: 1402
		private static Dictionary<Type, string> policyTypes = new Dictionary<Type, string>();

		// Token: 0x0400057B RID: 1403
		private static List<string> deletedUserSets = new List<string>();

		// Token: 0x0400057C RID: 1404
		private static PolicySet defaultPolicies;

		// Token: 0x0400057D RID: 1405
		private static PolicyBag defaultPolicyBag = new PolicyBag();

		// Token: 0x0400057E RID: 1406
		private static InvariantPolicyBag invariantPolicies = new InvariantPolicyBag();
	}
}
