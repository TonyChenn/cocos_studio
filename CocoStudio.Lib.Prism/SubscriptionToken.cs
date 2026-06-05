using System;

namespace CocoStudio.Lib.Prism
{
	// Token: 0x02000015 RID: 21
	public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002CDE File Offset: 0x00000EDE
		public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
		{
			this._unsubscribeAction = unsubscribeAction;
			this._token = Guid.NewGuid();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002CFC File Offset: 0x00000EFC
		public bool Equals(SubscriptionToken other)
		{
			return other != null && object.Equals(this._token, other._token);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002D38 File Offset: 0x00000F38
		public override bool Equals(object obj)
		{
			return object.ReferenceEquals(this, obj) || this.Equals(obj as SubscriptionToken);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002D68 File Offset: 0x00000F68
		public override int GetHashCode()
		{
			return this._token.GetHashCode();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002D90 File Offset: 0x00000F90
		public virtual void Dispose()
		{
			if (this._unsubscribeAction != null)
			{
				this._unsubscribeAction(this);
				this._unsubscribeAction = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400001A RID: 26
		private readonly Guid _token;

		// Token: 0x0400001B RID: 27
		private Action<SubscriptionToken> _unsubscribeAction;
	}
}
