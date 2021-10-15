using System;
using System.ComponentModel.DataAnnotations;

namespace Flightmanagement.Data
{
    //Passenger class
    public class Passenger
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(20)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(20)]
        [Required]
        [Display(Name = "Job")]
        public string Job { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; } 

        [RegularExpression("^[0-9]*$")]
        [Range(1, 115)]
        [Required]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Booking Info")]
        public string Booking_info { get; set; }

        [RegularExpression("^[0-9]*$")]
        [Required]
        [Display(Name = "Flight id")]
        public int Flight_id { get; set; }


        public Passenger() { }

        public Passenger(string name, string job, string email, int age, string booking_info, int flight_id)
        {
            this.Name = name;
            this.Job = job;
            this.Email = email;
            this.Age = age;
            this.Booking_info = booking_info;
            this.Flight_id = flight_id;
        }

        public override string ToString()
        {
            return $"[Id: {Id} Name : {Name}, Job: {Job}, Email: {Email}, " +
                   $"Age: {Age}, Booking info: {Booking_info} Flight id: {Flight_id}]";

        }
    }
}
