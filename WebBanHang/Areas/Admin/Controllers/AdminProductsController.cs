using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using WebBanHang.Extension;
using WebBanHang.Models;

namespace WebBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminProductsController : Controller
    {
        private readonly dbShopContext _context;
        public INotyfService _notyfService { get; }

        public AdminProductsController(dbShopContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int CateID = 0)
        {
            var pageNumber = page;
            var pageSize = 20;

            List<Product> lsProducts = new List<Product>();

            if (CateID != 0)
            {
                lsProducts = _context.Products
              .AsNoTracking()
              .Where(c => c.CateId == CateID)
              .Include(c => c.Cate)
              .OrderByDescending(c => c.ProductId).ToList();
            }
            else
			{
                lsProducts = _context.Products
              .AsNoTracking()
              .Include(c => c.Cate)
              .OrderByDescending(c => c.ProductId).ToList();
            }
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentCateID = CateID;
            ViewBag.CurrentPage = pageNumber;
          
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", CateID);
            return View(models);
        }
        public IActionResult Filtter(int CateID = 0)
        {
           
            var url = $"/Admin/AdminProducts?CateID={CateID}";
            if (CateID == 0)
			{
                url = $"/Admin/AdminProducts";
            }

			return Json(new { status = "success", redirectUrl = url });
		}

        public IActionResult CheckHangTonKho(string isStock, int CateID = 0)
        {
            List<Product> lsProducts = new List<Product>();
   

            if (isStock == "all")
			{
               lsProducts = _context.Products
                   .OrderByDescending(c => c.ProductId).ToList();
            }
            else if (isStock == "inStock")
			{
                lsProducts = _context.Products
                    .Where(c => c.UnitsInStock > 0)
                    .OrderByDescending(c => c.ProductId).ToList();
            }
            else if (isStock == "outStock")
            {
                lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(c => c.UnitsInStock == 0)
                    .OrderByDescending(c => c.ProductId).ToList();
            }

            return Json("success", lsProducts);
        }




        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName");
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ShortDesc,Description,CateId,Price,Discount,Image,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Tittle,Alias,MetaDesc,MetaKey,UnitsInStock,InStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fImage)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fImage != null)
                {
                    string extension = Path.GetExtension(fImage.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extension;
                    product.Image = await Utilities.UploadFile(fImage, @"products", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.Image)) product.Image = "default.jpg";
                product.Alias = Utilities.SEOUrl(product.ProductName);
                product.DateModified = DateTime.Now;
                product.DateCreated = DateTime.Now;

                _context.Add(product);
                await _context.SaveChangesAsync();
                _notyfService.Success("Add product successful!");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ShortDesc,Description,CateId,Price,Discount,Image,Video,DateCreated,DateModified,BestSellers,HomeFlag,Active,Tags,Tittle,Alias,MetaDesc,MetaKey,UnitsInStock,InStock")] Product product, Microsoft.AspNetCore.Http.IFormFile fImage)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if (fImage != null)
                    {
                        string extension = Path.GetExtension(fImage.FileName);
                        string image = Utilities.SEOUrl(product.ProductName) + extension;
                        product.Image = await Utilities.UploadFile(fImage, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Image)) product.Image = "default.jpg";
                    product.Alias = Utilities.SEOUrl(product.ProductName);
                    product.DateModified = DateTime.Now;



                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Update product successful!");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CateId", "CateName", product.CateId);
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Cate)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _notyfService.Success("Detele product successful!");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
