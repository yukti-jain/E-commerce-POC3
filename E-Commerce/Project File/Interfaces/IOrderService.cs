using eCommerce.Dto;
using System.Collections.Generic;

namespace eCommerce.Interfaces
{
    public interface IOrderService
    {
        void CreateOrder(int userId, OrdersDto orderDetails);
        List<OrdersDto> GetOrderList(int userId);
    }
}
