using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Storage.Data;
using Storage.Models;

namespace Storage.Controllers
{
	public class ProductsController : Controller
	{
		private readonly StorageContext _context;

		public ProductsController(StorageContext context)
		{
			_context = context;
		}

		// GET: Products
		public async Task<IActionResult> Index()
		{
			return View(await _context.Product.ToListAsync());
		}

		// View: ProductViewModel
		public async Task<IActionResult> Product()
		{
			var model = _context.Product.Select(p => new ProductViewModel
			{
				Id = p.Id,
				Name = p.Name,
				Price = p.Price,
				Count = p.Count,
				InventoryValue = p.Price * p.Count,
			});
			return View("Product", await model.ToListAsync());
		}

		// Filter by Category
		public async Task<IActionResult> FilterByCategory(string category)
		{
			if (string.IsNullOrEmpty(category))
			{
				return View("Index", await _context.Product.ToListAsync());
			}

			var filteredProducts = _context.Product
				.Where(p => p.Category.ToLower() == category.ToLower());

			return View("Index", await filteredProducts.ToListAsync());
		}

		// Search for Product
		public async Task<IActionResult> Search(string searchField)
		{
			if (!string.IsNullOrEmpty(searchField))
			{
				var results = _context.Product.Where(p => p.Name.Contains(searchField))
					.Select(p => new Product
					{
						Id = p.Id,
						Name = p.Name,
						Price = p.Price,
						Orderdate = p.Orderdate,
						Category = p.Category,
						Shelf = p.Shelf,
						Count = p.Count,
						Description = p.Description,
					});
				return View("Index", await results.ToListAsync());
			}
			else
				return RedirectToAction(nameof(Index));
		}

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Product
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// GET: Products/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
		{
			if (ModelState.IsValid)
			{
				_context.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Product.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Orderdate,Category,Shelf,Count,Description")] Product product)
		{
			if (id != product.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(product);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.Id))
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
			return View(product);
		}

		// GET: Products/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Product
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Product.FindAsync(id);
			if (product != null)
			{
				_context.Product.Remove(product);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductExists(int id)
		{
			return _context.Product.Any(e => e.Id == id);
		}
	}
}
