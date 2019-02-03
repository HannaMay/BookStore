using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Basket
    {
        private List<Goods> goodsCollection = new List<Goods>();

        public IEnumerable<Goods> GetGoods { get { return goodsCollection; } }

        public void AddItem (Book book, int quantity)
        {
            Goods good = goodsCollection.Where(b => b.Book.BookId == book.BookId).FirstOrDefault();
            if (good == null)
            {
                goodsCollection.Add(new Goods { Book = book, Quantity = quantity });
            }
            else
            {
                good.Quantity += quantity;
            }
        }

        public void RemoveItem (Book book) => goodsCollection.RemoveAll(l => l.Book.BookId == book.BookId);
        
        public decimal TotalSum () => goodsCollection.Sum(e => e.Book.Price * e.Quantity);
        
        public void ResetBasket () => goodsCollection.Clear();        
    }

    public class Goods
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
