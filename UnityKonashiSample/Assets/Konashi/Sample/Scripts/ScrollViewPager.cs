using UnityEngine;
using UI = UnityEngine.UI;
using System.Collections;

namespace Konashi
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class ScrollViewPager : MonoBehaviour
	{
		public enum ScaleType {
			Horizontal,
			Vertical,
			Both,
			None
		}
		public UI.CanvasScaler canvasScaler;
		public Vector2 scale = Vector2.one;
		public ScaleType type;
		
		RectTransform rectTransform;
		
		void OnEnable()
		{
			rectTransform = this.GetComponent<RectTransform>();
		}
		
		void Update()
		{
			if(canvasScaler.uiScaleMode == UI.CanvasScaler.ScaleMode.ConstantPhysicalSize) {
				UpdatePhysicalSize();
			}
			
			switch(canvasScaler.uiScaleMode) {
			case UI.CanvasScaler.ScaleMode.ConstantPixelSize:
				UpdatePhysicalSize();
				break;
			case UI.CanvasScaler.ScaleMode.ScaleWithScreenSize:
				UpdateScreenSize();
				break;
			case UI.CanvasScaler.ScaleMode.ConstantPhysicalSize:
			
				break;
			default:
				// unknown
				break;
			}
		}
		
		void UpdatePhysicalSize()
		{
			Vector2 size = rectTransform.sizeDelta;
			Vector2 screen = new Vector2(Screen.width, Screen.height);
			
			if(type == ScaleType.Both) {
				size.x = screen.x * scale.x;
				size.y = screen.y * scale.y;
			}
			else if(type == ScaleType.Horizontal) {
				size.x = screen.x * scale.x;
			}
			else if(type == ScaleType.Vertical) {
				size.y = screen.y * scale.y;
			}
			rectTransform.sizeDelta = size;
		}
		
		void UpdateScreenSize()
		{
			Vector2 size = rectTransform.sizeDelta;
			Vector2 screen = canvasScaler.referenceResolution;
			
			if(type == ScaleType.Both) {
				size.x = screen.x * scale.x;
				size.y = screen.y * scale.y;
			}
			else if(type == ScaleType.Horizontal) {
				size.x = screen.x * scale.x;
			}
			else if(type == ScaleType.Vertical) {
				size.y = screen.y * scale.y;
			}
			rectTransform.sizeDelta = size;
		}
		
	}
}