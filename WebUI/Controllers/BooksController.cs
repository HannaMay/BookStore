using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class BooksController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 4;
        //объявляем зависимость от интерфейса
        public BooksController(IBookRepository repo)
        {
            repository = repo;
        }
        //метод который вильтуализирует представление
        public ViewResult List(string genre, int page = 1)
        {
            BooksListViewModel boookListModel = new BooksListViewModel
            {
                Books = repository.Books
                .Where(b => genre == null || b.Genre == genre)
                .OrderBy(book => book.BookId)   
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    TotalItems = genre == null ? repository.Books.Count() : repository.Books.Where(b => b.Genre == genre).Count(),
                    ItemsPerPage = pageSize
                },
                CurrentGenre = genre
            };  
            return View(boookListModel);
        }        
    }
}