using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Projects;
using MonoDevelop.Core;

namespace CocosStudio.ExternalImport.StudioPlugin
{
	public sealed class ExternalCsdImportRequest
	{
		public string ProjectFile { get; set; }

		public string CsdFile { get; set; }

		public string ResourceFolder { get; set; }

		public string TargetDirectory { get; set; }

		public bool Overwrite { get; set; }

		public bool OpenAfterImport { get; set; }
	}

	public sealed class ExternalCsdImportResult
	{
		public ExternalCsdImportResult()
		{
			this.Code = string.Empty;
			this.Message = string.Empty;
			this.ImportedCsd = string.Empty;
			this.ImportedResourceFolder = string.Empty;
			this.OpenedCsd = string.Empty;
			this.Warnings = new List<string>();
		}

		public bool Success { get; set; }

		public string Code { get; set; }

		public string Message { get; set; }

		public string ImportedCsd { get; set; }

		public string ImportedResourceFolder { get; set; }

		public string OpenedCsd { get; set; }

		public List<string> Warnings { get; private set; }
	}

	public sealed class ExternalCsdImportService
	{
		private ExternalCsdImportService()
		{
		}

		public static ExternalCsdImportService Instance
		{
			get
			{
				return instance;
			}
		}

		public ExternalCsdImportResult Import(ExternalCsdImportRequest request)
		{
			ImportContext context;
			ExternalCsdImportResult validationResult = this.ValidateRequest(request, out context);
			if (validationResult != null)
			{
				return validationResult;
			}

			string stagingDirectory = this.CreateStagingDirectory();
			List<FileCommit> commits = new List<FileCommit>();
			List<string> createdDirectories = new List<string>();
			RegistrationState registrationState = new RegistrationState();
			string failureCode = "COPY_FAILED";
			try
			{
				this.StageFiles(context.Files, stagingDirectory);
				this.CommitDirectories(context.Directories, createdDirectories);
				this.CommitFiles(context.Files, stagingDirectory, commits, createdDirectories);

				failureCode = "REGISTER_FAILED";
				List<string> importedPaths = new List<string>();
				importedPaths.AddRange(context.Directories);
				importedPaths.AddRange(context.Files.Select((ImportFileMapping file) => file.TargetPath));
				this.RegisterPaths(importedPaths, registrationState);

				ExternalCsdImportResult result = this.CreateSuccessResult(context);
				this.OpenImportedCsd(context.TargetCsd, request.OpenAfterImport, result);
				return result;
			}
			catch (Exception ex)
			{
				this.RollbackFiles(commits, createdDirectories);
				this.RollbackRegistration(registrationState);
				LogConfig.Logger.Error("External CSD import failed.", ex);
				return Fail(failureCode, ex.Message);
			}
			finally
			{
				this.DeleteStagingDirectory(stagingDirectory);
			}
		}

		public ExternalCsdImportResult RegisterExistingResources(IEnumerable<string> paths, string csdFile, bool openAfterImport)
		{
			if (Services.ProjectOperations.CurrentSelectedSolution == null || Services.ProjectOperations.CurrentResourceGroup == null)
			{
				return Fail("PROJECT_NOT_OPEN", "Cocos Studio has no open project.");
			}

			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			List<string> normalizedPaths = new List<string>();
			try
			{
				foreach (string path in paths ?? Enumerable.Empty<string>())
				{
					if (string.IsNullOrWhiteSpace(path))
					{
						continue;
					}
					string fullPath = Path.GetFullPath(path);
					if (!IsPathInsideOrEqual(fullPath, rootFolder.FullPath))
					{
						return Fail("INVALID_TARGET", "Generated resource is outside the current project: " + fullPath);
					}
					if (File.Exists(fullPath) || Directory.Exists(fullPath))
					{
						normalizedPaths.Add(fullPath);
					}
				}

				RegistrationState registrationState = new RegistrationState();
				try
				{
					this.RegisterPaths(normalizedPaths, registrationState);
				}
				catch
				{
					this.RollbackRegistration(registrationState);
					throw;
				}

				ExternalCsdImportResult result = new ExternalCsdImportResult
				{
					Success = true,
					Code = "OK",
					Message = "Generated resources registered."
				};
				if (!string.IsNullOrWhiteSpace(csdFile))
				{
					string fullCsdPath = Path.GetFullPath(csdFile);
					result.ImportedCsd = ToProjectRelativePath(fullCsdPath, rootFolder.FullPath);
					this.OpenImportedCsd(fullCsdPath, openAfterImport, result);
				}
				return result;
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Register generated resources failed.", ex);
				return Fail("REGISTER_FAILED", ex.Message);
			}
		}

