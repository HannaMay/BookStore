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
    public class BasketController : Controller
    {
        private IBookRepository repository;
        public BasketController(IBookRepository repo)
        {
            repository = repo;
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
    }
}