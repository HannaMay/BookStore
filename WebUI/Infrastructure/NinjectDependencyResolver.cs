using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using Domain.Concreate;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            //Здесь будут привязки
            //отображение  данных без БД (иммитированное хранилище)
            /*
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { Name = "Язык программирования C# 5.0 и платформа. NET 4.5",
                    Author = "Троелсен Э",
                    Description = "Освойте технологию разработки приложений .NET с помощью нового издания известного бестселлера. Охватывая как базовые, так и " +
                    "новейшие концепции платформы, эта книга призвана научить вас всем тонкостям технологии .NET.",
                    Genre = "Программирование",
                    Price = 76.00m,
                    Publisher = "Вильямс",
                    PublishYear = 2013 },
                new Book { Name = "Красное и черное",
                    Author = "Стендаль Ф",
                    Description = "Этот знаменитый роман Стендаля самим своим названием символизирует противоречивые чувства, терзающие душу главного героя " +
                    "Жюльена Сореля: рассудочный цинизм сочетается в нем с почти детской сентиментальностью.",
                    Genre = "Художественная литература",
                    Price = 14.50m,
                    Publisher = "Азбука",
                    PublishYear = 2013 },
                new Book { Name = "Пармская обитель",
                    Author = "Стендаль Ф",
                    Description = "Битва при Ватерлоо ознаменовала крах восторженных иллюзий. Напряженность событий (убийства, заключение в тюрьму, побег, " +
                    "выяснение тайны рождения и смерти), глубокие психологические переживания героев, " +
                    "ради любви идущих на предательство и приносящих в жертву самое дорогое, а также впечатляющее изображение исторических событий той эпохи " +
                    "превращают роман в несомненный шедевр мировой литературы.",
                    Genre = "Художественная литература",
                    Price = 9.43m,
                    Publisher = "Азбука",
                    PublishYear = 2016 },
                new Book { Name = "Разговорный английский для тех, кто много путешествует + 2 CD",
                    Author = "Черниховская Н.О.",
                    Description = "Методика REAL ENGLISH — это быстрый и легкий способ овладения английским языком. Изучая английский реальных жизненных ситуаций, " +
                    "запоминая и используя готовые фразы для общения, " +
                    "вы быстро и качественно заговорите на английском.",
                    Genre = "Иностранные языки",
                    Price = 12.00m,
                    Publisher = "Эксмо",
                    PublishYear = 2016 },
                new Book { Name = "ASP.NET MVC 5 с примерами на C# 5.0 для профессионалов",
                    Author = "Фримен А.",
                    Description = "Инфраструктура ASP.NET MVC 5 представляет собой последнюю версию веб-платформы ASP.NET от Microsoft. Она предлагает высокопродуктивную модель программирования, " +
                    "которая способствует построению более чистой кодовой архитектуры, обеспечивает разработку через тестирование и поддерживает повсеместную " +
                    "расширяемость в комбинации со всеми преимуществами ASP.NET.",
                    Genre = "Программирование",
                    Price = 58.90m,
                    Publisher = "Вильямс",
                    PublishYear = 2015 }
            });
            kernel.Bind<IBookRepository>().ToConstant(mock.Object);
            */

            kernel.Bind<IBookRepository>().To<EFBookRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}