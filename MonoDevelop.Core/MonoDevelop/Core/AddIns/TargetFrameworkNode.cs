using System;
using System.IO;
using System.Xml;
using Mono.Addins;
using MonoDevelop.Core.Assemblies;
using MonoDevelop.Core.Serialization;

namespace MonoDevelop.Core.AddIns
{
	// Token: 0x020000BB RID: 187
	internal class TargetFrameworkNode : ExtensionNode
	{
		// Token: 0x0600066E RID: 1646 RVA: 0x00018A28 File Offset: 0x00016C28
		public TargetFramework CreateFramework()
		{
			Stream stream;
			if (this.resource != null)
			{
				stream = base.Addin.GetResource(this.resource);
			}
			else
			{
				if (this.file == null)
				{
					throw new InvalidOperationException("Framework xml source not specified");
				}
				stream = File.OpenRead(base.Addin.GetFilePath(this.file));
			}
			TargetFramework result;
			using (stream)
			{
				XmlTextReader reader = new XmlTextReader(stream);
				XmlDataSerializer xmlDataSerializer = new XmlDataSerializer(new DataContext());
				TargetFramework targetFramework = (TargetFramework)xmlDataSerializer.Deserialize(reader, typeof(TargetFramework));
				targetFramework.FrameworkNode = this;
				result = targetFramework;
			}
			return result;
		}

		// Token: 0x04000226 RID: 550
		[NodeAttribute]
		protected string resource;

		// Token: 0x04000227 RID: 551
		[NodeAttribute]
		protected string file;
	}
}
