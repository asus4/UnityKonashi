//
//  UnityKonashiPlugin.m
//
//  Created by Koki Ibukuro on 1/8/15.
//
//

#import <Foundation/Foundation.h>
#import "Konashi.h"

@interface UnityKonashiPlugin : NSObject

@end

@implementation UnityKonashiPlugin

static UnityKonashiPlugin *sharedManager = nil;
#define UnityKonashiSendMessage(method,value) UnitySendMessage("UnityKonashiPlugin", (method), (value))
#define MessageJson(...) [NSString stringWithFormat:@"{\"%@\":%i,\"%@\":%i}", __VA_ARGS__].UTF8String

- (id) init
{
    if(self = [super init]) {
        [self _setupHandlers];
    }
    return self;
}

- (void) _setupHandlers {
    Konashi.shared.connectedHandler = ^()
    {
        UnityKonashiSendMessage(KonashiEventConnectedNotification.UTF8String,"0");
    };
    Konashi.shared.disconnectedHandler = ^()
    {
        UnityKonashiSendMessage(KonashiEventDisconnectedNotification.UTF8String, "0");
    };
    Konashi.shared.readyHandler = ^()
    {
        UnityKonashiSendMessage(KonashiEventReadyToUseNotification.UTF8String, "0");
    };
    Konashi.shared.digitalInputDidChangeValueHandler = ^(KonashiDigitalIOPin pin, int value)
    {
        UnityKonashiSendMessage(KonashiEventDigitalIODidUpdateNotification.UTF8String,
                                MessageJson(@"pin", pin, @"value", value));
    };
    Konashi.shared.digitalOutputDidChangeValueHandler = ^(KonashiDigitalIOPin pin, int value)
    {
        UnityKonashiSendMessage("KonashiEventUpdatePioOutput",
                                MessageJson(@"pin", pin, @"value", value));
    };
    Konashi.shared.analogPinDidChangeValueHandler = ^(KonashiAnalogIOPin pin, int value)
    {
        UnityKonashiSendMessage(KonashiEventAnalogIODidUpdateNotification.UTF8String,
                                MessageJson(@"pin", pin, @"value", value));
    };
    Konashi.shared.uartRxCompleteHandler = ^(NSData *data)
    {
        UnityKonashiSendMessage(KonashiEventUartRxCompleteNotification.UTF8String,
                                [data base64EncodedStringWithOptions:0].UTF8String);
    };
    Konashi.shared.i2cReadCompleteHandler = ^(NSData *data)
    {
        UnityKonashiSendMessage(KonashiEventI2CReadCompleteNotification.UTF8String,
                                [data base64EncodedStringWithOptions:0].UTF8String);
    };
    Konashi.shared.batteryLevelDidUpdateHandler = ^(int value)
    {
        UnityKonashiSendMessage(KonashiEventBatteryLevelDidUpdateNotification.UTF8String,
                                ([NSString stringWithFormat:@"%i", value].UTF8String));
    };
    Konashi.shared.signalStrengthDidUpdateHandler = ^(int value)
    {
        UnityKonashiSendMessage(KonashiEventSignalStrengthDidUpdateNotification.UTF8String,
                                ([NSString stringWithFormat:@"%i", value].UTF8String));
    };
}

@end

#pragma mark -
#pragma mark - Unity Konashi Bridge

#define MakeStringCopy( _x_ ) ( _x_ != NULL && [_x_ isKindOfClass:[NSString class]] ) ? strdup( [_x_ UTF8String] ) : NULL
#define NSDataToBase64Char(data) MakeStringCopy([(data) base64EncodedStringWithOptions:0])

extern void UnitySendMessage(const char *, const char *, const char *);

