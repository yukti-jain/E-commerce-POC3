using eCommerce.Dto;
using eCommerce.Models;
using System.Collections.Generic;

namespace eCommerce.Interfaces
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        int AddProduct(Product product);
        int UpdateProduct(Product product);
        Product GetProductData(int productId);
        string DeleteProduct(int productId);
        List<Categories> GetCategories();
        List<Product> GetSimilarProducts(int productId);
        List<CartItemDto> GetProductsAvailableInCart(string cartId);
        List<Product> GetProductsAvailableInWishlist(string wishlistID);
    }
}
