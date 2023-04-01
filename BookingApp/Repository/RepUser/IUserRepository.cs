using BookingApp.Models;

namespace BookingApp.Repository.RepUser
{
    public interface IUserRepository
    {
        User Register(User user);
        User Login(User user);

    }
}
