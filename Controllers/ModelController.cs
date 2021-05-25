using Bike_Dealer.Models;
using Bike_Dealer.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bike_Dealer.Controllers
{
    public class ModelController : Controller
    {
        private readonly AppDbContext _db;

        [BindProperty]
        public   ViewModel viewmodel { get; set; }

        public ModelController(AppDbContext db)
        {
            _db = db;
            viewmodel = new ViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = new Models.Model()
            };

        }
        public IActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model.ToList());
        }

        public IActionResult Create()
        {
            return View(viewmodel);
        }

        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost(ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            _db.Models.Add(model.Models);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            viewmodel.Models = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.id == id);
            if (viewmodel.Models == null)
            {
                return NotFound();
            }

            return View(viewmodel);
        }
        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }

            _db.Update(viewmodel.Models);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Model model = _db.Models.Find(id);
            if (model != null)
            {
                _db.Models.Remove(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }


        [AllowAnonymous]
        [HttpGet("api/models/{MakeID}")]
        public IEnumerable<Model> Models(int MakeID)
        {
            var models = _db.Models.ToList().Where(m=>m.MakeID==MakeID);
            return models;
            
        }



    }
}

