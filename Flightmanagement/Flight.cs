using System;
using System.ComponentModel.DataAnnotations;

namespace Flightmanagement.Data
{
    //Flight class
    public class Flight
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
        [Display(Name = "Arrival Airpot")]
        public string Arrival_airport { get; set; }

        [Display(Name = "Current Capacity")]
        public int Current_capacity { get; set; }

        [RegularExpression("^[0-9]*$")]
        [Range(1, 2000)]
        [Display(Name = "Total Capacity")]
        public int Total_capacity { get; set; }


        public Flight() { }

        public Flight(string flight_number, string depart_date, string arrival_date, string depart_time, string arrival_time, string depart_airport, string arrival_airport, int current_capacity, int total_capacity)
        {
            this.Flight_number = flight_number;
            this.Depart_date = depart_date;
            this.Arrival_date = arrival_date;
            this.Depart_time = depart_time;
            this.Arrival_time = arrival_time;
            this.Depart_airport = depart_airport;
            this.Arrival_airport = arrival_airport;
            this.Current_capacity = current_capacity;
            this.Total_capacity = total_capacity;
        }

        public override string ToString()
        {
            return $"[Flight ID: {Id}, Flight number : {Flight_number}, Depart date: {Depart_date}, Arrival date: {Arrival_date}, " +
                   $"Depart time: {Depart_time}, Arrival time {Arrival_time}, Depart airport: {Depart_airport}, " +
                   $"Arrival airport: {Arrival_airport}, Current capacity: {Current_capacity}, Total capacity: {Total_capacity}]";

        }
    }
}
