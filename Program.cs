using Microsoft.EntityFrameworkCore;
using SUPIR_RAZOR.Data;
using SUPIR_RAZOR.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<MastersRepository>();
builder.Services.AddScoped<CustomersRepository>();
builder.Services.AddScoped<OrdersRepository>();
builder.Services.AddScoped<ProductsRepository>();




builder.Services.AddDbContext<SUPIR_RAZORDbContext>(
    options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
