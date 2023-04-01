using BookingApp.Models;
using BookingApp.Repository.RepReservation;
using BookingApp.Repository.RepRoom;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    public class RoomController : Controller
    {
        IRoomRepository roomRepository = null;
        IReservationRepository reservationRepository = null;
        public RoomController()
        {
            roomRepository = new RoomRepository();
            reservationRepository = new ReservationRepository();
        }


        public IActionResult Room(int page, string sort, string search)
        {
            Console.WriteLine(sort);
            if (page == 0)
            {
                page = 1;
            }
           
            string userName = HttpContext.Session.GetString("Logined");

            if (userName != null)
            {
                List<Models.Room> rooms = roomRepository.GetAllRoom();
                var recordsPerPage = 6;
                var pageNumber = page;
                if (search != null)
                {
                    rooms = rooms.Where(r => r.Name.Contains(search)).ToList();
                }
                if (sort == "des")
                {
                    rooms = rooms.OrderByDescending(product => product.Price).ToList();
                }
                else
                {
                    rooms = rooms.OrderBy(product => product.Price).ToList();
                }
                var recordsForPage = rooms
                    .Skip((pageNumber - 1) * recordsPerPage)
                    .Take(recordsPerPage)
                    .ToList();

                ViewBag.Rooms = recordsForPage;
                ViewBag.Pages = rooms.Count;

                ViewBag.Page = page > rooms.Count/6 ? rooms.Count /6: page;


                return View();
            }
            else
            {
                return View("/Views/Authentication/Error.cshtml");
            }

        }
        public IActionResult RoomDetail(int id)
        {
            Console.WriteLine(id);
            ViewBag.room = roomRepository.GetRoomById(id);
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateReservation(Reservation reservation)
        {
            Console.WriteLine(reservation.CheckIn);
            Console.WriteLine(reservation.CheckOut);
            Console.WriteLine(reservation.IdRoom);
            reservation.IdUser = int.Parse(HttpContext.Session.GetString("Logined"));
            Reservation checkRoom = reservationRepository.CreateReservation(reservation);
            if(reservation.CheckIn< DateTime.Now || reservation.CheckOut < DateTime.Now)
            {
                ViewBag.Msg = "Day booking is outdate";
                ViewBag.room = roomRepository.GetRoomById(reservation.IdRoom);
                return View("/Views/Room/RoomDetail.cshtml");
            }
            else
            {
                if (checkRoom != null)
                {
                    return RedirectToAction("MyReservation", "Reservation");
                }
                else
                {
                    ViewBag.Msg = "This Room is used by others";
                    ViewBag.room = roomRepository.GetRoomById(reservation.IdRoom);
                    return View("/Views/Room/RoomDetail.cshtml");
                }
            }
           

        }
    }
}