		private ExternalCsdImportResult ValidateRequest(ExternalCsdImportRequest request, out ImportContext context)
		{
			context = null;
			if (request == null)
			{
				return Fail("INVALID_REQUEST", "Import request is null.");
			}
			if (Services.ProjectOperations.CurrentSelectedSolution == null || Services.ProjectOperations.CurrentResourceGroup == null)
			{
				return Fail("PROJECT_NOT_OPEN", "Cocos Studio has no open project.");
			}

			string currentProject = Path.GetFullPath(Services.ProjectOperations.CurrentSelectedSolution.FileName.ToString());
			if (!string.IsNullOrWhiteSpace(request.ProjectFile))
			{
				string requestedProject;
				try
				{
					requestedProject = Path.GetFullPath(request.ProjectFile);
				}
				catch (Exception ex)
				{
					return Fail("PROJECT_MISMATCH", ex.Message);
				}
				if (!PathEquals(currentProject, requestedProject))
				{
					return Fail("PROJECT_MISMATCH", "The request targets another Cocos Studio project.");
				}
			}

			if (string.IsNullOrWhiteSpace(request.CsdFile))
			{
				return Fail("INVALID_CSD", "The CSD file is required.");
			}
			if (string.IsNullOrWhiteSpace(request.ResourceFolder))
			{
				return Fail("INVALID_RESOURCE_FOLDER", "The resource folder is required.");
			}
			string sourceCsd;
			string sourceResourceFolder;
			try
			{
				sourceCsd = Path.GetFullPath(request.CsdFile ?? string.Empty);
				sourceResourceFolder = Path.GetFullPath(request.ResourceFolder ?? string.Empty);
			}
			catch (Exception ex)
			{
				return Fail("INVALID_REQUEST", ex.Message);
			}
			if (!File.Exists(sourceCsd) || !string.Equals(Path.GetExtension(sourceCsd), ".csd", StringComparison.OrdinalIgnoreCase))
			{
				return Fail("INVALID_CSD", "The CSD file does not exist or is not a .csd file.");
			}
			if (!Directory.Exists(sourceResourceFolder))
			{
				return Fail("INVALID_RESOURCE_FOLDER", "The resource folder does not exist.");
			}
			if (IsPathInsideOrEqual(sourceCsd, sourceResourceFolder))
			{
				return Fail("INVALID_CSD", "The CSD file must not be inside the resource folder.");
			}
			if (HasReparsePoint(sourceResourceFolder))
			{
				return Fail("INVALID_RESOURCE_FOLDER", "The resource folder must not be a symbolic link or reparse point.");
			}

			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			string rootPath = Path.GetFullPath(rootFolder.FullPath);
			string targetDirectory;
			ExternalCsdImportResult targetResult = ResolveTargetDirectory(rootPath, request.TargetDirectory, out targetDirectory);
			if (targetResult != null)
			{
				return targetResult;
			}
			if (HasReparsePointInPath(targetDirectory, rootPath))
			{
				return Fail("INVALID_TARGET", "The target directory must not pass through a symbolic link or reparse point.");
			}

			string targetCsd = Path.Combine(targetDirectory, Path.GetFileName(sourceCsd));
			string targetResourceFolder = Path.Combine(targetDirectory, new DirectoryInfo(sourceResourceFolder).Name);
			if (PathEquals(targetCsd, targetResourceFolder))
			{
				return Fail("INVALID_TARGET", "The CSD and resource folder resolve to the same target path.");
			}
			if (Directory.Exists(targetCsd) || File.Exists(targetResourceFolder))
			{
				return Fail("INVALID_TARGET", "A target file and directory have conflicting types.");
			}
			if (IsPathInsideOrEqual(targetResourceFolder, sourceResourceFolder))
			{
				return Fail("INVALID_TARGET", "The target resource folder must not be inside the source resource folder.");
			}
			if (!request.Overwrite && (File.Exists(targetCsd) || Directory.Exists(targetResourceFolder)))
			{
				return Fail("TARGET_CONFLICT", "The target CSD or resource folder already exists.");
			}
			if (request.Overwrite && this.IsDirtyDocument(targetCsd))
			{
				return Fail("DIRTY_DOCUMENT", "The target CSD has unsaved changes in Cocos Studio.");
			}

			try
			{
				context = this.BuildImportContext(rootPath, sourceCsd, sourceResourceFolder, targetDirectory, targetCsd, targetResourceFolder, request.Overwrite);
			}
			catch (ImportValidationException ex)
			{
				return Fail(ex.Code, ex.Message);
			}
			catch (UnauthorizedAccessException ex)
			{
				return Fail("INVALID_RESOURCE_FOLDER", ex.Message);
			}
			catch (IOException ex)
			{
				return Fail("INVALID_RESOURCE_FOLDER", ex.Message);
			}
			catch (Exception ex)
			{
				return Fail("INVALID_TARGET", ex.Message);
			}
			return null;
		}

