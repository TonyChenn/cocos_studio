using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Addins;
using MonoDevelop.AnalysisCore.Extensions;
using MonoDevelop.Ide.Codons;

namespace MonoDevelop.AnalysisCore
{
	internal static class AnalysisExtensions
	{
		private const string EXT_RULES = "/MonoDevelop/AnalysisCore/Rules";

		private const string EXT_TYPES = "/MonoDevelop/AnalysisCore/Types";

		private const string EXT_FIX_HANDLERS = "/MonoDevelop/AnalysisCore/FixHandlers";

		private static KeyedNodeList<string, AnalysisRuleAddinNode> rulesByInput;

		private static Dictionary<RuleTreeType, RuleTreeRoot> analysisTreeCache;

		private static Dictionary<string, AnalysisTypeExtensionNode> ruleInputTypes;

		private static KeyedNodeList<string, FixHandlerExtensionNode> fixHandlers;

		static AnalysisExtensions()
		{
			rulesByInput = new KeyedNodeList<string, AnalysisRuleAddinNode>();
			analysisTreeCache = new Dictionary<RuleTreeType, RuleTreeRoot>();
			ruleInputTypes = new Dictionary<string, AnalysisTypeExtensionNode>();
			fixHandlers = new KeyedNodeList<string, FixHandlerExtensionNode>();
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/AnalysisCore/Rules", OnRuleNodeChanged);
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/AnalysisCore/Types", OnTypeNodeChanged);
			AddinManager.AddExtensionNodeHandler("/MonoDevelop/AnalysisCore/FixHandlers", OnFixHandlerNodeChanged);
		}

		private static void OnRuleNodeChanged(object sender, ExtensionNodeEventArgs args)
		{
			switch (args.Change)
			{
			case ExtensionChange.Add:
				AddRule(args.ExtensionNode);
				break;
			case ExtensionChange.Remove:
				RemoveRule(args.ExtensionNode);
				break;
			}
			analysisTreeCache.Clear();
		}

		private static void OnTypeNodeChanged(object sender, ExtensionNodeEventArgs args)
		{
			AnalysisTypeExtensionNode analysisTypeExtensionNode = (AnalysisTypeExtensionNode)args.ExtensionNode;
			switch (args.Change)
			{
			case ExtensionChange.Add:
				if (ruleInputTypes.ContainsKey(analysisTypeExtensionNode.Name))
				{
					throw new InvalidOperationException("Duplicate analysis node type '" + analysisTypeExtensionNode.Name + "' registered");
				}
				ruleInputTypes[analysisTypeExtensionNode.Name] = analysisTypeExtensionNode;
				break;
			case ExtensionChange.Remove:
				ruleInputTypes.Remove(analysisTypeExtensionNode.Name);
				break;
			}
			analysisTreeCache.Clear();
		}

		private static void OnFixHandlerNodeChanged(object sender, ExtensionNodeEventArgs args)
		{
			FixHandlerExtensionNode fixHandlerExtensionNode = (FixHandlerExtensionNode)args.ExtensionNode;
			switch (args.Change)
			{
			case ExtensionChange.Add:
				fixHandlers.Add(fixHandlerExtensionNode.FixName, fixHandlerExtensionNode);
				break;
			case ExtensionChange.Remove:
				fixHandlers.Remove(fixHandlerExtensionNode.FixName, fixHandlerExtensionNode);
				break;
			}
		}

		private static void AddRule(ExtensionNode extNode)
		{
			if (extNode is CategoryNode)
			{
				foreach (ExtensionNode childNode in extNode.ChildNodes)
				{
					AddRule(childNode);
				}
				return;
			}
			AnalysisRuleAddinNode analysisRuleAddinNode = (AnalysisRuleAddinNode)extNode;
			rulesByInput.Add(analysisRuleAddinNode.Input, analysisRuleAddinNode);
		}

		private static void RemoveRule(ExtensionNode extNode)
		{
			if (extNode is CategoryNode)
			{
				foreach (ExtensionNode childNode in extNode.ChildNodes)
				{
					RemoveRule(childNode);
				}
				return;
			}
			AnalysisRuleAddinNode analysisRuleAddinNode = (AnalysisRuleAddinNode)extNode;
			rulesByInput.Remove(analysisRuleAddinNode.Input, analysisRuleAddinNode);
		}

		internal static Type GetType(string name)
		{
			if (name == "Results")
			{
				return typeof(IEnumerable<Result>);
			}
			return ruleInputTypes[name].Type;
		}

		public static RuleTreeRoot GetAnalysisTree(RuleTreeType treeType)
		{
			if (analysisTreeCache.TryGetValue(treeType, out var value))
			{
				return value;
			}
			return analysisTreeCache[treeType] = BuildTree(treeType);
		}

		private static RuleTreeRoot BuildTree(RuleTreeType treeType)
		{
			IRuleTreeNode[] treeNodes = GetTreeNodes(treeType, treeType.Input, 0);
			if (treeNodes == null || treeNodes.Length == 0)
			{
				return null;
			}
			return new RuleTreeRoot(treeNodes, treeType);
		}

		private static IRuleTreeNode[] GetTreeNodes(RuleTreeType treeType, string input, int depth)
		{
			List<AnalysisRuleAddinNode> list = rulesByInput.Get(input);
			if (list == null)
			{
				return null;
			}
			List<AnalysisRuleAddinNode> list2 = list.Where((AnalysisRuleAddinNode n) => n.Supports(treeType.FileExtension)).ToList();
			if (list2.Count == 0)
			{
				return null;
			}
			List<IRuleTreeNode> list3 = new List<IRuleTreeNode>();
			foreach (AnalysisRuleAddinNode item in list2)
			{
				if (item.Output == "Results")
				{
					list3.Add(new RuleTreeLeaf(item));
					continue;
				}
				if (depth > 50)
				{
					throw new InvalidOperationException("Analysis tree too deep. Check for circular dependencies " + item.GetErrSource());
				}
				IRuleTreeNode[] treeNodes = GetTreeNodes(treeType, item.Output, depth + 1);
				if (treeNodes != null && treeNodes.Length != 0)
				{
					list3.Add(new RuleTreeBranch(treeNodes, item));
				}
			}
			return list3.ToArray();
		}

		public static IEnumerable<IFixHandler> GetFixHandlers(string fixType)
		{
			List<FixHandlerExtensionNode> list = fixHandlers.Get(fixType);
			if (list != null)
			{
				return list.Select((FixHandlerExtensionNode node) => node.FixHandler);
			}
			return new IFixHandler[0];
		}
	}
}
