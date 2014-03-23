using System;
using System.Collections.Generic;

namespace Arduino_GPS_Reader
{
    public class GPSData
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime DateAndTime { get; set; }
        public decimal Altitude { get; set; } // in meters
        public decimal Course { get; set; } // in degrees
        public decimal Speed { get; set; } // in kph
        public int Satellites { get; set; }

        public GPSData()
        {
            this.Latitude = 0m;
            this.Longitude = 0m;
            this.DateAndTime = new DateTime();
            this.Altitude = 0m;
            this.Course = 0m;
            this.Speed = 0m;
            this.Satellites = 0;
        }

        public GPSData(decimal latitude, decimal longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public GPSData(decimal latitude, decimal longitude, DateTime dateAndTime, 
                       decimal altitude, decimal course, decimal speed, int satellites) 
            : this(latitude, longitude)
        {
            this.DateAndTime = dateAndTime;
            this.Altitude = altitude;
            this.Course = course;
            this.Speed = speed;
            this.Satellites = satellites;
        }

        // the flag "safe" means throw exceptions on incomplete/inaccurate data or not
        public static GPSData ParseArduinoLine(string line, bool safe = true)
        {
            try
            {
                string[] splitLine = line.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                Dictionary<string, string> gpsValues = new Dictionary<string, string>();

                foreach (var item in splitLine)
                {
                    string[] splitValues = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    gpsValues.Add(splitValues[0].Trim(), splitValues[1].Trim());
                }

                if (gpsValues["latitude"].Substring(0, 2) != "42" ||   // this should be changed, if you don't live in Bulgaria
                    gpsValues["longitude"].Substring(0, 2) != "23")
                {
                    throw new ArgumentException("Incomplete line error.");
                }

                decimal latitude, longitude, altitude, course, speed;
                latitude = longitude = altitude = course = speed = 0; // or it won't compile :)
                DateTime dateAndTime = new DateTime();
                int satellites = 0;

                if (!(decimal.TryParse(gpsValues["latitude"], out latitude) &&
                      decimal.TryParse(gpsValues["longitude"], out longitude) &&
                      decimal.TryParse(gpsValues["altitude"], out altitude) &&
                      decimal.TryParse(gpsValues["course"], out course) &&
                      decimal.TryParse(gpsValues["speed"], out speed)))
                {
                    throw new ArgumentException("Error parsing latitude, longitude, altitude, course or speed.");
                }

                if (!DateTime.TryParse(gpsValues["datetime"], out dateAndTime))
                {
                    throw new ArgumentException("Error parsing date and time.");
                }

                if (!int.TryParse(gpsValues["satellites"], out satellites))
                {
                    throw new ArgumentException("Error parsing the visible satellites count.");
                }

                return new GPSData(latitude, longitude, dateAndTime, altitude, course, speed, satellites);
            }
            catch (Exception ex)
            {
                if (safe)
                {
                    throw ex;
                }
                else
                {
                    return new GPSData();
                }
            }
        }
    }
}
