using System;
using System.Collections;

namespace Modules.Communal.ResourcePanel
{
	public interface ITreeBuild
	{
		void UpdateAll();

		void Update();

		void Update(object objecData);

		void UpdateChildren();

		void Remove();

		void Remove(object dataObject);

		void AddChild(object dataObject);

		void AddChildren(IEnumerable dataObjects);

		void AddChild(object dataObject, bool moveToChild);

		void AddChild(object parent, object dataObject, bool moveToChild);
	}
}
