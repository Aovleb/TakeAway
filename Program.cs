using TakeAway.DAL;
using TakeAway.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("local");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IClientDAL>(x => new ClientDAL(connectionString));
builder.Services.AddScoped<IDishDAL>(x => new DishDAL(connectionString));
builder.Services.AddScoped<IMealDAL>(x => new MealDAL(connectionString));
builder.Services.AddScoped<IMenuDAL>(x => new MenuDAL(connectionString));
builder.Services.AddScoped<IOrderDAL>(x => new OrderDAL(connectionString));
builder.Services.AddScoped<IUserDAL>(x => new UserDAL(connectionString));
builder.Services.AddScoped<IRestaurantDAL>(x => new RestaurantDAL(connectionString));
builder.Services.AddScoped<IServiceDAL>(x => new ServiceDAL(connectionString));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
