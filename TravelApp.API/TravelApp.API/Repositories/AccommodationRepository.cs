using Microsoft.Data.SqlClient;
using TravelApp.API.Models;

namespace TravelApp.API.Repositories
{
    public class AccommodationRepository
    {
        private readonly string _connStr;

        public AccommodationRepository(string connectionString)
        {
            _connStr = connectionString;
        }

        // DB에서 숙소 전체 조회
        public List<Accommodation> GetAll()
        {
            var list = new List<Accommodation>();
            string sql = @"
                SELECT AccomId, Name, Address, AccomType, 
                       Phone, ImageUrl, Latitude, Longitude, BookingUrl
                FROM Accommodation
                WHERE Latitude IS NOT NULL AND Longitude IS NOT NULL";

            using var conn = new SqlConnection(_connStr);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Accommodation
                {
                    AccomId = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Address = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    AccomType = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Phone = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    ImageUrl = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    Latitude = reader.GetDouble(6),
                    Longitude = reader.GetDouble(7),
                    BookingUrl = reader.IsDBNull(8) ? "" : reader.GetString(8),
                });
            }
            return list;
        }
    }
}