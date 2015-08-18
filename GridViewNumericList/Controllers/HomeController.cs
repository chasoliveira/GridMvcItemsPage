using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GridViewNumericList.Filters;
using GridViewNumericList.Models;

namespace GridViewNumericList.Controllers
{
    public class HomeController : Controller
    {
        [GridMvcAttribute]
        public ActionResult Index()
        {
            var carList = new List<Car>();
            for (int i = 1; i <= 50; i++)
            {
                carList.Add(new Car { Id = i, Model = "Model 0" + i, Year = 2010 + i });
            }
            
            return View(carList.AsQueryable().OrderBy(a=>a.Model));
        }
    }
}