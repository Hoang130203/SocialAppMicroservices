using IdentityMicroservice.Model;
using MongoDB.Driver;


namespace IdentityMicroservice.Repository
{
    public class UserRepository(IMongoDatabase db) : IUserRepository
    {
        public readonly IMongoCollection<User> _collection= db.GetCollection<User>(User.DocumentName);  
        public User? GetUser(string username)
        {
           return  _collection.Find(u => u.Username == username).FirstOrDefault();
        }

        public User? GetUserById(string id)
        {
            return _collection.Find(u => u.Id == id).FirstOrDefault();
        }

        public void InsertUser(User user)
        {
            _collection.InsertOne(user);
        }

        public void UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                .Set(u => u.Username, user.Username)
                .Set(u => u.Password, user.Password)
                .Set(u => u.Salt, user.Salt)
                .Set(u => u.IsMale, user.IsMale)
                .Set(u => u.Email, user.Email)
                .Set(u => u.FirstName, user.FirstName)
                .Set(u => u.LastName, user.LastName)
                .Set(u => u.PhoneNumber, user.PhoneNumber)
                .Set(u => u.IsAdmin, user.IsAdmin);
            _collection.UpdateOneAsync(filter, update);
        }
    }
}
