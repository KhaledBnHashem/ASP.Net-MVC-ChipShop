using ChipOrderApp.Data;
using ChipOrderApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChipOrderApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart() { Items = new List<CartItem>() };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }
            return View(cart);
        }

        public IActionResult Add(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault();
            if (cart == null)
            {
                cart = new Cart() { Items = new List<CartItem>() };
                _context.Carts.Add(cart);
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.Product.Id == id);
            if (cartItem == null)
            {
                cartItem = new CartItem { Product = product, Quantity = 1 };
                cart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }

            cart.TotalPrice += product.Price;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product).FirstOrDefault();
            if (cart == null)
            {
                return NotFound();
            }

            var cartItem = cart.Items.FirstOrDefault(i => i.Product.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            cart.TotalPrice -= cartItem.Product.Price * cartItem.Quantity;
            cart.Items.Remove(cartItem);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }



}
