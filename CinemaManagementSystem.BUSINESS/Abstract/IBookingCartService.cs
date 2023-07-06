using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.BUSINESS.Abstract
{
    public interface IBookingCartService
    {
        void InitializeCart(string userId);
        BookingCart GetBookingCartByUserId(string userId);
        void BookingNow(string userId, int movieId, int quantity);
        void DeleteFromBookingCart(string userId, int movieId);
        void BookingClearCart(int bookingcartId);
    }
}
