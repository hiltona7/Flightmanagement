using Flightmanagement.Data;
using Flightmanagement.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flightmanagement.Web.Controllers
{
    public class PassengersController : Controller
    {
        //Passenger Controller
        private readonly IPassengerDAO passengerDAO;
        private readonly IFlightDAO flightDAO;

        public PassengersController(IPassengerDAO passengerDao, IFlightDAO flightDao)
        {
            this.passengerDAO = passengerDao;
            this.flightDAO = flightDao;
        }

        //Returns Index View for Passenger
        public IActionResult Index()
        {
            IEnumerable<Passenger> mPassengers = passengerDAO.GetPassengers();
            List<PassengerViewModel> model = new List<PassengerViewModel>();

            foreach (var passenger in mPassengers)
            {
                PassengerViewModel temp = new PassengerViewModel
                {
                    Id = passenger.Id,
                    Name = passenger.Name,
                    Job = passenger.Job,
                    Email = passenger.Email,
                    Age = passenger.Age,
                    Booking_info = passenger.Booking_info,
                    Flight_id = passenger.Flight_id,
                };

                model.Add(temp);
            }

            return View(model);
        }

        //Returns Details View for Passenger
        public IActionResult Details(int id)
        {
            Passenger model = passengerDAO.GetPassenger(id);

            return View(model);
        }

        //Returns Delete View for Passenger
        public IActionResult Delete(int id)
        {
            passengerDAO.DeletePassenger(id);

            return View();
        }

        //Returns Create View for Passenger
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //Posts created form from Create view for Passengers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] PassengerViewModel passenger)
        {
            
            //If flight is full or does not exist
            //don't create and return to Index
            if(flightDAO.GetCapacity(passenger.Flight_id) == 0)
            {
                var ctrl = new ErrorController();
                ctrl.ControllerContext = ControllerContext;
                return ctrl.Index();
            }

            //If passed previous conditions and model is valid
            if (ModelState.IsValid)
            {
                Passenger newPassenger = new Passenger();
                newPassenger.Name = passenger.Name;
                newPassenger.Job = passenger.Job;
                newPassenger.Email = passenger.Email;
                newPassenger.Age = passenger.Age;
                newPassenger.Flight_id = passenger.Flight_id;
                passengerDAO.AddPassenger(newPassenger);
                newPassenger.Booking_info = flightDAO.GetNumber(passenger.Flight_id) + "-" + newPassenger.Id;
                passengerDAO.UpdatePassenger(newPassenger.Id, newPassenger);
                return RedirectToAction("Index");
            }
            return View(passenger);
        }

        //Returns Edit View for Passenger
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Passenger model = passengerDAO.GetPassenger(id);

            return View(model);
        }

        //Posts edited form from Create view for Passengers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind] int id,[Bind] PassengerViewModel passenger)
        {

            //If flight is full and passenger is not already a part of flight throw error
            if ((flightDAO.GetCapacity(passenger.Flight_id) == 0) && (flightDAO.GetFlight(passenger.Flight_id).Id != passengerDAO.GetPassenger(id).Flight_id))
            {
                var ctrl = new ErrorController();
                ctrl.ControllerContext = ControllerContext;
                return ctrl.Index();
            }

            if (ModelState.IsValid)
            {
                Passenger newPassenger = new Passenger();
                newPassenger.Name = passenger.Name;
                newPassenger.Job = passenger.Job;
                newPassenger.Email = passenger.Email;
                newPassenger.Age = passenger.Age;
                newPassenger.Flight_id = passenger.Flight_id;
                newPassenger.Booking_info = flightDAO.GetNumber(newPassenger.Flight_id) + "-" + passenger.Id;
                passengerDAO.UpdatePassenger(id, newPassenger);
                return RedirectToAction("Index");
            }
            return View(passenger);
        }
    }
}
