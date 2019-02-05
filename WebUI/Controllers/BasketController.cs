using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BasketController : Controller, IOrderProcessor
    {
        private IBookRepository repository;

        private IOrderProcessor orderProcessor;

        public void ProcessOrder(Basket basket, DeliveryDetails delivery)
        {
            throw new NotImplementedException();
        }

        public BasketController(IBookRepository repo, IOrderProcessor order)
        {
            repository = repo;
            orderProcessor = order;
        }

        public ViewResult Index (Basket basket, string returnUrl)
        {
            return View(new BasketIndexViewModel
            {
                Basket = basket,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToBasket(Basket basket, int bookId, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                basket.AddItem(book, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToRouteResult RemoveFromBasket (Basket basket, int bookId, string returnUrl)
        {
            Book book = repository.Books.FirstOrDefault(b => b.BookId == bookId);

            if (book != null)
            {
                basket.RemoveItem(book);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
        
        public PartialViewResult Summary(Basket basket)
        {
            return PartialView(basket);
        }

        public ViewResult Checkout()
        {
            return View(new DeliveryDetails ());
        }

        [HttpPost]
        public ViewResult Checkout(Basket basket, DeliveryDetails delivery)
        {
            if (basket.GetGoods.Count() == 0)
            {
                ModelState.AddModelError("", "Извините корзина пуста!");

            }

            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(basket, delivery);
                basket.ResetBasket();
                return View("Complited");
            }
            else
            {
                return View(new DeliveryDetails());
            }
            
        }

        public PartialViewResult CopyBasket(Basket basket, string returnUrl)
        {
            return PartialView(new BasketIndexViewModel
            {
                Basket = basket,
                ReturnUrl = returnUrl
            }); ;
        }
        
    }
}