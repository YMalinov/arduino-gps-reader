using System;
using System.IO.Ports;

namespace Arduino_GPS_Reader
{
    class GPSTest
    {
        private const string SERIAL_PORT = "COM3";

        static void Main()
        {
            SerialPort serial = new SerialPort(SERIAL_PORT);

            serial.BaudRate = 115200;
            serial.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);

            serial.Open();

            Console.ReadLine();

            serial.Close();
        }

        private static void OnDataReceived(object sender, SerialDataReceivedEventArgs handler)
        {
            SerialPort sp = (SerialPort)sender;

            string inData = sp.ReadLine();

            var data = GPSData.ParseArduinoLine(inData);

            Console.WriteLine(data.Latitude + " " + data.Longitude + " " + data.DateAndTime + " " + data.Altitude + " " + data.Course + " " + data.Satellites + " " + data.Speed);
        }
    }
}
