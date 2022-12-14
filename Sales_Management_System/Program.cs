using Core.Interfaces;
using Infrastructure;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Sales_Management_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add connection string
            builder.Services.AddDbContext<AppDbContext>(o =>
            o.UseSqlServer(builder.Configuration.GetConnectionString("default"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            // Add Unit of work service
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add Invoice Service
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}