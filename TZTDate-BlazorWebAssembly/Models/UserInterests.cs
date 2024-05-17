using MongoDB.Bson;

namespace TZTDateBlazorWebAssembly.Models
{
    public class UserInterests
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public IEnumerable<string> Interests { get; set; }
        public int UserId { get; set; }
    }
}