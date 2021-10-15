using Flightmanagement.Data;
using Flightmanagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flightmanagement.Web.Controllers
{
    //Controller for Flight
    public class FlightsController : Controller
    {
        private readonly IFlightDAO flightDAO;
        private readonly IPassengerDAO passengerDAO;

        public FlightsController(IFlightDAO flightDao, IPassengerDAO passengerDao)
        {
            this.flightDAO = flightDao;
            this.passengerDAO = passengerDao;
        }

        //Returns Index View for Flights
        public IActionResult Index()
        {
            IEnumerable<Flight> mFlights = flightDAO.GetFlights();
            List<FlightViewModel> model = new List<FlightViewModel>();

            foreach (var flight in mFlights)
            {
                FlightViewModel temp = new FlightViewModel
                {
                    Id = flight.Id,
                    Flight_number = flight.Flight_number,
                    Depart_date = flight.Depart_date,
                    Arrival_date = flight.Arrival_date,
                    Depart_time = flight.Depart_time,
                    Arrival_time = flight.Arrival_time,
                    Depart_airport = flight.Depart_airport,
                    Arrival_airport = flight.Arrival_airport,
                    Current_capacity = flight.Current_capacity,
                    Total_capacity = flight.Total_capacity
                };

                model.Add(temp);
            }

            return View(model);
        }

        //Returns Details View for Flights
        public IActionResult Details(int id)
        {
            Flight model = flightDAO.GetFlight(id);

            return View(model);
        }

        //Returns Delete View for Flights
        public IActionResult Delete(int id)
        {

            passengerDAO.DeleteMatchingFlights(id);
            

            flightDAO.DeleteFlight(id);

            return View();
        }

        //Returns Create View for Flights
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Posts created form from Create view for Flights
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] FlightViewModel flight)
        {
            if (ModelState.IsValid)
            {
                Flight newFlight = new Flight();
                newFlight.Flight_number = flight.Flight_number;
                newFlight.Depart_date = flight.Depart_date;
                newFlight.Arrival_date = flight.Arrival_date;
                newFlight.Depart_time = flight.Depart_time;
                newFlight.Arrival_time = flight.Arrival_time;
                newFlight.Depart_airport = flight.Depart_airport;
                newFlight.Arrival_airport = flight.Arrival_airport;
                newFlight.Total_capacity = flight.Total_capacity;
                flightDAO.AddFlight(newFlight);
                return RedirectToAction("Index");
            }
            return View(flight);
        }

        //Returns Edit View for Flights
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Flight model = flightDAO.GetFlight(id);

            return View(model);
        }

        //Posts edited form from Edit view for Flights
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] FlightViewModel flight)
        {

            Flight tempFlight = flightDAO.GetFlight(id);
            if ( flight.Total_capacity < tempFlight.Current_capacity)
            {
                var ctrl = new ErrorController();
                ctrl.ControllerContext = ControllerContext;
                return ctrl.Index();
            }

            if (ModelState.IsValid)
            {
                Flight newFlight = new Flight();
                newFlight.Flight_number = flight.Flight_number;
                newFlight.Depart_date = flight.Depart_date;
                newFlight.Arrival_date = flight.Arrival_date;
                newFlight.Depart_time = flight.Depart_time;
                newFlight.Arrival_time = flight.Arrival_time;
                newFlight.Depart_airport = flight.Depart_airport;
                newFlight.Arrival_airport = flight.Arrival_airport;
                newFlight.Total_capacity = flight.Total_capacity;
                flightDAO.UpdateFlight(id, newFlight);
                return RedirectToAction("Index");
            }
            return View(flight);
        }
    }
}