using System;
using System.Collections;
using CocoStudio.Basic;
using MonoDevelop.Core;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Projects
{
	public abstract class SolutionItem : IExtendedDataItem, ILoadController, IPublish
	{
		public IDictionary ExtendedProperties
		{
			get
			{
				if (this.extendedProperties == null)
				{
					this.extendedProperties = new Hashtable();
				}
				return this.extendedProperties;
			}
		}

		public abstract string Name { get; set; }

		public Solution ParentSolution { get; internal set; }

		public SolutionFolder ParentFolder { get; internal set; }

		public void Publish(IProgressMonitor monitor, PublishInfo info)
		{
			try
			{
				this.OnPublish(monitor, info);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Run publish failed.", exception);
			}
		}

		protected virtual void OnPublish(IProgressMonitor monitor, PublishInfo info)
		{
		}

		void ILoadController.BeginLoad()
		{
			throw new NotImplementedException();
		}

		void ILoadController.EndLoad()
		{
			throw new NotImplementedException();
		}

		private Hashtable extendedProperties;
	}
}
