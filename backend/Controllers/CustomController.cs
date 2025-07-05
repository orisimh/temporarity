using backend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Services;
using backend.Dtos;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly FileReaderService _fileReaderService;


        public CustomController(AppDbContext db, FileReaderService fileReaderService)
        {
            _db = db;
            _fileReaderService = fileReaderService;

        }

        // Endpoint to get XML content
        [HttpGet("get-xml")]
        public async Task<IActionResult> GetXmlContent()
        {
            var res = await _fileReaderService.ParseXmlFileAsync();
            var xmlContent = await _fileReaderService.ReadXmlFileAsync();
            return Ok(xmlContent);
        }

        // Endpoint to get JSON content
        [HttpGet("get-json")]
        public async Task<IActionResult> GetJsonContent()
        {
            var jsonContent = await _fileReaderService.ReadJsonFileAsync();
            return Ok(jsonContent);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetOrders()
        {

            // Not work - cycle problem
            //var categories = await _db.Category.ToListAsync();
            //var itemsName = await _db.Order
            //                .Include(c => c.orditms)
            //                    .ThenInclude(o => o.Item)
            //                    // .ThenInclude(i => i.Name)

            //                .ToListAsync();

            var order = await _db.Order.Select(
                o => new //orderDto
                {
                    Id = o.Id,
                    //ordId = o.Id,
                    //Item = o.orditms.Select(i => i.Item).FirstOrDefault(),
                    ////ctg = o.orditms.Select(i => i.Item.Category).FirstOrDefault(),
                    //ordItemId = o.orditms.Select(i => i.Id).FirstOrDefault(),
                    //Quantity = o.orditms.Select(i => i.Quantity).FirstOrDefault(),

                    Items = o.orditms.Select(i => new
                    {
                        i.Id,
                        i.Quantity,
                        ItemId = i.Item.Id,
                        ItemName = i.Item.Name,
                        CategoryId = i.Item.Category.Id,
                        CategoryName = i.Item.Category.Name
                    })
                })
                    .ToListAsync();

            //var res = await _db.OrderItem.Select(o =>
            //new
            //{
            //    o.Order,
            //    o.Order.Id,
            //    ordID = o.Id,
            //    //o.ItemId,
            //    o.Item.Name,
            //    o.Quantity,
            //    o.Item,
            //    catName = o.Item.Category.Name,
            //    o.Item.Category
            //}

            //).ToListAsync();

            return Ok(order);
        }

        // Post: api/Order/Create
        [HttpPut("")]
        public async Task<IActionResult> Save([FromBody] orderDto ordDto)
        {

            if (ordDto == null)
                return BadRequest("No items provided.");

            // Create execution strategy to handle retries and transactions
            var strategy = _db.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _db.Database.BeginTransactionAsync();

                //try
                //{
                var order = new OrderItem()
                {

                    Id = (int)ordDto.ordItemId,
                    Item = new Item
                    {
                        Id = (int)ordDto.itemId
                    }
                };
                await _db.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(order);

                // }
            });
        }

        // Post: api/Order/Create
        //[HttpPost("create")]
        //public async Task<IActionResult> CreateOrder([FromBody] List<OrderItemDto> itemsDto)
        //{
        //    if (itemsDto == null || !itemsDto.Any())
        //        return BadRequest("No items provided.");

        //    // Create execution strategy to handle retries and transactions
        //    var strategy = _db.Database.CreateExecutionStrategy();

        //    return await strategy.ExecuteAsync(async () =>
        //    {
        //        using var transaction = await _db.Database.BeginTransactionAsync();

        //        try
        //        {
        //            var order = new Order();

        //            foreach (var dto in itemsDto)
        //            {
        //                // Validate quantity
        //                if (dto.Quantity < 1)
        //                {
        //                    throw new ArgumentException($"Invalid quantity for item '{dto.Name}'. Quantity must be at least 1.");
        //                }

        //                // Validate category exists
        //                var categoryExists = await _db.Category
        //                    .AnyAsync(c => c.Id == dto.CategoryId);

        //                if (!categoryExists)
        //                {
        //                    throw new ArgumentException($"Category with ID {dto.CategoryId} does not exist.");
        //                }

        //                // Get or create item
        //                var item = await _db.Item
        //                    .FirstOrDefaultAsync(i => i.Name == dto.Name && i.CategoryId == dto.CategoryId);

        //                if (item == null)
        //                {
        //                    item = new Item
        //                    {
        //                        Name = dto.Name,
        //                        CategoryId = dto.CategoryId
        //                    };
        //                    _db.Item.Add(item);
        //                    await _db.SaveChangesAsync(); // Save now to get ID
        //                }

        //                // Add OrderItem
        //                var orderItem = new OrderItem
        //                {
        //                    Quantity = dto.Quantity,
        //                    ItemId = item.Id
        //                };
        //                order.orditms.Add(orderItem);
        //            }

        //            _db.Order.Add(order);
        //            await _db.SaveChangesAsync();

        //            // If we reach here, everything succeeded - commit the transaction
        //            await transaction.CommitAsync();

        //            return Ok(new { order.Id });
        //        }
        //        catch (ArgumentException ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //        catch (Exception ex)
        //        {

        //            return StatusCode(500, "An error occurred while creating the order.");
        //        }
        //    });
        //}
    }
}
