using BookingApp.Models;

namespace BookingApp.Repository.RepRoom
{
    public interface IRoomRepository
    {
        Room GetRoomById(int id);
        List<Room> GetAllRoom();
        void UpdateRoom(Room room);
        void DeleteRoom(int id);
        void Create(Room room);

    }
}
