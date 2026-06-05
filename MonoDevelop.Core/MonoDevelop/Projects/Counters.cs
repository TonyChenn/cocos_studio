using System;
using MonoDevelop.Core.Instrumentation;

namespace MonoDevelop.Projects
{
	// Token: 0x02000100 RID: 256
	internal static class Counters
	{
		// Token: 0x040002E2 RID: 738
		public static Counter ItemsInMemory = InstrumentationService.CreateCounter("Projects in memory", "Project Model");

		// Token: 0x040002E3 RID: 739
		public static Counter ItemsLoaded = InstrumentationService.CreateCounter("Projects loaded", "Project Model");

		// Token: 0x040002E4 RID: 740
		public static Counter SolutionsInMemory = InstrumentationService.CreateCounter("Solutions in memory", "Project Model");

		// Token: 0x040002E5 RID: 741
		public static Counter SolutionsLoaded = InstrumentationService.CreateCounter("Solutions loaded", "Project Model");

		// Token: 0x040002E6 RID: 742
		public static TimerCounter ReadWorkspaceItem = InstrumentationService.CreateTimerCounter("Workspace item read", "Project Model", 0.0, false, "Projects.WorkspaceItemRead");

		// Token: 0x040002E7 RID: 743
		public static TimerCounter ReadSolutionItem = InstrumentationService.CreateTimerCounter("Solution item read", "Project Model", 0.0, false, "Projects.SolutionItemRead");

		// Token: 0x040002E8 RID: 744
		public static TimerCounter ReadMSBuildProject = InstrumentationService.CreateTimerCounter("MSBuild project read", "Project Model", 0.0, false, "Projects.MSBuildProjectRead");

		// Token: 0x040002E9 RID: 745
		public static TimerCounter WriteMSBuildProject = InstrumentationService.CreateTimerCounter("MSBuild project written", "Project Model", 0.0, false, "Projects.MSBuildProjectWritten");

		// Token: 0x040002EA RID: 746
		public static TimerCounter BuildSolutionTimer = InstrumentationService.CreateTimerCounter("Build solution", "Project Model", 0.0, false, "Projects.BuildSolution");

		// Token: 0x040002EB RID: 747
		public static TimerCounter BuildProjectAndReferencesTimer = InstrumentationService.CreateTimerCounter("Build project and references", "Project Model", 0.0, false, "Projects.BuildProjectAndReferences");

		// Token: 0x040002EC RID: 748
		public static TimerCounter BuildProjectTimer = InstrumentationService.CreateTimerCounter("Build project", "Project Model", 0.0, false, "Projects.BuildProject");

		// Token: 0x040002ED RID: 749
		public static TimerCounter CleanProjectTimer = InstrumentationService.CreateTimerCounter("Clean project", "Project Model", 0.0, false, "Projects.CleanProject");

		// Token: 0x040002EE RID: 750
		public static TimerCounter BuildWorkspaceItemTimer = InstrumentationService.CreateTimerCounter("Build workspace item", "Project Model");

		// Token: 0x040002EF RID: 751
		public static TimerCounter NeedsBuildingTimer = InstrumentationService.CreateTimerCounter("Check needs building", "Project Model");

		// Token: 0x040002F0 RID: 752
		public static TimerCounter BuildMSBuildProjectTimer = InstrumentationService.CreateTimerCounter("Build MSBuild project", "Project Model", 0.0, false, "Projects.BuildMSBuildProject");

		// Token: 0x040002F1 RID: 753
		public static TimerCounter CleanMSBuildProjectTimer = InstrumentationService.CreateTimerCounter("Clean MSBuild project", "Project Model", 0.0, false, "Projects.CleanMSBuildProject");

		// Token: 0x040002F2 RID: 754
		public static TimerCounter RunMSBuildTargetTimer = InstrumentationService.CreateTimerCounter("Run MSBuild target", "Project Model", 0.0, false, "Projects.RunMSBuildTarget");

		// Token: 0x040002F3 RID: 755
		public static TimerCounter ResolveMSBuildReferencesTimer = InstrumentationService.CreateTimerCounter("Resolve MSBuild references", "Project Model", 0.0, false, "Projects.ResolveMSBuildReferences");

		// Token: 0x040002F4 RID: 756
		public static TimerCounter HelpServiceInitialization = InstrumentationService.CreateTimerCounter("Help Service initialization", "IDE");

		// Token: 0x040002F5 RID: 757
		public static TimerCounter ParserServiceInitialization = InstrumentationService.CreateTimerCounter("Parser Service initialization", "IDE");
	}
}
