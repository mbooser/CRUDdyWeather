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
            output += "latitude=" + lat.ToString();
            output += "&longitude=" + lng.ToString();
            output += "&current=temperature_2m,weather_code,wind_speed_10m,relative_humidity_2m";

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

            return output;
        }

        /// <summary>
        /// Fetches the JSON data from the provided URL
        /// </summary>
        /// <param name="completeUrl"></param>
        /// <returns>JSON Object as String</returns>
        public async Task<string> FetchJSON(string completeUrl)
        {
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

        // Check and update the state in a thread-safe manner using SemaphoreSlim
        private async Task CheckAndUpdateStateAsync()
        {
            // Wait to enter the critical section
            await _semaphore.WaitAsync();

            try
            {
                string today = DateTime.UtcNow.ToString("yyyy-MM-dd");

                // If it's a new day, reset the count
                if (Date != today)
                {
                    Date = today;
                    Count = 0; // Reset count for the new day
                }
            }
            finally
            {
                // Always release the semaphore when done
                _semaphore.Release();
            }
        }
    }
}
