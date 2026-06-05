using System;

namespace MonoDevelop.Core
{
	// Token: 0x02000218 RID: 536
	public interface IUserDataMigrationHandler
	{
		// Token: 0x0600142D RID: 5165
		void Migrate(FilePath source, FilePath target);
	}
}
