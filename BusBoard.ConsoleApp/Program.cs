using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        private static void Main(string[] args)
        {
            //Part 1
            Console.WriteLine("Enter bus stop code");
            var stopCode = Console.ReadLine();
            GetNextBusesAtStop(stopCode);

            //Part 2
            Console.WriteLine("Enter postcode");
            var postcode = LookupPostcode(Console.ReadLine());
            GetNearestBusStops(postcode);
        }

        private static void GetNextBusesAtStop(string stopCode)
        {
            var busClient = new BusClient();
            var buses = busClient.GetBusesAtStop(stopCode, 5);

            Console.WriteLine("Next 5 buses at bus stop " + stopCode + " :");
            foreach (var bus in buses)
            {
                Console.WriteLine((bus.ConvertDate()).ToString("h:mm tt") + " " + bus.DestinationName);
            }
        }

        private static Postcode LookupPostcode(string postcode)
        {
            var client = new RestClient("https://api.postcodes.io/postcodes/");
            var request = new RestRequest(postcode);

            var response = client.Get(request);
            var result = (JObject.Parse(response.Content))["result"].ToString();
            return (JsonConvert.DeserializeObject<Postcode>(result));
        }

        private static void GetNearestBusStops(Postcode postcode)
        {
            var busClient = new BusClient();
            var busStops = busClient.GetBusCodes(postcode, "NaptanPublicBusCoachTram", 2);
            foreach (var busStop in busStops)
            {
                GetNextBusesAtStop(busStop.Id);
                Console.WriteLine("");
            }
        }
    }
}