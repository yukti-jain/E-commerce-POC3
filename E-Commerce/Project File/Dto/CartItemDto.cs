using eCommerce.Models;

namespace eCommerce.Dto
{
    public class CartItemDto
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
