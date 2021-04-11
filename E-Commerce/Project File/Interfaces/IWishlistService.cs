namespace eCommerce.Interfaces
{
   public interface IWishlistService
    {
        void ToggleWishlistItem(int userId, int productId);
        int ClearWishlist(int userId);
        string GetWishlistId(int userId);
    }
}