// This is simple bridge of Konashi.m
extern "C"
{
    #pragma mark -
    #pragma mark - Konashi control public methods
    
    KonashiResult _konashi_initialize()
    {
        sharedManager = [[UnityKonashiPlugin alloc] init];
        return [Konashi initialize];
    }
    
    KonashiResult _konashi_find()
    {
        return [Konashi find];
    }
    
    KonashiResult _konashi_findWithName(const char *name)
    {
        return [Konashi findWithName:[NSString stringWithUTF8String:name]];
    }
    
    const char* _konashi_softwareRevisionString()
    {
        return MakeStringCopy([Konashi softwareRevisionString]);
    }
    
    KonashiResult _konashi_disconnect()
    {
        return [Konashi disconnect];
    }
    
    BOOL _konashi_isConnected()
    {
        return [Konashi isConnected];
    }
    
    BOOL _konashi_isReady()
    {
        return [Konashi isReady];
    }
    
    const char* _konashi_peripheralName()
    {
        return MakeStringCopy([Konashi peripheralName]);
    }
    
    #pragma mark -
    #pragma mark - Konashi PIO public methods
    
    KonashiResult _konashi_pinMode(KonashiDigitalIOPin pin, KonashiPinMode mode)
    {
        return [Konashi pinMode:pin mode:mode];
    }
    
    KonashiResult _konashi_pinModeAll(int mode)
    {
        return [Konashi pinModeAll:mode];
    }
    
    KonashiResult _konashi_digitalWrite(KonashiDigitalIOPin pin, KonashiLevel value)
    {
        return [Konashi digitalWrite:pin value:value];
    }
    
    KonashiResult _konashi_digitalWriteAll(int value)
    {
        return [Konashi digitalWriteAll:value];
    }
    
    KonashiResult _konashi_pinPullup(KonashiDigitalIOPin pin, KonashiPinMode mode)
    {
        return [Konashi pinPullup:pin mode:mode];
    }
    
    KonashiResult _konashi_pinPullupAll(int mode)
    {
        return [Konashi pinPullupAll:mode];
    }
    
    #pragma mark -
    #pragma mark - Konashi PWM public methods
    
    KonashiResult _konashi_pwmMode(KonashiDigitalIOPin pin, KonashiPWMMode mode)
    {
        return [Konashi pwmMode:pin mode:mode];
    }
    
    KonashiResult _konashi_pwmPeriod(KonashiDigitalIOPin pin, unsigned int period)
    {
        return [Konashi pwmPeriod:pin period:period];
    }
    
    KonashiResult _konashi_pwmDuty(KonashiDigitalIOPin pin, unsigned int duty)
    {
        return [Konashi pwmDuty:pin duty:duty];
    }
    
    KonashiResult _konashi_pwmLedDrive(KonashiDigitalIOPin pin, int ratio)
    {
        return [Konashi pwmLedDrive:pin dutyRatio:ratio];
    }
    
    #pragma mark -
    #pragma mark - Konashi analog IO public methods
    
    int _konashi_analogReference()
    {
        return [Konashi analogReference];
    }
    
    KonashiResult _konashi_analogReadRequest(KonashiAnalogIOPin pin)
    {
        return [Konashi analogReadRequest:pin];
    }
    
    KonashiResult _konashi_analogWrite(KonashiAnalogIOPin pin ,int milliVolt)
    {
        return [Konashi analogWrite:pin milliVolt:milliVolt];
    }
    
    #pragma mark -
    #pragma mark - Konashi I2C public methods
    
    KonashiResult _konashi_i2cMode(KonashiI2CMode mode)
    {
        return [Konashi i2cMode:mode];
    }
    
    KonashiResult _konashi_i2cStartCondition()
    {
        return [Konashi i2cStartCondition];
    }
    
    KonashiResult _konashi_i2cRestartCondition()
    {
        return [Konashi i2cRestartCondition];
    }
    
    KonashiResult _konashi_i2cStopCondition()
    {
        return [Konashi i2cStopCondition];
    }
    
    KonashiResult _konashi_i2cWriteData(const unsigned char* bytes, unsigned int length, unsigned char address)
    {
        NSData * data = [NSData dataWithBytes:bytes length:length];
        return [Konashi i2cWriteData:data address:address];
    }
    
    KonashiResult _konashi_i2cWriteString(const char * data, unsigned char address)
    {
        return [Konashi i2cWriteString:[NSString stringWithUTF8String:data] address:address];
    }
    
    KonashiResult _konashi_i2cReadRequest(int length, unsigned char address)
    {
        return [Konashi i2cReadRequest:length address:address];
    }
    
    const char* _konashi_i2cReadData()
    {
        return NSDataToBase64Char([Konashi i2cReadData]);
    }

    #pragma mark -
    #pragma mark - Konashi UART public methods
    
    KonashiResult _konashi_uartMode(KonashiUartMode mode, KonashiUartBaudrate baudrate)
    {
        return [Konashi uartMode:mode baudrate:baudrate];
    }
    
    KonashiResult _konashi_uartWriteData(const unsigned char* bytes, unsigned int length)
    {
        NSData *data = [NSData dataWithBytes:bytes length:length];
        return [Konashi uartWriteData:data];
    }
    
    KonashiResult _konashi_uartWriteString(const char * utfStr)
    {
        NSString *str = [NSString stringWithUTF8String:utfStr];
        return [Konashi uartWriteData:[str dataUsingEncoding:NSASCIIStringEncoding]];
    }
    
    const char* _konashi_readUartData()
    {
        return NSDataToBase64Char([Konashi readUartData]);
    }
    
    #pragma mark -
    #pragma mark - Konashi hardware public methods
        
    KonashiResult _konashi_reset()
    {
        return [Konashi reset];
    }
    
    KonashiResult _konashi_batteryLevelReadRequest()
    {
        return [Konashi batteryLevelReadRequest];
    }
    
    KonashiResult _konashi_signalStrengthReadRequest()
    {
        return [Konashi signalStrengthReadRequest];
    }
}