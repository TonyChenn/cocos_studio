using System;
using System.Collections.Generic;
using System.Xml;

namespace MonoDevelop.Core.Assemblies
{
	// Token: 0x02000245 RID: 581
	public class SupportedFramework
	{
		// Token: 0x06001564 RID: 5476 RVA: 0x000571AA File Offset: 0x000553AA
		public SupportedFramework(TargetFramework target, string identifier, string display, string profile, Version minVersion, string minDisplayVersion)
		{
			this.MinimumVersionDisplayName = minDisplayVersion;
			this.MinimumVersion = minVersion;
			this.MaximumVersion = SupportedFramework.NoMaximumVersion;
			this.DisplayName = display;
			this.Identifier = identifier;
			this.Profile = profile;
			this.TargetFramework = target;
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x000571EC File Offset: 0x000553EC
		internal SupportedFramework(TargetFramework target)
		{
			this.MinimumVersionDisplayName = string.Empty;
			this.MinimumVersion = SupportedFramework.NoMinumumVersion;
			this.MaximumVersion = SupportedFramework.NoMaximumVersion;
			this.DisplayName = string.Empty;
			this.Identifier = string.Empty;
			this.Profile = string.Empty;
			this.TargetFramework = target;
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x06001566 RID: 5478 RVA: 0x00057248 File Offset: 0x00055448
		// (set) Token: 0x06001567 RID: 5479 RVA: 0x00057250 File Offset: 0x00055450
		public string DisplayName { get; internal set; }

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x00057259 File Offset: 0x00055459
		// (set) Token: 0x06001569 RID: 5481 RVA: 0x00057261 File Offset: 0x00055461
		public string Identifier { get; internal set; }

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600156A RID: 5482 RVA: 0x0005726A File Offset: 0x0005546A
		// (set) Token: 0x0600156B RID: 5483 RVA: 0x00057272 File Offset: 0x00055472
		public string Profile { get; internal set; }

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600156C RID: 5484 RVA: 0x0005727B File Offset: 0x0005547B
		// (set) Token: 0x0600156D RID: 5485 RVA: 0x00057283 File Offset: 0x00055483
		public string MinimumVersionDisplayName { get; internal set; }

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x0600156E RID: 5486 RVA: 0x0005728C File Offset: 0x0005548C
		// (set) Token: 0x0600156F RID: 5487 RVA: 0x00057294 File Offset: 0x00055494
		public Version MinimumVersion { get; internal set; }

		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x0005729D File Offset: 0x0005549D
		// (set) Token: 0x06001571 RID: 5489 RVA: 0x000572A5 File Offset: 0x000554A5
		public Version MaximumVersion { get; internal set; }

		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x06001572 RID: 5490 RVA: 0x000572AE File Offset: 0x000554AE
		// (set) Token: 0x06001573 RID: 5491 RVA: 0x000572B6 File Offset: 0x000554B6
		public string MonoSpecificVersion { get; internal set; }

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x06001574 RID: 5492 RVA: 0x000572BF File Offset: 0x000554BF
		// (set) Token: 0x06001575 RID: 5493 RVA: 0x000572C7 File Offset: 0x000554C7
		public string MonoSpecificVersionDisplayName { get; internal set; }

		// Token: 0x17000490 RID: 1168
		// (get) Token: 0x06001576 RID: 5494 RVA: 0x000572D0 File Offset: 0x000554D0
		// (set) Token: 0x06001577 RID: 5495 RVA: 0x000572D8 File Offset: 0x000554D8
		public TargetFramework TargetFramework { get; private set; }

		// Token: 0x06001578 RID: 5496 RVA: 0x000572E1 File Offset: 0x000554E1
		private static Version ParseVersion(string version, Version wildcard)
		{
			if (version == "*")
			{
				return wildcard;
			}
			return Version.Parse(version);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x000572F8 File Offset: 0x000554F8
		internal static SupportedFramework Load(TargetFramework target, FilePath path)
		{
			SupportedFramework supportedFramework = new SupportedFramework(target);
			supportedFramework.DisplayName = path.FileNameWithoutExtension;
			using (XmlReader xmlReader = XmlReader.Create(path))
			{
				if (!xmlReader.ReadToDescendant("Framework"))
				{
					throw new Exception("Missing Framework element");
				}
				if (!xmlReader.HasAttributes)
				{
					throw new Exception("Framework element does not contain any attributes");
				}
				while (xmlReader.MoveToNextAttribute())
				{
					string name;
					switch (name = xmlReader.Name)
					{
					case "MaximumVersion":
						supportedFramework.MaximumVersion = SupportedFramework.ParseVersion(xmlReader.Value, SupportedFramework.NoMaximumVersion);
						break;
					case "MinimumVersion":
						supportedFramework.MinimumVersion = SupportedFramework.ParseVersion(xmlReader.Value, SupportedFramework.NoMinumumVersion);
						break;
					case "Profile":
						supportedFramework.Profile = xmlReader.Value;
						break;
					case "Identifier":
						supportedFramework.Identifier = xmlReader.Value;
						break;
					case "MinimumVersionDisplayName":
						supportedFramework.MinimumVersionDisplayName = xmlReader.Value;
						break;
					case "DisplayName":
						supportedFramework.DisplayName = xmlReader.Value;
						break;
					case "MonoSpecificVersion":
						supportedFramework.MonoSpecificVersion = xmlReader.Value;
						break;
					case "MonoSpecificVersionDisplayName":
						supportedFramework.MonoSpecificVersionDisplayName = xmlReader.Value;
						break;
					}
				}
			}
			if (string.IsNullOrEmpty(supportedFramework.Identifier))
			{
				throw new Exception("Framework element did not specify an Identifier attribute");
			}
			return supportedFramework;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x000574E4 File Offset: 0x000556E4
		public override int GetHashCode()
		{
			if (this.DisplayName == null)
			{
				return 0;
			}
			return this.DisplayName.GetHashCode();
		}

		// Token: 0x0600157B RID: 5499 RVA: 0x000574FC File Offset: 0x000556FC
		public override bool Equals(object obj)
		{
			SupportedFramework supportedFramework = obj as SupportedFramework;
			return supportedFramework != null && string.Equals(this.DisplayName, supportedFramework.DisplayName) && string.Equals(this.Identifier, supportedFramework.Identifier) && string.Equals(this.Profile, supportedFramework.Profile) && string.Equals(this.MonoSpecificVersion, supportedFramework.MonoSpecificVersion) && string.Equals(this.MonoSpecificVersionDisplayName, supportedFramework.MonoSpecificVersionDisplayName) && string.Equals(this.MinimumVersionDisplayName, supportedFramework.MinimumVersionDisplayName) && this.MinimumVersion.Equals(supportedFramework.MinimumVersion) && this.MaximumVersion.Equals(supportedFramework.MaximumVersion);
		}

		// Token: 0x0400066E RID: 1646
		public static readonly Version NoMaximumVersion = new Version(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);

		// Token: 0x0400066F RID: 1647
		public static readonly Version NoMinumumVersion = new Version(0, 0, 0, 0);

		// Token: 0x04000670 RID: 1648
		public static IEqualityComparer<SupportedFramework> EqualityComparer = new SupportedFramework._Comparer();

		// Token: 0x02000246 RID: 582
		private class _Comparer : IEqualityComparer<SupportedFramework>
		{
			// Token: 0x0600157D RID: 5501 RVA: 0x000575F6 File Offset: 0x000557F6
			public bool Equals(SupportedFramework x, SupportedFramework y)
			{
				return x.Equals(y);
			}

			// Token: 0x0600157E RID: 5502 RVA: 0x000575FF File Offset: 0x000557FF
			public int GetHashCode(SupportedFramework obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
