using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Suma.Authen.Entities
{
    public class RefreshToken
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonRequired]
        public string Token { get; set; }

        [BsonRequired]
        public string AccountId { get; set; }

        [BsonRequired]
        public DateTime Expired { get; set; }
    }
}
