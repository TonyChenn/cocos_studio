using System;
using System.Collections.Generic;
using MonoDevelop.Core.StringParsing;

namespace MonoDevelop.Projects
{
	// Token: 0x0200026E RID: 622
	public class ProjectCreateParameters : IStringTagModel
	{
		// Token: 0x0600166A RID: 5738 RVA: 0x0005A59C File Offset: 0x0005879C
		public ProjectCreateParameters()
		{
			this.Clear();
		}

		// Token: 0x0600166B RID: 5739 RVA: 0x0005A5AA File Offset: 0x000587AA
		public void Clear()
		{
			this.parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x0600166C RID: 5740 RVA: 0x0005A5BC File Offset: 0x000587BC
		public void MergeTo(IDictionary<string, string> other)
		{
			foreach (KeyValuePair<string, string> keyValuePair in this.parameters)
			{
				other[keyValuePair.Key] = keyValuePair.Value;
			}
		}

		// Token: 0x170004C8 RID: 1224
		public string this[string name]
		{
			get
			{
				string result;
				if (this.parameters.TryGetValue(name, out result))
				{
					return result;
				}
				return string.Empty;
			}
			set
			{
				this.parameters[name] = value;
			}
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x0005A650 File Offset: 0x00058850
		public bool GetBoolean(string name, bool defaultValue = false)
		{
			string value = this[name];
			bool result;
			if (!string.IsNullOrEmpty(value) && bool.TryParse(value, out result))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x06001670 RID: 5744 RVA: 0x0005A67C File Offset: 0x0005887C
		object IStringTagModel.GetValue(string name)
		{
			string result;
			if (this.parameters.TryGetValue(name, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x040006C3 RID: 1731
		private Dictionary<string, string> parameters;
	}
}