		private ImportContext BuildImportContext(string rootPath, string sourceCsd, string sourceResourceFolder, string targetDirectory, string targetCsd, string targetResourceFolder, bool overwrite)
		{
			ImportContext context = new ImportContext
			{
				RootPath = rootPath,
				TargetCsd = targetCsd,
				TargetResourceFolder = targetResourceFolder
			};
			context.Files.Add(new ImportFileMapping(sourceCsd, targetCsd));
			context.Directories.Add(targetResourceFolder);

			Stack<string> pendingDirectories = new Stack<string>();
			pendingDirectories.Push(sourceResourceFolder);
			while (pendingDirectories.Count > 0)
			{
				string currentDirectory = pendingDirectories.Pop();
				foreach (string entry in Directory.GetFileSystemEntries(currentDirectory))
				{
					FileAttributes attributes = File.GetAttributes(entry);
					if ((attributes & FileAttributes.ReparsePoint) != 0)
					{
						throw new ImportValidationException("INVALID_RESOURCE_FOLDER", "Symbolic links and reparse points are not supported: " + entry);
					}
					if ((attributes & FileAttributes.Hidden) != 0)
					{
						continue;
					}
					string relativePath = GetRelativePath(sourceResourceFolder, entry);
					string targetPath = Path.Combine(targetResourceFolder, relativePath);
					if ((attributes & FileAttributes.Directory) != 0)
					{
						context.Directories.Add(targetPath);
						pendingDirectories.Push(entry);
					}
					else
					{
						if (string.Equals(Path.GetExtension(entry), ".ccs", StringComparison.OrdinalIgnoreCase))
						{
							throw new ImportValidationException("INVALID_RESOURCE_FOLDER", "A resource folder must not contain a .ccs project file: " + entry);
						}
						context.Files.Add(new ImportFileMapping(entry, targetPath));
					}
				}
			}

			HashSet<string> targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			foreach (ImportFileMapping mapping in context.Files)
			{
				if (!targets.Add(mapping.TargetPath))
				{
					throw new ImportValidationException("INVALID_TARGET", "Multiple files resolve to the same target: " + mapping.TargetPath);
				}
				if (!IsPathInsideOrEqual(mapping.TargetPath, rootPath))
				{
					throw new ImportValidationException("INVALID_TARGET", "Target path is outside the current project: " + mapping.TargetPath);
				}
				if (Directory.Exists(mapping.TargetPath))
				{
					throw new ImportValidationException("INVALID_TARGET", "A target file path is already a directory: " + mapping.TargetPath);
				}
				if (!overwrite && File.Exists(mapping.TargetPath))
				{
					throw new ImportValidationException("TARGET_CONFLICT", "Target file already exists: " + mapping.TargetPath);
				}
			}
			context.Directories = context.Directories.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(GetPathDepth).ToList();
			foreach (string directory in context.Directories)
			{
				if (Directory.Exists(directory) && HasReparsePoint(directory))
				{
					throw new ImportValidationException("INVALID_TARGET", "A target directory is a symbolic link or reparse point: " + directory);
				}
			}
			return context;
		}

