using System;

namespace BusBoard.ConsoleApp
{
    public class Bus
    {
        public string DestinationName { get; set; }
        public string ExpectedArrival { get; set; }

        public DateTime convertDate()
        {
            return Convert.ToDateTime(ExpectedArrival);
        }
    }
}