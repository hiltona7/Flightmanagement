using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Flightmanagement.Data
{
    public class PassengerDAO : IPassengerDAO
    {

        readonly string connString = ConfigurationManager.ConnectionStrings["Name"].ConnectionString; //Gets connectionString from configFile
        int tempId = 0; //Used to set flight_id to 0 if flight does not exist in dbo.flights table

        //Gets all passengers information from dbo.passengers table
        public IEnumerable<Passenger> GetPassengers()
        {
            List<Passenger> passengerList = new List<Passenger>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.passengers", conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (!reader.IsDBNull("flight_id"))
                        {
                            tempId = Convert.ToInt32(reader["flight_id"].ToString());
                        }
                        else
                        {
                            tempId = 0;
                        }
                        Passenger temp = new Passenger(
                            reader["name"].ToString(), reader["job"].ToString(),
                            reader["email"].ToString(), int.Parse(reader["age"].ToString()),
                            reader["booking_info"].ToString(), tempId
                            );

                        temp.Id = Convert.ToInt32(reader["id"]);

                        passengerList.Add(temp);
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not get all the passengers!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

            return passengerList;

        }

        //Gets passenger information from dbo.passengers table
        //with matching id
        public Passenger GetPassenger(int id)
        {
            Passenger passenger = new Passenger();

            string query = "SELECT * FROM dbo.passengers WHERE id = @id";
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
                        if (!reader.IsDBNull("flight_id"))
                        {
                            tempId = Convert.ToInt32(reader["flight_id"]);
                        }
                        else
                        {
                            tempId = 0;
                        }
                        passenger = new Passenger()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader["name"].ToString(),
                            Job = reader["job"].ToString(),
                            Email = reader["email"].ToString(),
                            Age = Convert.ToInt32(reader["age"]),
                            Booking_info = reader["booking_info"].ToString(),
                            Flight_id = tempId
                        };
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not get passenger of id #{0}!\n{1}", id, ex.Message);
                }
            }

            return passenger;
        }

        //Adds a passenger to the table dbo.passengers with given information
        public void AddPassenger(Passenger passenger)
        {
            string query = "[dbo].[AddPassenger]";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", passenger.Name);
                cmd.Parameters.AddWithValue("@job", passenger.Job);
                cmd.Parameters.AddWithValue("@email", passenger.Email);
                cmd.Parameters.AddWithValue("@age", passenger.Age);
                cmd.Parameters.AddWithValue("@booking_info", passenger.Booking_info);
                cmd.Parameters.AddWithValue("@flight_id", passenger.Flight_id);
                cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;


                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    int id = (int)cmd.Parameters["@id"].Value;
                    passenger.Id = id;
                    Console.WriteLine($"passenger # {id} added");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not add passenger!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Deletes passenger from dbo.passengers table with matching id
        public void DeletePassenger(int id)
        {

            string query = "DELETE FROM dbo.passengers WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    cmd.ExecuteScalar();
                    Console.WriteLine($"passenger # {id} deleted");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not delete passenger!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Updates passenger in dbo.passengers table with given id and new information
        public void UpdatePassenger(int id, Passenger passenger)
        {

            string query = "[dbo].[UpdatePassenger]";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", passenger.Name);
                cmd.Parameters.AddWithValue("@job", passenger.Job);
                cmd.Parameters.AddWithValue("@email", passenger.Email);
                cmd.Parameters.AddWithValue("@age", passenger.Age);
                cmd.Parameters.AddWithValue("@booking_info", passenger.Booking_info);
                cmd.Parameters.AddWithValue("@flight_id", passenger.Flight_id);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"passenger # {id} updated");

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not update passenger!\n{0}", ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        //Called whenever a flight is deleted, sets booking_info in dbo.passengers table to empty string
        //as the corresponding flight no longer exists in the dbo.flights
        public void DeleteMatchingFlights(int id)
        {
            int flights = 0;

            string query = "UPDATE dbo.passengers SET booking_info='' WHERE flight_id = @id;";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    conn.Open();
                    flights = cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Could not delete values of id #{0}!\n{1}", id, ex.Message);
                }
            }
        }
        

    }
}
