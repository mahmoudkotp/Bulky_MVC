﻿using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _db;

		//   لأنك عايز تسمح بقراءة من   بره الكلاس، لكن ممنوع حد يعدلها غير داخل الكلاس نفسه، وده بيدي أمان للكود.لأنك عايز تسمح بقراءة
		public ICategoryRepository Category { get; private set; }
		public IProductRepository Product { get; private set; }
		public ICompanyRepository Company { get; private set; }
		public IShoppingCartRepository ShoppingCart { get; private set; }

		public IApplicationUserRepository ApplicationUser { get; private set; }

		public IOrderHeaderRepository OrderHeader { get; private set; }
		public IOrderDetailRepository OrderDetail { get; private set; }

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			ShoppingCart = new ShoppingCartRepository(_db);
			Category = new CategoryRepository(_db);
			Product = new ProductRepository(_db);
			Company = new CompanyRepository(_db);
			ApplicationUser = new ApplicationUserRepository(_db);
			OrderHeader = new OrderHeaderRepository(_db);
			OrderDetail = new OrderDetailRepository(_db); 
		}



		public void Save()
		{
			_db.SaveChanges();
		}
	}

}
