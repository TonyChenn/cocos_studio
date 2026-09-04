using System;
using System.Collections.Generic;
using System.Reflection;
using CocoStudio.EngineAdapterWrap;
using CocoStudio.UndoManager.Recorder;

namespace CocoStudio.Model.ViewModel
{
	// Token: 0x0200012C RID: 300
	public class LayoutExtender : BaseExtender
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x0002BC14 File Offset: 0x00029E14
		// (set) Token: 0x06000B1E RID: 2846 RVA: 0x0002BC2B File Offset: 0x00029E2B
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

		// Token: 0x06000B1F RID: 2847 RVA: 0x0002BC3B File Offset: 0x00029E3B
		public LayoutExtender(AbstractNodeObject bindingObject)
		{
			this.objectInstance = bindingObject;
			this.objectInstance.ParentChanged += this.OnObjectParentChanged;
			this.propertyNames = this.CollectLayoutProperty();
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x0002BC78 File Offset: 0x00029E78
		internal override void OnObjectPropertyChanged(PropertyInfo propertyInfo)
		{
			if (this.propertyNames.Contains(propertyInfo.Name))
			{
				this.RefreshProperty();
			}
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0002BCA8 File Offset: 0x00029EA8
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

		// Token: 0x06000B22 RID: 2850 RVA: 0x0002BCD8 File Offset: 0x00029ED8
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

		// Token: 0x06000B23 RID: 2851 RVA: 0x0002BD7C File Offset: 0x00029F7C
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

		// Token: 0x06000B24 RID: 2852 RVA: 0x0002BDC8 File Offset: 0x00029FC8
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

		// Token: 0x06000B25 RID: 2853 RVA: 0x0002BE28 File Offset: 0x0002A028
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

		// Token: 0x06000B26 RID: 2854 RVA: 0x0002BF88 File Offset: 0x0002A188
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

		// Token: 0x06000B27 RID: 2855 RVA: 0x0002BFD4 File Offset: 0x0002A1D4
		public override void Dispose()
		{
			if (this.objectInstance != null)
			{
				this.objectInstance.ParentChanged -= this.OnObjectParentChanged;
				this.objectInstance = null;
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x040004A5 RID: 1189
		private static bool _layoutEnabled = false;

		// Token: 0x040004A6 RID: 1190
		private AbstractNodeObject objectInstance;

		// Token: 0x040004A7 RID: 1191
		private readonly HashSet<string> propertyNames;

		// Token: 0x040004A8 RID: 1192
		private bool isFirstAdded = true;
	}
}
