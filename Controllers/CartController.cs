using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Data;
using Task.Model;

namespace Task.Controllers
{
    public class CartController : Controller
    {
        public TaskAPIContext _context;
        public CartController(TaskAPIContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("/cart")]
        public async Task<ActionResult<IEnumerable<ShoppingCart>>> GetItem()
        {
            var getCookie = Request.Cookies.ContainsKey("nurseryCart") ? Request.Cookies["nurseryCart"] : "";

            if (string.IsNullOrEmpty(getCookie))
            {
                return Json("Empty Cart");
            }

            var allItems = await _context.cart.ToListAsync();
            if(allItems == null)
            {
                return Json("Empty Cart");
            }
            return allItems;
        }

        // GET: api/Product/2
        [HttpGet("/cart/{ItemId}")]
        public IActionResult AddItem(string ItemId)
        {
            if(ItemId != null)
            {
                var availableItemCount = _context.records.Where(x => x.ItemId == ItemId).Select(y => y.AvailableStocks).FirstOrDefault();
                if(Convert.ToInt32(availableItemCount)>0)
                {
                    if (Request.Cookies.ContainsKey("nurseryCart"))
                    {
                        var cookieValue = Request.Cookies["nurseryCart"].ToString();
                        if (!string.IsNullOrEmpty(cookieValue))
                        {
                            cookieValue += "," + ItemId;
                            Response.Cookies.Delete("nurseryCart");
                            Response.Cookies.Append("nurseryCart", cookieValue);
                        }
                        else
                        {
                            Response.Cookies.Append("nurseryCart", ItemId);
                        }
                    }
                    else
                    {
                        Response.Cookies.Append("nurseryCart", ItemId);
                    }
                }
                
            }

            return RedirectToAction("GetItem");
        }
    }
}
