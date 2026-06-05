using System;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects.Formats.MSBuild
{
	// Token: 0x020001C3 RID: 451
	internal class MSBuildNullableBoolDataValue : DataValue
	{
		// Token: 0x06001145 RID: 4421 RVA: 0x000462FF File Offset: 0x000444FF
		public MSBuildNullableBoolDataValue(string name, bool? value) : base(name, (value != null) ? (value.Value ? "True" : "False") : null)
		{
			this.RawValue = value;
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001146 RID: 4422 RVA: 0x00046330 File Offset: 0x00044530
		// (set) Token: 0x06001147 RID: 4423 RVA: 0x00046338 File Offset: 0x00044538
		public bool? RawValue { get; private set; }
	}
}
