using System;
using Mono.Addins;
using Mono.Addins.Setup;

namespace MonoDevelop.Core.Setup
{
	// Token: 0x0200021F RID: 543
	public class AddinSetupService : SetupService
	{
		// Token: 0x06001468 RID: 5224 RVA: 0x00054499 File Offset: 0x00052699
		internal AddinSetupService(AddinRegistry r) : base(r)
		{
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000544A4 File Offset: 0x000526A4
		public bool IsMainRepositoryRegistered(UpdateLevel level)
		{
			string mainRepositoryUrl = this.GetMainRepositoryUrl(level);
			return base.Repositories.ContainsRepository(mainRepositoryUrl);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x000544C8 File Offset: 0x000526C8
		public void RegisterMainRepository(UpdateLevel level, bool enable)
		{
			string mainRepositoryUrl = this.GetMainRepositoryUrl(level);
			if (!base.Repositories.ContainsRepository(mainRepositoryUrl))
			{
				AddinRepository addinRepository = base.Repositories.RegisterRepository(null, mainRepositoryUrl, false);
				addinRepository.Name = BrandingService.BrandApplicationName("MonoDevelop Add-in Repository");
				if (level != UpdateLevel.Stable)
				{
					AddinRepository addinRepository2 = addinRepository;
					object name = addinRepository2.Name;
					addinRepository2.Name = string.Concat(new object[]
					{
						name,
						" (",
						level,
						" channel)"
					});
				}
				if (!enable)
				{
					base.Repositories.SetRepositoryEnabled(mainRepositoryUrl, false);
				}
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00054554 File Offset: 0x00052754
		public string GetMainRepositoryUrl(UpdateLevel level)
		{
			string text;
			if (Platform.IsWindows)
			{
				text = "Win32";
			}
			else if (Platform.IsMac)
			{
				text = "Mac";
			}
			else
			{
				text = "Linux";
			}
			return string.Concat(new object[]
			{
				"http://addins.monodevelop.com/",
				level,
				"/",
				text,
				"/",
				AddinManager.CurrentAddin.Version,
				"/main.mrep"
			});
		}
	}
}
