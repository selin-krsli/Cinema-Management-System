using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.ENTITY;
using CinemaManagementSystem.WEBUI.Identity;
using CinemaManagementSystem.WEBUI.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CinemaManagementSystem.WEBUI.Controllers
{
    //[Authorize]
    public class BookingCartController : Controller
    {
        private IBookingCartService _bookingCartService;
        private UserManager<User> _userManager;
        private IOrderService _orderService;
        public BookingCartController(IBookingCartService bookingCartService, UserManager<User> userManager, IOrderService orderService)
        {
            _bookingCartService = bookingCartService;
            _orderService = orderService;
            _userManager = userManager;
            _orderService = orderService;

        }
        public IActionResult Index()
        {
            var bookingCart = _bookingCartService.GetBookingCartByUserId(_userManager.GetUserId(User));
            return View(new BookingCartModel()
            {
                BookingCartId = bookingCart.Id,
                BookingCartItems = bookingCart.BookingCartItems.Select(i=> new BookingCartItemModel()
                {
                    BookingCartItemId = i.Id,
                    MovieId = i.MovieId, 
                    Name = i.Movie.MovieName,
                    Price = (decimal)i.Movie.Price,
                    ImageUrl = i.Movie.ImageUrl,
                    Quantity = i.Quantity
                }).ToList(),

            });
        }
        [HttpPost]
        public IActionResult BookingNow(int movieId, int quantity)
        {
            var userId = _userManager.GetUserId(User);
            _bookingCartService.BookingNow(userId, movieId, quantity);
            return Redirect("/BookingCart/Index");
        }
        
        [HttpPost]
        public IActionResult DeleteFromBookingCart(int movieId)
        {
            var userId = _userManager.GetUserId(User);
            _bookingCartService.DeleteFromBookingCart(userId, movieId);
            return Redirect("/BookingCart/Index");
        }
        public IActionResult Checkout()
        {
            var cart = _bookingCartService.GetBookingCartByUserId(_userManager.GetUserId(User));

            var orderModel = new OrderModel();

            orderModel.BookingCartModel = new BookingCartModel()
            {
                BookingCartId = cart.Id,
                BookingCartItems = cart.BookingCartItems.Select(i => new BookingCartItemModel()
                {
                    BookingCartItemId = i.Id,
                    MovieId = i.MovieId,
                    Name = i.Movie.MovieName,
                    Price = i.Movie.Price,
                    ImageUrl = i.Movie.ImageUrl,
                    Quantity = i.Quantity

                }).ToList()
            };

            return View(orderModel);
        }
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {

            ModelState.Remove("BookingCartModel");
            if(ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var cart = _bookingCartService.GetBookingCartByUserId(userId);
                model.BookingCartModel = new BookingCartModel()
                {
                    BookingCartId = cart.Id,
                    BookingCartItems = cart.BookingCartItems.Select(i => new BookingCartItemModel()
                    {
                        BookingCartItemId = i.Id,
                        MovieId = i.MovieId,
                        Name = i.Movie.MovieName,
                        Price = (decimal)i.Movie.Price,
                        ImageUrl = i.Movie.ImageUrl,
                        Quantity = i.Quantity
                    }).ToList()
                };

                var payment = PaymentProcess(model);

                if (payment.Status == "success")
                { //Sipariş kaydı onaylanmışsa sipariş için diğer bilgileri de veri tabanına kayıt ediyoruz.
                    SaveOrder(model, payment, userId);//ve bunları kayıt ettikten sonra kullanıcı kartını temizlememiz gerekiyor.
                    ClearCart(model.BookingCartModel.BookingCartId);
                    return View("Success");
                }
                else
                {
                    var msg = new MessageInfo()
                    {
                        Message = $"{payment.ErrorMessage}",
                        AlertType = "danger"
                    };
                    TempData["message"] = JsonConvert.SerializeObject(msg);
                }
            }
            return View(model);
        }
        public IActionResult GetOrders()
        {
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrders(userId);

            var orderListModel = new List<OrderListModel>();
            OrderListModel orderModel;
            
            foreach(var order in orders)
            {
                orderModel = new OrderListModel();
                orderModel.OrderId = order.Id;
                orderModel.OrderNumber = order.OrderNumber;
                orderModel.OrderDate = order.OrderDate;
                orderModel.Phone = order.Phone;
                orderModel.FirstName = order.FirstName;
                orderModel.LastName = order.LastName;
                orderModel.Email = order.Email;
                orderModel.Address = order.Address;
                orderModel.City = order.City;
                orderModel.OrderState = order.OrderState;
                orderModel.PaymentType = order.PaymentType;

                orderModel.OrderItems = order.OrderItems.Select(i => new OrderItemModel()
                //orderItemdan movie tablosuna geçiş yapıyoruz.
                {
                    OrderItemId = i.Id,
                    Name = i.Movie.MovieName,
                    Price = (decimal)i.Price,
                    Quantity = i.Quantity,
                    ImageUrl = i.Movie.ImageUrl

                }).ToList();
                orderListModel.Add(orderModel);
            }
            return View("Orders", orderListModel);
        }

        private void ClearCart(int BookingCartId)
        {
           _bookingCartService.BookingClearCart(BookingCartId);
        }

        private void SaveOrder(OrderModel model, Payment payment, string userId)
        {
            var order = new Order();
            order.OrderNumber = new Random().Next(111111,999999).ToString();
            order.OrderState = EnumOrderState.completed;
            order.PaymentType = EnumPaymentType.CreditCard;
            order.PaymentId = payment.PaymentId;
            order.ConversationId = payment.ConversationId;
            order.OrderDate = new DateTime();
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.UserId = userId;
            order.Address = model.Address;
            order.Phone = model.Phone;
            order.Email = model.Email;
            order.City = model.City;
            //order.Note = model.Note;
            order.OrderItems = new List<ENTITY.OrderItem>();
            foreach(var item in model.BookingCartModel.BookingCartItems)
            {
                var orderItem = new CinemaManagementSystem.ENTITY.OrderItem()
                {
                    Price = item.Price,
                    Quantity = item.Quantity,
                    MovieId = item.MovieId,
                };
                order.OrderItems.Add(orderItem);
            }
            _orderService.Create(order);//kayıt DB'ye aktarılıyor.
        }

        private Payment PaymentProcess(OrderModel model)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-r6DE0vCnXsHkjAdNSOisMtM6KASgDkU9";
            options.SecretKey = "sandbox-tZJbKNZPn0FiyjTe417BlJwuho4VSble";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Price = model.BookingCartModel.TotalPrice().ToString();
            request.PaidPrice = model.BookingCartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = "B67832";
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.CardName;
            paymentCard.CardNumber = model.CardNumber;
            paymentCard.ExpireMonth = model.ExpirationMonth;
            paymentCard.ExpireYear = model.ExpirationYear;
            paymentCard.Cvc = model.CVC;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            //  paymentCard.CardNumber = "5528790000000008";
            // paymentCard.ExpireMonth = "12";
            // paymentCard.ExpireYear = "2030";
            // paymentCard.Cvc = "123";

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = model.FirstName;
            buyer.Surname = model.LastName;
            buyer.GsmNumber = "+905350000000";
            buyer.Email = "email@email.com";
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in model.BookingCartModel.BookingCartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.MovieId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Dramatic Films";
                basketItem.Price = item.Price.ToString();
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItems.Add(basketItem);
            }
            request.BasketItems = basketItems;
            return Payment.Create(request, options);
        }
    }
}
