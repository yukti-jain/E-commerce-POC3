using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using eCommerce.Interfaces;
using eCommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace eCommerce.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        readonly IWebHostEnvironment _hostingEnvironment;
        readonly IProductService _productService;
        readonly IConfiguration _config;
        readonly string coverImageFolderPath = string.Empty;

        public ProductController(IConfiguration config, IWebHostEnvironment hostingEnvironment, IProductService productService)
        {
            _config = config;
            _productService = productService;
            _hostingEnvironment = hostingEnvironment;
            coverImageFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "Upload");
            if (!Directory.Exists(coverImageFolderPath))
            {
                Directory.CreateDirectory(coverImageFolderPath);
            }
        }

        /// <summary>
        /// Get the list of available products
        /// </summary>
        /// <returns>List of Products</returns>
        [HttpGet]
        public async Task<List<Product>> Get()
        {
            return await Task.FromResult(_productService.GetAllProducts()).ConfigureAwait(true) ;
        }

        /// <summary>
        /// Get the specific product data corresponding to the ProductId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _productService.GetProductData(id);
            if(product!=null)
            {
                return Ok(product);
            }
            return NotFound();
        }

        /// <summary>
        /// Get the list of available categories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCategoriesList")]
        public async Task<IEnumerable<Categories>> CategoryDetails()
        {
            return await Task.FromResult(_productService.GetCategories()).ConfigureAwait(true) ;
        }



        /// <summary>
        /// Add a new productrecord
        /// </summary>
        /// <returns></returns>
        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Policy = UserRoles.Admin)]
        public int Post()
        {
            Product product = JsonConvert.DeserializeObject<Product>(Request.Form["productFormData"].ToString());

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(coverImageFolderPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    product.ProductName = fileName;
                }
            }
            else
            {
                product.ProductName = _config["DefaultProductFile"];
            }
            return _productService.AddProduct(product);
        }

        /// <summary>
        /// Update a particular product record
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = UserRoles.Admin)]
        public int Put()
        {
            Product product = JsonConvert.DeserializeObject<Product>(Request.Form["productFormData"].ToString());
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    string fileName = Guid.NewGuid() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(coverImageFolderPath, fileName);
                    bool isFileExists = Directory.Exists(fullPath);

                    if (!isFileExists)
                    {
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        product.ProductName = fileName;
                    }
                }
            }
            return _productService.UpdateProduct(product);
        }

        /// <summary>
        /// Delete a particular product record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = UserRoles.Admin)]
        public int Delete(int id)
        {
            string ProductName = _productService.DeleteProduct(id);
            if (ProductName != _config["DefaultProductFile"])
            {
                string fullPath = Path.Combine(coverImageFolderPath, ProductName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            return 1;
        }
    }
}
