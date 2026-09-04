using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000D6 RID: 214
	[FrameExtension(typeof(string))]
	public class StringFrame : Frame
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001AA24 File Offset: 0x00018C24
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x0001AA3B File Offset: 0x00018C3B
		public virtual string Value { get; set; }

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001AA44 File Offset: 0x00018C44
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this.Value = (string)this.PropertyHandler.GetValue(node, null);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0001AA65 File Offset: 0x00018C65
		public StringFrame()
		{
			this.innerClass.SetTween(false);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001AA7D File Offset: 0x00018C7D
		public StringFrame(string value) : this()
		{
			this.Value = value;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001AA90 File Offset: 0x00018C90
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this.Value, null);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001AAB4 File Offset: 0x00018CB4
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			StringFrame stringFrame = frame as StringFrame;
			if (stringFrame != null)
			{
				stringFrame.Value = this.Value;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001AAE8 File Offset: 0x00018CE8
		// (set) Token: 0x060006AE RID: 1710 RVA: 0x0001AAFB File Offset: 0x00018CFB
		public override bool Tween
		{
			get
			{
				return false;
			}
			set
			{
			}
		}
	}
}
