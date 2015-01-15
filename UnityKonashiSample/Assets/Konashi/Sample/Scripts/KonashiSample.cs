using UnityEngine;
using System.Collections;
using UI = UnityEngine.UI;

namespace Konashi
{
	public class KonashiSample : MonoBehaviour
	{
		void Start () {
			var konashi = KonashiPlugin.instance;
			
			// Initialize first to use konashi.
			konashi.Initialize();
			
			// Events for konashi
			konashi.OnConnected += () => {
				Debug.Log("Onconnected");
			};
			konashi.OnDisconnected += () => {
				Debug.Log("OnDisconnected");
			};
			konashi.OnReady += () => {
				Debug.Log("OnReady");
			};
			konashi.OnUpdatePioInput += (KonashiDigitalIOPin pin, int value) => {
				Debug.LogFormat("OnUpdatePioInput {0}:{1}", pin, value);
			};
			konashi.OnUpdatePioOutput += (KonashiDigitalIOPin pin, int value) => {
				Debug.LogFormat("OnUpdatePioOutput {0}:{1}", pin, value);
			};
			konashi.OnUpdateAnalogValue += (KonashiAnalogIOPin pin, int value) => {
				Debug.LogFormat("OnUpdateAnalogValue {0}:{1}", pin, value);
			};
			konashi.OnUartRxComplete += (byte[] data) => {
				// To Bits
//				Debug.LogFormat("OnUartRxComplete length:{0}", data.Length);
				// To string
				Debug.LogFormat("OnUartRxComplete :{0}", System.Text.Encoding.ASCII.GetString(data));
			};
			konashi.OnI2CReadComplete += (byte[] data) => {
				Debug.LogFormat("OnI2CReadComplete length:{0}", data.Length);
			};
			konashi.OnUpdateBatteryLevel += (int value) => {
				Debug.LogFormat("OnUpdateBatteryLevel :{0}", value);
			};
			konashi.OnUpdateSignalStrength += (int value) => {
				Debug.LogFormat("OnUpdateSignalStrength :{0}", value);
			};
		}
		
#region UI Events
		public void KonashiFind()
		{
			Log ("Now serching...Wait for seconds.");
			KonashiPlugin.Find();
			// UnityKonashiPlugin.Find("konashi_1234"); // Also able to find with name
		}
		
		public void KonashiStatus()
		{
			LogF("connected:{0} ready:{1}", KonashiPlugin.isConnected, KonashiPlugin.isReady);
		}
		
		public void KonashiInfo()
		{
			if(!KonashiPlugin.isConnected) {
				Debug.LogWarning("Konashi is not conected.");
				return;
			}
			LogF("name:{0} version:{1}",
			KonashiPlugin.peripheralName,
			KonashiPlugin.sofwareRevision);
		}
		
		public void KonashiDisconnect()
		{
			KonashiPlugin.Disconect();
		}
		
		public void KonashiBatteryLevelReadRequest()
		{
			KonashiPlugin.BatteryLevelReadRequest();
		}
		
		public void KonashiSignalStrengthReadRequest()
		{
			KonashiPlugin.SignalStrengthReadRequest();
		}
		
		public void KonashiSetupPinmode()
		{
			LogF("Set pin 0:In, 1~4:Out");
			LogF("Now push the switch - this is DisitalIO0.");
			KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO0, KonashiPinMode.Input);
			KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO1, KonashiPinMode.Output);
			KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO2, KonashiPinMode.Output);
			KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO3, KonashiPinMode.Output);
			KonashiPlugin.PinMode(KonashiDigitalIOPin.DigitalIO4, KonashiPinMode.Output);
			
			// All set
//			UnityKonashiPlugin.PinModeAll(KonashiPinMode.Input) 
		}
		
		public void KonashiLEDTest() {
			StartCoroutine(_RunLED());
		}
		
		IEnumerator _RunLED() {
			float duration = 0.5f;
			LogF("1:ON");
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO1, KonashiLevel.Low);
			LogF("2:ON");
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO2, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO2, KonashiLevel.Low);
			LogF("3:ON");
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO3, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO3, KonashiLevel.Low);
			LogF("4:ON");
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO4, KonashiLevel.High);
			yield return new WaitForSeconds(duration);
			KonashiPlugin.DigitalWrite(KonashiDigitalIOPin.DigitalIO4, KonashiLevel.Low);
		}
		
		public void KonashiSetupPWMMode()
		{
			LogF("Set 1~4:PWM LED");
			KonashiPlugin.PwmMode(KonashiDigitalIOPin.DigitalIO1,KonashiPWMMode.EnableLED);
			KonashiPlugin.PwmMode(KonashiDigitalIOPin.DigitalIO2,KonashiPWMMode.EnableLED);
			KonashiPlugin.PwmMode(KonashiDigitalIOPin.DigitalIO3,KonashiPWMMode.EnableLED);
			KonashiPlugin.PwmMode(KonashiDigitalIOPin.DigitalIO4,KonashiPWMMode.EnableLED);
		}
		
		public void KonashiLEDSlider1Changed(UI.Slider slider)
		{
			int level = (int)(slider.normalizedValue * 100);
			KonashiPlugin.PwmLedDrive(KonashiDigitalIOPin.DigitalIO1, level);
		}
		
		public void KonashiLEDSlider2Changed(UI.Slider slider)
		{
			int level = (int)(slider.normalizedValue * 100);
			KonashiPlugin.PwmLedDrive(KonashiDigitalIOPin.DigitalIO2, level);
		}
		
		public void KonashiLEDSlider3Changed(UI.Slider slider)
		{
			int level = (int)(slider.normalizedValue * 100);
			KonashiPlugin.PwmLedDrive(KonashiDigitalIOPin.DigitalIO3, level);
		}
		
		public void KonashiLEDSlider4Changed(UI.Slider slider)
		{
			int level = (int)(slider.normalizedValue * 100);
			KonashiPlugin.PwmLedDrive(KonashiDigitalIOPin.DigitalIO4, level);
		}
		
		public void KonashiUARTSetup(UI.Toggle toggle)
		{
			if(toggle.isOn) {
				Log("Turn On UART");
				KonashiPlugin.UartMode(KonashiUartMode.Enable,KonashiUartBaudrate.Rate9K6);
			}
			else {
				Log("Turn Off UART");
				KonashiPlugin.UartMode(KonashiUartMode.Disable,KonashiUartBaudrate.Rate9K6);
			}
		}
		
		public void KonashiUARTSend(UI.InputField input)
		{
			string text = input.text;
			if(!string.IsNullOrEmpty(text)) {
				KonashiPlugin.UartWriteString(text);
				LogF("Send UART - {0}", text);
			}
		}
		
#endregion // UI Events

#region Private
		void Log(string msg)
		{
			Debug.Log(msg);
		}
		
		void LogF(string msg, params object[] args )
		{
			Debug.LogFormat(msg, args);
		}		
#endregion // Private
	}
}
