using CinemaManagementSystem.BUSINESS.Abstract;
using CinemaManagementSystem.BUSINESS.Concrete;
using CinemaManagementSystem.DATA.Abstract;
using CinemaManagementSystem.DATA.Concrete.EfCore;
using CinemaManagementSystem.WEBUI.EmailServices;
using CinemaManagementSystem.WEBUI.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Server=DESKTOP-2PUAJM9;Database=CMS;User Id=selin;Password=selin12345;Encrypt=False"));
builder.Services.AddIdentity<User,IdentityRole>().AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    //password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    //Lockout
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Movie/List";
    options.LogoutPath = "/home/index";
    options.AccessDeniedPath = "/account/accessdenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true,
        Name = ".CinemaManagementSystem.Security.Cookie",
        SameSite = SameSiteMode.Strict
    };
});
builder.Services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
builder.Services.AddScoped<IMovieRepository, EfCoreMovieRepository>();
builder.Services.AddScoped<IBookingCartRepository, EfCoreBookingCartRepository>();
builder.Services.AddScoped<IOrderRepository, EfCoreOrderRepository>();

builder.Services.AddScoped<IMovieService, MovieManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<IBookingCartService, BookingCartManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();

 
//builder.Services.AddSingleton<IEmailSender, SmtpEmailSender>();

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
new SmtpEmailSender(configuration["EmailSender:Host"],
                    configuration.GetValue<int>("EmailSender:Port"),
                    configuration.GetValue<bool>("EmailSender:enableSSL"),
                    configuration["EmailSender:UserName"],
                    configuration["EmailSender:Password"]));

builder.Services.AddControllersWithViews();
builder.Services.AddRouting();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//uygulamayı yayınladıktan sonra çalışmasını istediğimiz IdentitySeed yapsını ekliyoruz.


app.UseHttpsRedirection();
app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    SeedDatabase.Seed();
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();

app.UseEndpoints(endpoints =>
  {
      //    app.MapControllerRoute(
      //    name: "orders",
      //    pattern: "orders",
      //    defaults: new { controller = "BookingCart", action = "GetOrders" }
      //);
      //    app.MapControllerRoute(
      //    name: "checkout",
      //    pattern: "checkout",
      //    defaults: new { controller = "BookingCart", action = "Checkout" }
      //);
      //    app.MapControllerRoute(
      //    name: "bookingcart",
      //    pattern: "bookingcart",
      //    defaults: new { controller = "BookingCart", action = "Index" }
      //);

      //    app.MapControllerRoute(
      //    name: "adminroles",
      //    pattern: "admin/role/list",
      //    defaults: new { controller = "Admin", action = "RoleList" }
      //);

      //    app.MapControllerRoute(
      //    name: "adminrolecreate",
      //    pattern: "admin/role/create",
      //    defaults: new { controller = "Admin", action = "RoleCreate" }
      //);

      //    app.MapControllerRoute(
      //    name: "adminrolecreate",
      //    pattern: "admin/role/create",
      //    defaults: new { controller = "Admin", action = "RoleCreate" }
      //);


      //    app.MapControllerRoute(
      //    name: "adminroleedit",
      //    pattern: "admin/role/{id?}",
      //    defaults: new { controller = "Admin", action = "RoleEdit" }
      //);
      //app.MapControllerRoute(
      //name: "adminmovielist",
      //pattern: "admin/movie",
      //defaults: new { controller = "Admin", action = "MovieList" }
      //);
      //app.MapControllerRoute(
      //name: "admincategories",
      //pattern: "admin/categories",
      //defaults: new { controller = "Admin", action = "CategoryList" }
      //);
      //app.MapControllerRoute(
      //name: "admincategorycreate",
      //pattern: "admin/categories/create",
      //defaults: new { controller = "Admin", action = "CategoryCreate" }
      //);
      //app.MapControllerRoute(
      //name: "admincategoryedit",
      //pattern: "admin/categories/{id?}",
      //defaults: new { controller = "Admin", action = "CategoryEdit" }
      //);
      //app.MapControllerRoute(
      //name: "adminmovies",
      //pattern: "admin/movies",
      //defaults: new { controller = "Admin", action = "MovieList" }
      //);

      //app.MapControllerRoute(
      //name: "adminmoviecreate",
      //pattern: "admin/movies/create",
      //defaults: new { controller = "Admin", action = "MovieCreate" }
      //);

      //app.MapControllerRoute(
      //name: "adminmovieedit",
      //pattern: "admin/movies/{id?}",
      //defaults: new { controller = "Admin", action = "MovieEdit" }
      //);
      app.MapControllerRoute(
    name: "adminmovielist",
    pattern: "admin/movies",
    defaults: new { controller = "Admin", action = "MovieList" }
    );
    app.MapControllerRoute(
    name: "search",
    pattern: "search",
    defaults: new { controller = "Movie", action = "Search" }
    );
    app.MapControllerRoute(
    name: "moviedetails",
    pattern: "{url}",
    defaults: new { controller = "Movie", action = "Details" }
    );
    app.MapControllerRoute(
    name: "movies",
    pattern: "movies/{category?}",
    defaults: new { controller = "Movie", action = "List" }
    );
    //app.MapControllerRoute(
    //    name: "Admin",
    //    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
    //    );
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run(async (context) =>
{
    var userManager = context.RequestServices.GetRequiredService<UserManager<User>>();
    var roleManager = context.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
    var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

   SeedIdentity.Seed(userManager, roleManager, configuration).Wait();
});

app.Run();

