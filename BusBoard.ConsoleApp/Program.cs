using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace BusBoard.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new RestClient("https://api.tfl.gov.uk/StopPoint/");
            var request = new RestRequest("{id}/Arrivals");
            
            Console.WriteLine("Enter bus stop code");
            var stop = Console.ReadLine();
            request.AddParameter("id", stop);
            
            var response = client.Get(request);
            var content = JsonConvert.DeserializeObject<List<Bus>>(response.Content);
            var buses = GetBuses(content, 5);
            Console.WriteLine("Next 5 buses at bus stop " + stop +" :");
            foreach (var bus in buses)
            {
                Console.WriteLine((bus.convertDate()).ToString("h:mm tt") + " " + bus.DestinationName);
            }

        }

        private static IEnumerable<Bus> GetBuses(IEnumerable<Bus> buses, int amount)
        {
            var sortedBuses = buses.OrderBy(bus => bus.ExpectedArrival);
            return (sortedBuses.ToList()).GetRange(0, amount);

        }

    }
}