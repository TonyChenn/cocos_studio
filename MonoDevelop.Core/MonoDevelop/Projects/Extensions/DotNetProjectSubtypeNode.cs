using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Addins;
using MonoDevelop.Projects.Formats.MSBuild;

namespace MonoDevelop.Projects.Extensions
{
	// Token: 0x0200019B RID: 411
	[ExtensionNodeChild(typeof(DotNetProjectSubtypeNodeImport), "RemoveImport")]
	[ExtensionNodeChild(typeof(DotNetProjectSubtypeNodeImport), "AddImport")]
	public class DotNetProjectSubtypeNode : ExtensionNode
	{
		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0003A815 File Offset: 0x00038A15
		public string Import
		{
			get
			{
				return this.import;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0003A820 File Offset: 0x00038A20
		public Type Type
		{
			get
			{
				if (this.itemType == null)
				{
					this.itemType = base.Addin.GetType(this.type, true);
					if (!typeof(DotNetProject).IsAssignableFrom(this.itemType))
					{
						throw new InvalidOperationException("Type must be a subclass of DotNetProject");
					}
				}
				return this.itemType;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0003A87B File Offset: 0x00038A7B
		public string Extension
		{
			get
			{
				return this.extension;
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0003A883 File Offset: 0x00038A83
		public string Exclude
		{
			get
			{
				return this.exclude;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0003A88B File Offset: 0x00038A8B
		public string Guid
		{
			get
			{
				return this.guid;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0003A893 File Offset: 0x00038A93
		public bool UseXBuild
		{
			get
			{
				return this.useXBuild;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0003A89B File Offset: 0x00038A9B
		public bool RequireXBuild
		{
			get
			{
				return this.useXBuild && this.requireXBuild;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0003A8AD File Offset: 0x00038AAD
		public bool IsMigration
		{
			get
			{
				return this.migrationHandler != null;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0003A8BB File Offset: 0x00038ABB
		public bool IsMigrationRequired
		{
			get
			{
				return this.migrationRequired;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0003A8C3 File Offset: 0x00038AC3
		public IDotNetSubtypeMigrationHandler MigrationHandler
		{
			get
			{
				return (IDotNetSubtypeMigrationHandler)base.Addin.CreateInstance(this.migrationHandler);
			}
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x0003A8DB File Offset: 0x00038ADB
		public bool SupportsType(string guid)
		{
			return string.Compare(this.guid, guid, true) == 0;
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x0003A8F0 File Offset: 0x00038AF0
		public DotNetProject CreateInstance(string language)
		{
			return (DotNetProject)Activator.CreateInstance(this.Type, new object[]
			{
				language
			});
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0003A919 File Offset: 0x00038B19
		public virtual bool CanHandleItem(SolutionEntityItem item)
		{
			return (!this.IsMigration || !this.IsMigrationRequired) && this.Type.IsAssignableFrom(item.GetType());
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0003A93E File Offset: 0x00038B3E
		public virtual bool CanHandleType(Type type)
		{
			return (!this.IsMigration || !this.IsMigrationRequired) && this.Type.IsAssignableFrom(type);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0003A960 File Offset: 0x00038B60
		public virtual bool CanHandleFile(string fileName, string typeGuid)
		{
			return (typeGuid != null && typeGuid.ToLower().Contains(this.guid.ToLower())) || (!string.IsNullOrEmpty(this.extension) && System.IO.Path.GetExtension(fileName) == "." + this.extension);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0003A9B8 File Offset: 0x00038BB8
		public virtual void InitializeHandler(SolutionEntityItem item)
		{
			MSBuildProjectHandler msbuildProjectHandler = (MSBuildProjectHandler)ProjectExtensionUtil.GetItemHandler(item);
			this.UpdateImports(item, msbuildProjectHandler.TargetImports);
			msbuildProjectHandler.SubtypeGuids.Add(this.guid);
			msbuildProjectHandler.UseMSBuildEngineByDefault |= this.UseXBuild;
			msbuildProjectHandler.RequireMSBuildEngine |= this.RequireXBuild;
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x0003AA3C File Offset: 0x00038C3C
		public void UpdateImports(SolutionEntityItem item, List<string> imports)
		{
			DotNetProject dotNetProject = (DotNetProject)item;
			if (!string.IsNullOrEmpty(this.import))
			{
				imports.AddRange(this.import.Split(new char[]
				{
					':'
				}));
			}
			if (!string.IsNullOrEmpty(this.exclude))
			{
				this.exclude.Split(new char[]
				{
					':'
				}).ToList<string>().ForEach(delegate(string i)
				{
					imports.Remove(i);
				});
			}
			foreach (object obj in base.ChildNodes)
			{
				DotNetProjectSubtypeNodeImport dotNetProjectSubtypeNodeImport = (DotNetProjectSubtypeNodeImport)obj;
				if (dotNetProjectSubtypeNodeImport.Language == dotNetProject.LanguageName)
				{
					if (dotNetProjectSubtypeNodeImport.IsAdd)
					{
						imports.AddRange(dotNetProjectSubtypeNodeImport.Projects.Split(new char[]
						{
							':'
						}));
					}
					else
					{
						dotNetProjectSubtypeNodeImport.Projects.Split(new char[]
						{
							':'
						}).ToList<string>().ForEach(delegate(string i)
						{
							imports.Remove(i);
						});
					}
				}
			}
		}

		// Token: 0x04000489 RID: 1161
		[NodeAttribute]
		private string guid;

		// Token: 0x0400048A RID: 1162
		[NodeAttribute]
		private string type;

		// Token: 0x0400048B RID: 1163
		[NodeAttribute]
		private string import;

		// Token: 0x0400048C RID: 1164
		[NodeAttribute]
		private string extension;

		// Token: 0x0400048D RID: 1165
		[NodeAttribute]
		private string exclude;

		// Token: 0x0400048E RID: 1166
		[NodeAttribute]
		private bool useXBuild;

		// Token: 0x0400048F RID: 1167
		[NodeAttribute]
		private bool requireXBuild = true;

		// Token: 0x04000490 RID: 1168
		[NodeAttribute]
		private string migrationHandler;

		// Token: 0x04000491 RID: 1169
		[NodeAttribute]
		private bool migrationRequired = true;

		// Token: 0x04000492 RID: 1170
		private Type itemType;
	}
}
