using System;
using CRUDdyWeather.Enums;

namespace CRUDdyWeather.Services
{


    public class UrlCaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string UrlBuilder(double lat, double lng, ForcastType type)
        {
            string output = "https://api.open-meteo.com/v1/forecast?";
            output += "latitude=" + lat.ToString();
            output += "&longitude=" + lng.ToString();
            output += "&current=temperature_2m,weather_code,wind_speed_10m,relative_humidity_2m";

            switch (type)
            {
                case ForcastType.Current:
                    //Assumed Behavior
                    break;
                case ForcastType.Daily:
                    output += "&daily=weather_code,temperature_2m_max,temperature_2m_mean,wind_speed_10m_max,wind_speed_10m_min,wind_speed_10m_mean,relative_humidity_2m_mean,relative_humidity_2m_max,relative_humidity_2m_min,temperature_2m_min";
                    break;
                case ForcastType.Hourly:
                    output += "&hourly=weather_code,wind_speed_10m,temperature_2m,relative_humidity_2m";
                    break;
                default:
                    throw new ArgumentException("Failed to Parse Forcast Type");
            }

            return output;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="completeUrl"></param>
        /// <returns></returns>
        public static async Task<string> FetchJSON(string completeUrl)
        {
            
            try
            {
                using HttpClient client = new HttpClient();
                return await client.GetStringAsync(completeUrl);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                throw new HttpRequestException("Failed to Contact API\n"+e.Message);
            }
        }
    }
}
