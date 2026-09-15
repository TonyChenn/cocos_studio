using System;
using System.Collections.ObjectModel;
using CocoStudio.Lib.Prism;
using CocoStudio.Model.ViewModel;

namespace CocoStudio.Model.Event
{
	public class CutVisualObjectsEvent : CompositePresentationEvent<ReadOnlyCollection<VisualObject>>
	{
	}
}
