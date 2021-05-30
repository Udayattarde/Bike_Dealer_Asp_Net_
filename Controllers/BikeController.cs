using Bike_Dealer.Models;
using Bike_Dealer.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using cloudscribe.Pagination.Models;

namespace Bike_Dealer.Controllers
{
    public class BikeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment environment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }

        public BikeController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            this.environment = environment;

            BikeVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike =new Models.Bike()
            };

        }
        public IActionResult Index(string searchString, string priceSort,int pageNumber=1,int pageSize=2)
          
        {
            ViewBag.Search = searchString;
            ViewBag.currentSort = priceSort;
            ViewBag.priceSorting=String.IsNullOrEmpty(priceSort) ? "price_Desc" : "";
            var count = (pageSize * pageNumber) -pageSize;
          var bikemodel = from b in _db.Bikes.Include(m => m.Make).Include(m=>m.Model) 
                          select b;

            var bikecount = bikemodel.Count();

            if(!String.IsNullOrEmpty(searchString))
            {
                bikemodel = bikemodel.Where(b => b.Make.Name.Contains(searchString));
                bikecount = bikemodel.Count();
               

            }

            switch (priceSort)
            {
                case "price_Desc":
                    bikemodel = bikemodel.OrderByDescending(b=>b.Price);
                    break;
                default:
                    bikemodel = bikemodel.OrderBy(b=>b.Price);
                    break;
            }


            bikemodel = bikemodel.Skip(count).Take(pageSize);
                             

            var result = new PagedResult<Bike>
            { 
                Data=bikemodel.AsNoTracking().ToList(),
                TotalItems=bikecount,
                PageNumber=pageNumber,
                PageSize=pageSize
            };

            return View(result);
        }

        public IActionResult Create()
        {
            return View(BikeVM);
        }

        [HttpPost, ActionName("Create")]
        public IActionResult CreatePost()
        {
            if (!ModelState.IsValid)
           
            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }
          
            
            _db.Bikes.Add(BikeVM.Bike);
           
            UploadImageIfAvailable();
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void UploadImageIfAvailable( )
        {
            var BikeID = BikeVM.Bike.Id;

            //Get wwrootPath to save the file on server
            string wwrootPath = environment.WebRootPath;

            //Get the Uploaded files
            var files = HttpContext.Request.Form.Files;

            //Get the reference of DBSet for the bike we have saved in our database
            var SavedBike = _db.Bikes.Find(BikeID);
            

            //Upload the file on server and save the path in database if user have submitted file
            if (files.Count != 0)
            {
                string folder = @"images\Bike\";
                //Extract the extension of submitted file
                var Extension = Path.GetExtension(files[0].FileName);

                //Create the relative image path to be saved in database table 
                var RelativeImagePath = folder + BikeID + Extension;

                //Create absolute image path to upload the physical file on server
                var AbsImagePath = Path.Combine(wwrootPath, RelativeImagePath);


                //Upload the file on server using Absolute Path
                using (var filestream = new FileStream(AbsImagePath, FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                //Set the path in database
                SavedBike.ImagePath = RelativeImagePath;
                _db.SaveChanges();


            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            BikeVM.Bike = _db.Bikes.SingleOrDefault(m => m.Id == id);
            BikeVM.Models = _db.Models.Where(m => m.MakeID == BikeVM.Bike.MakeID);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }

            return View(BikeVM);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditPost()
        {
            if (!ModelState.IsValid)

            {
                BikeVM.Makes = _db.Makes.ToList();
                BikeVM.Models = _db.Models.ToList();
                return View(BikeVM);
            }


            _db.Bikes.Update(BikeVM.Bike);
            
            UploadImageIfAvailable();
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Bike model = _db.Bikes.Find(id);
            if (model != null)
            {
                _db.Bikes.Remove(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }



    }
}

