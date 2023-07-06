namespace CinemaManagementSystem.WEBUI.Models
{
    public class BookingCartModel
    {
        public int BookingCartId { get; set; }
        public List<BookingCartItemModel> BookingCartItems { get; set; }
        public decimal TotalPrice()
        {
            return BookingCartItems.Sum(i => i.Price * i.Quantity);
        }
    }
    public class BookingCartItemModel
    {
        public int BookingCartItemId { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}