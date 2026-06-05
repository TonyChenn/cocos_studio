using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C1 RID: 449
	internal class MSBuildBoolDataValue : DataValue
	{
		// Token: 0x0600113F RID: 4415 RVA: 0x0004625D File Offset: 0x0004445D
		public MSBuildBoolDataValue(string name, bool value) : base(name, value ? "True" : "False")
		{
			this.RawValue = value;
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001140 RID: 4416 RVA: 0x0004627C File Offset: 0x0004447C
		// (set) Token: 0x06001141 RID: 4417 RVA: 0x00046284 File Offset: 0x00044484
		public bool RawValue { get; private set; }
	}
}
