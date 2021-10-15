using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flightmanagement.Data
{
    //Flight Data Access Object Class
    public class FlightDAO : IFlightDAO
    {
        
        readonly string connString = ConfigurationManager.ConnectionStrings["Name"].ConnectionString; //Gets connectionString from configFile App.config

        //Returns all flights information from dbo.flights table
        public IEnumerable<Flight> GetFlights()
        {
            List<Flight> flightList = new List<Flight>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.flights", conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Flight temp = new Flight(
                            reader["flight_number"].ToString(), reader["depart_date"].ToString(),
                            reader["arrival_date"].ToString(), reader["depart_time"].ToString(), 
                            reader["arrival_time"].ToString(), reader["depart_airport"].ToString(),
                            reader["arrival_airport"].ToString(), int.Parse(reader["current_capacity"].ToString()),
                            int.Parse(reader["total_capacity"].ToString())
                            );

                        temp.Id = Convert.ToInt32(reader["id"]);

                        flightList.Add(temp);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not get all the flights!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return flightList;

        }

        //Returns flight information from dbo.flights table that matches id
        public Flight GetFlight(int id)
        {
            Flight flight = new Flight();

            string query = "SELECT * FROM dbo.flights WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        flight = new Flight()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Flight_number = reader["flight_number"].ToString(),
                            Depart_date = reader["depart_date"].ToString(),
                            Arrival_date = reader["arrival_date"].ToString(),
                            Depart_time = reader["depart_time"].ToString(),
                            Arrival_time = reader["arrival_time"].ToString(),
                            Depart_airport = reader["depart_airport"].ToString(),
                            Arrival_airport = reader["arrival_airport"].ToString(),
                            Current_capacity = Convert.ToInt32(reader["current_capacity"]),
                            Total_capacity = Convert.ToInt32(reader["total_capacity"])
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not get flight of id #{0}!\n{1}", id, ex.Message);
                }
            }

            return flight;
        }

        //Adds a flight to the table dbo.flights with given information
        public void AddFlight(Flight flight)
        {
            string query = "[dbo].[AddFlight]";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@flight_number", flight.Flight_number);
                cmd.Parameters.AddWithValue("@depart_date", flight.Depart_date);
                cmd.Parameters.AddWithValue("@arrival_date", flight.Arrival_date);
                cmd.Parameters.AddWithValue("@depart_time", flight.Depart_time);
                cmd.Parameters.AddWithValue("@arrival_time", flight.Arrival_time);
                cmd.Parameters.AddWithValue("@depart_airport", flight.Depart_airport);
                cmd.Parameters.AddWithValue("@arrival_airport", flight.Arrival_airport);
                cmd.Parameters.AddWithValue("@total_capacity", flight.Total_capacity);
                cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@id"].Value;
                    flight.Id = id;
                    Console.WriteLine($"flight # {id} added");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not add flight!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Deletes flight from dbo.flights table with matching id
        public void DeleteFlight(int id)
        {

            string query = "DELETE FROM dbo.flights WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    cmd.ExecuteScalar();
                    Console.WriteLine($"flight # {id} deleted");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not delete flight!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Updates flight in dbo.flights table with given id and new information
        public void UpdateFlight(int id,Flight flight)
        {

            string query = "[dbo].[UpdateFlight]";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@flight_number", flight.Flight_number);
                cmd.Parameters.AddWithValue("@depart_date", flight.Depart_date);
                cmd.Parameters.AddWithValue("@arrival_date", flight.Arrival_date);
                cmd.Parameters.AddWithValue("@depart_time", flight.Depart_time);
                cmd.Parameters.AddWithValue("@arrival_time", flight.Arrival_time);
                cmd.Parameters.AddWithValue("@depart_airport", flight.Depart_airport);
                cmd.Parameters.AddWithValue("@arrival_airport", flight.Arrival_airport);
                cmd.Parameters.AddWithValue("@total_capacity", flight.Total_capacity);


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"flight # {id} updated");

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not update flight!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Returns int equal to remaining spots available for flight
        //matching id in dbo.flights table
        public int GetCapacity(int id)
        {
            int capacity = 0;

            string query = "SELECT total_capacity - current_capacity AS capacity FROM dbo.flights WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        capacity = Convert.ToInt32(reader["capacity"]);

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not get capacity of id #{0}!\n{1}", id, ex.Message);
                }
            }
            return capacity;
        }

        //Returns flightnumber from dbo.flights table with matching id
        public string GetNumber(int id)
        {
            string Number = "";

            string query = "SELECT flight_number FROM dbo.flights WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {

                        Number = reader["flight_number"].ToString();

                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not get flight number of id #{0}!\n{1}", id, ex.Message);
                }
            }

            return Number;
        }


    }
}
