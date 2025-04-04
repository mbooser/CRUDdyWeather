using System.Text.Json;
using CRUDdyWeather.Enums;
using CRUDdyWeather.Services;

namespace CRUDdyWeather.Models
{
    public class ClientAPI
    {
        public static string parseResponse(string json, ForcastType type, string key)
        {

            try
            {
                JsonDocument doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty(type.ToString()).GetProperty(key).ToString();

            }
            catch (JsonException e)
            {
                Console.WriteLine($"JSON parsing failed: {e.Message}");
            }
            return "Failed Parse";

        }
         /*
        static async Task Main()
        {



            using HttpClient client = new HttpClient();
            ForcastType type = ForcastType.Current;
            string url = UrlCaller.urlBuilder(40.8612, 79.8953, type);
            Console.WriteLine(url);
            string json;
            try
            {
                json = await client.GetStringAsync(url);
                //Console.WriteLine(json);
                Console.WriteLine(parseResponse(json, type, "time"));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
            }



        }*/
    }

}


