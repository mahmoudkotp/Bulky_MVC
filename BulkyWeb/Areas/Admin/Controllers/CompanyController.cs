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
	public class CompanyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CompanyController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();

			return View(objCompanyList);
		}

		public IActionResult Upsert(int? id)
		{

			if (id == null || id == 0)
			{
				// Create
				return View(new Company());
			}
			else
			{
				// Update
				Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
				return View(companyObj);
			}
		}

		[HttpPost]
		public IActionResult Upsert(Company companyobj)
		{

			if (ModelState.IsValid)
			{
				if(companyobj.Id == 0)
				{
					_unitOfWork.Company.Add(companyobj);
				}
				else
				{
					_unitOfWork.Company.Update(companyobj);
				}
			
				
				_unitOfWork.Save();
				TempData["success"] = "Company created successfully";
				return RedirectToAction("Index");
			}
			else 
			{
				return View(companyobj);
			}

		}


		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Company companyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
			if (companyFromDb == null)
			{
				return NotFound();
			}
			return View(companyFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Company obj)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Company.Update(obj);
				_unitOfWork.Save();
				TempData["success"] = "Company updated successfully";
				// Go back to the index page (Company)
				return RedirectToAction("Index");
			}
			return View();
		}

	
		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			List<Company> ObjCompanyList = _unitOfWork.Company.GetAll().ToList();
			return Json(new { data = ObjCompanyList });

		}
		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			Company companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
			if (companyToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			
			_unitOfWork.Company.Remove(companyToBeDeleted);
			_unitOfWork.Save();

			//List<Company> ObjCompanyList = _unitOfWork.Company.GetAll(includeProperties: "Category").ToList();
			return Json(new { success = true, message = "Delete Successful" });

		}
		#endregion
	}
}
