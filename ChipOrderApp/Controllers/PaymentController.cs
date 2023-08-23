using ChipOrderApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChipOrderApp.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault();
            if (cart == null)
            {
                return NotFound();
            }

            ViewBag.TotalPrice = cart.TotalPrice;
            return View();
        }

        [HttpPost]
        public IActionResult Pay(string paymentMethod)
        {
            ViewBag.PaymentMethod = paymentMethod;

            // Clear the cart after payment
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault();
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }

            return View("Confirmation");
        }
    }



}
