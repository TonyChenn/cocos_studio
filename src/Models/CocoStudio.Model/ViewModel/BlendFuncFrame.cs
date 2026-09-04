using System;
using CocoStudio.Model.ExtensionModel;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x020000BD RID: 189
	[FrameExtension(typeof(BlendFuncValue))]
	public class BlendFuncFrame : Frame
	{
		// Token: 0x06000600 RID: 1536 RVA: 0x00019414 File Offset: 0x00017614
		public BlendFuncFrame()
		{
			this.innerClass.SetTween(false);
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x00019438 File Offset: 0x00017638
		// (set) Token: 0x06000602 RID: 1538 RVA: 0x00019455 File Offset: 0x00017655
		public int Src
		{
			get
			{
				return (int)this._blendFuncValue.BlendSrc;
			}
			set
			{
				this._blendFuncValue = new BlendFuncValue((BlendSrc)value, this._blendFuncValue.BlendDst);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x00019470 File Offset: 0x00017670
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x0001948D File Offset: 0x0001768D
		public int Dst
		{
			get
			{
				return (int)this._blendFuncValue.BlendDst;
			}
			set
			{
				this._blendFuncValue = new BlendFuncValue(this._blendFuncValue.BlendSrc, (BlendDst)value);
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x000194A8 File Offset: 0x000176A8
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x000194C0 File Offset: 0x000176C0
		public BlendFuncValue Value
		{
			get
			{
				return this._blendFuncValue;
			}
			set
			{
				this._blendFuncValue = value;
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x000194CA File Offset: 0x000176CA
		protected override void OnUpdateProperty(AbstractNodeObject node)
		{
			this._blendFuncValue = (BlendFuncValue)this.PropertyHandler.GetValue(node, null);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x000194EA File Offset: 0x000176EA
		protected override void OnEnter(int nextFrameIndex, bool isChangeState)
		{
			this.PropertyHandler.SetValue(this.Node, this._blendFuncValue, null);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001950C File Offset: 0x0001770C
		protected override void SetValue(Frame frame)
		{
			base.SetValue(frame);
			BlendFuncFrame blendFuncFrame = frame as BlendFuncFrame;
			if (blendFuncFrame != null)
			{
				blendFuncFrame.Value = this._blendFuncValue;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x00019540 File Offset: 0x00017740
		// (set) Token: 0x0600060B RID: 1547 RVA: 0x00019553 File Offset: 0x00017753
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

		// Token: 0x040002BB RID: 699
		private BlendFuncValue _blendFuncValue = new BlendFuncValue();
	}
}
