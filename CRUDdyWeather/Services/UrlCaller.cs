using System.Text.Json;
using CRUDdyWeather.Enums;


namespace CRUDdyWeather.Services
{
    public class UrlCaller
    {
        /* Data Tacking for API Free Tier
         * Count is the amount of Calls made on the given day.
         * Limit is the Max Allowed on the free tier.
         */
        public string Date { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd");
        public int Count { get; set; } = 0;
        public int Limit { get; set; } = 10000;
        public HttpClient HttpClient { get; } = new HttpClient();

        /* SemaphoreSlim to manage the number of calls against Date and Count in further Methods
         * Limits one change at a time to avoid race conditions.
         */
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Builds URL Request from User Input
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="type"></param>
        /// <returns>Completed API URL</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string UrlBuilder(double lat, double lng, ForcastType type)
        {
            string output = "https://api.open-meteo.com/v1/forecast?";
            Console.WriteLine("URL Builder Called" + output);
            output += "latitude=" + lat.ToString();
            output += "&longitude=" + lng.ToString();
            output += "&current=temperature_2m,weather_code,wind_speed_10m,relative_humidity_2m&temperature_unit=fahrenheit";

            switch (type)
            {
                case ForcastType.Current:
                    // Assumed behavior does nothing, prevents error on default.
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
            Console.WriteLine("URL Builder Called "+output);
            return output;
        }

        /// <summary>
        /// Fetches the JSON data from the provided URL
        /// </summary>
        /// <param name="completeUrl"></param>
        /// <returns>JSON Object as String</returns>
        public async Task<string> FetchJSON(string completeUrl)
        {
            Console.WriteLine("FetchJSON Called Start");
            // Ensure thread-safe access to the state before making the request
            await CheckAndUpdateStateAsync();

            // Now, ensure that the count isn't exceeded while inside the lock
            await _semaphore.WaitAsync();

            try
            {
                // Ensure the count hasn't exceeded the limit
                if (Count >= Limit)
                {
                    throw new InvalidOperationException("Daily API request limit reached.");
                }

                // Attempt the request
                string output = await HttpClient.GetStringAsync(completeUrl);

                // Increment count safely inside the semaphore lock
                Count++;
                Console.WriteLine("FetchJSON Called " + output);
                return output;
            }
            catch (HttpRequestException e)
            {
                throw new HttpRequestException("Failed to Contact API\n" + e.Message);
            }
            finally
            {
                // Release the semaphore after the API request and increment are done
                _semaphore.Release();
            }
        }

        /// <summary>
        /// Checks Date with an async lock to protect against race conditions.
        /// </summary>
        /// <returns></returns>
        private async Task CheckAndUpdateStateAsync()
        {
            // Wait to enter the critical section
            await _semaphore.WaitAsync();

            try
            {
                string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

                // Reset Date + Count on a new day
                if (Date != today)
                {
                    Date = today;
                    Count = 0;
                }
            }
            finally
            {
                // Release async lock
                _semaphore.Release();
            }
            
        }
        /// <summary>
        /// Parses SearchEntity.DumpJSON for Field/Value "KEY"
        /// ForcastType will tell which section of JSON to Search.
        /// All API Calls will request CURRENT and up to one additonal type.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns>Value of the JSON Key Pair</returns>
        public static string parseResponse(string json, ForcastType type, string key)
        {
            try
            {
                // Parse the JSON
                JsonDocument doc = JsonDocument.Parse(json);

                // Check if the specific ForcastType exists in the JSON document
                if (doc.RootElement.TryGetProperty(type.ToString().ToLower(), out JsonElement typeElement))
                {
                    // Try to get the key property, returning null if it doesn't exist
                    if (typeElement.TryGetProperty(key, out JsonElement keyElement))
                    {
                        return keyElement.ToString();  // Return the value as a string
                    }
                    else
                    {
                        Console.WriteLine($"Key '{key}' not found in the '{type}' forecast.");
                        return "Key not found";
                    }
                }
                else
                {
                    Console.WriteLine($"ForcastType '{type}' not found in the response.");
                    return "ForcastType not found";
                }
            }
            catch (JsonException e)
            {
                // Catch any JSON-specific exceptions and log the error
                Console.WriteLine($"JSON parsing failed: {e.Message}");
                return "Failed to parse JSON";
            }
            catch (Exception e)
            {
                // Catch any other exceptions that might occur
                Console.WriteLine($"Unexpected error: {e.Message}");
                return "Unexpected error occurred";
            }
        }
        /// <summary>
        /// API Returns an integer representing a type of weather.
        /// Function matches the integer to the appropriate type of weather.
        /// 
        /// Similar functionality copied over to Index.cshtml script block.
        /// </summary>
        /// <param name="responseVal"></param>
        /// <returns>Weather Description as String</returns>
        public static string ParseWeather_Code(string responseVal)
        {
            switch(int.Parse(responseVal))
            {
                case 0:
                    return "Clear Skies";
                case 1:
                    return "Mainly Clear";
                case 2:
                    return "Partly Cloudy";
                case 3:
                    return "Overcast";
                case 45:
                    return "Fog";
                case 46:
                    return "Depositing Rime Fog";
                case 51:
                    return "Drizzle Light";
                case 53:
                    return "Drizzle Moderate";
                case 55:
                    return "Drizzle Dense";
                case 56:
                    return "Freezing Drizzel Light";
                case 57:
                    return "Freezing Drizzel Dense";
                case 61:
                    return "Rain Slight";
                case 63:
                    return "Rain Moderate";
                case 65:
                    return "Rain Heavy";
                case 66:
                    return "Freezing Rain Light";
                case 67:
                    return "Freezing Rain Heavy";
                case 71:
                    return "Snow Slight";
                case 73:
                    return "Snow Moderate";
                case 75:
                    return "Snow Heavy";
                case 77:
                    return "Snow Grains";
                case 80:
                    return "Rain Showers Slight";
                case 81:
                    return "Rain Showers Moderate";
                case 82:
                    return "Rain Showers Violent";
                case 85:
                    return "Snow Showers Slight";
                case 86:
                    return "Snow Showers Heavy";
                case 95:
                    return "Thunderstorm Slight or Moderate";
                case 96:
                    return "Thunderstorm with Slight Hail";
                case 99:
                    return "Thunderstorm with Heavy Hail";
                default:
                    return "Failed to Parse Int";
            }
        }
    }
}
