using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.DATA.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, CMSYSTEMContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using(var context = new CMSYSTEMContext())
            {
                //var orders = context.Orders
                //                    .Include(i => i.OrderItems)
                //                    .ThenInclude(i => i.Movie)
                //                    .AsQueryable();
                //if(!string.IsNullOrEmpty(userId))//user var ise biz burda filtreleme ekleyeceğiz.
                //{
                //    orders = orders.Where(i => i.UserId == userId);
                //}
                //return orders.ToList();
                List<Order> orders = new List<Order>(); 
                return orders;
            }
            
        }
    }
}
