using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public class BasketModelBinder : IModelBinder
    {
        private const string sessionKey = "Basket";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Basket basket = null;

            if (controllerContext.HttpContext.Session != null)
            {
                basket = (Basket)controllerContext.HttpContext.Session[sessionKey];
            }
            if (basket == null)
            {
                basket = new Basket();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = basket;
                }
            }


            return basket;
        }
    }
}