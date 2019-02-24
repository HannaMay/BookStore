using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebUI.Controllers;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Contains_Index_All_Books()
        {
            //организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Book1"},
                new Book { BookId = 2, Name = "Book2"},
                new Book { BookId = 3, Name = "Book3"},
                new Book { BookId = 4, Name = "Book4"},
                new Book { BookId = 5, Name = "Book5"}
            });
            AdminController controller = new AdminController(mock.Object);

            //действие (act)
            List<Book> result = ((IEnumerable<Book>)controller.Index().ViewData.Model).ToList();

            //утверждение (assert)            
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual(result[0].Name, "Book1");
            Assert.AreEqual(result[1].Name, "Book2");
        }
    }
}
