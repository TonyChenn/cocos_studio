using System;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Core;
using MonoDevelop.Core.Instrumentation;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Ide.TypeSystem;
using MonoDevelop.Projects;

namespace MonoDevelop.Refactoring
{
	public static class ExtensionMethods
	{
		private class ResolverAnnotation
		{
			public CancellationTokenSource SharedTokenSource;

			public Task<CSharpAstResolver> Task;

			public CSharpUnresolvedFile ParsedFile;
		}

		public sealed class ConstantModeResolveVisitorNavigator : IResolveVisitorNavigator
		{
			private readonly ResolveVisitorNavigationMode mode;

			private readonly IResolveVisitorNavigator targetForResolveCalls;

			public ConstantModeResolveVisitorNavigator(ResolveVisitorNavigationMode mode, IResolveVisitorNavigator targetForResolveCalls)
			{
				this.mode = mode;
				this.targetForResolveCalls = targetForResolveCalls;
			}

			ResolveVisitorNavigationMode IResolveVisitorNavigator.Scan(AstNode node)
			{
				return mode;
			}

			void IResolveVisitorNavigator.Resolved(AstNode node, ResolveResult result)
			{
				if (targetForResolveCalls != null)
				{
					targetForResolveCalls.Resolved(node, result);
				}
			}

			void IResolveVisitorNavigator.ProcessConversion(Expression expression, ResolveResult result, Conversion conversion, IType targetType)
			{
				if (targetForResolveCalls != null)
				{
					targetForResolveCalls.ProcessConversion(expression, result, conversion, targetType);
				}
			}
		}

		public static TimerCounter ResolveCounter = InstrumentationService.CreateTimerCounter("Resolve document", "Parsing");

		public static Task<CSharpAstResolver> GetSharedResolver(this Document document)
		{
			ParsedDocument parsedDocument = document.ParsedDocument;
			if (parsedDocument == null || document.IsProjectContextInUpdate || (document.Project != null && !(document.Project is DotNetProject)))
			{
				return null;
			}
			SyntaxTree unit = parsedDocument.GetAst<SyntaxTree>();
			CSharpUnresolvedFile parsedFile = parsedDocument.ParsedFile as CSharpUnresolvedFile;
			if (unit == null || parsedFile == null)
			{
				return null;
			}
			ICompilation compilation = document.Compilation;
			ResolverAnnotation resolverAnnotation = document.Annotation<ResolverAnnotation>();
			if (resolverAnnotation != null)
			{
				if (resolverAnnotation.ParsedFile == parsedFile)
				{
					return resolverAnnotation.Task;
				}
				if (resolverAnnotation.SharedTokenSource != null)
				{
					resolverAnnotation.SharedTokenSource.Cancel();
				}
				document.RemoveAnnotations<ResolverAnnotation>();
			}
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
			CancellationToken token = cancellationTokenSource.Token;
			Task<CSharpAstResolver> task = Task.Factory.StartNew(delegate
			{
				try
				{
					using (ResolveCounter.BeginTiming())
					{
						CSharpAstResolver cSharpAstResolver = new CSharpAstResolver(compilation, unit, parsedFile);
						cSharpAstResolver.ApplyNavigator(new ConstantModeResolveVisitorNavigator(ResolveVisitorNavigationMode.Resolve, null), token);
						return cSharpAstResolver;
					}
				}
				catch (OperationCanceledException)
				{
					return (CSharpAstResolver)null;
				}
				catch (Exception ex2)
				{
					LoggingService.LogError("Error while creating the resolver.", ex2);
					return (CSharpAstResolver)null;
				}
			}, token);
			Task<CSharpAstResolver> task2 = task.ContinueWith(delegate(Task<CSharpAstResolver> t)
			{
				if (t.IsCanceled)
				{
					return (CSharpAstResolver)null;
				}
				if (t.IsFaulted)
				{
					Exception innerException = t.Exception.Flatten().InnerException;
					if (!(innerException is TaskCanceledException))
					{
						LoggingService.LogWarning("Exception while getting shared AST resolver.", innerException);
					}
					return (CSharpAstResolver)null;
				}
				return t.Result;
			}, TaskContinuationOptions.ExecuteSynchronously);
			document.AddAnnotation(new ResolverAnnotation
			{
				Task = task2,
				ParsedFile = parsedFile,
				SharedTokenSource = cancellationTokenSource
			});
			return task2;
		}
	}
}
