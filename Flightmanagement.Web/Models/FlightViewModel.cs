using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flightmanagement.Web.Models
{
    //Model for Flight
    public class FlightViewModel
    {
        public int Id { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "Flight Number")]
        public string Flight_number { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Depart Date")]
        public string Depart_date { get; set; }

        [DataType(DataType.Date)]
        [Required]
        [Display(Name = "Arrival Date")]
        public string Arrival_date { get; set; }

        [DataType(DataType.Time)]
        [Required]
        [Display(Name = "Depart Time")]
        public string Depart_time { get; set; }

        [DataType(DataType.Time)]
        [Required]
        [Display(Name = "Arrival Time")]
        public string Arrival_time { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(20)]
        [Required]
        [Display(Name = "Depart Airport")]
        public string Depart_airport { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z'\s]*$")]
        [StringLength(20)]
        [Required]
        [Display(Name = "Arrival Airport")]
        public string Arrival_airport { get; set; }

        [Display(Name = "Current capacity")]
        public int Current_capacity { get; set; }

        [RegularExpression("^[0-9]*$")]
        [Range(1,2000)]
        [Required]
        [Display(Name = "Total capacity")]
        public int Total_capacity { get; set; }
    }
}
