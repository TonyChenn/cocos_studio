using System;
using System.ComponentModel;
using CocoStudio.Basic;
using CocoStudio.Core;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.Visiter;
using Modules.Communal.MultiLanguage;

namespace CocoStudio.Model
{
	public class ModelMetaData
	{
		public virtual Type Type { get; protected set; }

		public string DisplayName { get; protected set; }

		internal bool IsDefault { get; private set; }

		public EnumModelType ModelType { get; protected set; }

		public ScriptFileData ScriptData { get; protected set; }

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

		protected ModelMetaData(Type type, ScriptFileData scriptFile, string displayName = null, EnumModelType modelType = EnumModelType.TwoDimension)
		{
			this.Type = type;
			this.ScriptData = scriptFile;
			this.DisplayName = displayName;
			this.ModelType = modelType;
		}

		protected ModelMetaData()
		{
		}

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

		protected virtual AbstractNodeObject OnCreateObject()
		{
			return this.CreateDefaultObject(this.Type);
		}

		private AbstractNodeObject CreateDefaultObject(Type type)
		{
			return Activator.CreateInstance(this.Type) as AbstractNodeObject;
		}
	}
}
