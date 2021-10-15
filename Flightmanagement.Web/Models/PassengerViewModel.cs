using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flightmanagement.Web.Models
{
    public class PassengerViewModel
    {
        //Model for Passenger
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

        [Display(Name = "Booking info")]
        public string Booking_info { get; set; }

        [RegularExpression("^[0-9]*$")]
        [Required]
        [Display(Name = "Flight id")]
        public int Flight_id { get; set; }
    }
}