		private void StageFiles(IList<ImportFileMapping> files, string stagingDirectory)
		{
			string filesDirectory = Path.Combine(stagingDirectory, "files");
			Directory.CreateDirectory(filesDirectory);
			for (int index = 0; index < files.Count; index++)
			{
				string stagedPath = Path.Combine(filesDirectory, index.ToString("D8"));
				File.Copy(files[index].SourcePath, stagedPath, true);
				files[index].StagedPath = stagedPath;
			}
		}

		private void CommitDirectories(IEnumerable<string> directories, IList<string> createdDirectories)
		{
			foreach (string directory in directories)
			{
				if (File.Exists(directory))
				{
					throw new IOException("A target directory path is already a file: " + directory);
				}
				this.EnsureDirectory(directory, createdDirectories);
			}
		}

		private void CommitFiles(IList<ImportFileMapping> files, string stagingDirectory, IList<FileCommit> commits, IList<string> createdDirectories)
		{
			string backupDirectory = Path.Combine(stagingDirectory, "backups");
			Directory.CreateDirectory(backupDirectory);
			for (int index = 0; index < files.Count; index++)
			{
				ImportFileMapping mapping = files[index];
				this.EnsureDirectory(Path.GetDirectoryName(mapping.TargetPath), createdDirectories);
				FileCommit commit = new FileCommit
				{
					TargetPath = mapping.TargetPath,
					WasExisting = File.Exists(mapping.TargetPath)
				};
				if (commit.WasExisting)
				{
					commit.BackupPath = Path.Combine(backupDirectory, index.ToString("D8"));
					File.Copy(mapping.TargetPath, commit.BackupPath, true);
				}
				commits.Add(commit);
				File.Copy(mapping.StagedPath, mapping.TargetPath, true);
				File.SetAttributes(mapping.TargetPath, FileAttributes.Normal);
				Services.ProjectOperations.MarkFileDirty(mapping.TargetPath);
			}
		}

		private void RegisterPaths(IEnumerable<string> paths, RegistrationState state)
		{
			ResourceFolder rootFolder = Services.ProjectOperations.CurrentResourceGroup.RootFolder;
			IProgressMonitor monitor = Services.ProgressMonitors.GetConsoleProgressMonitor(false, true);
			IEnumerable<string> orderedPaths = paths.Where((string path) => !string.IsNullOrWhiteSpace(path))
				.Select(Path.GetFullPath)
				.Distinct(StringComparer.OrdinalIgnoreCase)
				.OrderBy(GetPathDepth)
				.ThenBy((string path) => path, StringComparer.OrdinalIgnoreCase);
			foreach (string path in orderedPaths)
			{
				if (PathEquals(path, rootFolder.FullPath))
				{
					continue;
				}
				ResourceItem existingItem = Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(rootFolder, path);
				if (existingItem != null)
				{
					existingItem.Refresh();
					if (!state.RefreshedItems.Any((ResourceItem item) => PathEquals(item.FullPath, existingItem.FullPath)))
					{
						state.RefreshedItems.Add(existingItem);
					}
					continue;
				}

				ResourceItem importRoot;
				ResourceItem addedItem = AddResourceItem(rootFolder, path, monitor, out importRoot);
				if (addedItem == null)
				{
					throw new InvalidOperationException("Failed to register imported resource: " + path);
				}
				if (importRoot != null && !state.AddedItems.Any((ResourceItem item) => PathEquals(item.FullPath, importRoot.FullPath)))
				{
					state.AddedItems.Add(importRoot);
				}
			}

			Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
			if (state.AddedItems.Count > 0)
			{
				Services.EventsService.GetEvent<AddResourcesEvent>().Publish(new AddResourcesArgs(rootFolder, state.AddedItems, false));
			}
			else
			{
				rootFolder.Refresh();
			}
		}

