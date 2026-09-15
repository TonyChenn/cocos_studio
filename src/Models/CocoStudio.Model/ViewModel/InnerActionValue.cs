using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Model.ViewModel
{
	public class InnerActionValue
	{
		public InnerActionValue()
		{
			this.AnimationNames = new List<string>();
			this.SingleFrameIndex = 0;
			this.ActivedAnimationName = string.Empty;
			this.ActionType = InnerActionType.SingleFrame;
		}

		public InnerActionValue(InnerActionType actionType, IEnumerable<string> animationNames, string activeAnimationName, int singleFrameIndex)
		{
			this.AnimationNames = animationNames;
			this.ActivedAnimationName = activeAnimationName;
			this.ActionType = actionType;
			this.SingleFrameIndex = singleFrameIndex;
		}

		public override bool Equals(object obj)
		{
			InnerActionValue innerActionValue = obj as InnerActionValue;
			bool flag = innerActionValue != null;
			flag &= (this.ActionType == innerActionValue.ActionType && this.SingleFrameIndex == innerActionValue.SingleFrameIndex && this.ActivedAnimationName == innerActionValue.ActivedAnimationName);
			flag &= (this.AnimationNames != null && innerActionValue.AnimationNames != null);
			if (flag)
			{
				int num = this.AnimationNames.Count<string>();
				flag = (num == innerActionValue.AnimationNames.Count<string>());
				if (flag)
				{
					for (int i = 0; i < num; i++)
					{
						if (this.AnimationNames.ElementAt(i) != innerActionValue.AnimationNames.ElementAt(i))
						{
							flag = false;
							break;
						}
					}
				}
			}
			return flag;
		}

		public override int GetHashCode()
		{
			int num = this.ActionType.GetHashCode();
			num |= this.SingleFrameIndex.GetHashCode();
			num ^= this.ActivedAnimationName.GetHashCode();
			return num | this.AnimationNames.GetHashCode();
		}

		public IEnumerable<string> AnimationNames { get; private set; }

		public string ActivedAnimationName { get; private set; }

		public InnerActionType ActionType { get; private set; }

		public int SingleFrameIndex { get; private set; }
	}
}
