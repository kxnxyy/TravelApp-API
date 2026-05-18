namespace TravelApp.API.Models
{
    // DB 숙소 기본 데이터
    public class Accommodation
    {
        public int AccomId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? AccomType { get; set; }
        public string? Phone { get; set; }
        public string? ImageUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? BookingUrl { get; set; }
    }

    // API 응답용 (거리 + 별점 + 리뷰수 포함)
    public class AccommodationWithDistance
    {
        public int AccommodationId { get; set; }
        public string? AccommodationName { get; set; }
        public string? AccommodationType { get; set; }
        public string? Address { get; set; }
        public string? Tel { get; set; }
        public string? ImageUrl { get; set; }
        public string? ReservationUrl { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Distance { get; set; }       // km
        public double AvgRating { get; set; }      // 평균 별점
        public int ReviewCount { get; set; }       // 리뷰 수
    }

    // 관광지 좌표용
    public class SpotCoord
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}