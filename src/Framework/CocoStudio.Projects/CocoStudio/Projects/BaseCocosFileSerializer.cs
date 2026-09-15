using System;

namespace CocoStudio.Projects
{
	public abstract class BaseCocosFileSerializer : IGameFileSerializer, IComparable
	{
		public string ID
		{
			get
			{
				return this.OnGetID();
			}
		}

		protected abstract string OnGetID();

		public string Label
		{
			get
			{
				return this.OnGetLabel();
			}
		}

		protected abstract string OnGetLabel();

		public bool IsDefault { get; internal set; }

		public string Serialize(PublishInfo info, GameFile projFile)
		{
			return this.OnSerialize(info, projFile);
		}

		protected abstract string OnSerialize(PublishInfo info, GameFile projFile);

		public virtual void ContextInitialize(PublishInfo publishInfo)
		{
		}

		public virtual void ContextFinalize(PublishInfo publishInfo)
		{
		}

		public virtual string Description { get; private set; }

		public string SolutionLink
		{
			get
			{
				return "http://www.cocos2d-x.org/download";
			}
		}

		protected virtual int DisplayIndex { get; private set; }

		public int CompareTo(object other)
		{
			return this.DisplayIndex.CompareTo(((BaseCocosFileSerializer)other).DisplayIndex);
		}

		private const string defaultSolutionLink = "http://www.cocos2d-x.org/download";
	}
}
