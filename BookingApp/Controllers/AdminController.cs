using BookingApp.Models;
using BookingApp.Repository.RepRoom;
using Microsoft.AspNetCore.Mvc;

namespace BookingApp.Controllers
{
    public class AdminController : Controller
    {

        IRoomRepository roomRepository = null;
        public AdminController() => roomRepository = new RoomRepository();
        public ActionResult Index()
        {
            string roll = HttpContext.Session.GetString("Roll");
            if (roll == "ADMIN")
            {
                List<Models.Room> rooms = roomRepository.GetAllRoom();
                return View(rooms);
            }
            else
            {
                return View("/Views/Authentication/Error.cshtml");
            }
            
        }

        // GET: RoomController1/Details/5
        public ActionResult Details(int id)
        {
            Models.Room room;
            if (id != null)
            {
                room = roomRepository.GetRoomById(id);
                return View(room);
            }
            else
            {
                return View();
            }

        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room , IFormFile Image)
        {
            if (Image != null)
            { 
                var fileName = Path.GetFileName(Image.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/room", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await  Image.CopyToAsync(stream);
                }
                room.Image= fileName;
            }
            

            if (room != null)
            {
                Console.WriteLine(room.Image);
                roomRepository.Create(room);
            }
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomController1/Edit/5
        public ActionResult Edit(int id)
        {
            Models.Room room;
            if (id != null)
            {
                room = roomRepository.GetRoomById(id);
                return View(room);
            }
            else
            {
                return View();
            }
        }

        // POST: RoomController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Room room)
        {
            Console.WriteLine(room.Id);
            roomRepository.UpdateRoom(room);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoomController1/Delete/5
        public ActionResult Delete(int id)
        {
            Models.Room room;
            if (id != null)
            {
                room = roomRepository.GetRoomById(id);
                return View(room);
            }
            else
            {
                return View();
            }
        }

        // POST: RoomController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Room room)
        {
            Console.WriteLine(room.Id);
            roomRepository.DeleteRoom(room.Id);
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
