using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;



namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.Role_Admin)]
	public class ProductController : Controller
	{
		// I fixed this error with changing the access modifier from internal
		// to public in the IProductRepository interface.
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();

			return View(objProductList);
		}

		public IActionResult Upsert(int? id)
		{

			ProductVM productVM = new()
			{
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				}),
				Product = new Product()
			};


			if (id == null || id == 0)
			{
				// This will be true for (Insert/Create)
				return View(productVM);
			}
			else
			{
				// This will be true for (Update)
				productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
				return View(productVM);
			}
		}

		[HttpPost]
		public IActionResult Upsert(ProductVM productVM, IFormFile? file)
		{
			if (productVM == null)
			{
				throw new ArgumentNullException(nameof(productVM));
			}

			if (productVM.Product == null)
			{
				productVM.Product = new Product(); // Prevent NullReferenceException
			}

			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if (file != null)
				{
					string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath, "images/product");

					// Ensure directory exists
					if (!Directory.Exists(productPath))
					{
						Directory.CreateDirectory(productPath);
					}

					if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
					{
						// Delete old image
						var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('/'));
						if (System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}

					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream);
					}
					productVM.Product.ImageUrl = "/images/product/" + fileName;
				}

				if (productVM.Product.Id == 0)
				{
					_unitOfWork.Product.Add(productVM.Product);
				}
				else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}

				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index");
			}

			productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString(),
			});

			return View(productVM);
		}


		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
			if (productFromDb == null)
			{
				return NotFound();
			}
			return View(productFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Product obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Product updated successfully";
				// Go back to the index page (Product)
				return RedirectToAction("Index");
			}
			return View();
		}

		//public IActionResult Delete(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}
		//	Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
		//	if (productFromDb == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(productFromDb);
		//}

		//[HttpPost, ActionName("Delete")]
		//public IActionResult DeletePost(int? id)
		//{

		//	Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
		//	if (obj == null)
		//	{
		//		return NotFound();
		//	}
		//	_unitOfWork.Product.Remove(obj);
		//	_unitOfWork.Save();
		//	TempData["success"] = "Product deleted successfully!";
		//	return RedirectToAction("Index");
		//}

		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<Product> ObjProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { data = ObjProductList });

		}
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			Product productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
			if (productToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,productToBeDeleted
				.ImageUrl.TrimStart('\\'));
			
			if (System.IO.File.Exists(oldImagePath))
			{
				System.IO.File.Delete(oldImagePath);
			}

			_unitOfWork.Product.Remove(productToBeDeleted);
			_unitOfWork.Save();

			//List<Product> ObjProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { success = true, message = "Delete Successful" });

		}
		#endregion
	}
}
