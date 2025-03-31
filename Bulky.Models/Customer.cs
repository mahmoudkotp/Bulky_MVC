using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models
{
	public class Customer
	{
		public int Id { get; set; }
		public string CustomerFName { get; set; }
		public string CustomerSName { get; set; }
		public string CustomerEmail { get; set; } = string.Empty;
		public string CustomerPhone { get; set; } = string.Empty;
		public string CityId { get; set; }
		public DateTime RegisterDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		//Admin Or Entery Or Customer
		public string CustomerType { get; set; }

	}
}
