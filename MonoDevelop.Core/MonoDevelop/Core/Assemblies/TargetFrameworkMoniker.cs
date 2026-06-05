using System;
using System.IO;

namespace MonoDevelop.Core.Assemblies
{
	/// <summary>
	/// Unique identifier for a target framework.
	/// </summary>
	// Token: 0x02000215 RID: 533
	[Serializable]
	public class TargetFrameworkMoniker : IEquatable<TargetFrameworkMoniker>
	{
		// Token: 0x06001400 RID: 5120 RVA: 0x0005311F File Offset: 0x0005131F
		private TargetFrameworkMoniker()
		{
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x00053127 File Offset: 0x00051327
		public TargetFrameworkMoniker(string version) : this(".NETFramework", version, null)
		{
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00053136 File Offset: 0x00051336
		public TargetFrameworkMoniker(string identifier, string version) : this(identifier, version, null)
		{
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00053144 File Offset: 0x00051344
		public TargetFrameworkMoniker(string identifier, string version, string profile)
		{
			if (version == null || version.Length == 0 || (version.Length == 1 && version[0] == 'v'))
			{
				throw new ArgumentException("A version must be provided", "version");
			}
			if (string.IsNullOrEmpty(identifier))
			{
				throw new ArgumentException("An identifier must be provided", "identifier");
			}
			if (version[0] == 'v')
			{
				version = version.Substring(1);
			}
			if (profile != null & profile == "")
			{
				profile = null;
			}
			this.identifier = identifier;
			this.version = version;
			this.profile = profile;
		}

		/// <summary>
		/// The root identifier of the framework, e.g. ".NETFramework" or "Silverlight"
		/// </summary>
		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x000531DF File Offset: 0x000513DF
		public string Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		/// <summary>
		/// The version of the framework.
		/// </summary>
		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001405 RID: 5125 RVA: 0x000531E7 File Offset: 0x000513E7
		public string Version
		{
			get
			{
				return this.version;
			}
		}

		/// <summary>
		/// Optional. A named subset of a particular framework version, e.g. "Client".
		/// </summary>
		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001406 RID: 5126 RVA: 0x000531EF File Offset: 0x000513EF
		public string Profile
		{
			get
			{
				return this.profile;
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x000531F8 File Offset: 0x000513F8
		public static TargetFrameworkMoniker Parse(string value)
		{
			TargetFrameworkMoniker result;
			if (!TargetFrameworkMoniker.TryParse(value, out result))
			{
				throw new FormatException(string.Format("Invalid framework moniker '{0}'", value));
			}
			return result;
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x00053221 File Offset: 0x00051421
		public static bool TryParse(string value, out TargetFrameworkMoniker moniker)
		{
			moniker = new TargetFrameworkMoniker();
			if (moniker.ParseInternal(value))
			{
				return true;
			}
			moniker = null;
			return false;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0005323C File Offset: 0x0005143C
		private bool ParseInternal(string value)
		{
			this.profile = null;
			int num = value.IndexOf(',');
			if (num < 1)
			{
				if (value == "SL2.0")
				{
					this.identifier = "Silverlight";
					this.version = "2.0";
					return true;
				}
				if (value == "SL3.0")
				{
					this.identifier = "Silverlight";
					this.version = "3.0";
					return true;
				}
				if (value == "IPhone")
				{
					this.identifier = "MonoTouch";
					this.version = "1.0";
					return true;
				}
				if (value[0] == 'v')
				{
					value = value.Substring(1);
				}
				this.identifier = ".NETFramework";
				this.version = value;
			}
			else
			{
				this.identifier = value.Substring(0, num);
				if (value.IndexOf(",Version=v", num, ",Version=v".Length, StringComparison.Ordinal) != num)
				{
					return false;
				}
				num += ",Version=v".Length;
				int num2 = value.IndexOf(',', num);
				if (num2 < 0)
				{
					this.version = value.Substring(num);
				}
				else
				{
					this.version = value.Substring(num, num2 - num);
					this.profile = value.Substring(num2 + ",Profile=".Length);
				}
			}
			Version version;
			return System.Version.TryParse(this.version, out version);
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00053380 File Offset: 0x00051580
		internal string ToLegacyIdString()
		{
			if (this.identifier == "Silverlight" && (this.version == "2.0" || this.version == "3.0"))
			{
				return "SL" + this.version;
			}
			if (this.identifier == "MonoTouch" && this.version == "1.0")
			{
				return "IPhone";
			}
			if (this.identifier == ".NETFramework")
			{
				return this.version;
			}
			return this.ToString();
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x00053420 File Offset: 0x00051620
		public override string ToString()
		{
			string text = this.identifier + ",Version=v" + this.version;
			if (this.profile != null)
			{
				text = text + ",Profile=" + this.profile;
			}
			return text;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x00053460 File Offset: 0x00051660
		public string GetAssemblyDirectoryName()
		{
			if (this.profile != null)
			{
				return Path.Combine(this.identifier, "v" + this.version, "Profile", this.profile);
			}
			return Path.Combine(this.identifier, "v" + this.version);
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x000534B8 File Offset: 0x000516B8
		public bool Equals(TargetFrameworkMoniker other)
		{
			return other != null && this.identifier == other.identifier && this.version == other.version && this.profile == other.profile;
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00053507 File Offset: 0x00051707
		public override bool Equals(object obj)
		{
			return this.Equals(obj as TargetFrameworkMoniker);
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00053518 File Offset: 0x00051718
		public override int GetHashCode()
		{
			int num = 0;
			if (this.identifier != null)
			{
				num ^= this.identifier.GetHashCode();
			}
			if (this.version != null)
			{
				num ^= this.version.GetHashCode();
			}
			if (this.profile != null)
			{
				num ^= this.profile.GetHashCode();
			}
			return num;
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x0005356A File Offset: 0x0005176A
		public static bool operator ==(TargetFrameworkMoniker a, TargetFrameworkMoniker b)
		{
			if (a == null)
			{
				return b == null;
			}
			return a.Equals(b);
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x0005357B File Offset: 0x0005177B
		public static bool operator !=(TargetFrameworkMoniker a, TargetFrameworkMoniker b)
		{
			if (a == null)
			{
				return b != null;
			}
			return !a.Equals(b);
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x00053592 File Offset: 0x00051792
		public static TargetFrameworkMoniker Default
		{
			get
			{
				return TargetFrameworkMoniker.NET_1_1;
			}
		}

		// Token: 0x17000433 RID: 1075
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x00053599 File Offset: 0x00051799
		public static TargetFrameworkMoniker NET_1_1
		{
			get
			{
				return new TargetFrameworkMoniker("1.1");
			}
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x000535A5 File Offset: 0x000517A5
		public static TargetFrameworkMoniker NET_2_0
		{
			get
			{
				return new TargetFrameworkMoniker("2.0");
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x000535B1 File Offset: 0x000517B1
		public static TargetFrameworkMoniker NET_3_0
		{
			get
			{
				return new TargetFrameworkMoniker("3.0");
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x000535BD File Offset: 0x000517BD
		public static TargetFrameworkMoniker NET_3_5
		{
			get
			{
				return new TargetFrameworkMoniker("3.5");
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x000535C9 File Offset: 0x000517C9
		public static TargetFrameworkMoniker NET_4_0
		{
			get
			{
				return new TargetFrameworkMoniker("4.0");
			}
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x000535D5 File Offset: 0x000517D5
		public static TargetFrameworkMoniker NET_4_5
		{
			get
			{
				return new TargetFrameworkMoniker("4.5");
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x000535E1 File Offset: 0x000517E1
		public static TargetFrameworkMoniker PORTABLE_4_0
		{
			get
			{
				return new TargetFrameworkMoniker(".NETPortable", "4.0", "Profile1");
			}
		}

		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x000535F7 File Offset: 0x000517F7
		public static TargetFrameworkMoniker SL_2_0
		{
			get
			{
				return new TargetFrameworkMoniker("Silverlight", "2.0");
			}
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x00053608 File Offset: 0x00051808
		public static TargetFrameworkMoniker SL_3_0
		{
			get
			{
				return new TargetFrameworkMoniker("Silverlight", "3.0");
			}
		}

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x0600141C RID: 5148 RVA: 0x00053619 File Offset: 0x00051819
		public static TargetFrameworkMoniker SL_4_0
		{
			get
			{
				return new TargetFrameworkMoniker("Silverlight", "4.0");
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x0600141D RID: 5149 RVA: 0x0005362A File Offset: 0x0005182A
		public static TargetFrameworkMoniker MONOTOUCH_1_0
		{
			get
			{
				return new TargetFrameworkMoniker("MonoTouch", "1.0");
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x0600141E RID: 5150 RVA: 0x0005363B File Offset: 0x0005183B
		public static TargetFrameworkMoniker UNKNOWN
		{
			get
			{
				return new TargetFrameworkMoniker("Unknown", "0.0");
			}
		}

		// Token: 0x04000601 RID: 1537
		public const string ID_NET_FRAMEWORK = ".NETFramework";

		// Token: 0x04000602 RID: 1538
		public const string ID_SILVERLIGHT = "Silverlight";

		// Token: 0x04000603 RID: 1539
		public const string ID_PORTABLE = ".NETPortable";

		// Token: 0x04000604 RID: 1540
		public const string ID_MONOTOUCH = "MonoTouch";

		// Token: 0x04000605 RID: 1541
		public const string ID_MONODROID = "MonoAndroid";

		// Token: 0x04000606 RID: 1542
		private string identifier;

		// Token: 0x04000607 RID: 1543
		private string version;

		// Token: 0x04000608 RID: 1544
		private string profile;
	}
}
