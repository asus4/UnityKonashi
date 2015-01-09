using UnityEngine;
using System.Collections;

namespace Konashi
{
	public enum KonashiLevel {
		Unknown = -1,
		Low	 = 0,
		High = 1,
	}
	
	public enum KonashiPinMode {
		Input	= 0,
		Output = 1,
		NoPulls = 0,
		Pullup = 1
	}
	
	public enum KonashiResult {
		Success = 0,
		Failure = -1
	}
	
	// Konashi I/0 pin
	public enum KonashiPinMask {
		PinMask0 = 0,
		PinMask1 = 1 << 0,
		PinMask2 = 1 << 1,
		PinMask3 = 1 << 2,
		PinMask4 = 1 << 3,
		PinMask5 = 1 << 4,
		PinMask6 = 1 << 5,
		PinMask7 = 1 << 6,
	}
	
	public enum KonashiDigitalIOPin {
		DigitalIO0 = 0,
		DigitalIO1 = 1,
		DigitalIO2 = 2,
		DigitalIO3 = 3,
		DigitalIO4 = 4,
		DigitalIO5 = 5,
		DigitalIO6 = 6,
		DigitalIO7 = 7,
		S1 = DigitalIO0,
		LED2 = DigitalIO1,
		LED3 = DigitalIO2,
		LED4 = DigitalIO3,
		LED5 = DigitalIO4,
		I2C_SDA = 6,
		I2C_SCL = 7
	}
	
	public enum KonashiAnalogIOPin {
		AnalogIO0 = 0,
		AnalogIO1 = 1,
		AnalogIO2 = 2
	}
	
	// Konashi PWM
	public enum KonashiPWMMode {
		Disable = 0,
		Enable = 1,
		EnableLED = 2
	}
	
	// Konashi I2C
	public enum KonashiI2CMode {
		Disable = 0,
		Enable = 1,
		Enable100K = 1,
		Enable400K = 2
	}
	
	public enum KonashiI2CCondition {
		Stop = 0,
		Start = 1,
		Restart = 2
	}
	
	// Konashi UART
	public enum KonashiUartMode {
		Disable = 0,
		Enable = 1
	}
	
	// Konashi UART baudrate
	public enum KonashiUartBaudrate {
		Rate2K4 = 0x000a,
		Rate9K6 = 0x0028,
		Rate19K2 = 0x0050,
		Rate38K4 = 0x00a0,
		Rate57K6 = 0x00f0,
		Rate76K8 = 0x0140,
		Rate115K2 = 0x01e0
	}
}
