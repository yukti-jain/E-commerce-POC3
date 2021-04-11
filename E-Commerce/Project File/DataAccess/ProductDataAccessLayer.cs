using eCommerce.Dto;
using eCommerce.Interfaces;
using eCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eCommerce.DataAccess
{
    public class ProductDataAccessLayer : IProductService
    {
        readonly AppDBContext _dbContext;

        public ProductDataAccessLayer(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                return _dbContext.Product.AsNoTracking().ToList();
            }
            catch
            {
                throw;
            }
        }

        public int AddProduct(Product product)
        {
            try
            {
                _dbContext.Product.Add(product);
                _dbContext.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public int UpdateProduct(Product product)
        {
            try
            {
                Product oldProductData = GetProductData(product.ProductId);

                if (oldProductData.ProductName != null)
                {
                    if (product.ProductName == null)
                    {
                        product.ProductName = oldProductData.ProductName;
                    }
                }

                _dbContext.Entry(product).State = EntityState.Modified;
                _dbContext.SaveChanges();

                return 1;
            }
            catch
            {
                throw;
            }
        }

        public Product GetProductData(int productId)
        {
            try
            {
                Product product = _dbContext.Product.FirstOrDefault(x => x.ProductId == productId);
                if (product != null)
                {
                    _dbContext.Entry(product).State = EntityState.Detached;
                    return product;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public string DeleteProduct(int productId)
        {
            try
            {
                Product product = _dbContext.Product.Find(productId);
                _dbContext.Product.Remove(product);
                _dbContext.SaveChanges();

                return (product.ProductName);
            }
            catch
            {
                throw;
            }
        }

        public List<Categories> GetCategories()
        {
            List<Categories> lstCategories = new List<Categories>();
            lstCategories = (from CategoriesList in _dbContext.Categories select CategoriesList).ToList();

            return lstCategories;
        }

        public List<Product> GetSimilarProducts(int productId)
        {
            List<Product> lstProduct = new List<Product>();
            Product product = GetProductData(productId);

            lstProduct = _dbContext.Product.Where(x => x.Category == product.Category && x.ProductId != product.ProductId)
                .OrderBy(u => Guid.NewGuid())
                .Take(5)
                .ToList();
            return lstProduct;
        }

        public List<CartItemDto> GetProductsAvailableInCart(string cartID)
        {
            try
            {
                List<CartItemDto> cartItemList = new List<CartItemDto>();
                List<CartItems> cartItems = _dbContext.CartItems.Where(x => x.CartId == cartID).ToList();

                foreach (CartItems item in cartItems)
                {
                    Product product = GetProductData(item.ProductId);
                    CartItemDto objCartItem = new CartItemDto
                    {
                        Product = product,
                        Quantity = item.Quantity
                    };

                    cartItemList.Add(objCartItem);
                }
                return cartItemList;
            }
            catch
            {
                throw;
            }
        }

        public List<Product> GetProductsAvailableInWishlist(string wishlistID)
        {
            try
            {
                List<Product> wishlist = new List<Product>();
                List<WishlistItems> cartItems = _dbContext.WishlistItems.Where(x => x.WishlistId == wishlistID).ToList();

                foreach (WishlistItems item in cartItems)
                {
                    Product product = GetProductData(item.ProductId);
                    wishlist.Add(product);
                }
                return wishlist;
            }
            catch
            {
                throw;
            }
        }
    }
}
