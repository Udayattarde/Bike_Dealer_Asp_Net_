using Bike_Dealer.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Dealer.Controllers
{
    public class MakeController : Controller
    {

        private readonly AppDbContext _db;
        public MakeController(AppDbContext db)
        {
            _db = db;

        }
        public IActionResult MakeIndex()
        {
            return View(_db.Makes.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Make model)
        {
            if(ModelState.IsValid)
            {
                _db.Add(model);

                _db.SaveChanges();
                return RedirectToAction("MakeIndex");
            }
            return View(model);
           
        }
       
       
        public IActionResult Delete(int id)
        {
            var model = _db.Makes.Find(id);
            if(model != null)
            {
                _db.Makes.Remove(model);
                _db.SaveChanges();
                return RedirectToAction("MakeIndex");
            }
            return NotFound();


        }

  
        public IActionResult Edit(int id)
        {
            var model = _db.Makes.Find(id);
            if (model ==null)
            {
                return NotFound();
            }
            return View(model);

        }
        [HttpPost]
        public IActionResult Edit(Make model)
        {
            if (ModelState.IsValid)
            {
                _db.Update(model);

                _db.SaveChanges();
                return RedirectToAction("MakeIndex");
            }
            return View(model);

        }

    }
}
