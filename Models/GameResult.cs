using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameAPI.Models
{
    public class GameResult
    {
        [BsonId]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
        
        public string PlayerName { get; set; } = string.Empty;
        
        public int Guesses { get; set; }
        
        public int TimeInSeconds { get; set; }
    }
}