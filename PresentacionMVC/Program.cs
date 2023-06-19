using PresentacionMVC.Controllers;


namespace PresentacionMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            builder.Services.AddHttpContextAccessor();

       

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",

            pattern: "{controller=Tipo}/{action=Index}/{id?}");
            // pattern: "{controller=Cabaña}/{action=CreateCabaña}/{id?}");
            //pattern: "{controller=Usuario}/{action=Login}");
            //pattern: "{controller=Cabaña}/{action=CreateCabaña}/{id?}");
            //pattern: "{controller=Tipo}/{action=Index}");
            //pattern: "{controller=Home}/{action=Index}/{id?}");// CON ESTE HABILITADO ENTREGAMOS 1 OBLIGATORIO
            //pattern: "{controller=Mantenimiento}/{action=CreateMantenimiento}/{8?}");

            app.Run();
        }
    }

  
}