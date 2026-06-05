using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace MonoDevelop.Core.Execution
{
	// Token: 0x02000208 RID: 520
	public class ExecutionEnvironment
	{
		// Token: 0x060013B2 RID: 5042 RVA: 0x00051871 File Offset: 0x0004FA71
		public ExecutionEnvironment()
		{
			this.variables = new Dictionary<string, string>();
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00051884 File Offset: 0x0004FA84
		public ExecutionEnvironment(Dictionary<string, string> vars)
		{
			this.variables = vars;
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060013B4 RID: 5044 RVA: 0x00051893 File Offset: 0x0004FA93
		public Dictionary<string, string> Variables
		{
			get
			{
				return this.variables;
			}
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0005189B File Offset: 0x0004FA9B
		public void MergeTo(ProcessStartInfo pinfo)
		{
			this.MergeTo(pinfo.EnvironmentVariables);
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x000518AC File Offset: 0x0004FAAC
		public void MergeTo(StringDictionary vars)
		{
			foreach (KeyValuePair<string, string> keyValuePair in this.variables)
			{
				if (keyValuePair.Value == null)
				{
					vars.Remove(keyValuePair.Key);
				}
				else
				{
					vars[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x040005D3 RID: 1491
		private Dictionary<string, string> variables;
	}
}
