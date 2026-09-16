using System;
using CocoStudio.Core;
using CocoStudio.Core.Events;
using CocoStudio.Core.View;
using CocoStudio.Model.ViewModel;
using CocoStudio.Projects;
using Gtk;

namespace CocoStudio.Model.Editor
{
	internal class ResourceLocateButton : IconButton
	{
		public ResourceLocateButton(ImageEventBox imageEventBox) : this(delegate
		{
			return imageEventBox.ResourceFile;
		})
		{
		}

		public ResourceLocateButton(Func<ResourceFile> getResourceFile) : base(ResourceLocateButton.locateIcon)
		{
			this.getResourceFile = getResourceFile;
			base.SetSizeRequest(18, 18);
			base.CanFocus = false;
			base.TooltipText = "在资源窗口中定位";
			base.Clicked += this.ResourceLocateButton_Clicked;
		}

		public Alignment CreateCenteredAlignment()
		{
			Alignment alignment = new Alignment(0.5f, 0.5f, 0f, 0f);
			alignment.Add(this);
			alignment.ShowAll();
			return alignment;
		}

		public static bool SupportsImageEditor(object target)
		{
			return target is SpriteObject || target is ImageViewObject || target is PanelObject || target is LoadingBarObject || target is TextBMFontObject;
		}

		public static bool SupportsGroupEditor(object target)
		{
			return target is ButtonObject || target is CheckBoxObject || target is SliderObject;
		}

		public static bool SupportsFileEditor(object target, string propertyName)
		{
			return target is TextFieldObject && propertyName == "FontResource" ||
				target is TextObject && propertyName == "FontResource" ||
				target is ParticleObject && propertyName == "FileData" ||
				target is FileNodeObject && propertyName == "FileData";
		}

		private void ResourceLocateButton_Clicked(object sender, EventArgs e)
		{
			ResourceFile resourceFile = this.getResourceFile();
			if (resourceFile != null && resourceFile.Parent != null)
			{
				foreach (Pad pad in Services.Workbench.Pads)
				{
					if (pad.Id == "Modules.Communal.ResourcePanel.ResourcePad")
					{
						pad.BringToFront();
						break;
					}
				}
				Services.EventsService.GetEvent<LocateResourceInPanelEvent>().Publish(resourceFile);
			}
		}

		private readonly Func<ResourceFile> getResourceFile;

		private static readonly Xwt.Drawing.Image locateIcon = Xwt.Drawing.Image.FromResource(typeof(ResourceLocateButton).Assembly, "CocoStudio.Model.EditorResource.resource_locator.png");
	}
}
