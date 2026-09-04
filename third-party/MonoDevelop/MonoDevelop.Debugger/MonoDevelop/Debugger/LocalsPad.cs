using System;
using System.Collections.Generic;
using Mono.Debugging.Client;

namespace MonoDevelop.Debugger
{
	public class LocalsPad : ObjectValuePad
	{
		private Dictionary<string, ObjectValue> lastLookup = new Dictionary<string, ObjectValue>();

		private StackFrame lastFrame;

		public LocalsPad()
		{
			tree.AllowEditing = true;
			tree.AllowAdding = false;
		}

		public override void OnUpdateList()
		{
			base.OnUpdateList();
			StackFrame currentFrame = DebuggingService.CurrentFrame;
			if (currentFrame == null || !FrameEquals(currentFrame, lastFrame))
			{
				tree.ClearExpressions();
				lastLookup = null;
			}
			lastFrame = currentFrame;
			if (currentFrame == null)
			{
				return;
			}
			ObjectValue[] allLocals = currentFrame.GetAllLocals();
			Dictionary<string, ObjectValue> dictionary = new Dictionary<string, ObjectValue>(allLocals.Length);
			ObjectValue[] array = allLocals;
			foreach (ObjectValue objectValue in array)
			{
				string name = objectValue.Name;
				if (string.IsNullOrWhiteSpace(name) || name == "?" || dictionary.ContainsKey(name))
				{
					continue;
				}
				dictionary.Add(name, objectValue);
				if (lastLookup != null)
				{
					if (lastLookup.TryGetValue(name, out var value))
					{
						tree.ReplaceValue(value, objectValue);
					}
					else
					{
						tree.AddValue(objectValue);
					}
				}
			}
			if (lastLookup != null)
			{
				foreach (KeyValuePair<string, ObjectValue> item in lastLookup)
				{
					if (!dictionary.ContainsKey(item.Key))
					{
						tree.RemoveValue(item.Value);
					}
				}
			}
			else
			{
				tree.ClearValues();
				tree.AddValues(dictionary.Values);
			}
			lastLookup = dictionary;
		}

		private static bool FrameEquals(StackFrame a, StackFrame z)
		{
			if (a == null || z == null)
			{
				return a == z;
			}
			if (a.SourceLocation == null || z.SourceLocation == null)
			{
				return a.SourceLocation == z.SourceLocation;
			}
			if (a.SourceLocation.FileName == null)
			{
				if (z.SourceLocation.FileName != null)
				{
					return false;
				}
			}
			else if (!a.SourceLocation.FileName.Equals(z.SourceLocation.FileName, StringComparison.Ordinal))
			{
				return false;
			}
			if (a.SourceLocation.MethodName == null)
			{
				if (z.SourceLocation.MethodName != null)
				{
					return false;
				}
				return true;
			}
			return a.SourceLocation.MethodName.Equals(z.SourceLocation.MethodName, StringComparison.Ordinal);
		}
	}
}
