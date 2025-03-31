using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Models.ViewModels
{
	public class ProductVM
	{
// ProductVM؟
//عند إنشاء أو تعديل منتج في صفحة الـ View(form)، حيث تحتاج:
//بيانات المنتج نفسه.
//قائمة الفئات لتظهر في الـ Dropdown.

		public Product Product { get; set; } = new Product();

		//IEnumerable<SelectListItem>: هي النوع المستخدم غالبًا لربط قائمة Dropdown في الـ View مع البيانات.
//تعني "تجاهل" هذه الخاصية من عملية التحقق(Model Validation).
//لأن هذه القائمة ليست مدخلة من المستخدم، بل يقوم السيرفر بملئها.
		[ValidateNever]
		public IEnumerable<SelectListItem> CategoryList { get; set; }

	}
}
