using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItems { get; set; } //Общее количество товара
        public int ItemsPerPage { get; set; } //количество товара на страницу
        public int CurrentPage { get; set; } //текущая страница
        public int TotalPages //общее количество страниц
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
            }
        }
    }
}