		private static ResourceItem AddResourceItem(ResourceFolder parentResourceItem, FilePath itemFileName, IProgressMonitor monitor, out ResourceItem importRoot)
		{
			Stack<string> stack = CreateParentStack(itemFileName, parentResourceItem.BaseDirectory);
			ResourceItem resourceItem = null;
			ResourceFolder resourceFolder = parentResourceItem;
			importRoot = null;
			while (stack.Count > 0)
			{
				if (resourceFolder == null)
				{
					throw new InvalidOperationException("An imported resource path has a file where a folder is required: " + itemFileName);
				}
				FilePath filePath = resourceFolder.FullPath;
				string name = stack.Pop();
				ResourceItem existingItem = resourceFolder.Items.FirstOrDefault((ResourceItem item) => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
				if (existingItem == null)
				{
					resourceItem = Services.ProjectsService.ReadResourceItem(monitor, filePath.Combine(new string[]
					{
						name
					}));
					if (resourceItem is ICocosFile)
					{
						((ICocosFile)resourceItem).Initialize(monitor);
					}
					resourceFolder.Items.Add(resourceItem);
					resourceFolder = resourceItem as ResourceFolder;
					if (importRoot == null)
					{
						importRoot = resourceItem;
					}
				}
				else
				{
					if (PathEquals(existingItem.FullPath.ToString(), itemFileName.ToString()))
					{
						return existingItem;
					}
					resourceFolder = existingItem as ResourceFolder;
				}
				if (resourceItem != null && PathEquals(resourceItem.FullPath.ToString(), itemFileName.ToString()))
				{
					return resourceItem;
				}
			}
			return resourceItem;
		}

		private static Stack<string> CreateParentStack(FilePath importPath, FilePath parent)
		{
			Stack<string> stack = new Stack<string>();
			while (importPath != parent)
			{
				stack.Push(importPath.FileName);
				importPath = importPath.ParentDirectory;
			}
			return stack;
		}

		private void RollbackRegistration(RegistrationState state)
		{
			foreach (ResourceItem item in state.AddedItems.OrderByDescending((ResourceItem resourceItem) => GetPathDepth(resourceItem.FullPath)).ToList())
			{
				ResourceFolder parent = item.Parent as ResourceFolder;
				if (parent != null && parent.Items.Contains(item))
				{
					parent.Items.Remove(item);
				}
			}
			foreach (ResourceItem item in state.RefreshedItems)
			{
				try
				{
					item.Refresh();
				}
				catch (Exception ex)
				{
					LogConfig.Logger.Error("Refresh resource after rollback failed.", ex);
				}
			}
			try
			{
				if (Services.ProjectOperations.CurrentSelectedSolution != null)
				{
					Services.ProjectOperations.CurrentSelectedSolution.Save(Services.ProgressMonitors.Default);
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Save project after external import rollback failed.", ex);
			}
		}

		private void RollbackFiles(IList<FileCommit> commits, IList<string> createdDirectories)
		{
			for (int index = commits.Count - 1; index >= 0; index--)
			{
				FileCommit commit = commits[index];
				try
				{
					if (commit.WasExisting && File.Exists(commit.BackupPath))
					{
						File.Copy(commit.BackupPath, commit.TargetPath, true);
						Services.ProjectOperations.MarkFileDirty(commit.TargetPath);
					}
					else if (!commit.WasExisting && File.Exists(commit.TargetPath))
					{
						File.Delete(commit.TargetPath);
					}
				}
				catch (Exception ex)
				{
					LogConfig.Logger.Error("Rollback imported file failed: " + commit.TargetPath, ex);
				}
			}
			foreach (string directory in createdDirectories.OrderByDescending(GetPathDepth))
			{
				try
				{
					if (Directory.Exists(directory) && Directory.GetFileSystemEntries(directory).Length == 0)
					{
						Directory.Delete(directory);
					}
				}
				catch (Exception ex)
				{
					LogConfig.Logger.Error("Rollback imported directory failed: " + directory, ex);
				}
			}
		}

		private void OpenImportedCsd(string targetCsd, bool openAfterImport, ExternalCsdImportResult result)
		{
			if (!openAfterImport)
			{
				return;
			}
			try
			{
				ResourceFile resourceFile = Services.ProjectOperations.CurrentResourceGroup.FindResourceItem(targetCsd) as ResourceFile;
				if (resourceFile == null || Services.Workbench.OpenDocument(resourceFile) == null)
				{
					result.Warnings.Add("The CSD was imported but could not be opened: " + targetCsd);
					return;
				}
				result.OpenedCsd = ToProjectRelativePath(targetCsd, Services.ProjectOperations.CurrentResourceGroup.RootFolder.FullPath);
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Open imported CSD failed.", ex);
				result.Warnings.Add("The CSD was imported but could not be opened: " + targetCsd);
			}
		}

		private bool IsDirtyDocument(string filePath)
		{
			if (Services.Workbench == null)
			{
				return false;
			}
			foreach (DocumentExtend document in Services.Workbench.Documents)
			{
				if (document.File != null && PathEquals(document.File.FullPath, filePath) && document.IsDirty)
				{
					return true;
				}
			}
			return false;
		}

		private ExternalCsdImportResult CreateSuccessResult(ImportContext context)
		{
			return new ExternalCsdImportResult
			{
				Success = true,
				Code = "OK",
				Message = "Import completed.",
				ImportedCsd = ToProjectRelativePath(context.TargetCsd, context.RootPath),
				ImportedResourceFolder = ToProjectRelativePath(context.TargetResourceFolder, context.RootPath)
			};
		}

		private string CreateStagingDirectory()
		{
			string root = Path.Combine(Path.GetTempPath(), "CocosStudioExternalImport");
			Directory.CreateDirectory(root);
			string stagingDirectory = Path.Combine(root, Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(stagingDirectory);
			return stagingDirectory;
		}

		private void DeleteStagingDirectory(string stagingDirectory)
		{
			try
			{
				string stagingRoot = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "CocosStudioExternalImport"));
				if (Directory.Exists(stagingDirectory) && IsPathInsideOrEqual(stagingDirectory, stagingRoot) && !PathEquals(stagingDirectory, stagingRoot))
				{
					Directory.Delete(stagingDirectory, true);
				}
			}
			catch (Exception ex)
			{
				LogConfig.Logger.Error("Delete external import staging directory failed.", ex);
			}
		}

		private void EnsureDirectory(string directory, IList<string> createdDirectories)
		{
			if (Directory.Exists(directory))
			{
				return;
			}
			Stack<string> missingDirectories = new Stack<string>();
			string current = directory;
			while (!string.IsNullOrWhiteSpace(current) && !Directory.Exists(current))
			{
				missingDirectories.Push(current);
				current = Path.GetDirectoryName(current);
			}
			while (missingDirectories.Count > 0)
			{
				string missingDirectory = missingDirectories.Pop();
				Directory.CreateDirectory(missingDirectory);
				createdDirectories.Add(missingDirectory);
			}
		}

		private static ExternalCsdImportResult ResolveTargetDirectory(string rootPath, string relativeTarget, out string targetDirectory)
		{
			targetDirectory = null;
			if (string.IsNullOrWhiteSpace(relativeTarget))
			{
				return Fail("INVALID_TARGET", "The target directory is required.");
			}
			if (Path.IsPathRooted(relativeTarget))
			{
				return Fail("INVALID_TARGET", "The target directory must be relative to the project root.");
			}
			string[] segments = relativeTarget.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
			if (segments.Any((string segment) => segment == ".."))
			{
				return Fail("INVALID_TARGET", "The target directory must not contain '..'.");
			}
			try
			{
				targetDirectory = Path.GetFullPath(Path.Combine(rootPath, relativeTarget));
			}
			catch (Exception ex)
			{
				return Fail("INVALID_TARGET", ex.Message);
			}
			if (!IsPathInsideOrEqual(targetDirectory, rootPath))
			{
				return Fail("INVALID_TARGET", "The target directory is outside the current project.");
			}
			return null;
		}

		private static ExternalCsdImportResult Fail(string code, string message)
		{
			return new ExternalCsdImportResult
			{
				Success = false,
				Code = code,
				Message = message ?? string.Empty
			};
		}

		private static bool HasReparsePoint(string path)
		{
			return (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0;
		}

		private static bool HasReparsePointInPath(string path, string rootPath)
		{
			string current = Path.GetFullPath(path);
			string root = Path.GetFullPath(rootPath);
			while (IsPathInsideOrEqual(current, root))
			{
				if (Directory.Exists(current) && HasReparsePoint(current))
				{
					return true;
				}
				if (PathEquals(current, root))
				{
					break;
				}
				current = Path.GetDirectoryName(current);
				if (string.IsNullOrWhiteSpace(current))
				{
					break;
				}
			}
			return false;
		}

		private static bool PathEquals(string left, string right)
		{
			return string.Equals(Path.GetFullPath(left).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), Path.GetFullPath(right).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar), StringComparison.OrdinalIgnoreCase);
		}

