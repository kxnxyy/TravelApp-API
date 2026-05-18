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

        // spotId로 관광지 좌표 조회
        public SpotCoord? GetSpotCoord(int spotId)
        {
            string sql = @"
                SELECT Latitude, Longitude
                FROM TouristSpot
                WHERE SpotId = @SpotId";

            using var conn = new SqlConnection(_connStr);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SpotId", spotId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new SpotCoord
                {
                    Latitude = reader.GetDouble(0),
                    Longitude = reader.GetDouble(1)
                };
            }
            return null;
        }

        // 숙소 전체 조회 (평균 별점, 리뷰수 포함)
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
                    Name = reader.IsDBNull(1) ? "" : reader.GetString(1),
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

        // 특정 숙소 평균 별점 + 리뷰 수 조회
        public (double avgRating, int reviewCount) GetRating(int accomId)
        {
            string sql = @"
                SELECT COUNT(*), 
                       ISNULL(AVG(CAST(Rating AS FLOAT)), 0)
                FROM Review
                WHERE TargetType = 'ACCOM' AND TargetId = @AccomId";

            using var conn = new SqlConnection(_connStr);
            conn.Open();
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AccomId", accomId);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                int count = reader.GetInt32(0);
                double avgRating = reader.GetDouble(1);
                return (Math.Round(avgRating, 1), count);
            }
            return (0.0, 0);
        }
    }
}