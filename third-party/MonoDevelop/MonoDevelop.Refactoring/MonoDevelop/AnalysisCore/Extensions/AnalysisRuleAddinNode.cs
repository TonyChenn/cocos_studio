using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using Mono.Addins;

namespace MonoDevelop.AnalysisCore.Extensions
{
	internal abstract class AnalysisRuleAddinNode : ExtensionNode
	{
		[NodeAttribute(Description = "Comma separated list of file extensions to which this rule applies. It applies to all if none is specified.")]
		private string[] fileExtensions;

		[NodeAttribute(Required = true, Description = "The ID of the input type")]
		private string input;

		[NodeAttribute("func", Required = true, Description = "The static Func<T,CancellationToken,T> that processes the rule.")]
		private string funcName;

		private Func<object, CancellationToken, object> cachedInstance;

		private static int dynamicMethodKey;

		public string[] FileExtensions => fileExtensions;

		public string Input => input;

		public abstract string Output { get; }

		public string FuncName => funcName;

		public Func<object, CancellationToken, object> Analyze
		{
			get
			{
				if (cachedInstance == null)
				{
					CreateFunc();
				}
				return cachedInstance;
			}
		}

		private void CreateFunc()
		{
			Func<object, CancellationToken, object> func = null;
			try
			{
				if (string.IsNullOrEmpty(funcName))
				{
					throw new InvalidOperationException("Rule extension does not specify a func " + GetErrSource());
				}
				int num = funcName.LastIndexOf('.');
				if (num <= 0)
				{
					throw new InvalidOperationException("Rule func name ' " + funcName + " 'is invalid " + GetErrSource());
				}
				string typeName = funcName.Substring(0, num);
				Type type = base.Addin.GetType(typeName, throwIfNotFound: true);
				Type type2 = AnalysisExtensions.GetType(Input);
				Type type3 = AnalysisExtensions.GetType(Output);
				string text = funcName.Substring(num + 1);
				MethodInfo method = type.GetMethod(text, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, Type.DefaultBinder, new Type[2]
				{
					type2,
					typeof(CancellationToken)
				}, new ParameterModifier[1] { default(ParameterModifier) });
				if (method == null)
				{
					throw new InvalidOperationException("Rule func ' " + funcName + "' could not be resolved " + GetErrSource());
				}
				if (method.ReturnType != type3)
				{
					throw new InvalidOperationException("Rule func ' " + funcName + "' has wrong output type " + GetErrSource());
				}
				DynamicMethod dynamicMethod = new DynamicMethod(text + "_obj" + dynamicMethodKey++, typeof(object), new Type[2]
				{
					typeof(object),
					typeof(CancellationToken)
				}, restrictedSkipVisibility: true);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Castclass, type2);
				iLGenerator.Emit(OpCodes.Ldarg_1);
				iLGenerator.Emit((method.IsFinal || !method.IsVirtual) ? OpCodes.Call : OpCodes.Callvirt, method);
				iLGenerator.Emit(OpCodes.Ret);
				func = (Func<object, CancellationToken, object>)dynamicMethod.CreateDelegate(typeof(Func<object, CancellationToken, object>));
			}
			finally
			{
				cachedInstance = func ?? new Func<object, CancellationToken, object>(NullRule);
			}
		}

		internal string GetErrSource()
		{
			return $"({base.Addin.Id}:{base.Path})";
		}

		private static object NullRule(object o, CancellationToken cancellationToken)
		{
			return null;
		}

		public bool Supports(string extension)
		{
			if (fileExtensions != null && fileExtensions.Length > 0)
			{
				return fileExtensions.Any((string fe) => string.Compare(fe, extension, StringComparison.OrdinalIgnoreCase) == 0);
			}
			return true;
		}
	}
}
