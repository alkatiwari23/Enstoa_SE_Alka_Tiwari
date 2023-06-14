using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Task.Data;
using Task.Model;

namespace Task.Controllers
{
    public class CartController : Controller
    {
        private readonly TaskAPIContext _context;

        public CartController(TaskAPIContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("api/cart")]
        public ResponseModel<ShoppingCartViewModel> GetCartItems()
        {
            ResponseModel<ShoppingCartViewModel> response = new();
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SESSION_KEY")))
            {
                response.IsSuccess = false;
                response.ErrorMsg = "No items inside the cart";
                return response;
            }
            else
            {
                List<ShoppingCartViewModel> cartViewModels = new();
                var data = HttpContext.Session.GetString("SESSION_KEY").ToString();
                var models = JsonSerializer.Deserialize<List<SessionModel>>(data);
                foreach (var item in models)
                {
                    ShoppingCartViewModel model = new()
                    {
                        Itemid = item.ItemId,
                        ItemName = item.ItemName,
                        Quantity = item.Quantity
                    };
                    cartViewModels.Add(model);
                }

                response.IsSuccess = true;
                response.List = cartViewModels;
            }
            return response;
        }

        // GET: api/Product/2
        [HttpPost("/api/cart/AddItems")]
        public ResponseModel<ShoppingCartViewModel> AddItemAsync([FromBody] AddCartRequest request)
        {
            try
            {
                ResponseModel<ShoppingCartViewModel> response = new ResponseModel<ShoppingCartViewModel>();

                if (request != null)
                {
                    var product = _context.records.Where(x => x.ItemId == request.ItemId).FirstOrDefault();
                    if (product == null)
                    {
                        response.IsSuccess = false;
                        response.ErrorMsg = "Wrong Item selected";
                        return response;
                    }
                    if (product.AvailableStocks > 0)
                    {
                        if (product.AvailableStocks < request.Quantity)
                        {
                            response.IsSuccess = false;
                            response.ErrorMsg = $"Quantity must be less than Availablity. Current Availability : {product.AvailableStocks}";
                            return response;
                        }
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString("SESSION_KEY")))
                        {
                            List<SessionModel> models = new();
                            SessionModel model = new()
                            {
                                ItemId = product.ItemId,
                                Quantity = request.Quantity,
                                ItemName = product.ItemName
                            };
                            models.Add(model);
                            //var product = JsonSerializer.Serialize()
                            HttpContext.Session.SetString("SESSION_KEY", JsonSerializer.Serialize(models));
                        }
                        else
                        {
                            var data = HttpContext.Session.GetString("SESSION_KEY").ToString();
                            var models = JsonSerializer.Deserialize<List<SessionModel>>(data);
                            SessionModel model = new()
                            {
                                ItemId = product.ItemId,
                                Quantity = request.Quantity,
                                ItemName = product.ItemName
                            };
                            models.Add(model);
                            HttpContext.Session.SetString("SESSION_KEY", JsonSerializer.Serialize(models));
                        }
                        product.AvailableStocks -= request.Quantity;
                        _context.records.Update(product);
                        _context.SaveChanges();
                        response.IsSuccess = true;
                        response.data = new ShoppingCartViewModel() { Itemid = product.ItemId, Quantity = request.Quantity, ItemName = product.ItemName };
                        return response;
                    }
                    else
                    {
                        response.IsSuccess = true;
                        response.ErrorMsg = "Product out of stock";
                        return response;
                    }

                }
                else
                {
                    response.IsSuccess = false;
                    response.ErrorMsg = "Internal Server Error";
                    return response;
                }
            }
            catch (Exception ex)
            {

                ResponseModel<ShoppingCartViewModel> response = new()
                {
                    IsSuccess = false,
                    ErrorMsg = ex.Message

                };
                return response;
            }

        }
    }
}
