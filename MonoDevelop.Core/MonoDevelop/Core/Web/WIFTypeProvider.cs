using System;
using System.ServiceModel;

namespace MonoDevelop.Core.Web
{
	// Token: 0x02000265 RID: 613
	internal abstract class WIFTypeProvider
	{
		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600163E RID: 5694
		public abstract Type ChannelFactory { get; }

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x0600163F RID: 5695
		public abstract Type RequestSecurityToken { get; }

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001640 RID: 5696
		public abstract Type EndPoint { get; }

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001641 RID: 5697
		public abstract Type RequestTypes { get; }

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06001642 RID: 5698
		public abstract Type KeyTypes { get; }

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001643 RID: 5699
		protected abstract string AssemblyName { get; }

		// Token: 0x06001644 RID: 5700 RVA: 0x0005A2D0 File Offset: 0x000584D0
		public static WIFTypeProvider GetWIFTypes()
		{
			WIFTypeProvider wiftypeProvider = new WIFTypeProvider.WIFTypes45();
			if (wiftypeProvider.ChannelFactory != null)
			{
				return wiftypeProvider;
			}
			wiftypeProvider = new WIFTypeProvider.WIFTypes40();
			if (wiftypeProvider.ChannelFactory != null)
			{
				return wiftypeProvider;
			}
			return null;
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x0005A30A File Offset: 0x0005850A
		protected string QualifyTypeName(string typeName)
		{
			return typeName + ',' + this.AssemblyName;
		}

		// Token: 0x02000266 RID: 614
		private sealed class WIFTypes40 : WIFTypeProvider
		{
			// Token: 0x170004BA RID: 1210
			// (get) Token: 0x06001647 RID: 5703 RVA: 0x0005A328 File Offset: 0x00058528
			public override Type ChannelFactory
			{
				get
				{
					string typeName = base.QualifyTypeName("Microsoft.IdentityModel.Protocols.WSTrust.WSTrustChannelFactory");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004BB RID: 1211
			// (get) Token: 0x06001648 RID: 5704 RVA: 0x0005A348 File Offset: 0x00058548
			public override Type RequestSecurityToken
			{
				get
				{
					string typeName = base.QualifyTypeName("Microsoft.IdentityModel.Protocols.WSTrust.RequestSecurityToken");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004BC RID: 1212
			// (get) Token: 0x06001649 RID: 5705 RVA: 0x0005A367 File Offset: 0x00058567
			public override Type EndPoint
			{
				get
				{
					return typeof(EndpointAddress);
				}
			}

			// Token: 0x170004BD RID: 1213
			// (get) Token: 0x0600164A RID: 5706 RVA: 0x0005A374 File Offset: 0x00058574
			public override Type RequestTypes
			{
				get
				{
					string typeName = base.QualifyTypeName("Microsoft.IdentityModel.SecurityTokenService.RequestTypes");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004BE RID: 1214
			// (get) Token: 0x0600164B RID: 5707 RVA: 0x0005A394 File Offset: 0x00058594
			public override Type KeyTypes
			{
				get
				{
					string typeName = base.QualifyTypeName("Microsoft.IdentityModel.SecurityTokenService.KeyTypes");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004BF RID: 1215
			// (get) Token: 0x0600164C RID: 5708 RVA: 0x0005A3B3 File Offset: 0x000585B3
			protected override string AssemblyName
			{
				get
				{
					return "Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35";
				}
			}
		}

		// Token: 0x02000267 RID: 615
		private sealed class WIFTypes45 : WIFTypeProvider
		{
			// Token: 0x170004C0 RID: 1216
			// (get) Token: 0x0600164E RID: 5710 RVA: 0x0005A3C2 File Offset: 0x000585C2
			public override Type ChannelFactory
			{
				get
				{
					return Type.GetType("System.ServiceModel.Security.WSTrustChannelFactory, System.ServiceModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
				}
			}

			// Token: 0x170004C1 RID: 1217
			// (get) Token: 0x0600164F RID: 5711 RVA: 0x0005A3D0 File Offset: 0x000585D0
			public override Type RequestSecurityToken
			{
				get
				{
					string typeName = base.QualifyTypeName("System.IdentityModel.Protocols.WSTrust.RequestSecurityToken");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004C2 RID: 1218
			// (get) Token: 0x06001650 RID: 5712 RVA: 0x0005A3F0 File Offset: 0x000585F0
			public override Type EndPoint
			{
				get
				{
					string typeName = base.QualifyTypeName("System.IdentityModel.Protocols.WSTrust.EndpointReference");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004C3 RID: 1219
			// (get) Token: 0x06001651 RID: 5713 RVA: 0x0005A410 File Offset: 0x00058610
			public override Type RequestTypes
			{
				get
				{
					string typeName = base.QualifyTypeName("System.IdentityModel.Protocols.WSTrust.RequestTypes");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004C4 RID: 1220
			// (get) Token: 0x06001652 RID: 5714 RVA: 0x0005A430 File Offset: 0x00058630
			public override Type KeyTypes
			{
				get
				{
					string typeName = base.QualifyTypeName("System.IdentityModel.Protocols.WSTrust.KeyTypes");
					return Type.GetType(typeName);
				}
			}

			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x06001653 RID: 5715 RVA: 0x0005A44F File Offset: 0x0005864F
			protected override string AssemblyName
			{
				get
				{
					return "System.IdentityModel, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
				}
			}
		}
	}
}
