using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class BasketTest
    {
        [TestMethod]
        public void Can_Add_New_Goods()
        {
            //Организация
            Book book1 = new Book { BookId = 1, Name = "Book1" };
            Book book2 = new Book { BookId = 2, Name = "Book2" };

            Basket basket = new Basket();

            //Действия
            basket.AddItem(book1, 1);
            basket.AddItem(book2, 1);

            List<Goods> goodscollect = basket.GetGoods.ToList();

            //Утверждение
            Assert.AreEqual(goodscollect.Count, 2);
            Assert.AreEqual(goodscollect[0].Book, book1);
            Assert.AreEqual(goodscollect[1].Book, book2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Goods()
        {
            //Организация
            Book book1 = new Book { BookId = 1, Name = "Book1" };
            Book book2 = new Book { BookId = 2, Name = "Book2" };

            Basket basket = new Basket();

            //Действия
            basket.AddItem(book1, 1);
            basket.AddItem(book2, 1);
            basket.AddItem(book1, 5);

            List<Goods> goodscollect = basket.GetGoods.OrderBy(b => b.Book.BookId).ToList();

            //Утверждение
            Assert.AreEqual(goodscollect.Count, 2);
            Assert.AreEqual(goodscollect[0].Quantity, 6);
            Assert.AreEqual(goodscollect[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Goods()
        {
            //Организация
            Book book1 = new Book { BookId = 1, Name = "Book1" };
            Book book2 = new Book { BookId = 2, Name = "Book2" };
            Book book3 = new Book { BookId = 3, Name = "Book3" };

            Basket basket = new Basket();

            //Действия
            basket.AddItem(book1, 1);
            basket.AddItem(book2, 1);
            basket.AddItem(book1, 5);
            basket.AddItem(book3, 2);

            basket.RemoveItem(book2);

            //Утверждение
            Assert.AreEqual(basket.GetGoods.Where(b => b.Book == book2).Count(), 0);
            Assert.AreEqual(basket.GetGoods.Count(), 2);
        }

        [TestMethod]
        public void Can_Sum_Price_Goods()
        {
            //Организация
            Book book1 = new Book { BookId = 1, Name = "Book1", Price = 12 };
            Book book2 = new Book { BookId = 2, Name = "Book2", Price = 20 };
            Book book3 = new Book { BookId = 3, Name = "Book3", Price = 14 };

            Basket basket = new Basket();

            //Действия
            basket.AddItem(book1, 1);
            basket.AddItem(book2, 1);
            basket.AddItem(book1, 5);
            basket.AddItem(book3, 2);

            basket.RemoveItem(book2);

            basket.TotalSum();

            //Утверждение
            Assert.AreEqual(basket.TotalSum(), 100);
        }


        [TestMethod]
        public void Can_Clear_Basket()
        {
            //Организация
            Book book1 = new Book { BookId = 1, Name = "Book1", Price = 12 };
            Book book2 = new Book { BookId = 2, Name = "Book2", Price = 20 };
            Book book3 = new Book { BookId = 3, Name = "Book3", Price = 14 };

            Basket basket = new Basket();

            //Действия
            basket.AddItem(book1, 1);
            basket.AddItem(book2, 1);
            basket.AddItem(book1, 5);
            basket.AddItem(book3, 2);

            basket.RemoveItem(book2);

            basket.ResetBasket();

            //Утверждение
            Assert.AreEqual(basket.GetGoods.Count(), 0);
        }

        [TestMethod]
        //добавление товара в корзину пользователя
        public void Can_Add_To_Basket()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1", Price = 12 }
            }.AsQueryable());

            Basket basket = new Basket();
            BasketController controller = new BasketController(mock.Object, null);

            //Действия
            controller.AddToBasket(basket, 1, null);
           
            //Утверждение
            Assert.AreEqual(basket.GetGoods.Count(), 1);
            Assert.AreEqual(basket.GetGoods.ToList()[0].Book.BookId, 1);
        }

        [TestMethod]
        //после добавление товара в корзинну - происходит перенаправление на страницу корзины
        public void Can_Redirect_To_Basket_Page()
        {
            //Организация
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1", Genre = "Genre1" }
            }.AsQueryable());

            Basket basket = new Basket();
            BasketController controller = new BasketController(mock.Object, null);

            //Действия          
            
            RedirectToRouteResult redirectToRoute = controller.AddToBasket(basket, 2, "myUrl");

            //Утверждение
            Assert.AreEqual(redirectToRoute.RouteValues["action"], "Index");
            Assert.AreEqual(redirectToRoute.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        //удаление товара из корзину пользователя
        public void Can_View_Basket_Contents()
        {
            //Организация
            Basket basket = new Basket();
            BasketController target = new BasketController(null, null);

            //Действия
            BasketIndexViewModel result = (BasketIndexViewModel)target.Index(basket, "myUrl").ViewData.Model;          
            
            //Утверждение
            Assert.AreSame(result.Basket, basket);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        
        [TestMethod]
        
        //проверка на пустую корзину пользователя
        public void Cannot_Checkout_Empty_Basket()
        {
            //Организация
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Basket basket = new Basket();
            DeliveryDetails delivery = new DeliveryDetails();

            BasketController controller = new BasketController(null, mock.Object);

            ViewResult viewResult = controller.Checkout(basket, delivery);

            //Действия
            mock.Verify(m => m.ProcessOrder(It.IsAny<Basket>(), It.IsAny<DeliveryDetails>()), Times.Never());          
            
            //Утверждение
            Assert.AreEqual("", viewResult.ViewName);
            Assert.AreEqual(false, viewResult.ViewData.ModelState.IsValid); 
        }

        [TestMethod]

        //проверка на пустую корзину пользователя
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            //Организация
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Basket basket = new Basket();
            basket.AddItem(new Book(), 1);

            BasketController controller = new BasketController(null, mock.Object);
            controller.ModelState.AddModelError("error", "error");

            ViewResult viewResult = controller.Checkout(basket, new DeliveryDetails());

            //Действия
            mock.Verify(m => m.ProcessOrder(It.IsAny<Basket>(), It.IsAny<DeliveryDetails>()), Times.Never());

            //Утверждение
            Assert.AreEqual("", viewResult.ViewName);
            Assert.AreEqual(false, viewResult.ViewData.ModelState.IsValid);
        }
        
        [TestMethod]
        public void Cannot_Checkout_Add_SubmitOrder()
        {
            //Организация
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Basket basket = new Basket();
            basket.AddItem(new Book(), 1);
            BasketController controller = new BasketController(null, mock.Object);          
            ViewResult result = controller.Checkout(basket, new DeliveryDetails());
                
            //Действия
            
            mock.Verify(m => m.ProcessOrder(It.IsAny<Basket>(), It.IsAny<DeliveryDetails>()), Times.Once());

            //Утверждение
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);              
            
        }
    }
}   
