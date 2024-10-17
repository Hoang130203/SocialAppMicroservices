using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Middleware;

namespace IdentityMicroservice.Model
{
    public class User
    {
        public static readonly string DocumentName = nameof(User);

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public string? Salt { get; set; }

        public bool? IsMale { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }   


        public void SetPassword(string password, IEncryptor encryptor)
        {
            Salt = encryptor.GetSalt();
            Password = encryptor.GetHash(password, Salt);
        }

        public bool ValidatePassword(string password, IEncryptor encryptor)
        {
            return Password == encryptor.GetHash(password, Salt);
        }
    }
}
