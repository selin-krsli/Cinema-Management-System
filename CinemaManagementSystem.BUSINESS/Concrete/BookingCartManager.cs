using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.ENTITY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.BUSINESS.Concrete
{
    public class BookingCartManager : IBookingCartService
    {
        private IBookingCartRepository _bookingcartRepository;
        public BookingCartManager(IBookingCartRepository bookingcartRepository)
        {
            _bookingcartRepository = bookingcartRepository;
        }

        public void InitializeCart(string userId)
        {
            _bookingcartRepository.Create(new BookingCart() { UserId = userId });
        }
        public void BookingClearCart(int bookingcartId)
        {
            _bookingcartRepository.BookingClearCart(bookingcartId);
        }

        public void BookingNow(string userId, int movieId, int quantity)
        {
            //burada iki şey yapıyoruz. Eklenmek istenen ürün sepette var mı (güncelleme)
            //ürün sepette var ve yeni kayıt oluşturma (kayıt ekleme)
            var cart = GetBookingCartByUserId(userId);
            if(cart != null)
            {
                //update
                //adding movie
                var index = cart.BookingCartItems.FindIndex(i=>i.MovieId == movieId);
                if(index<0)
                {
                    cart.BookingCartItems.Add(new BookingCartItem()
                    {
                        MovieId = movieId,
                        Quantity = quantity,
                        BookingCartId = cart.Id
                    });
                }
                else
                {
                    cart.BookingCartItems[index].Quantity += quantity;
                    //aynı üründen bir tane daha eklendiği için yeni bir cartitem kaydı değil de var olan kaydın quantitiy alanını arttırıyoruz.
                }
                _bookingcartRepository.Update(cart);
            }
        }

        public void DeleteFromBookingCart(string userId, int movieId)
        {
            var cart = GetBookingCartByUserId(userId);
            if(cart!=null)
            {
                _bookingcartRepository.DeleteFromBookingCart(cart.Id, movieId);
            }
        }

        public BookingCart GetBookingCartByUserId(string userId)
        {
            return _bookingcartRepository.GetByUserId(userId);
        }
    }
}
