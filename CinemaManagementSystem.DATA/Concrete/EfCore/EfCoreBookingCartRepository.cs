using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.ENTITY;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.DATA.Concrete.EfCore
{
    public class EfCoreBookingCartRepository : EfCoreGenericRepository<BookingCart, CMSYSTEMContext>, IBookingCartRepository
    {
        public void BookingClearCart(int bookingcartId)
        {
            using(var context = new CMSYSTEMContext())
            {
                var cmd = @"delete from BookingCartItems where BookingCartId=@p0";
                context.Database.ExecuteSqlRaw(cmd, bookingcartId);
            }
        }

        public void DeleteFromBookingCart(int cartId, int movieId)
        {
            using(var context = new CMSYSTEMContext())
            {
                var cmd = @"delete from BookingCartItems where BookingCartId=@p0 and MovieId=@p1";
                context.Database.ExecuteSqlRaw(cmd,cartId,movieId);    
            }    
        }

        public BookingCart GetByUserId(string userId)
        {
           using(var context = new CMSYSTEMContext())
            {
                return context.BookingCarts
                        .Include(i => i.BookingCartItems)
                        .ThenInclude(i => i.Movie)
                        .FirstOrDefault(i => i.UserId == userId);//bulduğu ilk kaydı bana getirsin
            }
        }
        public override void Update(BookingCart entity)
        {
            using (var context = new CMSYSTEMContext())
            {
                context.BookingCarts.Update(entity);
                context.SaveChanges();
            }
        }
    }
}
