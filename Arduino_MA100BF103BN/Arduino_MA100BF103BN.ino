/**  
 *  Author:    Bernardo Campos Diocaretz
 *  E-Mail:    bernardo.camposdiocaretz@uts.edu.au
 *  Created:   13/10/2022
 * 
 *  (c) Copyright C3 Climate Change Cluster
 *      University of Technology Sydney
 * 
 *  Description:
 *  This arduino script is used to build an 8-channel temperature probe using 
 *  an MCP3008 ADC chip and 8x Amphenol Advanced Sensors MA100BF103BN
**/

#include <ArduinoJson.h>
#include <Adafruit_MCP3008.h>

Adafruit_MCP3008 adc;
StaticJsonDocument<256> tt_data;

void setup() {
  Serial.begin(9600);
  adc.begin(); 
}

void loop() {
  String probe_name[8] = {"A", "B", "C", "D", "E", "F", "G", "H"};
  for (int chan = 0; chan < 8; chan++) {
    tt_data[probe_name[chan]] = getTemp(getResistance(adc.readADC(chan)));
  }

  serializeJson(tt_data, Serial);
  Serial.print("\n");
  delay(1000);
}

float getResistance(float x) {
  float Vin = 5;
  float Rs = 10000;
  float Vout = (x*Vin)/1024;
  float Rt = ((Rs * Vin) / Vout) - Rs;
  return Rt;
}

float getTemp(float x) {
  float y = 0;
  float y1 = 0;
  float y2 = 0;
  float x1 = 0;
  float x2 = 0;
  // Resistance vs Temperature values for MA100BF103BN NTC Thermistor obtained from datasheet
  float temp[51] = {32739.8, 31109.2, 29569.5, 28115.0, 26740.6, 25441.4, 24212.9, 23050.9, 21951.4, 20910.8,
                    19925.5, 18992.3, 18108.2, 17270.4, 16476.1, 15722.9, 15008.5, 14330.6, 13687.1, 13076.3,
                    12496.1, 11945.0, 11421.3, 10923.4, 10450.1, 10000.0, 9571.77, 9164.26, 8776.38, 8407.07,
                    8055.35, 7720.30, 7401.03, 7096.72, 6806.60, 6529.94, 6266.03, 6014.23, 5773.93, 5544.53,
                    5325.5, 5116.3, 4916.46, 4725.49, 4542.98, 4368.49, 4201.64, 4042.05, 3889.38, 3743.29, 3603.46
                   };

  for (int i = 0; i < 51; i++) {
    if (x <= temp[i] && x > temp[i + 1]) {
      y1 = i;
      y2 = i + 1;
      x1 = temp[i];
      x2 = temp[i + 1];
      break;
    }
  }
  
  if (x1 > 0 || x2 > 0) {
    y = y1 + ((x - x1) / (x2 - x1)) * (y2 - y1);
  }
  
  return y;
}
