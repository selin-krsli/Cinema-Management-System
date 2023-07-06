using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.ENTITY
{
    public class BookingCart
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<BookingCartItem> BookingCartItems { get; set; }
        //herhangi bir userId ile ilgili kullanıcının kartına ulaşacaz, ve kartına ulaştığımızda
        //o kartın içeriisnde hangi ürünler var dediğimizde direkt bookingitem ile ilgili beş ürünse o ürünlere götürecek.
    }
}
