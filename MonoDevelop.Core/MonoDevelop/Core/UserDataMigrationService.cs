using System;
using System.IO;
using Mono.Addins;
using MonoDevelop.Core.AddIns;

namespace MonoDevelop.Core
{
	// Token: 0x02000217 RID: 535
	internal static class UserDataMigrationService
	{
		// Token: 0x06001425 RID: 5157 RVA: 0x00053688 File Offset: 0x00051888
		public static void SetMigrationSource(UserProfile profile, string version)
		{
			if (UserDataMigrationService.profile != null)
			{
				throw new InvalidOperationException("Already set");
			}
			if (profile == null)
			{
				throw new ArgumentNullException("profile");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			UserDataMigrationService.profile = profile;
			UserDataMigrationService.version = version;
		}

		// Token: 0x17000442 RID: 1090
		// (get) Token: 0x06001426 RID: 5158 RVA: 0x000536C4 File Offset: 0x000518C4
		public static string SourceVersion
		{
			get
			{
				return UserDataMigrationService.version;
			}
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x06001427 RID: 5159 RVA: 0x000536CB File Offset: 0x000518CB
		public static bool HasSource
		{
			get
			{
				return UserDataMigrationService.version != null;
			}
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x000536D8 File Offset: 0x000518D8
		public static void StartMigration()
		{
			if (UserDataMigrationService.profile != null && !UserDataMigrationService.handlerAdded)
			{
				AddinManager.AddExtensionNodeHandler("/MonoDevelop/Core/UserDataMigration", new ExtensionNodeEventHandler(UserDataMigrationService.HandleUserDataMigration));
				UserDataMigrationService.handlerAdded = true;
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x00053704 File Offset: 0x00051904
		private static int IndexOfVersion(UserDataMigrationNode node, string version)
		{
			for (int i = 0; i < UserProfile.ProfileVersions.Length; i++)
			{
				if (string.Equals(UserProfile.ProfileVersions[i], version))
				{
					return i;
				}
			}
			if (node != null)
			{
				LoggingService.LogWarning("Migration in addin '{0}' refers to unknown version '{1}'", new object[]
				{
					node.Addin.Id,
					version
				});
			}
			return -1;
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x0005375C File Offset: 0x0005195C
		private static bool CheckVersion(UserDataMigrationNode node, string version)
		{
			string sourceVersion = node.SourceVersion;
			if (string.Equals(sourceVersion, version))
			{
				return true;
			}
			if (sourceVersion[sourceVersion.Length - 1] == '+')
			{
				int num = UserDataMigrationService.IndexOfVersion(null, version);
				int num2 = UserDataMigrationService.IndexOfVersion(node, sourceVersion.Substring(0, sourceVersion.Length - 1));
				return num2 >= 0 && num >= num2;
			}
			if (sourceVersion.IndexOf('-') <= 0)
			{
				return false;
			}
			string[] array = sourceVersion.Split(new char[]
			{
				'-'
			});
			if (array.Length != 2)
			{
				throw new Exception("Invalid migration sourceVersion range in " + node.Addin.Id);
			}
			int num3 = UserDataMigrationService.IndexOfVersion(null, version);
			int num4 = UserDataMigrationService.IndexOfVersion(node, array[0]);
			int num5 = UserDataMigrationService.IndexOfVersion(node, array[1]);
			return num4 >= 0 && num5 >= 0 && num3 >= num4 && num3 >= num5;
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x00053838 File Offset: 0x00051A38
		private static void HandleUserDataMigration(object sender, ExtensionNodeEventArgs args)
		{
			if (args.Change != ExtensionChange.Add)
			{
				return;
			}
			UserDataMigrationNode userDataMigrationNode = (UserDataMigrationNode)args.ExtensionNode;
			if (!UserDataMigrationService.CheckVersion(userDataMigrationNode, UserDataMigrationService.version))
			{
				return;
			}
			FilePath filePath = FilePath.Null;
			FilePath filePath2 = FilePath.Null;
			try
			{
				filePath = UserDataMigrationService.profile.GetLocation(userDataMigrationNode.SourceKind).Combine(new FilePath[]
				{
					userDataMigrationNode.SourcePath
				});
				filePath2 = UserProfile.Current.GetLocation(userDataMigrationNode.TargetKind).Combine(new FilePath[]
				{
					userDataMigrationNode.TargetPath
				});
				bool flag = Directory.Exists(filePath);
				if (flag)
				{
					if (Directory.Exists(filePath2))
					{
						return;
					}
				}
				else if (File.Exists(filePath2) || Directory.Exists(filePath2) || !File.Exists(filePath))
				{
					return;
				}
				LoggingService.LogInfo("Migrating '{0}' to '{1}'", new object[]
				{
					filePath,
					filePath2
				});
				if (!flag)
				{
					Directory.CreateDirectory(filePath2.ParentDirectory);
				}
				IUserDataMigrationHandler handler = userDataMigrationNode.GetHandler();
				if (handler != null)
				{
					handler.Migrate(filePath, filePath2);
				}
				else if (flag)
				{
					UserDataMigrationService.DirectoryCopy(filePath, filePath2);
				}
				else
				{
					File.Copy(filePath, filePath2);
				}
			}
			catch (Exception ex)
			{
				string message = string.Format("{0}: Failed to migrate '{1}' to '{2}'", userDataMigrationNode.Addin.Id, filePath.ToString() ?? "", filePath2.ToString() ?? "");
				LoggingService.LogError(message, ex);
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00053A10 File Offset: 0x00051C10
		private static void DirectoryCopy(FilePath source, FilePath target)
		{
			string[] files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
			for (int i = 0; i < files.Length; i++)
			{
				FilePath filePath = files[i];
				string text = FileService.AbsoluteToRelativePath(source, filePath);
				FilePath filePath2 = target.Combine(new string[]
				{
					text
				});
				FilePath parentDirectory = filePath2.ParentDirectory;
				if (!Directory.Exists(parentDirectory))
				{
					Directory.CreateDirectory(parentDirectory);
				}
				File.Copy(filePath, filePath2);
			}
		}

		// Token: 0x04000609 RID: 1545
		private static UserProfile profile;

		// Token: 0x0400060A RID: 1546
		private static string version;

		// Token: 0x0400060B RID: 1547
		private static bool handlerAdded;
	}
}
