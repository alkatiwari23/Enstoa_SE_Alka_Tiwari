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
    
    public class ProductController : Controller
    {
        public TaskAPIContext _context;
        public ProductController(TaskAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/list")]
        public ResponseModel<GetRecordResponseModel> GetProduct()
        {
            ResponseModel<GetRecordResponseModel> response = new();
            var records = _context.records.ToList();
            if (records.Count > 0)
            {
                List<GetRecordResponseModel> getRecords = new();
                foreach (var item in records)
                {
                    GetRecordResponseModel model = new()
                    {
                        ItemId = item.ItemId,
                        ItemName = item.ItemName,
                        QuantityInStock = item.AvailableStocks
                    };
                    getRecords.Add(model);
                }
                response.IsSuccess = true;
                response.List = getRecords;
            }
            else
            {
                response.IsSuccess = false;
                response.ErrorMsg = "No Record Found";
            }
            return response;
        }

        [HttpPost]
        [Route("/list")]
        public ResponseModel<Record> PostProduct([FromBody] AddNewRecordRequest request)
        {
            ResponseModel<Record> model = new();
            if (string.IsNullOrEmpty(request.ItemName))
            {
                model.IsSuccess = false;
                model.ErrorMsg = "Item name should not be null";
                return model;
            }
            if (request.QuantityInStock <= 0)
            {
                model.IsSuccess = false;
                model.ErrorMsg = "Quantity must be greater than 0";
                return model;
            }
            Record record = new()
            {
                ItemId = Guid.NewGuid().ToString(),
                ItemName = request.ItemName,
                AvailableStocks = request.QuantityInStock
            };
            _context.records.Add(record);
            _context.SaveChanges();
            model.IsSuccess = true;
            model.data = record;
            return model;
        }

        [HttpGet("/list/{ItemId}")]
        public ResponseModel<Record> GetProductById(string ItemId)
        {
            ResponseModel<Record> response = new();

            var thisRecord = _context.records.Find(ItemId);

            if (thisRecord == null)
            {
                response.ErrorMsg = "No record found";
                return response;
            }
            else
            {
                response.IsSuccess = true;
                response.data = thisRecord;
                return response;
            }
        }

        [HttpPut("/list/{ItemId}")]
        public ResponseModel<Record> PutEmployee([FromBody] UpdateRecordRequest request, string ItemId)
        {
            ResponseModel<Record> response = new();
            var detail = _context.records.Where(x => x.ItemId == ItemId).FirstOrDefault();
            if (detail != null)
            {
                detail.ItemName = request.ItemName;
                detail.AvailableStocks = request.StockInQuantity;
                _context.records.Update(detail);
                _context.SaveChanges();
                response.IsSuccess = true;
                response.data = detail;
                return response;
            }
            else
            {
                response.ErrorMsg = "No data found";
                return response;
            }
        }

        [HttpDelete("/list/{ItemId}")]
        public ResponseModel<Record> DeleteProduct(string ItemId)
        {
            ResponseModel<Record> response = new();
            var product = _context.records.Find(ItemId);
            if (product == null)
            {
                response.ErrorMsg = "No data found.";
                return response;
            }
            else
            {
                _context.records.Remove(product);
                _context.SaveChanges();
                response.IsSuccess = true;
                response.data = product;
                return response;
            }


        }
    }
}
