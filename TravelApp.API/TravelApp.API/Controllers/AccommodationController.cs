using Microsoft.AspNetCore.Mvc;
using TravelApp.API.Models;
using TravelApp.API.Services;

namespace TravelApp.API.Controllers
{
    [ApiController]
    [Route("api/accommodations")]
    public class AccommodationController : ControllerBase
    {
        private readonly AccommodationService _service;

        public AccommodationController(AccommodationService service)
        {
            _service = service;
        }

        // GET /api/accommodations/recommend?spotId=1
        [HttpGet("recommend")]
        public IActionResult GetRecommend([FromQuery] int spotId)
        {
            if (spotId <= 0)
                return BadRequest(ApiResponse<object>.Fail("올바른 spotId를 입력해주세요."));

            try
            {
                var (success, message, data) = _service.GetRecommendBySpot(spotId);

                if (!success)
                    return NotFound(ApiResponse<object>.Fail(message));

                return Ok(ApiResponse<List<AccommodationWithDistance>?>.Ok(data, message, data!.Count));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.Fail($"서버 오류: {ex.Message}"));
            }
        }
    }
}