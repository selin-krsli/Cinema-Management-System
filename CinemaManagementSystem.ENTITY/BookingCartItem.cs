using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaManagementSystem.ENTITY
{
    public class BookingCartItem
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; } //Navigation Property yazmalıyım ki ilgili movieıd ilgili movie tablsoundaki hangi kayda karşılık geliyor.
        public int BookingCartId { get; set; }
        public BookingCart BookingCart { get; set; }
        public int Quantity { get; set; }
    }
}
