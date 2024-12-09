using MongoDB.Driver;
using GameAPI.Models;

namespace GameAPI.Services
{
    public interface IGameService
    {
        Task CreateAsync(GameResult gameResult);
        Task<List<GameResult>> GetLeaderboardAsync();
    }

    public class GameService : IGameService
    {
        private readonly IMongoCollection<GameResult> _gameResults;

        public GameService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("GameDatabase");
            _gameResults = database.GetCollection<GameResult>("GameResults");
        }

        public async Task CreateAsync(GameResult gameResult)
        {
            await _gameResults.InsertOneAsync(gameResult);
        }

        public async Task<List<GameResult>> GetLeaderboardAsync()
        {
            return await _gameResults
                .Find(result => true)
                .SortBy(result => result.Guesses) // Sort by guesses (ascending)
                .ThenBy(result => result.TimeInSeconds) // Then by time (ascending)
                .Limit(10) // Limit to top 10
                .ToListAsync();
        }
    }
}