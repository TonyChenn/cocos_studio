using System;
using System.Collections.Generic;
using System.Linq;
using CocoStudio.Basic;
using CocoStudio.Projects.Formates;
using Mono.Addins;

namespace CocoStudio.Core
{
	public class CompositeProcesserManager
	{
		public static CompositeProcesserManager Instance { get; private set; } = new CompositeProcesserManager();

		private CompositeProcesserManager()
		{
			this.Initialize();
		}

		public IEnumerable<string> AfterTypes
		{
			get
			{
				return this.afterTypes;
			}
			set
			{
				this.afterTypes = value;
			}
		}

		public List<string> CompositeFilterTypes
		{
			get
			{
				return this.compositeFilterTypes.ToList<string>();
			}
			set
			{
				this.compositeFilterTypes = value;
			}
		}

		public List<string> PretreatmentTypes
		{
			get
			{
				return this.pretreatmentTypes.ToList<string>();
			}
			set
			{
				this.pretreatmentTypes = value;
			}
		}

		private void Initialize()
		{
			try
			{
				HashSet<string> hashSet = new HashSet<string>();
				HashSet<string> hashSet2 = new HashSet<string>();
				HashSet<string> hashSet3 = new HashSet<string>();
				ICompositeResourceProcesser[] extensionObjects = AddinManager.GetExtensionObjects<ICompositeResourceProcesser>();
				foreach (ICompositeResourceProcesser compositeResourceProcesser in extensionObjects)
				{
					List<string> list = compositeResourceProcesser.GetPretreatmentTypes();
					if (list != null)
					{
						foreach (string item in list)
						{
							hashSet.Add(item);
						}
					}
					List<string> filterTypes = compositeResourceProcesser.GetFilterTypes();
					if (filterTypes != null)
					{
						foreach (string item2 in filterTypes)
						{
							hashSet2.Add(item2);
						}
					}
					List<string> list2 = compositeResourceProcesser.GetAfterTypes();
					if (list2 != null)
					{
						foreach (string item3 in list2)
						{
							hashSet3.Add(item3);
						}
					}
				}
				this.pretreatmentTypes = hashSet;
				this.compositeFilterTypes = hashSet2;
				this.afterTypes = hashSet3;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Debug("Load resource panel addins failed", exception);
			}
		}

		private IEnumerable<string> pretreatmentTypes;

		private IEnumerable<string> compositeFilterTypes;

		private IEnumerable<string> afterTypes;
	}
}
