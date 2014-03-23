Arduino-GPS-Reader
===============

An Arduino-based GPS decoder written in .NET C#. Tested with the EM-406A GPS module and an Arduino UNO r3 board. To run this do the following:


* install the Arduino IDE and Windows drivers, if you haven't already
* upload the arduino/arduino_gps.ino file into the Arduino, adding a reference to the TinyGPS library, again located in the arduino folder 
* connect the GPS's RX and TX pins to digital pins 2 and 3 on the Arduino 
* if you've hooked up the GPS module pins correctly, its LED should light up. After a while it should start blinking, which means it has a fix
* Include the .cs files in a sample project, changing the SERIAL_PORT constant in GPSTest.cs to whatever your Arduino's serial port is
* run the program

Currently, you can get the following information from the reader:
* Latitude and Longitude
* Date and time
* Altitude
* Course
* Speed
* Number of visible satellites
