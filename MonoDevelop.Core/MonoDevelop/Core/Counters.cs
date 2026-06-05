using System;
using MonoDevelop.Core.Instrumentation;

namespace MonoDevelop.Core
{
	// Token: 0x02000006 RID: 6
	internal static class Counters
	{
		// Token: 0x0400001C RID: 28
		public static TimerCounter RuntimeInitialization = InstrumentationService.CreateTimerCounter("Runtime initialization", "Runtime", 0.0, false, "Core.RuntimeInitialization");

		// Token: 0x0400001D RID: 29
		public static TimerCounter PropertyServiceInitialization = InstrumentationService.CreateTimerCounter("Property Service initialization", "Runtime");

		// Token: 0x0400001E RID: 30
		public static Counter AddinsLoaded = InstrumentationService.CreateCounter("Add-ins loaded", "Add-in Engine", true, "Core.AddinsLoaded");

		// Token: 0x0400001F RID: 31
		public static Counter ProcessesStarted = InstrumentationService.CreateCounter("Processes started", "Process Service");

		// Token: 0x04000020 RID: 32
		public static Counter ExternalObjects = InstrumentationService.CreateCounter("External objects", "Process Service");

		// Token: 0x04000021 RID: 33
		public static Counter ExternalHostProcesses = InstrumentationService.CreateCounter("External processes hosting objects", "Process Service");

		// Token: 0x04000022 RID: 34
		public static TimerCounter TargetRuntimesLoading = InstrumentationService.CreateTimerCounter("Target runtimes loaded", "Assembly Service", 0.0, true);

		// Token: 0x04000023 RID: 35
		public static Counter PcFilesParsed = InstrumentationService.CreateCounter(".pc Files parsed", "Assembly Service");

		// Token: 0x04000024 RID: 36
		public static Counter FileChangeNotifications = InstrumentationService.CreateCounter("File change notifications", "File Service");

		// Token: 0x04000025 RID: 37
		public static Counter FilesRemoved = InstrumentationService.CreateCounter("Files removed", "File Service");

		// Token: 0x04000026 RID: 38
		public static Counter FilesCreated = InstrumentationService.CreateCounter("Files created", "File Service");

		// Token: 0x04000027 RID: 39
		public static Counter FilesRenamed = InstrumentationService.CreateCounter("Files renamed", "File Service");

		// Token: 0x04000028 RID: 40
		public static Counter DirectoriesRemoved = InstrumentationService.CreateCounter("Directories removed", "File Service");

		// Token: 0x04000029 RID: 41
		public static Counter DirectoriesCreated = InstrumentationService.CreateCounter("Directories created", "File Service");

		// Token: 0x0400002A RID: 42
		public static Counter DirectoriesRenamed = InstrumentationService.CreateCounter("Directories renamed", "File Service");

		// Token: 0x0400002B RID: 43
		public static Counter LogErrors = InstrumentationService.CreateCounter("Errors", "Log");

		// Token: 0x0400002C RID: 44
		public static Counter LogWarnings = InstrumentationService.CreateCounter("Warnings", "Log");

		// Token: 0x0400002D RID: 45
		public static Counter LogMessages = InstrumentationService.CreateCounter("Information messages", "Log");

		// Token: 0x0400002E RID: 46
		public static Counter LogFatalErrors = InstrumentationService.CreateCounter("Fatal errors", "Log");

		// Token: 0x0400002F RID: 47
		public static Counter LogDebug = InstrumentationService.CreateCounter("Debug messages", "Log");
	}
}
