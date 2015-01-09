using UnityEngine;
using System.Collections;

namespace Konashi
{
	public class KonashiTest : MonoBehaviour {
		Vector2 scrollPosition = Vector2.zero;
		string log = "hogehoge";
		
		void Start()
		{
			var konashi = UnityKonashiPlugin.instance;
			
			// To use konashi initialize first.
			konashi.Initialize();
			
			// Events
			konashi.OnConnected += () => {
				log = "Onconnected";
			};
			konashi.OnDisconnected += () => {
				log = "OnDisconnected";
			};
			konashi.OnReady += () => {
				log = "OnReady";
			};
			konashi.OnUpdatePioInput += (KonashiDigitalIOPin pin, int value) => {
				log = string.Format("OnUpdatePioInput {0}:{1}", pin, value);
			};
			konashi.OnUpdatePioOutput += (KonashiDigitalIOPin pin, int value) => {
				log = string.Format("OnUpdatePioOutput {0}:{1}", pin, value);
			};
			konashi.OnUpdateAnalogValue += (KonashiAnalogIOPin pin, int value) => {
				log = string.Format("OnUpdateAnalogValue {0}:{1}", pin, value);
			};
			konashi.OnUartRxComplete += (byte[] data) => {
				log = string.Format("OnUartRxComplete length:{0}", data.Length);
			};
			konashi.OnI2CReadComplete += (byte[] data) => {
				log = string.Format("OnI2CReadComplete length:{0}", data.Length);
			};
			konashi.OnUpdateBatteryLevel += (int value) => {
				log = string.Format("OnUpdateBatteryLevel :{0}", value);
			};
			konashi.OnUpdateSignalStrength += (int value) => {
				log = string.Format("OnUpdateSignalStrength :{0}", value);
			};
		}
		
		
		void OnGUI()
		{
			GUILayout.Label("Log");
			GUILayout.Label(log);
			
			scrollPosition =  GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(300));
			if(DrawButton("Find"))
			{
				UnityKonashiPlugin.Find();
			}
			if(DrawButton("Software rivision"))
			{
				log = "softwre rivision : " + UnityKonashiPlugin.sofwareRevision;
			}
			if(DrawButton("Disconnect"))
			{
				UnityKonashiPlugin.Disconect();
			}
			if(DrawButton("is connected?"))
			{
				log = "is connected ? : " + UnityKonashiPlugin.isConnected;
			}
			if(DrawButton("is ready?"))
			{
				log = "is ready ? : " + UnityKonashiPlugin.isReady;
			}
			if(DrawButton("peripheralName"))
			{
				log = "peripheralName : " + UnityKonashiPlugin.peripheralName;
			}
			if(DrawButton("pinmode 0:in 1:out"))
			{
				UnityKonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO0, KonashiPinMode.Input);
				UnityKonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO1, KonashiPinMode.Output);
			}
			if(DrawButton("pinmode all in"))
			{
				UnityKonashiPlugin.PinModeAll(KonashiPinMode.Input);
			}
			if(DrawButton("digital wirte 1:High"))
			{
				UnityKonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.High);
			}
			if(DrawButton("digital wirte 1:Log"))
			{
				UnityKonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.Low);
			}
			if(DrawButton("digital write all high"))
			{
				UnityKonashiPlugin.DigitalWriteAll(KonashiLevel.High);
			}
			if(DrawButton("digital write all log"))
			{
				UnityKonashiPlugin.DigitalWriteAll(KonashiLevel.Low);
			}
			if(DrawButton("BatteryLevelReadRequest"))
			{
				UnityKonashiPlugin.BatteryLevelReadRequest();
			}
			if(DrawButton("SignalStrengthReadRequest"))
			{
				UnityKonashiPlugin.SignalStrengthReadRequest();
			}
			GUILayout.EndScrollView();
		}
		
		bool DrawButton(string label) {
			return GUILayout.Button(label, GUILayout.MinWidth(200), GUILayout.MinHeight(100));
		}
		
		
	}
}
