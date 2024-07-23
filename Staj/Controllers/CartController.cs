using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Staj.Entity;
using Staj.Identity;
using Staj.Models;

namespace Staj.Controllers
{
 
    public class CartController : Controller
    {
        
        private DataContext db = new DataContext();
       
        // GET: Cart
        public ActionResult Index()
        {
            return View(GetCart());
        }

        [Authorize]
        public ActionResult AddToCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i=>i.Id == Id);
            if (product != null)
            {
                GetCart().AddProduct(product, 1);
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);
            if (product != null)
            {
                GetCart().DeleteProduct(product);
            }

            return RedirectToAction("Index");
        }

        public Cart GetCart()
        {
            var cart = (Cart)Session["cart"];
            if(cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public PartialViewResult Summary() 
        { 
            return PartialView(GetCart());
        }

        public ActionResult Checkout() 
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ActionResult Checkout(ShippingDetails entity)
        {
            var cart = GetCart();

            if(cart.CartLines.Count == 0) 
            {
                ModelState.AddModelError("CartError" , "Sepetinizde ürün bulunmamaktadır.");
            }
            else 
            { 
                if(ModelState.IsValid)
                {
                    SaveOrder(cart , entity);
                    cart.Clear();
                    return View("Completed");
                }
                else
                {
                    return View(entity);
                }

            }

            return View();
        }

        private void SaveOrder(Cart cart, ShippingDetails entity)
        {
            var order = new Order();
            order.OrderNumber = (new Random()).Next(111111, 999999).ToString();
            order.Total = cart.Total();
            order.OrderDate = DateTime.Now;
            order.OrderStatus = EnumOrderStatus.Bekleniyor;
            order.Username = User.Identity.Name;
            order.Username = entity.Username;
            order.AddressTitle = entity.AddressTitle;
            order.Address = entity.Address;
            order.City = entity.City;
            order.District = entity.District;
            order.Neighborhood = entity.Neighborhood;
            order.PostCode = entity.PostCode;
            
            order.OrderLines = new List<OrderLine>();

            foreach (var pr in cart.CartLines) 
            {
                var orderLine = new OrderLine();
                orderLine.Quantity = pr.Quantity;
                orderLine.Price = pr.Product.Price * pr.Quantity;
                orderLine.ProductId = pr.Product.Id; 

                order.OrderLines.Add(orderLine);
            }

            db.Orders.Add(order);
            db.SaveChanges();
        }
    }
}