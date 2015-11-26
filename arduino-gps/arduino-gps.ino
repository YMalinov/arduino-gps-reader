#include <SoftwareSerial.h>
#include <TinyGPS.h>

// use pins 0 and 1 (respectively for rx and tx) for UART
#define RXPIN 2
#define TXPIN 3

#define TERMBAUD  115200
#define GPSBAUD  4800

TinyGPS gps;

SoftwareSerial uart_gps(RXPIN, TXPIN);

void getgps(TinyGPS &gps);

void setup()
{
  Serial.begin(TERMBAUD);
  uart_gps.begin(GPSBAUD);
}

void loop()
{
  while(uart_gps.available())
  {
      int c = uart_gps.read();
      if(gps.encode(c))      // if there is a new valid sentence...
      {
        delay(500);          // a rudimentary delay as you probably don't need GPS information every 0.1 secs
        getgps(gps);
      }
  }
}

void getgps(TinyGPS &gps)
{
  float latitude, longitude;

  gps.f_get_position(&latitude, &longitude);
  
  Serial.print("latitude="); Serial.print(latitude,6);
  Serial.print(";longitude="); Serial.print(longitude,6);
  
  int year;
  byte month, day, hour, minute, second, hundredths;
  gps.crack_datetime(&year,&month,&day,&hour,&minute,&second,&hundredths);

  Serial.print(";datetime="); Serial.print(hour, DEC); Serial.print(":"); 
  Serial.print(minute, DEC); Serial.print(":"); Serial.print(second, DEC); 
  Serial.print("."); Serial.print(hundredths, DEC);
  Serial.print(" ");
  Serial.print(day, DEC); Serial.print("/"); 
  Serial.print(month, DEC); Serial.print("/"); Serial.print(year);

  Serial.print(";altitude="); Serial.print(gps.f_altitude());  

  Serial.print(";course="); Serial.print(gps.f_course()); 

  Serial.print(";speed="); Serial.print(gps.f_speed_kmph());
  
  Serial.print(";satellites=");
  Serial.println(gps.satellites());
}
