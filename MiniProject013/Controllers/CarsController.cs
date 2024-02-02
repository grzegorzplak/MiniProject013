using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniProject013.Models;

namespace MiniProject013.Controllers
{
    public class CarsController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _environment;

        public CarsController(Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars;
            return View(cars);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car, IFormFile? file)
        {
            if(ModelState.IsValid)
            {
                string wwwRootPath = _environment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string carPath = Path.Combine(wwwRootPath, @"images/cars");
                    if(!string.IsNullOrEmpty(car.CarImage))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, car.CarImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(carPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    car.CarImage = @"\images\cars\" + fileName;

                    _context.Add(car);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(car);
        }

        [HttpGet]
        public IActionResult Update(int? id)
        {
            Car car = _context.Cars.Find(id);
            return View(car);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Car car, IFormFile? file)
        {
            ModelState.Remove("CarImage");
            if (ModelState.IsValid)
            {
                string wwwRootPath = _environment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string carPath = Path.Combine(wwwRootPath, @"images/cars");
                    if (!string.IsNullOrEmpty(car.CarImage))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, car.CarImage.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(carPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    car.CarImage = @"\images\cars\" + fileName;
                }
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            Car car = _context.Cars.Find(id);
            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            Car? car = _context.Cars.Find(id);
            if (car == null)
            {
                return View(car);
            }
            if (!string.IsNullOrEmpty(car.CarImage))
            {
                if (System.IO.File.Exists(Path.Combine(_environment.WebRootPath, car.CarImage.TrimStart('\\'))))
                {
                    System.IO.File.Delete(Path.Combine(_environment.WebRootPath, car.CarImage.TrimStart('\\')));
                }
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
