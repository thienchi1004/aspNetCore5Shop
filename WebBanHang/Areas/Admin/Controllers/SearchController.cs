using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SearchController : Controller
	{
		private readonly dbShopContext _context;

		public SearchController(dbShopContext context)
		{
			_context = context;
		}
		//GET: Search/FindProducts
		[HttpPost]
		public IActionResult FindProducts (string keyword)
{
			List<Product> lst = new List<Product>();
			if(string.IsNullOrEmpty(keyword) || keyword.Length < 1)
			{
				return PartialView("ListProductSearchPartial", null);
			}
			lst = _context.Products
				.AsNoTracking()
				.Include(p => p.Cate)
				.Where(p => p.ProductName.Contains(keyword))
				.OrderByDescending(p => p.ProductName)
				.Take(10)
				.ToList();
			if(lst.Count == 0)
			{
				return PartialView("ListProductSearchPartial", null);
			}
			else
			{
				return PartialView("ListProductSearchPartial", lst);
			}
		}
	}
}
