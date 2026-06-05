using System;
using System.Collections.Generic;
using MonoDevelop.Core;

namespace MonoDevelop.Projects
{
	// Token: 0x02000151 RID: 337
	public class CustomCommandCollection : List<CustomCommand>
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x0002DF14 File Offset: 0x0002C114
		public CustomCommandCollection Clone()
		{
			CustomCommandCollection customCommandCollection = new CustomCommandCollection();
			customCommandCollection.CopyFrom(this);
			return customCommandCollection;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002DF30 File Offset: 0x0002C130
		public void CopyFrom(CustomCommandCollection col)
		{
			base.Clear();
			foreach (CustomCommand customCommand in col)
			{
				base.Add(customCommand.Clone());
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0002DF8C File Offset: 0x0002C18C
		public void ExecuteCommand(IProgressMonitor monitor, IWorkspaceObject entry, CustomCommandType type, ConfigurationSelector configuration)
		{
			this.ExecuteCommand(monitor, entry, type, null, configuration);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0002DF9C File Offset: 0x0002C19C
		public void ExecuteCommand(IProgressMonitor monitor, IWorkspaceObject entry, CustomCommandType type, ExecutionContext context, ConfigurationSelector configuration)
		{
			foreach (CustomCommand customCommand in this)
			{
				if (customCommand.Type == type)
				{
					customCommand.Execute(monitor, entry, context, configuration);
				}
				if (monitor.IsCancelRequested)
				{
					break;
				}
			}
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0002E000 File Offset: 0x0002C200
		public bool HasCommands(CustomCommandType type)
		{
			foreach (CustomCommand customCommand in this)
			{
				if (customCommand.Type == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0002E058 File Offset: 0x0002C258
		public bool CanExecute(IWorkspaceObject entry, CustomCommandType type, ExecutionContext context, ConfigurationSelector configuration)
		{
			bool flag = false;
			bool flag2 = true;
			foreach (CustomCommand customCommand in this)
			{
				if (customCommand.Type == type)
				{
					flag = true;
					if (!customCommand.CanExecute(entry, context, configuration))
					{
						flag2 = false;
						break;
					}
				}
			}
			return flag && flag2;
		}
	}
}
