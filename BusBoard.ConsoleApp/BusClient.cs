using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace BusBoard.ConsoleApp
{
    public class BusClient
    {
        private readonly RestClient _client = new RestClient("https://api.tfl.gov.uk/StopPoint/");

        public List<Bus> GetBusesAtStop(string stopCode, int amount)
        {
            var request = new RestRequest("{id}/Arrivals");
            request.AddParameter("id", stopCode);
            var response = _client.Get(request);
            var buses = JsonConvert.DeserializeObject<List<Bus>>(response.Content);
            var sortedBuses = buses.OrderBy(bus => bus.ExpectedArrival);
            return(sortedBuses.ToList()).GetRange(0, amount);
        }
        
        public List<BusStop> GetBusCodes(Postcode postcode, string stopType, int amount)
        {
            var request = new RestRequest();
            request.AddParameter("lat", postcode.Latitude);
            request.AddParameter("lon", postcode.Longitude);
            request.AddParameter("stopTypes", stopType);

            var response = _client.Get(request);
            var result = (JObject.Parse(response.Content))["stopPoints"].ToString();
            return(JsonConvert.DeserializeObject<List<BusStop>>(result).GetRange(0, amount));
        }
    }
}