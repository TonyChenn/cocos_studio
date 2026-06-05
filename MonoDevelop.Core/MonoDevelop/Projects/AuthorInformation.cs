using System;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	// Token: 0x02000209 RID: 521
	[DataItem]
	public sealed class AuthorInformation
	{
		// Token: 0x060013B7 RID: 5047 RVA: 0x00051924 File Offset: 0x0004FB24
		public AuthorInformation(string name, string email, string copyright, string company, string trademark)
		{
			this.Name = name;
			this.Email = email;
			this.Copyright = copyright;
			this.Company = company;
			this.Trademark = trademark;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00051951 File Offset: 0x0004FB51
		internal AuthorInformation()
		{
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060013B9 RID: 5049 RVA: 0x00051959 File Offset: 0x0004FB59
		// (set) Token: 0x060013BA RID: 5050 RVA: 0x00051961 File Offset: 0x0004FB61
		[ItemProperty]
		public string Name { get; private set; }

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060013BB RID: 5051 RVA: 0x0005196A File Offset: 0x0004FB6A
		// (set) Token: 0x060013BC RID: 5052 RVA: 0x00051972 File Offset: 0x0004FB72
		[ItemProperty]
		public string Email { get; private set; }

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0005197B File Offset: 0x0004FB7B
		// (set) Token: 0x060013BE RID: 5054 RVA: 0x00051983 File Offset: 0x0004FB83
		[ItemProperty]
		public string Copyright { get; private set; }

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0005198C File Offset: 0x0004FB8C
		// (set) Token: 0x060013C0 RID: 5056 RVA: 0x00051994 File Offset: 0x0004FB94
		[ItemProperty]
		public string Company { get; private set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x060013C1 RID: 5057 RVA: 0x0005199D File Offset: 0x0004FB9D
		// (set) Token: 0x060013C2 RID: 5058 RVA: 0x000519A5 File Offset: 0x0004FBA5
		[ItemProperty]
		public string Trademark { get; private set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060013C3 RID: 5059 RVA: 0x000519B0 File Offset: 0x0004FBB0
		public static AuthorInformation Default
		{
			get
			{
				string text = AuthorInformation.GetValueOrMigrate<string>("Author.Name", "ChangeLogAddIn.Name") ?? Environment.UserName;
				string valueOrMigrate = AuthorInformation.GetValueOrMigrate<string>("Author.Email", "ChangeLogAddIn.Email");
				string copyright = PropertyService.Get<string>("Author.Copyright", text);
				string company = PropertyService.Get<string>("Author.Company", "");
				string trademark = PropertyService.Get<string>("Author.Trademark", "");
				return new AuthorInformation(text, valueOrMigrate, copyright, company, trademark);
			}
		}

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x060013C4 RID: 5060 RVA: 0x00051A1E File Offset: 0x0004FC1E
		public bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.Email);
			}
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00051A40 File Offset: 0x0004FC40
		private static T GetValueOrMigrate<T>(string name, string oldName)
		{
			T t = PropertyService.Get<T>(name);
			if (t != null)
			{
				return t;
			}
			t = PropertyService.Get<T>(oldName);
			if (t != null)
			{
				PropertyService.Set(oldName, null);
				PropertyService.Set(name, t);
			}
			return t;
		}
	}
}
