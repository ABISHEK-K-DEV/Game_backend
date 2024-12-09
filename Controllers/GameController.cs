using Microsoft.AspNetCore.Mvc;
using GameAPI.Models;
using GameAPI.Services;

namespace GameAPI.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitResult([FromBody] GameResult result)
        {
            if (result == null || string.IsNullOrEmpty(result.PlayerName))
            {
                return BadRequest(new { error = "Invalid data received." });
            }

            try
            {
                await _gameService.CreateAsync(result);
                return Ok(new { message = "Result submitted successfully." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { error = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard()
        {
            try
            {
                var leaderboard = await _gameService.GetLeaderboardAsync();
                return Ok(leaderboard);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error while fetching leaderboard.");
            }
        }
    }
}