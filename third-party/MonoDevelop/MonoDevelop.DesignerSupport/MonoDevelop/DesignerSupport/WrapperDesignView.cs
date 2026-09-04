using System;
using System.IO;
using Gtk;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using MonoDevelop.Projects;

namespace MonoDevelop.DesignerSupport
{
	public class WrapperDesignView : AbstractViewContent
	{
		private IViewContent content;

		private VBox contentBox;

		private Widget topBar;

		public override string TabPageLabel => content.TabPageLabel;

		public Widget TopBar
		{
			get
			{
				return topBar;
			}
			protected set
			{
				if (topBar != null)
				{
					contentBox.Remove(topBar);
				}
				if (value != null)
				{
					contentBox.PackStart(value, expand: false, fill: false, 0u);
				}
				topBar = value;
			}
		}

		protected IViewContent Content => content;

		public override Project Project
		{
			get
			{
				return base.Project;
			}
			set
			{
				base.Project = value;
				content.Project = value;
			}
		}

		public override Widget Control => contentBox;

		public override bool IsDirty
		{
			get
			{
				return content.IsDirty;
			}
			set
			{
				content.IsDirty = value;
			}
		}

		public override bool IsReadOnly => content.IsReadOnly;

		public override string ContentName
		{
			get
			{
				return content.ContentName;
			}
			set
			{
				content.ContentName = value;
			}
		}

		public WrapperDesignView(IViewContent content)
		{
			this.content = content;
			contentBox = new VBox();
			contentBox.PackEnd(content.Control, expand: true, fill: true, 0u);
			contentBox.ShowAll();
			content.ContentChanged += OnTextContentChanged;
			content.DirtyChanged += OnTextDirtyChanged;
			IdeApp.Workbench.ActiveDocumentChanged += OnActiveDocumentChanged;
		}

		protected override void OnWorkbenchWindowChanged(EventArgs e)
		{
			base.OnWorkbenchWindowChanged(e);
			content.WorkbenchWindow = WorkbenchWindow;
		}

		public override void Dispose()
		{
			content.ContentChanged -= OnTextContentChanged;
			content.DirtyChanged -= OnTextDirtyChanged;
			IdeApp.Workbench.ActiveDocumentChanged -= OnActiveDocumentChanged;
			base.Dispose();
		}

		public override void Load(string fileName)
		{
			ContentName = fileName;
			content.Load(fileName);
		}

		public override void LoadNew(Stream content, string mimeType)
		{
			this.content.LoadNew(content, mimeType);
		}

		public override void Save(string fileName)
		{
			content.Save(fileName);
		}

		private void OnTextContentChanged(object s, EventArgs args)
		{
			OnContentChanged(args);
		}

		private void OnTextDirtyChanged(object s, EventArgs args)
		{
			OnDirtyChanged(args);
		}

		private void OnActiveDocumentChanged(object s, EventArgs args)
		{
			if (IdeApp.Workbench.ActiveDocument.GetContent<WrapperDesignView>() == this)
			{
				OnDocumentActivated();
			}
		}

		protected virtual void OnDocumentActivated()
		{
		}

		public override object GetContent(Type type)
		{
			return base.GetContent(type) ?? content.GetContent(type);
		}
	}
}
