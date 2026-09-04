using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model
{
	// Token: 0x02000134 RID: 308
	public class ModelMetaData
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000B5D RID: 2909 RVA: 0x0002D0EC File Offset: 0x0002B2EC
		// (set) Token: 0x06000B5E RID: 2910 RVA: 0x0002D103 File Offset: 0x0002B303
		public virtual Type Type { get; protected set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x0002D10C File Offset: 0x0002B30C
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x0002D123 File Offset: 0x0002B323
		public string DisplayName { get; protected set; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0002D12C File Offset: 0x0002B32C
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x0002D143 File Offset: 0x0002B343
		internal bool IsDefault { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0002D14C File Offset: 0x0002B34C
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x0002D163 File Offset: 0x0002B363
		public EnumModelType ModelType { get; protected set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x0002D16C File Offset: 0x0002B36C
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x0002D183 File Offset: 0x0002B383
		public ScriptFileData ScriptData { get; protected set; }

		// Token: 0x06000B67 RID: 2919 RVA: 0x0002D18C File Offset: 0x0002B38C
		public ModelMetaData(ModelExtensionNode extensionNode)
		{
			this.Type = extensionNode.Type;
			this.IsDefault = extensionNode.Data.IsDefault;
			this.ModelType = extensionNode.Data.ModelType;
			DisplayNameAttribute[] array = this.Type.GetCustomAttributes(typeof(DisplayNameAttribute), true) as DisplayNameAttribute[];
			if (array.Length > 0)
			{
				this.DisplayName = array[0].DisplayName;
			}
			else
			{
				this.DisplayName = this.Type.Name;
			}
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x0002D21F File Offset: 0x0002B41F
		protected ModelMetaData(Type type, ScriptFileData scriptFile, string displayName = null, EnumModelType modelType = EnumModelType.TwoDimension)
		{
			this.Type = type;
			this.ScriptData = scriptFile;
			this.DisplayName = displayName;
			this.ModelType = modelType;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002D24B File Offset: 0x0002B44B
		protected ModelMetaData()
		{
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002D258 File Offset: 0x0002B458
		public AbstractNodeObject CreateObject()
		{
			AbstractNodeObject result;
			try
			{
				AbstractNodeObject abstractNodeObject = this.OnCreateObject();
				abstractNodeObject.Name = Services.ProjectOperations.CurrentSelectedProject.CreateObjectName(abstractNodeObject, "");
				result = abstractNodeObject;
			}
			catch (Exception exception)
			{
				LogConfig.Output.Info(LanguageInfo.CreateObjectError + " : " + this.DisplayName, exception);
				result = null;
			}
			return result;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002D2C8 File Offset: 0x0002B4C8
		protected virtual AbstractNodeObject OnCreateObject()
		{
			return this.CreateDefaultObject(this.Type);
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0002D2E8 File Offset: 0x0002B4E8
		private AbstractNodeObject CreateDefaultObject(Type type)
		{
			return Activator.CreateInstance(this.Type) as AbstractNodeObject;
		}
	}
}
