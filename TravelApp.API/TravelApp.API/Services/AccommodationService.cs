using TravelApp.API.Models;
using TravelApp.API.Repositories;

namespace TravelApp.API.Services
{
    public class AccommodationService
    {
        private readonly AccommodationRepository _repo;

        public AccommodationService(AccommodationRepository repo)
        {
            _repo = repo;
        }

        // spotId 기반 거리순 숙소 추천
        public (bool success, string message, List<AccommodationWithDistance>? data)
            GetRecommendBySpot(int spotId, int top = 20)
        {
            // 1. spotId로 관광지 좌표 조회
            var coord = _repo.GetSpotCoord(spotId);
            if (coord == null)
                return (false, "해당 관광지를 찾을 수 없습니다.", null);

            // 2. 숙소 전체 조회
            var allAccoms = _repo.GetAll();
            if (!allAccoms.Any())
                return (false, "숙소 데이터가 없습니다.", null);

            // 3. 거리 계산 + 4. 정렬
            var result = allAccoms
                .Select(a =>
                {
                    // 평균 별점, 리뷰 수 조회
                    var (avgRating, reviewCount) = _repo.GetRating(a.AccomId);

                    return new AccommodationWithDistance
                    {
                        AccommodationId = a.AccomId,
                        AccommodationName = a.Name,
                        AccommodationType = a.AccomType,
                        Address = a.Address,
                        Tel = a.Phone,
                        ImageUrl = a.ImageUrl,
                        ReservationUrl = a.BookingUrl,
                        Latitude = a.Latitude,
                        Longitude = a.Longitude,
                        Distance = CalcDistance(coord.Latitude, coord.Longitude,
                                                         a.Latitude, a.Longitude),
                        AvgRating = avgRating,
                        ReviewCount = reviewCount
                    };
                })
                .OrderBy(a => a.Distance)  // 4. 오름차순 정렬
                .Take(top)                 // 5. 상위 top개만
                .ToList();

            return (true, "거리순 숙소 추천 성공", result);
        }

        // 하버사인 공식 거리 계산 (km)
        private double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            const double R = 6371;
            double dLat = ToRad(lat2 - lat1);
            double dLng = ToRad(lng2 - lng1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                     + Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2))
                     * Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return Math.Round(R * c, 1);
        }

        private double ToRad(double degree) => degree * Math.PI / 180;
    }
}