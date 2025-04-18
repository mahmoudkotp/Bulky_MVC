﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
	public class ShoppingCart
	{		
		public int Id { get; set; }

		[ForeignKey("ProductId")]
		[ValidateNever]
		public int ProductId { get; set; }
		public Product Product { get; set; }

		//public short ProductQuantity { get; set; }
		[Range(1,1000, ErrorMessage = "Please Enter a value between 1 and 1000")]
		public int Count { get; set; }

		//public int CustomerId { get; set; }
		public string ApplicationUserId { get; set; }
		[ForeignKey("ApplicationUserId")]
		[ValidateNever]
		public ApplicationUser ApplicationUser { get; set; }

		//For properties that should not be persisted in the database like TotalPrice
		[NotMapped]
		public double Price { get; set; }

	}
}
