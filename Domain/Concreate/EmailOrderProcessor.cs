using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace Domain.Concreate
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@gmail.ru";
        public string MailFormAddress = "bookstore@gmail.com";
        public bool UseSsl = true;
        public string UserName = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"e:\mails";          
    }

    //реализация интерфейса для обработки заказа и отпрпки данных админу
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings email)
        {
            emailSettings = email;
        }
        public void ProcessOrder(Entities.Basket basket, Entities.DeliveryDetails delivery)
        {
            using (var SmtpClient = new SmtpClient())
            {
                SmtpClient.EnableSsl = emailSettings.UseSsl;
                SmtpClient.Host = emailSettings.ServerName;
                SmtpClient.Port = emailSettings.ServerPort;
                SmtpClient.UseDefaultCredentials = false;
                SmtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    SmtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    SmtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    SmtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ обработан")
                    .AppendLine("-----")
                    .AppendLine("Товары:");

                foreach (var item in basket.GetGoods)
                {
                    var subTotal = item.Book.Price * item.Quantity;
                    body.AppendFormat("{0} x {1} (итого: {2:c}", item.Quantity, item.Book.Name, subTotal);
                }

                body.AppendFormat("Общая стоимость: {0}", basket.TotalSum())
                    .AppendLine("-----")
                    .AppendLine("Доставка:")
                    .AppendLine(delivery.FIO)
                    .AppendLine(delivery.Phone)
                    .AppendLine(delivery.Email)
                    .AppendLine(delivery.TypeDelivery)
                    .AppendLine(delivery.TypePaySystem)
                    .AppendLine("-----");

                //body.AppendFormat("Общая стоимость: {0}", basket.TotalSum())
                //    .AppendLine("-----")
                //    .AppendLine("Доставка:")
                //    .AppendLine(delivery.FIO)
                //    .AppendLine(delivery.Phone)
                //    .AppendLine(delivery.Email)
                //    .AppendLine(delivery.Basket.GetGoods.ToString())
                //    .AppendLine(delivery.TypeDelivery)
                //    .AppendLine(delivery.TypePaySystem)
                //    .AppendLine("-----");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFormAddress, 
                    emailSettings.MailToAddress,
                    "Новый заказ обработан",
                    body.ToString()
                    );

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }

                SmtpClient.Send(mailMessage);
            }
        }
    }
}
