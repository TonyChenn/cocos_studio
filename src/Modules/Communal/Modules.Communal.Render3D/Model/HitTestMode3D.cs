using System;
using System.Collections.Generic;
using System.Drawing;
using CocoStudio.Model;
using CocoStudio.Model.ViewModel;
using CocoStudio.Model.ViewModel.HitTest;
using Modules.Communal.Render.Model;

namespace Modules.Communal.Render3D.Model
{
	internal class HitTestMode3D : HitTestMode
	{
		public override HitTestResult GetHitVisual(VisualObject rootObject, CocoStudio.Model.PointF point)
		{
			List<HitTestResult> allHitVisual = base.GetAllHitVisual(rootObject, point);
			if (allHitVisual.Count == 0)
			{
				return null;
			}
			Dictionary<int, HitTestResult> dictionary = new Dictionary<int, HitTestResult>();
			for (int i = 0; i < allHitVisual.Count; i++)
			{
				Node3DObject node3DObject = allHitVisual[i].HitVisual as Node3DObject;
				if (node3DObject != null)
				{
					int num = 16777215 / allHitVisual.Count * (i + 1);
					int blue = num & 255;
					int green = num >> 8 & 255;
					int red = num >> 16;
					node3DObject.SetPixelRenderMode(Color.FromArgb(red, green, blue));
					dictionary.Add(num, allHitVisual[i]);
				}
			}
			CameraObject camera = GameWindow.Current.GetSceneObject().GetCamera();
			Color pickColor = camera.GetPickColor(point);
			for (int j = 0; j < allHitVisual.Count; j++)
			{
				Node3DObject node3DObject2 = allHitVisual[j].HitVisual as Node3DObject;
				if (node3DObject2 != null)
				{
					node3DObject2.RestoreRenderMode();
				}
			}
			if (pickColor == Color.Black)
			{
				return null;
			}
			int num2 = (int)pickColor.R << 16;
			num2 += (int)pickColor.G << 8;
			num2 += (int)pickColor.B;
			HitTestResult result;
			dictionary.TryGetValue(num2, out result);
			return result;
		}

		private const int ColorMax = 16777215;
	}
}
