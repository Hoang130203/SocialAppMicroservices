using IdentityMicroservice.Model;

namespace IdentityMicroservice.Repository
{
    public interface IUserRepository
    {
        User? GetUser(string username);
        User? GetUserById(string id);
        void InsertUser(User user);
        void UpdateUser(User user);
    }
}
