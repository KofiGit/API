using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Filter;
using Api.Helpers;
using Api.Models;
using Api.Services;
using Api.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ProductDbContext _database;
        private readonly IUriService _uriService;

        public ProductController(ProductDbContext context, IUriService uriService)
        {
            this._database = context;
            this._uriService = uriService;
        }

        /// <summary>
        /// Returns page of product
        /// </summary>
        /// <param name="filter">Page Filter</param>
        /// <returns>Paged Response List of product</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProducts([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await _database.Products
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _database.Products.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Product>(pagedData, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        /// <summary>
        /// Get Product by id
        /// </summary>
        /// <param name="id">Id of product we would like to get</param>
        /// <returns>Product json</returns>
        [HttpGet("{id}", Name = "GetProductById")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductById(long id)
        {
            try
            {
                var prod = await _database.FindAsync<Product>(id);

                if (prod == null)
                {
                    return NotFound();
                }
            
                return Ok(prod);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Add new product to database
        /// </summary>
        /// <param name="product">Product json</param>
        /// <returns>Added product json</returns>
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> InsertProduct(Product product)
        {
            try
            {
                await _database.AddAsync(product);
                _database.SaveChanges();
            
                return CreatedAtRoute(nameof(GetProductById), new { id = product.Id }, product);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update description of product
        /// </summary>
        /// <param name="Id">Product id you want to update</param>
        /// <param name="description">New description</param>
        /// <returns>Updated product json</returns>
        [HttpPut("data")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProductDescription(long Id, string description)
        {
            try
            {
                var prod = await _database.FindAsync<Product>(Id);

                if (prod == null)
                {
                    return NotFound();
                }
                
                prod.Description = description;
                _database.SaveChanges();

                return Ok(prod);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}