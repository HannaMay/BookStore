using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DeliveryDetails
    {
        public Basket Basket { get; set; }
        //personal data
        [Required(ErrorMessage = "Пожалуйста укажите ваше Ф.И.О.")]
        public string FIO { get; set; }
        [Required(ErrorMessage = "Пожалуйста ваш e-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пожалуйста ваш телефон")]
        public string Phone { get; set; }

        //shipping data
        public string TypeDelivery { get; set; }
        public string TypePaySystem { get; set; }

        //addition
        public string Comment { get; set; }
        

    }
}
