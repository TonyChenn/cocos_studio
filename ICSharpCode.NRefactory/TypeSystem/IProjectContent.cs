using System;
using System.Collections.Generic;

namespace ICSharpCode.NRefactory.TypeSystem
{
	/// <summary>
	/// Represents an assembly consisting of source code (parsed files).
	/// </summary>
	// Token: 0x020000EC RID: 236
	public interface IProjectContent : IUnresolvedAssembly, IAssemblyReference
	{
		/// <summary>
		/// Gets the path to the project file (e.g. .csproj).
		/// </summary>
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060008C9 RID: 2249
		string ProjectFileName { get; }

		/// <summary>
		/// Gets a parsed file by its file name.
		/// </summary>
		// Token: 0x060008CA RID: 2250
		IUnresolvedFile GetFile(string fileName);

		/// <summary>
		/// Gets the list of all files in the project content.
		/// </summary>
		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060008CB RID: 2251
		IEnumerable<IUnresolvedFile> Files { get; }

		/// <summary>
		/// Gets the referenced assemblies.
		/// </summary>
		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060008CC RID: 2252
		IEnumerable<IAssemblyReference> AssemblyReferences { get; }

		/// <summary>
		/// Gets the compiler settings object.
		/// The concrete type of the settings object depends on the programming language used to implement this project.
		/// </summary>
		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060008CD RID: 2253
		object CompilerSettings { get; }

		/// <summary>
		/// Creates a new <see cref="T:ICSharpCode.NRefactory.TypeSystem.ICompilation" /> that allows resolving within this project.
		/// </summary>
		/// <remarks>
		/// This method does not support <see cref="T:ICSharpCode.NRefactory.TypeSystem.ProjectReference" />s. When dealing with a solution
		/// containing multiple projects, consider using <see cref="M:ICSharpCode.NRefactory.TypeSystem.ISolutionSnapshot.GetCompilation(ICSharpCode.NRefactory.TypeSystem.IProjectContent)" /> instead.
		/// </remarks>
		// Token: 0x060008CE RID: 2254
		ICompilation CreateCompilation();

		/// <summary>
		/// Creates a new <see cref="T:ICSharpCode.NRefactory.TypeSystem.ICompilation" /> that allows resolving within this project.
		/// </summary>
		/// <param name="solutionSnapshot">The parent solution snapshot to use for the compilation.</param>
		/// <remarks>
		/// This method is intended to be called by ISolutionSnapshot implementations. Other code should
		/// call <see cref="M:ICSharpCode.NRefactory.TypeSystem.ISolutionSnapshot.GetCompilation(ICSharpCode.NRefactory.TypeSystem.IProjectContent)" /> instead.
		/// This method always creates a new compilation, even if the solution snapshot already contains
		/// one for this project.
		/// </remarks>
		// Token: 0x060008CF RID: 2255
		ICompilation CreateCompilation(ISolutionSnapshot solutionSnapshot);

		/// <summary>
		/// Changes the assembly name of this project content.
		/// </summary>
		// Token: 0x060008D0 RID: 2256
		IProjectContent SetAssemblyName(string newAssemblyName);

		/// <summary>
		/// Changes the project file name of this project content.
		/// </summary>
		// Token: 0x060008D1 RID: 2257
		IProjectContent SetProjectFileName(string newProjectFileName);

		/// <summary>
		/// Changes the path to the assembly location (the output path where the project compiles to).
		/// </summary>
		// Token: 0x060008D2 RID: 2258
		IProjectContent SetLocation(string newLocation);

		/// <summary>
		/// Add assembly references to this project content.
		/// </summary>
		// Token: 0x060008D3 RID: 2259
		IProjectContent AddAssemblyReferences(IEnumerable<IAssemblyReference> references);

		/// <summary>
		/// Add assembly references to this project content.
		/// </summary>
		// Token: 0x060008D4 RID: 2260
		IProjectContent AddAssemblyReferences(params IAssemblyReference[] references);

		/// <summary>
		/// Removes assembly references from this project content.
		/// </summary>
		// Token: 0x060008D5 RID: 2261
		IProjectContent RemoveAssemblyReferences(IEnumerable<IAssemblyReference> references);

		/// <summary>
		/// Removes assembly references from this project content.
		/// </summary>
		// Token: 0x060008D6 RID: 2262
		IProjectContent RemoveAssemblyReferences(params IAssemblyReference[] references);

		/// <summary>
		/// Adds the specified files to the project content.
		/// If a file with the same name already exists, updated the existing file.
		/// </summary>
		/// <remarks>
		/// You can create an unresolved file by calling <c>ToTypeSystem()</c> on a syntax tree.
		/// </remarks>
		// Token: 0x060008D7 RID: 2263
		IProjectContent AddOrUpdateFiles(IEnumerable<IUnresolvedFile> newFiles);

		/// <summary>
		/// Adds the specified files to the project content.
		/// If a file with the same name already exists, this method updates the existing file.
		/// </summary>
		/// <remarks>
		/// You can create an unresolved file by calling <c>ToTypeSystem()</c> on a syntax tree.
		/// </remarks>
		// Token: 0x060008D8 RID: 2264
		IProjectContent AddOrUpdateFiles(params IUnresolvedFile[] newFiles);

		/// <summary>
		/// Removes the files with the specified names.
		/// </summary>
		// Token: 0x060008D9 RID: 2265
		IProjectContent RemoveFiles(IEnumerable<string> fileNames);

		/// <summary>
		/// Removes the files with the specified names.
		/// </summary>
		// Token: 0x060008DA RID: 2266
		IProjectContent RemoveFiles(params string[] fileNames);

		/// <summary>
		/// Removes types and attributes from oldFile from the project, and adds those from newFile.
		/// </summary>
		// Token: 0x060008DB RID: 2267
		[Obsolete("Use RemoveFiles()/AddOrUpdateFiles() instead")]
		IProjectContent UpdateProjectContent(IUnresolvedFile oldFile, IUnresolvedFile newFile);

		/// <summary>
		/// Removes types and attributes from oldFiles from the project, and adds those from newFiles.
		/// </summary>
		// Token: 0x060008DC RID: 2268
		[Obsolete("Use RemoveFiles()/AddOrUpdateFiles() instead")]
		IProjectContent UpdateProjectContent(IEnumerable<IUnresolvedFile> oldFiles, IEnumerable<IUnresolvedFile> newFiles);

		/// <summary>
		/// Sets the compiler settings object.
		/// The concrete type of the settings object depends on the programming language used to implement this project.
		/// Using the incorrect type of settings object results in an <see cref="T:System.ArgumentException" />.
		/// </summary>
		// Token: 0x060008DD RID: 2269
		IProjectContent SetCompilerSettings(object compilerSettings);
	}
}
