using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Konashi
{
	public class UnityKonashiPlugin : MonoBehaviour {
		#region singlton
		static UnityKonashiPlugin _instance;
		public static UnityKonashiPlugin instance
		{
			get
			{
				if(_instance == null)
				{
					
					_instance = GameObject.FindObjectOfType<UnityKonashiPlugin>();
					if(_instance == null) {
						_instance = new GameObject("UnityKonashiPlugin").AddComponent<UnityKonashiPlugin>();
					}
					DontDestroyOnLoad(_instance.gameObject);
				}
				return _instance;
			}
		}
		
		void Awake() 
		{
			if(_instance == null)
			{
				_instance = this;
				DontDestroyOnLoad(this);
			}
			else
			{
				if(this != _instance)
					Destroy(this.gameObject);
			}
		}
		#endregion
		
		#region events
		public delegate void DigitalPinValueHandler(KonashiDigitalIOPin pin, int value);
		public delegate void AnalogPinValueHandler(KonashiAnalogIOPin pin, int value);
		public delegate void RawDataHandler(byte[] data);
		public delegate void ValueHandler(int value);
		public delegate void VoidHandler();
		
		public event VoidHandler OnConnected;
		public event VoidHandler OnDisconnected;
		public event VoidHandler OnReady;
		public event DigitalPinValueHandler OnUpdatePioInput;
		public event DigitalPinValueHandler OnUpdatePioOutput;
		public event AnalogPinValueHandler OnUpdateAnalogValue;
		public event RawDataHandler OnUartRxComplete;
		public event RawDataHandler OnI2CReadComplete;
		public event ValueHandler OnUpdateBatteryLevel;
		public event ValueHandler OnUpdateSignalStrength;
		#endregion
		
		#region konashi events
		
		// Konashi control public methods
		void KonashiEventConnected(string value)
		{
			Log ("KonashiEventConnected");
			if(OnConnected != null) {
				OnConnected();
			}
		}
		
		void KonashiEventDisconnected(string value)
		{
			Log ("KonashiEventDisconnected");
			if(OnDisconnected != null) {
				OnDisconnected();
			}
		}
		
		void KonashiEventReady(string value)
		{
			Log("KonashiEventReady");
			if(OnReady != null) {
				OnReady();
			}
		}
		
		void KonashiEventUpdatePioInput(string json)
		{
			Log("KonashiEventUpdatePioInput + " + json);
			if(OnUpdatePioInput != null) {
				int pin, value;
				if(DecodePinValueJson(json, out pin, out value)) {
					OnUpdatePioInput((KonashiDigitalIOPin)pin, value);
				}
			}
		}
		
		void KonashiEventUpdatePioOutput(string json)
		{
			Log("KonashiEventUpdatePioOutput + " +json);
			if(OnUpdatePioOutput != null) {
				int pin, value;
				if(DecodePinValueJson(json, out pin, out value)) {
					OnUpdatePioOutput((KonashiDigitalIOPin)pin, value);
				}
			}
		}
		
		void KonashiEventUpdateAnalogValue(string json)
		{
			Log("KonashiEventUpdateAnalogValue + " +json);
			if(OnUpdateAnalogValue != null) {
				int pin, value;
				if(DecodePinValueJson(json, out pin, out value)) {
					OnUpdateAnalogValue((KonashiAnalogIOPin)pin, value);
				}
			}
		}
		
		void KonashiEventUartRxComplete(string strData)
		{
			byte[] data = DecodeBase64(strData);
			Log("KonashiEventUartRxComplete + " +data);
			if(OnUartRxComplete != null) {
				OnUartRxComplete(data);
			}
		}
		
		void KonashiEventI2CReadComplete(string strData)
		{
			byte[] data = DecodeBase64(strData);
			Log("KonashiEventI2CReadComplete + " +data);
			if(OnI2CReadComplete != null) {
				OnI2CReadComplete(data);
			}
		}
		
		void KonashiEventUpdateBatteryLevel(string strInt)
		{
			int level;
			if(int.TryParse(strInt, out level)) {
				Log("KonashiEventUpdateBatteryLevel" + level);
				if(OnUpdateBatteryLevel != null) {
					OnUpdateBatteryLevel(level);
				}
			}
		}
		
		void KonashiEventUpdateSignalStrength(string strInt)
		{
			int level;
			if(int.TryParse(strInt, out level)) {
				Log("KonashiEventUpdateSignalStrength" + level);
				if(OnUpdateSignalStrength != null) {
					OnUpdateSignalStrength(level);
				}
			}
		}
		#endregion
		
		#region private methods
		static void Log(string msg) {
			Debug.Log(msg.ToString());
		}
		
		static byte[] DecodeBase64(string strData)
		{
			if(string.IsNullOrEmpty(strData)) {
				return new byte[0];
			}
			else {
				return Convert.FromBase64String(strData);
			}
		}
		
		static bool DecodePinValueJson(string json, out int pin, out int value)
		{
			bool success = false;
			try {
				var dict = MiniJSON.Json.Deserialize(json) as Dictionary<string,object>;
				pin = (int)(long) dict["pin"];
				value = (int)(long) dict["value"];
				success = true;
			}
			catch(Exception ex) {
				Debug.LogException(ex);
				pin = -1;
				value = -1;
			}
			finally {
			}
			return success;
		}
		#endregion
		
		#region konashi methods
		// Konashi control public methods
		public KonashiResult Initialize()
		{
			return _konashi_initialize();
		}
		
		static public KonashiResult Find()
		{
			return _konashi_find();
		}
		
		static public KonashiResult Find(string name)
		{
			return _konashi_findWithName(name);
		}
		
		static public string sofwareRevision
		{
			get {return _konashi_softwareRevisionString();}
		}
		
		static public KonashiResult Disconect()
		{
			return _konashi_disconnect();
		}
		
		static public bool isConnected
		{
			get {return _konashi_isConnected();}
		}
		
		static public bool isReady
		{
			get {return _konashi_isReady();}	
		}
		
		static public string peripheralName
		{
			get {return _konashi_peripheralName();}
		}
		
		// Konashi PIO public methods
		static public KonashiResult PinMode(KonashiDigitalIOPin pin, KonashiPinMode mode)
		{
			return _konashi_pinMode(pin, mode);
		}
		
		static public KonashiResult PinModeAll(KonashiPinMode mode)
		{
			return _konashi_pinModeAll(mode);
		}
		
		static public KonashiResult DigitalWrite(KonashiDigitalIOPin pin, KonashiLevel value)
		{
			return _konashi_digitalWrite(pin, value);
		}
		
		static public KonashiResult DigitalWriteAll(KonashiLevel value)
		{
			return _konashi_digitalWriteAll(value);
		}
		
		static public KonashiResult PinPullup(KonashiDigitalIOPin pin, KonashiPinMode mode)
		{
			return _konashi_pinPullup(pin, mode);
		}
		
		static public KonashiResult PinPullupAll(int mode)
		{
			return _konashi_pinPullupAll(mode);
		}
		
		// Konashi PWM public methods
		static public KonashiResult PwmMode(KonashiDigitalIOPin pin, KonashiPWMMode mode)
		{
			return _konashi_pwmMode(pin, mode);
		}
		
		static public KonashiResult PwmPeriod(KonashiDigitalIOPin pin, uint period)
		{
			return _konashi_pwmPeriod(pin, period);
		}
		
		static public KonashiResult PwmDuty(KonashiDigitalIOPin pin, uint duty)
		{
			return _konashi_pwmDuty(pin, duty);
		}
		
		static public KonashiResult PwmLedDrive(KonashiDigitalIOPin pin, int ratio)
		{
			return _konashi_pwmLedDrive(pin, ratio);
		}
		
		// Konashi analog IO public methods
		static public int AnalogReference()
		{
			return _konashi_analogReference();
		}
		
		static public KonashiResult AnalogReadRequest(KonashiAnalogIOPin pin)
		{
			return _konashi_analogReadRequest(pin);
		}
		
		static public KonashiResult AnalogWrite(KonashiAnalogIOPin pin ,int milliVolt)
		{
			return _konashi_analogWrite(pin, milliVolt);
		}
		
		// Konashi I2C public methods
		static public KonashiResult I2cMode(KonashiI2CMode mode)
		{
			return _konashi_i2cMode(mode);
		}
		
		static public KonashiResult I2cStartCondition()
		{
			return _konashi_i2cStartCondition();
		}
		
		static public KonashiResult I2cRestartCondition()
		{
			return _konashi_i2cRestartCondition();
		}
		
		static public KonashiResult I2cStopCondition()
		{
			return _konashi_i2cStopCondition();
		}
		
		static public KonashiResult I2cWriteData(byte[] bytes, byte address)
		{
			return _konashi_i2cWriteData(bytes, (uint)bytes.Length, address);
		}
		
		static public KonashiResult I2cWriteString(string data, byte address)
		{
			return _konashi_i2cWriteString(data, address);
		}
		
		static public KonashiResult I2cReadRequest(int length, byte address)
		{
			return _konashi_i2cReadRequest(length, address);
		}
		
		static public byte[] I2cReadData()
		{
			return DecodeBase64(_konashi_i2cReadData());
		}
		
		// Konashi UART public methods
		static public KonashiResult UartMode(KonashiUartMode mode, KonashiUartBaudrate baudrate)
		{
			return _konashi_uartMode(mode, baudrate);
		}
		
		static public KonashiResult UartWriteData(byte[] bytes)
		{
			return _konashi_uartWriteData(bytes, (uint)bytes.Length);
		}
		
		static public KonashiResult UartWriteString(string utfStr)
		{
			return _konashi_uartWriteString(utfStr);
		}
		
		static public byte[] ReadUartData()
		{
			return DecodeBase64(_konashi_readUartData());
		}
		
		// Konashi hardware public methods
		static public KonashiResult Reset()
		{
			return _konashi_reset();
		}
		
		static public KonashiResult BatteryLevelReadRequest()
		{
			return _konashi_batteryLevelReadRequest();
		}
		
		static public KonashiResult SignalStrengthReadRequest()
		{
			return _konashi_signalStrengthReadRequest();
		}
		#endregion
		
		#if (UNITY_IPHONE && !UNITY_EDITOR)
		// Konashi control public methods
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_initialize ();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_find ();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_findWithName (string name);
		[DllImport("__Internal")]
		static extern string _konashi_softwareRevisionString();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_disconnect();
		[DllImport("__Internal")]
		static extern bool _konashi_isConnected();
		[DllImport("__Internal")]
		static extern bool _konashi_isReady();
		[DllImport("__Internal")]
		static extern string _konashi_peripheralName();
		
		// Konashi PIO public methods
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pinMode(KonashiDigitalIOPin pin, KonashiPinMode mode);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pinModeAll(KonashiPinMode mode);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_digitalWrite(KonashiDigitalIOPin pin, KonashiLevel value);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_digitalWriteAll(KonashiLevel value);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pinPullup(KonashiDigitalIOPin pin, KonashiPinMode mode);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pinPullupAll(int mode);
		
		// Konashi PWM public methods
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pwmMode(KonashiDigitalIOPin pin, KonashiPWMMode mode);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pwmPeriod(KonashiDigitalIOPin pin, uint period);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pwmDuty(KonashiDigitalIOPin pin, uint duty);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_pwmLedDrive(KonashiDigitalIOPin pin, int ratio);
		
		// Konashi analog IO public methods
		[DllImport("__Internal")]
		static extern int _konashi_analogReference();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_analogReadRequest(KonashiAnalogIOPin pin);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_analogWrite(KonashiAnalogIOPin pin ,int milliVolt);
		[DllImport("__Internal")]
		
		// Konashi I2C public methods
		static extern KonashiResult _konashi_i2cMode(KonashiI2CMode mode);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_i2cStartCondition();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_i2cRestartCondition();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_i2cStopCondition();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_i2cWriteData(byte[] bytes, uint length, byte address);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_i2cWriteString(string data, byte address);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_i2cReadRequest(int length, byte address);
		[DllImport("__Internal")]
		static extern string _konashi_i2cReadData();
		
		// Konashi UART public methods
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_uartMode(KonashiUartMode mode, KonashiUartBaudrate baudrate);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_uartWriteData(byte[] bytes, uint length);
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_uartWriteString(string utfStr);
		[DllImport("__Internal")]
		static extern string _konashi_readUartData();
		
		// Konashi hardware public methods
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_reset();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_batteryLevelReadRequest();
		[DllImport("__Internal")]
		static extern KonashiResult _konashi_signalStrengthReadRequest();
		#else
		// Konashi control public methods
		static KonashiResult _konashi_initialize (){return KonashiResult.Success;}
		static KonashiResult _konashi_find (){return KonashiResult.Success;}
		static KonashiResult _konashi_findWithName (string name){return KonashiResult.Success;}
		static string _konashi_softwareRevisionString(){return "";}
		static KonashiResult _konashi_disconnect(){return KonashiResult.Success;}
		static bool _konashi_isConnected(){return false;}
		static bool _konashi_isReady(){return false;}
		static string _konashi_peripheralName(){return "";}
		
		// Konashi PIO public methods
		static KonashiResult _konashi_pinMode(KonashiDigitalIOPin pin, KonashiPinMode mode){return KonashiResult.Success;}
		static KonashiResult _konashi_pinModeAll(KonashiPinMode mode){return KonashiResult.Success;}
		static KonashiResult _konashi_digitalWrite(KonashiDigitalIOPin pin, KonashiLevel value){return KonashiResult.Success;}
		static KonashiResult _konashi_digitalWriteAll(KonashiLevel value){return KonashiResult.Success;}
		static KonashiResult _konashi_pinPullup(KonashiDigitalIOPin pin, KonashiPinMode mode){return KonashiResult.Success;}
		static KonashiResult _konashi_pinPullupAll(int mode){return KonashiResult.Success;}
		
		// Konashi PWM public methods
		static KonashiResult _konashi_pwmMode(KonashiDigitalIOPin pin, KonashiPWMMode mode){return KonashiResult.Success;}
		static KonashiResult _konashi_pwmPeriod(KonashiDigitalIOPin pin, uint period){return KonashiResult.Success;}
		static KonashiResult _konashi_pwmDuty(KonashiDigitalIOPin pin, uint duty){return KonashiResult.Success;}
		static KonashiResult _konashi_pwmLedDrive(KonashiDigitalIOPin pin, int ratio){return KonashiResult.Success;}
		
		// Konashi analog IO public methods
		static int _konashi_analogReference(){return 0;}
		static KonashiResult _konashi_analogReadRequest(KonashiAnalogIOPin pin){return KonashiResult.Success;}
		static KonashiResult _konashi_analogWrite(KonashiAnalogIOPin pin ,int milliVolt){return KonashiResult.Success;}
		
		// Konashi I2C public methods
		static KonashiResult _konashi_i2cMode(KonashiI2CMode mode){return KonashiResult.Success;}
		static KonashiResult _konashi_i2cStartCondition(){return KonashiResult.Success;}
		static KonashiResult _konashi_i2cRestartCondition(){return KonashiResult.Success;}
		static KonashiResult _konashi_i2cStopCondition(){return KonashiResult.Success;}
		static KonashiResult _konashi_i2cWriteData(byte[] bytes, uint length, byte address){return KonashiResult.Success;}
		static KonashiResult _konashi_i2cWriteString(string data, byte address){return KonashiResult.Success;}
		static KonashiResult _konashi_i2cReadRequest(int length, byte address){return KonashiResult.Success;}
		static string _konashi_i2cReadData(){return "";}
		
		// Konashi UART public methods
		static KonashiResult _konashi_uartMode(KonashiUartMode mode, KonashiUartBaudrate baudrate){return KonashiResult.Success;}
		static KonashiResult _konashi_uartWriteData(byte[] bytes, uint length){return KonashiResult.Success;}
		static KonashiResult _konashi_uartWriteString(string utfStr){return KonashiResult.Success;}
		static string _konashi_readUartData(){return "";}
		
		// Konashi hardware public methods
		static KonashiResult _konashi_reset(){return KonashiResult.Success;}
		static KonashiResult _konashi_batteryLevelReadRequest(){return KonashiResult.Success;}
		static KonashiResult _konashi_signalStrengthReadRequest(){return KonashiResult.Success;}
		#endif
	}
}
