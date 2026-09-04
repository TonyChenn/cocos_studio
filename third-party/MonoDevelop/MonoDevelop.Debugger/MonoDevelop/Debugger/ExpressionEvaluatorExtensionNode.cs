using Mono.Addins;
using Mono.Debugging.Client;

namespace MonoDevelop.Debugger
{
	public class ExpressionEvaluatorExtensionNode : TypeExtensionNode
	{
		[NodeAttribute("name")]
		public string Name;

		[NodeAttribute("extension")]
		public string extension;

		private IExpressionEvaluator instance;

		public IExpressionEvaluator Evaluator
		{
			get
			{
				if (instance == null)
				{
					instance = (IExpressionEvaluator)CreateInstance(typeof(IExpressionEvaluator));
				}
				return instance;
			}
		}
	}
}
