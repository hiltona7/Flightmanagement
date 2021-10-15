using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flightmanagement.Data
{
    //Interface for PassengerDAO
    public interface IPassengerDAO
    {
        public IEnumerable<Passenger> GetPassengers();

        public Passenger GetPassenger(int id);

        public void AddPassenger(Passenger passenger);

        public void DeletePassenger(int id);

        public void UpdatePassenger(int id, Passenger passenger);

        public void DeleteMatchingFlights(int id);
    }
}
