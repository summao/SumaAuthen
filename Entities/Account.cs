using System;
using System.ComponentModel.DataAnnotations;
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

        [BsonRequired]
        [MaxLength(20)]
        public string MobileNumber { get; set; }

        [BsonRequired]
        [MaxLength(200)]
        public string PasswordHash { get; set; }

        [BsonRequired]
        [MaxLength(100)]
        public string ProfileName { get; set; }

        public DateTime? Birthdate { get; set; }

        [BsonRequired]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public Role Role { get; set; }
        
        [BsonRequired]
        public DateTime Created { get; set; }
        
        public DateTime? Updated { get; set; }
    }
}