using UnityEngine;
using System.Collections;

namespace Konashi {
	public class KonashiSample3D : MonoBehaviour {
		
		// set your konashi name
		public string konashiName = "konashi#1-2345";
		
		void Start () {
			
			var konashi = KonashiPlugin.instance;
			konashi.Initialize();
			
			konashi.OnReady += () => {
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO1, KonashiPinMode.Output);
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO2, KonashiPinMode.Output);
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO3, KonashiPinMode.Output);
			};
			
			KonashiPlugin.Find(konashiName);
		}
		
		
		
		
		
	}
}
