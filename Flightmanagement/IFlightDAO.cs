using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flightmanagement.Data
{
    //Interface for FlightDAO
    public interface IFlightDAO
    {
        public IEnumerable<Flight> GetFlights();

        public Flight GetFlight(int id);

        public void AddFlight(Flight flight);

        public void DeleteFlight(int id);

        public void UpdateFlight(int id, Flight flight);

        public int GetCapacity(int id);

        public string GetNumber(int id);
    }
}
