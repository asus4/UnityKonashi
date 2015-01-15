using UnityEngine;
using System.Collections;

namespace Konashi
{
	public class KonashiTest : MonoBehaviour {
		Vector2 scrollPosition = Vector2.zero;
		string log = "hogehoge";
		
		void Start()
		{
			var konashi = KonashiPlugin.instance;
			
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
				KonashiPlugin.Find();
			}
			if(DrawButton("Software rivision"))
			{
				log = "softwre rivision : " + KonashiPlugin.sofwareRevision;
			}
			if(DrawButton("Disconnect"))
			{
				KonashiPlugin.Disconect();
			}
			if(DrawButton("is connected?"))
			{
				log = "is connected ? : " + KonashiPlugin.isConnected;
			}
			if(DrawButton("is ready?"))
			{
				log = "is ready ? : " + KonashiPlugin.isReady;
			}
			if(DrawButton("peripheralName"))
			{
				log = "peripheralName : " + KonashiPlugin.peripheralName;
			}
			if(DrawButton("pinmode 0:in 1:out 2:out 3:out"))
			{
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO0, KonashiPinMode.Input);
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO1, KonashiPinMode.Output);
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO2, KonashiPinMode.Output);
				KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO3, KonashiPinMode.Output);
			}
			if(DrawButton("Run LED Task"))
			{
				StartCoroutine(RunLED());
			}
			if(DrawButton("pinmode all in"))
			{
				KonashiPlugin.PinModeAll(KonashiPinMode.Input);
			}
			if(DrawButton("digital wirte 1:High"))
			{
				KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.High);
			}
			if(DrawButton("digital wirte 1:Log"))
			{
				KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.Low);
			}
			if(DrawButton("digital write all high"))
			{
				KonashiPlugin.DigitalWriteAll(KonashiLevel.High);
			}
			if(DrawButton("digital write all low"))
			{
				KonashiPlugin.DigitalWriteAll(KonashiLevel.Low);
			}
			if(DrawButton("BatteryLevelReadRequest"))
			{
				KonashiPlugin.BatteryLevelReadRequest();
			}
			if(DrawButton("SignalStrengthReadRequest"))
			{
				KonashiPlugin.SignalStrengthReadRequest();
			}
			GUILayout.EndScrollView();
		}
		
		bool DrawButton(string label) {
			return GUILayout.Button(label, GUILayout.MinWidth(200), GUILayout.MinHeight(100));
		}
		
		IEnumerator RunLED() {
			float duration = 0.5f;
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.Low);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO2, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO2, KonashiLevel.Low);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO3, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO3, KonashiLevel.Low);
		}
		
		
	}
}
