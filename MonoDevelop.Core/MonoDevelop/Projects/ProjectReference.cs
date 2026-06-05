using System;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Projects
{
	/// <summary>
	/// This class represent a reference information in an Project object.
	/// </summary>
	// Token: 0x02000138 RID: 312
	[DataItem(FallbackType = typeof(UnknownProjectReference))]
	public class ProjectReference : ProjectItem, ICloneable
	{
		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000B9D RID: 2973 RVA: 0x0002B74C File Offset: 0x0002994C
		// (remove) Token: 0x06000B9E RID: 2974 RVA: 0x0002B784 File Offset: 0x00029984
		public event EventHandler StatusChanged;

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x0002B7BC File Offset: 0x000299BC
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x0002B7E7 File Offset: 0x000299E7
		[ItemProperty("Package", DefaultValue = "")]
		internal string packageName
		{
			get
			{
				SystemPackage systemPackage = this.Package;
				if (systemPackage != null && !systemPackage.IsGacPackage)
				{
					return systemPackage.Name;
				}
				return string.Empty;
			}
			set
			{
				this.package = value;
			}
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0002B7F0 File Offset: 0x000299F0
		public ProjectReference()
		{
		}

		// Token: 0x06000BA2 RID: 2978 RVA: 0x0002B818 File Offset: 0x00029A18
		internal void SetOwnerProject(DotNetProject project)
		{
			this.ownerProject = project;
			this.UpdatePackageReference();
		}

		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002B827 File Offset: 0x00029A27
		public ProjectReference(ReferenceType referenceType, string reference) : this(referenceType, reference, null)
		{
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002B834 File Offset: 0x00029A34
		public ProjectReference(ReferenceType referenceType, string reference, string hintPath)
		{
			if (referenceType == ReferenceType.Assembly)
			{
				this.specificVersion = false;
				if (hintPath == null)
				{
					hintPath = reference;
					reference = Path.GetFileNameWithoutExtension(reference);
				}
			}
			this.referenceType = referenceType;
			this.reference = reference;
			this.hintPath = hintPath;
			this.UpdatePackageReference();
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002B89C File Offset: 0x00029A9C
		public ProjectReference(Project referencedProject)
		{
			this.referenceType = ReferenceType.Project;
			this.reference = referencedProject.Name;
			this.specificVersion = true;
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002B8EC File Offset: 0x00029AEC
		public ProjectReference(SystemAssembly asm)
		{
			this.referenceType = ReferenceType.Package;
			this.reference = asm.FullName;
			if (asm.Package.IsFrameworkPackage)
			{
				this.specificVersion = false;
			}
			if (!asm.Package.IsGacPackage)
			{
				this.package = asm.Package.Name;
			}
			this.UpdatePackageReference();
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002B96A File Offset: 0x00029B6A
		protected void InitCustomReference(string reference)
		{
			this.Reference = reference;
			this.referenceType = ReferenceType.Custom;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002B97C File Offset: 0x00029B7C
		public static ProjectReference RenameReference(ProjectReference pref, string newReference)
		{
			ProjectReference projectReference = (ProjectReference)pref.MemberwiseClone();
			projectReference.reference = newReference;
			return projectReference;
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x0002B99D File Offset: 0x00029B9D
		public Project OwnerProject
		{
			get
			{
				return this.ownerProject;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0002B9A5 File Offset: 0x00029BA5
		// (set) Token: 0x06000BAB RID: 2987 RVA: 0x0002B9AD File Offset: 0x00029BAD
		internal ReferenceType internalReferenceType
		{
			get
			{
				return this.referenceType;
			}
			set
			{
				this.referenceType = value;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000BAC RID: 2988 RVA: 0x0002B9B6 File Offset: 0x00029BB6
		public ReferenceType ReferenceType
		{
			get
			{
				return this.referenceType;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0002B9BE File Offset: 0x00029BBE
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x0002B9C6 File Offset: 0x00029BC6
		public string Reference
		{
			get
			{
				return this.reference;
			}
			internal set
			{
				this.reference = value;
				this.UpdatePackageReference();
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0002B9D5 File Offset: 0x00029BD5
		public string StoredReference
		{
			get
			{
				if (!string.IsNullOrEmpty(this.loadedReference))
				{
					return this.loadedReference;
				}
				return this.reference;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0002B9F1 File Offset: 0x00029BF1
		// (set) Token: 0x06000BB1 RID: 2993 RVA: 0x0002BA12 File Offset: 0x00029C12
		public bool LocalCopy
		{
			get
			{
				if (this.localCopy != null)
				{
					return this.localCopy.Value;
				}
				return this.DefaultLocalCopy;
			}
			set
			{
				this.localCopy = new bool?(value);
				if (this.ownerProject != null)
				{
					this.ownerProject.NotifyModified(null);
				}
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0002BA34 File Offset: 0x00029C34
		// (set) Token: 0x06000BB3 RID: 2995 RVA: 0x0002BA3C File Offset: 0x00029C3C
		public bool ReferenceOutputAssembly
		{
			get
			{
				return this.referenceOutputAssembly;
			}
			set
			{
				if (this.referenceOutputAssembly != value)
				{
					this.referenceOutputAssembly = value;
					this.OnStatusChanged();
				}
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0002BA54 File Offset: 0x00029C54
		internal bool DefaultLocalCopy
		{
			get
			{
				return this.referenceType != ReferenceType.Package;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0002BA62 File Offset: 0x00029C62
		public bool CanSetLocalCopy
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0002BA65 File Offset: 0x00029C65
		// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x0002BA6D File Offset: 0x00029C6D
		public bool SpecificVersion
		{
			get
			{
				return this.specificVersion;
			}
			set
			{
				if (this.specificVersion != value)
				{
					this.specificVersion = value;
					this.OnStatusChanged();
				}
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0002BA85 File Offset: 0x00029C85
		public bool CanSetSpecificVersion
		{
			get
			{
				return this.ReferenceType != ReferenceType.Project && this.ReferenceType != ReferenceType.Custom && (this.ReferenceType != ReferenceType.Package || this.Package == null || !this.Package.IsFrameworkPackage);
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0002BABC File Offset: 0x00029CBC
		// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0002BAC4 File Offset: 0x00029CC4
		[ItemProperty("Aliases", DefaultValue = "")]
		public string Aliases { get; set; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000BBB RID: 3003 RVA: 0x0002BACD File Offset: 0x00029CCD
		public bool IsValid
		{
			get
			{
				return string.IsNullOrEmpty(this.ValidationErrorMessage);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0002BADC File Offset: 0x00029CDC
		public virtual string ValidationErrorMessage
		{
			get
			{
				if (this.customError != null)
				{
					return this.customError;
				}
				if (this.ReferenceType == ReferenceType.Package)
				{
					if (!this.IsExactVersion && this.SpecificVersion)
					{
						return GettextCatalog.GetString("Specified version not found: expected {0}, found {1}", this.GetVersionNum(this.StoredReference), this.GetVersionNum(this.Reference));
					}
					if (this.notFound)
					{
						if (this.ownerProject == null)
						{
							return GettextCatalog.GetString("Assembly not found");
						}
						bool flag = Runtime.SystemAssemblyService.DefaultRuntime == this.TargetRuntime;
						string value = base.ExtendedProperties["_OriginalMSBuildReferenceHintPath"] as string;
						bool flag2 = string.IsNullOrEmpty(value);
						if (this.TargetRuntime.IsInstalled(this.TargetFramework) || !flag2)
						{
							if (flag)
							{
								return GettextCatalog.GetString("Assembly not found for framework {0}", this.TargetFramework.Name);
							}
							return GettextCatalog.GetString("Assembly not found for framework {0} (in {1})", this.TargetFramework.Name, this.TargetRuntime.DisplayName);
						}
						else
						{
							if (flag)
							{
								return GettextCatalog.GetString("Framework {0} is not installed", this.TargetFramework.Name);
							}
							return GettextCatalog.GetString("Framework {0} is not installed (in {1})", this.TargetFramework.Name, this.TargetRuntime.DisplayName);
						}
					}
				}
				else if (this.ReferenceType == ReferenceType.Project)
				{
					if (this.ownerProject != null && this.ownerProject.ParentSolution != null && this.ReferenceOutputAssembly)
					{
						DotNetProject dotNetProject = this.ownerProject.ParentSolution.FindProjectByName(this.reference) as DotNetProject;
						if (dotNetProject != null && !this.ownerProject.TargetFramework.CanReferenceAssembliesTargetingFramework(dotNetProject.TargetFramework))
						{
							return GettextCatalog.GetString("Incompatible target framework ({0})", dotNetProject.TargetFramework.Name);
						}
					}
				}
				else if (this.ReferenceType == ReferenceType.Assembly && !File.Exists(this.hintPath))
				{
					return GettextCatalog.GetString("File not found");
				}
				return string.Empty;
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0002BCAD File Offset: 0x00029EAD
		public void SetInvalid(string message)
		{
			this.customError = message;
			this.OnStatusChanged();
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000BBE RID: 3006 RVA: 0x0002BCBC File Offset: 0x00029EBC
		public bool IsExactVersion
		{
			get
			{
				if (this.ReferenceType == ReferenceType.Package)
				{
					string a = MonoDevelop.Core.Assemblies.AssemblyContext.NormalizeAsmName(this.StoredReference);
					string b = MonoDevelop.Core.Assemblies.AssemblyContext.NormalizeAsmName(this.Reference);
					return a == b;
				}
				return true;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0002BCF3 File Offset: 0x00029EF3
		public string HintPath
		{
			get
			{
				return this.hintPath;
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0002BCFC File Offset: 0x00029EFC
		private string GetVersionNum(string asmName)
		{
			int num = asmName.IndexOf(',');
			if (num != -1)
			{
				num++;
				int num2 = asmName.IndexOf(',', num);
				if (num2 == -1)
				{
					num2 = asmName.Length;
				}
				string text = asmName.Substring(num, num2 - num).Trim();
				if (text.Length > 0)
				{
					return text;
				}
			}
			return "0.0.0.0";
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0002BD50 File Offset: 0x00029F50
		internal ProjectReference GetRefreshedReference()
		{
			if (this.customError != null)
			{
				return null;
			}
			if (this.ReferenceType == ReferenceType.Package)
			{
				if (!string.IsNullOrEmpty(this.hintPath) && File.Exists(this.hintPath))
				{
					ProjectReference projectReference = (ProjectReference)base.MemberwiseClone();
					projectReference.referenceType = ReferenceType.Assembly;
					return projectReference;
				}
			}
			else if (this.ReferenceType == ReferenceType.Assembly && !string.IsNullOrEmpty(this.hintPath) && !File.Exists(this.hintPath))
			{
				ProjectReference projectReference2 = (ProjectReference)base.MemberwiseClone();
				projectReference2.referenceType = ReferenceType.Package;
				return projectReference2;
			}
			return null;
		}

		/// <summary>
		/// Returns the file name to an assembly, regardless of what 
		/// type the assembly is.
		/// </summary>
		// Token: 0x06000BC2 RID: 3010 RVA: 0x0002BDD8 File Offset: 0x00029FD8
		private string GetReferencedFileName(ConfigurationSelector configuration)
		{
			switch (this.ReferenceType)
			{
			case ReferenceType.Assembly:
				return this.hintPath;
			case ReferenceType.Project:
				if (this.ownerProject != null && this.ownerProject.ParentSolution != null)
				{
					Project project = this.ownerProject.ParentSolution.FindProjectByName(this.reference);
					if (project != null)
					{
						return project.GetOutputFileName(configuration);
					}
				}
				return null;
			case ReferenceType.Package:
			{
				string assemblyLocation = this.AssemblyContext.GetAssemblyLocation(this.Reference, this.package, (this.ownerProject != null) ? this.ownerProject.TargetFramework : null);
				if (assemblyLocation != null)
				{
					return assemblyLocation;
				}
				return this.reference;
			}
			default:
				return null;
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0002BE80 File Offset: 0x0002A080
		public virtual string[] GetReferencedFileNames(ConfigurationSelector configuration)
		{
			string referencedFileName = this.GetReferencedFileName(configuration);
			if (referencedFileName != null)
			{
				return new string[]
				{
					referencedFileName
				};
			}
			return new string[0];
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0002BEAC File Offset: 0x0002A0AC
		private void UpdatePackageReference()
		{
			if (this.referenceType == ReferenceType.Package && this.ownerProject != null)
			{
				this.notFound = false;
				string text = this.AssemblyContext.FindInstalledAssembly(this.reference, this.package, this.ownerProject.TargetFramework);
				if (text == null)
				{
					text = this.reference;
				}
				text = this.AssemblyContext.GetAssemblyNameForVersion(text, this.package, this.ownerProject.TargetFramework);
				this.notFound = (text == null);
				if (text != null && text != this.reference)
				{
					SystemAssembly assemblyFromFullName = this.AssemblyContext.GetAssemblyFromFullName(text, this.package, this.ownerProject.TargetFramework);
					bool flag = assemblyFromFullName != null && assemblyFromFullName.Package.IsFrameworkPackage;
					if (this.loadedReference == null && !flag)
					{
						this.loadedReference = this.reference;
					}
					this.reference = text;
				}
				this.cachedPackage = null;
				this.OnStatusChanged();
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000BC5 RID: 3013 RVA: 0x0002BF98 File Offset: 0x0002A198
		private IAssemblyContext AssemblyContext
		{
			get
			{
				if (this.ownerProject != null)
				{
					return this.ownerProject.AssemblyContext;
				}
				return Runtime.SystemAssemblyService.DefaultAssemblyContext;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0002BFB8 File Offset: 0x0002A1B8
		private TargetRuntime TargetRuntime
		{
			get
			{
				if (this.ownerProject != null)
				{
					return this.ownerProject.TargetRuntime;
				}
				return Runtime.SystemAssemblyService.DefaultRuntime;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000BC7 RID: 3015 RVA: 0x0002BFD8 File Offset: 0x0002A1D8
		private TargetFramework TargetFramework
		{
			get
			{
				if (this.ownerProject != null)
				{
					return this.ownerProject.TargetFramework;
				}
				return null;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0002BFF0 File Offset: 0x0002A1F0
		public SystemPackage Package
		{
			get
			{
				if (this.referenceType == ReferenceType.Package)
				{
					if (this.cachedPackage != null)
					{
						return this.cachedPackage;
					}
					if (this.package != null)
					{
						return this.AssemblyContext.GetPackage(this.package);
					}
					TargetFramework fx = (this.ownerProject == null) ? null : this.ownerProject.TargetFramework;
					SystemAssembly assemblyFromFullName = this.AssemblyContext.GetAssemblyFromFullName(this.reference, null, fx);
					if (assemblyFromFullName != null)
					{
						return this.cachedPackage = assemblyFromFullName.Package;
					}
				}
				return null;
			}
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0002C06E File Offset: 0x0002A26E
		internal void ResetReference()
		{
			this.cachedPackage = null;
			if (this.loadedReference != null)
			{
				this.reference = this.loadedReference;
				this.loadedReference = null;
				this.UpdatePackageReference();
				return;
			}
			this.UpdatePackageReference();
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0002C09F File Offset: 0x0002A29F
		public object Clone()
		{
			return base.MemberwiseClone();
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0002C0A7 File Offset: 0x0002A2A7
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ProjectReference);
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0002C0B5 File Offset: 0x0002A2B5
		public bool Equals(ProjectReference other)
		{
			return other != null && this.StoredReference == other.StoredReference && this.referenceType == other.referenceType && this.package == other.package;
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0002C0F0 File Offset: 0x0002A2F0
		public override int GetHashCode()
		{
			int num = 0;
			if (this.StoredReference != null)
			{
				num ^= this.StoredReference.GetHashCode();
			}
			if (this.package != null)
			{
				num ^= this.package.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0002C12C File Offset: 0x0002A32C
		internal void NotifyStatusChanged()
		{
			this.OnStatusChanged();
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0002C134 File Offset: 0x0002A334
		protected virtual void OnStatusChanged()
		{
			if (this.StatusChanged != null)
			{
				this.StatusChanged(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// Resolves a project for a ReferenceType.Project reference type in a given solution.
		/// </summary>
		/// <returns>The project, or <c>null</c> if it couldn't be resolved.</returns>
		/// <param name="inSolution">The solution the project is in.</param>
		/// <exception cref="T:System.ArgumentNullException">Thrown if inSolution == null</exception>
		/// <exception cref="T:System.InvalidOperationException">Thrown if ReferenceType != ReferenceType.Project</exception>
		// Token: 0x06000BD0 RID: 3024 RVA: 0x0002C14F File Offset: 0x0002A34F
		public Project ResolveProject(Solution inSolution)
		{
			if (inSolution == null)
			{
				throw new ArgumentNullException("inSolution");
			}
			if (this.ReferenceType != ReferenceType.Project)
			{
				throw new InvalidOperationException("ResolveProject is only definied for Project reference type.");
			}
			return inSolution.FindProjectByName(this.Reference);
		}

		// Token: 0x04000382 RID: 898
		private ReferenceType referenceType = ReferenceType.Custom;

		// Token: 0x04000383 RID: 899
		private DotNetProject ownerProject;

		// Token: 0x04000384 RID: 900
		private string reference = string.Empty;

		// Token: 0x04000385 RID: 901
		private bool? localCopy;

		// Token: 0x04000386 RID: 902
		private string loadedReference;

		// Token: 0x04000387 RID: 903
		private bool specificVersion = true;

		// Token: 0x04000388 RID: 904
		private bool notFound;

		// Token: 0x04000389 RID: 905
		private string package;

		// Token: 0x0400038A RID: 906
		private SystemPackage cachedPackage;

		// Token: 0x0400038B RID: 907
		private string customError;

		// Token: 0x0400038C RID: 908
		private string hintPath;

		// Token: 0x0400038E RID: 910
		private bool referenceOutputAssembly = true;
	}
}
