using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Suma.Authen.Entities
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string MobileNumber { get; set; }

        public string PasswordHash { get; set; }

        public string ProfileName { get; set; }

        public string Username { get; set; }

        public DateTime? Birthdate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; }
        
        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }
    }
}