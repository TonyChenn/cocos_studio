using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Microsoft.Build.BuildEngine
{
	// Token: 0x020001DE RID: 478
	internal sealed class ConditionFunctionExpression : ConditionExpression
	{
		// Token: 0x06001222 RID: 4642 RVA: 0x00049B28 File Offset: 0x00047D28
		static ConditionFunctionExpression()
		{
			Type typeFromHandle = typeof(ConditionFunctionExpression);
			string[] array = new string[]
			{
				"Exists"
			};
			ConditionFunctionExpression.functions = new Dictionary<string, MethodInfo>();
			foreach (string key in array)
			{
				ConditionFunctionExpression.functions.Add(key, typeFromHandle.GetMethod(key, BindingFlags.Static | BindingFlags.NonPublic));
			}
		}

		// Token: 0x06001223 RID: 4643 RVA: 0x00049B8B File Offset: 0x00047D8B
		public ConditionFunctionExpression(string name, List<ConditionFactorExpression> args)
		{
			this.args = args;
			this.name = name;
		}

		// Token: 0x06001224 RID: 4644 RVA: 0x00049BA4 File Offset: 0x00047DA4
		public override bool BoolEvaluate(IExpressionContext context)
		{
			if (!ConditionFunctionExpression.functions.ContainsKey(this.name))
			{
				throw new InvalidOperationException();
			}
			if (ConditionFunctionExpression.functions[this.name] == null)
			{
				throw new InvalidOperationException();
			}
			MethodInfo methodInfo = ConditionFunctionExpression.functions[this.name];
			object[] array = new object[this.args.Count + 1];
			int num = 0;
			foreach (ConditionFactorExpression conditionFactorExpression in this.args)
			{
				array[num++] = conditionFactorExpression.StringEvaluate(context);
			}
			array[num] = context;
			return (bool)methodInfo.Invoke(null, array);
		}

		// Token: 0x06001225 RID: 4645 RVA: 0x00049C6C File Offset: 0x00047E6C
		public override float NumberEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001226 RID: 4646 RVA: 0x00049C73 File Offset: 0x00047E73
		public override string StringEvaluate(IExpressionContext context)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001227 RID: 4647 RVA: 0x00049C7A File Offset: 0x00047E7A
		public override bool CanEvaluateToBool(IExpressionContext context)
		{
			return ConditionFunctionExpression.functions.ContainsKey(this.name);
		}

		// Token: 0x06001228 RID: 4648 RVA: 0x00049C8C File Offset: 0x00047E8C
		public override bool CanEvaluateToNumber(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x06001229 RID: 4649 RVA: 0x00049C8F File Offset: 0x00047E8F
		public override bool CanEvaluateToString(IExpressionContext context)
		{
			return false;
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00049C94 File Offset: 0x00047E94
		private static bool Exists(string file, IExpressionContext context)
		{
			string text = null;
			if (context.FullFileName != string.Empty)
			{
				text = Path.GetDirectoryName(context.FullFileName);
			}
			if (!Path.IsPathRooted(file) && text != null && text != string.Empty)
			{
				file = Path.Combine(text, file);
			}
			return File.Exists(file);
		}

		// Token: 0x04000535 RID: 1333
		private List<ConditionFactorExpression> args;

		// Token: 0x04000536 RID: 1334
		private string name;

		// Token: 0x04000537 RID: 1335
		private static Dictionary<string, MethodInfo> functions;
	}
}