		private static bool IsPathInsideOrEqual(string path, string parentPath)
		{
			string normalizedPath = Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			string normalizedParent = Path.GetFullPath(parentPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
			return string.Equals(normalizedPath, normalizedParent, StringComparison.OrdinalIgnoreCase) || normalizedPath.StartsWith(normalizedParent + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
		}

		private static string GetRelativePath(string parentPath, string fullPath)
		{
			string normalizedParent = Path.GetFullPath(parentPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
			Uri parentUri = new Uri(normalizedParent, UriKind.Absolute);
			Uri fullUri = new Uri(Path.GetFullPath(fullPath), UriKind.Absolute);
			return Uri.UnescapeDataString(parentUri.MakeRelativeUri(fullUri).ToString()).Replace('/', Path.DirectorySeparatorChar);
		}

		private static string ToProjectRelativePath(string fullPath, string rootPath)
		{
			return GetRelativePath(rootPath, fullPath).Replace(Path.DirectorySeparatorChar, '/');
		}

		private static int GetPathDepth(string path)
		{
			return Path.GetFullPath(path).Count((char character) => character == Path.DirectorySeparatorChar || character == Path.AltDirectorySeparatorChar);
		}

		private sealed class ImportContext
		{
			public ImportContext()
			{
				this.Files = new List<ImportFileMapping>();
				this.Directories = new List<string>();
			}

			public string RootPath;

			public string TargetCsd;

			public string TargetResourceFolder;

			public List<ImportFileMapping> Files;

			public List<string> Directories;
		}

		private sealed class ImportFileMapping
		{
			public ImportFileMapping(string sourcePath, string targetPath)
			{
				this.SourcePath = sourcePath;
				this.TargetPath = targetPath;
			}

			public string SourcePath;

			public string TargetPath;

			public string StagedPath;
		}

		private sealed class FileCommit
		{
			public string TargetPath;

			public string BackupPath;

			public bool WasExisting;
		}

		private sealed class RegistrationState
		{
			public RegistrationState()
			{
				this.AddedItems = new List<ResourceItem>();
				this.RefreshedItems = new List<ResourceItem>();
			}

			public List<ResourceItem> AddedItems;

			public List<ResourceItem> RefreshedItems;
		}

		private sealed class ImportValidationException : Exception
		{
			public ImportValidationException(string code, string message) : base(message)
			{
				this.Code = code;
			}

			public string Code { get; private set; }
		}

		private static readonly ExternalCsdImportService instance = new ExternalCsdImportService();
	}
}
