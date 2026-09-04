using System;
using System.Collections.Generic;
using System.Linq;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200010B RID: 267
	public class InnerActionValue
	{
		// Token: 0x06000949 RID: 2377 RVA: 0x000253EB File Offset: 0x000235EB
		public InnerActionValue()
		{
			this.AnimationNames = new List<string>();
			this.SingleFrameIndex = 0;
			this.ActivedAnimationName = string.Empty;
			this.ActionType = InnerActionType.SingleFrame;
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0002541E File Offset: 0x0002361E
		public InnerActionValue(InnerActionType actionType, IEnumerable<string> animationNames, string activeAnimationName, int singleFrameIndex)
		{
			this.AnimationNames = animationNames;
			this.ActivedAnimationName = activeAnimationName;
			this.ActionType = actionType;
			this.SingleFrameIndex = singleFrameIndex;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0002544C File Offset: 0x0002364C
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

		// Token: 0x0600094C RID: 2380 RVA: 0x00025534 File Offset: 0x00023734
		public override int GetHashCode()
		{
			int num = this.ActionType.GetHashCode();
			num |= this.SingleFrameIndex.GetHashCode();
			num ^= this.ActivedAnimationName.GetHashCode();
			return num | this.AnimationNames.GetHashCode();
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00025588 File Offset: 0x00023788
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x0002559F File Offset: 0x0002379F
		public IEnumerable<string> AnimationNames { get; private set; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x000255A8 File Offset: 0x000237A8
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x000255BF File Offset: 0x000237BF
		public string ActivedAnimationName { get; private set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x000255C8 File Offset: 0x000237C8
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x000255DF File Offset: 0x000237DF
		public InnerActionType ActionType { get; private set; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x000255E8 File Offset: 0x000237E8
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x000255FF File Offset: 0x000237FF
		public int SingleFrameIndex { get; private set; }
	}
}
