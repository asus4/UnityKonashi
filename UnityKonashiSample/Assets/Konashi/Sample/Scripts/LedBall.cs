using UnityEngine;
using System.Collections;

namespace Konashi
{
	[RequireComponent(typeof(Collider))]
	[RequireComponent(typeof(MeshRenderer))]
	public class LedBall : MonoBehaviour
	{
		[RangeAttribute(0.0f, 1.0f)]
		public float emission;
		
		public Color emissionColor0 = Color.black;
		public Color emissionColor1 = Color.white;
		
		public KonashiDigitalIOPin pin;
		
		
		Material mat;
		int matID_EmissionColor;
		bool touched;
		
		void OnEnable()
		{
			matID_EmissionColor = Shader.PropertyToID("_EmissionColor");
			mat = this.GetComponent<MeshRenderer>().material;
		}
		
		
		void Update()
		{
			float target = touched ? 1.0f : 0.0f;
			emission += (target - emission) * 0.1f;
			
			mat.SetColor(matID_EmissionColor, Color.Lerp(emissionColor0, emissionColor1, emission));
		}
		
		
		void OnMouseDown()
		{
			touched = true;
			KonashiPlugin.DigitalWrite(pin, KonashiLevel.High);
		}
		
		void OnMouseUp()
		{
			touched = false;
			KonashiPlugin.DigitalWrite(pin, KonashiLevel.Low);
		}
	}
	
}
