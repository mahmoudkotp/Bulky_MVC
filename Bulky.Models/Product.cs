﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		
		[Required]
		public string Title { get; set; }

		public string Description { get; set; }

		[Required]
		public string ISBN { get; set; }

		[Required]
		public string Author { get; set; }

		[Required]
		[Display(Name = "List Price")]
		[Range(1, 1000)]
		public double ListPrice { get; set; }

		[Required]
		[Display(Name = "List 1-50")]
		[Range(1, 1000)]
		public double Price { get; set; }

		[Required]
		[Display(Name = "Price 50+")]
		[Range(1, 1000)]
		public double Price50 { get; set; }

		[Required]
		[Display(Name = "Price for 100+")]
		[Range(1, 1000)]
		public double Price100 { get; set; }

		public int CategoryId { get; set; }

		[ForeignKey("CategoryId")] // It will take from 1 to 3
		[ValidateNever]
		public Category Category { get; set; }
		[ValidateNever]
		public string ImageUrl { get; set; }


		////more then 32000
		//public int Id { get; set; }
		//public string ProductName { get; set; }
		//public string ProductDescription { get; set; }
		//public decimal Price { get; set; }
		//public short CategoryId { get; set; }
		////each product exist in one category
		//public Category Category { get; set; }
		//public string ImageUrl { get; set; }
		//public int QuantityStock { get; set; }
		//public DateTime AddedDate { get; set; }
		//public short CompanyId { get; set; }
		//public decimal Width { get; set; }
		//public decimal Height { get; set; }
		//public string ProductModel { get; set; }

	}
}
