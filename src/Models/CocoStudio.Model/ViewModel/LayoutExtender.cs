using System;
using System.Collections.Generic;
using System.Reflection;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.Model.ViewModel
{
	public class LayoutExtender : BaseExtender
	{
		public static bool LayoutEnabled
		{
			get
			{
				return LayoutExtender._layoutEnabled;
			}
			set
			{
				LayoutExtender._layoutEnabled = value;
				CSCocosHelp.RefreshLayoutSystemState(value);
			}
		}

		public LayoutExtender(AbstractNodeObject bindingObject)
		{
			this.objectInstance = bindingObject;
			this.objectInstance.ParentChanged += this.OnObjectParentChanged;
			this.propertyNames = this.CollectLayoutProperty();
		}

		internal override void OnObjectPropertyChanged(PropertyInfo propertyInfo)
		{
			if (this.propertyNames.Contains(propertyInfo.Name))
			{
				this.RefreshProperty();
			}
		}

		protected void OnObjectParentChanged(object sender, EventArgs e)
		{
			if (this.isFirstAdded)
			{
				this.InitLayoutProperty();
			}
			else
			{
				this.RefreshProperty();
			}
		}

		private HashSet<string> CollectLayoutProperty()
		{
			HashSet<string> hashSet = new HashSet<string>();
			PropertyInfo[] properties = this.objectInstance.GetType().GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (!hashSet.Contains(propertyInfo.Name))
				{
					object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(LayoutRefreshAttribute), false);
					if (customAttributes != null && customAttributes.Length > 0)
					{
						hashSet.Add(propertyInfo.Name);
					}
				}
			}
			hashSet.TrimExcess();
			return hashSet;
		}

		public static void RefreshLayout(VisualObject vObject)
		{
			AbstractNodeObject abstractNodeObject = vObject as AbstractNodeObject;
			if (abstractNodeObject != null)
			{
				CSNode2D csnode2D = abstractNodeObject.GetCSVisual() as CSNode2D;
				if (csnode2D != null)
				{
					csnode2D.RefreshLayout();
					LayoutExtender.RefreshBouding(abstractNodeObject);
				}
			}
		}

		private void RefreshProperty()
		{
			if (LayoutExtender.LayoutEnabled)
			{
				AbstractNodeObject parent = this.objectInstance.Parent;
				if (parent != null)
				{
					LayoutExtender.RefreshLayout(parent);
				}
				else
				{
					LayoutExtender.RefreshLayout(this.objectInstance);
				}
				NodeObject nodeObject = this.objectInstance as NodeObject;
				if (nodeObject != null)
				{
					nodeObject.LayoutState = true;
				}
			}
		}

		private static void RefreshBouding(AbstractNodeObject abstractObject)
		{
			CSNode2D csnode2D = abstractObject.GetCSVisual() as CSNode2D;
			if (csnode2D != null)
			{
				PointF position = csnode2D.GetPosition();
				SizeF size = csnode2D.GetSize();
				csnode2D.SetPosition(position);
				csnode2D.SetSize(size);
				DefaultRecorder defaultRecorder = abstractObject.Recorder as DefaultRecorder;
				if (defaultRecorder != null)
				{
					NodeObject nodeObject = abstractObject as NodeObject;
					if (nodeObject != null)
					{
						nodeObject.LayoutState = true;
						defaultRecorder.UpdateCachedValue("LeftMargin", nodeObject.LeftMargin);
						defaultRecorder.UpdateCachedValue("RightMargin", nodeObject.RightMargin);
						defaultRecorder.UpdateCachedValue("TopMargin", nodeObject.TopMargin);
						defaultRecorder.UpdateCachedValue("BottomMargin", nodeObject.BottomMargin);
					}
					defaultRecorder.UpdateCachedValue("Size", abstractObject.Size);
					defaultRecorder.UpdateCachedValue("Position", abstractObject.Position);
				}
				foreach (AbstractNodeObject abstractObject2 in abstractObject.Children)
				{
					LayoutExtender.RefreshBouding(abstractObject2);
				}
			}
		}

		private void InitLayoutProperty()
		{
			CSNode2D csnode2D = this.objectInstance.GetCSVisual() as CSNode2D;
			if (csnode2D != null)
			{
				PointF position = csnode2D.GetPosition();
				SizeF size = csnode2D.GetSize();
				csnode2D.SetPosition(position);
				csnode2D.SetSize(size);
			}
		}

		public override void Dispose()
		{
			if (this.objectInstance != null)
			{
				this.objectInstance.ParentChanged -= this.OnObjectParentChanged;
				this.objectInstance = null;
			}
			GC.SuppressFinalize(this);
		}

		private static bool _layoutEnabled = false;

		private AbstractNodeObject objectInstance;

		private readonly HashSet<string> propertyNames;

		private bool isFirstAdded = true;
	}
}
