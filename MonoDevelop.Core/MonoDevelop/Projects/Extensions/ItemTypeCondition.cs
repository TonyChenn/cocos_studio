using System;
using System.Collections.Generic;
using Mono.Addins;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x020001A1 RID: 417
	public class ItemTypeCondition : ConditionType
	{
		// Token: 0x06000FE3 RID: 4067 RVA: 0x0003ADDD File Offset: 0x00038FDD
		public ItemTypeCondition()
		{
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0003ADE5 File Offset: 0x00038FE5
		public ItemTypeCondition(Type objType) : this(objType, null)
		{
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x0003ADEF File Offset: 0x00038FEF
		public ItemTypeCondition(Type objType, IDictionary<string, string> typeNameAliases)
		{
			this.objType = objType;
			if (typeNameAliases != null)
			{
				this.aliases = typeNameAliases;
				return;
			}
			this.aliases = new Dictionary<string, string>();
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0003AE14 File Offset: 0x00039014
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0003AE1C File Offset: 0x0003901C
		public Type ObjType
		{
			get
			{
				return this.objType;
			}
			set
			{
				if (value != this.objType)
				{
					this.objType = value;
					this.typeNames = null;
					base.NotifyChanged();
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0003AE40 File Offset: 0x00039040
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0003AE48 File Offset: 0x00039048
		public IDictionary<string, string> Aliases
		{
			get
			{
				return this.aliases;
			}
			set
			{
				this.aliases = value;
				base.NotifyChanged();
			}
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x0003AE58 File Offset: 0x00039058
		public override bool Evaluate(NodeElement conditionNode)
		{
			foreach (string type in conditionNode.GetAttribute("value").Split(new char[]
			{
				'|'
			}))
			{
				if (this.MatchesType(type))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x0003AEAA File Offset: 0x000390AA
		public void AddTypeAlias(string alias, string fullName)
		{
			this.aliases[alias] = fullName;
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x0003AEBC File Offset: 0x000390BC
		private bool MatchesType(string type)
		{
			if (type.IndexOf('.') == -1)
			{
				string text;
				if (this.aliases.TryGetValue(type, out text))
				{
					type = text;
				}
				else
				{
					type = "MonoDevelop.Projects." + type;
				}
			}
			if (this.typeNames == null)
			{
				this.typeNames = new List<string>();
				this.typeNames.Add(this.objType.FullName);
				this.typeNames.Add(this.objType.AssemblyQualifiedName);
				Type baseType = this.objType.BaseType;
				while (baseType != null)
				{
					this.typeNames.Add(baseType.FullName);
					this.typeNames.Add(baseType.AssemblyQualifiedName);
					baseType = baseType.BaseType;
				}
				Type[] interfaces = this.objType.GetInterfaces();
				foreach (Type type2 in interfaces)
				{
					this.typeNames.Add(type2.FullName);
					this.typeNames.Add(type2.AssemblyQualifiedName);
				}
			}
			return this.typeNames.Contains(type);
		}

		// Token: 0x040004A0 RID: 1184
		private Type objType;

		// Token: 0x040004A1 RID: 1185
		private List<string> typeNames;

		// Token: 0x040004A2 RID: 1186
		private IDictionary<string, string> aliases;
	}
}
