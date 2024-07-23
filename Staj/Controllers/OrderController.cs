using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Staj.Entity;
using Staj.Models;

namespace Staj.Controllers
{
    [Authorize(Roles = "admin")]
    public class OrderController : Controller
    {
        DataContext db = new DataContext();

        // GET: Order
        public ActionResult Index()
        {
           var orders = db.Orders.Select(i=> new AdminOrderModel() 
           { 
                Id= i.Id,
                OrderNumber = i.OrderNumber,
                OrderDate = i.OrderDate,
                OrderStatus = i.OrderStatus,
                Count = i.OrderLines.Count,
                Total = i.Total
           }).OrderByDescending(i=> i.OrderDate).ToList();


            return View(orders);
        }

        public ActionResult Details(int id) 
        {
            var entity = db.Orders.Where(i => i.Id == id).Select(i => new OrderDetailsModel()
            {
                OrderId = i.Id,
                Username = i.Username,
                OrderNumber = i.OrderNumber,
                Total = i.Total,
                OrderDate = i.OrderDate,
                OrderStatus = i.OrderStatus,
                AddressTitle = i.AddressTitle,
                Address = i.Address,
                City = i.City,
                District = i.District,
                Neighborhood = i.Neighborhood,
                PostCode = i.PostCode,
                OrderLines = i.OrderLines.Select(a => new OrderLineModel()
                {
                    ProductId = a.ProductId,
                    ProductName = a.Product.Name,
                    Image = a.Product.Image,
                    Quantity = a.Quantity,
                    Price = a.Price
                }).ToList()

            }).FirstOrDefault();

            return View(entity);
        }

        public ActionResult UpdateOrderStatus(int OrderId , EnumOrderStatus OrderStatus)
        {
            var order = db.Orders.FirstOrDefault(i => i.Id == OrderId);

            if(order != null)
            {
                order.OrderStatus = OrderStatus;
                db.SaveChanges();


                return RedirectToAction("Details" , new {id = OrderId});
            }

            return RedirectToAction("Index");
        }
    }
}