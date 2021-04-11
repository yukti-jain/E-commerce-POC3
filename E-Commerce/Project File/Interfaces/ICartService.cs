namespace eCommerce.Interfaces
{
    public interface ICartService
    {
        void AddProductToCart(int userId, int productId);
        void RemoveCartItem(int userId, int productId);
        void DeleteOneCartItem(int userId, int productId);
        int GetCartItemCount(int userId);
        void MergeCart(int tempUserId, int permUserId);
        int ClearCart(int userId);
        string GetCartId(int userId);
    }
}
