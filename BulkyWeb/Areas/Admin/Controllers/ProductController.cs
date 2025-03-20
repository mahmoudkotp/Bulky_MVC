using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BulkyBook.Models.ViewModels;


namespace BulkyBookWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		// I fixed this error with changing the access modifier from internal
		// to public in the IProductRepository interface.
		private readonly IUnitOfWork _unitOfWork;

		public ProductController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
			
			return View(objProductList);
		}

		public IActionResult Upsert(int? id)
		{
			//IEnumerable<SelectListItem> CategoryList =
			// Passing to View
			//ViewBag.Category = CategoryList;
			//ViewData["CategoryList"] = CategoryList;

			ProductVM productVM = new ()
			{				
				CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString(),
				}),
				Product = new Product()
			};
			if(id == null || id == 0)
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
			
			if (ModelState.IsValid)
			{
				_unitOfWork.Product.Add(productVM.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				// Go back to the index page (Product)
				return RedirectToAction("Index");
			}
			else
			{
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
					{
						Text = u.Name,
						Value = u.Id.ToString(),
					});
				return View(productVM);
			}
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

		public IActionResult Delete(int? id)
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

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{

			Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
			if (obj == null)
			{
				return NotFound();
			}
			_unitOfWork.Product.Remove(obj);
			_unitOfWork.Save();
			TempData["success"] = "Product deleted successfully!";
			return RedirectToAction("Index");
		}
	}
}
