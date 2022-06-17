using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bachelorprosjekt.Data;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BachelorprosjektContextConnection") ?? throw new InvalidOperationException("Connection string 'BachelorprosjektContextConnection' not found.");

builder.Services.AddDbContext<BachelorprosjektContext>(options =>
    options.UseSqlServer(connectionString)); ;


//Add Role services to Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BachelorprosjektContext>();

builder.Services.AddDbContext<BachelorprosjektContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BachelorprosjektContext") ?? throw new InvalidOperationException("Connection string 'BachelorprosjektContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();



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
app.UseAuthentication(); ;

app.UseAuthorization();

//TODO
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/
app.Run();
